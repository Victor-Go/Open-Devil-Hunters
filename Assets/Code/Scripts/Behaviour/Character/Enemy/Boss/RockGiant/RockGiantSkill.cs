using System.Linq;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.RockGiant
{
  public class RockGiantSkill : EnemySkill
  {
    private bool exploded;
    private Vector2 targetPosition;
    private Vector2 vectorToTarget;

    private AudioWrapper _audioWrapper;

    private static readonly int HurtLayerMask = LayerMask.GetMask("EnemyHurt", "PlayerHurt");

    protected override void Awake()
    {
      base.Awake();

      _audioWrapper = new AudioWrapper(new[]
        {
          "Audio/Sound/Enemy/Skill/rock-giant_skill_0",
          "Audio/Sound/Enemy/Skill/rock-giant_skill_1",
        },
        null
      );

      Init();
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);
      Init();
    }

    private void Init()
    {
      animator.SetTrigger("Reset");
      transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
      exploded = false;
    }

    public void SetTargetPosition(Vector2 targetPosition)
    {
      this.targetPosition = targetPosition;
    }

    private void Explode()
    {
      exploded = true;
      _audioWrapper.PlayRandomly();
      animator.ResetTrigger("Reset");
      animator.SetTrigger("Explode");
    }

    public void OnExplosionAnimationFinished()
    {
      animator.ResetTrigger("Explode");
      animator.SetTrigger("Reset");

      DoExplode();

      objectPool.Recycle(ObjectName, gameObject);
    }

    private static readonly Collider2D[] explodeCollidersBuffer = new Collider2D[32];

    private void DoExplode()
    {
      var count = Physics2D.OverlapCircleNonAlloc(transform.position, 0.5f, explodeCollidersBuffer, HurtLayerMask);

      for (int i = 0; i < count; i++)
      {
        var collider = explodeCollidersBuffer[i];
        var enemyHurt = collider.gameObject.GetComponent<EnemyHurt>();
        if (enemyHurt != null)
        {
          enemyHurt.TriggerEnemyStun(0.5f);
        }

        var playerHurt = collider.gameObject.GetComponent<PlayerHurtHandling>();
        if (playerHurt != null)
        {
          playerHurt.TriggerThunderHurtManually(SkillConfigurations.HitPoint, 1f);
        }
      }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer) || exploded)
      {
        return;
      }

      Explode();
    }

    private void FixedUpdate()
    {
      if (paused || exploded) return;

      var dt = Time.fixedDeltaTime;
      vectorToTarget = (targetPosition - rb.position).normalized;
      var movement = vectorToTarget * dt * SkillConfigurations.Speed;

      rb.MovePosition(rb.position + movement);

      if ((rb.position - targetPosition).sqrMagnitude <= 0.01f)
      {
        Explode();
      }
    }
  }
}