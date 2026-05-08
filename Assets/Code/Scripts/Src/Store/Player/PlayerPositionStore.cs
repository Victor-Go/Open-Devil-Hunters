using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using UnityEngine;

namespace Code.Scripts.Src.Store.Player
{
  public struct PlayerPositionState : IState
  {
    public int NumberOfPlayers { get; set; }
    public bool[] PlayerAlive { get; set; }
    public Vector2[] PlayerPositions { get; set; }
    public Transform[] PlayerTransforms { get; set; }
    public Vector2 CenterPosition { get; set; }
  }

  public struct PlayerPositionData : IActionData
  {
    public int NumberOfPlayers { get; set; }
    public int PlayerNumber { get; set; }
    public bool PlayerAlive { get; set; }
    public Vector2 ReportedPlayerPosition { get; set; }
    public Transform PlayerTransform { get; set; }
  }

  public class PlayerPositionStore : IStore
  {
    private Vector2 GetCenterPosition(PlayerPositionState playerPositionState)
    {
      if (!playerPositionState.PlayerAlive.Any(a => a))
      {
        return playerPositionState.CenterPosition;
      }

      var sum = Vector2.zero;
      for (var i = 0; i < playerPositionState.PlayerAlive.Length; i++)
      {
        if (playerPositionState.PlayerAlive[i])
        {
          sum += playerPositionState.PlayerPositions[i];
        }
      }

      return sum / playerPositionState.PlayerAlive.Count(a => a);
    }

    public IState InitialState
    {
      get
      {
        var state = new PlayerPositionState()
        {
          CenterPosition = Vector2.zero
        };

        var playerPositions = new Vector2[GeneralConfigurations.MaximumPlayers];
        var playerTransforms = new Transform[GeneralConfigurations.MaximumPlayers];
        for (int playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
        {
          playerPositions[playerNumber] = Vector2.zero;
        }

        state.PlayerPositions = playerPositions;
        state.PlayerTransforms = playerTransforms;

        return state;
      }
    }

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      PlayerPositionState playerPositionState = (PlayerPositionState)state;
      PlayerPositionData playerPositionData = (PlayerPositionData)data;

      switch (action)
      {
        case StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS:
          int numberOfPlayers = playerPositionData.NumberOfPlayers;
          playerPositionState.NumberOfPlayers = numberOfPlayers;
          playerPositionState.PlayerAlive ??= new int[GeneralConfigurations.MaximumPlayers]
            .Select((_, index) => index < numberOfPlayers).ToArray();
          playerPositionState.PlayerPositions ??= new Vector2[numberOfPlayers];
          playerPositionState.PlayerTransforms ??= new Transform[numberOfPlayers];
          return playerPositionState;
        case StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE:
          playerPositionState.PlayerAlive[playerPositionData.PlayerNumber] = playerPositionData.PlayerAlive;
          playerPositionState.CenterPosition = GetCenterPosition(playerPositionState);
          playerPositionState.NumberOfPlayers = playerPositionState.PlayerAlive.Count(p => p);
          return playerPositionState;
        case StoreActions.PlayerPositionStore_SET_PLAYER_TRANSFORM:
          playerPositionState.PlayerTransforms[playerPositionData.PlayerNumber] = playerPositionData.PlayerTransform;
          return playerPositionState;
        case StoreActions.PlayerPositionStore_SET_PLAYER_POSITION:
          playerPositionState.PlayerPositions[playerPositionData.PlayerNumber] =
            playerPositionData.ReportedPlayerPosition;
          playerPositionState.CenterPosition = GetCenterPosition(playerPositionState);
          return playerPositionState;
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}