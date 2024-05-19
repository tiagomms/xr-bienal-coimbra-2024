using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <> denotes this is a generic class
public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    // create a private reference to T instance
    private static T _instance;

    private static bool _canCreate = true;

    public static T Instance
    {
        get
        {
            // if instance is null
            if (_instance == null && _canCreate)
            {
                // find the generic instance
                _instance = FindObjectOfType<T>();

                // if it's null again create a new object
                // and attach the generic instance
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        // create the instance
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public virtual void ForceDestroy()
    {
        if (_instance != null)
        {
            _instance = null;
            Destroy(this.gameObject);
        }
    }
}
