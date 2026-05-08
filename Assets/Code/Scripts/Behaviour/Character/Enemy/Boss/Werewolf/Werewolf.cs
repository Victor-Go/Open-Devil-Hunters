using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.Character.Enemy;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.Werewolf
{


  public class Werewolf : PoolableBigBoss
  {
    public BoxCollider2D clawCollider;

    private readonly List<string> attackSoundNames = new()
    {
      "Audio/Sound/Enemy/Boss/werewolf-attack_0"
    };

    private const float invokeDistance = 4; // Greater than this distance to invoke
    private const float chaseDistance = 1; // Less than this distance to attack
    private const float restAfterAttackTimeout = 2;
    private const float restAfterInvokeTimeout = 1;
    private const float restAfterFailedToChaseTimeout = 2;
    private const float leaveDistance = 9;
    private const float leaveRestTimeout = 3;
    private const float maxChasingDistance = 15;
    private const float attackBiasY = 0.1f;
    private const float attackDistance = 2f;
    private const float acceleration = 1.05f;

    private WerewolfIdleState idleState;
    private ChaseState chaseState;
    private LeaveState leaveState;
    private InvokeState invokeState;
    private AttackState attackState;

    private string transitStatusTimeoutScheduleId;
    private Vector2 leavePosition;
    private float currentApproachDistance;

    private float symmetryAxisOffset;
    private float symmetryInitColliderOffset;
    private float symmetryClawInitColliderOffset;
    private CircleCollider2D hitCollider;
    private CircleCollider2D hurtCollider;
    private CircleCollider2D movementCollider;

    private readonly List<AudioClip> attackClips = new();

    private int numberOfInvokedWolves;

    protected override void Awake()
    {
      base.Awake();

      animator = GetComponentInChildren<Animator>();

      hitCollider = transform.Find("Hit").GetComponent<CircleCollider2D>();
      hurtCollider = transform.Find("Hurt").GetComponent<CircleCollider2D>();
      movementCollider = GetComponent<CircleCollider2D>();

      var symmetryX = transform.Find("Symmetry").localPosition.x;
      symmetryAxisOffset = animatorTransform.localPosition.x - symmetryX;
      symmetryInitColliderOffset = movementCollider.offset.x - symmetryX;
      symmetryClawInitColliderOffset = clawCollider.offset.x - symmetryX;

      SetAnimatorHorizontalPosition(AnimatorDirection.Left);
      foreach (var name in attackSoundNames)
      {
        attackClips.Add(resourceManager.GetResource(name));
      }

      idleState = new WerewolfIdleState(this);
      chaseState = new ChaseState(this);
      leaveState = new LeaveState(this);
      invokeState = new InvokeState(this);
      attackState = new AttackState(this);
    }

    protected override void Start()
    {
      base.Start();

      clawCollider.enabled = false;

      SelectOnePlayerToChase();

      Chase();
    }

    private void SetAnimatorHorizontalPosition(AnimatorDirection animatorDirection)
    {
      var animPos = animatorTransform.localPosition;
      var collPos = movementCollider.offset;
      var hitPos = hitCollider.offset;
      var hurtPos = hurtCollider.offset;
      var clawPos = clawCollider.offset;

      switch (animatorDirection)
      {
        case AnimatorDirection.Left:
          animatorTransform.localPosition = new Vector2(symmetryAxisOffset, animPos.y);
          movementCollider.offset = new Vector2(symmetryInitColliderOffset, collPos.y);
          hitCollider.offset = new Vector2(symmetryInitColliderOffset, hitPos.y);
          hurtCollider.offset = new Vector2(symmetryInitColliderOffset, hurtPos.y);
          clawCollider.offset = new Vector2(symmetryClawInitColliderOffset, clawPos.y);
          break;
        case AnimatorDirection.Right:
          animatorTransform.localPosition = new Vector2(-symmetryAxisOffset, animPos.y);
          movementCollider.offset = new Vector2(-symmetryInitColliderOffset, collPos.y);
          hitCollider.offset = new Vector2(-symmetryInitColliderOffset, hitPos.y);
          hurtCollider.offset = new Vector2(-symmetryInitColliderOffset, hurtPos.y);
          clawCollider.offset = new Vector2(-symmetryClawInitColliderOffset, clawPos.y);
          break;
      }
    }

    protected override void SetAnimatorDirection(Vector2 direction)
    {
      base.SetAnimatorDirection(direction);

      if (direction.x > GeneralConfigurations.AnimationDirectionSettingThreshold)
      {
        SetAnimatorHorizontalPosition(AnimatorDirection.Right);
      }
      else if (direction.x < -GeneralConfigurations.AnimationDirectionSettingThreshold)
      {
        SetAnimatorHorizontalPosition(AnimatorDirection.Left);
      }
    }

    private void Idle()
    {
      StateMachine.ChangeState(idleState);
    }

    private void Chase()
    {
      scheduling.ClearSchedule(transitStatusTimeoutScheduleId);
      StateMachine.ChangeState(chaseState);
    }

    private float sqrChasedDistance;

    private void DoChase(float dt)
    {
      var currentPosition = (Vector2)transform.position;
      var selectedPlayerPosition = playerPositions[selectedPlayerNumber];
      var playerLeft = selectedPlayerPosition + Vector2.left * chaseDistance;
      var playerRight = selectedPlayerPosition + Vector2.right * chaseDistance;
      var targetPosition = (playerLeft - currentPosition).sqrMagnitude < (playerRight - currentPosition).sqrMagnitude
        ? playerLeft
        : playerRight;
      var vectorToTarget = targetPosition - currentPosition;

      if (Mathf.Abs(vectorToTarget.y) <= attackBiasY && vectorToTarget.sqrMagnitude <= attackDistance * attackDistance)
      {
        sqrChasedDistance = 0;
        var random = Random.Range(0, 100);
        if (random < 66)
        {
          Attack();
        }
        else
        {
          Invoke();
        }

        return;
      }

      if (sqrChasedDistance >= maxChasingDistance * maxChasingDistance)
      {
        sqrChasedDistance = 0;

        if (vectorToTarget.sqrMagnitude <= invokeDistance * invokeDistance)
        {
          Invoke();
        }
        else
        {
          scheduling.ClearSchedule(transitStatusTimeoutScheduleId);
          transitStatusTimeoutScheduleId = scheduling.SetTimeout(() => Leave(), restAfterFailedToChaseTimeout);
        }

        return;
      }

      var updatedSpeed = updatedEnemyConfigurations.Speed;
      var speed = Mathf.Max(updatedSpeed, updatedSpeed * 2 * (1 - Mathf.Pow(acceleration, -sqrChasedDistance)));
      speed = Mathf.Clamp(speed, 1, 3f);

      var sqrDistance = vectorToTarget.sqrMagnitude;
      var movement = sqrDistance > vectorToTarget.sqrMagnitude
        ? vectorToTarget
        : vectorToTarget.normalized * (dt * speed);

      sqrChasedDistance += movement.sqrMagnitude;
      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(movement);
      sqrChasedDistance += movement.sqrMagnitude;
    }

    private void Leave()
    {
      scheduling.ClearSchedule(transitStatusTimeoutScheduleId);

      var sum = Vector2.zero;
      for (var i = 0; i < playerAlive.Length; i++)
      {
        if (!playerAlive[i])
        {
          continue;
        }

        sum += playerPositions[i];
      }

      var centerPosition = sum / playerAlive.Count(p => p);

      leavePosition =
        (Vector2)(Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector2.right * leaveDistance) +
        centerPosition;

      StateMachine.ChangeState(leaveState);
    }

    private void DoLeave(float dt)
    {
      var movement = (leavePosition - (Vector2)transform.position).normalized * (dt * updatedEnemyConfigurations.Speed);

      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(movement);

      var sqrDistance = ((Vector2)transform.position - leavePosition).sqrMagnitude;
      if (sqrDistance <= 0.1f)
      {
        Idle();

        scheduling.ClearSchedule(transitStatusTimeoutScheduleId);
        transitStatusTimeoutScheduleId = scheduling.SetTimeout(() => Chase(), leaveRestTimeout);
      }
    }

    private void Invoke()
    {
      scheduling.ClearSchedule(transitStatusTimeoutScheduleId);
      StateMachine.ChangeState(invokeState);
    }

    public void OnInvokeAnimationFinished()
    {
      DoInvoke();
    }

    private void DoInvoke()
    {
      var wolvesToInvoke = Random.Range(5, 10);
      for (var i = 0; i < wolvesToInvoke; i++)
      {
        var portal = objectPool.GetObject("Enemy/Boss/Werewolf/Portal");
        portal.transform.SetParent(null);

        Vector2 portalPosition = transform.position + Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) *
          Vector2.right * Random.Range(1, 4f);
        portal.transform.position = portalPosition;

        var nearestPlayerPosition = GeneralUtils.GetNearestPosition(portalPosition, playerPositions);
        var vectorToPlayer = (nearestPlayerPosition - portalPosition).normalized;

        portal.GetComponent<Portal>()
          .SetPrefabName("Enemy/Boss/Werewolf/Wolf")
          .SetDirection(vectorToPlayer.x < 0 ? PortalDirection.LEFT : PortalDirection.RIGHT)
          .SetEnemyInstantiatedAction(wolf =>
          {
            var wolfController = wolf.GetComponent<Wolf>();
            wolfController.SetInitTargetPosition(portalPosition +
                                                 new Vector2(1 / Mathf.Abs(vectorToPlayer.x) * vectorToPlayer.x, 0));
            wolfController.SetEnemyBaseStrength(totalStrength);
          });
      }

      Idle();

      scheduling.ClearSchedule(transitStatusTimeoutScheduleId);
      transitStatusTimeoutScheduleId = scheduling.SetTimeout(() => Leave(), restAfterInvokeTimeout);
    }

    private void Attack()
    {
      StateMachine.ChangeState(attackState);
      AudioWrapper.PlayClip(attackClips[Random.Range(0, attackClips.Count)], transform.position);
    }

    public void OnAttackAnimationStart()
    {
      clawCollider.enabled = true;
    }

    public void OnAttackAnimationFinished()
    {
      DoAttack();
    }

    private void DoAttack()
    {
      animator.ResetTrigger("Attack");

      clawCollider.enabled = false;

      Idle();

      scheduling.ClearSchedule(transitStatusTimeoutScheduleId);
      transitStatusTimeoutScheduleId = scheduling.SetTimeout(() => Leave(), restAfterAttackTimeout);
    }

    protected override void FixedUpdate()
    {
      if (paused)
      {
        rb.velocity = Vector2.zero;
        return;
      }

      if (stopHandlingMovement)
      {
        return;
      }

      // BaseBossController handles state execution
    }

    private class WerewolfIdleState : BossIdleState
    {
      public WerewolfIdleState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        boss.ResetAnimTrigger("Invoke");
      }
    }

    private class ChaseState : BossWalkState
    {
      public ChaseState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        boss.ResetAnimTrigger("Invoke");
      }
      public override void Execute(float dt) { ((Werewolf)boss).DoChase(dt); }
    }

    private class LeaveState : BossWalkState
    {
      public LeaveState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        boss.ResetAnimTrigger("Invoke");
      }
      public override void Execute(float dt) { ((Werewolf)boss).DoLeave(dt); }
    }

    private class InvokeState : BaseState<BaseBossController>
    {
      public InvokeState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        boss.ResetAnimTrigger("Idle");
        boss.ResetAnimTrigger("Walk");
        boss.ResetAnimTrigger("Attack");
        boss.SetAnimTrigger("Invoke");
      }
    }

    private class AttackState : BossAttackState
    {
      public AttackState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        boss.ResetAnimTrigger("Invoke");
      }
    }
    }
  }
}