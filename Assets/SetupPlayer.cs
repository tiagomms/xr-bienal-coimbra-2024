

#if UNITY_EDITOR_OSX
#define USE_SIMULATOR
#endif

#if UNITY_ANDROID || UNITY_EDITOR_WIN
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
    private bool _hideOnBuildHelpfulObjects = true;

    [Space]
    [SerializeField] 
    private bool shouldUseDebugUiText = false;
    
    [SerializeField] 
    private StringGameEvent writeToDebugUi;
    
    [Space]
    [SerializeField]
    private OVRManager _ovrManager;
    
    private XRInputSubsystem _inputSubSystem;
    private Vector3 _boundarySize;
    private List<Vector3> _boundaries;
    private Vector3[] _points;

    private Vector3 _previousClosestPoint;
    private bool _forceBoundaryRecalculation = false;
    private int _frameCounter = 0;
    
    
    private List<Vector3> _boundariesXrToolkit;
    
    [SerializeField]
    private Transform _mainCamera;
    [SerializeField]
    private Transform _xrOrigin;
    [SerializeField]
    private Transform _desiredCenterPosition;

    
    private Vector3 _offset;
    private Vector3 _targetForward;
    private Vector3 _cameraForward;
    private float _angle;

    /// <summary>
    /// VERY IMPORTANT
    /// TODO: when switching to OSX and Windows, before pushing always set manually Project Settings > XR interaction toolkit > use xr device simulator
    /// OVRManager interferes with XROrigin tracking origin level.
    ///  - So for the simulator, OVRManager must be set to eyelevel
    ///  - For non-simulator (Quest Link and Quest), FloorLevel to work properly
    /// We will recenter manually.
    /// </summary>
    private void Awake()
    {
        _xrPlayer.SetActive(true);

        #if USE_SIMULATOR
        
        // (1) make sure xrSimulator is on
        _xrSimulator.SetActive(true);
        _ovrManager.trackingOriginType = OVRManager.TrackingOrigin.EyeLevel;

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
        DebugManager.Instance.Log("Start - Setup player");
        //SetUpXrIntegrationToolkitBoundaryCheck();
    }

    private void OnEnable()
    {
        OVRManager.HMDMounted += OnHeadsetOn;
        OVRManager.HMDUnmounted += OnHeadsetOff;
        
        /*
        XRDevice.OnDeviceStarted += SetUpXrIntegrationToolkitBoundaryCheck;
        XRDevice.OnDeviceCanceled += CleanUpBoundaries;
        */
        
    }

    private void OnDisable()
    {
        OVRManager.HMDMounted -= OnHeadsetOn;
        OVRManager.HMDUnmounted -= OnHeadsetOff;
        
        /*
        XRDevice.OnDeviceStarted -= SetUpXrIntegrationToolkitBoundaryCheck;
        XRDevice.OnDeviceCanceled -= CleanUpBoundaries;
        */
    }

    private IEnumerator CheckIfBoundaryChanged()
    {
        for (_frameCounter = 0; _frameCounter < 20; _frameCounter++)
        {
            yield return new WaitForSeconds(10f);
            RecalculateBoundary(_frameCounter);
        }
        _forceBoundaryRecalculation = false;

    }

    private void OnHeadsetOff()
    {
        #if OCULUS_PLATFORM
        #endif
        
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
        _forceBoundaryRecalculation = true;
        _frameCounter = 0;
        DebugManager.Instance.Log("++++++++++++++++++++++++++++");

        /*
        List<Vector3> currentBoundariesXrIntegrationToolkit = new List<Vector3>();
        //Vector3 currentPlayAreaSize = _inputSubSystem.
        if (_inputSubSystem.TryGetBoundaryPoints(currentBoundariesXrIntegrationToolkit))
        {
            //got boundaries, keep only those which didn't change.
            
            if ((_boundariesXrToolkit != currentBoundariesXrIntegrationToolkit || _boundariesXrToolkit.Count != currentBoundariesXrIntegrationToolkit.Count))
            {
                _boundariesXrToolkit = currentBoundariesXrIntegrationToolkit;
                
                string pointsStr = string.Join(" ; ", _boundariesXrToolkit.Select(p => p.ToString()));
            
                DebugManager.Instance.Log("XROrigin Boundary: " + pointsStr);
            }
        }
        */
        StartCoroutine(CheckIfBoundaryChanged());

        StartCoroutine(PauseAtNextFrame());
#endif
    }

    private void RecalculateBoundary(int frameCounter)
    {

        if (OVRManager.boundary.GetConfigured())
        {
            _boundarySize = OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.PlayArea);
            _points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

            string pointsStr = "(" + frameCounter + ") PlayArea size:\n" + _boundarySize + "\n\n" + string.Join("\n", _points.Select(p => p.ToString()));

            if (shouldUseDebugUiText)
            {
                writeToDebugUi.Raise(pointsStr);
            }
            //DebugManager.Instance.Log("playArea size: " + _boundarySize + " --- OVRManager Boundary: " + pointsStr);
        }
    }

    public void Recenter()
    {
    /*
        _offset = _mainCamera.position - _xrOrigin.position;
        _offset.y = 0;
        _xrOrigin.position = _desiredCenterPosition.position + _offset;
      
        _targetForward = _desiredCenterPosition.forward;
        _targetForward.y = 0;
        _cameraForward = _mainCamera.forward;
        _cameraForward.y = 0;

        _angle = Vector3.SignedAngle(_cameraForward, _targetForward, Vector3.up);
      
        _xrOrigin.RotateAround(_mainCamera.position, Vector3.up, _angle);
     */
    }

    IEnumerator  PauseAtNextFrame()
    {
        yield return new WaitForEndOfFrame();

        Recenter();

        /*
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPaused = !EditorApplication.isPaused;
        }
        */
    }

    /*
    private void Update()
    {
        XRDevice.IsHmdMounted();
    }
    */
    
    /*
    private void CenterPlayer()
    {

        point1.transform.localPosition = (points[0]);
        point2.transform.localPosition = (points[1]);
        point3.transform.localPosition = (points[2]);
        point4.transform.localPosition = (points[3]);

 

        Vector3 pointA = midPoint(point1.transform.position, point2.transform.position);
        Vector3 pointB = midPoint(point3.transform.position, point4.transform.position);

 

        Vector3 between = pointB - pointA;

 

        float distance = between.magnitude;

 

        squareMarker.transform.position = pointA + (between / 2.0f);
        squareMarker.transform.LookAt(pointB);

 

        worldContainer.transform.position = squareMarker.transform.position;
        worldContainer.transform.rotation = squareMarker.transform.rotation;

 

    }

    */
    private Vector3 midPoint(Vector3 a, Vector3 b)
    {

        float x = (a.x + b.x) / 2;
        float y = (a.y + b.y) / 2;
        float z = (a.z + b.z) / 2;

        return new Vector3(x, y, z);

    }
    
    private void SetUpXrIntegrationToolkitBoundaryCheck()
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

        if (!_inputSubSystem.running)
        {
            // Start the subsystem if not started yet
            _inputSubSystem.Start();
        }
        
        if (_inputSubSystem != null)
        {
            //_inputSubSystem.boundaryChanged += RefreshBoundaries;
            //RefreshBoundaries(_inputSubSystem);
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
            
            if ((_boundariesXrToolkit != currentBoundaries || _boundariesXrToolkit.Count != currentBoundaries.Count))
            {
                _boundariesXrToolkit = currentBoundaries;
                /*
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
                */
                
            }
        }
    }
    
    
    private void CleanUpBoundaries()
    {
        if (_boundariesXrToolkit != null)
        {
            //_inputSubSystem.boundaryChanged -= RefreshBoundaries;
            //_inputSubSystem = null;
            _boundariesXrToolkit = null;
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

