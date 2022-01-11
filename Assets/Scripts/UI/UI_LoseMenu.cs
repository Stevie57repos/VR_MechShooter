using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.SceneReference;
public class UI_LoseMenu : MonoBehaviour
{
    [SerializeField]
    private SceneReference _restart;
    public void RestartLevel()
    {
        _restart.LoadScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
