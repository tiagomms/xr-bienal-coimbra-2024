using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public enum PortalType
{
    ToNewScene, ToNewLocationInSameScene, EndGameScene
}

[System.Serializable]
public class PortalSettings
{
    public float enterPortalAnimDuration = 3f;
    public float leavePortalAnimDuration = 3f;
    public Color fadeBaseColor;

    public PortalType PortalType { get; set; }
    public string NextSceneName { get; set; }
    
    public GameAreaBoundaryProperties NextCageOrigin { get; set; }
}

public abstract class AbstractPortalToSomeNewPlace : MonoBehaviour
{
    [Tooltip("Drag here Portal Mesh if you change it")] 
    [SerializeField]
    protected GameObject _portalMeshPrefab;
    
    [Space(height: 20)]
    [Tooltip("Set portal sound when opened, if you want")]
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip portalSound;
    [SerializeField] protected float audioVolume = 1f;
    [SerializeField] protected float audioMaxDistance = 3f;

    [Space(height: 20)] 
    [Tooltip("Portal Settings when Fading")]
    [SerializeField] protected PortalSettings portalSettings;
    
    public static Action<PortalSettings> OnPortalEnter;
    public static Action<PortalSettings> OnPortalThrough;

    protected bool AlwaysOpenFlag = false;
    private bool _isOpen = false;
    
    protected virtual void Awake()
    {
        // to make sure portal is not seen at start unless it is always open
        _portalMeshPrefab.SetActive(AlwaysOpenFlag);
        _isOpen = AlwaysOpenFlag;
    }

    protected virtual void Start()
    {
        if (AlwaysOpenFlag)
        {
            OpenPortal();
        }
    }

    protected virtual void OpenPortal()
    {
        if (_isOpen && !AlwaysOpenFlag) return;
        
        _isOpen = true;
        _portalMeshPrefab.SetActive(true);
        
        if (_audioSource != null && portalSound != null)
        {
            _audioSource.clip = portalSound;
            _audioSource.volume = audioVolume;
            _audioSource.maxDistance = audioMaxDistance;
            _audioSource.Play();
        }
        
    }

    /// <summary>
    /// Portal is never closed if it is always open x) - opposite condition of open portal
    /// </summary>
    public virtual void ClosePortal()
    {
        if (!(_isOpen && !AlwaysOpenFlag)) return;
        
        _isOpen = false;
        _portalMeshPrefab.SetActive(false);
        portalSettings.NextSceneName = null;
        
            
        if (_audioSource != null)
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        }
        
    }

    public virtual void EnterPortal(Transform player)
    {
        if (!_isOpen)
            return;
        
    }
    
    protected virtual void LeavePortal(Transform player)
    {
        if (!_isOpen)
            return;
        
    }
}
