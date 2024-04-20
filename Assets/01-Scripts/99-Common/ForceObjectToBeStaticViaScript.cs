using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceObjectToBeStaticViaScript : MonoBehaviour
{
    [SerializeField] private bool setStaticAfterSceneSetup = true;
    private bool isAlreadyStatic = false;

    private void Start() {
        isAlreadyStatic = gameObject.isStatic;
    }

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
        if (!isAlreadyStatic)
        {
            GameObject newThing = this.gameObject;
            newThing.isStatic = true;
            newThing.transform.parent = transform;
            StaticBatchingUtility.Combine(gameObject);
            isAlreadyStatic = true;
        }
    }
}
