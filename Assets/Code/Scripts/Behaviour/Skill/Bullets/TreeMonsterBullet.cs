using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill
{
  public class TreeMonsterBullet : EnemyBullet
  {
    private System.Func<float, float> df = x => Mathf.Sin(5 * x) / 50;
    private System.Func<float, float> rad = x => Mathf.Sin(5 * x);
    private float lifeTime;
    private Vector2 lastBasePosition;
    private GameObject trail;

    protected override void Awake()
    {
      base.Awake();

      trail = transform.Find("Trail").gameObject;
      trailRenderer = trail.GetComponent<TrailRenderer>();
    }

    protected override void Start()
    {
      base.Start();
      Init();
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);
      Init();

      trail.transform.SetParent(transform);
      trail.transform.localPosition = Vector2.zero;
      trailRenderer.Clear();
    }

    private void Init()
    {
      lifeTime = 0;
      lastBasePosition = transform.position;
    }

    private void FixedUpdate()
    {
      if (paused)
      {
        return;
      }

      var configs = (RangingEnemySkillConfigurations)SkillConfigurations;
      var speed = configs.Speed;

      var movement = direction * speed * Time.fixedDeltaTime;
      lifeTime += speed * Time.fixedDeltaTime;

      if (lifeTime > configs.Range)
      {
        trail.transform.SetParent(null);
        objectPool.Recycle(ObjectName, gameObject);
      }

      var yAxis = Vector2.Perpendicular(movement).normalized;

      var y = df(lifeTime);
      var tan = rad(lifeTime);

      transform.position = lastBasePosition + movement + yAxis * y;
      transform.rotation = Quaternion.Euler(0, 0,
        Mathf.Atan(tan) * Mathf.Rad2Deg + Vector2.SignedAngle(Vector2.right, movement));
      lastBasePosition = transform.position;
    }
  }
}