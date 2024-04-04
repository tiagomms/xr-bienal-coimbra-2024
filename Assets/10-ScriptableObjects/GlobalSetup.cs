using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSetup : ScriptableObject
{
    public string lastSceneName;
    public Transform lastScenePortalEntry;
    public bool isBoundaryRotated = false;
    public HashSet<string> scenesVisited;

    public void Reset()
    {
        lastSceneName = null;
        lastScenePortalEntry = null;
        scenesVisited = new HashSet<string>();
    }
}
