using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.RockGiant
{


  public class RockGiant : PoolableLittleBoss
  {
    public Transform LaunchPosition;

    private readonly string[] launchSoundNames =
    {
      "Audio/Sound/Enemy/Boss/rockgiant-launch_0",
    };

    private List<AudioClip> launchClips;

    [SerializeField] private const float bumpDistance = 2.5f; // Distance to bump player

    [SerializeField] private const float leaveDistance = 3f; // Distance to leave player
    [SerializeField] private const float chaseDistance = 4.5f; // Distance to throw rocks

    [SerializeField] private const float waitForAttackTimeout = 2.5f;

    [SerializeField] private const float bumpFinishedWaitTimeout = 1;

    private BossIdleState idleState;
    private ChaseState chaseState;
    private LeaveState leaveState;
    private BumpState bumpState;
    private AttackState attackState;

    private string statusTransitionScheduleId;
    private Vector2 bumpPosition;
    private float initLaunchPositionX;

    private System.Action attackAction;
    private System.Action evaluateDistanceAction;

    protected override void Awake()
    {
      base.Awake();

      animator = GetComponentInChildren<Animator>();
      initLaunchPositionX = LaunchPosition.localPosition.x;

      launchClips = launchSoundNames.Select(name => (AudioClip)resourceManager.GetResource(name)).ToList();

      attackAction = Attack;
      evaluateDistanceAction = EvaluateDistanceAfterBump;

      idleState = new BossIdleState(this);
      chaseState = new ChaseState(this);
      leaveState = new LeaveState(this);
      bumpState = new BumpState(this);
      attackState = new AttackState(this);
    }

    protected override void Start()
    {
      base.Start();

      SelectOnePlayerToChase();
      StateMachine.ChangeState(chaseState);
    }

    protected override void SetAnimatorDirection(Vector2 direction)
    {
      base.SetAnimatorDirection(direction);

      if (direction.x < -GeneralConfigurations.AnimationDirectionSettingThreshold)
      {
        var lpos = LaunchPosition.localPosition;
        LaunchPosition.localPosition = new Vector2(initLaunchPositionX, lpos.y);
      }
      else if (direction.x > GeneralConfigurations.AnimationDirectionSettingThreshold)
      {
        var lpos = LaunchPosition.localPosition;
        LaunchPosition.localPosition = new Vector2(-initLaunchPositionX, lpos.y);
      }
    }

    private void EvaluateDistanceAfterBump()
    {
      SelectOnePlayerToChase();
      var playerPosition = playerPositions[selectedPlayerNumber];
      var sqrDistance = (playerPosition - (Vector2)transform.position).sqrMagnitude;
      ChangeStatusRelativeToSqrDistance(sqrDistance);
    }

    private void ChangeStatusRelativeToSqrDistance(float sqrDistance)
    {
      if (StateMachine.CurrentState == bumpState)
      {
        if (sqrDistance <= 0.1f)
        {
          StateMachine.ChangeState(idleState);

          scheduling.ClearSchedule(statusTransitionScheduleId);
          statusTransitionScheduleId = scheduling.SetTimeout(evaluateDistanceAction, bumpFinishedWaitTimeout);
        }

        return;
      }

      if (sqrDistance <= Mathf.Pow(bumpDistance, 2))
      {
        StateMachine.ChangeState(bumpState);
      }
      else if (sqrDistance <= Mathf.Pow(leaveDistance, 2))
      {
        StateMachine.ChangeState(leaveState);
      }
      else if (sqrDistance <= Mathf.Pow(chaseDistance, 2))
      {
        StateMachine.ChangeState(idleState);
        scheduling.ClearSchedule(statusTransitionScheduleId);
        statusTransitionScheduleId = scheduling.SetTimeout(attackAction, waitForAttackTimeout);
      }
      else
      {
        StateMachine.ChangeState(chaseState);
      }
    }

    private class ChaseState : BossWalkState
    {
      public ChaseState(BaseBossController boss) : base(boss) {}
      public override void Execute(float dt) { ((RockGiant)boss).DoChase(dt); }
    }

    private class LeaveState : BossWalkState
    {
      public LeaveState(BaseBossController boss) : base(boss) {}
      public override void Execute(float dt) { ((RockGiant)boss).DoLeave(dt); }
    }

    private class BumpState : BossWalkState
    {
      public BumpState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        ((RockGiant)boss).OnEnterBump();
      }
      public override void Execute(float dt) { ((RockGiant)boss).DoBump(dt); }
    }

    private class AttackState : BossAttackState
    {
      public AttackState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        ((RockGiant)boss).OnEnterAttack();
      }
    }

    public void OnEnterBump()
    {
      var playerPosition = playerPositions[selectedPlayerNumber];
      var vectorToTarget = playerPosition - (Vector2)transform.position;
      var direction = vectorToTarget.normalized;
      bumpPosition = (Vector2)transform.position + direction * 2;

      AudioWrapper.PlayClip(launchClips[Random.Range(0, launchClips.Count)], transform.position);
    }

    private void DoChase(float dt)
    {
      var playerPosition = playerPositions[selectedPlayerNumber];
      var vectorToTarget = playerPosition - (Vector2)transform.position;
      var movement = vectorToTarget.normalized * (dt * updatedEnemyConfigurations.Speed);

      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(movement);

      var sqrDistance = vectorToTarget.sqrMagnitude;
      ChangeStatusRelativeToSqrDistance(sqrDistance);
    }

    private void DoLeave(float dt)
    {
      var playerPosition = playerPositions[selectedPlayerNumber];
      var vectorToTarget = playerPosition - (Vector2)transform.position;
      var movement = -vectorToTarget.normalized * (dt * updatedEnemyConfigurations.Speed * 1.5f);

      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(movement);

      var sqrDistance = vectorToTarget.sqrMagnitude;
      ChangeStatusRelativeToSqrDistance(sqrDistance);
    }

    private void DoBump(float dt)
    {
      var vectorToTarget = bumpPosition - (Vector2)transform.position;
      var movement = vectorToTarget.normalized * (dt * updatedEnemyConfigurations.Speed * 2.5f);

      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(movement);

      var sqrDistance = ((Vector2)transform.position - bumpPosition).sqrMagnitude;
      ChangeStatusRelativeToSqrDistance(sqrDistance);
    }

    private void Attack()
    {
      StateMachine.ChangeState(attackState);
    }

    public void OnEnterAttack()
    {
      var playerPosition = playerPositions[selectedPlayerNumber];
      var vectorToTarget = playerPosition - (Vector2)transform.position;
      SetAnimatorDirection(vectorToTarget);
    }

    public void OnAttackAnimationFinished()
    {
      DoAttack();
    }

    private void DoAttack()
    {
      animator.ResetTrigger("Attack");

      var rock = objectPool.GetObject("Enemy/Boss/RockGiant/RockGiantSkill");
      rock.transform.SetParent(null);

      var launchPosition = LaunchPosition.position;
      rock.transform.position = launchPosition;

      var playerPosition = selectedPlayerNumber <= playerPositions.Length - 1
        ? playerPositions[selectedPlayerNumber]
        : playerPositions[0];

      rock.GetComponent<RockGiantSkill>().SetTargetPosition(playerPosition);

      StateMachine.ChangeState(chaseState);
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
  }
}