using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// https://blog.devgenius.io/scriptableobject-game-events-1f3401bbde72

public class VoidGameEventListener : MonoBehaviour, IGameEventListener
{
    [Tooltip("Event to register with.")]
    [SerializeField]
    private GameEvent @event;

    [Tooltip("Response to invoke when Event is raised.")]
    [SerializeField]
    private UnityEvent response;
    private void OnEnable()
    {
        if (@event != null)
            @event.RegisterListener(this);
    }    
    private void OnDisable()
    {
        if (@event != null)
            @event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }
    
}