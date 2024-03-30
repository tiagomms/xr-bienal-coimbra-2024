using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableGrabbingHandModel : MonoBehaviour
{
    private GameObject[] _leftHandModels;
    private GameObject[] _rightHandModels;

    void Awake()
    {
        _leftHandModels = GameObject.FindGameObjectsWithTag(Tags.LEFT_HAND_MODEL_TAG);
        _rightHandModels = GameObject.FindGameObjectsWithTag(Tags.RIGHT_HAND_MODEL_TAG);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(HideGrabbingHand);
        grabInteractable.selectExited.AddListener(ShowGrabbingHand);
    }

    private void HideGrabbingHand(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.parent.CompareTag(Tags.LEFT_HAND_TAG))
        {
            SetActiveToGameObjectsArray(_leftHandModels, false);
        }
        else if (args.interactorObject.transform.parent.CompareTag(Tags.RIGHT_HAND_TAG))
        {
            SetActiveToGameObjectsArray(_rightHandModels, false);
        }
    }

    private void ShowGrabbingHand(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.parent.CompareTag(Tags.LEFT_HAND_TAG))
        {
            SetActiveToGameObjectsArray(_leftHandModels, true);
        }
        else if (args.interactorObject.transform.parent.CompareTag(Tags.RIGHT_HAND_TAG))
        {
            SetActiveToGameObjectsArray(_rightHandModels, true);
        }
    }
    
    
    private void SetActiveToGameObjectsArray(GameObject[] array, bool isActive)
    {
        foreach (GameObject o in array)
        {
            o.SetActive(isActive);
        }
    }

}
