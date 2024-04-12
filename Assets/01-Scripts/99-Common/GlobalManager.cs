#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
#define USE_SIMULATOR
#elif UNITY_ANDROID
#define USE_QUEST
#endif

using System;
using System.Collections.Generic;
using UnityEngine;


public enum PlatformUsed
{
    Simulator, Quest
}

public class GlobalManager : GenericSingleton<GlobalManager>
{
    // TODO: document very well globalSettings setup
    [Tooltip("Global Settings for testing/playing game - Mandatory")]
    [SerializeField] private GlobalSettings _globalSettings;
    
    [Space(height: 20)]
    [Tooltip("Only change values if and only if you have Quest Link on your Windows")]
    [SerializeField] private ConnectionTypeSettings _connectionType;
    
    private string _lastSceneName;
    private GameAreaBoundaryProperties _lastSceneCageOrigin;
    private bool _isCalibrated = false;
    private bool _isBoundaryRotated = false;
    private HashSet<string> _scenesVisited = new HashSet<string>();

    private PlatformUsed _currentPlatform;
    
    private Vector3 _boundarySize;
    private Vector3[] _boundaryPoints;
    
    public static Action OnCalibrationLost;

    /// <summary>
    /// Only makes sense to calibrate if and only if it is the quest - there is no calibration in the simulator
    /// </summary>
    public override void Awake()
    {
        GetPlatformUsed();
        base.Awake();
    }

    private void Start()
    {
        if (_currentPlatform == PlatformUsed.Quest)
        {
            ResetToCalibrationScene();
        }
    }
    
    private void OnEnable()
    {
        if (Settings != null && Settings.enableCalibrationOnTakingHeadsetOff)
        {
            OVRManager.HMDMounted += OnHeadsetOn;
            OVRManager.HMDUnmounted += OnHeadsetOff;    
        }
    }

    private void OnDisable()
    {
        if (Settings != null && Settings.enableCalibrationOnTakingHeadsetOff)
        {
            OVRManager.HMDMounted -= OnHeadsetOn;
            OVRManager.HMDUnmounted -= OnHeadsetOff;    
        }
    }

    private void OnHeadsetOff()
    {
        IsCalibrated = false;
        OnCalibrationLost.Invoke();
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
        /*
        if (_currentPlatform == PlatformUsed.Simulator)
        {
            Destroy(_globalSettings);
        }
        */

    }
    
    
    #region Setters/Getters
    public HashSet<string> ScenesVisited => _scenesVisited;

    public bool DisableHandModelWhenGrabbing => Settings != null && Settings. disableHandModelWhenGrabbing;

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

    public PlatformUsed CurrentPlatform
    {
        get => _currentPlatform;
        set => _currentPlatform = value;
    }

    public GlobalSettings Settings => _globalSettings;

    private void GetPlatformUsed()
    {
        PlatformUsed result = PlatformUsed.Quest;

        if (_connectionType == null)
        {
            _connectionType = ConnectionTypeSettings.CreateSimulatorInstance();
        }
        
#if USE_SIMULATOR
        if (_connectionType.type == ConnectionType.Standard)
        {
            result = PlatformUsed.Simulator;
        }
#endif

        _currentPlatform = result;
    }

    #endregion

}
