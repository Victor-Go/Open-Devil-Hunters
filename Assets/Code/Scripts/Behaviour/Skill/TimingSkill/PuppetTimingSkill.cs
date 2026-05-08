using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.TimingSkill
{
  public class PuppetTimingSkill : Bullet
  {
    private float _detectOutOfCamera = 0.5f;
    private float _timeToUpdateUniqueId = 1;
    private float stuckTimer = 0;
    private Vector2 lastPosition;

    private float _speed;

    public override void SetSkillConfigurations(BasicSkillConfigurations skillConfigurations)
    {
      SkillConfigurations = skillConfigurations;
      _speed = SkillConfigurations.Speed * Random.Range(0.8f, 1.2f);
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);
      lastPosition = initialPosition;
      stuckTimer = 0;
    }

    public override void SetTargetRelativeDirection(Vector2 relativeDirection)
    {
      this.relativeDirection = relativeDirection.normalized;
      targetPosition = (Vector2)transform.position + relativeDirection;

      var ls = transform.localScale;

      transform.localScale = relativeDirection.x > 0
        ? new Vector3(Mathf.Abs(ls.x) * -1, ls.y, ls.z)
        : new Vector3(Mathf.Abs(ls.x), ls.y, ls.z);
    }

    protected override void FixedUpdate()
    {
      if (paused)
      {
        return;
      }

      var dt = Time.fixedDeltaTime;

      if ((lastPosition - rb.position).sqrMagnitude / dt <= 0.01f)
      {
        stuckTimer += dt;
        if (stuckTimer > 1)
        {
          RecycleBullet();
        }
      }
      else
      {
        stuckTimer = 0;
      }

      lastPosition = rb.position;

      var distance = _speed * dt;
      var movement = distance * relativeDirection.normalized;

      _detectOutOfCamera -= dt;
      _timeToUpdateUniqueId -= dt;

      if (_detectOutOfCamera <= 0)
      {
        _detectOutOfCamera = 0.5f;
        if (!GeneralUtils.IsInCamera(transform.position, 3))
        {
          RecycleBullet();
        }
      }

      if (_timeToUpdateUniqueId <= 0)
      {
        _timeToUpdateUniqueId = 1;
        GenerateUniqueId();
      }

      rb.MovePosition(rb.position + movement);
    }
  }
}