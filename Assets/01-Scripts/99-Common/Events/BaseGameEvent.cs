using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// based on https://forum.unity.com/threads/generic-scriptable-object-events.1148393/
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public abstract class BaseGameEvent<TParameter> : ScriptableObject
{
    private List<IEventListener<TParameter>> listeners = new List<IEventListener<TParameter>>();
    public void RegisterListener(IEventListener<TParameter> _listener)
    {
        listeners.Add(_listener);
    }
 
    public void UnregisterListener(IEventListener<TParameter> _listener)
    {
        listeners.Remove(_listener);
    }
 
    public void Raise(TParameter _t)
    {
        // there was a small bug here; --i replaced by i--
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].RaiseEvent(_t);
        }
    }
}