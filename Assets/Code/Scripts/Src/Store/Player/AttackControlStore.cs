using System;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Src.Store.Player
{
  public class AttackControlConfigurations
  {
    public int AttackPerRound { get; set; }
    public float AttackCoolingTime { get; set; }
    public float ReloadTime { get; set; }

    public AttackControlConfigurations Clone()
    {
      return (AttackControlConfigurations)MemberwiseClone();
    }
  }

  public struct AttackControlData
  {
    public AttackControlConfigurations AttackControlConfigurations { get; set; }
    public int RemainingRoundCount { get; set; }
    public float CoolingCountdown { get; set; }
    public float ReloadCountdown { get; set; }
    public bool Reloading { get; set; }
    public bool Cooling { get; set; }
    public bool InAttackingState { get; set; }
  }

  public struct AttackControlState : IState
  {
    public int NumberOfPlayers { get; set; }
    public bool[] PlayerAlive { get; set; }
    public AttackControlData[] AttackControlDatas { get; set; }
  }

  public struct AttackControlActionData : IActionData
  {
    public int NumberOfPlayers { get; set; }
    public int PlayerNumber { get; set; }
    public bool PlayerAlive { get; set; }
    public AttackControlConfigurations AttackControlConfigurations { get; set; }
    public int AttackPerRound { get; set; }
    public int RemainingRoundCount { get; set; }
    public float CoolingCountdown { get; set; }
    public float ReloadTime { get; set; }
    public float ReloadCountdown { get; set; }
    public bool Reloading { get; set; }
    public bool Cooling { get; set; }
    public float CoolingTime { get; set; }
    public bool InAttackingState { get; set; } // This determines player facing control
  }

  [Serializable]
  public class AttackControlUpgrade : IActionData
  {
    public int PlayerNumber { get; set; }

    public float ReloadCountdownDecrementByPercentage { get; set; }

    public float CoolingCountdownDecrementByPercentage { get; set; }
  }

  public class AttackControlStore : IStore, IStore<AttackControlActionData>
  {
    public IState InitialState => new AttackControlState();

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      if (action == StoreActions.AttackControlStore_APPLY_ATTACK_UPGRADE)
      {
        AttackControlState acs = (AttackControlState)state;
        var attackUpgrade = (AttackControlUpgrade)data;

        acs.AttackControlDatas[attackUpgrade.PlayerNumber].ReloadCountdown *=
          (1 + attackUpgrade.ReloadCountdownDecrementByPercentage);
        acs.AttackControlDatas[attackUpgrade.PlayerNumber].CoolingCountdown *=
          (1 + attackUpgrade.CoolingCountdownDecrementByPercentage);

        return acs;
      }

      if (data == null) return Commit(state, action, default(AttackControlActionData));
      return Commit(state, action, (AttackControlActionData)data);
    }

    public IState Commit(IState state, StoreActions action, AttackControlActionData attackControlActionData)
    {
      AttackControlState attackControlState = (AttackControlState)state;
      AttackControlConfigurations acc;

      switch (action)
      {
        case StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS:
          attackControlState.NumberOfPlayers = attackControlActionData.NumberOfPlayers;
          attackControlState.PlayerAlive ??= new int[GeneralConfigurations.MaximumPlayers]
            .Select((_, index) => index < attackControlActionData.NumberOfPlayers).ToArray();
          attackControlState.AttackControlDatas ??= new AttackControlData[attackControlActionData.NumberOfPlayers];
          return attackControlState;
        case StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE:
          attackControlState.PlayerAlive[attackControlActionData.PlayerNumber] = attackControlActionData.PlayerAlive;
          attackControlState.NumberOfPlayers = attackControlState.PlayerAlive.Count(p => p);
          return attackControlState;
        case StoreActions.AttackControlStore_SET_ATTACK_CONTROL_STATE:
          if (attackControlActionData.AttackControlConfigurations != null)
          {
            acc = ObjectCopier.Clone(attackControlActionData.AttackControlConfigurations);

            acc.AttackPerRound = Mathf.Min(attackControlActionData.AttackControlConfigurations.AttackPerRound,
              GeneralConfigurations.MaximumAttackPerRound);
            acc.AttackCoolingTime = Mathf.Max(acc.AttackCoolingTime, GeneralConfigurations.MinimumCoolingTime);
            attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].AttackControlConfigurations =
              acc;
          }

          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].RemainingRoundCount =
            attackControlActionData.RemainingRoundCount;
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].CoolingCountdown =
            attackControlActionData.CoolingCountdown;
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].ReloadCountdown =
            attackControlActionData.ReloadCountdown;
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].Reloading =
            attackControlActionData.Reloading;
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].Cooling =
            attackControlActionData.Cooling;
          return attackControlState;
        case StoreActions.AttackControlStore_SET_IN_ATTACKING_STATE:
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].InAttackingState =
            attackControlActionData.InAttackingState;
          return attackControlState;
        case StoreActions.AttackControlStore_SET_ATTACK_CONTROL_CONFIGURATIONS:
          acc = ObjectCopier.Clone(attackControlActionData.AttackControlConfigurations);
          var stateAcc = attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber]
            .AttackControlConfigurations;

          stateAcc.AttackCoolingTime = Mathf.Max(acc.AttackCoolingTime, GeneralConfigurations.MinimumCoolingTime);
          stateAcc.ReloadTime = Mathf.Max(acc.ReloadTime, GeneralConfigurations.MinimumReloadTime);
          stateAcc.AttackPerRound = acc.AttackPerRound != 0 ? acc.AttackPerRound : stateAcc.AttackPerRound;

          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].AttackControlConfigurations =
            stateAcc;
          return attackControlState;
        case StoreActions.AttackControlStore_SET_ATTACK_PER_ROUND:
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].AttackControlConfigurations
            .AttackPerRound = Mathf.Min(attackControlActionData.AttackPerRound,
            GeneralConfigurations.MaximumAttackPerRound);
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].RemainingRoundCount =
            attackControlActionData.AttackPerRound;
          return attackControlState;
        case StoreActions.AttackControlStore_SET_REMAINING_ROUND_COUNT:
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].RemainingRoundCount =
            Mathf.Min(attackControlActionData.RemainingRoundCount, GeneralConfigurations.MaximumAttackPerRound);
          return attackControlState;
        case StoreActions.AttackControlStore_SET_COOLING_TIME:
          var coolingTime = Mathf.Clamp(attackControlActionData.CoolingTime, GeneralConfigurations.MinimumCoolingTime,
            float.MaxValue);
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].AttackControlConfigurations
            .AttackCoolingTime = coolingTime;
          return attackControlState;
        case StoreActions.AttackControlStore_SET_COOLING_COUNTDOWN:
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].CoolingCountdown =
            attackControlActionData.CoolingCountdown;
          return attackControlState;
        case StoreActions.AttackControlStore_SET_RELOAD_TIME:
          var reloadTime = attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber]
            .AttackControlConfigurations.ReloadTime;
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].AttackControlConfigurations
            .ReloadTime = Mathf.Clamp(attackControlActionData.ReloadTime, GeneralConfigurations.MinimumReloadTime,
            reloadTime);
          return attackControlState;
        case StoreActions.AttackControlStore_SET_RELOAD_COUNTDOWN:
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].ReloadCountdown =
            attackControlActionData.ReloadCountdown;
          return attackControlState;
        case StoreActions.AttackControlStore_SET_RELOADING:
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].Reloading =
            attackControlActionData.Reloading;
          return attackControlState;
        case StoreActions.AttackControlStore_SET_COOLING:
          attackControlState.AttackControlDatas[attackControlActionData.PlayerNumber].Cooling =
            attackControlActionData.Cooling;
          return attackControlState;
        case StoreActions.AttackControlStore_APPLY_ATTACK_UPGRADE:
          // We can skip boxing here, but the generic signature requires AttackControlActionData. 
          // If we pass AttackControlUpgrade, it won't match T, so it uses the IActionData signature in IStore.
          return attackControlState;
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}