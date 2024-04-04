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
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class GlobalManager : GenericSingleton<GlobalManager>
{
    public string lastSceneName;
    public Transform lastScenePortalEntry;
    public bool isCalibrated = false;
    public bool isBoundaryRotated = false;
    public HashSet<string> scenesVisited;
    
    public Vector3 boundarySize;
    public Vector3[] boundaryPoints;
    
    public static Action OnCalibrationCompleted;
    public static Action OnCalibrationLost;
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

    private void OnHeadsetOff()
    {
        isCalibrated = false;
    }

    private void OnHeadsetOn()
    {
        SceneTransitionManager.Instance.GoToSceneAsyncByIndexInXSeconds(0, 1);
    }
    
        
    public void ResetGame()
    {
        lastSceneName = null;
        lastScenePortalEntry = null;
        scenesVisited = new HashSet<string>();
    }

}
