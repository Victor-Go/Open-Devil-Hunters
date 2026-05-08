using System.Collections.Generic;
using Code.Scripts.Behaviour.Character.Enemy;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.IceTiger
{

  public class IceTiger : PoolableBigBoss
  {
    private float chaseRange { get; } = 3;
    private float bumpDistance { get; } = 6; // bumpDistance should be bigger than the max value of chase range.
    private const float waitForAttackTimeout = 1.5f;
    private const float waitForBumpTimeout = 2.5f;
    private float restAfterAttackTimeout { get; } = 2;
    private float restAfterBumpTimeout { get; } = 3;
    private float maxAngularVelocity { get; } = 2;
    private int numberOfSkills { get; } = 3; // This value must be odd to points to player.
    private float skillAngle { get; } = 30;

    private static readonly float[] nextTimeouts = new[] { waitForBumpTimeout, waitForAttackTimeout };
    private long statusTransitionScheduleId;
    private float currentChaseDistance;
    private Vector2 lastBumpDirection;
    private float bumpedDistance;
    private readonly List<GameObject> aims = new();
    private Transform launchTransform;
    private Vector2 targetPosition;
    private TrailRenderer trailRenderer;

    private float initLaunchTransformX;

    private BaseState<BaseBossController> pendingNextState;
    private BaseState<BaseBossController>[] nextStates;

    private IdleState idleState;
    private ChaseState chaseState;
    private BumpState bumpState;
    private AttackState attackState;

    private System.Action handlePendingStatusTransitionAction;
    private System.Action chaseAction;

    protected override void Awake()
    {
      base.Awake();

      var animatorTransform = transform.Find("Animator");
      animator = animatorTransform.GetComponent<Animator>();
      trailRenderer = animatorTransform.GetComponentInChildren<TrailRenderer>();
      launchTransform = transform.Find("LaunchPosition");

      idleState = new IdleState(this);
      chaseState = new ChaseState(this);
      bumpState = new BumpState(this);
      attackState = new AttackState(this);
      nextStates = new BaseState<BaseBossController>[] { bumpState, attackState };

      handlePendingStatusTransitionAction = HandlePendingStatusTransition;
      chaseAction = Chase;
    }

    protected override void Start()
    {
      base.Start();

      initLaunchTransformX = launchTransform.localPosition.x;
      trailRenderer.emitting = false;

      SelectOnePlayerToChase();
      Chase();

      for (int i = 0; i < numberOfSkills; i++)
      {
        GameObject aim = objectPool.GetObject("Enemy/EnemyStripeAim");
        aim.transform.SetParent(null);
        aim.GetComponent<EnemyLineAim>().SetLength(2);
        aim.SetActive(false);

        aims.Add(aim);
      }
    }

    protected override void SetAnimatorDirection(Vector2 direction)
    {
      base.SetAnimatorDirection(direction);

      if (direction.x > GeneralConfigurations.AnimationDirectionSettingThreshold)
      {
        var lpos = launchTransform.localPosition;
        lpos.x = -initLaunchTransformX;
        launchTransform.localPosition = lpos;
      }
      else if (direction.x < -GeneralConfigurations.AnimationDirectionSettingThreshold)
      {
        var lpos = launchTransform.localPosition;
        lpos.x = initLaunchTransformX;
        launchTransform.localPosition = lpos;
      }
    }

    private void Idle()
    {
      StateMachine.ChangeState(idleState);
    }

    private void Chase()
    {
      StateMachine.ChangeState(chaseState);
    }

    private void HandlePendingStatusTransition()
    {
      StateMachine.ChangeState(pendingNextState);
    }

    private void Bump()
    {
      StateMachine.ChangeState(bumpState);
    }

    private void Attack()
    {
      StateMachine.ChangeState(attackState);
    }

    private class IdleState : BossIdleState
    {
      public IdleState(BaseBossController boss) : base(boss) {}
      public override void Enter() 
      {
        base.Enter();
        boss.ResetAnimTrigger("Chase");
        boss.ResetAnimTrigger("Bump");
        if (boss.GetComponent<Rigidbody2D>() is Rigidbody2D rb) rb.bodyType = RigidbodyType2D.Dynamic;
      }
    }

    private class ChaseState : BaseState<BaseBossController>
    {
      public ChaseState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        boss.ResetAnimTrigger("Idle");
        boss.ResetAnimTrigger("Bump");
        boss.ResetAnimTrigger("Attack");
        boss.SetAnimTrigger("Chase");
        var bossCast = (IceTiger)boss;
        bossCast.currentChaseDistance = bossCast.chaseRange * Random.Range(1, 1.5f);
        bossCast.rb.bodyType = RigidbodyType2D.Dynamic;
      }
      public override void Execute(float dt)
      {
        var bossCast = (IceTiger)boss;
        var playerPosition = bossCast.playerPositions[bossCast.selectedPlayerNumber];
        var vectorToTarget = playerPosition - (Vector2)bossCast.transform.position;
        var movement = vectorToTarget.normalized * (dt * bossCast.updatedEnemyConfigurations.Speed);

        bossCast.rb.MovePosition(bossCast.rb.position + movement);
        bossCast.SetAnimatorDirection(movement);

        var sqrDistance = ((Vector2)bossCast.transform.position - playerPosition).sqrMagnitude;
        if (sqrDistance <= Mathf.Pow(bossCast.currentChaseDistance, 2))
        {
          bossCast.SetAnimatorDirection(vectorToTarget);
          bossCast.Idle();

          int index = Random.Range(0, 2);
          bossCast.pendingNextState = bossCast.nextStates[index];
          float timeout = nextTimeouts[index];

          if (bossCast.pendingNextState == bossCast.attackState)
          {
            bossCast.targetPosition = playerPosition;
            bossCast.SetAnimatorDirection(playerPosition - (Vector2)bossCast.transform.position);
            bossCast.rb.bodyType = RigidbodyType2D.Kinematic;
            bossCast.rb.velocity = Vector2.zero;
            bossCast.PlaceWarningAim();
          }

          bossCast.scheduling.ClearSchedule(bossCast.statusTransitionScheduleId);
          bossCast.statusTransitionScheduleId = bossCast.scheduling.SetTimeout(bossCast.handlePendingStatusTransitionAction, timeout);
        }
      }
    }

    private class BumpState : BaseState<BaseBossController>
    {
      public BumpState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        boss.ResetAnimTrigger("Idle");
        boss.ResetAnimTrigger("Chase");
        boss.ResetAnimTrigger("Attack");
        boss.SetAnimTrigger("Bump");

        var bossCast = (IceTiger)boss;
        bossCast.bumpedDistance = 0;

        var playerPosition = bossCast.playerPositions[bossCast.selectedPlayerNumber];
        var vectorToTarget = playerPosition - (Vector2)bossCast.transform.position;
        bossCast.lastBumpDirection = vectorToTarget;

        bossCast.rb.bodyType = RigidbodyType2D.Dynamic;
        bossCast.trailRenderer.emitting = true;
      }
      public override void Execute(float dt)
      {
        var bossCast = (IceTiger)boss;
        var playerPosition = bossCast.playerPositions[bossCast.selectedPlayerNumber];
        var vectorToTarget = playerPosition - (Vector2)bossCast.transform.position;

        var dMaxAngle = dt * bossCast.maxAngularVelocity;
        float angle = Vector2.SignedAngle(vectorToTarget, bossCast.lastBumpDirection);
        if (Mathf.Abs(angle) > dMaxAngle)
        {
          angle = angle < 0 ? -dMaxAngle : dMaxAngle;
        }

        Vector2 direction = Quaternion.AngleAxis(angle, Vector3.back) * bossCast.lastBumpDirection;
        var movement = direction.normalized * (dt * bossCast.updatedEnemyConfigurations.Speed * 1.5f);

        bossCast.lastBumpDirection = direction;
        bossCast.bumpedDistance += movement.magnitude;

        bossCast.rb.MovePosition(bossCast.rb.position + movement);
        bossCast.SetAnimatorDirection(movement);

        if (bossCast.bumpedDistance > bossCast.bumpDistance)
        {
          bossCast.trailRenderer.emitting = false;
          bossCast.Idle();

          bossCast.scheduling.ClearSchedule(bossCast.statusTransitionScheduleId);
          bossCast.statusTransitionScheduleId = bossCast.scheduling.SetTimeout(bossCast.chaseAction, bossCast.restAfterBumpTimeout);
        }
      }
    }

    private class AttackState : BossAttackState
    {
      public AttackState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        boss.ResetAnimTrigger("Chase");
        boss.ResetAnimTrigger("Bump");
      }
    }

    public void OnAttackAnimationFinished()
    {
      DoAttack();
    }

    private void PlaceWarningAim()
    {
      var vectorToTarget = (targetPosition - (Vector2)launchTransform.position).normalized;
      var directAngle = Vector2.SignedAngle(Vector2.right, vectorToTarget);
      var startAngle = directAngle - skillAngle * ((numberOfSkills - 1) / 2);

      for (int i = 0; i < aims.Count; i++)
      {
        var aim = aims[i];
        var pos = launchTransform.position;

        aim.transform.position = pos;
        aim.GetComponent<EnemyLineAim>().SetAngle(startAngle + i * skillAngle);
        aim.SetActive(true);
      }
    }

    private void SetWarningAimActive(bool active = true)
    {
      foreach (var aim in aims) aim.SetActive(active);
    }

    private void DoAttack()
    {
      animator.ResetTrigger("Attack");

      var vectorToTarget = (targetPosition - (Vector2)launchTransform.position).normalized;
      var directAngle = Vector2.SignedAngle(Vector2.right, vectorToTarget);
      var startAngle = directAngle - skillAngle * ((numberOfSkills - 1) / 2);

      for (int i = 0; i < numberOfSkills; i++)
      {
        for (int j = 0; j < 8; j++)
        {
          GameObject skill = objectPool.GetObject("Enemy/Boss/IceTiger/IceTigerSkill");
          var direction = Quaternion.AngleAxis(startAngle + i * skillAngle, Vector3.forward) * Vector2.right;
          skill.transform.SetParent(null);
          skill.transform.position = launchTransform.position + direction.normalized * j * 0.5f;
        }
      }

      SetWarningAimActive(false);
      Idle();

      scheduling.ClearSchedule(statusTransitionScheduleId);
      statusTransitionScheduleId = scheduling.SetTimeout(chaseAction, restAfterAttackTimeout);
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

      // Execution handled by BaseBossController
    }
  }
}