using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.SceneReference;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField]
    private SceneReference _startLevelScene;

    public void StartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        _startLevelScene.LoadScene();
    }

    // TODO : Async scene loading
    private IEnumerator LoadLevel()
    {
        yield return _startLevelScene.LoadSceneAsync();
        yield return SceneManager.SetActiveScene(SceneManager.GetSceneByName(_startLevelScene.SceneName));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
