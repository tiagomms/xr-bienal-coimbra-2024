using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerTransformOnActions : MonoBehaviour
{
    private void OnEnable()
    {
        AbstractPortalToSomeNewPlace.OnPortalThrough += ChangePlayerTransformComingFromPortal;
        SetupScene.OnSceneSetUp += ChangePlayerTransform;
    }

    private void OnDisable()
    {
        AbstractPortalToSomeNewPlace.OnPortalThrough -= ChangePlayerTransformComingFromPortal;
        SetupScene.OnSceneSetUp -= ChangePlayerTransform;
    }

    private void ChangePlayerTransformComingFromPortal(PortalSettings portalSettings)
    {
        ChangePlayerTransform(portalSettings.NextCageOrigin);
    }
    
    public void ChangePlayerTransform(GameAreaBoundaryProperties nextCageOrigin)
    {
        float startLocationOffsetY = nextCageOrigin.StartLocation.localPosition.y;
        transform.position = nextCageOrigin.transform.position + new Vector3(0, startLocationOffsetY - 0.01f, 0);
        // explainer: Environment is the parent and it may be rotated 90 degrees based on boundary size
        // so what matters is the fake cage local rotation
        transform.rotation = nextCageOrigin.transform.localRotation;
    }
}
