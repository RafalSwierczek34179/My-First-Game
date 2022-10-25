using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOrb : MonoBehaviour
{

    public Transform _player;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Abilities _abilities = collision.transform.GetComponent<Abilities>();
            if (_abilities._abilityActive == false)
            {
                _abilities._abilityPoints += 1;
            }
            _abilities._orbExists = false;
            _abilities._gameManager.GetComponent<WinConditions>()._orbcounter += 1;
            Destroy(this.gameObject);
        }
    }

}
