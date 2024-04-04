using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerPlayerInvoke : MonoBehaviour
{
    public UnityEvent<Transform> OnTriggerEntered;
    public UnityEvent<Transform> OnTriggerExited;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(TagManager.PLAYER_COLLIDER_TAG))
        {
            OnTriggerEntered.Invoke(other.transform);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag(TagManager.PLAYER_COLLIDER_TAG))
        {
            OnTriggerExited.Invoke(other.transform);
        }
    }
}
