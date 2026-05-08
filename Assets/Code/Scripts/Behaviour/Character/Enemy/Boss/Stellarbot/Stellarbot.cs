using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Steam.Achievements;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.Stellarbot
{
  public enum StellarbotStatus
  {
    Idle,
    Approach,
    MoveAndPunch,
    Prepare,
    Punch,
    Leave,
  }

  public class Stellarbot : PoolableBigBoss
  {
    [SerializeField] private float idleDuration = 3;
    [SerializeField] private Transform launchPosition;
    [SerializeField] private float maxLeaveDistance = 10;
    [SerializeField] private float keepDistance = 4;
    [SerializeField] private float prepareTimeout = 1.5f;
    [SerializeField] private float approachDistance = 2f;
    [SerializeField] private float punchDistance = 2f;
    [SerializeField] private float punchBiasY = 0.1f;
    [SerializeField] private float acceleration = 2f;

    private ParticleSystem _particleSystem;
    private StellarbotStatus _stellarbotStatus = StellarbotStatus.Idle;
    private float leaveSqrDistance;
    private float prepareCountdown;
    private AudioSource _audioSource;

    protected override void Awake()
    {
      base.Awake();

      _particleSystem = GetComponentInChildren<ParticleSystem>();
      animator = GetComponentInChildren<Animator>();
      _audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
      base.Start();

      SelectOnePlayerToChase();
      Approach();

      maxLeaveDistance *= Random.Range(0.75f, 1.25f);
    }

    public override void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      base.OnEnemyDied(playerNumber, hurtType);

      AchievementsManager.UnlockAchievement(Achievements.StellarbotKiller);
    }

    private void Idle()
    {
      _stellarbotStatus = StellarbotStatus.Idle;

      _particleSystem.Stop();
      animator.SetTrigger("Idle");
      animator.ResetTrigger("Attack");
      animator.ResetTrigger("Walk");

      _audioSource.Stop();
    }

    private float idleTime;

    private void DoIdle(float dt)
    {
      rb.velocity = Vector2.zero;

      idleTime += dt;
      if (idleTime >= idleDuration)
      {
        idleTime = 0;

        var playersCenter = GetPlayersCenterPosition();
        var sqrMagnitude = (playersCenter - rb.position).sqrMagnitude;
        if (sqrMagnitude > keepDistance * keepDistance)
        {
          Approach();
        }
        else
        {
          Leave();
        }
      }
    }

    private void Approach()
    {
      SelectOnePlayerToChase();

      _stellarbotStatus = StellarbotStatus.Approach;

      _particleSystem.Stop();
      animator.SetTrigger("Walk");
      animator.ResetTrigger("Attack");
      animator.ResetTrigger("Idle");

      _audioSource.Stop();
    }

    private float approachTime;

    private void DoApproach(float dt)
    {
      var currentPosition = (Vector2)transform.position;
      var selectedPlayerPosition = playerPositions[selectedPlayerNumber];
      var playerLeft = selectedPlayerPosition + Vector2.left * approachDistance;
      var playerRight = selectedPlayerPosition + Vector2.right * approachDistance;
      var targetPosition = (playerLeft - currentPosition).sqrMagnitude < (playerRight - currentPosition).sqrMagnitude
        ? playerLeft
        : playerRight;
      var vectorToTarget = targetPosition - currentPosition;

      if (Mathf.Abs(vectorToTarget.y) <= punchBiasY && vectorToTarget.sqrMagnitude <= punchDistance * punchDistance)
      {
        Prepare();
        approachTime = 0;
        return;
      }

      var updatedSpeed = updatedEnemyConfigurations.Speed;
      var speed = Mathf.Max(updatedSpeed, updatedSpeed * 2 * (1 - Mathf.Pow(acceleration, -approachTime)));
      speed = Mathf.Clamp(speed, 1, 2.5f);

      var sqrDistance = vectorToTarget.sqrMagnitude;
      var movement = sqrDistance > vectorToTarget.sqrMagnitude
        ? vectorToTarget
        : vectorToTarget.normalized * (dt * speed);

      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(movement);

      approachTime += dt; // For calculating speed
    }

    private bool multiPlayerLeaveClockWise;

    private void Leave()
    {
      _stellarbotStatus = StellarbotStatus.Leave;

      _particleSystem.Stop();
      animator.SetTrigger("Walk");
      animator.ResetTrigger("Attack");
      animator.ResetTrigger("Idle");

      multiPlayerLeaveClockWise = Random.Range(0, 2) == 0;

      _audioSource.Stop();
    }

    private void DoLeave(float dt)
    {
      var centerPosition = GetPlayersCenterPosition();
      if (leaveSqrDistance > maxLeaveDistance * maxLeaveDistance ||
          (rb.position - centerPosition).sqrMagnitude > 8 * 8)
        // If the distance between enemy and player is greater than 8, then stop.
      {
        leaveSqrDistance = 0;
        Idle();
        return;
      }

      Vector2 leaveDirection;
      if (numberOfPlayers <= 1)
      {
        leaveDirection = rb.position - playerPositions[0];
      }
      else
      {
        var direction = playerPositions[0] - playerPositions[1];
        var perpendicular = Vector2.Perpendicular(direction) * (multiPlayerLeaveClockWise ? 1 : -1);
        leaveDirection = perpendicular + centerPosition;
      }

      var movement = leaveDirection.normalized * (dt * updatedEnemyConfigurations.Speed * 1.5f);
      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(movement);
      leaveSqrDistance += movement.sqrMagnitude;
    }

    private void Prepare()
    {
      rb.velocity = Vector2.zero;

      SetAnimatorDirection(playerPositions[selectedPlayerNumber] - rb.position);

      _stellarbotStatus = StellarbotStatus.Prepare;

      _particleSystem.Play();
      animator.SetTrigger("Idle");
      animator.ResetTrigger("Attack");
      animator.ResetTrigger("Walk");

      _audioSource.Play();
    }

    private void DoPrepare(float dt)
    {
      rb.velocity = Vector2.zero;

      if (prepareCountdown <= 0)
      {
        prepareCountdown = prepareTimeout;

        punchCount = Random.Range(2, 5);
        punchesGoUp = Random.Range(0, 2) == 0;
        Punch();
        return;
      }

      prepareCountdown -= dt;
    }

    private int punchCount;
    private bool punchesGoUp;

    private void Punch()
    {
      rb.velocity = Vector2.zero;

      _stellarbotStatus = StellarbotStatus.Punch;

      _particleSystem.Stop();
      animator.SetTrigger("Attack");
      animator.ResetTrigger("Idle");
      animator.ResetTrigger("Walk");

      _audioSource.Stop();
    }

    public void OnPunchAnimationFinished()
    {
      _particleSystem.Stop();
      DoPunch();
    }

    private void DoPunch()
    {
      if (punchCount > 0)
      {
        punchCount--;
        MoveAndPunch(rb.position + (punchesGoUp ? Vector2.up * 2 : Vector2.down * 2));
      }
      else
      {
        Idle();
      }

      animator.ResetTrigger("Attack");

      var fist = objectPool.GetObject("Enemy/Boss/Stellarbot/StellarbotPunch", launchPosition.position);
      fist.transform.SetParent(null);

      var fistController = fist.GetComponent<StellarbotPunch>();
      fistController.SetDirection(GetAnimatorDirection() == AnimatorDirection.Left
        ? StellarbotPunchDirection.Left
        : StellarbotPunchDirection.Right);
      fistController.SetConfigurations(SkillPresets.BossSkills["StellarbotPunch"].SkillConfigurations);
      fistController.SetHostStrength(totalStrength);
    }

    private Vector2 movePosition;

    private void MoveAndPunch(Vector2 position)
    {
      movePosition = position;

      _stellarbotStatus = StellarbotStatus.MoveAndPunch;
      _particleSystem.Stop();
      animator.SetTrigger("Walk");
      animator.ResetTrigger("Attack");
      animator.ResetTrigger("Idle");

      _audioSource.Stop();
    }

    private void DoMoveAndPunch(float dt)
    {
      var vectorToTarget = movePosition - rb.position;
      if (Mathf.Abs(vectorToTarget.y) <= punchBiasY && vectorToTarget.sqrMagnitude <= punchDistance * punchDistance)
      {
        Punch();
        return;
      }

      var updatedSpeed = Mathf.Clamp(updatedEnemyConfigurations.Speed * 3f, 0.5f, 5f);

      var sqrDistance = vectorToTarget.sqrMagnitude;
      var movement = sqrDistance > vectorToTarget.sqrMagnitude
        ? vectorToTarget
        : vectorToTarget.normalized * (dt * updatedSpeed);

      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(movement);
    }

    protected override void FixedUpdate()
    {
      if (stopHandlingMovement || paused)
      {
        rb.velocity = Vector2.zero;
        return;
      }

      switch (_stellarbotStatus)
      {
        case StellarbotStatus.Idle: DoIdle(Time.fixedDeltaTime); break;
        case StellarbotStatus.Approach: DoApproach(Time.fixedDeltaTime); break;
        case StellarbotStatus.Leave: DoLeave(Time.fixedDeltaTime); break;
        case StellarbotStatus.Prepare: DoPrepare(Time.fixedDeltaTime); break;
        case StellarbotStatus.MoveAndPunch: DoMoveAndPunch(Time.fixedDeltaTime); break;
      }
    }
  }
}