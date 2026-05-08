using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Steam.Achievements;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.GameOver
{
  public class GameOverUI : UIWindow
  {
    [SerializeField] private float duration = 0.5f;

    private ResourceManager resourceManager;
    private StoreManager storeManager;
    private Scheduling scheduling;

    private Image backgroundImage;
    private float timeElapsed;
    private Color initColor;
    private Color finalColor;
    private bool fadingIn;
    private Text finalScoreText;
    private Text honorGainedText;
    private GameObject failText;
    private GameObject successText;
    private GameObject abortedText;
    private GameOverStatus gameOverStatus;
    private string scheduleId;

    private void Awake()
    {
      resourceManager = ResourceManager.Instance;
      storeManager = StoreManager.Instance;
      scheduling = Scheduling.Instance;

      backgroundImage = transform.Find("Background").GetComponent<Image>();
      var color = backgroundImage.color;
      initColor = new Color(color.r, color.g, color.b, 0);
      finalColor = color;

      finalScoreText = transform.Find("FinalScoreText").GetComponent<Text>();
      honorGainedText = transform.Find("HonorGainedText").GetComponent<Text>();

      failText = transform.Find("FailedText").gameObject;
      successText = transform.Find("SuccessText").gameObject;
      abortedText = transform.Find("AbortedText").gameObject;
    }

    private void Start()
    {
      fadingIn = true;

      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;

      ShowFinalDataAndSave();

      HandleGameOverStatus();
    }

    private void SetGameStatus(GameStates gameState)
    {
      storeManager.Commit(StoreNames.GameStateStore, StoreActions.GameStateStore_SET_GAME_STATE, new GameStateData()
      {
        GameState = gameState
      });
    }

    private void HandleGameOverStatus()
    {
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);

      var mails = Mailer.Instance.GetMails(MailAddresses.GAME_OVER);
      if (mails.Count != 1)
      {
        throw new System.Exception("The quantity of Game Over Mail is not 1.");
      }

      successText.SetActive(false);
      failText.SetActive(false);
      abortedText.SetActive(false);
      gameOverStatus = ((GameOverRequest)mails.First().MailContent).Status;
      switch (gameOverStatus)
      {
        case GameOverStatus.Success:
          successText.SetActive(true);
          var successClips = new[]
          {
            "AudioClip/success_0",
            "AudioClip/success_1",
          };
          AudioClip clip = resourceManager.GetResource(successClips[Random.Range(0, successClips.Length)]);
          AudioWrapper.PlayClip(clip, Vector2.zero);
          if (levelConfigs.InitialNumberOfPlayers == 2)
          {
            AchievementsManager.UnlockAchievement(Achievements.Brotherhood);
          }

          break;
        case GameOverStatus.Failed:
          failText.SetActive(true);
          if (levelConfigs.InitialNumberOfPlayers == 2)
          {
            AchievementsManager.UnlockAchievement(Achievements.Ace);
          }

          break;
        case GameOverStatus.Aborted:
          abortedText.SetActive(true);
          IgnoreFading();
          break;
      }

      SetGameStatus(GameStates.GAME_OVER);
    }

    private void IgnoreFading()
    {
      fadingIn = false;
      backgroundImage.color = finalColor;
    }

    private void ShowFinalDataAndSave()
    {
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      var finalScore = 0;
      for (var playerNumber = 0; playerNumber < levelConfigs.InitialNumberOfPlayers; playerNumber++)
      {
        var playerData = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
        var battleData = storeManager.GetState<BattleDataState>(StoreNames.BattleDataStore);

        var deadSeconds = playerData.PlayerDatas[playerNumber].DeadTime;
        var finalLevel = playerData.PlayerDatas[playerNumber].Level;
        var enemyKilled = battleData.KilledEnemies.Count(e => e.KilledByPlayerNumber == playerNumber);

        finalScore +=
          GeneralConfigurations.GetFinalScore(levelConfigs.LevelDifficulty, deadSeconds, finalLevel, enemyKilled);
      }

      if (gameOverStatus == GameOverStatus.Success)
      {
        finalScore = (int)(finalScore * 1.25f);
      }

      finalScoreText.text = $"{I18nUtils.GetText("UI/FinalScore")}: {finalScore}";

      var honorBonus = storeManager.GetState<LevelGeneralState>(StoreNames.LevelGeneralStore).HonorBonus;
      var finalHonor = Mathf.CeilToInt(GeneralConfigurations.GetFinalHonor(finalScore) * (1 + honorBonus));
      honorGainedText.text = $"{I18nUtils.GetText("UI/HonorGained")}: {finalHonor}";

      switch (levelConfigs.MapConfiguration.MapName)
      {
        case MapNames.FOREST: AchievementsManager.UnlockAchievement(Achievements.Tarzan); break;
        case MapNames.DESERT: AchievementsManager.UnlockAchievement(Achievements.Wustenfuchs); break;
        case MapNames.DUNGEON: AchievementsManager.UnlockAchievement(Achievements.DungeonMaster); break;
        case MapNames.GRAVEYARD: AchievementsManager.UnlockAchievement(Achievements.Buddha); break;
        case MapNames.HELL: AchievementsManager.UnlockAchievement(Achievements.DanteAlighitri); break;
      }

      storeManager.Commit(StoreNames.GameDataStore, StoreActions.GameDataStore_INCREASE_HONOR, new GameDataActionData()
      {
        PlayerHonor = finalHonor
      });
      storeManager.Commit(StoreNames.LevelGeneralStore, StoreActions.LevelGeneralStore_RESET, new LevelGeneralData());

      SaveSystem.SaveGame();
    }

    public void GoBackToMainMenu()
    {
      AudioWrapper.PlayClip(ResourceManager.Instance.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      Mailer.Instance.SendMail(MailSenders.ROUTE_REQUEST, MailAddresses.MAIN_ROUTER, new RouteRequest()
      {
        Route = "FightPreparation"
      });
      SceneManager.LoadScene("Main");
    }

    protected override void Update()
    {
      base.Update();

      if (fadingIn)
      {
        backgroundImage.color = Color.Lerp(initColor, finalColor, timeElapsed / duration);
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= duration)
        {
          fadingIn = false;
        }
      }
    }

    private void OnDestroy()
    {
      scheduling.ClearSchedule(scheduleId);
    }
  }
}