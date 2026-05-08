using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using UnityEngine;
using Version = Code.Scripts.Src.Configurations.Version;

namespace Code.Scripts.Src.Store.Level
{
  public enum PlayModes
  {
    CASUAL,
    STANDARD,
    INFINITE,
  }

  public class EquippedGems
  {
    public List<GemProperties> Gems { get; set; } = new();
  }

  public struct RuneContainer
  {
    public RuneTypes RuneType { get; set; }
    public int RuneLevel { get; set; }
    public BaseRune RuneInstance { get; set; }
  }

  public class LevelRunesConfiguration
  {
    public List<RuneContainer> ActivatedRunes { get; set; } = new();
  }

  public class LevelConfigurationState : IState
  {
    public int InitialNumberOfPlayers { get; set; } = -1;

    // Realtime number of player?
    public int NumberOfPlayers { get; set; } = -1;
    public bool[] PlayerAlive { get; set; }
    public PlayerConfiguration[] PlayerConfigurations { get; set; }
    public PlayerConfiguration[] FightPreparationShowingPlayerConfigurations { get; set; }
    public LevelEnemyConfiguration EnemyConfiguration { get; set; }
    public EquippedGems[] EquippedGems { get; set; }
    public PlayModes PlayMode { get; set; } = GeneralConfigurations.DefaultMode;
    public LevelRunesConfiguration[] RunesConfigurations { get; set; }
    public int LevelDifficulty { get; set; }
    public MapConfiguration MapConfiguration { get; set; }
  }

  public struct LevelConfigurationData : IActionData
  {
    public int InitialNumberOfPlayers { get; set; }
    public int PlayerNumber { get; set; }
    public int NumberOfPlayers { get; set; }
    public bool PlayerAlive { get; set; }
    public int LevelDifficulty { get; set; }
    public PlayerConfiguration PlayerConfiguration { get; set; }
    public LevelEnemyConfiguration EnemyConfiguration { get; set; }
    public List<GemProperties> Gems { get; set; }
    public RuneTypes RuneType { get; set; }
    public int RuneLevel { get; set; }
    public GemProperties Gem { get; set; }
    public PlayModes PlayMode { get; set; }
    public MapNames MapName { get; set; }
    public LevelRunesConfiguration RunesConfiguration { get; set; }
    public MapConfiguration MapConfiguration { get; set; }
  }

  public class LevelConfigurationStore : IStore
  {
    private static LevelConfigurationState GetInitState()
    {
      var state = new LevelConfigurationState
      {
        PlayerConfigurations = new PlayerConfiguration[GeneralConfigurations.MaximumPlayers],
        EnemyConfiguration = new(),
        FightPreparationShowingPlayerConfigurations = new PlayerConfiguration[GeneralConfigurations.MaximumPlayers],
        EquippedGems = new EquippedGems[GeneralConfigurations.MaximumPlayers],
        RunesConfigurations = new LevelRunesConfiguration[GeneralConfigurations.MaximumPlayers],
        PlayMode = GeneralConfigurations.DefaultMode,
      };

      for (int i = 0; i < state.PlayerConfigurations.Length; i++)
      {
        state.EquippedGems[i] = new();
        state.RunesConfigurations[i] = new();
      }

      return state;
    }

    public IState InitialState => GetInitState();

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      var configurationState = (LevelConfigurationState)state;
      var configurationData = (LevelConfigurationData)data;

      int playerNumber;

