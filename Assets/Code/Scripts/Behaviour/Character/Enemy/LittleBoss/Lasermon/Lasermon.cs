using System.Collections.Generic;
using Code.Scripts.Behaviour.Character.Enemy;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Steam.Achievements;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.LittleBoss.Lasermon
{


  public class Lasermon : PoolableLittleBoss
  {
    public Transform[] PurpleLaunchTransforms;
    public Transform GreenLaunchTransform;
    public float ApprochDistance = 1.5f;

    private float waitForAttackTimeout = 3f;
    private float attackTimeout = 5f;

    private BossIdleState idleState;
    private WalkState walkState;
    private PrepareAttackState prepareAttackState;
    private AttackState attackState;

    private static readonly int[][] purpleAngleRanges = new[]
    {
      new[] { 165, 135 },
      new[] { -165, -135 },
    };

    private Vector2[] initPurpleInitLaunchPositions;
    private Vector2 initGreenInitLaunchPosition;

    private List<GameObject> purpleGameObjects = new();
    private GameObject greenGameObject;

    private float currentApprochDistance;

    private string statusTransitionScheduleId;

    protected override void Awake()
    {
      base.Awake();

      animator = GetComponentInChildren<Animator>();

      initPurpleInitLaunchPositions = new Vector2[PurpleLaunchTransforms.Length];
      for (int i = 0; i < PurpleLaunchTransforms.Length; i++)
      {
        initPurpleInitLaunchPositions[i] = PurpleLaunchTransforms[i].localPosition;
      }

      initGreenInitLaunchPosition = GreenLaunchTransform.localPosition;

      idleState = new BossIdleState(this);
      walkState = new WalkState(this);
      prepareAttackState = new PrepareAttackState(this);
      attackState = new AttackState(this);
    }

    protected override void Start()
    {
      base.Start();

      SelectOnePlayerToChase();
      Walk();
    }

    public override void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      base.OnEnemyDied(playerNumber, hurtType);
      AchievementsManager.UnlockAchievement(Achievements.LasermonKiller);
    }

    private void Walk()
    {
      StateMachine.ChangeState(walkState);
    }

    private void Idle()
    {
      StateMachine.ChangeState(idleState);
    }

    public override void SetAnimatorHorizontalPosition(AnimatorDirection animatorDirection)
    {
      int x = animatorDirection == AnimatorDirection.Right ? -1 : 1;

      for (int i = 0; i < initPurpleInitLaunchPositions.Length; i++)
      {
        var purplePos = initPurpleInitLaunchPositions[i];
        purplePos.x *= x;
        PurpleLaunchTransforms[i].localPosition = purplePos;
      }

      var greenPos = initGreenInitLaunchPosition;
      greenPos.x *= x;
      GreenLaunchTransform.localPosition = greenPos;
    }

    private class WalkState : BossWalkState
    {
      public WalkState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        var bossCast = (Lasermon)boss;
        bossCast.currentApprochDistance = bossCast.ApprochDistance * Random.Range(1, 1.5f);
      }
      public override void Execute(float dt)
      {
        var bossCast = (Lasermon)boss;
        var playerPosition = bossCast.playerPositions[bossCast.selectedPlayerNumber];
        var movement = (playerPosition - (Vector2)bossCast.transform.position).normalized *
                       (dt * bossCast.updatedEnemyConfigurations.Speed);
        bossCast.rb.MovePosition(bossCast.rb.position + movement);
        bossCast.SetAnimatorDirection(movement);

        if ((bossCast.rb.position - playerPosition).sqrMagnitude < Mathf.Pow(bossCast.currentApprochDistance, 2) &&
            Mathf.Abs((bossCast.rb.position - playerPosition).y) < 1)
        {
          bossCast.SetAnimatorDirection(playerPosition - bossCast.rb.position);
          bossCast.Idle();

          bossCast.scheduling.ClearSchedule(bossCast.statusTransitionScheduleId);
          bossCast.statusTransitionScheduleId = bossCast.scheduling.SetTimeout(() => { bossCast.PrepareAttack(); }, bossCast.waitForAttackTimeout);
        }
      }
    }

    private void PrepareAttack()
    {
      StateMachine.ChangeState(prepareAttackState);
    }

    private class PrepareAttackState : BaseState<BaseBossController>
    {
      public PrepareAttackState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        boss.ResetAnimTrigger("Idle");
        boss.ResetAnimTrigger("Walk");
        boss.ResetAnimTrigger("Attack");
        boss.SetAnimTrigger("PrepareAttack");
      }
    }

    private void RecycleSkillGameObject()
    {
      foreach (var purple in purpleGameObjects)
      {
        objectPool.Recycle(purple);
      }

      purpleGameObjects.Clear();

      if (greenGameObject)
      {
        objectPool.Recycle(greenGameObject);
        greenGameObject = null;
      }
    }

    private void DoAttack()
    {
      animator.ResetTrigger("PrepareAttack");
      animator.SetTrigger("Attack");

      rb.bodyType = RigidbodyType2D.Kinematic;
      RecycleSkillGameObject();

      var greenAngle = 180;

      var greenLaunchPosition = GreenLaunchTransform.position;

      bool flip = animatorTransform.localScale.x < 0;
      if (flip)
      {
        greenAngle = 180 - greenAngle;
      }

      for (int i = 0; i < PurpleLaunchTransforms.Length; i++)
      {
        GameObject purple = objectPool.GetObject("Enemy/LittleBoss/Lasermon/LasermonSkillPurple");
        purple.transform.SetParent(null);

        var purpleLaunchPosition = PurpleLaunchTransforms[i].position;

        var angleRange = purpleAngleRanges[i];
        int angle0 = angleRange[0];
        int angle1 = angleRange[1];

        if (flip)
        {
          angle0 = 180 - angle0;
          angle1 = 180 - angle1;
        }

        purple.GetComponent<LasermonSkill>()
          .SetLength(10)
          .SetPivot(purpleLaunchPosition)
          .SetAngleRange(angle0, angle1)
          .SetSpeed(30);

        purpleGameObjects.Add(purple);
      }

      GameObject green = objectPool.GetObject("Enemy/LittleBoss/Lasermon/LasermonSkillGreen");
      green.transform.SetParent(null);

      green.GetComponent<LasermonSkill>()
        .SetLength(10)
        .SetPivot(greenLaunchPosition)
        .SetAngle(greenAngle);
      greenGameObject = green;
    }

    private void Attack()
    {
      StateMachine.ChangeState(attackState);
      DoAttack();

      scheduling.ClearSchedule(statusTransitionScheduleId);
      statusTransitionScheduleId = scheduling.SetTimeout(() =>
      {
        RecycleSkillGameObject();

        rb.bodyType = RigidbodyType2D.Dynamic;

        Walk();
      }, attackTimeout);
    }

    private class AttackState : BossAttackState
    {
      public AttackState(BaseBossController boss) : base(boss) {}
      public override void Enter()
      {
        base.Enter();
        boss.ResetAnimTrigger("PrepareAttack");
      }
    }

    public void OnPrepareAttackAnimationFinished()
    {
      Attack();
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

    protected override void OnDestroy()
    {
      scheduling.ClearSchedule(statusTransitionScheduleId);
    }
  }
}