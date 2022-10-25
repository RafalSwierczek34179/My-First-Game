using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Help_abilities_menu_buttons : MonoBehaviour
{

    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }

    public void HowButton()
    {
        SceneManager.LoadScene(2);
    }

}
