using System.Collections.Generic;
using Code.Scripts.Src;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Src.Store.Level
{
  public class LevelGeneral
  {
    public int ResurrectionCount { get; set; }
    public float ResurrectionPercentage { get; set; }
    public bool PoisonedOnSpawn { get; set; }

    public Dictionary<int, float> RecoverOnKilled { get; set; } =
      new() { { 0, 0 }, { 1, 0 } }; // Key: PlayerNumber, Value: Recover Percentage
  }

  public class LevelGeneralState : IState
  {
    public float HonorBonus { get; set; }
    public LevelGeneral[] PlayLevelGeneral { get; set; } = { new(), new() };
  }

  public class LevelGeneralData : IActionData
  {
    public int AddResurrection { get; set; }
    public int RemoveResurrection { get; set; }
    public float ResurrectionPercentage { get; set; }
    public float HonorBonus { get; set; }
    public bool PoisonedOnSpawn { get; set; }
    public int PlayerNumber { get; set; }
    public float RecoverPercentageOnKilled { get; set; }
  }

  public class LevelGeneralStore : IStore
  {
    public IState InitialState => new LevelGeneralState();

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      var levelGeneralState = (LevelGeneralState)state;
      var levelGeneralData = ObjectCopier.Clone((LevelGeneralData)data);
      var playerNumber = levelGeneralData.PlayerNumber;

      switch (action)
      {
        case StoreActions.LevelGeneralStore_ADD_RESURRECTION:
          levelGeneralState.PlayLevelGeneral[playerNumber].ResurrectionCount += levelGeneralData.AddResurrection;
          levelGeneralState.PlayLevelGeneral[playerNumber].ResurrectionPercentage =
            Mathf.Max(levelGeneralState.PlayLevelGeneral[playerNumber].ResurrectionPercentage,
              levelGeneralData.ResurrectionPercentage);
          return levelGeneralState;
        case StoreActions.LevelGeneralStore_REMOVE_RESURRECTION:
          levelGeneralState.PlayLevelGeneral[playerNumber].ResurrectionCount -= Mathf.Max(0, levelGeneralData.RemoveResurrection);
          return levelGeneralState;
        case StoreActions.LevelGeneralStore_SET_SET_HONOR_BONUS:
          levelGeneralState.HonorBonus = Mathf.Max(levelGeneralData.HonorBonus, levelGeneralState.HonorBonus);
          return levelGeneralState;
        case StoreActions.LevelGeneralStore_SET_ENEMY_SPAWN_POISONED:
          levelGeneralState.PlayLevelGeneral[playerNumber].PoisonedOnSpawn = levelGeneralData.PoisonedOnSpawn;
          return levelGeneralState;
        case StoreActions.LevelGeneralStore_SET_RECOVER_ON_KILLED:
          levelGeneralState.PlayLevelGeneral[playerNumber].RecoverOnKilled[levelGeneralData.PlayerNumber] =
            levelGeneralData.RecoverPercentageOnKilled;
          return levelGeneralState;
        case StoreActions.LevelGeneralStore_RESET:
          return new LevelGeneralState();
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}