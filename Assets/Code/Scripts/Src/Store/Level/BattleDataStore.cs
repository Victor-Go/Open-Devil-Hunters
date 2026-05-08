using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Steam.Achievements;
using UnityEngine;

namespace Code.Scripts.Src.Store.Level
{
  public struct KilledEnemy
  {
    public EnemyNames EnemyId { get; set; }
    public int KilledTimestamp { get; set; }
    public int KilledByPlayerNumber { get; set; }
  }

  public struct BattleDataState : IState
  {
    public int NumberOfPlayers { get; set; }
    public bool[] PlayerAlive { get; set; }
    public List<KilledEnemy> KilledEnemies { get; set; }
    public bool FirstBloodAchievement { get; set; }
    public bool RampageAchievement { get; set; }
    public bool LegendaryAchievement { get; set; }
    public int HistoricalKilledCounter { get; set; }
  }

  public struct BattleData : IActionData
  {
    public int NumberOfPlayers { get; set; }
    public int PlayerNumber { get; set; }
    public bool PlayerAlive { get; set; }
    public EnemyNames KilledEnemyId { get; set; }
    public int KilledTimestamp { get; set; }
  }

  public class BattleDataStore : IStore
  {
    public IState InitialState => new BattleDataState()
    {
      FirstBloodAchievement = AchievementsManager.GetAchievementIsUnlocked(Achievements.FirstBlood),
      RampageAchievement = AchievementsManager.GetAchievementIsUnlocked(Achievements.Rampage),
      LegendaryAchievement = AchievementsManager.GetAchievementIsUnlocked(Achievements.Legendary),
      HistoricalKilledCounter = PlayerPrefs.GetInt(nameof(PlayerPrefVariables.HistoricalKilledCounter), 0),
    };

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      var battleDataState = (BattleDataState)state;
      var battleData = (BattleData)data;

      switch (action)
      {
        case StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS:
          var numberOfPlayers = battleData.NumberOfPlayers;
          battleDataState.NumberOfPlayers = numberOfPlayers;
          battleDataState.PlayerAlive ??= new int[GeneralConfigurations.MaximumPlayers]
            .Select((_, index) => index < numberOfPlayers).ToArray();
          battleDataState.KilledEnemies ??= new();

          return battleDataState;
        case StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE:
          battleDataState.PlayerAlive[battleData.PlayerNumber] = battleData.PlayerAlive;
          battleDataState.NumberOfPlayers = battleDataState.PlayerAlive.Count(p => p);
          return battleDataState;
        case StoreActions.BattleDataStore_ADD_KILLED_ENEMY:
          battleDataState.KilledEnemies.Add(new KilledEnemy()
          {
            EnemyId = battleData.KilledEnemyId,
            KilledTimestamp = battleData.KilledTimestamp,
            KilledByPlayerNumber = battleData.PlayerNumber,
          });

          if (!battleDataState.FirstBloodAchievement)
          {
            battleDataState.FirstBloodAchievement = true;
            AchievementsManager.UnlockAchievement(Achievements.FirstBlood);
          }

          if (battleDataState is { RampageAchievement: false, LegendaryAchievement: false })
          {
            battleDataState.HistoricalKilledCounter++;
            PlayerPrefs.SetInt(nameof(PlayerPrefVariables.HistoricalKilledCounter),
              battleDataState.HistoricalKilledCounter);

            if (battleDataState.HistoricalKilledCounter >= 10000)
            {
              battleDataState.RampageAchievement = true;
              AchievementsManager.UnlockAchievement(Achievements.Rampage);
            }

            if (battleDataState.HistoricalKilledCounter >= 1000000)
            {
              battleDataState.LegendaryAchievement = true;
              AchievementsManager.UnlockAchievement(Achievements.Legendary);
            }
          }

          if (battleDataState.KilledEnemies.Count >= 10000)
          {
            AchievementsManager.UnlockAchievement(Achievements.Gatling);
          }

          return battleDataState;
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}