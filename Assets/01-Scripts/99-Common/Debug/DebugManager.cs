using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private static DebugManager _instance;

    // Singleton instance
    public static DebugManager Instance => _instance ?? (_instance = new DebugManager());

    private DebugManager()
    {
        // avoid external instantiation
    }

    private void Awake()
    {
        // Ensure only one instance of the DebugManager exists
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        // Only enable the logger if running in the Unity Editor
        #if UNITY_EDITOR
            DontDestroyOnLoad(gameObject);
        #else
            Destroy(gameObject);
        #endif
    }

    #region Log
    // Log method
    public void Log(string message)
    {
        #if UNITY_EDITOR
            Debug.Log(message);
        #endif
    }

    public void Warning(string message)
    {
        #if UNITY_EDITOR
            Debug.LogWarning(message);
        #endif
    }

    // Add other logging methods as needed (Warning, Error, etc.)

    #endregion

    #region HelpfulMethods
    /// <summary>
    /// When testing with Unity Editor - it may be helpful to freeze app on a certain point
    /// </summary>
    /// <returns></returns>
    public IEnumerator PauseAtNextFrame()
    {
        #if UNITY_EDITOR
        yield return new WaitForEndOfFrame();
        
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPaused = !EditorApplication.isPaused;
        }
        #else
        yield return null;
        #endif
    }
    
    #endregion
    
}