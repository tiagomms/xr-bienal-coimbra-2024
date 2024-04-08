using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
// by requiring this component it makes sure 
[RequireComponent(typeof(DisableGrabbingHandModel))]


// Setup XR Grab Interactable
// like Valem Tutorial: https://www.youtube.com/watch?v=Gs1_KpA7UdA
// but instead of making this process for every single object - make it automatically on awake
public class SetupXrGrabbableObjects : MonoBehaviour
{
    [Tooltip("Rigidbody definitions")] 
    [SerializeField] private float mass = 1f;
    [SerializeField] private bool useGravity = false;
    [SerializeField] private bool isKinematic = true;
    
    [Space(height: 10)]
    [Tooltip("Meshcollider definitions")]
    [SerializeField] private bool setAllConvex = true;
    
    private Rigidbody _rigidbody;
    private XRGrabInteractable _xrGrabInteractable;
    private MeshCollider[] _meshColliders;
    private DisableGrabbingHandModel _disableGrabbingHandModel;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _xrGrabInteractable = GetComponent<XRGrabInteractable>();
        _meshColliders = GetComponentsInChildren<MeshCollider>();
        _disableGrabbingHandModel = GetComponent<DisableGrabbingHandModel>();
        
        // make sure any internal mesh collider is set to convex due to rigidbody
        if (setAllConvex)
        {
            foreach (MeshCollider meshCollider in _meshColliders)
            {
                meshCollider.convex = true;
            }    
        }
        
        // rigidbody: set continuous movement detection and serialize field values
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rigidbody.mass = mass;
        _rigidbody.useGravity = useGravity;
        _rigidbody.isKinematic = isKinematic;
        
        // xr grab interactable: movement type, velocity tracking; use dynamic attach - true
        _xrGrabInteractable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        _xrGrabInteractable.useDynamicAttach = true;
        
    }

    private void Start()
    {
        // disable hand model when grabbing object?
        _disableGrabbingHandModel.enabled = GlobalManager.Instance.DisableHandModelWhenGrabbing;
    }
}
