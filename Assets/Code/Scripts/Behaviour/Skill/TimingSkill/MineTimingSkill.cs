using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.TimingSkill
{
  public class MineTimingSkill : PlayerBasicSkill
  {
    public string MineId { get; set; }

    private const float maximumLifetime = 90;

    private Scheduling scheduling;

    private float remainingLifeTime;
    private string triggerMineExplodeScheduleId;

    private static readonly int EnemyHurtLayerMask = LayerMask.GetMask("EnemyHurt");
    private static readonly int PlayerSkillTriggerLayerMask = LayerMask.GetMask("PlayerSkillTrigger");
    private static readonly Collider2D[] overlapCollidersBuffer = new Collider2D[64];

    protected override void Awake()
    {
      base.Awake();
      scheduling = Scheduling.Instance;
    }

    protected override void Start()
    {
      base.Start();
      remainingLifeTime = maximumLifetime;
      MineId = System.Guid.NewGuid().ToString();
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      remainingLifeTime = maximumLifetime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer))
      {
        return;
      }

      Explode();
    }

    public void Explode()
    {
      var explosion = objectPool.GetObject("Effect/MineExplosion");
      explosion.transform.SetParent(null);
      explosion.transform.position = transform.position;

      var range = ((BasicSkillConfigurations)SkillConfigurations).Range;

      var countEnemies = Physics2D.OverlapCircleNonAlloc(transform.position, range, overlapCollidersBuffer, EnemyHurtLayerMask);
      for (int i = 0; i < countEnemies; i++)
      {
        overlapCollidersBuffer[i].GetComponent<EnemyHurt>().TriggerEnemyHurt(PlayerNumber, this);
      }

      var countMines = Physics2D.OverlapCircleNonAlloc(transform.position, range, overlapCollidersBuffer, PlayerSkillTriggerLayerMask);
      var triggerMineList = new List<GameObject>();
      for (int i = 0; i < countMines; i++)
      {
        var mine = overlapCollidersBuffer[i];
        var mineController = mine.GetComponent<MineTimingSkill>();
        if (mineController != null)
        {
          triggerMineList.Add(mine.gameObject);
        }
      }

      if (triggerMineList.Count > 0)
      {
        triggerMineExplodeScheduleId = scheduling.SetTimeout(() =>
        {
          for (int i = 0; i < triggerMineList.Count; i++)
          {
            var mine = triggerMineList[i];
            if (mine.activeSelf)
            {
              mine.GetComponent<MineTimingSkill>().Explode();
            }
          }

          triggerMineList.Clear();
        }, 1f);

        AudioWrapper.PlayClip(resourceManager.GetResource("Audio/Sound/Skill/mine-triggered"), transform.position);
      }

      objectPool.Recycle(ObjectName, gameObject);
    }

    private void Update()
    {
      if (paused) return;

      remainingLifeTime -= Time.deltaTime;
      if (remainingLifeTime <= 0)
      {
        Explode();
      }
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      scheduling.ClearSchedule(triggerMineExplodeScheduleId);
    }
  }
}