      switch (action)
      {
        case StoreActions.LevelConfigurationStore_SET_LEVEL_INIT_NUMBER_OF_PLAYERS:
          var initialPlayerNumber = configurationData.InitialNumberOfPlayers;
          configurationState.InitialNumberOfPlayers = initialPlayerNumber;
          configurationState.NumberOfPlayers = initialPlayerNumber;
          configurationState.PlayerAlive =
            new int[GeneralConfigurations.MaximumPlayers]
              .Select((_, index) => index < initialPlayerNumber).ToArray();

          return configurationState;
        case StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS:
          configurationState.NumberOfPlayers = configurationData.NumberOfPlayers;
          configurationState.PlayerAlive =
            new int[GeneralConfigurations.MaximumPlayers]
              .Select((_, index) => index < configurationState.InitialNumberOfPlayers).ToArray();
          return configurationState;
        case StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE:
          configurationState.PlayerAlive[configurationData.PlayerNumber] = configurationData.PlayerAlive;
          configurationState.NumberOfPlayers = configurationState.PlayerAlive.Count(p => p);
          return configurationState;
        case StoreActions.LevelConfigurationStore_SET_PLAYER_CONFIGURATIONS:
          playerNumber = configurationData.PlayerNumber;
          configurationState.PlayerConfigurations[playerNumber] = configurationData.PlayerConfiguration;

          return configurationState;
        case StoreActions.LevelConfigurationStore_SET_FIGHT_PREPARATION_SHOWING_PLAYER_CONFIGURATIONS:
          playerNumber = configurationData.PlayerNumber;
          configurationState.FightPreparationShowingPlayerConfigurations[playerNumber] =
            configurationData.PlayerConfiguration;

          return configurationState;
        case StoreActions.LevelConfigurationStore_ADD_EQUIPPED_GEM:
          playerNumber = configurationData.PlayerNumber;
          if (configurationState.EquippedGems[playerNumber].Gems.Count < 3)
          {
            var gem = configurationData.Gem;
            configurationState.EquippedGems[playerNumber].Gems.Add(gem);
          }

          return configurationState;
        case StoreActions.LevelConfigurationStore_REMOVE_EQUIPPED_GEM:
          playerNumber = configurationData.PlayerNumber;
          configurationState.EquippedGems[playerNumber].Gems.RemoveAll(gem => gem.UUID == configurationData.Gem.UUID);
          return configurationState;
        case StoreActions.LevelConfigurationStore_SET_RUNE:
          playerNumber = configurationData.PlayerNumber;

          var runeType = configurationData.RuneType;
          var runeLevel = configurationData.RuneLevel;
          var @class = RuneUtils.GetRuneClass(runeType);

          configurationState.RunesConfigurations[playerNumber].ActivatedRunes
            .RemoveAll(rc => RuneUtils.GetRuneClass(rc.RuneType) == @class);

          var runeInstance =
            (BaseRune)Activator.CreateInstance(RunePresets.Presets[runeType].RuneControllerType);
          runeInstance.SetRuneLevel(runeLevel);

          configurationState.RunesConfigurations[playerNumber].ActivatedRunes.Add(new()
          {
            RuneInstance = runeInstance,
            RuneType = runeType,
            RuneLevel = runeLevel,
          });
          return configurationState;
        case StoreActions.LevelConfigurationStore_UNSET_RUNE:
          playerNumber = configurationData.PlayerNumber;
          configurationState.RunesConfigurations[playerNumber].ActivatedRunes
            .RemoveAll(rc => rc.RuneType == configurationData.RuneType);
          return configurationState;
        case StoreActions.LevelConfigurationStore_SET_LEVEL_DIFFICULTY:
          configurationState.LevelDifficulty = Mathf.Clamp(configurationData.LevelDifficulty, 0, 9);
          return configurationState;
        case StoreActions.LevelConfigurationStore_SET_MAP:
          configurationState.MapConfiguration = MapConfigurations.Configurations
            .Where(elem => elem.MapName == configurationData.MapName).ToList().First();
          return configurationState;
        case StoreActions.LevelConfigurationStore_SET_PLAY_MODE:
          configurationState.PlayMode = configurationData.PlayMode;
          return configurationState;
        case StoreActions.LevelConfigurationStore_SET_ENEMY_CONFIGURATION:
          configurationState.EnemyConfiguration = configurationData.EnemyConfiguration;
          return configurationState;
        case StoreActions.LevelConfigurationStore_CLEAR:
          return GetInitState();
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}