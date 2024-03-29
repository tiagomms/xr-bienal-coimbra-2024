using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using UnityEngine.XR.Management;

public class SetupPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _xrPlayer;

    [SerializeField]
    private GameObject _xrSimulator;

    [SerializeField]
    private bool _hideOnBuildHelpfulObjects = true;
    
    // TODO: implement a general approach later if required
    [SerializeField] 
    private bool shouldUseDebugUiText = false;

    [SerializeField] private List<ChangeUiTextViaScript> debugUiTexts; 
    
    private XRInputSubsystem _inputSubSystem;
    private List<Vector3> _boundaries;

    /// <summary>
    /// VERY IMPORTANT
    /// TODO: when switching to OSX and Windows, before pushing always set manually Project Settings > XR interaction toolkit > use xr device simulator
    /// 
    /// </summary>
    private void Awake() {
        #if UNITY_EDITOR_OSX
        
        // (1) make sure xrSimulator is on
        _xrPlayer.SetActive(true);
        _xrSimulator.SetActive(true);

        #elif UNITY_ANDROID || UNITY_EDITOR_WIN
        

        // (1) make sure xrSimulator is off 
        //(for some reason this does not work - had to put xrSimulator disabled manually)
        _xrPlayer.SetActive(true);
        _xrSimulator.SetActive(false);

        // (2) force hide of all game objects with tag 'HideOnBuild"
        if (_hideOnBuildHelpfulObjects)
        {
            GameObject[] hideObjs = GameObject.FindGameObjectsWithTag("HideOnBuild");
            foreach (GameObject item in hideObjs)
            {
                item.SetActive(false);
            }
        }
        #endif    
    }

    private void OnEnable()
    {
        XRDevice.OnDeviceStarted += SetUpBoundaries;
        XRDevice.OnDeviceCanceled += CleanUpBoundaries;
    }

    private void OnDisable()
    {
        CleanUpBoundaries();
        XRDevice.OnDeviceStarted -= SetUpBoundaries;
        XRDevice.OnDeviceCanceled -= CleanUpBoundaries;
    }

    private void Update()
    {
        XRDevice.IsHmdMounted();
    }
    
    private void SetUpBoundaries()
    {
        if (_inputSubSystem == null)
        {
            var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
            if (loader == null)
            {
                Debug.LogWarning("Could not get active Loader.");
                return;
            }
            _inputSubSystem = loader.GetLoadedSubsystem<XRInputSubsystem>();
            DebugManager.Instance.Log("Found inputSubSystem: " + _inputSubSystem.ToString());
        }

        if (_inputSubSystem != null)
        {
            //_inputSubSystem.boundaryChanged += RefreshBoundaries;
            RefreshBoundaries(_inputSubSystem);
        }

        
    }
 
    private void RefreshBoundaries(XRInputSubsystem inputSubsystem)
    {
        DebugManager.Instance.Log("Refresh boundaries called");

        List<Vector3> currentBoundaries = new List<Vector3>();
        //if (UnityEngine.Experimental.XR.Boundary.TryGetGeometry(currentBoundaries))
        if (inputSubsystem.TryGetBoundaryPoints(currentBoundaries))
        {
            DebugManager.Instance.Log("Tried to get boundaries");
            //got boundaries, keep only those which didn't change.
            
            if ((_boundaries != currentBoundaries || _boundaries.Count != currentBoundaries.Count))
            {
                _boundaries = currentBoundaries;
                DebugManager.Instance.Log("boundaries set up. Positions: ");

                if (shouldUseDebugUiText)
                {
                    string newText = "Boundary Positions:\n";
                    for (int i = 0; i < _boundaries.Count; i++)
                    {
                        string line = "(" + i + ") " + _boundaries[i].ToString();
                        newText += line;
                        DebugManager.Instance.Log(line);
                    }
                    
                    
                }
                
                
                
            }
        }
    }
    
    
    private void CleanUpBoundaries()
    {
        if (_boundaries != null)
        {
            //_inputSubSystem.boundaryChanged -= RefreshBoundaries;
            //_inputSubSystem = null;
            _boundaries = null;
            DebugManager.Instance.Log("boundaries cleaned up!");
        }
    }
}

/*
public class Scenestart : MonoBehaviour
{
    private XRInputSubsystem _inputSubSystem;
    private List<Vector3> _boundaries;
    // Start is called before the first frame update
    void Start()
    {
    }
 
    // Update is called once per frame
    void Update()
    {
        if (_inputSubSystem == null)
        {
            VRAwake();
        }
    }
 
    void VRAwake()
    {
        List<XRInputSubsystem> list = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances<XRInputSubsystem>(list);
        foreach (XRInputSubsystem sSystem in list)
        {
            if (sSystem.running)
            {
                _inputSubSystem = sSystem;
                break;
            }
        }
        if (_inputSubSystem != null)
        {
            _inputSubSystem.boundaryChanged += RefreshBoundaries;
        }
    }
 
    private void RefreshBoundaries(XRInputSubsystem inputSubsystem)
    {
        List<Vector3> currentBoundaries = new List<Vector3>();
        //if (UnityEngine.Experimental.XR.Boundary.TryGetGeometry(currentBoundaries))
        if (inputSubsystem.TryGetBoundaryPoints(currentBoundaries))
        {
            //got boundaries, keep only those which didn't change.
            if (currentBoundaries != null && (_boundaries != currentBoundaries || _boundaries.Count != currentBoundaries.Count))
            {
                _boundaries = currentBoundaries;
            }
        }
    }
 
}
*/