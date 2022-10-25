using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    //-----------------------------------------------------------------
    //---------------------------HUD-----------------------------------

    public Text AI_Counter;

    void AI_Gauge_Update()
    {
        switch (_num_AI_stuck)
        {
            case 4:
                AI_Counter.text = "Beans left: 0";
                break;
            case 3:
                AI_Counter.text = "Beans left: 1";
                break;
            case 2:
                AI_Counter.text = "Beans left: 2";
                break;
            case 1:
                AI_Counter.text = "Beans left: 3";
                break;
            case 0:
                AI_Counter.text = "Beans left: 4";
                break;
        }
    }

    //-----------------------------------------------------------------

    public int _num_AI_stuck;










    private void Update()
    {
        AI_Gauge_Update();
    }

}
