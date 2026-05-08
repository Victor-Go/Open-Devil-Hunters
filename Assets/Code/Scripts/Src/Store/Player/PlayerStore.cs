using System;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Configurations;
using UnityEngine;

namespace Code.Scripts.Src.Store.Player
{
  public class PlayerData
  {
    public bool PlayerDied { get; set; }
    public float MaximumHp { get; set; }
    public float CurrentHp { get; set; }
    public int Level { get; set; }
    public float MovingSpeed { get; set; }
    public float AttackingMovingSpeed { get; set; }
    public float PickUpRadius { get; set; }
    public float CurrentExperience { get; set; }
    public float CurrentLevelExperience { get; set; }
    public float RequiredExperienceToLevelUp { get; set; }
    public CharacterStatus PlayerStatus { get; set; }
    public bool Poisoned { get; set; }
    public float ImmortalSecondAfterHurt { get; set; }
    public int DeadTime { get; set; }
  }

  public class PlayerState : IState
  {
    public int NumberOfPlayers { get; set; }
    public bool[] PlayerAlive { get; set; }
    public PlayerData[] PlayerDatas { get; set; } = new PlayerData[GeneralConfigurations.MaximumPlayers];
  }

  public struct PlayerActionData : IActionData
  {
    public int NumberOfPlayers { get; set; }
    public int PlayerNumber { get; set; }
    public bool PlayerAlive { get; set; }
    public float CurrentHp { get; set; }
    public float AddCurrentHp { get; set; }
    public float AddCurrentHpPercentage { get; set; }
    public float MaximumHp { get; set; }
    public float AugmentMaximumHp { get; set; }
    public bool DoNotAugmentCurrentHp { get; set; }
    public int Level { get; set; }
    public float MovingSpeed { get; set; }
    public float AttackingMovingSpeed { get; set; }
    public float PickUpRadius { get; set; }
    public float CurrentExperience { get; set; }
    public float Experience { get; set; }
    public float CurrentLevelExperience { get; set; }
    public float RequiredExperienceToLevelUp { get; set; }
    public CharacterStatus PlayerStatus { get; set; }
    public bool Poisoned { get; set; }
    public float ImmortalSecondAfterHurt { get; set; }
    public int DeadTime { get; set; }
  }

  [Serializable]
  public class MovementUpgrade : IActionData
  {
    public int PlayerNumber { get; set; }

    public float AugmentMovingSpeedByValue { get; set; }

    public float AugmentMovingSpeedByPercentage { get; set; }

    public float AugmentAttackingMovingSpeedByValue { get; set; }

    public float AugmentAttackingMovingSpeedByPercentage { get; set; }
  }

  public class PlayerStore : IStore
  {
    public IState InitialState
    {
      get
      {
        var state = new PlayerState();
        for (var i = 0; i < GeneralConfigurations.MaximumPlayers; i++)
        {
          var playerData = new PlayerData()
          {
            Level = 0,
            RequiredExperienceToLevelUp = PlayerExperienceConfigurations.GetLevelUpRequiredExperience(0),
            ImmortalSecondAfterHurt = GeneralConfigurations.PlayerInitData.ImmortalSeconds,
            CurrentExperience = 0,
          };
          state.PlayerDatas[i] = playerData;
        }

        return state;
      }
    }

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      var playerState = (PlayerState)state;

      int playerNumber = 0;
      PlayerActionData playerData = default;
      if (data is PlayerActionData actionData)
      {
        playerData = actionData;
        playerNumber = playerData.PlayerNumber;
      }

      float maxHp, currentHp;
      float newMovingSpeed, newAttackingMovingSpeed;

