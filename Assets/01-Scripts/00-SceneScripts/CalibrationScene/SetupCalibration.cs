using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetupCalibration : MonoBehaviour
{
    [SerializeField]
    private Transform startLocation;

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

    public static Action OnCalibrationStart;
    public static Action OnCalibrationCompleted;
    // Start is called before the first frame update
    void Start()
    {
        if (GlobalManager.Instance.CurrentPlatform == PlatformUsed.Quest)
        {
            if (!GlobalManager.Instance.IsCalibrated)
            {
                StartCoroutine(PerformCalibrationSetup());
            }
        }
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
            OnCalibrationCompleted.Invoke();
            startLocation.gameObject.SetActive(true);
        }
        else
        {
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
