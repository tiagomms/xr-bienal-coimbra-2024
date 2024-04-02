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
    None,
    Started,
    Working,
    Canceled,
    NotSupported,
    Weird,
    Failed
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

    private static XRDeviceState xrDeviceState = XRDeviceState.None;
    public static XRDeviceState XrDeviceState { get => xrDeviceState; }

    public static event Action OnDeviceStarted;
    public static event Action OnDeviceCanceled;

    /// <summary>
    /// returns true if the HMD is mounted on the users head. Returns false if the current headset does not support this feature or if the HMD is not mounted.
    /// </summary>
    /// <returns></returns>
    public static XRDeviceState IsHmdMounted()
    {
        #if UNITY_EDITOR_OSX
        
            // works on simulator
            SetMountedStateToTrue();

        #elif UNITY_ANDROID || UNITY_EDITOR_WIN

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
                        xrDeviceState = XRDeviceState.Canceled;
                        OnDeviceCanceled?.Invoke();
                    }
                }
                else 
                {
                    xrDeviceState = XRDeviceState.NotSupported;
                }
            } else
            {
                xrDeviceState = XRDeviceState.Failed;
            }
        #endif
        //DebugManager.Instance.Log("XR Device State ----------- " + xrDeviceState.ToString());
        return xrDeviceState;
    }

    private static void SetMountedStateToTrue()
    {
        if (xrDeviceState == XRDeviceState.Started)
        {
            xrDeviceState = XRDeviceState.Working;
        }
        else if (xrDeviceState != XRDeviceState.Working)
        {
            xrDeviceState = XRDeviceState.Started;
            OnDeviceStarted?.Invoke();
        }
    }
}