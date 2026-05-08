using Code.Scripts.Behaviour.Skill;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Behaviour.Skill.ActiveSkill;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Src.Rune
{
  public class PushAwayEnemiesRune : BaseRune
  {
    private ObjectPool objectPool = ObjectPool.Instance;
    private Scheduling scheduling = Scheduling.Instance;

    private string resetAvailabilitySchedule;
    private bool available = true;

    private static readonly int EnemyLayerMask = LayerMask.GetMask("Enemy");
    private static readonly Collider2D[] overlapResults = new Collider2D[32];

    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .AddPlayerReloadAction((context) =>
        {
          if (!available) return context;

          // Animation
          GameObject pushAway = objectPool.GetObject("Status/PushAway");
          pushAway.transform.SetParent(null);
          pushAway.transform.position = context.PlayerPosition;

          // Add force
          var playerPosition = context.PlayerPosition;
          var range = 3;
          var sqrRange = range * range;
          var force = (2 + (level + 1) * 0.2f);

          int hitCount = Physics2D.OverlapCircleNonAlloc(playerPosition, range, overlapResults, EnemyLayerMask);
          for(int i = 0; i < hitCount; i++)
          {
            var enemy = overlapResults[i];
            var sqrMag = ((Vector2)enemy.transform.position - playerPosition).sqrMagnitude;
            enemy.GetComponent<Types.Enemy>().AddRepelForce(force * (1 - (sqrMag / sqrRange)),
              (Vector2)enemy.transform.position - playerPosition);
          }

          available = false;
          resetAvailabilitySchedule = scheduling.SetTimeout(() => available = true, 15 - level);

          return context;
        });
    }

    ~PushAwayEnemiesRune()
    {
      scheduling.ClearSchedule(resetAvailabilitySchedule);
      resetAvailabilitySchedule = null;
    }
  }

  public class InstantReloadRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .AddPlayerReloadAction((context) =>
        {
          int possibility = 10 + level * 5;
          bool instantReload = Random.Range(0, 100) < possibility;

          if (instantReload)
          {
            context.CoolingTime = 0;
            // FIXME: Add some cool effects.
          }

          return context;
        });
    }
  }

  public class IncreaseAttackPerRoundRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      var attackControlState = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore);
      var attackPerRound = attackControlState.AttackControlDatas[playerNumber].AttackControlConfigurations
        .AttackPerRound;

      storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_ATTACK_PER_ROUND,
        new AttackControlActionData()
        {
          PlayerNumber = playerNumber,
          AttackPerRound = attackPerRound + level + 1
        });
    }
  }

  public class ClonedProjectileRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      var skillConfigs = skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .BasicSkill;

      var skillCount = skillConfigs.SkillCount;
      skillCount += level;

      skillConfigs.SkillCount = skillCount;
    }
  }

  public class InstantKillRune : BaseRune
  {
    private ObjectPool objectPool;

    public InstantKillRune()
    {
      objectPool = ObjectPool.Instance;
    }

    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .AddEnemyDirectHurtAction((context) =>
        {
          var possibility = 0.01f + level * 0.005f;
          var instantKill = Random.Range(0, 1f) < possibility;

          if (instantKill && !context.IsBoss)
          {
            context.HitPoint = float.MaxValue;
            var enemyPosition = context.CurrentPosition;
            var ik = objectPool.GetObject("Status/InstantKill");
            ik.transform.SetParent(null);
            ik.transform.position = enemyPosition;
          }

          return context;
        });
    }
  }

  public class SkillFissionActiveSkillRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      var levelConfigs = StoreManager.Instance.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      if (levelConfigs.PlayerConfigurations[playerNumber].PlayerName == PlayerNames.JEANNE_D_ARC)
      {
        return;
      }

      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .SetActiveSkill(ActiveSkillPresets.Configurations["SkillFissionActiveSkill"],
          new SkillFissionActiveSkillConfigurations
          {
            PossibilityOfFission = 0.25f + level * 0.1f,
            Countdown = 90,
          });
    }
  }

  public class MeteoriteStrikeActiveSkillRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .SetActiveSkill(ActiveSkillPresets.Configurations["MeteoriteStrikeActiveSkill"],
          new MeteoriteStrikeActiveSkillConfigurations()
          {
            Countdown = 90,
            Duration = 10,
            Quantity = 10 + 5 * level,
          });
    }
  }

  public class ThunderStrikeActiveSkillRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .SetActiveSkill(ActiveSkillPresets.Configurations["ThunderStrikeActiveSkill"],
          new ThunderStrikeActiveSkillConfigurations()
          {
            Countdown = 90,
            Duration = 10,
            PercentageOfEnemies = 0.15f + level * 0.05f
          });
    }
  }

  public class HailStrikeActiveSkillRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .SetActiveSkill(ActiveSkillPresets.Configurations["HailStrikeActiveSkill"],
          new HailStrikeActiveSkillConfigurations()
          {
            Countdown = 90,
            Duration = 7 + 2 * level,
            Quantity = 10,
          });
    }
  }

  public class FullFieldPoisoningActiveSkillRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .SetActiveSkill(ActiveSkillPresets.Configurations["FullFieldPoisoningActiveSkill"],
          new PersistActiveSkillConfigurations()
          {
            Countdown = 90,
            Duration = 10 + 2 * level,
          });
    }
  }
}