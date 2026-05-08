using System;
using Code.Scripts.Src.Configurations;
using UnityEngine;

namespace Code.Scripts.Src.Utils
{
  public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
  {
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static T Instance
    {
      get
      {
        if (_applicationIsQuitting)
        {
          Debug.LogWarning($"Singleton Instance of {typeof(T)} is already destroyed. Returning null.");
          return null;
        }

        lock (_lock)
        {
          if (_instance == null)
          {
            Debug.LogWarning($"No singleton class {typeof(T)} has been attached to gameObject, creating new one.");
            var singletonObject = new GameObject(typeof(T).Name);
            _instance = singletonObject.AddComponent<T>();
            DontDestroyOnLoad(singletonObject);
          }

          return _instance;
        }
      }
    }

    protected virtual void Awake()
    {
      if (_instance == null)
      {
        _instance = this as T;
      }
      else if (_instance != this && DebugConfigurations.DebugEnabled)
      {
        throw new Exception($"Multiple gameObjects ({name}) contains singleton that will cause errors!");
      }
    }

    protected virtual void OnApplicationQuit()
    {
      _applicationIsQuitting = true;
    }

    protected virtual void OnDestroy()
    {
      if (_instance == this)
      {
        _instance = null;
      }
    }
  }
}