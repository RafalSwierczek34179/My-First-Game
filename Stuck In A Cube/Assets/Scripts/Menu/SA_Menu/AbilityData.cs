using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityData
{
    public string _abilitySelection;

    public AbilityData (SA_Menu_Buttons _ability)
    {
        _abilitySelection = _ability._abilitySelected;
    }
}
