using System.Linq;
using Code.Scripts.Behaviour.Animation;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.Animation;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Bullets
{
  public class ArtilleryShell : Bullet
  {
    public override void SetTargetRelativeDirection(Vector2 relativeDirection)
    {
      base.SetTargetRelativeDirection(relativeDirection);

      targetPosition = (Vector2)transform.position +
                       relativeDirection.normalized * ((BasicSkillConfigurations)SkillConfigurations).Range;
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer))
      {
        return;
      }

      Explode();

      var skillHitContext = new BasicSkillHitContext()
      {
      };
      foreach (var interceptor in SkillConfigurations.SkillHitInterceptors)
      {
        interceptor(skillHitContext);
      }
    }

    private static readonly Collider2D[] explodeCollidersBuffer = new Collider2D[32];

    protected void Explode()
    {
      var count = Physics2D.OverlapCircleNonAlloc(transform.position, 1f, explodeCollidersBuffer);

      for (int i = 0; i < count; i++)
      {
        var collider = explodeCollidersBuffer[i];
        if (!TriggerGroup.CanInteract(gameObject.layer, collider.gameObject.layer))
        {
          continue;
        }

        var enemyHurt = collider.gameObject.GetComponent<EnemyHurt>();
        if (enemyHurt != null)
        {
          enemyHurt.TriggerEnemyHurt(PlayerNumber, this, transform.position);
        }
      }

      var explosion = objectPool.GetObject("Status/PlayerSkill/ArtilleryShellExplosion0");
      explosion.transform.SetParent(null);
      explosion.transform.position = transform.position;
      explosion.GetComponent<SimpleAnimation>()
        .SetRecycleAnimationAfterFinished(true)
        .CanMoveWhenTimeStopped = true;

      RecycleBullet();
    }

    protected override void RecycleBullet()
    {
      objectPool.Recycle(ObjectName, gameObject);
    }

    protected override void FixedUpdate()
    {
      if (paused)
      {
        return;
      }

      var dt = Time.fixedDeltaTime;
      rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, dt * SkillConfigurations.Speed));

      if ((rb.position - targetPosition).sqrMagnitude <= 0.1f)
      {
        Explode();
      }
    }
  }
}