using UnityEngine;

namespace Code.Scripts.Src.Store.Level
{
  public enum GameStates
  {
    NORMAL,
    PAUSED_MENU,  // This state will trigger pause menu open
    PAUSED_OTHERS,
    UPGRADING,
    PICKED_GEM,
    ONLY_PLAYER_CAN_MOVE,
    ONLY_ANIMATION_CAN_PLAY,
    GAME_OVER,
  }

  public struct GameState : IState
  {
    public GameStates CurrentGameState { get; set; }
  }

  public struct GameStateData : IActionData
  {
    public GameStates GameState { get; set; }
  }

  public class GameStateStore : IStore
  {
    public IState InitialState => new GameState();

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      GameState gameState = (GameState)state;
      GameStateData gameStateData = (GameStateData)data;

      switch (action)
      {
        case StoreActions.GameStateStore_SET_GAME_STATE:
          gameState.CurrentGameState = gameStateData.GameState;
          if (gameState.CurrentGameState.Equals(GameStates.PAUSED_MENU))
          {
            Time.timeScale = 0;
          }
          else
          {
            Time.timeScale = 1;
          }

          return gameState;
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}