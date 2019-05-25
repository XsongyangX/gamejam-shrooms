using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public string playScene;
    public string creditsScene;

    public void LoadPlayScene()
    {
        SceneManager.LoadScene(playScene, LoadSceneMode.Single);
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(creditsScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
