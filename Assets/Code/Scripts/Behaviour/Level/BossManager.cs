using System.Collections.Generic;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Level
{
  public class BossManager : MonoBehaviour, IEventHandler
  {
    private EventManager eventManager;
    private StoreManager storeManager;
    private Scheduling scheduling;

    private GameObject wall;

    private int remainingBoss;

    private string displayGameOverUIScheduleId;

    private const int bossPerRound = 2;
    private readonly HashSet<EnemyNames> seenBosses = new();

    private void Awake()
    {
      eventManager = EventManager.Instance;
      storeManager = StoreManager.Instance;
      scheduling = Scheduling.Instance;
    }

    private void Start()
    {
      eventManager
        .AddEventHandler(Events.BOSS_APPEAR, this)
        .AddEventHandler(Events.BOSS_DIED, this);
    }

    public void OnEvent(Events @event, IEventData data)
    {
      switch (@event)
      {
        case Events.BOSS_APPEAR:
          remainingBoss++;
          BuildWall();
          seenBosses.Add(((BossAppearEventData)data).Enemy.EnemyConfigurationName);
          break;
        case Events.BOSS_DIED:
          remainingBoss--;
          HandleAnyBossDied();
          break;
        default:
          throw new InvalidEventHandlingException(@event);
      }
    }

    private void BuildWall()
    {
      if (wall == null)
      {
        var playerPositions = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore);
        wall = Instantiate(Resources.Load<GameObject>("Environment/Wall"), null, true);
        wall.transform.position = playerPositions.CenterPosition;
      }
    }

    private void DestroyWall()
    {
      if (wall != null)
      {
        Destroy(wall);
        wall = null;
      }
    }

    private void HandleAnyBossDied()
    {
      if (remainingBoss != 0) return;

      DestroyWall();

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      if (seenBosses.Count < bossPerRound || levelConfigs.PlayMode == PlayModes.INFINITE) return;

      eventManager.PublishEvent(Events.GAME_OVER, null);

      Mailer.Instance.SendMail(MailSenders.GAME_OVER_REQUEST, MailAddresses.GAME_OVER, new GameOverRequest()
      {
        Status = GameOverStatus.Success,
      });

      var currentDifficulty = levelConfigs.LevelDifficulty;
      var mapName = levelConfigs.MapConfiguration.MapName;
      if (currentDifficulty < 9)
      {
        storeManager.Commit(StoreNames.GameDataStore, StoreActions.GameDataStore_SET_MAP_UNLOCKED_LEVEL_DIFFICULTY,
          new GameDataActionData
          {
            UnlockDifficulty = currentDifficulty + 1,
            MapName = mapName,
          });
      }

      displayGameOverUIScheduleId =
        scheduling.SetTimeout(
          () => { WindowManager.Instance.OpenWindow(WindowNames.GameOverUI, OpenWindowActions.CloseOthers); }, 3);
    }

    private void OnDestroy()
    {
      eventManager.RemoveEventHandler(this);
      scheduling.ClearSchedule(displayGameOverUIScheduleId);
    }
  }
}