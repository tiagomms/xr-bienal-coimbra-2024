using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectStatic : MonoBehaviour
{
    [SerializeField] private bool setStaticAfterSceneSetup = true;

    private void OnEnable() 
    {
        if (setStaticAfterSceneSetup)
        {
            SetupScene.OnSceneSetUp += ForceObjectToBeStaticAfterSceneSetup;
        }
    }

    private void OnDisable ()
    {
        if (setStaticAfterSceneSetup)
        {
            SetupScene.OnSceneSetUp -= ForceObjectToBeStaticAfterSceneSetup;
        }
    }

    private void ForceObjectToBeStaticAfterSceneSetup(GameAreaBoundaryProperties ga)
    {
        ForceObjectToBeStatic();
    }

    // set object static
    public void ForceObjectToBeStatic()
    {
        GameObject newThing = this.gameObject;
        newThing.isStatic = true;
        newThing.transform.parent = transform;
        StaticBatchingUtility.Combine(gameObject);
    }
}
