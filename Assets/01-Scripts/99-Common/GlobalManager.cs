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
    // TODO: create scriptable objects to handle disable/or not disable hand model when grabbing stuff (or other global definitions)
    [Space(height: 10)] [Tooltip("Disable Hand Model When Grabbing Object")]
    [SerializeField] private bool disableHandModelWhenGrabbing = true;
    
    private string _lastSceneName;
    private Transform _lastSceneCageOrigin;
    private Transform _lastSceneLocalStartLocation;
    private bool _isCalibrated = false;
    private bool _isBoundaryRotated = false;
    private HashSet<string> _scenesVisited = new HashSet<string>();
    
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
        IsCalibrated = false;
    }

    private void OnHeadsetOn()
    {
        SceneTransitionManager.Instance.GoToSceneAsyncByIndexInXSeconds(0, 1);
    }
    
        
    public void ResetGame()
    {
        _lastSceneName = null;
        _lastSceneCageOrigin = null;
        _lastSceneLocalStartLocation = null;
        ResetScenesVisited();
    }

    private void OnDestroy()
    {
        _scenesVisited = null;
    }
    
    
    #region Setters/Getters
    public HashSet<string> ScenesVisited => _scenesVisited;

    public bool DisableHandModelWhenGrabbing => disableHandModelWhenGrabbing;

    public void AddSceneVisited(string sceneName)
    {
        _scenesVisited.Add(sceneName);
    }

    public void RemoveSceneVisited(string sceneName)
    {
        _scenesVisited.Remove(sceneName);
    }

    public void ResetScenesVisited()
    {
        _scenesVisited = new HashSet<string>();
    }

    public string LastSceneName
    {
        get => _lastSceneName;
        set => _lastSceneName = value;
    }

    public Transform LastSceneCageOrigin
    {
        get => _lastSceneCageOrigin;
        set => _lastSceneCageOrigin = value;
    }

    public bool IsCalibrated
    {
        get => _isCalibrated;
        set => _isCalibrated = value;
    }

    public bool IsBoundaryRotated
    {
        get => _isBoundaryRotated;
        set => _isBoundaryRotated = value;
    }

    public Transform LastSceneLocalStartLocation
    {
        get => _lastSceneLocalStartLocation;
        set => _lastSceneLocalStartLocation = value;
    }

    #endregion

}
