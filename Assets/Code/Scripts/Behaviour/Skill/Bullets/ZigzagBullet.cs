using System.Collections.Generic;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Bullets
{
  public class ZigzagBullet : Bullet
  {
    public float MinC = 0.1f;
    public float MaxC = 0.75f;

    public float MinD = 2f;
    public float MaxD = 5f;

    protected float c;
    protected float d;
    protected float dxk; // The k value of dx(x)

    protected Vector2 yAxis;
    protected float t;

    protected float speed;

    protected override void Awake()
    {
      base.Awake();

      c = Random.Range(MinC, MaxC);
      d = Random.Range(MinD, MaxD);
      dxk = d / c;
    }

    private float f(float t)
    {
      var dx = dxk * t;

      var a = c * (-1 + 2 * (Mathf.Floor(dx) % 2));
      var b = -c * (Mathf.Floor(dx) % 2);

      return (dx - Mathf.Floor(dx)) * a + b + c / 2;
    }

    public override void SetSkillConfigurations(BasicSkillConfigurations skillConfigurations)
    {
      base.SetSkillConfigurations(skillConfigurations);
      speed = SkillConfigurations.Speed;
    }

    public override void SetTargetRelativeDirection(Vector2 relativeDirection)
    {
      base.SetTargetRelativeDirection(relativeDirection);

      yAxis = Vector2.Perpendicular(relativeDirection).normalized;
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      t = 0;

      c = Random.Range(MinC, MaxC);
      d = Random.Range(MinD, MaxD);
      dxk = d / c;
    }

    protected override void FixedUpdate()
    {
      if (paused)
      {
        return;
      }

      var dt = Time.fixedDeltaTime;
      t += dt;

      var distance = dt * speed;
      var movement = distance * relativeDirection.normalized;

      rangeElapsed += distance;
      if (rangeElapsed >= ((BasicSkillConfigurations)SkillConfigurations).Range)
      {
        RecycleBullet();
        return;
      }

      var y = f(t);
      var combinedMovement = rb.position + movement + yAxis * y;
      transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.right, rb.position - combinedMovement));
      rb.MovePosition(combinedMovement);
    }
  }
}