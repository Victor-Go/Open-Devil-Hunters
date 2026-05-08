using System;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.Werewolf
{
  public enum PortalDirection
  {
    LEFT,
    RIGHT,
  }

  public class Portal : PoolableAndPauseableGameObject
  {
    public float Speed;

    private Transform launchPosition;
    private string prefabToTeleport;
    private bool closing;
    private Vector2 openLocalScale = Vector2.one;
    private Action<GameObject> enemyInstantiatedAction;

    protected override void Awake()
    {
      base.Awake();

      launchPosition = transform.Find("LaunchPosition");
    }

    protected override void Start()
    {
      base.Start();

      transform.localScale = Vector2.zero;
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      transform.localScale = Vector2.zero;
      closing = false;
    }

    public Portal SetEnemyInstantiatedAction(Action<GameObject> action)
    {
      enemyInstantiatedAction = action;
      return this;
    }

    public Portal SetPrefabName(string prefabName)
    {
      prefabToTeleport = prefabName;
      return this;
    }

    public Portal SetDirection(PortalDirection direction)
    {
      var dir = direction == PortalDirection.LEFT ? 1 : -1;
      var ls = transform.localScale;
      ls.x *= dir;
      transform.localScale = ls;

      openLocalScale = new Vector2(dir, 1);

      return this;
    }

    private void FixedUpdate()
    {
      if (paused) return;

      if (!closing)
      {
        transform.localScale = Vector2.Lerp(transform.localScale, openLocalScale, Time.fixedDeltaTime * Speed);

        if (((Vector2)transform.localScale - openLocalScale).sqrMagnitude <= 0.005f)
        {
          var enemy = objectPool.GetObject(prefabToTeleport);
          enemy.transform.SetParent(null);
          enemy.transform.position = launchPosition.position;

          enemyInstantiatedAction(enemy);

          closing = true;
        }
      }
      else
      {
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.fixedDeltaTime * Speed * 1.5f);

        if (((Vector2)transform.localScale - Vector2.zero).sqrMagnitude <= 0.01f)
        {
          objectPool.Recycle("Portal", gameObject);
        }
      }
    }
  }
}