using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Src.Store.Game
{
  [Serializable]
  public class RuneData
  {
    public RuneTypes RuneType { get; set; }
    public int RuneLevel { get; set; }
  }

  [Serializable]
  public class GameDataState : IState
  {
    public int PlayerBalance { get; set; }
    public List<PlayerNames> UnlockedPlayers { get; set; } = new();
    public List<RuneData> UnlockedRunes { get; set; } = new();
    public List<GemProperties> OwnedeGems { get; set; } = new();

    public Dictionary<MapNames, int> UnlockedDifficulties { get; set; } = new()
    {
      { MapNames.FOREST, 0 },
      { MapNames.DESERT, 0 },
      { MapNames.DUNGEON, 0 },
      { MapNames.GRAVEYARD, 0 },
      { MapNames.HELL, 0 },
    };
  }

  public struct GameDataActionData : IActionData
  {
    public GameDataState SavedState { get; set; }
    public int PlayerHonor { get; set; }
    public PlayerNames PlayerName { get; set; }
    public RuneTypes RuneType { get; set; }
    public MapNames MapName { get; set; }
    public int UnlockDifficulty { get; set; }
    public GemProperties Gem { get; set; }
  }

  public class GameDataStore : IStore
  {
    public IState InitialState
    {
      get
      {
        var state = new GameDataState();

        foreach (var hero in GeneralConfigurations.DefaultUnlockedPlayers)
        {
          state.UnlockedPlayers.Add(hero);
        }

        return state;
      }
    }


    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      var gameActionData = (GameDataActionData)data;
      var gameDataState = (GameDataState)state;

      switch (action)
      {
        case StoreActions.GameDataStore_LOAD_FROM_SAVE:
          gameDataState = gameActionData.SavedState;

          // Added Wukong as default hero, failsafe for old saves
          if (gameDataState.UnlockedPlayers.All(p => p != PlayerNames.WUKONG))
          {
            gameDataState.UnlockedPlayers.Add(PlayerNames.WUKONG);
          }
          
          return gameDataState;
        case StoreActions.GameDataStore_DECREASE_PLAYER_HONOR:
          gameDataState.PlayerBalance -= gameActionData.PlayerHonor;
          return gameDataState;
        case StoreActions.GameDataStore_UNLOCK_HERO:
          gameDataState.UnlockedPlayers.Add(gameActionData.PlayerName);
          return gameDataState;
        case StoreActions.GameDataStore_UNLOCK_RUNE:
          gameDataState.UnlockedRunes.Add(new()
          {
            RuneType = gameActionData.RuneType,
            RuneLevel = 0
          });
          return gameDataState;
        case StoreActions.GameDataStore_ADD_GEM:
          gameDataState.OwnedeGems.Add(gameActionData.Gem);
          return gameDataState;
        case StoreActions.GameDataStore_UPGRADE_RUNE:
          for (int i = 0; i < gameDataState.UnlockedRunes.Count; i++)
          {
            if (gameDataState.UnlockedRunes[i].RuneType == gameActionData.RuneType)
            {
              var rune = gameDataState.UnlockedRunes[i];
              rune.RuneLevel++;
              gameDataState.UnlockedRunes[i] = rune;
              break;
            }
          }

          return gameDataState;
        case StoreActions.GameDataStore_SET_MAP_UNLOCKED_LEVEL_DIFFICULTY:
          var currentDifficulty = gameDataState.UnlockedDifficulties[gameActionData.MapName];
          gameDataState.UnlockedDifficulties[gameActionData.MapName] =
            currentDifficulty < gameActionData.UnlockDifficulty ? gameActionData.UnlockDifficulty : currentDifficulty;
          return gameDataState;
        case StoreActions.GameDataStore_INCREASE_HONOR:
          gameDataState.PlayerBalance += gameActionData.PlayerHonor;
          return gameDataState;
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}