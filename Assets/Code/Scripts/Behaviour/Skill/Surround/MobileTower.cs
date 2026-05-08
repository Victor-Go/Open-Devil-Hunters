using Code.Scripts.Behaviour.Skill;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Surround
{
  public abstract class MobileTower : ShootingSurroundSkill
  {
    public float FireInterval = 1.5f;
    public int BulletCount = 1;

    protected GameObject targetedEnemy;
    protected Transform firePosition;

    protected float timeSinceLastFire;
    protected float timeSinceLastAim;

    protected bool zooming = true;

    protected override void Awake()
    {
      base.Awake();

      firePosition = transform.Find("FirePosition");
    }

    protected override void Start()
    {
      base.Start();

      AimTarget();
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      transform.localScale = Vector2.zero;
      zooming = false;
    }

    protected abstract void Fire();

    protected virtual void AimTarget()
    {
      targetedEnemy = AttackUtils.GetNearestEnemyAtPosition(transform.position, 5);
    }

    protected void TurnTowerTowardTarget()
    {
      if (targetedEnemy != null)
      {
        var vec = (Vector2)(targetedEnemy.transform.position - transform.position);
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.down, vec));
      }
    }

    private void Update()
    {
      if (paused) return;

      var dt = Time.deltaTime;

      timeSinceLastFire += dt;
      timeSinceLastAim += dt;

      if (zooming)
      {
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one, dt);
        if (((Vector2)transform.localScale - Vector2.one).sqrMagnitude <= 0.01f)
        {
          zooming = false;
        }
      }

      if (timeSinceLastFire > FireInterval && !zooming)
      {
        timeSinceLastFire = 0;
        Fire();
      }

      if (timeSinceLastAim > 0.25f || targetedEnemy == null || !targetedEnemy.activeSelf)
      {
        AimTarget();
      }

      TurnTowerTowardTarget();
    }
  }
}