using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{

    public static MainMenuController instance;

    public void PlayGame()
    {
        Time.timeScale = 1f;
        //playerModel[GameController.instance.GetSelectedModel()].SetActive(true);
        SceneFader.instance.LoadScene("Gameplay");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
