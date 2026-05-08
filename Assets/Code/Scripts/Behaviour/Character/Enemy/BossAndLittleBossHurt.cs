using Code.Scripts.Src;
using Code.Scripts.Behaviour.Animation;
using Code.Scripts.Behaviour.Character.Enemy.LittleBoss.Witch;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy
{
  public class BossAndLittleBossHurt : EnemyHurt
  {
    [SerializeField] private int numberOfGemsOnDied = 0;
    private Scheduling scheduling;
    private EnemyHpBar hpBar;
    private static readonly string[] poisonedList = new[] { "Status/Poisoned" };

    protected override void Awake()
    {
      base.Awake();
      scheduling = Scheduling.Instance;
      hpBar = transform.parent.GetComponentInChildren<EnemyHpBar>();
    }

    public override void EnemyReset()
    {
      base.EnemyReset();

      if (hpBar != null)
      {
        hpBar.SetPercentage(1);
      }
    }

    protected override void HandleHurt(int playerNumber, float hurtPoint, HurtTypes hurtType,
      bool playHurtAnimation = true)
    {
      base.HandleHurt(playerNumber, hurtPoint, hurtType, playHurtAnimation);

      if (hpBar != null)
      {
        hpBar.SetPercentage(HealthPoint / InitialHealthPoint);
      }
    }

    public override void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      base.OnEnemyDied(playerNumber, hurtType);

      if (numberOfGemsOnDied > 0)
      {
        var init = Random.Range(0, 360f);
        var degree = 360 / numberOfGemsOnDied;

        var bonus = Random.Range(0, 2);
        for (var i = 0; i < numberOfGemsOnDied + bonus; i++)
        {
          var gem = objectPool.GetObject("Environment/PickableGem");
          gem.transform.position = Quaternion.Euler(0f, 0f, degree * i + init) * Vector2.right + transform.position;
        }
      }
    }

    protected string witchResolvePoisonScheduleId;

    protected override void HandlePoisonAdditionalEffects(PoisonAdditionalEffect additionalEffect)
    {
      poisonedHurtPercentage = additionalEffect.HurtPercentagePerSecond / 10;
      rootController.OnEnemyPoisoned();
      if (rootController is Witch)
      {
        witchResolvePoisonScheduleId = scheduling.SetTimeout(() =>
        {
          isPoisoned = false;
          poisonedHurtPercentage = 0;
          ((Witch)rootController).OnEnemyPoisonResolved();
          poisonedAnimation.FadeOutAnimation();
        }, 3);
      }

      if (!isPoisoned)
      {
        isPoisoned = true;

        var poisoned = objectPool.GetObject(poisonedList[Random.Range(0, poisonedList.Length)]);
        poisoned.transform.SetParent(rootGameObject.transform);
        poisonedAnimation = poisoned.GetComponent<SimpleAnimation>();
        var size = rootController.GetSize();
        poisoned.transform.localPosition = new Vector2(size.x / 3, size.y / 2 + 0.1f);
      }
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();

      scheduling.ClearSchedule(witchResolvePoisonScheduleId);
    }
  }
}