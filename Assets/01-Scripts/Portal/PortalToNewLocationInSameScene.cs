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
    private Transform _nextLocation;

    protected override void Awake()
    {
        AlwaysOpenFlag = isAlwaysOpen;
        base.Awake();
    }
    
    public void OpenPortalToNewLocation(Transform transformIfSameScene)
    {
        base.OpenPortal();
        _nextLocation = transformIfSameScene;
    }

    public override void ActivatePortal(Transform player)
    {
        base.ActivatePortal(player);
        // Teleport the player to the destination position
        if (_nextLocation != null)
        {
            player.position = _nextLocation.position;
            // TODO: check if rotation is needed to change as well
        }
    }
    
}
