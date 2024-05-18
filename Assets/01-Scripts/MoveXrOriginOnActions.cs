using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveXrOriginOnActions : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;

    private void OnEnable()
    {
        AbstractPortalToSomeNewPlace.OnPortalThrough += MoveXrOriginFromPortal;
        SetupScene.OnSceneSetUp += MoveXrOrigin;
    }

    private void OnDisable()
    {
        AbstractPortalToSomeNewPlace.OnPortalThrough -= MoveXrOriginFromPortal;
        SetupScene.OnSceneSetUp -= MoveXrOrigin;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void MoveXrOriginFromPortal(PortalSettings portalSettings)
    {
        MoveXrOrigin(portalSettings.NextCageOrigin);
    }
    
    public void MoveXrOrigin(GameAreaBoundaryProperties nextCageOrigin)
    {
        _characterController.enabled = false;
        float startLocationOffsetY = nextCageOrigin.StartLocation.localPosition.y;
        transform.position = nextCageOrigin.transform.position + new Vector3(0, startLocationOffsetY - 0.01f, 0);
        // explainer: Environment is the parent and it may be rotated 90 degrees based on boundary size
        // so what matters is the fake cage local rotation
        transform.rotation = nextCageOrigin.transform.localRotation;
        _characterController.enabled = true;
    }
}
