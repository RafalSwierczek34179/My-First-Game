using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver_buttons : MonoBehaviour
{

    public void RetryButton()
    {
        SceneManager.LoadScene(4);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

}
