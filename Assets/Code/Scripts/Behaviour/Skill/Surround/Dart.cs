using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Surround
{
  public class Dart : PlayerBasicSkill
  {
    public float rotationSpeed = -720;

    private Scheduling scheduling;
    private string resetUuidScheduleId;
    private bool zooming = true;

    protected override void Awake()
    {
      base.Awake();

      scheduling = Scheduling.Instance;
      transform.localScale = Vector2.zero;
    }

    protected override void Start()
    {
      base.Start();

      resetUuidScheduleId = scheduling.SetInterval(() => { GenerateUniqueId(); }, 1);
    }

    private void Update()
    {
      if (paused) return;

      var dt = Time.deltaTime;

      if (zooming)
      {
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one, dt);
        if (((Vector2)transform.localScale - Vector2.one).sqrMagnitude <= 0.01f)
        {
          zooming = false;
        }
      }

      var rotation = transform.rotation;
      transform.rotation = rotation * Quaternion.AngleAxis(rotationSpeed * dt, Vector3.forward);
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      scheduling.ClearSchedule(resetUuidScheduleId);
    }
  }
}