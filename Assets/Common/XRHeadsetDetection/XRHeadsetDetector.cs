using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;


/// <summary>
/// https://forum.unity.com/threads/openxr-new-input-system-detect-hmd-removed.1096345/
/// 
/// Setup: Unity Editor + Quest 2 via Cable Link
/// Unity version: 2021.3.17
/// Relevant packages: OpenXR@1.6.0, Input System@1.4.4
/// 2. Add script instance to the scene and run Play mode.
/// 3. Set Action as Button and bind to XR HMD/userPresence
/// 4. Put the headset on and off.
/// </summary>
public class XRHeadsetDetector : MonoBehaviour
    {
        public InputAction UserPresence;
 
        private void Start()
        {
            UserPresence.started += x => Debug.Log("Headset is on");
            UserPresence.canceled += x => Debug.Log("Headset is off");
        }
 
        private void OnEnable()
        {
            UserPresence.Enable();
        }
 
        private void OnDisable()
        {
            UserPresence.Disable();
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