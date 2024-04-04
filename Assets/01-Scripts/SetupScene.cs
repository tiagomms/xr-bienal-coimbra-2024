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
    
    private GameObject environmentParent;

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
        if (!GlobalManager.Instance.isCalibrated) return;

        environmentParent = GameObject.FindWithTag(TagManager.ENVIRONMENT_TAG);

        if (GlobalManager.Instance.isBoundaryRotated)
        {
            environmentParent.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        }
    }
}
