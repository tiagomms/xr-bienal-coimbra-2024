using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PortalToNewLocationInSameScene is exactly what it says - it holds the logic for opening a portal in a new location
/// in the same scene.
/// Usually it will be always open - but depending on setup users may open it up
/// </summary>
public class PortalToNewLocationInSameScene : AbstractPortalToSomeNewPlace
{
    [SerializeField] protected bool isAlwaysOpen = true;
    

    protected override void Awake()
    {
        portalSettings.PortalType = PortalType.ToNewLocationInSameScene;
        AlwaysOpenFlag = isAlwaysOpen;
        base.Awake();
    }

    public void OpenPortalToNewLocation(Transform transformIfSameScene)
    {
        portalSettings.NextSceneName = null;
        portalSettings.NextLocation = transformIfSameScene;
        base.OpenPortal();
    }

    public override void ActivatePortal(Transform player)
    {
        base.ActivatePortal(player);
        // Teleport the player to the destination position
        if (portalSettings.NextLocation != null)
        {
            OnPortalActivated?.Invoke(portalSettings);
            
            // TODO: check if rotation is needed to change as well
            player.position = portalSettings.NextLocation.position;
        }
    }
    
}
