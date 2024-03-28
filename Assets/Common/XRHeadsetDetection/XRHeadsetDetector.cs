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

