#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
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
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using UnityEngine.XR.Management;

public class SetupPlayer : MonoBehaviour
{

    [SerializeField]
    private GameObject _xrPlayer;

    [SerializeField]
    private GameObject _xrSimulator;
    
    [SerializeField]
    private OVRManager _ovrManager;    
    
    [SerializeField]
    private XROrigin _xrOrigin;
    
    [SerializeField]
    private CharacterControllerDriver _characterControllerDriver;

    [Space(height: 20)] 
    [SerializeField] private GameObject _leftHand;
    [SerializeField] private GameObject _rightHand;
    [SerializeField] private GameObject _handVisualizer;
    [SerializeField] private GameObject _handsSmoothingPostProcessor;
    
    [Space(height: 10)]
    [SerializeField]
    private Transform _xrControllerLeft;    
    [SerializeField]
    private Transform _xrControllerLeftStabilizer;
    [SerializeField]
    private Transform _xrControllerRight; 
    [SerializeField]
    private Transform _xrControllerRightStabilizer;

    public XROrigin XrOrigin
    {
        get => _xrOrigin;
        set => _xrOrigin = value;
    }

    /// <summary>
    /// VERY IMPORTANT
    /// (1) if xrSimulator is active even before awake - it is always triggered - so start it inactive
    /// (2) OVRManager interferes with XROrigin tracking origin level.
    ///  - So for the simulator, OVRManager must be set to eyelevel
    ///  - For non-simulator (Quest Link and Quest), FloorLevel to work properly
    /// March 2024: Recenter does not work properly on Quest Link - that is a reported bug in some channels
    /// That being said, the build works fine with a guardian.
    /// </summary>
    private void Awake()
    {
        _xrPlayer.SetActive(true);

        if (GlobalManager.Instance.CurrentPlatform == PlatformUsed.Simulator)
        {
            // (1) make sure xrSimulator is on
            _xrSimulator.SetActive(true);

            if (_ovrManager != null)
            {
                _ovrManager.trackingOriginType = OVRManager.TrackingOrigin.EyeLevel;
            }
            
                    
            // (2) ever since not using the xr controller prefab, the controllers do not appear on the simulator
            //this part is just to push them ahead. It is not perfect, but at least controllers are seen again
            // SOLVED: increased by accident the camera minimum clipping pane. reverted back and all is good
        }
        else
        {
            // (1) make sure xrSimulator is off 
            //(for some reason this does not work - had to put xrSimulator disabled manually)
            _xrSimulator.SetActive(false);
            _ovrManager.trackingOriginType = OVRManager.TrackingOrigin.FloorLevel;
        }
    }

    private void Start()
    {
        _characterControllerDriver.enabled = GlobalManager.Instance.CurrentPlatform == PlatformUsed.Simulator;
    }

    private void OnEnable()
    {
        GlobalManager.OnCalibrationLost += ResetXrOrigin;
    }

    private void OnDisable()
    {
        GlobalManager.OnCalibrationLost -= ResetXrOrigin;
    }

    private void ResetXrOrigin()
    {
        _xrOrigin.transform.position = new Vector3();
        _xrOrigin.transform.rotation = Quaternion.identity;
    }
}

