#if UNITY_EDITOR_OSX 
#define USE_SIMULATOR
#elif UNITY_ANDROID || UNITY_EDITOR_WIN
#define USE_QUEST
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupScene : MonoBehaviour
{
    [SerializeField]
    private bool _hideOnBuildHelpfulObjects = true;

    [SerializeField] private Transform initialCageOrigin;
    
    private GameObject environmentParent;

    public static Action<Transform> OnSceneSetUp;

    private void OnEnable()
    {
        GlobalManager.OnCalibrationCompleted += RotateEnvironmentIfNeeded;
        SceneManager.sceneLoaded += SceneSetupOnSceneLoaded;
    }

    private void OnDisable()
    {
        GlobalManager.OnCalibrationCompleted -= RotateEnvironmentIfNeeded;
        SceneManager.sceneLoaded -= SceneSetupOnSceneLoaded;

    }

    private void SceneSetupOnSceneLoaded(Scene scene1, LoadSceneMode mode)
    {
        HideHelpfulObjects();
        RotateEnvironmentIfNeeded();
        
        if (initialCageOrigin != null)
        {
            OnSceneSetUp?.Invoke(initialCageOrigin);
        }
        else
        {
            DebugManager.Instance.Warning("Jaula inicial não foi definida em SetUpScene. Pode levar a erros!");
        }
    }

    private void HideHelpfulObjects()
    {
        // (1) force hide of all game objects with tag 'HideOnBuild"
        if (_hideOnBuildHelpfulObjects)
        {
            GameObject[] hideObjs = GameObject.FindGameObjectsWithTag("HideOnBuild");
            foreach (GameObject item in hideObjs)
            {
                item.SetActive(false);
            }
        }
    }

    private void RotateEnvironmentIfNeeded()
    {
        if (!GlobalManager.Instance.IsCalibrated) return;

        environmentParent = GameObject.FindWithTag(TagManager.ENVIRONMENT_TAG);

        if (GlobalManager.Instance.IsBoundaryRotated)
        {
            environmentParent.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        }
    }
}
