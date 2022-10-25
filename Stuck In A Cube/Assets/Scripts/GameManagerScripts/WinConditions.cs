using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
//using UnityEditor.SceneManagement;

public class WinConditions : MonoBehaviour
{
    float _time = 120f;

    public Transform _door1;
    public Transform _door2;

    bool _happened = false;

    public Transform _enemy1;
    public Transform _enemy2;
    public Transform _enemy3;
    public Transform _enemy4;

    private Transform[] Enemies = new Transform[4];//Stores the transforms off all enemies/AI, it's filled in in the Start() procedure

    public bool _timeStop = false;

    [SerializeField] Text _clock;

    bool _gameOver = false;

    public int _conditionCheck = 0;

    int _stuck_AI;

    public GameObject GameOver_screen;

    float _statsTimer = 0f;

    [SerializeField] Transform _player;

    public int _totalBeansCaught = 0;

    public int _abilityCounter = 0;

    public int _orbcounter = 0;

    [SerializeField] LvlManager LevelManager;

    [SerializeField] Text V_D_text;
    [SerializeField] Text Bean_Counter;
    [SerializeField] Text TimeDisplay;
    [SerializeField] Text AbilityDisplay;
    [SerializeField] Text CatchCounter;
    [SerializeField] Text AbilityCounter;
    [SerializeField] Text OrbCounter;
    [SerializeField] Text CurrentLevelDisplay;
    [SerializeField] Text DeltaXPDisplay;
    [SerializeField] Text XP2LvlUpDisplay;
    [SerializeField] Text TotalXPDisplay;

    private void Start()
    {
        Enemies[0] = _enemy1;
        Enemies[1] = _enemy2;
        Enemies[2] = _enemy3;
        Enemies[3] = _enemy4;
    }

    void Update()
    {
        ClockDisplay();
        if (_timeStop == false)
        {
            Timer();
            if (_time <= 0f && _happened == false)//Checks if the timer is below 0 and if this has already been ran before since there's no need to turn off visuals multiple times etc
            {
                TimesUp();
                _happened = true;//Validation so that this is only ran once
            }
        }
        StatsTimer();
        CalcNumAI_Stuck();
        if (_gameOver == false)
        {
            ConditionCheck();
        }
    }

    void ConditionCheck()
    {

        if (_conditionCheck + _stuck_AI == 4)
        {
            Cursor.lockState = CursorLockMode.None;
            int XPGained = 0;
            _gameOver = true;
            GameOver_screen.SetActive(true);



            switch (_stuck_AI)
            {
                case 0:
                    V_D_text.text = "Total Defeat!";
                    Bean_Counter.text = "Beans Caught: 0";
                    break;
                case 1:
                    V_D_text.text = "Defeat!";
                    Bean_Counter.text = "Beans Caught: 1";
                    break;
                case 2:
                    V_D_text.text = "Victory!";
                    Bean_Counter.text = "Beans Caught: 2";
                    XPGained += 100;
                    break;
                case 3:
                    V_D_text.text = "Good Victory!";
                    Bean_Counter.text = "Beans Caught: 3";
                    XPGained += 100;
                    break;
                case 4:
                    V_D_text.text = "Flawless Victory!";
                    Bean_Counter.text = "Beans Caught: 4";
                    XPGained += 100;
                    break;
            }
            TimeDisplay.text = "Time: " + _statsTimer.ToString("000");
            if (_statsTimer <= 20f)
            {
                XPGained += 150;
            }
            else if (_statsTimer <= 30f)
            {
                XPGained += 100;
            }
            else if (_statsTimer <= 60f)
            {
                XPGained += 50;
            }
            AbilityDisplay.text = "Ability: " + _player.GetComponent<Abilities>()._playersAbility;
            CatchCounter.text = "Total Catches: " + _totalBeansCaught.ToString();
            XPGained += _totalBeansCaught * 5;
            AbilityCounter.text = "Ability Used: " + _abilityCounter.ToString();
            XPGained += _abilityCounter * 5;
            OrbCounter.text = "Point Orbs Picked Up: " + _orbcounter.ToString();
            XPGained += _orbcounter * 5;


            if (SaveSystem_Level.LoadLevel() == null)
            {
                LevelManager.NewAccount(XPGained);
            }
            else
            {
                LevelManager.UpdateXP_Level(XPGained);
            }



            DeltaXPDisplay.text = "XP Gained: " + XPGained.ToString();

            CurrentLevelDisplay.text = SaveSystem_Level.LoadLevel().currentLevel.ToString();

            switch (SaveSystem_Level.LoadLevel().currentLevel)
            {
                case 1:
                    XP2LvlUpDisplay.text = "XP Until Next Level: " + (999 - SaveSystem_Level.LoadLevel().TotalXP).ToString();
                    break;
                case 2:
                    XP2LvlUpDisplay.text = "XP Until Next Level: " + (1999 - SaveSystem_Level.LoadLevel().TotalXP).ToString();
                    break;
                case 3:
                    XP2LvlUpDisplay.text = "Already At Highest Level!";
                    break;
            }

            TotalXPDisplay.text = "Total XP: " + SaveSystem_Level.LoadLevel().TotalXP.ToString();


        }
    }

    void StatsTimer()
    {
        if (_gameOver == false)
        {
            _statsTimer += 1 * Time.deltaTime;
        }
    }

    void ClockDisplay()
    {
        if (_time >= 100f)
        {
            _clock.text = _time.ToString("000");
        }
        else if (_time >= 20f)
        {
            _clock.text = _time.ToString("00");
        }
        else if (_time >= 10f)
        {
            _clock.color = new Color(1f, 0f, 0f, 0.4f);
            _clock.text = _time.ToString("00");
        }
        else if (_time > 0f)
        {
            _clock.text = _time.ToString("0");
        }
        else
        {
            _clock.text = "Time's up!";
        }
    }

    void Timer()
    {
        if (_time > 0f)
        {
            _time -= 1 * Time.deltaTime;
        }
        else
        {
            _time = 0f;
        }
    }
    void DoorVisuals()//Disables the mesh renderer, so the player can't see the doors anymore, and turns off nav mesh obstacle so AI can walk through
    {
        MeshRenderer _visuals1 = _door1.transform.GetComponent<MeshRenderer>();
        MeshRenderer _visuals2 = _door2.transform.GetComponent<MeshRenderer>();
        NavMeshObstacle _obstacle1 = _door1.transform.GetComponent<NavMeshObstacle>();
        NavMeshObstacle _obstacle2 = _door2.transform.GetComponent<NavMeshObstacle>();
        _visuals1.enabled = false;
        _visuals2.enabled = false;
        _obstacle1.enabled = false;
        _obstacle2.enabled = false;
    }
    void TimesUp()
    {
        DoorVisuals();
        for (int i = 0; i < Enemies.Length; i++)//Notifies every AI that the doors are open
        {
            AI_Behaviour enemyUpdate = Enemies[i].transform.GetComponent<AI_Behaviour>();
            enemyUpdate._doorsOpen = true;
        }
    }

    void CalcNumAI_Stuck()
    {
        int tempCounter = 0;
        foreach (Transform enemy in Enemies)
        {
            bool stuck = enemy.GetComponent<AI_Behaviour>().Stuck;
            if (stuck == true)
            {
                tempCounter += 1;
            }
        }
        transform.GetComponent<GameStats>()._num_AI_stuck = tempCounter;
        _stuck_AI = tempCounter;
    }

}
