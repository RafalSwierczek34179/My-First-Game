using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int currentLevel;
    public int TotalXP;

    public LevelData(LvlManager LevelManager)
    {
        currentLevel = LevelManager._currentLevel;
        TotalXP = LevelManager._TotalXP;
    }


}
