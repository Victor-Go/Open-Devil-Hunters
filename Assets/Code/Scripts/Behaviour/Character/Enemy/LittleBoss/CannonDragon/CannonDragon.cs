using System.Collections.Generic;
using Code.Scripts.Behaviour.Character.Enemy;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Steam.Achievements;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.LittleBoss.CannonDragon
{

  public class CannonDragon : PoolableLittleBoss
  {
    public Transform[] LaunchPositions;

    private float[] launchPositionXOffset;

    private const float maxApproachDistance = 6;
    private const float minApproachDistance = 4.5f;
    private const float waitForAttackTimeout = 1;
    private const float restAfterAttackTimeout = 1;

    private GameObject aim;
    private Vector2 targetPosition;

    private BossIdleState idleState;
    private WalkState walkState;
    private AttackState attackState;

    private System.Action attackAction;
    private System.Action walkAction;

    private float currentApproachDistance;
    private long statusTransitionScheduleId;
    private AudioWrapper launchAudio;

    protected override void Awake()
    {
      base.Awake();

      launchAudio = new AudioWrapper(new[]
        {
          "Audio/Sound/Enemy/Skill/cannon-dragon-skill_0",
          "Audio/Sound/Enemy/Skill/cannon-dragon-skill_1",
        },
        transform
      );

      animator = GetComponentInChildren<Animator>();

      launchPositionXOffset = new float[LaunchPositions.Length];
      for (var i = 0; i < LaunchPositions.Length; i++)
      {
        launchPositionXOffset[i] = LaunchPositions[i].localPosition.x;
      }

      idleState = new BossIdleState(this);
      walkState = new WalkState(this);
      attackState = new AttackState(this);

      attackAction = Attack;
      walkAction = Walk;
    }

    protected override void Start()
    {
      base.Start();

      var _aim = objectPool.GetObject("Enemy/EnemyAim");
      _aim.transform.SetParent(null);
      _aim.SetActive(false);
      aim = _aim;

      SelectOnePlayerToChase();
      Walk();
    }

    public override void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      base.OnEnemyDied(playerNumber, hurtType);
      AchievementsManager.UnlockAchievement(Achievements.CannonDragonKiller);
      SetWarningAimActive(false);
    }

    private void Idle()
    {
      StateMachine.ChangeState(idleState);
    }

    private void Walk()
    {
      StateMachine.ChangeState(walkState);
    }

    private class WalkState : BossWalkState
    {
      public WalkState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        var bossCast = (CannonDragon)boss;
        bossCast.currentApproachDistance = Random.Range(minApproachDistance + 1, maxApproachDistance);
        bossCast.rb.bodyType = RigidbodyType2D.Dynamic;
      }
      public override void Execute(float dt)
      {
        var bossCast = (CannonDragon)boss;
        var playerPosition = bossCast.playerPositions[bossCast.selectedPlayerNumber];
        var vectorToTarget = playerPosition - (Vector2)bossCast.transform.position;
        var movement = vectorToTarget.normalized * (dt * bossCast.updatedEnemyConfigurations.Speed);

        if (vectorToTarget.sqrMagnitude < Mathf.Pow(minApproachDistance, 2))
          movement *= -2; // Leave player with double speed.

        bossCast.rb.MovePosition(bossCast.rb.position + movement);
        bossCast.SetAnimatorDirection(movement);

        var distance = vectorToTarget.sqrMagnitude;
        if (Mathf.Pow(minApproachDistance, 2) < distance && distance < Mathf.Pow(bossCast.currentApproachDistance, 2))
        {
          bossCast.SetAnimatorDirection(vectorToTarget);
          bossCast.Idle();

          bossCast.rb.bodyType = RigidbodyType2D.Kinematic; // Prevent position change
          bossCast.rb.velocity = Vector2.zero;
          bossCast.SetTargetPosition();
          bossCast.PlaceWarningAim();

          bossCast.scheduling.ClearSchedule(bossCast.statusTransitionScheduleId);
          bossCast.statusTransitionScheduleId = bossCast.scheduling.SetTimeout(bossCast.attackAction, waitForAttackTimeout);
        }
      }
    }

    public override void SetAnimatorHorizontalPosition(AnimatorDirection animatorDirection)
    {
      int sign = animatorDirection == AnimatorDirection.Right ? -1 : 1;
      for (var i = 0; i < LaunchPositions.Length; i++)
      {
        var pos = LaunchPositions[i].localPosition;
        pos.x = launchPositionXOffset[i] * sign;
        LaunchPositions[i].localPosition = pos;
      }
    }

    private class AttackState : BossAttackState
    {
      public AttackState(BaseBossController boss) : base(boss) {}
    }

    private void SetTargetPosition()
    {
      targetPosition = playerPositions[selectedPlayerNumber];
    }

    // Place aim to warn player
    private void PlaceWarningAim()
    {
      aim.transform.position = targetPosition;
      aim.SetActive(true);
    }

    private void Attack()
    {
      StateMachine.ChangeState(attackState);
    }

    public void OnAttackAnimationFinished()
    {
      DoAttack();
    }

    private void SetWarningAimActive(bool active = true)
    {
      aim.SetActive(active);
    }

    private void DoAttack()
    {
      animator.ResetTrigger("Attack");

      launchAudio.PlayRandomly();

      const int dispersion = 3;
      var bulletMultiple = Random.Range(3, 5);
      var numberOfBullets = LaunchPositions.Length * bulletMultiple;
      var perpendicular = Vector2.Perpendicular(targetPosition - (Vector2)transform.position).normalized;
      var initPosition = perpendicular * dispersion / 2;
      var segment = -perpendicular * dispersion / numberOfBullets;
      for (var i = 0; i < numberOfBullets; i++)
      {
        var bullet = objectPool.GetObject("Enemy/LittleBoss/CannonDragon/CannonDragonSkill");
        bullet.transform.SetParent(null);
        bullet.transform.position = LaunchPositions[i / bulletMultiple].position;
        bullet.GetComponent<CannonDragonSkill>()
          .SetTargetPosition(targetPosition + initPosition + segment * i);
      }

      Idle();
      SetWarningAimActive(false);

      scheduling.ClearSchedule(statusTransitionScheduleId);
      statusTransitionScheduleId = scheduling.SetTimeout(walkAction, restAfterAttackTimeout * Random.Range(1, 1.5f));
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