using System.Linq;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.Werewolf
{
  public enum WolfStatus
  {
    IDLE,
    INIT_MOVE,
    LEAVE,
    CHASE,
  }

  public class Wolf : InvokedEnemy
  {
    private const float leaveDistance = 6;
    private const float chaseDistance = 5;
    private const float leaveWaitTimeout = 3;
    private const float chaseWaitTimeout = 2;

    private WolfStatus wolfStatus;
    private Vector2 leavePosition;
    private Vector2 initTargetPosition;
    private Vector2 chasePosition;
    private string statusTransitionScheduleId;

    private Vector2 lastPosition;
    private float stuckForSeconds;

    protected override void Awake()
    {
      base.Awake();

      animator = GetComponentInChildren<Animator>();
    }

    protected override void Start()
    {
      base.Start();

      SelectOnePlayerToChase();
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      SelectOnePlayerToChase();
    }

    public void SetInitTargetPosition(Vector2 initPosition)
    {
      initTargetPosition = initPosition;
      GoInitPosition();
    }

    private void Idle()
    {
      wolfStatus = WolfStatus.IDLE;
      animator.ResetTrigger("Walk");
      animator.SetTrigger("Idle");
    }

    private void GoInitPosition()
    {
      wolfStatus = WolfStatus.INIT_MOVE;
      animator.ResetTrigger("Idle");
      animator.SetTrigger("Walk");
    }

    private void DoGoInitPosition(float dt)
    {
      var movement = (initTargetPosition - (Vector2)transform.position).normalized *
                     (dt * updatedEnemyConfigurations.Speed);

      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(movement);

      var sqrDistance = ((Vector2)transform.position - initTargetPosition).sqrMagnitude;
      if (sqrDistance <= 0.1f)
      {
        Leave();
      }
    }

    private void Leave()
    {
      wolfStatus = WolfStatus.LEAVE;
      animator.ResetTrigger("Idle");
      animator.SetTrigger("Walk");

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

        scheduling.ClearSchedule(statusTransitionScheduleId);
        statusTransitionScheduleId = scheduling.SetTimeout(() => { Chase(); }, chaseWaitTimeout);
      }
    }

    private void Chase()
    {
      wolfStatus = WolfStatus.CHASE;
      animator.ResetTrigger("Idle");
      animator.SetTrigger("Walk");

      var playerPosition = playerPositions[selectedPlayerNumber];
      var vectorToPlayer = (playerPosition - (Vector2)transform.position).normalized;
      chasePosition = vectorToPlayer * chaseDistance + playerPosition;
    }

    private void DoChase(float dt)
    {
      var movement = (chasePosition - (Vector2)transform.position).normalized * (dt * updatedEnemyConfigurations.Speed);

      rb.MovePosition(rb.position + movement);
      SetAnimatorDirection(movement);

      var sqrDistance = ((Vector2)transform.position - chasePosition).sqrMagnitude;
      if (sqrDistance <= 0.1f)
      {
        Idle();

        scheduling.ClearSchedule(statusTransitionScheduleId);
        statusTransitionScheduleId = scheduling.SetTimeout(() => { Leave(); }, leaveWaitTimeout);
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

      switch (wolfStatus)
      {
        case WolfStatus.LEAVE:
          DoLeave(Time.fixedDeltaTime);
          break;
        case WolfStatus.CHASE:
          DoChase(Time.fixedDeltaTime);
          break;
        case WolfStatus.INIT_MOVE:
          DoGoInitPosition(Time.fixedDeltaTime);
          break;
      }

      if ((lastPosition - rb.position).sqrMagnitude <= 1)
      {
        stuckForSeconds += Time.fixedDeltaTime;

        if (stuckForSeconds >= 1)
        {
          stuckForSeconds = 0;
          Leave();
        }
      }

      lastPosition = rb.position;
    }
  }
}