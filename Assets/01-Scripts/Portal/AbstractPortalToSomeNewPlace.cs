using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using UnityEngine;
using UnityEngine.Events;

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
    
    public Transform NextCageOrigin { get; set; }
}

public abstract class AbstractPortalToSomeNewPlace : MonoBehaviour
{
    [SerializeField] protected AudioClip portalSound;
    [SerializeField] protected float audioVolume = 1f;
    [SerializeField] protected float audioMaxDistance = 3f;

    [Space(height: 10)] 
    [Tooltip("Portal Settings when Fading")]
    [SerializeField] protected PortalSettings portalSettings;
    
    public static Action<PortalSettings> OnPortalEnter;
    public static Action<PortalSettings> OnPortalThrough;

    protected bool AlwaysOpenFlag = false;
    private bool _isOpen = false;
    private AudioSource _audioSource;
    private GameObject _portalPrefab;
    
    protected virtual void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        _portalPrefab = transform.GetChild(0).gameObject;
        // to make sure portal is not seen at start unless it is always open
        _portalPrefab.SetActive(AlwaysOpenFlag);
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
        _portalPrefab.SetActive(true);
        
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
        _portalPrefab.SetActive(false);
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
