using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy
{
  public class BossIdleState : BaseState<BaseBossController>
  {
    public BossIdleState(BaseBossController boss) : base(boss) {}
    public override void Enter()
    {
      boss.ResetAnimTrigger("Walk");
      boss.ResetAnimTrigger("Attack");
      boss.SetAnimTrigger("Idle");
    }
  }

  public class BossWalkState : BaseState<BaseBossController>
  {
    public BossWalkState(BaseBossController boss) : base(boss) {}
    public override void Enter()
    {
      boss.ResetAnimTrigger("Idle");
      boss.ResetAnimTrigger("Attack");
      boss.SetAnimTrigger("Walk");
    }
  }

  public class BossAttackState : BaseState<BaseBossController>
  {
    public BossAttackState(BaseBossController boss) : base(boss) {}
    public override void Enter()
    {
      boss.ResetAnimTrigger("Idle");
      boss.ResetAnimTrigger("Walk");
      boss.SetAnimTrigger("Attack");
    }
  }
}
