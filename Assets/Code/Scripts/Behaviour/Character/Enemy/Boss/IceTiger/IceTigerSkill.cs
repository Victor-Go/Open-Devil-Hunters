using Code.Scripts.Src;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy
{
  public class IceTigerSkill : EnemySkill
  {
    public float maintainSeconds;

    private Scheduling scheduling;
    private CircleCollider2D circleCollider;
    private string scheduleId;

    protected override void Awake()
    {
      base.Awake();

      scheduling = Scheduling.Instance;
      circleCollider = GetComponent<CircleCollider2D>();
    }

    protected override void Start()
    {
      base.Start();

      circleCollider.enabled = false;
    }

    public void OnAppearAnimationFinished()
    {
      circleCollider.enabled = true;

      scheduling.ClearSchedule(scheduleId);
      scheduleId = scheduling.SetTimeout(() => { animator.SetTrigger("Disappear"); }, maintainSeconds);
    }

    public void OnDisappearAnimationFinished()
    {
      objectPool.Recycle(ObjectName, gameObject);
      scheduling.ClearSchedule(scheduleId);

      animator.ResetTrigger("Disappear");
      animator.SetTrigger("Reset");
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();

      scheduling.ClearSchedule(scheduleId);
    }
  }
}