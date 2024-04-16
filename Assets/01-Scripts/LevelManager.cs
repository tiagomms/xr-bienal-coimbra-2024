using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Tooltip("Should be set to true except if calibration and start menu scenes, or some other hidden scene")]
    [SerializeField] private bool _sendLevelInfoToGlobalManager = true;
    private void OnEnable()
    {
        if (_sendLevelInfoToGlobalManager)
        {
            SceneManager.sceneLoaded += SetSceneInGlobalManager;
            SetupScene.OnSceneSetUp += ChangeCageOriginInGlobalManager;
        }

    }
    private void OnDisable()
    {
        if (_sendLevelInfoToGlobalManager)
        {
            SceneManager.sceneLoaded -= SetSceneInGlobalManager;
            SetupScene.OnSceneSetUp -= ChangeCageOriginInGlobalManager;
        }
    }

    private void ChangeCageOriginInGlobalManager(GameAreaBoundaryProperties thisSceneCageOrigin)
    {
        GlobalManager.Instance.LastSceneCageOrigin = thisSceneCageOrigin;
    }

    private void SetSceneInGlobalManager(Scene arg0, LoadSceneMode arg1)
    {
        GlobalManager.Instance.LastSceneName = arg0.name;
        GlobalManager.Instance.AddSceneVisited(arg0.name);
    }

}
