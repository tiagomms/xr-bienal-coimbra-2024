using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupScene : MonoBehaviour
{
    [SerializeField] private GameAreaBoundaryProperties initialCageOrigin;
    
    private GameObject environmentParent;

    public static Action<GameAreaBoundaryProperties> OnSceneSetUp;

    private void OnEnable()
    {
        SetupCalibration.OnCalibrationCompleted += RotateEnvironmentIfNeeded;
        SceneManager.sceneLoaded += SceneSetupOnSceneLoaded;
    }

    private void OnDisable()
    {
        SetupCalibration.OnCalibrationCompleted -= RotateEnvironmentIfNeeded;
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
            DebugManager.Instance.Warning("PlayerBoundary não foi definido em SetUpScene. Pode levar a erros!");
        }
    }

    private void HideHelpfulObjects()
    {
        // (1) force hide of all game objects with tag 'HideOnBuild"
        if (GlobalManager.Instance.Settings.hideOnBuildHelpfulObjects)
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