      switch (action)
      {
        case StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS:
          playerState.NumberOfPlayers = playerData.NumberOfPlayers;
          playerState.PlayerAlive =
            new int[GeneralConfigurations.MaximumPlayers]
              .Select((_, index) => index < playerData.NumberOfPlayers).ToArray();
          return playerState;
        case StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE:
          playerState.PlayerAlive[playerData.PlayerNumber] = playerData.PlayerAlive;
          playerState.PlayerDatas[playerNumber].PlayerDied = !playerData.PlayerAlive;
          playerState.NumberOfPlayers = playerState.PlayerAlive.Count(p => p);
          return playerState;
        case StoreActions.PlayerStore_INITIALIZE:
          playerState.PlayerDatas[playerNumber].PickUpRadius = playerData.PickUpRadius;
          playerState.PlayerDatas[playerNumber].MovingSpeed = playerData.MovingSpeed;
          playerState.PlayerDatas[playerNumber].AttackingMovingSpeed = playerData.AttackingMovingSpeed;
          playerState.PlayerDatas[playerNumber].MaximumHp = playerData.MaximumHp;

          playerState.PlayerDatas[playerNumber].CurrentHp =
            playerData.CurrentHp != 0 ? playerData.CurrentHp : playerData.MaximumHp;

          return playerState;
        case StoreActions.PlayerStore_SET_CURRENT_HP:
          playerState.PlayerDatas[playerNumber].CurrentHp =
            Mathf.Min(playerData.CurrentHp, playerState.PlayerDatas[playerNumber].MaximumHp);
          return playerState;
        case StoreActions.PlayerStore_ADD_CURRENT_HP:
          playerState.PlayerDatas[playerNumber].CurrentHp += playerData.AddCurrentHp;
          playerState.PlayerDatas[playerNumber].CurrentHp = Mathf.Min(playerState.PlayerDatas[playerNumber].CurrentHp,
            playerState.PlayerDatas[playerNumber].MaximumHp);
          return playerState;
        case StoreActions.PlayerStore_ADD_CURRENT_HP_PERCENTAGE:
          playerState.PlayerDatas[playerNumber].CurrentHp +=
            playerState.PlayerDatas[playerNumber].MaximumHp * playerData.AddCurrentHpPercentage;
          playerState.PlayerDatas[playerNumber].CurrentHp = Mathf.Min(playerState.PlayerDatas[playerNumber].CurrentHp,
            playerState.PlayerDatas[playerNumber].MaximumHp);
          return playerState;
        case StoreActions.PlayerStore_SET_MAXIMUM_HP:

          maxHp = Mathf.Clamp(
            playerData.MaximumHp,
            0,
            GeneralConfigurations.MaximumHp
          );
          playerState.PlayerDatas[playerNumber].MaximumHp = maxHp;
          return playerState;
        case StoreActions.PlayerStore_AUGMENT_MAXIMUM_HP:
          maxHp = playerState.PlayerDatas[playerNumber].MaximumHp + playerData.AugmentMaximumHp;
          maxHp = Mathf.Clamp(maxHp, 0, GeneralConfigurations.MaximumHp);

          var doNotAugmentCurrentHp = playerData.DoNotAugmentCurrentHp;
          if (!doNotAugmentCurrentHp)
          {
            currentHp = playerState.PlayerDatas[playerNumber].CurrentHp + playerData.AugmentMaximumHp;
            currentHp = Mathf.Min(currentHp, maxHp);
            playerState.PlayerDatas[playerNumber].CurrentHp = currentHp;
          }

          playerState.PlayerDatas[playerNumber].MaximumHp = maxHp;
          return playerState;
        case StoreActions.PlayerStore_SET_LEVEL_UP_DATA:
          playerState.PlayerDatas[playerNumber].Level = playerData.Level;
          playerState.PlayerDatas[playerNumber].CurrentLevelExperience = playerData.CurrentLevelExperience;
          playerState.PlayerDatas[playerNumber].RequiredExperienceToLevelUp = playerData.RequiredExperienceToLevelUp;
          return playerState;
        case StoreActions.PlayerStore_SET_MOVING_SPEED:
          playerState.PlayerDatas[playerNumber].MovingSpeed = Mathf.Clamp(
            playerData.MovingSpeed,
            0,
            GeneralConfigurations.PlayerMaxSpeed);
          return playerState;
        case StoreActions.PlayerStore_SET_ATTACKING_MOVING_SPEED:
          var aMovingSpeed = Mathf.Clamp(playerData.AttackingMovingSpeed, 0,
            GeneralConfigurations.MaximumPlayerAttackingMovingSpeed);
          playerState.PlayerDatas[playerNumber].AttackingMovingSpeed = aMovingSpeed;

          var movingSpeed = playerState.PlayerDatas[playerNumber].MovingSpeed;
          if (aMovingSpeed > movingSpeed)
          {
            playerState.PlayerDatas[playerNumber].MovingSpeed = aMovingSpeed;
          }

          return playerState;
        case StoreActions.PlayerStore_SET_CURRENT_EXPERIENCE:
          playerState.PlayerDatas[playerNumber].CurrentExperience = playerData.CurrentExperience;
          return playerState;
        case StoreActions.PlayerStore_ADD_CURRENT_EXPERIENCE:
          playerState.PlayerDatas[playerNumber].CurrentExperience += playerData.Experience;
          return playerState;
        case StoreActions.PlayerStore_SET_PICK_UP_RADIUS:
          var pickUpRadius = playerData.PickUpRadius;
          playerState.PlayerDatas[playerNumber].PickUpRadius = Mathf.Clamp(
            pickUpRadius,
            0.5f,
            GeneralConfigurations.MaximumPickRadius);
          return playerState;
        case StoreActions.PlayerStore_APPLY_MOVEMENT_UPGRADE:
          var initialMovingSpeed = playerState.PlayerDatas[playerNumber].MovingSpeed;
          var initialAttackingMovingSpeed = playerState.PlayerDatas[playerNumber].AttackingMovingSpeed;

          var movementUpgrade = (MovementUpgrade)data;
          newMovingSpeed = initialMovingSpeed * (1 + movementUpgrade.AugmentMovingSpeedByPercentage);
          newMovingSpeed += movementUpgrade.AugmentMovingSpeedByValue;

          newAttackingMovingSpeed =
            initialAttackingMovingSpeed * (1 + movementUpgrade.AugmentAttackingMovingSpeedByPercentage);
          newAttackingMovingSpeed += movementUpgrade.AugmentAttackingMovingSpeedByValue;

          newMovingSpeed = Mathf.Clamp(Mathf.RoundToInt(newMovingSpeed * 100) / 100f, 0.1f,
            GeneralConfigurations.PlayerMaxSpeed);
          newAttackingMovingSpeed = Mathf.Clamp(Mathf.RoundToInt(newAttackingMovingSpeed * 100) / 100f, 0.1f,
            GeneralConfigurations.MaximumPlayerAttackingMovingSpeed);

          playerState.PlayerDatas[playerNumber].MovingSpeed = newMovingSpeed;
          playerState.PlayerDatas[playerNumber].AttackingMovingSpeed = newAttackingMovingSpeed;

          return playerState;
        case StoreActions.PlayerStore_SET_PLAYER_STATUS:
          playerState.PlayerDatas[playerNumber].PlayerStatus = playerData.PlayerStatus;
          return playerState;
        case StoreActions.PlayerStore_SET_PLAYER_POISONED:
          playerState.PlayerDatas[playerNumber].Poisoned = playerData.Poisoned;
          return playerState;
        case StoreActions.PlayerStore_SET_IMMORTAL_SECONDS:
          playerState.PlayerDatas[playerNumber].ImmortalSecondAfterHurt = playerData.ImmortalSecondAfterHurt;
          return playerState;
        case StoreActions.PlayerStore_SET_DEAD_TIME:
          playerState.PlayerDatas[playerNumber].DeadTime = playerData.DeadTime;
          playerState.PlayerDatas[playerNumber].PlayerDied = true;
          playerState.PlayerAlive[playerData.PlayerNumber] = false;
          return playerState;
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}