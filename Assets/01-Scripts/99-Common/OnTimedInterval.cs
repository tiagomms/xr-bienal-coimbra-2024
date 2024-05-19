using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Class that triggers an event between min and maxInterval seconds randomly in loop forever
/// if you want to set an event at a specific time - place the same value in both min and max interval 
/// </summary>
public class OnTimedInterval : MonoBehaviour
{
    [Tooltip("The minimum range")] 
    public float minInterval = 0.0f;

    [Tooltip("The maximum range")] 
    public float maxInterval = 1.0f;

    public UnityEvent onIntervalElapsed;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IntervalRoutine());
    }

    private IEnumerator IntervalRoutine()
    {
        float interval = Random.Range(minInterval, maxInterval);
        yield return new WaitForSeconds(interval);

        PlayEvent();
        //StartCoroutine(IntervalRoutine());
    }

    private void PlayEvent()
    {
        onIntervalElapsed.Invoke();
    }
}
