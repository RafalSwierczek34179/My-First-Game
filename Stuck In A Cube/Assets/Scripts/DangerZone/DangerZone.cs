using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{

    public GameObject _player;
    Vector3[] AI_pos = new Vector3[4];
    public Transform AI_0;
    public Transform AI_1;
    public Transform AI_2;
    public Transform AI_3;

    private void Update()
    {
        AI_pos[0] = AI_0.transform.position;
        AI_pos[1] = AI_1.transform.position;
        AI_pos[2] = AI_2.transform.position;
        AI_pos[3] = AI_3.transform.position;
        ZoneEffect();
    }

    public void Teleport()
    {
        transform.position = _player.transform.position - new Vector3(0f, 1.3f, 0f);
    }

    private void ZoneEffect()
    {
        if (Vector3.Distance(AI_pos[0], this.transform.position) <= 2f)
        {
            AI_Behaviour AI_access = AI_0.transform.GetComponent<AI_Behaviour>();
            AI_access.InDangerZone();
        }
        else
        {
            AI_Behaviour AI_access = AI_0.transform.GetComponent<AI_Behaviour>();
            AI_access.OutsideDangerZone();
        }
        if (Vector3.Distance(AI_pos[1], this.transform.position) <= 2f)
        {
            AI_Behaviour AI_access = AI_1.transform.GetComponent<AI_Behaviour>();
            AI_access.InDangerZone();
        }
        else
        {
            AI_Behaviour AI_access = AI_1.transform.GetComponent<AI_Behaviour>();
            AI_access.OutsideDangerZone();
        }
        if (Vector3.Distance(AI_pos[2], this.transform.position) <= 2f)
        {
            AI_Behaviour AI_access = AI_2.transform.GetComponent<AI_Behaviour>();
            AI_access.InDangerZone();
        }
        else
        {
            AI_Behaviour AI_access = AI_2.transform.GetComponent<AI_Behaviour>();
            AI_access.OutsideDangerZone();
        }
        if (Vector3.Distance(AI_pos[3], this.transform.position) <= 2f)
        {
            AI_Behaviour AI_access = AI_3.transform.GetComponent<AI_Behaviour>();
            AI_access.InDangerZone();
        }
        else
        {
            AI_Behaviour AI_access = AI_3.transform.GetComponent<AI_Behaviour>();
            AI_access.OutsideDangerZone();
        }
    }


}
