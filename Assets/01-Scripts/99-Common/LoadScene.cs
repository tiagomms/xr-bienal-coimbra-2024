using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load Scene class that essentially can load a new scene or reload
/// </summary>
public static class LoadScene
{
    public static void LoadSceneUsingName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
