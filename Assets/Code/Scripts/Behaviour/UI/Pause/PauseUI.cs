using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.Behaviour.UI.Pause
{
  public class PauseUI : UIWindow
  {
    private StoreManager storeManager;
    private ResourceManager resourceManager;

    private GameObject resumeButton;

    private GameStates previousState;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      resourceManager = ResourceManager.Instance;

      resumeButton = transform.Find("Buttons/Resume").gameObject;
    }

    public override void OnWindowOpen()
    {
      base.OnWindowOpen();
      storeManager.Commit(StoreNames.BackgroundCanvas, StoreActions.BackgroundCanvas_SET_ACTIVE,
        new BackgroundCanvasActionData() { Active = true });

      previousState = storeManager.GetState<GameState>(StoreNames.GameStateStore).CurrentGameState;
      storeManager.Commit(StoreNames.GameStateStore, StoreActions.GameStateStore_SET_GAME_STATE,
        new GameStateData() { GameState = GameStates.PAUSED_MENU });

      EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    public void Resume()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      storeManager.Commit(StoreNames.BackgroundCanvas, StoreActions.BackgroundCanvas_SET_ACTIVE,
        new BackgroundCanvasActionData() { Active = false });
      storeManager.Commit(StoreNames.GameStateStore, StoreActions.GameStateStore_SET_GAME_STATE,
        new GameStateData() { GameState = previousState });
      CloseWindow();
    }

    public void OpenOptionsWindow()
    {
      OpenSubWindow(WindowNames.OptionsUI);
    }

    public void ShowControlGuide()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      OpenSubWindow(WindowNames.ControlGuide);
    }

    public void ExitLevel()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      storeManager.Commit(StoreNames.BackgroundCanvas, StoreActions.BackgroundCanvas_SET_ACTIVE,
        new BackgroundCanvasActionData() { Active = false });

      Mailer.Instance.SendMail(MailSenders.ROUTE_REQUEST, MailAddresses.MAIN_ROUTER, new RouteRequest()
      {
        Route = "FightPreparation"
      });

      Mailer.Instance.SendMail(MailSenders.GAME_OVER_REQUEST, MailAddresses.GAME_OVER, new GameOverRequest()
      {
        Status = GameOverStatus.Aborted,
      });

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      var timePassed = storeManager.GetState<LevelTimePassedState>(StoreNames.LevelTimePassedStore).LevelTimePassed;
      for (var playerNumber = 0; playerNumber < levelConfigs.PlayerAlive.Length; playerNumber++)
      {
        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_DEAD_TIME, new PlayerActionData()
        {
          PlayerNumber = playerNumber,
          DeadTime = timePassed,
        });
      }

      WindowManager.Instance.OpenWindow(WindowNames.GameOverUI, OpenWindowActions.CloseOthers);
    }
  }
}