using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SA_Menu_Buttons : MonoBehaviour
{

    public string _abilitySelected = "Time Stop";


    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }

    public void TS_Button()
    {
        _abilitySelected = "Time Stop";
        SaveSystem_abilities.SaveAbility(this);
    }

    public void DZ_Button()
    {
        _abilitySelected = "Danger Zone";
        SaveSystem_abilities.SaveAbility(this);
    }

    public void Predator_Button()
    {
        _abilitySelected = "Predator";
        SaveSystem_abilities.SaveAbility(this);
    }

}
