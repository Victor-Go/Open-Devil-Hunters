using System;
using UnityEngine;

namespace Code.Scripts.Src.Utils
{
  public class Singleton<T> where T : class
  {
    private static T _instance;
    private static readonly object _lock = new object();

    protected Singleton()
    {
    }

    public static T Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (_lock)
          {
            _instance ??= Activator.CreateInstance(typeof(T), true) as T;
          }
        }

        return _instance;
      }
    }

    public static void Dispose()
    {
      lock (_lock)
      {
        _instance = null;
      }
    }
  }
}