using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.SceneReference;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField]
    private SceneReference _startLevelScene;

    public void StartLevel()
    {
        _startLevelScene.LoadSceneAsync();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
