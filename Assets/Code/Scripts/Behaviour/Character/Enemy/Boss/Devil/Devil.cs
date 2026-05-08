using System.Collections.Generic;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.Skill.Bullets;
using Code.Scripts.Src.Steam.Achievements;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.Devil
{
  public enum DevilStatus
  {
    IDLE,
    APPROACH,
    LEAVE,
    WALK_ATTACK,
    ATTACK,
  }

  public class Devil : PoolableBigBoss
  {
    private float approachDistance { get; } = 4;
    private float leaveDistance { get; } = 8;
    private float attackDistance { get; } = 5;
    private float prepareAttackTimeout { get; } = 3;
    private float restAfterAttackTimeout { get; } = 3;
    private float restAfterLeavingTimeout { get; } = 5;
    private float walkAndAttackDuration { get; } = 10;
    private float maxWalkAndAttackDistance { get; } = 4;
    private float restAfterWalkAndAttackTimeout { get; } = 5;
    private float maxLeaveTimeout { get; } = 10;
    private float maxLeaveCount { get; } = 2;

    private Transform leftLaunchPosition;
    private Transform rightLaunchPosition;

    private Vector2 initLeftLaunchPosition;
    private Vector2 initRightLaunchPosition;

    private DevilStatus devilStatus;
    private string statusTransitionScheduleId;
    private float walkAndAttackTimeout;
    private float currentApprochDistance;
    private float currentLeaveDistance;
    private int currentWalkAndAttackDirection;
    private float walkAndAttackLaunchTimeout;
    private float leaveDuration;
    private float currentLeaveCount;

    private readonly List<string> launchSoundNames = new()
    {
      "Audio/Sound/Enemy/Boss/devil-launch_0",
      "Audio/Sound/Enemy/Boss/devil-launch_1",
    };

    private List<AudioClip> launchClips;
    private System.Action attackAction;
    private System.Action leaveOrApproachOrWalkAndAttackAction;
    private readonly List<Vector2> playersInRangeCached = new List<Vector2>(4);

    protected override void Awake()
    {
      base.Awake();

      animator = GetComponentInChildren<Animator>();

      leftLaunchPosition = transform.Find("LeftLaunchPosition");
      rightLaunchPosition = transform.Find("RightLaunchPosition");

      initLeftLaunchPosition = leftLaunchPosition.localPosition;
      initRightLaunchPosition = rightLaunchPosition.localPosition;

      launchClips = new List<AudioClip>(launchSoundNames.Count);
      for (var i = 0; i < launchSoundNames.Count; i++)
      {
        launchClips.Add((AudioClip)resourceManager.GetResource(launchSoundNames[i]));
      }

      attackAction = Attack;
      leaveOrApproachOrWalkAndAttackAction = LeaveOrApprochOrWalkAndAttack;
    }

    protected override void Start()
    {
      base.Start();

      SelectOnePlayerToChase();
      LeaveOrApprochOrWalkAndAttack();
    }

    public override void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      base.OnEnemyDied(playerNumber, hurtType);
      AchievementsManager.UnlockAchievement(Achievements.DevilHunter);
    }

    protected override void SetAnimatorDirection(Vector2 direction)
    {
      base.SetAnimatorDirection(direction);

      if (direction.x < 0)
      {
        leftLaunchPosition.localPosition = initLeftLaunchPosition;
        rightLaunchPosition.localPosition = initRightLaunchPosition;
      }
      else if (direction.x > 0)
      {
        leftLaunchPosition.localPosition = new Vector2(-initLeftLaunchPosition.x, initLeftLaunchPosition.y);
        rightLaunchPosition.localPosition = new Vector2(-initRightLaunchPosition.x, initRightLaunchPosition.y);
      }
    }

    private void Idle()
    {
      devilStatus = DevilStatus.IDLE;
      animator.ResetTrigger("Walk");
      animator.ResetTrigger("WalkAndAttack");
      animator.ResetTrigger("Attack");
      animator.SetTrigger("Idle");

      var nearest = GetNearestPlayerPosition();
      SetAnimatorDirection(nearest - (Vector2)transform.position);
    }

    private void Approach()
    {
      devilStatus = DevilStatus.APPROACH;
      animator.ResetTrigger("Idle");
      animator.ResetTrigger("WalkAndAttack");
      animator.ResetTrigger("Attack");
      animator.SetTrigger("Walk");

      currentLeaveCount = 0;
      currentApprochDistance = Random.Range(1, 1.25f) * approachDistance;
    }

    private void DoApproch(float dt)
    {
      var playerPosition = playerPositions[selectedPlayerNumber];
      var movement = (playerPosition - (Vector2)transform.position).normalized *
                     (dt * updatedEnemyConfigurations.Speed);

      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(movement);

      var sqrDistance = ((Vector2)transform.position - playerPosition).sqrMagnitude;
      if (sqrDistance <= Mathf.Pow(currentApprochDistance, 2))
      {
        Idle();

        scheduling.ClearSchedule(statusTransitionScheduleId);
        statusTransitionScheduleId = scheduling.SetTimeout(attackAction, prepareAttackTimeout);
      }
    }

    private void Leave()
    {
      devilStatus = DevilStatus.LEAVE;
      animator.ResetTrigger("Idle");
      animator.ResetTrigger("WalkAndAttack");
      animator.ResetTrigger("Attack");
      animator.SetTrigger("Walk");

      currentLeaveDistance = Random.Range(0.85f, 1.25f) * leaveDistance;
      leaveDuration = 0;
      currentLeaveCount++;
    }

    private void DoLeave(float dt)
    {
      var centerPosition = GetPlayersCenterPosition();
      var movement = ((Vector2)transform.position - centerPosition).normalized *
                     (dt * updatedEnemyConfigurations.Speed);

      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(movement);

      leaveDuration += dt;

      var sqrDistance = ((Vector2)transform.position - centerPosition).sqrMagnitude;
      if (sqrDistance > Mathf.Pow(currentLeaveDistance, 2) || leaveDuration > maxLeaveTimeout)
      {
        Idle();

        scheduling.ClearSchedule(statusTransitionScheduleId);
        statusTransitionScheduleId =
          scheduling.SetTimeout(leaveOrApproachOrWalkAndAttackAction, restAfterLeavingTimeout);
      }
    }

    private void Attack()
    {
      devilStatus = DevilStatus.ATTACK;
      animator.ResetTrigger("Idle");
      animator.ResetTrigger("WalkAndAttack");
      animator.ResetTrigger("Walk");
      animator.SetTrigger("Attack");

      currentLeaveCount = 0;

      AudioWrapper.PlayClip(launchClips[Random.Range(0, launchClips.Count)], transform.position);
    }

    public void OnAttackAnimationFinished()
    {
      DoAttack();
    }

    private void DoAttack()
    {
      animator.ResetTrigger("Attack");

      playersInRangeCached.Clear();
      float sqrAttackDistance = attackDistance * attackDistance;
      for (int i = 0; i < playerPositions.Length; i++)
      {
        if ((playerPositions[i] - (Vector2)transform.position).sqrMagnitude < sqrAttackDistance)
        {
          playersInRangeCached.Add(playerPositions[i]);
        }
      }

      if (playersInRangeCached.Count > 0)
      {
        SetAnimatorDirection(playersInRangeCached[0] - (Vector2)transform.position);

        var number = Random.Range(0, 2);
        var numberMultiple = Random.Range(2, 4);
        var numberOfBullets = 2 * numberMultiple;
        switch (number)
        {
          case 0:
            for (var i = 0; i < numberOfBullets; i += 2)
            {
              var bat0 = objectPool.GetObject("Enemy/Boss/Devil/DevilBurningBat", leftLaunchPosition.position);
              bat0.GetComponent<LinearEnemySkill>()
                .SetDirection(Quaternion.Euler(0, 0, Random.Range(-10f, 10f)) *
                              (playersInRangeCached[0] - (Vector2)leftLaunchPosition.position))
                .SetHostStrength(totalStrength);

              var bat1 = objectPool.GetObject("Enemy/Boss/Devil/DevilBurningBat", rightLaunchPosition.position);
              bat1.GetComponent<LinearEnemySkill>()
                .SetDirection(Quaternion.Euler(0, 0, Random.Range(-10f, 10f)) *
                              (playersInRangeCached[^1] - (Vector2)rightLaunchPosition.position))
                .SetHostStrength(totalStrength);
            }

            break;
          case 1:
            var skull0 = objectPool.GetObject("Enemy/Boss/Devil/DevilPoisonSkull", leftLaunchPosition.position);
            skull0.GetComponent<LinearEnemySkill>()
              .SetDirection(playersInRangeCached[0] - (Vector2)leftLaunchPosition.position)
              .SetHostStrength(totalStrength);

            var skull1 = objectPool.GetObject("Enemy/Boss/Devil/DevilPoisonSkull", rightLaunchPosition.position);
            skull1.GetComponent<LinearEnemySkill>()
              .SetDirection(playersInRangeCached[^1] - (Vector2)rightLaunchPosition.position)
              .SetHostStrength(totalStrength);

            break;
        }
      }

      Idle();
      scheduling.ClearSchedule(statusTransitionScheduleId);
      statusTransitionScheduleId =
        scheduling.SetTimeout(leaveOrApproachOrWalkAndAttackAction, restAfterAttackTimeout);
    }

    private void WalkAndAttack()
    {
      devilStatus = DevilStatus.WALK_ATTACK;
      animator.ResetTrigger("Idle");
      animator.ResetTrigger("Attack");
      animator.ResetTrigger("Walk");
      animator.SetTrigger("WalkAndAttack");

      walkAndAttackTimeout = walkAndAttackDuration;
      currentWalkAndAttackDirection = -1 * Random.Range(0, 1f) > 0.5f ? 1 : -1;
      currentLeaveCount = 0;
    }

    private Vector2 GetNearestPlayerPosition()
    {
      return GeneralUtils.GetNearestPosition(transform.position, playerPositions);
    }

    private void DoWalkAndAttack(float dt)
    {
      var playerPosition = GetNearestPlayerPosition();
      var vectorFromPlayer = (Vector2)transform.position - playerPosition;
      var movingDirection = Vector2.Perpendicular(vectorFromPlayer) * currentWalkAndAttackDirection;

      var sqrDistance = vectorFromPlayer.sqrMagnitude;
      if (sqrDistance <= Mathf.Pow(approachDistance, 2)) // If the distance to player is too close, then leave
      {
        movingDirection = movingDirection.normalized + vectorFromPlayer.normalized;
      }
      else if (sqrDistance > maxWalkAndAttackDistance) // If the distance to player is too far, then get close to it.
      {
        movingDirection = movingDirection.normalized + -vectorFromPlayer.normalized;
      }

      var movement = movingDirection.normalized * (dt * updatedEnemyConfigurations.Speed);

      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(-vectorFromPlayer);

      walkAndAttackLaunchTimeout -= dt;
      if (walkAndAttackLaunchTimeout <= 0)
      {
        walkAndAttackLaunchTimeout = 1;

        var shockWave0 = objectPool.GetObject("Enemy/Boss/Devil/DevilShockWave", leftLaunchPosition.position);
        shockWave0.GetComponent<LinearEnemySkill>()
          .SetDirection(playerPosition - (Vector2)leftLaunchPosition.position)
          .SetHostStrength(totalStrength);
        var shockWave1 = objectPool.GetObject("Enemy/Boss/Devil/DevilShockWave", rightLaunchPosition.position);
        shockWave1.GetComponent<LinearEnemySkill>()
          .SetDirection(playerPosition - (Vector2)rightLaunchPosition.position)
          .SetHostStrength(totalStrength);
      }

      walkAndAttackTimeout -= dt;
      if (walkAndAttackTimeout <= 0)
      {
        Idle();

        scheduling.ClearSchedule(statusTransitionScheduleId);
        statusTransitionScheduleId =
          scheduling.SetTimeout(leaveOrApproachOrWalkAndAttackAction, restAfterWalkAndAttackTimeout);
      }
    }

    private void LeaveOrApprochOrWalkAndAttack()
    {
      var nearestPlayerPosition = GetNearestPlayerPosition();
      var sqrDistance = (nearestPlayerPosition - (Vector2)transform.position).sqrMagnitude;

      if (sqrDistance < Mathf.Pow(approachDistance, 2) && currentLeaveCount < maxLeaveCount)
      {
        Leave();
      }
      else if (sqrDistance > Mathf.Pow(leaveDistance - 2, 2))
      {
        Approach();
      }
      else
      {
        if (Random.Range(0, 1f) > 0.65f)
        {
          WalkAndAttack();
        }
        else
        {
          if (Random.Range(0, 1f) < 0.5f || currentLeaveCount >= maxLeaveCount) Approach();
          else Leave();
        }
      }
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

      switch (devilStatus)
      {
        case DevilStatus.LEAVE:
          DoLeave(Time.fixedDeltaTime);
          break;
        case DevilStatus.APPROACH:
          DoApproch(Time.fixedDeltaTime);
          break;
        case DevilStatus.WALK_ATTACK:
          DoWalkAndAttack(Time.fixedDeltaTime);
          break;
      }
    }
  }
}