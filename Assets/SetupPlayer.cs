/*
 * So if build is made for UNITY_ANDROID it is true for both editors. In order to not change things over laziness,
 * I use #if and #elif
 */
#if UNITY_EDITOR_OSX
#define USE_SIMULATOR
#elif UNITY_ANDROID || UNITY_EDITOR_WIN
#define USE_QUEST
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
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
    private Transform _environmentParent;

    [SerializeField] 
    private bool ifIntroScene = false;

    [Space(height: 10)]
    [SerializeField]
    private bool _hideOnBuildHelpfulObjects = true;

    [SerializeField] 
    private bool shouldUseDebugUiText = false;
    
    [SerializeField] 
    private StringGameEvent writeToDebugUi;
    
    [Space(height: 10)]
    [SerializeField]
    private OVRManager _ovrManager;
    
    [Space(height: 10)]
    [SerializeField]
    private Transform _xrControllerLeft;    
    [SerializeField]
    private Transform _xrControllerLeftStabilizer;
    [SerializeField]
    private Transform _xrControllerRight; 
    [SerializeField]
    private Transform _xrControllerRightStabilizer;
    [SerializeField]
    private Vector3 _xrSimulatorMoveControllersLocalPosition = new Vector3(0, 0, 0.1f);
    
    
    private Vector3 _boundarySize;
    private Vector3[] _points;

    private int _frameCounter = 0;
    
    /// <summary>
    /// VERY IMPORTANT
    /// TODO: when switching to OSX and Windows, before pushing always set manually:
    ///     Project Settings > XR interaction toolkit > use xr device simulator
    /// OVRManager interferes with XROrigin tracking origin level.
    ///  - So for the simulator, OVRManager must be set to eyelevel
    ///  - For non-simulator (Quest Link and Quest), FloorLevel to work properly
    /// March 2024: Recenter does not work properly on Quest Link - that is a reported bug in some channels
    /// That being said, the build works fine with a guardian. 
    /// </summary>
    private void Awake()
    {
        _xrPlayer.SetActive(true);


        #if USE_SIMULATOR
        // (1) make sure xrSimulator is on
        _xrSimulator.SetActive(true);
        _ovrManager.trackingOriginType = OVRManager.TrackingOrigin.EyeLevel;
        
        if (_xrControllerLeft != null && _xrControllerLeftStabilizer != null)
        {
            
            for (int i = 0; i < _xrControllerLeft.childCount; i++)
            {
                _xrControllerLeft.GetChild(i).position += _xrSimulatorMoveControllersLocalPosition;
            }
            
            _xrControllerLeftStabilizer.localPosition += _xrSimulatorMoveControllersLocalPosition * 2;
        }       
        if (_xrControllerRight != null && _xrControllerRightStabilizer != null)
        {
            
            for (int i = 0; i < _xrControllerRight.childCount; i++)
            {
                _xrControllerRight.GetChild(i).position += _xrSimulatorMoveControllersLocalPosition;
            }
            
            _xrControllerRightStabilizer.localPosition += _xrSimulatorMoveControllersLocalPosition * 2;
        }
        
        #elif USE_QUEST 


        // (1) make sure xrSimulator is off 
        //(for some reason this does not work - had to put xrSimulator disabled manually)
        _xrSimulator.SetActive(false);
        _ovrManager.trackingOriginType = OVRManager.TrackingOrigin.FloorLevel;

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

    private void Start()
    {
        #if USE_QUEST 
        // set environment false if intro scene
        _environmentParent.gameObject.SetActive(!ifIntroScene);
        #endif
    }

#if USE_QUEST 
    private void OnEnable()
    {
        OVRManager.HMDMounted += OnHeadsetOn;
        OVRManager.HMDUnmounted += OnHeadsetOff;
    }

    private void OnDisable()
    {
        OVRManager.HMDMounted -= OnHeadsetOn;
        OVRManager.HMDUnmounted -= OnHeadsetOff;
    }
    #endif

    private IEnumerator CheckIfBoundaryChanged()
    {
        yield return new WaitForSeconds(3f);
        RecalculateBoundary(_frameCounter);
        
        // set environment active so you can see
        _environmentParent.gameObject.SetActive(true);
        
        // We setup the cage as the biggest size the forward, and not the right
        // if boundary size width is bigger than length, we should rotate the environment 90 degrees over the Y axis
        if (_environmentParent != null && _boundarySize.x > _boundarySize.z)
        {
            _environmentParent.eulerAngles = new Vector3(0f, 90f, 0f);
        }

        if (shouldUseDebugUiText)
        {
            string pointsStr = "(" + _frameCounter + ") PlayArea size:\n" + _boundarySize + "\n\n" 
                               + string.Join("\n", _points.Select(p => p.ToString()));
            writeToDebugUi.Raise(pointsStr);
            DebugManager.Instance.Log(pointsStr);
        }
        
        _frameCounter++;

    }

    private void OnHeadsetOff()
    {
        #if USE_SIMULATOR
        
        #elif USE_QUEST
        DebugManager.Instance.Log("----------------------------");
        DebugManager.Instance.Log("Headset Off" + _boundarySize);
        
        #endif
    }

    private void OnHeadsetOn()
    {
        #if USE_SIMULATOR
        
        #elif USE_QUEST
        DebugManager.Instance.Log("++++++++++++++++++++++++++++");
        
        StartCoroutine(CheckIfBoundaryChanged());

        #endif
    }

    private void RecalculateBoundary(int frameCounter)
    {
        if (OVRManager.boundary.GetConfigured())
        {
            _boundarySize = OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.PlayArea);
            _points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
        }
    }
}

