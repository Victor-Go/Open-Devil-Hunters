using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Player.Base
{
  public class GumdamSkill : CircularTracingBullet
  {
    public float ExplosionRange = 0.5f;

    private static readonly int EnemyHurtLayerMask = LayerMask.GetMask("EnemyHurt");
    private static readonly Collider2D[] overlapCollidersBuffer = new Collider2D[64];

    public void Explode()
    {
      var explosion = objectPool.GetObject("Effect/SkillExplosion");
      explosion.transform.SetParent(null);
      explosion.transform.position = transform.position;

      var count = Physics2D.OverlapCircleNonAlloc(transform.position, ExplosionRange, overlapCollidersBuffer, EnemyHurtLayerMask);
      for (int i = 0; i < count; i++)
      {
        overlapCollidersBuffer[i].GetComponent<EnemyHurt>().TriggerEnemyHurt(PlayerNumber, this);
      }
    }

    protected override void OnTriggerStay2D(Collider2D other)
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
          Explode();
        }
      }
    }

    protected override void FixedUpdate()
    {
      if (paused) return;

      var dt = Time.fixedDeltaTime;
      var distance = SkillConfigurations.Speed * dt;
      var vectorToTarget = (targetPosition - (Vector2)transform.position).normalized;

      distanceElapsed += distance;

      var circularConfigs = (CircularSkillConfigurations)SkillConfigurations;
      currentAngleIncrementPerSecond += dt * circularConfigs.AnglePerSecondIncrementPerSecond;

      var anglePerSecond = circularConfigs.AnglePerSecond;
      var dMaxAngle = dt * (anglePerSecond + currentAngleIncrementPerSecond);
      var angle = Vector2.SignedAngle(vectorToTarget, lastDirection);
      if (Mathf.Abs(angle) > dMaxAngle)
      {
        angle = angle < 0 ? -dMaxAngle : dMaxAngle;
      }

      Vector2 direction = Quaternion.AngleAxis(angle, Vector3.back) * lastDirection;
      lastDirection = direction.normalized;
      var movement = direction * circularConfigs.Speed * dt;
      rb.MovePosition(movement + rb.position);

      var eulerZ = Vector2.SignedAngle(Vector2.left, movement);
      transform.rotation = Quaternion.Euler(0, 0, eulerZ);

      if (RecycleWhenArrivesPosition && ((Vector2)transform.position - targetPosition).sqrMagnitude <= 0.01f)
      {
        objectPool.Recycle(ObjectName, gameObject);
        Explode();
        return;
      }

      if (distanceElapsed >= circularConfigs.Range)
      {
        objectPool.Recycle(ObjectName, gameObject);
        Explode();
        return;
      }
    }
  }
}