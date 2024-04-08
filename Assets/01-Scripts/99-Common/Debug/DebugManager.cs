using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class DebugManager : GenericSingleton<DebugManager>
{
    #region Logs
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