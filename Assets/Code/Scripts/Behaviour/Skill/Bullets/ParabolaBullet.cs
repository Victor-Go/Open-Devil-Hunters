using Code.Scripts.Behaviour.Skill;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Bullets
{
  public enum ParabolaBulletDirection
  {
    LEFT,
    RIGHT,
  }

  public class ParabolaBullet : EnemySkill
  {
    public float MinHorizontalRange;
    public float MaxHorizontalRange;
    public float MinVerticalRange;
    public float MaxVerticalRange;

    private GameObject bullet;
    private ParabolaBulletEntity parabolaBulletEntity;

    private float t;

    private ParabolaBulletDirection movingDirection;
    private float m, n; // m: The 2nd x value when y = 0 in a parabola; n: The highest y of the equation.
    private System.Func<float, float, float, float> f = (x, m, n) => (-4 * n / (m * m)) * (x * x) + 4 * x * n / m;

    private GameObject trail;

    protected override void Awake()
    {
      base.Awake();
      animator = GetComponentInChildren<Animator>();
      parabolaBulletEntity = GetComponentInChildren<ParabolaBulletEntity>();
      bullet = transform.Find("Bullet").gameObject;
      trail = bullet.transform.Find("Trail").gameObject;
      trailRenderer = trail.GetComponent<TrailRenderer>();

      Init();
    }

    public override EnemySkill SetConfigurations(BaseEnemySkillConfigurations configs)
    {
      base.SetConfigurations(configs);
      parabolaBulletEntity.ParabolaBullet = this;
      parabolaBulletEntity.SetConfigurations(configs);
      return this;
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      bullet.transform.localPosition = Vector2.zero;
      trail.transform.SetParent(bullet.transform, true);
      trail.transform.localPosition = Vector2.zero;
      trailRenderer.Clear();

      Init();
    }

    private void Init()
    {
      t = 0;
      m = Random.Range(MinHorizontalRange, MaxHorizontalRange);
      n = Random.Range(MinVerticalRange, MaxVerticalRange);
      movingDirection =
        (ParabolaBulletDirection)System.Enum.GetValues(typeof(ParabolaBulletDirection)).GetValue(Random.Range(0, 2));
    }

    public void OnAnimationFinished()
    {
      trail.transform.SetParent(null);
      objectPool.Recycle(ObjectName, gameObject);
    }

    protected void FixedUpdate()
    {
      if (paused) return;

      var speed = SkillConfigurations.Speed;

      t += Time.fixedDeltaTime;

      float y = f(t * speed, m, n);
      if (y <= 0)
      {
        bullet.transform.localPosition = new Vector2(0, 0);
        animator.SetTrigger("Explode");
      }
      else
      {
        bullet.transform.localPosition = new Vector2(0, y);

        transform.position += (Vector3)(Time.fixedDeltaTime * speed *
                                        (movingDirection.Equals(ParabolaBulletDirection.LEFT)
                                          ? Vector2.left
                                          : Vector2.right));
      }
    }
  }
}