using System.Collections.Generic;
using Code.Scripts.Behaviour.Character.Enemy;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.CellMonster
{

  public class CellMonster : PoolableLittleBoss, IEnemyCanInvoke
  {
    public Transform SprayPosition;
    public Transform WormLaunchPosition;

    private const int minInvoke = 15;
    private const int maxInvoke = 35;
    private const float approachDistance = 3;
    private const float leaveDistance = 8;
    private const float invokeRestTimeout = 3;
    private const float leaveRestTimeout = 3;

    private float initSprayPositionX;
    private float initWormLaunchPositionX;
    private long statusTransitionScheduleId;

    private BossIdleState idleState;
    private ApproachState approachState;
    private LeaveState leaveState;
    private InvokeState invokeState;
    private System.Action approachAction;
    private System.Action invokeAction;
    private System.Action leaveAction;

    private float currentApproachDistance;
    private Vector2 leavePosition;
    private float leaveSpeedMultiple = 1;

    private int numberOfInvokedEnemies;

    protected override void Awake()
    {
      base.Awake();

      initSprayPositionX = SprayPosition.localPosition.x;
      initWormLaunchPositionX = WormLaunchPosition.localPosition.x;
      animatorTransform = transform.Find("Animator");

      idleState = new IdleState(this);
      approachState = new ApproachState(this);
      leaveState = new LeaveState(this);
      invokeState = new InvokeState(this);

      approachAction = Approach;
      invokeAction = Invoke;
      leaveAction = Leave;
    }

    protected override void Start()
    {
      base.Start();

      SelectOnePlayerToChase();
      Approach();
    }

    private void SetPositions(AnimatorDirection animatorDirection)
    {
      var sprayPos = SprayPosition.localPosition;
      var wormLaunchPos = WormLaunchPosition.localPosition;

      switch (animatorDirection)
      {
        case AnimatorDirection.Left:
          SprayPosition.localPosition = new Vector2(initSprayPositionX, sprayPos.y);
          WormLaunchPosition.localPosition = new Vector2(initWormLaunchPositionX, wormLaunchPos.y);
          break;
        case AnimatorDirection.Right:
          SprayPosition.localPosition = new Vector2(-initSprayPositionX, sprayPos.y);
          WormLaunchPosition.localPosition = new Vector2(-initWormLaunchPositionX, wormLaunchPos.y);
          break;
      }
    }

    protected override void SetAnimatorDirection(Vector2 direction)
    {
      base.SetAnimatorDirection(direction);
      SetPositions(direction.x < 0 ? AnimatorDirection.Left : AnimatorDirection.Right);
    }

    private void Idle()
    {
      StateMachine.ChangeState(idleState);
    }

    private void Approach()
    {
      StateMachine.ChangeState(approachState);
    }

    private void Leave()
    {
      StateMachine.ChangeState(leaveState);
    }

    private void Invoke()
    {
      StateMachine.ChangeState(invokeState);
    }

    private class ApproachState : BossWalkState
    {
      public ApproachState(BaseBossController boss) : base(boss) {}
      public override void Enter() 
      {
        base.Enter();
        ((CellMonster)boss).currentApproachDistance = Random.Range(1, 1.5f) * approachDistance;
      }
      public override void Execute(float dt) 
      {
        var bossCast = (CellMonster)boss;
        var playerPosition = bossCast.playerPositions[bossCast.selectedPlayerNumber];
        var movement = (playerPosition - (Vector2)bossCast.transform.position).normalized *
                       (dt * bossCast.updatedEnemyConfigurations.Speed);

        bossCast.rb.MovePosition(bossCast.rb.position + movement);
        bossCast.SetAnimatorDirection(movement);

        var sqrDistance = ((Vector2)bossCast.transform.position - playerPosition).sqrMagnitude;
        if (sqrDistance <= Mathf.Pow(bossCast.currentApproachDistance, 2))
        {
          bossCast.Invoke();

          bossCast.scheduling.ClearSchedule(bossCast.statusTransitionScheduleId);
          bossCast.statusTransitionScheduleId = bossCast.scheduling.SetTimeout(bossCast.invokeAction, leaveRestTimeout);
        }
      }
    }

    private class LeaveState : BossWalkState
    {
      public LeaveState(BaseBossController boss) : base(boss) {}
      public override void Enter() 
      {
        base.Enter();
        var bossCast = (CellMonster)boss;
        bossCast.leaveSpeedMultiple = Random.Range(1.5f, 2f);

        var currentLeaveDistance = Random.Range(1, 1.5f) * leaveDistance;

        var sum = Vector2.zero;
        for (var i = 0; i < bossCast.numberOfPlayers; i++)
        {
          sum += bossCast.playerPositions[i];
        }

        var centerPosition = sum / bossCast.numberOfPlayers;
        bossCast.leavePosition =
          (Vector2)(Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector2.right * currentLeaveDistance) +
          centerPosition;
      }
      public override void Execute(float dt) 
      {
        var bossCast = (CellMonster)boss;
        var movement = (bossCast.leavePosition - (Vector2)bossCast.transform.position).normalized * (dt * bossCast.updatedEnemyConfigurations.Speed);

        bossCast.rb.MovePosition(bossCast.rb.position + movement * bossCast.leaveSpeedMultiple);
        bossCast.SetAnimatorDirection(movement);

        var sqrDistance = ((Vector2)bossCast.transform.position - bossCast.leavePosition).sqrMagnitude;
        if (sqrDistance <= 0.1f)
        {
          bossCast.Idle();

          bossCast.scheduling.ClearSchedule(bossCast.statusTransitionScheduleId);
          bossCast.statusTransitionScheduleId = bossCast.scheduling.SetTimeout(bossCast.approachAction, 1);
        }
      }
    }

    private class InvokeState : BaseState<BaseBossController>
    {
      public InvokeState(BaseBossController boss) : base(boss) {}
      public override void Enter() { boss.SetAnimTrigger("Invoke"); }
    }

    public void OnInvokeAnimationForSpray()
    {
      animator.ResetTrigger("Invoke");

      animator.speed = 0;

      var direction = animatorTransform.localScale.x > 0
        ? CellMonsterInvokeAnimationDirection.LEFT
        : CellMonsterInvokeAnimationDirection.RIGHT;

      GameObject animation = objectPool.GetObject("Enemy/Boss/CellMonster/CellMonsterInvokeEffect");
      animation.transform.SetParent(null);
      animation.GetComponent<CellMonsterInvokeAnimation>()
        .SetAnimationFinishedCallback(OnSprayAnimationFinished)
        .SetPosition(SprayPosition.position, direction);
    }

    public void OnSprayAnimationFinished()
    {
      animator.speed = paused ? 0 : 1;

      DoInvoke();
    }

    public void NoticeInvokedEnemyDied(string objectName)
    {
      numberOfInvokedEnemies--;
    }

    private void DoInvoke()
    {
      var worms = new List<GameObject>();
      var wormsToInvoke = Random.Range(minInvoke, maxInvoke);
      for (var i = 0; i < wormsToInvoke; i++)
      {
        var worm = objectPool.GetObject("Enemy/Boss/CellMonster/Worm");
        worm.transform.SetParent(null);
        worm.transform.position = WormLaunchPosition.position;

        var wormController = worm.GetComponent<InvokedEnemy>();
        wormController.SetParentEnemy(this);
        wormController.SetEnemyBaseStrength(totalStrength);
        worms.Add(worm);
      }

      storeManager.Commit(Assets.Code.Scripts.Src.StoreNames.LevelStore,
        Assets.Code.Scripts.Src.StoreActions.LevelStore_ADD_ENEMIES, new LevelData()
        {
          Enemies = worms
        });

      numberOfInvokedEnemies += worms.Count;

      Idle();

      scheduling.ClearSchedule(statusTransitionScheduleId);
      statusTransitionScheduleId = scheduling.SetTimeout(leaveAction, invokeRestTimeout);
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