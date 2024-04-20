using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// from: https://forum.unity.com/threads/changing-objects-to-from-static-at-runtime.575080/ at the bottom
public class ForceObjectToBeStaticViaScript : MonoBehaviour
{
    [SerializeField] private bool setStaticAfterSceneSetup = true;
    [SerializeField] private bool forceChildrenToBeStatic = false;
    private bool isAlreadyStatic = false;

    private void Start() {
        isAlreadyStatic = gameObject.isStatic;
    }

    private void OnEnable() 
    {
        // plan A - trigger right after event
        if (setStaticAfterSceneSetup)
        {
            SetupScene.OnSceneSetUp += ForceObjectToBeStaticAfterSceneSetup;
        }

        // plan B - if object was inactive and scene was already setup
        if (SetupScene.IsSceneSetup)
        {
            ForceObjectToBeStatic();
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
            SetObjectStatic(this.gameObject, false);

            /*
            GameObject newThing = this.gameObject;
            newThing.isStatic = true;
            newThing.transform.parent = transform;
            StaticBatchingUtility.Combine(gameObject);
            isAlreadyStatic = true;

            */

            if (forceChildrenToBeStatic)
            {
                Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>(false);
                foreach (Transform child in allChildren)
                {
                    SetObjectStatic(child.gameObject, true);
                }
            }
            isAlreadyStatic = true;
        }
    }

    private void SetObjectStatic(GameObject gb, bool traversingChildren = false)
    {
        GameObject newThing = gb;
        newThing.isStatic = true;
        if (!traversingChildren)
        {
            newThing.transform.parent = transform;
        }
        StaticBatchingUtility.Combine(gb);
    }
}
