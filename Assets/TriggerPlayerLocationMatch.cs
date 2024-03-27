using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPlayerLocationMatch : MonoBehaviour
{
    public UnityEvent onMatch;
    //public UnityEvent noLongerMatch;
    [SerializeField] float invokeOnMatchDelay = 2f;
    [SerializeField] float dotForwardThreshold = 0.75f;

    private bool isMatching = false;
    private bool alreadyMatched = false;

    private Coroutine currentCoroutine;
    private void OnTriggerStay(Collider other) {
        if (!alreadyMatched)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!isMatching && Vector3.Dot(transform.forward, other.transform.forward) > dotForwardThreshold)
                {
                    isMatching = true;
                    DebugManager.Instance.Log("isMatching!");
                    currentCoroutine = StartCoroutine(InvokeOnMatchAfterXSeconds());
                }
                else if (isMatching && Vector3.Dot(transform.forward, other.transform.forward) <= dotForwardThreshold)
                {
                    NoLongerMatching();
                }
            }
        }
    }

    private void NoLongerMatching()
    {
        isMatching = false;
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = null;
        DebugManager.Instance.Log(" not Matching!");
    }

    private void OnTriggerExit(Collider other) {
        if (!alreadyMatched)
        {
            NoLongerMatching();
        }
    }

    IEnumerator InvokeOnMatchAfterXSeconds()
    {
        yield return new WaitForSeconds(invokeOnMatchDelay);

        if (isMatching)
        {
            onMatch.Invoke();
            alreadyMatched = true;
            DebugManager.Instance.Log("WE HAVE A MATCH!");
        }
    }
}
