using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Help_Menu_buttons : MonoBehaviour
{

    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }

    public void AbilitiesButton()
    {
        SceneManager.LoadScene(3);
    }

}
