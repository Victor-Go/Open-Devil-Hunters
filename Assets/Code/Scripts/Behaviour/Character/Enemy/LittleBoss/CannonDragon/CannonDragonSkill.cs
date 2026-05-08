using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.LittleBoss.CannonDragon
{
  public class CannonDragonSkill : EnemySkill
  {
    private Vector2 targetPosition;
    private float speedVariant;

    protected override void Start()
    {
      base.Start();
      
      speedVariant = Random.Range(0.85f, 1.15f);
    }

    public void SetTargetPosition(Vector2 targetPosition)
    {
      this.targetPosition = targetPosition;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer))
      {
        return;
      }

      objectPool.Recycle(ObjectName, gameObject);
    }

    private void PlayExplosionAnimation()
    {
      var explosion = objectPool.GetObject("Status/EnemySkill/CannonDragonExplosion0");
      explosion.transform.SetParent(null);
      explosion.transform.position = transform.position;
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);
      
      speedVariant = Random.Range(0.85f, 1.15f);
    }

    private void FixedUpdate()
    {
      if (paused) return;

      var dt = Time.fixedDeltaTime;
      var direction = (targetPosition - (Vector2)transform.position).normalized;
      var movement = direction * (SkillConfigurations.Speed * speedVariant * dt);
      transform.position = (Vector2)transform.position + movement;
      transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.left, movement), Vector3.forward);

      if (((Vector2)transform.position - targetPosition).sqrMagnitude <= 0.01f)
      {
        PlayExplosionAnimation();
        objectPool.Recycle(ObjectName, gameObject);
      }
    }
  }
}