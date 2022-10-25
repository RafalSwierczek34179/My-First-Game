using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{

    public string _playersAbility;
    public int _abilityPoints = 0;
    float _orbTimer = 20f;
    public bool _orbExists = false;
    public GameObject _pointorb;
    private Vector3[] OrbPositions = { new Vector3(-7.5f, 2f, 6.5f), 
        new Vector3(4f, 2f, 5f), new Vector3(-0.5f, 2f, -1f), 
        new Vector3(-19f, 2f, 3.5f), new Vector3(-2f, 2f, 9f), 
        new Vector3(12.5f, 2f, -6.5f), new Vector3(4.5f, 2f, -6.5f), 
        new Vector3(-3f, 2f, -7f), new Vector3(-7.5f, 2f, -13.5f), 
        new Vector3(12f, 2f, -13.5f), new Vector3(14.5f, 2f, -2f)};
    System.Random _rand = new System.Random();
    public GameObject _gameManager;
    public bool _abilityActive = false;
    public Transform _dangerZone;
    bool _predatorActive = false;
    float _predatorTimer = 0f;
    public Transform _cam;
    public Transform _enemiesFolder;
    public Slider _abilityGauge;
    Text _abilityText;


    private void Start()
    {

        if (SaveSystem_abilities.LoadAbility() == null)
        {
            _playersAbility = "Time Stop";
        }
        else
        {
            _playersAbility = SaveSystem_abilities.LoadAbility()._abilitySelection;
        }

        _abilityText = _abilityGauge.GetComponentInChildren<Text>();
        if (_playersAbility == "Time Stop")
        {
            _abilityText.text = "Time Stop";
        }
        else if (_playersAbility == "Danger Zone")
        {
            _abilityText.text = "Danger Zone";
        }
        else
        {
            _abilityText.text = "Predator";
        }
    }

    private void Update()
    {

        PointCheck();
        OrbSpawn();
        if (_predatorActive == true)
        {
            Predator();
        }
        AbilityGauge();
    }


    void AbilityGauge()//Dictates the size and colour of the ability gauge on bottom of players screen
    {
        Player playerAbility = transform.GetComponent<Player>();
        if (playerAbility._abilityReady == true)
        {
            _abilityGauge.value = 3;
            _abilityGauge.GetComponentInChildren<Image>().color = new Color(0f, 1f, 0f, 0.2f);

        }
        else
        {
            _abilityGauge.value = _abilityPoints;
            if (_abilityGauge.value == 1)
            {
                _abilityGauge.GetComponentInChildren<Image>().color = new Color(1f, 0f, 0f, 0.2f);
            }
            else
            {
                _abilityGauge.GetComponentInChildren<Image>().color = new Color(1f, 0.71f, 0f, 0.2f);
            }
        }
    }

    void PointCheck()
    {
        Player playerAbility = transform.GetComponent<Player>();
        if (_abilityPoints >= 3)
        {
            playerAbility._abilityReady = true;
            _abilityPoints = 0;
        }
        if (playerAbility._abilityReady == true)
        {
            _abilityPoints = 0;
        }
    }

    void OrbSpawn()
    {
        if (_orbExists == false)
        {
            _orbTimer += 1 * Time.deltaTime;
            if (_orbTimer >= 3f)
            {

                Instantiate(_pointorb, OrbPositions[_rand.Next(12)], Quaternion.identity);
                _orbExists = true;
                _orbTimer = 0f;
            }
        }
    }

    public void AbilityActivation()
    {
        _gameManager.GetComponent<WinConditions>()._abilityCounter += 1;
        if (_playersAbility == "Time Stop")
        {
            TimeStop();
        }
        else if (_playersAbility == "Danger Zone")
        {
            DangerZone();
        }
        else
        {
            _predatorActive = true;
        }
    }


    public void TimeStop()
    {
        WinConditions TimeStop = _gameManager.GetComponent<WinConditions>();
        TimeStop._timeStop = true;
    }
    public void DangerZone()
    {
        DangerZone _dzAccess = _dangerZone.GetComponent<DangerZone>();
        _dzAccess.Teleport();
    }

    private void Predator()
    {
        PlayerObstacleRadius _invisibility = _enemiesFolder.transform.GetComponent<PlayerObstacleRadius>();
        _invisibility._predator_active = true;
        _predatorTimer += 1 * Time.deltaTime;
        Player _player_access = transform.GetComponent<Player>();
        float _tempBoost = _player_access._originalSprintSpeed * 1.5f;
        _player_access._sprintspeed = _tempBoost;
        _player_access._stamina = 2f;
        if (Input.GetKey(KeyCode.LeftShift) == false)
        {
            MainCamera _cam_access = _cam.transform.GetComponent<MainCamera>();//Resets camera zoom effect if player stops sprinting in predator mode
            _cam_access.NormalZoom();
        }
        if (_predatorTimer >= 7.5f)//disables predator mode when its time limit is reached
        {
            _invisibility._predator_active = false;
            _player_access._sprintspeed = _player_access._originalSprintSpeed;
            _predatorActive = false;
            _abilityActive = false;
            _predatorTimer = 0f;
        }
    }

}
