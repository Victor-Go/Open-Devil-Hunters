using System.Linq;
using Code.Scripts.Behaviour.Character;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Steam.Achievements;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.LittleBoss.Specter
{


  public class Specter : PoolableBigBoss
  {
    private const float appearDistance = 1.5f;
    private const float idleTimeout = 1.5f;
    private const float suckDuration = 3;
    private const float wanderTime = 5;
    private const float disappearTime = 3;
    private const float attackDistance = 3;
    
    private SpecterIdleState idleState;
    private AppearingState appearingState;
    private WanderingState wanderingState;
    private DisappearingState disappearingState;

    private Transform launchTransform;
    private BoxCollider2D hitCollider;
    private BoxCollider2D hurtCollider;
    private PlayerHurtHandling currentConfinedPlayer;
    private float skillHitPoint;
    private string triggerPlayerNormalHitScheduleId;
    private string clearPlayerNormalHitScheduleId;

    private string statusTransitionScheduleId;

    private GameObject specterSkillAnimation;

    private AudioSource specterSuckAudio;

    // Wander
    private Vector2 wanderDirection;
    private Vector2 yAxis;
    private readonly System.Func<float, float> wanderVariationEquation = x => Mathf.Cos(2 * x);
    private float wanderTimeElapsed;
    
    private static readonly int PlayerEnvironmentColliderLayerMask = LayerMask.GetMask("PlayerEnvironmentCollider");

    protected override void Awake()
    {
      base.Awake();

      animatorTransform = transform.Find("Animator");
      animator = animatorTransform.GetComponent<Animator>();

      launchTransform = transform.Find("LaunchPosition");

      hitCollider = transform.Find("Hit").GetComponent<BoxCollider2D>();
      hurtCollider = transform.Find("Hurt").GetComponent<BoxCollider2D>();

      var configs = SkillPresets.BossSkills["SpecterSkill"].SkillConfigurations;
      var upgradeIncrement = configs.StrengthUpgrade.IncreaseHitPointCoefficient;
      var timePassed = storeManager.GetState<LevelTimePassedState>(StoreNames.LevelTimePassedStore).LevelTimePassed;
      skillHitPoint = configs.HitPoint * Mathf.Pow(1 + upgradeIncrement, Mathf.RoundToInt(timePassed / 30f));

      specterSuckAudio = GetComponent<AudioSource>();

      idleState = new SpecterIdleState(this);
      appearingState = new AppearingState(this);
      wanderingState = new WanderingState(this);
      disappearingState = new DisappearingState(this);
    }

    protected override void Start()
    {
      base.Start();

      SelectOnePlayerToChase();
      Appear();
    }

    public override void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      base.OnEnemyDied(playerNumber, hurtType);
      AchievementsManager.UnlockAchievement(Achievements.SpecterKiller);
    }

    protected override void SelectOnePlayerToChase()
    {
      if (numberOfPlayers == 1)
      {
        for (var i = 0; i < playerAlive.Length; i++)
        {
          if (playerAlive[i])
          {
            selectedPlayerNumber = i;
          }
        }
      }
      else
      {
        selectedPlayerNumber = Random.Range(0, numberOfPlayers);
      }
    }

    private void SetColliderStatus(bool status)
    {
      hitCollider.enabled = status;
      hurtCollider.enabled = status;
    }

    private void Appear()
    {
      gameObject.SetActive(true);

      SetColliderStatus(false);

      StateMachine.ChangeState(appearingState);

      var playerPosition = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore)
        .PlayerPositions[selectedPlayerNumber];
      float distance = Random.Range(1, 1.5f) * appearDistance;
      transform.position = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector2.right * distance +
                           (Vector3)playerPosition;
    }

    private void Disappear()
    {
      SetColliderStatus(false);

      StateMachine.ChangeState(disappearingState);
    }

    private static readonly Collider2D[] attackCollidersBuffer = new Collider2D[32];

    private void TryAttack()
    {
      SetColliderStatus(true);

      var count = Physics2D.OverlapCircleNonAlloc(launchTransform.position, attackDistance, attackCollidersBuffer, PlayerEnvironmentColliderLayerMask);

      if (count > 0)
      {
        animator.SetTrigger("Attack");
        specterSuckAudio.Play();

        var player = attackCollidersBuffer[Random.Range(0, count)];
        player.GetComponent<PlayerAction>().StopHandlingMovementAndAttackFor(suckDuration);
        currentConfinedPlayer = player.GetComponentInChildren<PlayerHurtHandling>();
        currentConfinedPlayer.TriggerNormalHit(skillHitPoint);

        // Skill animation
        specterSkillAnimation = objectPool.GetObject("Enemy/LittleBoss/Specter/SpecterSkill");
        var size = specterSkillAnimation.GetComponent<SpriteRenderer>().bounds.size;
        specterSkillAnimation.transform.SetParent(null);

        var direction = ((Vector2)player.transform.position - (Vector2)launchTransform.position).normalized;
        specterSkillAnimation.transform.position = (Vector2)launchTransform.position + direction * (size / 2).magnitude;
        specterSkillAnimation.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, direction));
        SetAnimatorDirection(direction);

        scheduling.ClearSchedule(triggerPlayerNormalHitScheduleId);
        scheduling.ClearSchedule(clearPlayerNormalHitScheduleId);

        triggerPlayerNormalHitScheduleId = scheduling.SetInterval(() =>
        {
          // Check again if player is in damage toRange.
          var countInner = Physics2D.OverlapCircleNonAlloc(launchTransform.position, attackDistance, attackCollidersBuffer, PlayerEnvironmentColliderLayerMask);

          for (int i = 0; i < countInner; i++)
          {
            var collider = attackCollidersBuffer[i];
            if (collider.GetComponentInChildren<PlayerHurtHandling>() == currentConfinedPlayer)
            {
              currentConfinedPlayer.TriggerNormalHit(skillHitPoint);
              return;
            }
          }

          scheduling.ClearSchedule(triggerPlayerNormalHitScheduleId);
          triggerPlayerNormalHitScheduleId = null;

          currentConfinedPlayer = null;

          objectPool.Recycle(specterSkillAnimation);
          Wander();
        }, 1);

        clearPlayerNormalHitScheduleId = scheduling.SetTimeout(() =>
        {
          specterSuckAudio.Stop();

          scheduling.ClearSchedule(triggerPlayerNormalHitScheduleId);
          triggerPlayerNormalHitScheduleId = null;

          currentConfinedPlayer = null;

          objectPool.Recycle(specterSkillAnimation);
          Wander();
        }, suckDuration + 0.1f);
      }
      else
      {
        Wander();
      }
    }

    private void Idle()
    {
      SetColliderStatus(true);
      StateMachine.ChangeState(idleState);
    }

    private void Wander()
    {
      SetColliderStatus(true);
      StateMachine.ChangeState(wanderingState);

      wanderDirection = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector2.right;
      yAxis = Vector2.Perpendicular(wanderDirection).normalized;
      wanderTimeElapsed = 0;
    }

    public void OnAppearAnimationFinished()
    {
      animator.ResetTrigger("Appear");

      Idle();

      scheduling.ClearSchedule(statusTransitionScheduleId);
      statusTransitionScheduleId = scheduling.SetTimeout(() => TryAttack(), idleTimeout);
    }

    public void OnDisappearAnimationFinished()
    {
      animator.ResetTrigger("Disappear");

      SelectOnePlayerToChase();
      gameObject.SetActive(false);

      statusTransitionScheduleId = scheduling.SetTimeout(() => Appear(), disappearTime);
    }

    private void DoWander(float dt)
    {
      var currentPosition = transform.position;
      var movement = yAxis * (wanderVariationEquation(wanderTimeElapsed) * dt) +
                     wanderDirection * (updatedEnemyConfigurations.Speed * dt);
      transform.position = (Vector2)currentPosition + movement;

      SetAnimatorDirection(movement);

      wanderTimeElapsed += dt;

      if (wanderTimeElapsed >= wanderTime)
      {
        Disappear();
        wanderTimeElapsed = 0;
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

      // BaseBossController handles state execution
    }

    private class SpecterIdleState : BaseState<BaseBossController>
    {
      public SpecterIdleState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        boss.ResetAnimTrigger("Attack");
        boss.ResetAnimTrigger("Appear");
        boss.ResetAnimTrigger("Disappear");
        boss.SetAnimTrigger("WalkIdle");
      }
    }

    private class WanderingState : BaseState<BaseBossController>
    {
      public WanderingState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        boss.ResetAnimTrigger("Attack");
        boss.ResetAnimTrigger("Appear");
        boss.ResetAnimTrigger("Disappear");
        boss.SetAnimTrigger("WalkIdle");
      }
      public override void Execute(float dt) { ((Specter)boss).DoWander(dt); }
    }

    private class AppearingState : BaseState<BaseBossController>
    {
      public AppearingState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        boss.ResetAnimTrigger("WalkIdle");
        boss.ResetAnimTrigger("Attack");
        boss.ResetAnimTrigger("Disappear");
        boss.SetAnimTrigger("Appear");
      }
    }

    private class DisappearingState : BaseState<BaseBossController>
    {
      public DisappearingState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        boss.ResetAnimTrigger("WalkIdle");
        boss.ResetAnimTrigger("Attack");
        boss.ResetAnimTrigger("Appear");
        boss.SetAnimTrigger("Disappear");
      }
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();

      scheduling.ClearSchedule(statusTransitionScheduleId);
      scheduling.ClearSchedule(triggerPlayerNormalHitScheduleId);
      scheduling.ClearSchedule(clearPlayerNormalHitScheduleId);
    }
  }
}