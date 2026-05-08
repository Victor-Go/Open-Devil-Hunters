using System.Collections.Generic;
using Code.Scripts.Behaviour.Character.Enemy;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Behaviour.Skill.Bullets;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Steam.Achievements;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.LittleBoss.ThunderDemon
{


  public class ThunderDemon : PoolableLittleBoss
  {
    public Transform LaunchPosition;

    private float approachDistance { get; } = 4;
    private float waitForAttackTimeout { get; } = 2;
    private float restAfterAttackTimeout { get; } = 4;
    private int skillCount { get; } = 9;

    private CapsuleCollider2D movementCollider;
    private CapsuleCollider2D hitCollider;
    private CapsuleCollider2D hurtCollider;
    private Transform launchTransform;

    private float symmetryAxisOffset;

    private float
      symmetryInitColliderOffset; // Since collider is set to animator with offset at the beginning, thus it should be handled differently by animator and hit/hurt node.

    private float symmetryLaunchPositionOffset;
    private float currentApprochDistance;
    private Vector2 targetPosition;
    private long statusTransitionScheduleId;
    private List<GameObject> aims = new();
    private AudioClip[] attackClips;

    private static readonly string[] attackClipNames = new[]
    { 
      "Audio/Sound/Enemy/Boss/thunder-demon_skill_0", 
      "Audio/Sound/Enemy/Boss/thunder-demon_skill_1" 
    };

    private BossIdleState idleState;
    private WalkState walkState;
    private AttackState attackState;
    private System.Action attackAction;
    private System.Action walkAction;

    protected override void Awake()
    {
      base.Awake();

      movementCollider = GetComponent<CapsuleCollider2D>();
      hitCollider = transform.Find("Hit").GetComponent<CapsuleCollider2D>();
      hurtCollider = transform.Find("Hurt").GetComponent<CapsuleCollider2D>();
      launchTransform = transform.Find("LaunchPosition");

      animatorTransform = transform.Find("Animator");
      animator = animatorTransform.GetComponent<Animator>();

      var symmetryX = transform.Find("Symmetry").localPosition.x;
      symmetryAxisOffset = animatorTransform.localPosition.x - symmetryX;
      symmetryInitColliderOffset = movementCollider.offset.x - symmetryX;
      symmetryLaunchPositionOffset = launchTransform.localPosition.x;
      SetAnimatorHorizontalPosition(AnimatorDirection.Left);

      var attackClipNames = new[]
        { "Audio/Sound/Enemy/Boss/thunder-demon_skill_0", "Audio/Sound/Enemy/Boss/thunder-demon_skill_1" };
      attackClips = new AudioClip[attackClipNames.Length];
      var i = 0;
      foreach (var clipName in attackClipNames)
      {
        attackClips[i++] = Resources.Load<AudioClip>(clipName);
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

      SelectOnePlayerToChase();
      Walk();

      for (var i = 0; i < skillCount; i++)
      {
        var aim = objectPool.GetObject("Enemy/EnemyLineAim");
        aim.transform.SetParent(null);
        aim.GetComponent<EnemyLineAim>().SetLength(1);
        aim.SetActive(false);

        aims.Add(aim);
      }
    }

    public override void SetAnimatorHorizontalPosition(AnimatorDirection animatorDirection)
    {
      var animPos = animatorTransform.localPosition;
      var collPos = movementCollider.offset;
      var hitPos = hitCollider.offset;
      var hurtPos = hurtCollider.offset;
      var launchPos = launchTransform.localPosition;

      switch (animatorDirection)
      {
        case AnimatorDirection.Left:
          animatorTransform.localPosition = new Vector2(symmetryAxisOffset, animPos.y);
          movementCollider.offset = new Vector2(symmetryInitColliderOffset, collPos.y);
          hitCollider.offset = new Vector2(symmetryInitColliderOffset, hitPos.y);
          hurtCollider.offset = new Vector2(symmetryInitColliderOffset, hurtPos.y);
          launchTransform.localPosition = new Vector2(symmetryLaunchPositionOffset, launchPos.y);
          break;
        case AnimatorDirection.Right:
          animatorTransform.localPosition = new Vector2(-symmetryAxisOffset, animPos.y);
          movementCollider.offset = new Vector2(-symmetryInitColliderOffset, collPos.y);
          hitCollider.offset = new Vector2(-symmetryInitColliderOffset, hitPos.y);
          hurtCollider.offset = new Vector2(-symmetryInitColliderOffset, hurtPos.y);
          launchTransform.localPosition = new Vector2(-symmetryLaunchPositionOffset, launchPos.y);
          break;
      }
    }

    private void Idle()
    {
      StateMachine.ChangeState(idleState);
    }

    private void Walk()
    {
      StateMachine.ChangeState(walkState);
    }

    private void Attack()
    {
      StateMachine.ChangeState(attackState);
    }

    private class WalkState : BossWalkState
    {
      public WalkState(BaseBossController boss) : base(boss) {}
      public override void Enter() 
      { 
        base.Enter();
        var bossCast = (ThunderDemon)boss;
        bossCast.currentApprochDistance = Random.Range(0.5f, 1.25f) * bossCast.approachDistance;
        bossCast.rb.bodyType = RigidbodyType2D.Dynamic;
      }
      public override void Execute(float dt) 
      {
        var bossCast = (ThunderDemon)boss;
        var playerPosition = bossCast.playerPositions[bossCast.selectedPlayerNumber];
        var movement = (playerPosition - (Vector2)bossCast.transform.position).normalized *
                       (dt * bossCast.updatedEnemyConfigurations.Speed);

        bossCast.rb.MovePosition(bossCast.rb.position + movement);
        bossCast.SetAnimatorDirection(movement);

        var distanceVec = playerPosition - bossCast.rb.position;
        if (distanceVec.sqrMagnitude < Mathf.Pow(bossCast.currentApprochDistance, 2))
        {
          bossCast.targetPosition = playerPosition;
          bossCast.SetAnimatorDirection(distanceVec);
          bossCast.Idle();

          bossCast.rb.bodyType = RigidbodyType2D.Kinematic; // Prevent position change
          bossCast.rb.velocity = Vector2.zero;
          bossCast.PlaceWarningAim();

          bossCast.scheduling.ClearSchedule(bossCast.statusTransitionScheduleId);
          bossCast.statusTransitionScheduleId = bossCast.scheduling.SetTimeout(bossCast.attackAction, bossCast.waitForAttackTimeout);
        }
      }
    }

    private class AttackState : BossAttackState
    {
      public AttackState(BaseBossController boss) : base(boss) {}
    }

    public void OnAttackAnimationFinished()
    {
      DoAttack();
    }

    private void DoAttack()
    {
      var clip = attackClips[Random.Range(0, attackClips.Length)];
      AudioWrapper.PlayClip(clip, transform.position);

      animator.ResetTrigger("Attack");

      var unified = (targetPosition - (Vector2)launchTransform.position).normalized;
      var angle = 360f / skillCount;
      for (var i = 0; i < skillCount; i++)
      {
        var skill = objectPool.GetObject("Enemy/LittleBoss/ThunderDemon/ThunderDemonSkill", launchTransform.position);
        var direction = Quaternion.AngleAxis(angle * i, Vector3.forward) * unified;

        skill.GetComponent<LinearEnemySkill>()
          .SetDirection(direction)
          .SetHostStrength(totalStrength);
      }

      Idle();
      SetWarningAimActive(false);

      scheduling.ClearSchedule(statusTransitionScheduleId);
      statusTransitionScheduleId = scheduling.SetTimeout(walkAction, restAfterAttackTimeout);
    }

    // Overridden by BaseBossController already

    // Place aim to warn player
    private void PlaceWarningAim()
    {
      var unified = (targetPosition - (Vector2)launchTransform.position).normalized;
      var initAngle = Vector2.SignedAngle(Vector2.right, unified);
      float angle = 360 / skillCount;
      for (int i = 0; i < aims.Count; i++)
      {
        var aim = aims[i];
        var pos = launchTransform.position;

        aim.transform.position = pos;
        aim.GetComponent<EnemyLineAim>().SetAngle(initAngle + angle * i);
        aim.SetActive(true);
      }
    }

    private void SetWarningAimActive(bool active = true)
    {
      foreach (var aim in aims) aim.SetActive(active);
    }

    // Walk logic moved to WalkState

    public override void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      base.OnEnemyDied(playerNumber, hurtType);

      SetWarningAimActive(false);
      AchievementsManager.UnlockAchievement(Achievements.ThunderDemonKiller);
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