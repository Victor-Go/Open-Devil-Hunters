using System;
using Code.Scripts.Src;
using Code.Scripts.Src.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Src.Types
{
  public abstract class EnvironmentMovableObject : PauseableGameObject, IPoolableGameObject
  {
    public float Speed;
    public float Life = 10;

    public string ObjectName { get; set; }

    protected ObjectPool objectPool;

    protected const float dfCoefficient = 1.25f;
    protected Func<float, float> df = Mathf.Sin;
    protected Func<float, float> rad = x => Mathf.Sin(5 * x) / 2;

    protected TrailRenderer trailRenderer;
    protected GameObject trail;
    protected Vector2 trailLocalPosition;

    protected Vector2 lastBasePosition;
    protected Vector2 direction;
    protected float lifeTime;

    protected override void Awake()
    {
      base.Awake();
      objectPool = ObjectPool.Instance;

      var trailTransform = transform.Find("Trail");
      if (trailTransform)
      {
        trail = trailTransform.gameObject;
        trailLocalPosition = trail.transform.localPosition;
        trailRenderer = trail.GetComponent<TrailRenderer>();
      }
    }

    protected override void Start()
    {
      base.Start();

      Init();
    }

    public virtual void ObjectReset(Vector2 initialPosition)
    {
      gameObject.SetActive(true);

      Init();
    }

    protected void SetPosition(Vector2 position)
    {
      transform.position = position;
      lastBasePosition = position;
    }

    protected void RecycleGameObject()
    {
      if (trail != null)
      {
        trail.transform.SetParent(null);
      }

      objectPool.Recycle(ObjectName, gameObject);
    }

    protected void Init()
    {
      lifeTime = 0;
      var topRightCornerWorldPosition = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
      var bottomLeftCornerWorldPosition = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
      var subVector = topRightCornerWorldPosition - bottomLeftCornerWorldPosition;

      var radius = subVector.magnitude / 2;
      var centerPoint = subVector / 2 + bottomLeftCornerWorldPosition;

      direction = (Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector2.right).normalized;

      var startPoint = centerPoint - radius * direction;
      SetPosition(startPoint);

      if (trail != null)
      {
        trail.transform.SetParent(transform);
        trail.transform.localPosition = trailLocalPosition;
        trailRenderer.Clear();
      }
    }

    protected virtual void FixedUpdate()
    {
      if (paused) return;

      var dt = Time.fixedDeltaTime;
      lifeTime += dt;

      Vector2 yAxis = Vector2.Perpendicular(direction).normalized;
      float y = dfCoefficient * df(lifeTime) * dt;
      float tan = rad(lifeTime);

      var movement = Time.fixedDeltaTime * Speed * direction;
      SetPosition(lastBasePosition + movement + yAxis * y);
      transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan(tan) * Mathf.Rad2Deg);

      if (lifeTime >= Life && !GeneralUtils.IsInCamera(transform.position, 1))
      {
        RecycleGameObject();
      }
    }
  }
}