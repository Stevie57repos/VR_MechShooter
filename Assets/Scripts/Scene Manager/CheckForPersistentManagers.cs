using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RoboRyanTron.SceneReference;

public class CheckForPersistentManagers : MonoBehaviour
{
    public SceneReference PersistentScene;

    private void Awake()
    {
        LoadPersistentScene();
    }
    private void LoadPersistentScene()
    {
        if (!CheckScenes())
        {
            PersistentScene.LoadSceneAsync();
        }
    }
    private bool CheckScenes()
    {
        bool isPersistentSceneLoaded = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            string sceneName = scene.name;
            if (sceneName == PersistentScene.SceneName)
                isPersistentSceneLoaded = true;
        }
        return isPersistentSceneLoaded;
    }
}
