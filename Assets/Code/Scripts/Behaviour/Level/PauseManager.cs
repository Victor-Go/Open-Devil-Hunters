using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Behaviour.UI.Pause;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using UnityEngine;

namespace Code.Scripts.Behaviour.Level
{
  public class PauseManager : MonoBehaviour
  {
    private StoreManager storeManager;
    private PauseUI pauseUi;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
    }

    private void ShowPauseUi()
    {
      pauseUi = WindowManager.Instance.OpenWindow(WindowNames.PauseUI, OpenWindowActions.Show).GetComponent<PauseUI>();
    }

    private void OnApplicationFocus(bool focus)
    {
      if (!focus)
      {
        var currentGameState = storeManager.GetState<GameState>(StoreNames.GameStateStore).CurrentGameState;

        switch (currentGameState)
        {
          case GameStates.NORMAL:
          case GameStates.ONLY_PLAYER_CAN_MOVE:
            if (!DebugConfigurations.DebugEnabled)
            {
              ShowPauseUi();
            }

            break;
        }
      }
    }

    private void Update()
    {
      bool toggleMenu = Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7);

      if (toggleMenu)
      {
        var currentGameState = storeManager.GetState<GameState>(StoreNames.GameStateStore).CurrentGameState;

        switch (currentGameState)
        {
          case GameStates.PAUSED_MENU:
            pauseUi.Resume();
            pauseUi = null;
            break;
          case GameStates.NORMAL:
          case GameStates.ONLY_PLAYER_CAN_MOVE:
            ShowPauseUi();
            break;
          case GameStates.PAUSED_OTHERS:
          case GameStates.PICKED_GEM:
          case GameStates.UPGRADING:
          case GameStates.GAME_OVER:
            // Ignore.
            break;
        }
      }
    }
  }
}