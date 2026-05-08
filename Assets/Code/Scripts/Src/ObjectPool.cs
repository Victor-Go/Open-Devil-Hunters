using Code.Scripts.Src.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Src
{
  [Serializable]
  public class InvalidObjectNameException : Exception
  {
    public InvalidObjectNameException() : base()
    {
    }

    public InvalidObjectNameException(string objectName)
      : base($"Invalid Object Name: {objectName}. Check if it's been set.")
    {
      Console.WriteLine("Invalid Object Name: {0}. Check if it's been set.", objectName);
    }
  }

  public class ObjectPool : SingletonMonoBehaviour<ObjectPool>
  {
    private readonly ResourceManager resourceManager = ResourceManager.Instance;
    private readonly Dictionary<string, Queue<GameObject>> objects = new();
    private readonly Dictionary<GameObject, IPoolableGameObject> poolableCache = new();
    private readonly Dictionary<GameObject, Coroutine> recycleCoroutines = new();

    private IPoolableGameObject GetPoolable(GameObject gameObject)
    {
      if (!poolableCache.TryGetValue(gameObject, out var poolable))
      {
        poolable = gameObject.GetComponentInChildren<IPoolableGameObject>();
        poolableCache[gameObject] = poolable;
      }
      return poolable;
    }

    public int GetAvailableObjectCount(string objectName)
    {
      if (!objects.TryGetValue(objectName, out var o))
      {
        throw new InvalidObjectNameException(objectName);
      }

      return o.Count;
    }

    public ObjectPool PrepareObjects(string objectName, int quantity)
    {
      if (!objects.ContainsKey(objectName))
      {
        objects.Add(objectName, new());
      }

      var quantityToCreate = quantity - objects[objectName].Count;
      for (var i = 0; i < quantityToCreate; i++)
      {
        GameObject gameObject = Instantiate(resourceManager.GetResource(objectName));
        var poolable = GetPoolable(gameObject);
        if (poolable != null) poolable.ObjectName = objectName;
        gameObject.SetActive(false);
        objects[objectName].Enqueue(gameObject);
      }

      return this;
    }

    public GameObject GetObject(string objectName)
    {
      GameObject _gameObject;
      if (!objects.ContainsKey(objectName) || objects[objectName].Count == 0)
      {
        _gameObject = Instantiate(resourceManager.GetResource(objectName), null, false);

        var controller = GetPoolable(_gameObject);

        if (controller == null)
        {
          throw new Exception(
            $"Unable to get IPoolableGameObject component in prefab {objectName}, check if IPoolableGameObject is attached.");
        }

        controller.ObjectName = objectName;
        return _gameObject;
      }

      _gameObject = objects[objectName].Dequeue();
      GetPoolable(_gameObject)?.ObjectReset(Vector2.zero);
      return _gameObject;
    }

    public GameObject GetObject(string objectName, Vector2 initialPosition)
    {
      GameObject _gameObject;
      if (!objects.ContainsKey(objectName) || objects[objectName].Count == 0)
      {
        _gameObject = Instantiate(resourceManager.GetResource(objectName), null, false);
        _gameObject.transform.position = initialPosition;

        var controller = GetPoolable(_gameObject);

        if (controller == null)
        {
          throw new Exception(
            $"Unable to get IPoolableGameObject component in prefab {objectName}, check if IPoolableGameObject is attached.");
        }

        controller.ObjectName = objectName;
        return _gameObject;
      }

      _gameObject = objects[objectName].Dequeue();
      GetPoolable(_gameObject)?.ObjectReset(initialPosition);
      return _gameObject;
    }

    public void Recycle(GameObject gameObject)
    {
      if (gameObject != null)
      {
        if (recycleCoroutines.TryGetValue(gameObject, out var coroutine))
        {
          if (coroutine != null) StopCoroutine(coroutine);
          recycleCoroutines.Remove(gameObject);
        }

        var poolableGameObject = GetPoolable(gameObject);
        if (poolableGameObject != null)
        {
          var objectName = poolableGameObject.ObjectName;
          Recycle(objectName, gameObject);
        }
        else
        {
          throw new InvalidConstraintException($"GameObject {gameObject.name} is not a poolable object.");
        }
      }
    }

    public void Recycle(GameObject gameObject, float realSeconds)
    {
      if (gameObject == null) return;
      
      if (recycleCoroutines.TryGetValue(gameObject, out var existingCoroutine) && existingCoroutine != null)
      {
        StopCoroutine(existingCoroutine);
      }
      recycleCoroutines[gameObject] = StartCoroutine(RecycleAfter(gameObject, realSeconds));
    }

    private IEnumerator RecycleAfter(GameObject gameObject, float seconds)
    {
      yield return new WaitForSeconds(seconds);
      recycleCoroutines.Remove(gameObject);
      Recycle(gameObject);
    }

    public void Recycle(string objectName, GameObject gameObject)
    {
      if (objectName == null || objectName.Equals(""))
      {
        throw new Exception("Recycling an object which object name equals toQuat null or empty!");
      }

      if (!objects.ContainsKey(objectName))
      {
        objects.Add(objectName, new());

        if (DebugConfigurations.DebugEnabled)
        {
          Debug.LogWarningFormat("Recycled new object {0}, check if it will be reused.", objectName);
        }
      }

      gameObject.transform.SetParent(null);
      gameObject.SetActive(false);
      objects[objectName].Enqueue(gameObject);
    }

    public void Reset()
    {
      foreach (var gameObjectQueue in objects.Values)
      {
        foreach (var gameObject in gameObjectQueue)
        {
          Destroy(gameObject);
        }
      }

      objects.Clear();
    }
  }
}