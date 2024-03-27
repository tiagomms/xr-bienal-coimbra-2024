using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

/// <summary>
/// Max Aigner: https://forum.unity.com/threads/openxr-new-input-system-detect-hmd-removed.1096345/
/// </summary>
/// 

public enum XRDeviceState
{
    none,
    started,
    working,
    canceled,
    notSupported,
    weird,
    failed
}

public class XRDevice
{
    private static UnityEngine.XR.InputDevice headDevice;
    public XRDevice() {
        if (headDevice == null)
        {
            headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        }
    }

    private static XRDeviceState xrDeviceState = XRDeviceState.none;
    public static XRDeviceState XrDeviceState { get => xrDeviceState; }

    public static event Action onDeviceStarted;
    public static event Action onDeviceCanceled;

    /// <summary>
    /// returns true if the HMD is mounted on the users head. Returns false if the current headset does not support this feature or if the HMD is not mounted.
    /// </summary>
    /// <returns></returns>
    public static XRDeviceState IsHMDMounted()
    {
        #if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
            SetMountedStateToTrue();

        #elif UNITY_ANDROID

            if(headDevice == null || headDevice.isValid == false)
            {
                headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
            }
            if (headDevice != null)
            {
                bool presenceFeatureSupported = headDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.userPresence, out bool userPresent);
                if (headDevice.isValid && presenceFeatureSupported)
                {
                    if (userPresent)
                    {
                        SetMountedStateToTrue();
                    }
                    else
                    {
                        xrDeviceState = XRDeviceState.canceled;
                        onDeviceCanceled?.Invoke();
                    }
                }
                else 
                {
                    xrDeviceState = XRDeviceState.notSupported;
                }
            } else
            {
                xrDeviceState = XRDeviceState.failed;
            }
        #endif
        DebugManager.Instance.Log("XR Device State ----------- " + xrDeviceState.ToString());
        return xrDeviceState;
    }

    private static void SetMountedStateToTrue()
    {
        if (xrDeviceState != XRDeviceState.started && xrDeviceState != XRDeviceState.working)
        {
            xrDeviceState = XRDeviceState.started;
            onDeviceStarted?.Invoke();
        }
        else if (xrDeviceState == XRDeviceState.started)
        {
            xrDeviceState = XRDeviceState.working;
        }
        else
        {
            xrDeviceState = XRDeviceState.weird;
        }
    }
}