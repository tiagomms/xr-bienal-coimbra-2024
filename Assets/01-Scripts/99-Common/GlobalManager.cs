using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : GenericSingleton<GlobalManager>
{
    // TODO: create scriptable objects to handle disable/or not disable hand model when grabbing stuff (or other global definitions)
    private GlobalSettings _globalSettings;
    private string _lastSceneName;
    private GameAreaBoundaryProperties _lastSceneCageOrigin;
    private bool _isCalibrated = false;
    private bool _isBoundaryRotated = false;
    private HashSet<string> _scenesVisited = new HashSet<string>();
    
    private Vector3 _boundarySize;
    private Vector3[] _boundaryPoints;
    
    public static Action OnCalibrationCompleted;
    public static Action OnCalibrationLost;

    private void Start()
    {
        ResetToCalibrationScene();
    }

    private void OnEnable()
    {
        SetupCalibration.OnCalibrationStart += SetGlobalSettings;
    }

    private void OnDisable()
    {
        if (_globalSettings != null && _globalSettings.EnableCalibrationOnTakingHeadsetOff)
        {
            OVRManager.HMDMounted -= OnHeadsetOn;
            OVRManager.HMDUnmounted -= OnHeadsetOff;    
        }
        SetupCalibration.OnCalibrationStart -= SetGlobalSettings;
    }

    private void SetGlobalSettings(GlobalSettings newSettings)
    {
        // only apply once
        if (_globalSettings != null) return;

        _globalSettings = newSettings;

        if (_globalSettings.EnableCalibrationOnTakingHeadsetOff)
        {
            OVRManager.HMDMounted += OnHeadsetOn;
            OVRManager.HMDUnmounted += OnHeadsetOff;
        }
    }

    private void OnHeadsetOff()
    {
        IsCalibrated = false;
    }

    private void OnHeadsetOn()
    {
        ResetToCalibrationScene();
    }

    private void ResetToCalibrationScene()
    {
        SceneTransitionManager.Instance.GoToSceneAsyncByIndexInXSeconds(0, 1);
    }
    
        
    public void ResetGame()
    {
        _lastSceneName = null;
        _lastSceneCageOrigin = null;
        ResetScenesVisited();
    }

    private void OnDestroy()
    {
        _scenesVisited = null;
    }
    
    
    #region Setters/Getters
    public HashSet<string> ScenesVisited => _scenesVisited;

    public bool DisableHandModelWhenGrabbing => _globalSettings != null && _globalSettings. DisableHandModelWhenGrabbing;

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

    public GameAreaBoundaryProperties LastSceneCageOrigin
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

    public Vector3 BoundarySize
    {
        get => _boundarySize;
        set => _boundarySize = value;
    }

    public Vector3[] BoundaryPoints
    {
        get => _boundaryPoints;
        set => _boundaryPoints = value;
    }

    #endregion

}
