using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Portal To New Scene should never be opened from the start - so no need to add that logic here
/// </summary>
public class PortalToNewScene : AbstractPortalToSomeNewPlace
{
    protected override void Awake()
    {
        portalSettings.PortalType = PortalType.ToNewScene;
        base.Awake();
    }

    // MARK: I know this situation is not ideal, but UnityEvents were killing me. Better keep it simple
    public void OpenPortalToNewScene(string nextSceneName)
    {
        portalSettings.NextSceneName = nextSceneName;
        portalSettings.NextLocation = null;
        base.OpenPortal();
    }
    
    public override void ActivatePortal(Transform player)
    {
        base.ActivatePortal(player);

        if (portalSettings.NextSceneName != null)
        {
            // Scene Transition Manager is reading this
            OnPortalActivated?.Invoke(portalSettings);    
        }
        
    }
}
