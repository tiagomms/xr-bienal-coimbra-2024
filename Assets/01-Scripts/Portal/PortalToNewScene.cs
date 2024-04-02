using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Portal To New Scene should never be opened from the start - so no need to add that logic here
/// </summary>
public class PortalToNewScene : AbstractPortalToSomeNewPlace
{
    private string _nextSceneName;

    // MARK: I know this situation is not ideal, but UnityEvents were killing me. Better keep it simple
    public void OpenPortalToNewScene(string nextSceneName)
    {
        base.OpenPortal();
        _nextSceneName = nextSceneName;
    }
    
    public override void ActivatePortal(Transform player)
    {
        base.ActivatePortal(player);
        // Load the next scene
        LoadScene.LoadSceneUsingName(_nextSceneName);
    }
}
