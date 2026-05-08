using Code.Scripts.Src.Store.Level;

namespace Code.Scripts.Src.Utils
{
  public static class LevelUtils
  {
    public static bool CanNotMove()
    {
      StoreManager storeManager = StoreManager.Instance;
      return CanNotMove(storeManager);
    }

    public static bool CanNotMove(StoreManager storeManager)
    {
      var gameState = storeManager.GetState<GameState>(StoreNames.GameStateStore).CurrentGameState;
      return EverythingCanNotMove(gameState);
    }

    public static bool EverythingCanNotMove(GameStates gameState)
    {
      return !gameState.Equals(GameStates.NORMAL);
    }

    public static bool PlayerCanMove(GameStates gameState)
    {
      return gameState.Equals(GameStates.NORMAL) || gameState.Equals(GameStates.ONLY_PLAYER_CAN_MOVE);
    }

    public static bool PlayerAnimationCanPlay(GameStates gameState)
    {
      return gameState.Equals(GameStates.NORMAL) || gameState.Equals(GameStates.ONLY_ANIMATION_CAN_PLAY) ||
             gameState.Equals(GameStates.ONLY_PLAYER_CAN_MOVE);
    }
  }
}