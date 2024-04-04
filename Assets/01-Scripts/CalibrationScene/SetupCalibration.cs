#if UNITY_EDITOR_OSX 
#define USE_SIMULATOR
#elif UNITY_ANDROID || UNITY_EDITOR_WIN
#define USE_QUEST
#endif

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetupCalibration : MonoBehaviour
{
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
    
    private int _calibrationCounter;
    // Start is called before the first frame update
    void Start()
    {
        uiParentObject.SetActive(shouldUseDebugUiText);
        #if USE_SIMULATOR
            GlobalManager.Instance.isCalibrated = true;
            startLocation.position = new Vector3();
            uiParentObject.SetActive(false);
        #endif

        if (!GlobalManager.Instance.isCalibrated || GlobalManager.Instance.lastScenePortalEntry == null)
        {
            startLocation.position = new Vector3();
        }
        else
        {
            startLocation.position = GlobalManager.Instance.lastScenePortalEntry.position;
            startLocation.rotation = GlobalManager.Instance.lastScenePortalEntry.rotation;
        }

        #if USE_QUEST
        if (!GlobalManager.Instance.isCalibrated)
        {
            StartCoroutine(PerformCalibrationSetup());
        }
        #endif
    }
    
    private IEnumerator PerformCalibrationSetup()
    {
        startLocation.gameObject.SetActive(false);
        
        while (_calibrationCounter < maxCalibrationAttempts && !GlobalManager.Instance.isCalibrated)
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
            WriteToDebug("Calibration error occured\nPlease take headset off and put it on again");
        }
    }

    private void CalculateBoundary()
    {
        if (OVRManager.boundary.GetConfigured())
        {
            GlobalManager.Instance.boundarySize = OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.PlayArea);
            GlobalManager.Instance.boundaryPoints = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

            if (GlobalManager.Instance.boundaryPoints != null)
            {
                GlobalManager.Instance.isBoundaryRotated = GlobalManager.Instance.boundarySize.x > GlobalManager.Instance.boundarySize.z;
                GlobalManager.Instance.isCalibrated = true;

                if (shouldUseDebugUiText)
                {
                    uiParentObject.SetActive(true);
                    string pointsStr = "(" + _calibrationCounter + ") PlayArea size:\n" + GlobalManager.Instance.boundarySize + "\n\n" 
                                       + string.Join("\n", GlobalManager.Instance.boundaryPoints.Select(p => p.ToString()));
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
}
