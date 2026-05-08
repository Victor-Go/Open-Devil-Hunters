using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Behaviour.Skill.ActiveSkill;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Src.Enemy
{
  public abstract class EnemyGenerationConfigurations
  {
    public List<EnemyGenerationSetting> EnemyGenerationSets { get; set; }
  }

  public abstract class QuantityBasedEnemyGenerationConfigurations : EnemyGenerationConfigurations
  {
    public int Quantity { get; set; }
  }

  public abstract class EnemyGenerator : IStoreChangedHandler
  {
    private readonly ObjectPool objectPool = ObjectPool.Instance;
    private bool poisonedOnSpawn;
    private int poisonedOnSpawnByPlayerNumber;
    private int lastEnemyQuantity;

    protected EnemyGenerationConfigurations generationConfigs { get; set; }

    protected EnemyGenerator()
    {
      var storeManager = StoreManager.Instance;

      storeManager
        .Subscribe(StoreNames.LevelStore, this)
        .Subscribe(StoreNames.LevelConfigurationStore, this)
        .Subscribe(StoreNames.LevelGeneralStore, this);
    }

    protected List<GameObject> GenerateEnemies(
      int enemiesToGenerate,
      int difficultyInfiniteModeLoopIncrement,
      int playerAlive,
      bool ignoreQuantityLimit = false
    )
    {
      var totalCoefficient = generationConfigs.EnemyGenerationSets.Sum(s => s.EnemyGeneratingCoefficient);
      List<GameObject> enemies = new();

      var enemyQuantity = ignoreQuantityLimit
        ? enemiesToGenerate
        : Mathf.Min(GeneralConfigurations.MaxEnemyOnScreen - lastEnemyQuantity, enemiesToGenerate);
      for (var i = 0; i < enemyQuantity; i++)
      {
        var random = Random.Range(0, totalCoefficient);
        var currentCoefficient = 0;

        var set = generationConfigs.EnemyGenerationSets;
        var selectedEnemySet = set[Random.Range(0, set.Count)];
        foreach (var enemySet in generationConfigs.EnemyGenerationSets)
        {
          if (currentCoefficient < random && random < enemySet.EnemyGeneratingCoefficient + currentCoefficient)
          {
            selectedEnemySet = enemySet;
            break;
          }

          currentCoefficient += enemySet.EnemyGeneratingCoefficient;
        }

        var enemyConfigs =
          EnemyConfigurations.Normal.Values.FirstOrDefault(config =>
            config.EnemyPrefabName == selectedEnemySet.EnemyPrefabName);
        var quantityToGenerate = 1;
        if (enemyConfigs != null)
        {
          quantityToGenerate = enemyConfigs.QuantityFactor;
        }

        for (var j = 0; j < quantityToGenerate; j++)
        {
          var enemy = objectPool.GetObject(selectedEnemySet.EnemyPrefabName);
          var enemyScript = enemy.GetComponent<Types.Enemy>();
          enemyScript.SetEnemyBaseStrength(selectedEnemySet.Strength + difficultyInfiniteModeLoopIncrement);

          if (poisonedOnSpawn)
          {
            FullFieldPoisoningActiveSkill.AddPoisonEffect(poisonedOnSpawnByPlayerNumber, enemyScript);
          }

          enemies.Add(enemy);
        }
      }

      return enemies;
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelStore:
          var levelState = (LevelState)state;
          var enemies = levelState.Enemies;
          lastEnemyQuantity = enemies.Count;
          break;
        case StoreNames.LevelGeneralStore:
          var levelGeneralState = (LevelGeneralState)state;
          poisonedOnSpawn = levelGeneralState.PlayLevelGeneral[0].PoisonedOnSpawn ||
                            levelGeneralState.PlayLevelGeneral[1].PoisonedOnSpawn;
          if (poisonedOnSpawn)
          {
            poisonedOnSpawnByPlayerNumber = levelGeneralState.PlayLevelGeneral[0].PoisonedOnSpawn ? 0 : 1;
          }

          break;
        case StoreNames.LevelConfigurationStore:
          var levelConfigurationState = (LevelConfigurationState)state;

          break;
        default: throw new InvalidStoreEventException();
      }
    }

    ~EnemyGenerator()
    {
      StoreManager.Instance.Unsubscribe(this);
    }
  }

  #region Quantity based

  public class QuantityMaintainingConfig : QuantityBasedEnemyGenerationConfigurations
  {
    public TimePeriod TimePeriod { get; set; }
  }

  public class QuantityMaintainingEnemyGenerator : EnemyGenerator
  {
    public QuantityMaintainingEnemyGenerator(QuantityMaintainingConfig quantityMaintainingConfig)
    {
      generationConfigs = quantityMaintainingConfig;
    }

    public List<GameObject> OnEnemyQuantityChanged(
      int secondsPassed,
      int currentEnemyQuantity,
      int difficultyInfiniteModeLoopIncrement,
      int playerAlive
    )
    {
      var quantityGenerationConfigs = (QuantityMaintainingConfig)generationConfigs;
      var timePeriod = quantityGenerationConfigs.TimePeriod;

      if (secondsPassed < timePeriod.Start || secondsPassed > timePeriod.End)
      {
        return new();
      }

      var minToGen = GeneralConfigurations.GetMinQuantityGenerationPerSecond(secondsPassed);
      if (minToGen < 1)
      {
        minToGen = Random.Range(0, 1f) < minToGen ? 1 : 0;
      }

      return GenerateEnemies(
        Mathf.Max(Mathf.RoundToInt(minToGen),
          quantityGenerationConfigs.Quantity - currentEnemyQuantity),
        difficultyInfiniteModeLoopIncrement, playerAlive);
    }
  }

  #endregion

  #region Time generators

  public abstract class TimeBasedGenerator : EnemyGenerator
  {
    protected bool ignoreQuantityLimit;

    public abstract List<GameObject> OnTimeChanged(int secondsPassed, int difficultyInfiniteModeLoopIncrement,
      int playerAlive);
  }

  #region Time point

  public class TimePointConfig : QuantityBasedEnemyGenerationConfigurations
  {
    public int TimePoint { get; set; }
  }

  public class TimePointEnemyGenerator : TimeBasedGenerator
  {
    public TimePointEnemyGenerator(TimePointConfig timePointConfig, bool ignoreQuantityLimit = false)
    {
      generationConfigs = timePointConfig;
      this.ignoreQuantityLimit = ignoreQuantityLimit;
    }

    public override List<GameObject> OnTimeChanged(int secondsPassed, int difficultyInfiniteModeLoopIncrement,
      int playerAlive)
    {
      var timeBasedConfigs = (TimePointConfig)generationConfigs;

      var timePoint = timeBasedConfigs.TimePoint;

      if (timePoint.Equals(secondsPassed))
      {
        return GenerateEnemies(timeBasedConfigs.Quantity, difficultyInfiniteModeLoopIncrement, playerAlive,
          ignoreQuantityLimit);
      }

      return new List<GameObject>();
    }
  }

  #endregion

  #region Time range

  public class TimeRangeConfig : EnemyGenerationConfigurations
  {
    public TimePeriod TimePeriod { get; set; }
    public float EnemiesPerSecond { get; set; }
  }

  public class TimeRangeEnemyGenerator : TimeBasedGenerator
  {
    private float nextSpawnQuantity;

    public TimeRangeEnemyGenerator(TimeRangeConfig timeRangeConfig, bool ignoreQuantityLimit = false)
    {
      generationConfigs = timeRangeConfig;
      this.ignoreQuantityLimit = ignoreQuantityLimit;
    }

    public override List<GameObject> OnTimeChanged(int secondsPassed, int difficultyInfiniteModeLoopIncrement,
      int playerAlive)
    {
      var enemies = new List<GameObject>();
      var timeBasedConfigs = (TimeRangeConfig)generationConfigs;
      var timePeriod = timeBasedConfigs.TimePeriod;

      if (secondsPassed < timePeriod.Start || secondsPassed > timePeriod.End) return enemies;

      if (nextSpawnQuantity >= 1)
      {
        enemies = GenerateEnemies((int)nextSpawnQuantity, difficultyInfiniteModeLoopIncrement, playerAlive,
          ignoreQuantityLimit);
        nextSpawnQuantity %= 1;
      }

      nextSpawnQuantity += timeBasedConfigs.EnemiesPerSecond;

      return enemies;
    }
  }

  #endregion

  #endregion

  public enum EnemySpawnMode
  {
    Angled,
    Around,
  }

  public class EnemyFactory : IStoreChangedHandler, IEventHandler
  {
    private readonly ObjectPool objectPool = ObjectPool.Instance;
    private readonly StoreManager storeManager = StoreManager.Instance;

    private readonly List<QuantityMaintainingEnemyGenerator> quantityBasedNormalGenerators = new();
    private readonly List<TimeBasedGenerator> timeBasedNormalGenerators = new();

    private readonly List<QuantityMaintainingEnemyGenerator> quantityBasedBossGenerators = new();
    private readonly List<TimeBasedGenerator> timeBasedBossGenerators = new();

    private readonly List<QuantityMaintainingEnemyGenerator> infiniteQuantityBasedNormalGenerators = new();
    private readonly List<TimeBasedGenerator> infiniteTimeBasedNormalGenerators = new();

    private readonly List<QuantityMaintainingEnemyGenerator> infiniteQuantityBasedBossGenerators = new();
    private readonly List<TimeBasedGenerator> infiniteTimeBasedBossGenerators = new();

    private readonly List<QuantityMaintainingEnemyGenerator> bossFightQuantityBasedGenerators = new();
    private readonly List<TimeBasedGenerator> bossFightTimeBasedGenerators = new();

    private readonly Scheduling scheduling = Scheduling.Instance;

    private int lastEnemyQuantity;
    private int gameTimePassedInSeconds; // Real level duration
    private int logicSecondsPassed; // Logical game duration for infinite mode
    private Vector2 playerPosition;
    private int playerAlive;

    private float spawnDegree;
    private readonly string resetGapDegreeScheduleId;
    private EnemySpawnMode spawnMode = EnemySpawnMode.Angled;

    private readonly PlayModes playMode;

    private int remainingBoss;

    public EnemyFactory()
    {
      playerPosition = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore).CenterPosition;
      storeManager
        .Subscribe(StoreNames.LevelConfigurationStore, this)
        .Subscribe(StoreNames.LevelTimePassedStore, this)
        .Subscribe(StoreNames.PlayerPositionStore, this)
        .Subscribe(StoreNames.LevelStore, this);

      spawnDegree = Random.Range(0, 360);
      resetGapDegreeScheduleId = scheduling.SetInterval(() =>
      {
        spawnDegree = Random.Range(0, 360);
        spawnMode = Random.Range(0, 100) < 80 ? EnemySpawnMode.Angled : EnemySpawnMode.Around;
      }, 15);

      playMode = StoreManager.Instance.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore).PlayMode;

      EventManager.Instance
        .AddEventHandler(Events.BOSS_APPEAR, this)
        .AddEventHandler(Events.BOSS_DIED, this);
    }

    public void OnEvent(Events @event, IEventData data)
    {
      switch (@event)
      {
        case Events.BOSS_APPEAR:
          remainingBoss++;
          break;
        case Events.BOSS_DIED:
          remainingBoss--;
          break;
        default:
          throw new InvalidEventHandlingException(@event);
      }
    }

    private void loadConfigurations(List<QuantityMaintainingEnemyGenerator> quantity,
      List<TimeBasedGenerator> timeBased,
      List<EnemyFactoryConfiguration> enemyConfigs,
      bool dispatchQuantityChanged = false,
      bool ignoreQuantityLimit = false)
    {
      if (enemyConfigs == null) return;

      foreach (var config in enemyConfigs)
      {
        switch (config.GeneratingType)
        {
          case EnemyGenerationTypes.QuantityMaintain:
            quantity.Add(
              new QuantityMaintainingEnemyGenerator((QuantityMaintainingConfig)config.Configurations));
            break;
          case EnemyGenerationTypes.TimePoint:
            timeBased.Add(new TimePointEnemyGenerator((TimePointConfig)config.Configurations, ignoreQuantityLimit));
            break;
          case EnemyGenerationTypes.TimeRange:
            timeBased.Add(new TimeRangeEnemyGenerator((TimeRangeConfig)config.Configurations, ignoreQuantityLimit));
            break;
        }
      }

      // Initialization.
      if (dispatchQuantityChanged)
      {
        DispatchQuantityChanged(quantity);
      }
    }

    private void loadNormalConfigurations(List<EnemyFactoryConfiguration> enemyConfigs)
    {
      loadConfigurations(quantityBasedNormalGenerators, timeBasedNormalGenerators, enemyConfigs, true);
    }

    private void loadBossConfigurations(List<EnemyFactoryConfiguration> enemyConfigs)
    {
      loadConfigurations(quantityBasedBossGenerators, timeBasedBossGenerators, enemyConfigs, false, true);
    }

    private void loadInfiniteNormalConfigurations(List<EnemyFactoryConfiguration> infiniteConfigurations)
    {
      loadConfigurations(infiniteQuantityBasedNormalGenerators, infiniteTimeBasedNormalGenerators,
        infiniteConfigurations);
    }

    private void loadInfiniteBossConfigurations(List<EnemyFactoryConfiguration> infiniteConfigurations)
    {
      loadConfigurations(infiniteQuantityBasedBossGenerators, infiniteTimeBasedBossGenerators, infiniteConfigurations,
        false, true);
    }

    private void loadBossFightConfigurations(BossFightNormalEnemyConfiguration config)
    {
      var sets = config.EnemyPrefabNames.Select(enemyPrefabName => new EnemyGenerationSetting()
        {
          EnemyPrefabName = enemyPrefabName,
          EnemyGeneratingCoefficient = 1,
          Strength = config.BaseStrength,
        })
        .ToList();

      switch (config.GeneratingType)
      {
        case EnemyGenerationTypes.QuantityMaintain:
          bossFightQuantityBasedGenerators.Add(new QuantityMaintainingEnemyGenerator(new()
          {
            Quantity = Random.Range(GeneralConfigurations.MinEnemiesInBossFight,
              GeneralConfigurations.MaxEnemiesInBossFight + 1),
            EnemyGenerationSets = sets,
            TimePeriod = new TimePeriod
            {
              Start = 0,
              End = int.MaxValue,
            },
          }));
          break;
        case EnemyGenerationTypes.TimeRange:
          bossFightTimeBasedGenerators.Add(new TimeRangeEnemyGenerator(new()
          {
            EnemiesPerSecond = Random.Range(
              GeneralConfigurations.MinEnemiesPerSecondInBossFight,
              GeneralConfigurations.MaxEnemiesPerSecondInBossFight
            ),
            EnemyGenerationSets = sets,
            TimePeriod = new TimePeriod
            {
              Start = 0,
              End = int.MaxValue,
            },
          }));
          break;
      }
    }

    public void LoadEnemyConfigs(LevelEnemyConfiguration configs)
    {
      loadNormalConfigurations(configs.NormalEnemyFactoryConfigurations);
      loadBossConfigurations(configs.BossFactoryConfigurations);
      loadInfiniteNormalConfigurations(configs.InfiniteNormalEnemyFactoryConfigurations);
      loadInfiniteBossConfigurations(configs.InfiniteBossFactoryConfigurations);
      loadBossFightConfigurations(configs.BossFightNormalEnemyConfiguration);
    }

    private void PlaceEnemies(int secondsPassed, List<GameObject> enemies)
    {
      if (enemies.Count == 0)
      {
        return;
      }

      foreach (var enemy in enemies)
      {
        float angle;
        if (secondsPassed <= 15)
        {
          angle = Random.Range(0, 2) == 0 ? Random.Range(60f, 120f) : Random.Range(240f, 300f);
        }
        else if (spawnMode == EnemySpawnMode.Angled)
        {
          angle = Random.Range(spawnDegree - 30f, spawnDegree + 30f) % 360;
        }
        else
        {
          angle = Random.Range(0, 360f);
        }

        var clampedAngle = angle < 0 ? angle + 360 : angle;

        Vector2 rotatedVector =
          Quaternion.Euler(0, 0, clampedAngle) * Vector2.right * GeneralConfigurations.SpawnDistance;

        var placement = playerPosition + rotatedVector + rotatedVector.normalized * Random.Range(1, 1.5f);
        enemy.transform.position = placement;
        var appearSmoke = objectPool.GetObject("Status/AppearSmoke");
        appearSmoke.transform.position = placement;
      }

      storeManager.Commit(StoreNames.LevelStore, StoreActions.LevelStore_ADD_ENEMIES, new LevelData()
      {
        Enemies = enemies,
      });
    }

    /**
     * Returns the logical time for infinite loop
     */
    private int GetLogicalTimeInSeconds()
    {
      if (playMode == PlayModes.INFINITE)
      {
        return gameTimePassedInSeconds % (GeneralConfigurations.InfiniteEnemyGenerationLoopMinutes * 60);
      }

      return gameTimePassedInSeconds;
    }

    private int GetInfiniteLoops()
    {
      return playMode == PlayModes.INFINITE
        ? gameTimePassedInSeconds / (GeneralConfigurations.InfiniteEnemyGenerationLoopMinutes * 60)
        : 0;
    }

    private void DispatchQuantityChanged(List<QuantityMaintainingEnemyGenerator> generators)
    {
      var logicalTime = GetLogicalTimeInSeconds();
      foreach (var generator in generators)
      {
        var enemies = generator.OnEnemyQuantityChanged(
          logicalTime,
          lastEnemyQuantity,
          GeneralConfigurations.GetInfiniteLoopDifficultyIncrementToLoopCount(GetInfiniteLoops()),
          playerAlive
        );

        PlaceEnemies(gameTimePassedInSeconds, enemies);
      }
    }

    private void DispatchTimeChanged(List<TimeBasedGenerator> generators)
    {
      var logicalTime = GetLogicalTimeInSeconds();
      foreach (var generator in generators)
      {
        var enemies = generator.OnTimeChanged(
          logicalTime,
          GeneralConfigurations.GetInfiniteLoopDifficultyIncrementToLoopCount(GetInfiniteLoops()),
          playerAlive
        );

        if (enemies.Count > 0)
        {
          PlaceEnemies(gameTimePassedInSeconds, enemies);
        }
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      var minutesToBoss = GeneralConfigurations.GetMinutesToBoss(playMode);

      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          playerAlive = ((LevelConfigurationState)state).PlayerAlive.Count(b => b);
          break;
        case StoreNames.PlayerPositionStore:
          var positionState = (PlayerPositionState)state;
          playerPosition = positionState.CenterPosition;
          break;
        case StoreNames.LevelTimePassedStore:
          var levelTimePassedState = (LevelTimePassedState)state;
          gameTimePassedInSeconds = levelTimePassedState.LevelTimePassed;

          if (playMode == PlayModes.INFINITE && gameTimePassedInSeconds > minutesToBoss * 60)
          {
            DispatchTimeChanged(infiniteTimeBasedBossGenerators);
          }
          else
          {
            DispatchTimeChanged(timeBasedBossGenerators);
          }

          if (lastEnemyQuantity > GeneralConfigurations.MaxEnemyOnScreen) break;

          if (playMode == PlayModes.INFINITE && gameTimePassedInSeconds > minutesToBoss * 60)
          {
            DispatchTimeChanged(remainingBoss > 0 ? bossFightTimeBasedGenerators : infiniteTimeBasedNormalGenerators);
          }
          else
          {
            DispatchTimeChanged(remainingBoss > 0 ? bossFightTimeBasedGenerators : timeBasedNormalGenerators);
          }

          break;
        case StoreNames.LevelStore:
          var levelState = (LevelState)state;
          var enemies = levelState.Enemies;
          lastEnemyQuantity = enemies.Count;

          if (playMode == PlayModes.INFINITE && gameTimePassedInSeconds > minutesToBoss * 60)
          {
            DispatchQuantityChanged(infiniteQuantityBasedBossGenerators);
          }
          else
          {
            DispatchQuantityChanged(quantityBasedBossGenerators);
          }

          if (lastEnemyQuantity > GeneralConfigurations.MaxEnemyOnScreen) break;

          if (playMode == PlayModes.INFINITE && gameTimePassedInSeconds > minutesToBoss * 60)
          {
            DispatchQuantityChanged(remainingBoss > 0
              ? bossFightQuantityBasedGenerators
              : infiniteQuantityBasedNormalGenerators);
          }
          else
          {
            DispatchQuantityChanged(remainingBoss > 0
              ? bossFightQuantityBasedGenerators
              : quantityBasedNormalGenerators);
          }

          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    public void Destroy()
    {
      storeManager.Unsubscribe(this);
    }

    ~EnemyFactory()
    {
      storeManager.Unsubscribe(this);
      scheduling.ClearSchedule(resetGapDegreeScheduleId);
    }
  }
}