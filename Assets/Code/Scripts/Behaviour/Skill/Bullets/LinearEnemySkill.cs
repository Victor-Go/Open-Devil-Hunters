using System;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Bullets
{
  public class LinearEnemySkill : EnemySkill
  {
    public bool ChangeDirection;

    protected Func<float, float> df;

    protected float lifeTime;
    protected Vector2 direction;
    protected Vector2 yAxis;

    protected float speed;

    protected GameObject trail;

    protected override void Awake()
    {
      base.Awake();

      var skillConfigs =
        (LinearEnemySkillConfigurations)SkillPresets.BossSkills[SkillPresetName].SkillConfigurations;

      speed = skillConfigs.Speed;

      SetConfigurations(skillConfigs);
      df = skillConfigs.df;

      trail = transform.Find("Trail")?.gameObject;
    }

    /**
   * Be careful, direction means the vector of direction rather than the vector to target.
   */
    public LinearEnemySkill SetDirection(Vector2 direction)
    {
      this.direction = direction;
      yAxis = Vector2.Perpendicular(direction).normalized;

      if (ChangeDirection)
      {
        var eulerAngles = transform.rotation.eulerAngles;
        var angle = Vector2.SignedAngle(Vector2.left, direction);
        eulerAngles.z = angle;
        transform.rotation = Quaternion.Euler(eulerAngles);
      }
      else
      {
        if (direction.x < 0) transform.localScale = new Vector2(1, 1);
        else if (direction.x > 0) transform.localScale = new Vector2(-1, 1);
      }

      return this;
    }

    public LinearEnemySkill MovePosition(Vector2 position)
    {
      rb.MovePosition(position);
      return this;
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      lifeTime = 0;

      if (trail != null)
      {
        trail.SetActive(true);
      }
      
      if (trailRenderer != null)
      {
        trailRenderer.Clear();
      }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer))
      {
        return;
      }

      objectPool.Recycle(ObjectName, gameObject);
    }

    private void FixedUpdate()
    {
      if (paused) return;

      var dt = Time.fixedDeltaTime;
      lifeTime += dt;

      if (!GeneralUtils.IsInCamera(transform.position, 1))
      {
        if (trail != null)
        {
          trail.SetActive(false);
        }

        if (trailRenderer)
        {
          trailRenderer.Clear();
        }

        objectPool.Recycle(ObjectName, gameObject);
      }

      var y = df(lifeTime) * dt;
      var movement = dt * speed * direction.normalized;

      MovePosition(rb.position + movement + yAxis * y);
    }
  }
}