using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Steam.Achievements;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.LittleBoss.Witch
{

  public class Witch : PoolableLittleBoss
  {
    private const float ApproachStopDistance = 3;
    private const float LeaveDistance = 10;
    private const float ChargeTimeout = 2;
    private const float WaitingTimeout = 3;
    private const int BulletMinQuantity = 3;
    private const int BulletMaxQuantity = 6;

    public GameObject firePosition;

    private BossIdleState idleState;
    private WaitingToApproachState waitingToApproachState;
    private ApproachingState approachingState;
    private ChargingState chargingState;
    private AttackingState attackingState;
    private LeavingState leavingState;
    
    private System.Action startLeavingAction;
    private long statusTransitionScheduleId;

    private Vector2 movingTargetPosition;
    private float switchStatusCountdown;
    private int bulletRound;
    private float approachStopDistance;
    private readonly string skillId = "WitchBullet";

    protected override void Awake()
    {
      base.Awake();

      chaseRandomThresholdDistance = 2;

      idleState = new BossIdleState(this);
      waitingToApproachState = new WaitingToApproachState(this);
      approachingState = new ApproachingState(this);
      chargingState = new ChargingState(this);
      attackingState = new AttackingState(this);
      leavingState = new LeavingState(this);
      
      startLeavingAction = StartLeaving;
    }

    protected override void Start()
    {
      base.Start();

      storeManager.Subscribe(StoreNames.PlayerPositionStore, this);

      ChangeStatusToApproaching();
      SelectOnePlayerToChase();
    }

    public override void EnemyReset()
    {
      bulletRound = 0;
      scheduling.ClearSchedule(statusTransitionScheduleId);
      ChangeStatusToApproaching();
    }

    public override void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      base.OnEnemyDied(playerNumber, hurtType);
      AchievementsManager.UnlockAchievement(Achievements.WitchKiller);
    }

    private void ChangeStatusToApproaching()
    {
      StateMachine.ChangeState(approachingState);
    }

    private void StartLeaving()
    {
      movingTargetPosition =
        (Vector2)(Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) *
                  (LeaveDistance * Random.Range(0.75f, 1.5f) * Vector2.right)) +
        playerPositions[selectedPlayerNumber];
      StateMachine.ChangeState(leavingState);
    }

    public void OnEnemyPoisonResolved()
    {
      poisoned = false;
      spriteRenderer.color = Color.white;
    }

    private class ApproachingState : BossWalkState
    {
      public ApproachingState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        var bossCast = (Witch)boss;
        bossCast.approachStopDistance = ApproachStopDistance * Random.Range(0.66f, 1.33f);
      }
      public override void Execute(float dt)
      {
        var bossCast = (Witch)boss;
        Vector2 direction = bossCast.playerPositions[bossCast.selectedPlayerNumber] - bossCast.rb.position;
        Vector2 updatedPosition = direction.normalized * (bossCast.updatedEnemyConfigurations.Speed * dt) + bossCast.rb.position;

        if (direction.sqrMagnitude < bossCast.approachStopDistance * bossCast.approachStopDistance)
        {
          bossCast.StateMachine.ChangeState(bossCast.chargingState);
        }
        else
        {
          bossCast.rb.MovePosition(updatedPosition);
          bossCast.SetAnimatorDirection(direction);
        }
      }
    }

    private class ChargingState : BossIdleState
    {
      public ChargingState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        var bossCast = (Witch)boss;
        bossCast.switchStatusCountdown = ChargeTimeout;
      }
      public override void Execute(float dt)
      {
        var bossCast = (Witch)boss;
        bossCast.switchStatusCountdown -= dt;
        if (bossCast.switchStatusCountdown <= 0)
        {
          bossCast.StateMachine.ChangeState(bossCast.attackingState);
        }
      }
    }

    private class AttackingState : BossAttackState
    {
      public AttackingState(BaseBossController boss) : base(boss) {}
    }

    private class LeavingState : BossWalkState
    {
      public LeavingState(BaseBossController boss) : base(boss) {}
      public override void Execute(float dt)
      {
        var bossCast = (Witch)boss;
        Vector2 direction = bossCast.movingTargetPosition - bossCast.rb.position;
        Vector2 updatedPosition = direction.normalized * (bossCast.updatedEnemyConfigurations.Speed * 2 * dt) + bossCast.rb.position;

        if (direction.sqrMagnitude < 0.25f)
        {
          bossCast.StateMachine.ChangeState(bossCast.waitingToApproachState);
        }
        else
        {
          bossCast.rb.MovePosition(updatedPosition);
          bossCast.SetAnimatorDirection(direction);
        }
      }
    }

    private class WaitingToApproachState : BossIdleState
    {
      public WaitingToApproachState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        var bossCast = (Witch)boss;
        bossCast.switchStatusCountdown = WaitingTimeout;
      }
      public override void Execute(float dt)
      {
        var bossCast = (Witch)boss;
        bossCast.switchStatusCountdown -= dt;
        if (bossCast.switchStatusCountdown <= 0)
        {
          bossCast.ChangeStatusToApproaching();
        }
      }
    }

    private void SpawnBullets()
    {
      var quantity = BulletMinQuantity + Mathf.RoundToInt(bulletRound / 3f);
      quantity = Mathf.Min(quantity, BulletMaxQuantity);

      var angle = 360f / quantity;
      var preset = SkillPresets.BossSkills[skillId];
      for (var i = 0; i < quantity; i++)
      {
        var witchBullet = objectPool.GetObject(preset.SkillPrefabName, firePosition.transform.position);

        var bulletController = witchBullet.GetComponent<WitchBullet>();
        bulletController.SetConfigurations(preset.SkillConfigurations);
        bulletController.SetHostStrength(totalStrength);
        bulletController.SetAngle(i * angle);
      }

      bulletRound++;
    }

    public void OnAttack()
    {
      SpawnBullets();

      StateMachine.ChangeState(idleState);

      scheduling.ClearSchedule(statusTransitionScheduleId);
      statusTransitionScheduleId = scheduling.SetTimeout(startLeavingAction, 3);
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