using System;
using System.Collections.Generic;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Src
{
  public enum StoreNames
  {
    GameDataStore,
    BackgroundCanvas,
    LevelConfigurationStore,
    GameSettingStore,
    GameStateStore,
    PlayerStore,
    PlayerPositionStore,
    AttackControlStore,
    LevelSettingStore,
    LevelStore,
    TargetEnemyStore,
    LevelTimePassedStore,
    HighPrecisionLevelTimeStore,
    UpgradeStore,
    LevelCollectionStore,
    BattleDataStore,
    ActiveSkillStore,
    TimingSkillStore,
    LevelGeneralStore,
  }

  public enum StoreActions
  {
    BackgroundCanvas_SET_ACTIVE,
    NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS,
    NumberOfPlayersRelated_SET_PLAYER_ALIVE,

    GameDataStore_LOAD_FROM_SAVE,
    GameDataStore_DECREASE_PLAYER_HONOR,
    GameDataStore_UNLOCK_HERO,
    GameDataStore_UNLOCK_RUNE,
    GameDataStore_SET_MAP_UNLOCKED_LEVEL_DIFFICULTY,
    GameDataStore_UPGRADE_RUNE,
    GameDataStore_ADD_GEM,
    GameDataStore_INCREASE_HONOR,

    LevelConfigurationStore_SET_LEVEL_INIT_NUMBER_OF_PLAYERS,
    LevelConfigurationStore_ADD_EQUIPPED_GEM,
    LevelConfigurationStore_REMOVE_EQUIPPED_GEM,
    LevelConfigurationStore_SET_RUNE,
    LevelConfigurationStore_UNSET_RUNE,
    LevelConfigurationStore_SET_PLAYER_CONFIGURATIONS,
    LevelConfigurationStore_SET_FIGHT_PREPARATION_SHOWING_PLAYER_CONFIGURATIONS,
    LevelConfigurationStore_SET_LEVEL_DIFFICULTY,
    LevelConfigurationStore_SET_MAP,
    LevelConfigurationStore_SET_PLAY_MODE,
    LevelConfigurationStore_SET_ENEMY_CONFIGURATION,
    LevelConfigurationStore_CLEAR,

    GameSettingStore_LOAD_FROM_SAVE,
    GameSettingStore_SET_LOCALE,
    GameSettingStore_ADD_SHOWN_NOTIFICATION,

    GameStateStore_SET_GAME_STATE,

    PlayerStore_INITIALIZE,
    PlayerStore_SET_CURRENT_HP,
    PlayerStore_ADD_CURRENT_HP,
    PlayerStore_ADD_CURRENT_HP_PERCENTAGE,
    PlayerStore_SET_MAXIMUM_HP,
    PlayerStore_AUGMENT_MAXIMUM_HP,
    PlayerStore_SET_CURRENT_EXPERIENCE,
    PlayerStore_ADD_CURRENT_EXPERIENCE,
    PlayerStore_SET_LEVEL_UP_DATA,
    PlayerStore_SET_MOVING_SPEED,
    PlayerStore_SET_ATTACKING_MOVING_SPEED,
    PlayerStore_SET_PICK_UP_RADIUS,
    PlayerStore_APPLY_MOVEMENT_UPGRADE,
    PlayerStore_SET_PLAYER_STATUS,
    PlayerStore_SET_PLAYER_POISONED,
    PlayerStore_SET_IMMORTAL_SECONDS,
    PlayerStore_SET_DEAD_TIME,

    PlayerPositionStore_SET_PLAYER_POSITION,
    PlayerPositionStore_SET_PLAYER_TRANSFORM,

    AttackControlStore_SET_ATTACK_CONTROL_STATE,
    AttackControlStore_SET_ATTACK_PER_ROUND,
    AttackControlStore_SET_ATTACK_CONTROL_CONFIGURATIONS,
    AttackControlStore_SET_REMAINING_ROUND_COUNT,
    AttackControlStore_SET_COOLING_TIME,
    AttackControlStore_SET_COOLING_COUNTDOWN,
    AttackControlStore_SET_RELOAD_TIME,
    AttackControlStore_SET_RELOAD_COUNTDOWN,
    AttackControlStore_SET_RELOADING,
    AttackControlStore_SET_COOLING,
    AttackControlStore_APPLY_ATTACK_UPGRADE,
    AttackControlStore_SET_IN_ATTACKING_STATE,

    LevelSettingStore_SET_AIMING_MODE,
    LevelSettingStore_SET_FIRING_MODE,

    LevelStore_SET_ENEMIES,
    LevelStore_ADD_ENEMY,
    LevelStore_ADD_ENEMIES,
    LevelStore_REMOVE_ENEMY,

    TargetEnemyStore_SET_TARGET_ENEMY,
    TargetEnemyStore_SET_CONTROLLER_AIM_POSITION,

    LevelTimePassedStore_SET_LEVEL_TIME_PASSED,
    HighPrecisionLevelTimeStore_SET_LEVEL_TIME_PASSED,

    UpgradeStore_ADD_UPGRADE,

    LevelCollectionStore_ADD_GEM,
    LevelCollectionStore_CLEAR,

    BattleDataStore_ADD_KILLED_ENEMY,

    ActiveSkillStore_SET_COOLING_COUNTDOWN,
    ActiveSkillStore_SET_ACTIVE_SKILL_ICON,
    ActiveSkillStore_SET_BULLET_FISSION_POSSIBILITY,

    TimingSkillStore_SET_TIMING_SKILL,
    TimingSkillStore_REMOVE_TIMING_SKILL,
    TimingSkillStore_UPDATE_DELTA_TIME,
    TimingSkillStore_SET_TIME_TO_LAUNCH,
    TimingSkillStore_RESET,

    LevelGeneralStore_ADD_RESURRECTION,
    LevelGeneralStore_REMOVE_RESURRECTION,
    LevelGeneralStore_SET_SET_HONOR_BONUS,
    LevelGeneralStore_SET_ENEMY_SPAWN_POISONED,
    LevelGeneralStore_SET_RECOVER_ON_KILLED,
    LevelGeneralStore_RESET,
  }

  [Serializable]
  public class InvalidStoreActionException : Exception
  {
    public InvalidStoreActionException() : base()
    {
    }

    public InvalidStoreActionException(StoreActions action)
      : base($"Invalid Store Action: {action}")
    {
      Console.WriteLine("Invalid Store Action: {0}", action);
    }
  }

  [Serializable]
  public class InvalidStoreEventException : Exception
  {
    public InvalidStoreEventException() : base()
    {
    }

    public InvalidStoreEventException(StoreNames storeName)
      : base($"Invalid Store Event: {storeName}")
    {
      Console.WriteLine("Invalid Store Event: {0}", storeName);
    }
  }


  public interface IState
  {
  }

  public interface IActionData
  {
  }

  public interface IStore
  {
    public IState InitialState { get; }

    public IState Commit(IState state, StoreActions action, IActionData data);
  }

  public interface IStore<T> : IStore where T : struct, IActionData
  {
    public IState Commit(IState state, StoreActions action, ref T data);
  }

  public interface IStoreChangedHandler
  {
    public abstract void OnStoreChanged(StoreNames storeName, IState state);
  }

  [Serializable]
  public class InvalidStoreNameException : Exception
  {
    public InvalidStoreNameException() : base()
    {
    }

    public InvalidStoreNameException(StoreNames storeName)
      : base($"Invalid Store Name: {storeName}. Check if store is initialized.")
    {
      Console.WriteLine("Invalid Store Name: {0}. Check if store is initialized.", storeName);
    }
  }

  public class StoreManager : Singleton<StoreManager>
  {
    private readonly Dictionary<StoreNames, Tuple<IStore, IState>> stores = new();
    private readonly Dictionary<StoreNames, List<IStoreChangedHandler>> observers = new();
    private bool _destroyed;

    public StoreManager SetStore(StoreNames storeName, IStore store, bool replace = false)
    {
      if (!stores.ContainsKey(storeName) || replace)
      {
        stores[storeName] = new Tuple<IStore, IState>(store, store.InitialState);
      }

      return this;
    }

    public StoreManager Commit(StoreNames storeName, StoreActions action, IActionData data)
    {
      if (!stores.TryGetValue(storeName, out var store1))
      {
        if (DebugConfigurations.DebugEnabled)
        {
          throw new InvalidStoreNameException(storeName);
        }

        return this;
      }

      var oldState = store1.Item2;
      var store = store1.Item1;
      var newState = store.Commit(oldState, action, data);
      stores[storeName] = new Tuple<IStore, IState>(store, newState);

      InvokeObservers(storeName, newState);

      return this;
    }

    public StoreManager Commit<T>(StoreNames storeName, StoreActions action, ref T data) where T : struct, IActionData
    {
      if (!stores.TryGetValue(storeName, out var store1))
      {
        if (DebugConfigurations.DebugEnabled)
        {
          throw new InvalidStoreNameException(storeName);
        }

        return this;
      }

      var oldState = store1.Item2;
      var store = store1.Item1;
      
      IState newState;
      if (store is IStore<T> genericStore)
      {
        newState = genericStore.Commit(oldState, action, ref data);
      }
      else
      {
        newState = store.Commit(oldState, action, data);
      }

      stores[storeName] = new Tuple<IStore, IState>(store, newState);

      InvokeObservers(storeName, newState);

      return this;
    }

    private void InvokeObservers(StoreNames storeName, IState newState)
    {
      try
      {
        if (observers.ContainsKey(storeName))
        {
          var list = observers[storeName];
          for (int i = list.Count - 1; i >= 0; i--)
          {
            if (list[i] == null) list.RemoveAt(i);
          }
          
          foreach (var handler in list)
          {
            handler.OnStoreChanged(storeName, newState);
          }
        }
      }
      catch (StackOverflowException e)
      {
        Debug.LogError($"Seems an Stack Overflow Exception has been throw during state change: {e.Message}");
      }
    }

    public StoreManager Subscribe(StoreNames storeName, IStoreChangedHandler handler)
    {
      if (!observers.ContainsKey(storeName))
      {
        observers.Add(storeName, new List<IStoreChangedHandler>());
      }

      observers[storeName].Add(handler);
      return this;
    }

    public StoreManager Unsubscribe(IStoreChangedHandler handler)
    {
      foreach (var storeObserver in observers)
      {
        storeObserver.Value.Remove(handler);
      }

      return this;
    }

    public State GetState<State>(StoreNames storeName) where State : struct, IState
    {
      if (!stores.TryGetValue(storeName, value: out var store))
      {
        throw new InvalidStoreNameException(storeName);
      }

      return (State)store.Item2;
    }

    public void Destroy()
    {
      _destroyed = true;
      stores.Clear();
      observers.Clear();
    }
  }
}