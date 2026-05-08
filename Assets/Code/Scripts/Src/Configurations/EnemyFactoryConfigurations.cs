using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Enemy;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Src.Configurations
{
  public enum EnemyGenerationTypes
  {
    TimePoint,
    TimeRange,
    QuantityMaintain,
  }

  public struct EnemyFactoryConfiguration
  {
    public EnemyGenerationTypes GeneratingType { get; set; }
    public EnemyGenerationConfigurations Configurations { get; set; }
  }

  public struct BossFightNormalEnemyConfiguration
  {
    public EnemyGenerationTypes GeneratingType { get; set; }
    public List<string> EnemyPrefabNames { get; set; }
    public int BaseStrength { get; set; }
  }

  public struct MonsterAttributeRatio
  {
    public float Physical { get; set; } // Percentage
    public float Ice { get; set; }
    public float Thunder { get; set; }
    public float Fire { get; set; }
    public float Poison { get; set; }
  }

  public struct LevelEnemyConfiguration
  {
    public string MapDescriptionName { get; set; }
    public MonsterAttributeRatio MonsterAttributeRatio { get; set; }
    public List<EnemyFactoryConfiguration> NormalEnemyFactoryConfigurations { get; set; }
    public List<EnemyFactoryConfiguration> BossFactoryConfigurations { get; set; }
    [CanBeNull] public List<EnemyFactoryConfiguration> InfiniteNormalEnemyFactoryConfigurations { get; set; }
    [CanBeNull] public List<EnemyFactoryConfiguration> InfiniteBossFactoryConfigurations { get; set; }
    public BossFightNormalEnemyConfiguration BossFightNormalEnemyConfiguration { get; set; }
  }

  public class LevelEnemies
  {
    public readonly List<EnemyNames>
      NormalEnemies = new(); // Each level should have at least 3 types of normal enemies

    public readonly List<EnemyNames> LittleBosses = new();
    public readonly List<EnemyNames> Bosses = new();
  }

  public static class EnemyFactoryConfigurations
  {
    private static readonly Dictionary<MapNames, LevelEnemies> levelEnemyConfigurations = new()
    {
      {
        MapNames.FOREST,
        new()
        {
          NormalEnemies =
            { EnemyNames.CUNNING_MOUSE, EnemyNames.FIRE_LAMPREY, EnemyNames.WASP },
          LittleBosses = { EnemyNames.ROCK_GIANT },
          Bosses = { EnemyNames.ICE_TIGER },
        }
      },
      {
        MapNames.DESERT,
        new()
        {
          NormalEnemies =
            { EnemyNames.LIGHTNING_DRAGON, EnemyNames.MUMMY, EnemyNames.SCORPION, EnemyNames.WASP },
          LittleBosses = { EnemyNames.CELL_MONSTER },
          Bosses = { EnemyNames.STELLARBOT },
        }
      },
      {
        MapNames.DUNGEON,
        new()
        {
          NormalEnemies =
          {
            EnemyNames.FIRE_LAMPREY, EnemyNames.LITTLE_DEMON, EnemyNames.SIREN,
            EnemyNames.SKELETON
          },
          LittleBosses = { EnemyNames.THUNDER_DEMON },
          Bosses = { EnemyNames.WEREWOLF },
        }
      },
      {
        MapNames.GRAVEYARD,
        new()
        {
          NormalEnemies =
            { EnemyNames.FLEA, EnemyNames.LIGHTNING_DRAGON, EnemyNames.SCORPION, EnemyNames.SKELETON },
          LittleBosses = { EnemyNames.WITCH },
          Bosses = { EnemyNames.SPECTER },
        }
      },
      {
        MapNames.HELL,
        new()
        {
          NormalEnemies =
          {
            EnemyNames.CUNNING_MOUSE, EnemyNames.FIRE_LAMPREY, EnemyNames.LITTLE_DEMON,
            EnemyNames.MUMMY, EnemyNames.SIREN,
          },
          LittleBosses = { EnemyNames.CANNON_DRAGON },
          Bosses = { EnemyNames.DEVIL },
        }
      }
    };

    private static List<EnemyFactoryConfiguration> GenerateNormalEnemyConfigurations(int minutes, MapNames mapName,
      int levelDifficulty, float difficultyToStrengthCoefficient, bool harderFirst)
    {
      var enemyConfigs = new List<EnemyFactoryConfiguration>();
      var enemyCandidates = levelEnemyConfigurations[mapName];

      var wavesPerMinute = Random.Range(1.5f, 3f);
      var secondsPerWave = 60 / wavesPerMinute;
      var numberOfSets = Mathf.RoundToInt(wavesPerMinute * minutes);
      var timePoints = Enumerable.Range(0, numberOfSets)
        .Select((_, index) =>
          index == 0 ? 0 : Mathf.RoundToInt(index * secondsPerWave + secondsPerWave * Random.Range(-0.35f, 0.35f))
        )
        .ToList();
      timePoints.Sort();

      for (var i = 0; i < timePoints.Count; i++)
      {
        var numberOfTypes = Random.Range(1, 3);
        enemyCandidates.NormalEnemies.Shuffle();
        var enemyNames = enemyCandidates.NormalEnemies.Where((_, index) => index < numberOfTypes).ToList();

        int startPoint = timePoints[i],
          endPoint = i + 1 < timePoints.Count ? timePoints[i + 1] : minutes * 60,
          averageTimePoint = i + 1 < timePoints.Count ? (startPoint + endPoint) / 2 : minutes * 60;

        var difficulty = GeneralConfigurations.GetEnemyDifficultyToTimePoint(averageTimePoint, levelDifficulty);
        var enemySets = enemyNames
          .Select(enemy =>
            new EnemyGenerationSetting
            {
              EnemyPrefabName = EnemyConfigurations.Normal[enemy].EnemyPrefabName,
              EnemyGeneratingCoefficient = Random.Range(1, 4),
              Strength = GeneralConfigurations.GetStrengthByDifficulty(difficulty, difficultyToStrengthCoefficient,
                harderFirst),
            }
          ).ToList();

        EnemyFactoryConfiguration enemyConfig;
        if (Random.Range(0, 100) < 65 ||
            i == 0) // Should use quantity maintain setting at the beginning to make the game more interesting
        {
          // Maintain enemy quantity
          enemyConfig = new EnemyFactoryConfiguration
          {
            GeneratingType = EnemyGenerationTypes.QuantityMaintain,
            Configurations = new QuantityMaintainingConfig
            {
              TimePeriod = new TimePeriod
              {
                Start = startPoint,
                End = endPoint,
              },
              Quantity = GeneralConfigurations.GetEnemyGenerationMaintainQuantity(averageTimePoint),
              EnemyGenerationSets = enemySets
            }
          };
        }
        else
        {
          var enemyPerSecond = GeneralConfigurations.GetEnemyGenerationPerSecond(averageTimePoint);
          if (enemyPerSecond < 1)
          {
            enemyPerSecond = Random.Range(0, 1f) < enemyPerSecond ? 1 : 0;
          }

          // Generate enemy every second
          enemyConfig = new EnemyFactoryConfiguration
          {
            GeneratingType = EnemyGenerationTypes.TimeRange,
            Configurations = new TimeRangeConfig
            {
              TimePeriod = new()
              {
                Start = startPoint,
                End = endPoint,
              },
              EnemiesPerSecond = enemyPerSecond,
              EnemyGenerationSets = enemySets
            }
          };
        }

        enemyConfigs.Add(enemyConfig);
      }

      return enemyConfigs;
    }

    private static List<EnemyFactoryConfiguration> GenerateBossesConfigurations(int minutesToBoss, MapNames mapName,
      int levelDifficulty, float difficultyToStrengthCoefficient, bool harderFirst)
    {
      var enemyConfigs = new List<EnemyFactoryConfiguration>();

      var levelEnemies = levelEnemyConfigurations[mapName];
      var littleBoss =
        EnemyConfigurations.Boss[levelEnemies.LittleBosses[Random.Range(0, levelEnemies.LittleBosses.Count)]];
      var boss = EnemyConfigurations.Boss[levelEnemies.Bosses[Random.Range(0, levelEnemies.Bosses.Count)]];

      var littleBossTimePoint = minutesToBoss * 60 / 2;
      var littleBossQuantity = Random.Range(0, 100) < 70 ? 1 : 2;
      var multipleLittleBossStrengthDecrement =
        (littleBossQuantity - 1) * GeneralConfigurations.MultiBossDifficultyDecrement;
      var littleBossDifficulty =
        GeneralConfigurations.GetEnemyDifficultyToTimePoint(littleBossTimePoint, levelDifficulty);
      enemyConfigs.Add(new EnemyFactoryConfiguration
      {
        GeneratingType = EnemyGenerationTypes.TimePoint,
        Configurations = new TimePointConfig
        {
          TimePoint = littleBossTimePoint,
          Quantity = littleBossQuantity,
          EnemyGenerationSets = new List<EnemyGenerationSetting>
          {
            new()
            {
              EnemyPrefabName = littleBoss.EnemyPrefabName,
              Strength = GeneralConfigurations.GetStrengthByDifficulty(littleBossDifficulty,
                           difficultyToStrengthCoefficient, harderFirst) -
                         multipleLittleBossStrengthDecrement
            }
          }
        }
      });

      var bigBossQuantity = Random.Range(0, 100) < 85 ? 1 : 2;
      var multipleBigBossStrengthDecrement =
        (bigBossQuantity - 1) * GeneralConfigurations.MultiBossDifficultyDecrement;
      var bigBossTimePoint = minutesToBoss * 60;
      var bigBossDifficulty = GeneralConfigurations.GetEnemyDifficultyToTimePoint(bigBossTimePoint, levelDifficulty);
      enemyConfigs.Add(new EnemyFactoryConfiguration
      {
        GeneratingType = EnemyGenerationTypes.TimePoint,
        Configurations = new TimePointConfig
        {
          TimePoint = bigBossTimePoint,
          Quantity = bigBossQuantity,
          EnemyGenerationSets = new()
          {
            new()
            {
              EnemyPrefabName = boss.EnemyPrefabName,
              Strength = GeneralConfigurations.GetStrengthByDifficulty(bigBossDifficulty,
                           difficultyToStrengthCoefficient, harderFirst) -
                         multipleBigBossStrengthDecrement
            }
          }
        }
      });

      return enemyConfigs;
    }

    // Enemy Configs contains 2 parts: Normal (Casual/Normal) play mode, infinite playmode.
    // If the player chooses infinite playmode, then the EnemyFactory should use the infinite configurations after normal enemies are done (AKA: After 10/15 minutes + a certain time).
    public static LevelEnemyConfiguration GenerateEnemyConfiguration(PlayModes playMode, MapNames mapName,
      int levelDifficulty)
    {
      var configs = new LevelEnemyConfiguration
      {
        MapDescriptionName = mapName switch
        {
          MapNames.FOREST => I18nUtils.GetText("MapDescription/Forest"),
          MapNames.DESERT => I18nUtils.GetText("MapDescription/Desert"),
          MapNames.DUNGEON => I18nUtils.GetText("MapDescription/Dungeon"),
          MapNames.GRAVEYARD => I18nUtils.GetText("MapDescription/Graveyard"),
          MapNames.HELL => I18nUtils.GetText("MapDescription/Hell"),
          _ => ""
        }
      };

      var minutesToBoss = GeneralConfigurations.GetMinutesToBoss(playMode);

      var difficultyToStrengthCoefficient = Random.Range(0.75f, 1.25f);
      var harderFirst = Random.Range(0, 3) == 0;

      // Generate normal enemies
      configs.NormalEnemyFactoryConfigurations = GenerateNormalEnemyConfigurations(minutesToBoss, mapName,
        levelDifficulty, difficultyToStrengthCoefficient,
        harderFirst);

      // Generate bosses
      configs.BossFactoryConfigurations = GenerateBossesConfigurations(
        DebugConfigurations.DebugEnabled ? 1 : minutesToBoss, mapName, levelDifficulty,
        difficultyToStrengthCoefficient, harderFirst);

      #region Generate boss fight enemies

      var normalEnemies = levelEnemyConfigurations[mapName].NormalEnemies;
      var enemyPrefabNames = normalEnemies
        .Select(enemy =>
          EnemyConfigurations.Normal[enemy].EnemyPrefabName
        )
        .ToList();

      var enemyGenerationTypes = new List<EnemyGenerationTypes>
        { EnemyGenerationTypes.QuantityMaintain, EnemyGenerationTypes.TimeRange };
      configs.BossFightNormalEnemyConfiguration = new BossFightNormalEnemyConfiguration
      {
        // BaseStrength is levelDifficulty since there will be 2 waves of boss that should let EnemyFactory to determine its base strength
        BaseStrength = levelDifficulty,
        EnemyPrefabNames = enemyPrefabNames,
        GeneratingType = enemyGenerationTypes[Random.Range(0, enemyGenerationTypes.Count)],
      };

      #endregion

      #region Generate infinite Enemies

      if (playMode == PlayModes.INFINITE)
      {
        // Generate normal enemies for infinite mode
        configs.InfiniteNormalEnemyFactoryConfigurations =
          GenerateNormalEnemyConfigurations(GeneralConfigurations.InfiniteEnemyGenerationLoopMinutes, mapName,
            levelDifficulty, difficultyToStrengthCoefficient, harderFirst);

        // Generate bosses for infinite mode
        configs.InfiniteBossFactoryConfigurations =
          GenerateBossesConfigurations(minutesToBoss, mapName, levelDifficulty, difficultyToStrengthCoefficient,
            harderFirst);
      }

      #endregion

      #region Calculate level property ratio

      var ratio = new MonsterAttributeRatio
      {
        Physical = 0,
        Ice = 0,
        Fire = 0,
        Thunder = 0,
        Poison = 0,
      };
      var total = 0;

      configs.NormalEnemyFactoryConfigurations.ForEach(enemyConfig =>
        enemyConfig.Configurations.EnemyGenerationSets.ForEach(
          set =>
          {
            var found = EnemyConfigurations.Normal.Values.ToList()
              .Find(ec => ec.EnemyPrefabName == set.EnemyPrefabName);
            if (found == null) return;

            found.DefendTypes?.ForEach(type =>
            {
              switch (type)
              {
                case HurtTypes.PHYSICAL: ratio.Physical++; break;
                case HurtTypes.MAGIC_ICE: ratio.Ice++; break;
                case HurtTypes.MAGIC_FIRE: ratio.Fire++; break;
                case HurtTypes.MAGIC_THUNDER: ratio.Thunder++; break;
                case HurtTypes.POISON: ratio.Poison++; break;
              }

              total++;
            });
          }));

      ratio.Physical /= total;
      ratio.Ice /= total;
      ratio.Fire /= total;
      ratio.Thunder /= total;
      ratio.Poison /= total;

      configs.MonsterAttributeRatio = ratio;

      #endregion

      if (DebugConfigurations.DebugEnabled)
      {
        Debug.Log($"Strength curve coefficient: {difficultyToStrengthCoefficient}\nHarder first: {harderFirst}");
      }

      return configs;
    }
  }
}