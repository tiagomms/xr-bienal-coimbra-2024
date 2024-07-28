using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  https://blog.devgenius.io/scriptableobject-game-events-1f3401bbde72

public interface IGameEventListener
{
    void OnEventRaised();
}

[CreateAssetMenu(fileName = "GameEvent", menuName = "Game Events/Void Event")]
public class GameEvent : ScriptableObject
{
    private readonly List<IGameEventListener> m_eventListeners = new List<IGameEventListener>();

    public void Raise()
    {
        for (int i = m_eventListeners.Count - 1; i >= 0; i--)
        {
            m_eventListeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(IGameEventListener listener)
    {
        if (!m_eventListeners.Contains(listener))
            m_eventListeners.Add(listener);
    }
    public void UnregisterListener(IGameEventListener listener)
    {
        if (m_eventListeners.Contains(listener))
            m_eventListeners.Remove(listener);
    }
}


