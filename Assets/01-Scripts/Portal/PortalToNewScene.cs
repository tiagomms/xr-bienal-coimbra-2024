using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Portal To New Scene should never be opened from the start - so no need to add that logic here
/// </summary>
public class PortalToNewScene : AbstractPortalToSomeNewPlace
{
    [Space(height: 20)]
    [Tooltip("Write here the scene name just like the scene file")]
    [SerializeField] private string nextSceneName;
    protected override void Awake()
    {
        portalSettings.PortalType = PortalType.ToNewScene;
        base.Awake();
    }

    // MARK: I know this situation is not ideal, but UnityEvents were killing me. Better keep it simple
    public void OpenPortalToNewScene()
    {
        base.OpenPortal();
    }
    
    protected override void SetPortalNextStepSettings()
    {
        portalSettings.NextSceneName = nextSceneName;
        portalSettings.NextCageOrigin = null;
    }
    
    public override void EnterPortal(Transform player)
    {
        base.EnterPortal(player);
        if (portalSettings.NextSceneName != null)
        {
            // Scene Transition Manager is reading this
            OnPortalEnter?.Invoke(portalSettings);    
        }
        
    }
}
