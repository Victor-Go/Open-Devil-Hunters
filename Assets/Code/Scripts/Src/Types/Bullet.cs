using System.Collections.Generic;
using Code.Scripts.Behaviour.Animation;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.Animation;
using Code.Scripts.Src.Configurations;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scripts.Src.Types
{
  public class Bullet : PlayerBasicSkill
  {
    protected float rangeElapsed;
    protected Rigidbody2D rb;
    protected TrailRenderer trailRenderer;
    protected ParticleSystem _particleSystem;
    protected readonly List<string> touchedEnemies = new();
    protected int penetrated;

    protected override void Awake()
    {
      base.Awake();
      rb = GetComponent<Rigidbody2D>();
      trailRenderer = GetComponentInChildren<TrailRenderer>();
      storeManager = StoreManager.Instance;
      _particleSystem = GetComponent<ParticleSystem>();
    }

    protected override void Start()
    {
      base.Start();

      storeManager.Subscribe(StoreNames.GameStateStore, this);
      var gameState = storeManager.GetState<GameState>(StoreNames.GameStateStore);
      HandleGameStateChanged(gameState);
    }

    protected override void HandleGameStateChanged(GameState state)
    {
      base.HandleGameStateChanged(state);

      if (_particleSystem != null)
      {
        if (paused)
        {
          _particleSystem.Pause();
        }
        else
        {
          _particleSystem.Play();
        }
      }
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer))
      {
        return;
      }

      var enemyController = other.gameObject.GetComponent<EnemyHurt>();
      if (!touchedEnemies.Contains(enemyController.UniqueId))
      {
        touchedEnemies.Add(enemyController.UniqueId);
        penetrated++;

        var skillHitContext = new BasicSkillHitContext()
        {
          ObjectPool = objectPool,
          CurrentPosition = transform.position,
          SkillConfigurations = (BasicSkillConfigurations)SkillConfigurations,
        };
        foreach (var interceptor in SkillConfigurations.SkillHitInterceptors)
        {
          interceptor(skillHitContext);
        }

        if (penetrated >= ((BasicSkillConfigurations)SkillConfigurations).Penetration)
        {
          RecycleBullet();
        }
      }
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      var gameState = storeManager.GetState<GameState>(StoreNames.GameStateStore);
      HandleGameStateChanged(gameState);

      rangeElapsed = penetrated = 0;
      if (trailRenderer != null)
      {
        trailRenderer.Clear();
      }

      touchedEnemies.Clear();
    }

    public override void SetTargetRelativeDirection(Vector2 relativeDirection)
    {
      base.SetTargetRelativeDirection(relativeDirection);
      if (trailRenderer != null)
      {
        trailRenderer.Clear();
      }
    }

    protected virtual void RecycleBullet()
    {
      objectPool.Recycle(ObjectName, gameObject);

      var hurtType = SkillConfigurations.SkillHurt.HurtType;
      switch (hurtType)
      {
        case HurtTypes.PHYSICAL:
          PlaceDisappearEffect("Status/SkillDisappear/PhysicalDisappear0");
          break;
        case HurtTypes.MAGIC_ICE:
          PlaceDisappearEffect("Status/SkillDisappear/IceDisappear0");
          break;
        case HurtTypes.MAGIC_FIRE:
          PlaceDisappearEffect("Status/SkillDisappear/FireDisappear0");
          break;
        case HurtTypes.MAGIC_THUNDER:
          PlaceDisappearEffect("Status/SkillDisappear/ThunderDisappear0");
          break;
        case HurtTypes.POISON:
          PlaceDisappearEffect("Status/SkillDisappear/PoisonDisappear0");
          break;
      }
    }

    protected void PlaceDisappearEffect(string effectName)
    {
      var gameObject = objectPool.GetObject(effectName);
      gameObject.transform.SetParent(null);
      gameObject.transform.position = transform.position;
      gameObject.GetComponent<SimpleAnimation>().SetRecycleAnimationAfterFinished(true);
    }

    protected virtual void FixedUpdate()
    {
      if (paused)
      {
        return;
      }

      var dt = Time.fixedDeltaTime;
      var distance = Mathf.Clamp(SkillConfigurations.Speed * dt, 0, GeneralConfigurations.BulletMaximumSpeed * dt);
      var movement = distance * relativeDirection.normalized;

      rangeElapsed += distance;

      if (rangeElapsed >= ((BasicSkillConfigurations)SkillConfigurations).Range)
      {
        RecycleBullet();
        return;
      }

      rb.MovePosition(rb.position + movement);
    }
  }
}