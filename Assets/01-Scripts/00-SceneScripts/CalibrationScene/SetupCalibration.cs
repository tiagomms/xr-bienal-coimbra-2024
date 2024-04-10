#if UNITY_EDITOR_OSX 
#define USE_SIMULATOR
#elif UNITY_ANDROID || UNITY_EDITOR_WIN
#define USE_QUEST
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetupCalibration : MonoBehaviour
{
    [Tooltip("Global Settings for testing/playing game - Mandatory")] 
    [SerializeField]
    private GlobalSettings _globalSettings;

    [SerializeField]
    private Transform startLocation;

    [SerializeField] 
    private GameObject uiParentObject;
    
    [SerializeField] 
    private bool shouldUseDebugUiText = false;
    
    [SerializeField] 
    private StringGameEvent writeToDebugUi;

    [SerializeField] 
    private int maxCalibrationAttempts = 5;
    
    [SerializeField] 
    private float secondsBetweenCalibrations = 2f;

    [SerializeField] private DefaultFadeSceneSetup defaultFadeSceneSetup;
    
    private int _calibrationCounter;

    public static Action<GlobalSettings> OnCalibrationStart;
    // Start is called before the first frame update
    void Start()
    {
        uiParentObject.SetActive(shouldUseDebugUiText);
        #if USE_SIMULATOR
            GlobalManager.Instance.isCalibrated = true;
            startLocation.position = new Vector3();
            uiParentObject.SetActive(false);
        #endif

        /*
        if (!GlobalManager.Instance.IsCalibrated || GlobalManager.Instance.LastSceneCageOrigin == null)
        {
            startLocation.position = new Vector3();
        }
        else
        {
            startLocation.position = GlobalManager.Instance.LastSceneCageOrigin.position;
            startLocation.rotation = GlobalManager.Instance.LastSceneCageOrigin.rotation;
        }
        */

        if (!GlobalManager.Instance.IsCalibrated)
        {
            startLocation.position = new Vector3();
        }

        #if USE_QUEST
        if (!GlobalManager.Instance.IsCalibrated)
        {
            startLocation.position = new Vector3();
            StartCoroutine(PerformCalibrationSetup());
        }
        OnCalibrationStart.Invoke(_globalSettings);
        #endif
        
    }
    
    private IEnumerator PerformCalibrationSetup()
    {
        startLocation.gameObject.SetActive(false);
        
        while (_calibrationCounter < maxCalibrationAttempts && !GlobalManager.Instance.IsCalibrated)
        {
            yield return new WaitForSeconds(secondsBetweenCalibrations);
            CalculateBoundary();
            _calibrationCounter++;
        }

        if (_calibrationCounter != maxCalibrationAttempts)
        {
            GlobalManager.OnCalibrationCompleted.Invoke();
            startLocation.gameObject.SetActive(true);
        }
        else
        {
            uiParentObject.SetActive(true);
            WriteToDebug("Calibration error occured\nPlease take headset off and put it on again\nIf that doesn't work press Meta's on/off button.");
        }
    }

    private void CalculateBoundary()
    {
        if (OVRManager.boundary.GetConfigured())
        {
            GlobalManager.Instance.BoundarySize = OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.PlayArea);
            GlobalManager.Instance.BoundaryPoints = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

            if (GlobalManager.Instance.BoundaryPoints != null)
            {
                GlobalManager.Instance.IsBoundaryRotated = GlobalManager.Instance.BoundarySize.x > GlobalManager.Instance.BoundarySize.z;
                GlobalManager.Instance.IsCalibrated = true;

                if (shouldUseDebugUiText)
                {
                    uiParentObject.SetActive(true);
                    string pointsStr = "PlayArea size:\n" + GlobalManager.Instance.BoundarySize; 
                    //string pointsStr = "(" + _calibrationCounter + ") PlayArea size:\n" + GlobalManager.Instance.BoundarySize + "\n\n" 
                      //                 + string.Join("\n", GlobalManager.Instance.BoundaryPoints.Select(p => p.ToString()));
                    WriteToDebug(pointsStr);    
                }
            }
        }
    }

    private void WriteToDebug(string text)
    {
        if (writeToDebugUi != null)
        {
            writeToDebugUi.Raise(text);
        }

        DebugManager.Instance.Log(text);
    }
    
    public void GoToStartScene()
    {
        DefaultFadeSceneSetup.TriggerFadeOut.Invoke(defaultFadeSceneSetup.DefaultFadeOutDuration, defaultFadeSceneSetup.DefaultFadeOutColor);
        SceneTransitionManager.Instance.GoToSceneAsyncByIndexInXSeconds(1, defaultFadeSceneSetup.DefaultFadeOutDuration);
    }
}
