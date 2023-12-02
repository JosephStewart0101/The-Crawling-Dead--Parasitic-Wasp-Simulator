using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Doesn't destroy new instances of the object, overrides them instead.
/// Useful for resetting the state.
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// Allows for only one of this object to exist in the scene
/// </summary>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        base.Awake();
    }

    public virtual void Start()
    {
        
    }
}

/// <summary>
/// Base class for singleton persistence. Use if you want a class to be exclusive to one gameObject
/// and the object will remain through scene changes. (Useful for managers & timers)
/// </summary>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}