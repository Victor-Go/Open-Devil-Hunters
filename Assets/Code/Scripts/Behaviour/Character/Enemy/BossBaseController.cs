using UnityEngine;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Behaviour.Character.Enemy
{
  public interface IEntityState
  {
    void Enter();
    void Execute(float dt);
    void Exit();
  }

  public abstract class BaseState<T> : IEntityState where T : BaseBossController
  {
    protected T boss;
    public BaseState(T boss) { this.boss = boss; }
    public virtual void Enter() {}
    public virtual void Execute(float dt) {}
    public virtual void Exit() {}
  }

  public class StateMachine
  {
    public IEntityState CurrentState { get; private set; }

    public void ChangeState(IEntityState newState)
    {
      CurrentState?.Exit();
      CurrentState = newState;
      CurrentState?.Enter();
    }

    public void Execute(float dt)
    {
      CurrentState?.Execute(dt);
    }
  }

  public abstract class BaseBossController : PoolableBoss
  {
    public StateMachine StateMachine { get; protected set; }
    protected Camera mainCamera;

    protected override void Awake()
    {
      base.Awake();
      StateMachine = new StateMachine();
      mainCamera = Camera.main;
    }

    protected override void FixedUpdate()
    {
      base.FixedUpdate();
      
      if (paused)
      {
        if (rb != null) rb.velocity = Vector2.zero;
        return;
      }

      if (stopHandlingMovement)
      {
        return;
      }

      StateMachine.Execute(Time.fixedDeltaTime);
    }

    public float GetSqrDistanceToPlayer()
    {
      var playerPosition = playerPositions[selectedPlayerNumber];
      return (playerPosition - (Vector2)transform.position).sqrMagnitude;
    }

    public Vector2 GetVectorToPlayer()
    {
      return playerPositions[selectedPlayerNumber] - (Vector2)transform.position;
    }

    public void SetAnimTrigger(string triggerName)
    {
      if (animator != null)
      {
        animator.SetTrigger(triggerName);
      }
    }

    public void ResetAnimTrigger(string triggerName)
    {
      if (animator != null)
      {
        animator.ResetTrigger(triggerName);
      }
    }

    protected override void SetAnimatorDirection(Vector2 direction)
    {
      base.SetAnimatorDirection(direction);
      if (direction.x > Code.Scripts.Src.Configurations.GeneralConfigurations.AnimationDirectionSettingThreshold)
      {
        SetAnimatorHorizontalPosition(AnimatorDirection.Right);
      }
      else if (direction.x < -Code.Scripts.Src.Configurations.GeneralConfigurations.AnimationDirectionSettingThreshold)
      {
        SetAnimatorHorizontalPosition(AnimatorDirection.Left);
      }
    }

    public virtual void SetAnimatorHorizontalPosition(AnimatorDirection animatorDirection)
    {
      // To be overridden by bosses that need to flip colliders
    }
  }
}
