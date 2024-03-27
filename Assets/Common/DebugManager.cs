using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private static DebugManager _instance;

    // Singleton instance
    public static DebugManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DebugManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("DebugManager");
                    _instance = obj.AddComponent<DebugManager>();
                }
            }
            return _instance;
        }
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

    // Log method
    public void Log(string message)
    {
        #if UNITY_EDITOR
            Debug.Log(message);
        #endif
    }

    // Add other logging methods as needed (Warning, Error, etc.)
}