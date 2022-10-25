using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlManager : MonoBehaviour
{

    [SerializeField] Slider LvlBar;
    [SerializeField] Button TS_button;
    [SerializeField] Button DZ_button;
    [SerializeField] Button Predator_button;
    public int _currentLevel;
    public int _TotalXP;

    private void Start()
    {
        if (SaveSystem_Level.LoadLevel() == null)
        {
            TS_button.interactable = true;
            LvlBar.value = 1;
        }
        else
        {
            _currentLevel = SaveSystem_Level.LoadLevel().currentLevel;
            _TotalXP = SaveSystem_Level.LoadLevel().TotalXP;
            LvlBar.value = _currentLevel;
            TS_button.interactable = true;
            switch (_currentLevel)
            {
                case 2:
                    DZ_button.interactable = true;
                    break;
                case 3:
                    DZ_button.interactable = true;
                    Predator_button.interactable = true;
                    break;
            }
        }
    }

    public void UpdateXP_Level(int XP)
    {
        //add xp, calc new level, save

        _currentLevel = SaveSystem_Level.LoadLevel().currentLevel;
        _TotalXP = SaveSystem_Level.LoadLevel().TotalXP;
        _TotalXP += XP;
        if (_TotalXP <= 999)
        {
            _currentLevel = 1;
        }
        else if(_TotalXP <= 1999)
        {
            _currentLevel = 2;
        }
        else
        {
            _currentLevel = 3;
        }
        SaveSystem_Level.SaveLevel(this);
    }

    public void NewAccount(int XP)
    {
        _currentLevel = 1;
        _TotalXP = XP;
        SaveSystem_Level.SaveLevel(this);
    }

}
