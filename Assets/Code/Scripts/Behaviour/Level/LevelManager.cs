using System.Linq;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Behaviour.UI.Gem;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Enemy;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Level
{
  public class LevelManager : MonoBehaviour, IEventHandler
  {
    private ResourceManager resourceManager;
    private EventManager eventManager;
    private StoreManager storeManager;
    private EnemyFactory enemyFactory;
    private ObjectPool objectPool;
    private Scheduling scheduling;
    private SkillAndUpgradeManager skillAndUpgradeManager;

    private int numberOfPlayers;

    private PlayModes playMode;

    private AudioSource audioSource;
    [SerializeField] private AudioClip forestBgm;
    [SerializeField] private AudioClip desertBgm;
    [SerializeField] private AudioClip dungeonBgm;
    [SerializeField] private AudioClip graveyardBgm;
    [SerializeField] private AudioClip hellBgm;

    private string hellIntervalHurtId;

    private void Awake()
    {
      if (!DebugConfigurations.DebugEnabled)
      {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
      }
      else
      {
        Application.targetFrameRate = -1;
      }

      scheduling = Scheduling.Instance;
      scheduling.Init();

      resourceManager = ResourceManager.Instance;

      eventManager = EventManager.Instance;
      objectPool = ObjectPool.Instance;

      skillAndUpgradeManager = SkillAndUpgradeManager.Instance;

      audioSource = GetComponent<AudioSource>();

      storeManager = StoreManager.Instance;
      storeManager
        .SetStore(StoreNames.BackgroundCanvas, new BackgroundCanvasStore())
        .SetStore(StoreNames.LevelConfigurationStore, new LevelConfigurationStore())
        .SetStore(StoreNames.GameSettingStore, new GameSettingStore())
        .SetStore(StoreNames.LevelStore, new LevelStore())
        .SetStore(StoreNames.TargetEnemyStore, new TargetEnemyStore())
        .SetStore(StoreNames.LevelSettingStore, new LevelSettingStore())
        .SetStore(StoreNames.PlayerStore, new PlayerStore())
        .SetStore(StoreNames.PlayerPositionStore, new PlayerPositionStore())
        .SetStore(StoreNames.AttackControlStore, new AttackControlStore())
        .SetStore(StoreNames.GameStateStore, new GameStateStore())
        .SetStore(StoreNames.LevelTimePassedStore, new LevelTimePassedStore())
        .SetStore(StoreNames.HighPrecisionLevelTimeStore, new HighPrecisionGameLevelStore())
        .SetStore(StoreNames.UpgradeStore, new UpgradeStore())
        .SetStore(StoreNames.LevelCollectionStore, new LevelCollectionStore())
        .SetStore(StoreNames.BattleDataStore, new BattleDataStore())
        .SetStore(StoreNames.ActiveSkillStore, new ActiveSkillStore())
        .SetStore(StoreNames.TimingSkillStore, new TimingSkillStore())
        .SetStore(StoreNames.LevelGeneralStore, new LevelGeneralStore());

      var levelConfigurations = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      var _numberOfPlayers = levelConfigurations.NumberOfPlayers;

      storeManager.Commit(StoreNames.ActiveSkillStore, StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS,
        new ActiveSkillData() { NumberOfPlayers = _numberOfPlayers });
      storeManager.Commit(StoreNames.AttackControlStore, StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS,
        new AttackControlActionData() { NumberOfPlayers = _numberOfPlayers });
      storeManager.Commit(StoreNames.BattleDataStore, StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS,
        new BattleData() { NumberOfPlayers = _numberOfPlayers });
      storeManager.Commit(StoreNames.LevelCollectionStore, StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS,
        new LevelCollectionData() { NumberOfPlayers = _numberOfPlayers });
      storeManager.Commit(StoreNames.PlayerPositionStore, StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS,
        new PlayerPositionData() { NumberOfPlayers = _numberOfPlayers });
      storeManager.Commit(StoreNames.PlayerStore, StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS,
        new PlayerActionData() { NumberOfPlayers = _numberOfPlayers });
      storeManager.Commit(StoreNames.TargetEnemyStore, StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS,
        new TargetEnemyData() { NumberOfPlayers = _numberOfPlayers });
      storeManager.Commit(StoreNames.UpgradeStore, StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS,
        new UpgradedSkillData() { NumberOfPlayers = _numberOfPlayers });
      storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS,
        new TimingSkillActionData() { NumberOfPlayers = _numberOfPlayers });
    }

    private void Start()
    {
      var levelConfig = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      numberOfPlayers = levelConfig.NumberOfPlayers;
      playMode = levelConfig.PlayMode;

      Initialize();
      eventManager.AddEventHandler(Events.PLAYER_DIED, this);
      eventManager.PublishEvent(Events.LEVEL_INITIALIZED, null);

      TryToShowControlGuide();

      audioSource.clip = levelConfig.MapConfiguration.MapName switch
      {
        MapNames.FOREST => forestBgm,
        MapNames.DESERT => desertBgm,
        MapNames.DUNGEON => dungeonBgm,
        MapNames.GRAVEYARD => graveyardBgm,
        MapNames.HELL => hellBgm,
        _ => audioSource.clip
      };
      audioSource.Play();
    }

    private void Initialize()
    {
      InitializeObjectPool();
      InitializeEnemyFactory();

      SpawnPlayers();
      ApplyGemEffects();
      ApplyRuneEffects();
      ApplyHeroFeatures();
    }

    private void InitializeObjectPool()
    {
      objectPool
        .PrepareObjects("Environment/ExpPoint", 10)
        .PrepareObjects("Environment/HurtValueTMP", 30)
        .PrepareObjects("Status/AppearSmoke", 30);
    }

    private void InitializeEnemyFactory()
    {
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);

      var enemyConfigs = levelConfigs.EnemyConfiguration;
      enemyFactory = new EnemyFactory();
      enemyFactory.LoadEnemyConfigs(enemyConfigs);
    }

    private void SpawnPlayers()
    {
      var levelConfigs =
        storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      var playerAlive = levelConfigs.PlayerAlive;
      var mapConfig = levelConfigs.MapConfiguration;

      for (var playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
      {
        if (!playerAlive[playerNumber])
        {
          continue;
        }

        var playerConfigs = levelConfigs.PlayerConfigurations[playerNumber];

        // Have to use native Resources.Load since dynamic type will cause typing error.
        var player = (GameObject)Instantiate(Resources.Load(playerConfigs.PlayerPrefabName));
        player.transform.position = new Vector2(-(numberOfPlayers - 1) + 2 * playerNumber, 0);
        var playerManager = player.GetComponent<PlayerManager>();
        playerManager
          .SetPlayerNumber(playerNumber)
          .InitPlayerConfigurations(levelConfigs.PlayerConfigurations[playerNumber]);

        if (
          mapConfig.MapName == MapNames.HELL &&
          playerConfigs.PlayerName != PlayerNames.ARCHANGEL &&
          playerConfigs.PlayerName != PlayerNames.PALADIN
        )
        {
          var hurt = player.GetComponentInChildren<PlayerHurtHandling>();
          var randomHurt = Random.Range(8.5f, 11.5f);
          hellIntervalHurtId = scheduling.SetInterval(() => hurt.TriggerNormalHit(randomHurt), 10);
        }
      }
    }

    private void ApplyGemEffects()
    {
      var gems = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore).EquippedGems;

      for (var playerNumber = 0; playerNumber < numberOfPlayers; playerNumber++)
      {
        var playerGems = gems[playerNumber].Gems;
        foreach (var gem in playerGems)
        {
          // Basic Skill
          skillAndUpgradeManager.SkillAndUpgradeControllers[playerNumber]
            .UpgradeBasicSkill(gem.BasicSkillUpgrader);

          // Movement
          var movementUpgrade = ObjectCopier.Clone(gem.MovementUpgrade);
          movementUpgrade.PlayerNumber = playerNumber;
          storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_APPLY_MOVEMENT_UPGRADE,
            movementUpgrade);

          // Attack control
          var attackControlUpgrade = ObjectCopier.Clone(gem.AttackControlUpgrade);
          attackControlUpgrade.PlayerNumber = playerNumber;
          storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_APPLY_ATTACK_UPGRADE,
            attackControlUpgrade);

          // Resurrection
          storeManager.Commit(StoreNames.LevelGeneralStore, StoreActions.LevelGeneralStore_ADD_RESURRECTION,
            new LevelGeneralData()
            {
              PlayerNumber = playerNumber,
              AddResurrection = 1,
              ResurrectionPercentage = 0.25f,
            });

          // Hp
          storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_AUGMENT_MAXIMUM_HP,
            new PlayerActionData()
            {
              PlayerNumber = playerNumber,
              AugmentMaximumHp = gem.IncreaseMaxHp,
              DoNotAugmentCurrentHp = false,
            });
        }
      }
    }

    private void ApplyRuneEffects()
    {
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      for (var playerNumber = 0; playerNumber < numberOfPlayers; playerNumber++)
      {
        foreach (var rune in levelConfigs.RunesConfigurations[playerNumber].ActivatedRunes)
        {
          rune.RuneInstance.Apply(playerNumber);
        }
      }
    }

    private void ApplyHeroFeatures()
    {
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      var playerAlive = levelConfigs.PlayerAlive;
      for (var playerNumber = 0; playerNumber < numberOfPlayers; playerNumber++)
      {
        if (!playerAlive[playerNumber])
        {
          continue;
        }

        var recover = levelConfigs.PlayerConfigurations[playerNumber].RecoverPerSecond;
        if (recover <= 0) continue;

        var number = playerNumber;
        skillAndUpgradeManager
          .SkillAndUpgradeControllers[playerNumber]
          .AddTimingAction(1, () =>
          {
            var playerState = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
            var maxHp = playerState.PlayerDatas[number].MaximumHp;

            storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_ADD_CURRENT_HP,
              new PlayerActionData()
              {
                PlayerNumber = number,
                AddCurrentHp =
                  maxHp * recover // If this value is updated, don't forget to update in FightPreparation and PauseUI.
              });
          });
      }
    }

    private void TryToShowControlGuide()
    {
      var notif = NotificationUtils.GetNotReadNotification(NotificationCatagories.BATTLEFIELD_BEGIN);
      if (notif != null)
      {
        scheduling.SetTimeout(() =>
        {
          WindowManager.Instance.OpenWindow(WindowNames.ControlGuide, OpenWindowActions.WaitForOthers);

          storeManager.Commit(StoreNames.GameSettingStore, StoreActions.GameSettingStore_ADD_SHOWN_NOTIFICATION,
            new GameSettingData()
            {
              NotificationId = notif.Id
            });

          SaveSystem.SaveGame();
        }, 0.0001f);
      }
    }

    public void OnEvent(Events @event, IEventData data)
    {
      switch (@event)
      {
        case Events.PLAYER_DIED:
          OnPlayerDied(((PlayerDiedEventData)data).PlayerNumber);
          break;
        default:
          throw new InvalidEventHandlingException(@event);
      }
    }

    private void OnPlayerDied(int playerNumber)
    {
      var timePassed = storeManager.GetState<LevelTimePassedState>(StoreNames.LevelTimePassedStore).LevelTimePassed;

      storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_DEAD_TIME, new PlayerActionData()
      {
        PlayerNumber = playerNumber,
        DeadTime = timePassed,
      });

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      var playerData = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
      var numberOfDeads = playerData.PlayerDatas.Count(pd => pd.PlayerDied);

      storeManager.Commit(StoreNames.LevelConfigurationStore, StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE,
        new LevelConfigurationData() { PlayerNumber = playerNumber, PlayerAlive = false });
      storeManager.Commit(StoreNames.ActiveSkillStore, StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE,
        new ActiveSkillData() { PlayerNumber = playerNumber, PlayerAlive = false });
      storeManager.Commit(StoreNames.AttackControlStore, StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE,
        new AttackControlActionData() { PlayerNumber = playerNumber, PlayerAlive = false });
      storeManager.Commit(StoreNames.BattleDataStore, StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE,
        new BattleData() { PlayerNumber = playerNumber, PlayerAlive = false });
      storeManager.Commit(StoreNames.LevelCollectionStore, StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE,
        new LevelCollectionData() { PlayerNumber = playerNumber, PlayerAlive = false });
      storeManager.Commit(StoreNames.PlayerPositionStore, StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE,
        new PlayerPositionData() { PlayerNumber = playerNumber, PlayerAlive = false });
      storeManager.Commit(StoreNames.PlayerStore, StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE,
        new PlayerActionData() { PlayerNumber = playerNumber, PlayerAlive = false });
      storeManager.Commit(StoreNames.TargetEnemyStore, StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE,
        new TargetEnemyData() { PlayerNumber = playerNumber, PlayerAlive = false });

      if (numberOfDeads >= levelConfigs.InitialNumberOfPlayers) // Game Over: Failed!
      {
        Mailer.Instance.SendMail(MailSenders.GAME_OVER_REQUEST, MailAddresses.GAME_OVER, new GameOverRequest()
        {
          Status = GameOverStatus.Failed,
        });

        WindowManager.Instance.OpenWindow(WindowNames.GameOverUI, OpenWindowActions.CloseOthers);
      }
    }

    private void OnLevelQuit()
    {
      resourceManager.ClearResources();
      eventManager.Destroy();
      storeManager.Destroy();
      enemyFactory.Destroy();
      objectPool.Reset();
      skillAndUpgradeManager.Reset();

      scheduling.Reset();

      Time.timeScale = 1;
    }

    private void OnDestroy()
    {
      scheduling.ClearSchedule(hellIntervalHurtId);
      OnLevelQuit();
    }

    private void OnApplicationQuit()
    {
      OnLevelQuit();
    }
  }
}