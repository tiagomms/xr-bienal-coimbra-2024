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

    public override void EnterPortal(Transform player)
    {
        base.EnterPortal(player);
        // Teleport the player to the destination position
        if (portalSettings.NextLocation != null)
        {
            StartCoroutine(GoThroughPortalInSameScene(player));
        }
    }

    private IEnumerator GoThroughPortalInSameScene(Transform player)
    {
        OnPortalEnter?.Invoke(portalSettings);
        
        yield return new WaitForSeconds(portalSettings.enterPortalAnimDuration);
        
        LeavePortal(player);
    }

    protected override void LeavePortal(Transform player)
    {
        base.LeavePortal(player);

        OnPortalThrough?.Invoke(portalSettings);

        // TODO: check if rotation is needed to change as well
        player.position = portalSettings.NextLocation.position;
    }
}
