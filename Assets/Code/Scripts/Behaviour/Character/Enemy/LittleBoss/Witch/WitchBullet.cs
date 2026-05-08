using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.LittleBoss.Witch
{
  public class WitchBulletConfigurations : BaseEnemySkillConfigurations
  {
    public float AnglePerSecond { get; set; }
    public float LifeTime { get; set; }
  }

  public class WitchBullet : EnemySkill
  {
    private float actualLifeTime;
    private Vector2 targetPosition;
    private Vector2 lastDirection;
    private float lifeTime;
    private bool disappearing;
    private WitchBulletConfigurations cachedWitchBulletConfigurations;

    protected override void Awake()
    {
      base.Awake();
      SetTargetPosition();
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);
      transform.position = initialPosition;
      SetTargetPosition();
      lifeTime = 0;
      disappearing = false;

      actualLifeTime = cachedWitchBulletConfigurations.LifeTime * Random.Range(0.9f, 1.1f);
    }

    public override EnemySkill SetConfigurations(BaseEnemySkillConfigurations configs)
    {
      base.SetConfigurations(configs);

      cachedWitchBulletConfigurations = (WitchBulletConfigurations)configs;
      actualLifeTime = cachedWitchBulletConfigurations.LifeTime * Random.Range(0.66f, 1.33f);
      return this;
    }

    private void SetTargetPosition()
    {
      var playerPositionState = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore);
      var playerPositions = playerPositionState.PlayerPositions;

      var nearest = float.MaxValue;
      targetPosition = playerPositions[0];
      for (var i = 1; i < playerPositionState.PlayerAlive.Length; i++)
      {
        if (!playerPositionState.PlayerAlive[i])
        {
          continue;
        }

        var distance = ((Vector2)transform.position - playerPositions[i]).sqrMagnitude;
        if (distance <= nearest + 4f && Random.Range(0, 1f) > 0.5f)
        {
          nearest = distance;
          targetPosition = playerPositions[i];
        }
      }
    }

    public void SetAngle(float angle)
    {
      lastDirection = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer))
      {
        return;
      }

      objectPool.Recycle(ObjectName, gameObject);
    }

    private void Update()
    {
      if (paused)
      {
        return;
      }

      if (!disappearing)
      {
        lifeTime += Time.deltaTime;
        if (lifeTime >= actualLifeTime)
        {
          disappearing = true;
        }
      }
      else
      {
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime);
        if (transform.localScale.x <= 0.1f)
        {
          objectPool.Recycle(ObjectName, gameObject);
        }
      }
    }

    private void FixedUpdate()
    {
      if (paused)
      {
        return;
      }

      var dt = Time.fixedDeltaTime;
      var dMaxAngle = dt * cachedWitchBulletConfigurations.AnglePerSecond;

      var angle = Vector2.SignedAngle(targetPosition - (Vector2)transform.position, lastDirection);
      if (Mathf.Abs(angle) > dMaxAngle)
      {
        angle = angle < 0 ? -dMaxAngle : dMaxAngle;
      }

      var direction = Quaternion.AngleAxis(angle, Vector3.back) * lastDirection;
      lastDirection = direction;

      Vector2 movement = direction * cachedWitchBulletConfigurations.Speed * dt;
      rb.MovePosition(movement + rb.position);
    }
  }
}