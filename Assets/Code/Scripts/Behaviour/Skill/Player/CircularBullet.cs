using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Player
{
  public class CircularBullet : Bullet
  {
    public bool RecycleWhenArrivesPosition;

    protected Vector2 lastDirection;
    protected float distanceElapsed;
    protected float currentAngleIncrementPerSecond;

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      distanceElapsed = 0;
      currentAngleIncrementPerSecond = 0;
    }

    public void SetInitialDirection(Vector2 direction)
    {
      lastDirection = direction.normalized;
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
        return;
      }

      if (distanceElapsed >= circularConfigs.Range)
      {
        objectPool.Recycle(ObjectName, gameObject);
        return;
      }
    }
  }
}