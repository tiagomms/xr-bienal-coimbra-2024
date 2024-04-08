using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PortalToNewLocationInSameScene is exactly what it says - it holds the logic for opening a portal in a new location
/// in the same scene.
/// Usually it will be always open - but depending on setup users may open it up
/// </summary>
public class PortalToNewCageInSameScene : AbstractPortalToSomeNewPlace
{
    [Space(height: 20)]
    [Tooltip("Drag here the next player boundary")]
    [SerializeField] protected GameAreaBoundaryProperties nextPlayerBoundary;
    [SerializeField] protected bool isAlwaysOpen = true;

    protected override void Awake()
    {
        portalSettings.PortalType = PortalType.ToNewLocationInSameScene;
        AlwaysOpenFlag = isAlwaysOpen;
        base.Awake();
    }

    public void OpenPortalToNewLocation()
    {
        portalSettings.NextSceneName = null;
        portalSettings.NextCageOrigin = nextPlayerBoundary;
        base.OpenPortal();
    }

    public override void EnterPortal(Transform playerCollider)
    {
        base.EnterPortal(playerCollider);
        // Teleport the player to the destination position
        if (portalSettings.NextCageOrigin != null)
        {
            StartCoroutine(GoThroughPortalInSameScene(playerCollider));
        }
    }

    private IEnumerator GoThroughPortalInSameScene(Transform playerCollider)
    {
        OnPortalEnter?.Invoke(portalSettings);
        
        yield return new WaitForSeconds(portalSettings.enterPortalAnimDuration);
        
        LeavePortal(playerCollider);
    }

    protected override void LeavePortal(Transform playerCollider)
    {
        base.LeavePortal(playerCollider);

        OnPortalThrough?.Invoke(portalSettings);

    }
}
