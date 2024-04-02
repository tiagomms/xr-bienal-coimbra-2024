using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractPortalToSomeNewPlace : MonoBehaviour
{
    [SerializeField] protected AudioClip portalSound;
    [SerializeField] protected float audioVolume = 1f;
    [SerializeField] protected float audioMaxDistance = 3f;
    
    [Space(height: 30)]
    // Unity event for opening the portal
    public UnityEvent onPortalOpened;   
    public UnityEvent onPortalClosed;

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
        
        onPortalOpened.Invoke();
    }

    /// <summary>
    /// Portal is never closed if it is always open x) - opposite condition of open portal
    /// </summary>
    public virtual void ClosePortal()
    {
        if (!(_isOpen && !AlwaysOpenFlag)) return;
        
        _isOpen = false;
        _portalPrefab.SetActive(false);
            
        if (_audioSource != null)
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        }
        
        onPortalClosed.Invoke();
    }

    public virtual void ActivatePortal(Transform player)
    {
        if (!_isOpen)
            return;
    }
}
