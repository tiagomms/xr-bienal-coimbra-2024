using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class DisableGrabbingHandModel : MonoBehaviour
{
    private GameObject[] _leftHandModels;
    private GameObject[] _rightHandModels;
    private XRGrabInteractable _grabInteractable;

    void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        _grabInteractable.selectEntered.AddListener(HideGrabbingHand);
        _grabInteractable.selectExited.AddListener(ShowGrabbingHand);
    }

    private void OnDisable()
    {
        _grabInteractable.selectEntered.RemoveListener(HideGrabbingHand);
        _grabInteractable.selectExited.RemoveListener(ShowGrabbingHand);
    }

    private void HideGrabbingHand(SelectEnterEventArgs args)
    {
        // TODO: get XROrigin event that detects change of hand / controller 
        _leftHandModels = GameObject.FindGameObjectsWithTag(TagManager.LEFT_HAND_MODEL_TAG);
        _rightHandModels = GameObject.FindGameObjectsWithTag(TagManager.RIGHT_HAND_MODEL_TAG);
        
        if (args.interactorObject.transform.parent.CompareTag(TagManager.LEFT_HAND_TAG))
        {
            SetActiveToGameObjectsArray(_leftHandModels, false);
        }
        else if (args.interactorObject.transform.parent.CompareTag(TagManager.RIGHT_HAND_TAG))
        {
            SetActiveToGameObjectsArray(_rightHandModels, false);
        }
    }

    private void ShowGrabbingHand(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.parent.CompareTag(TagManager.LEFT_HAND_TAG))
        {
            SetActiveToGameObjectsArray(_leftHandModels, true);
        }
        else if (args.interactorObject.transform.parent.CompareTag(TagManager.RIGHT_HAND_TAG))
        {
            SetActiveToGameObjectsArray(_rightHandModels, true);
        }
        
        _leftHandModels = null;
        _rightHandModels = null;
    }
    
    
    private void SetActiveToGameObjectsArray(GameObject[] array, bool isActive)
    {
        foreach (GameObject o in array)
        {
            o.SetActive(isActive);
        }
    }

}
