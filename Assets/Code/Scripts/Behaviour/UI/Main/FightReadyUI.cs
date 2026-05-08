using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class FightReadyUI : UIAnimation, IStoreChangedHandler
  {
    private StoreManager storeManager;
    private ResourceManager resourceManager;

    private GameObject decreaseDifficultyGO;
    private GameObject increaseDifficultyGO;
    private Text levelText;
    private Text unlockedLevelText;
    private Text descriptionText;
    private Image littleBossImage;
    private Image bigBossImage;

    private MapNames currentMapName;
    private int levelDifficulty;
    private PlayModes currentPlayMode;
    private LevelEnemyConfiguration currentEnemyConfiguration;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      resourceManager = ResourceManager.Instance;

      targetTransform = transform.Find("FightReady").GetComponent<RectTransform>();
      decreaseDifficultyGO = targetTransform.Find("Difficulty/DecreaseDifficulty").gameObject;
      increaseDifficultyGO = targetTransform.Find("Difficulty/IncreaseDifficulty").gameObject;
      levelText = targetTransform.Find("Difficulty/Level").GetComponent<Text>();
      unlockedLevelText = targetTransform.Find("Difficulty/UnlockedLevel").GetComponent<Text>();
      descriptionText = targetTransform.Find("Description/Text").GetComponent<Text>();
      littleBossImage = targetTransform.Find("LittleBoss").GetComponent<Image>();
      bigBossImage = targetTransform.Find("BigBoss").GetComponent<Image>();
    }

    protected override void Start()
    {
      base.Start();

      storeManager.Subscribe(StoreNames.LevelConfigurationStore, this);

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      currentMapName = levelConfigs.MapConfiguration.MapName;
      levelDifficulty = levelConfigs.LevelDifficulty;
      currentPlayMode = levelConfigs.PlayMode;

      currentEnemyConfiguration =
        EnemyFactoryConfigurations.GenerateEnemyConfiguration(currentPlayMode, currentMapName, levelDifficulty);

      UpdateUI(levelConfigs);
    }

    protected override void OnWindowOpened()
    {
      base.OnWindowOpened();

      if (!PlayerPrefs.HasKey("ShowedFightReadyGuideUI") ||
          PlayerPrefs.GetInt("ShowedFightReadyGuideUI") != 1)
      {
        OpenGuideWindow();
      }
    }

    public void OpenGuideWindow()
    {
      OpenSubWindow(WindowNames.FightReadyGuideUI);
    }

    private void UpdateLocalData(LevelConfigurationState levelConfigs)
    {
      if (currentPlayMode == levelConfigs.PlayMode && currentMapName == levelConfigs.MapConfiguration.MapName &&
          levelDifficulty == levelConfigs.LevelDifficulty) return;

      currentPlayMode = levelConfigs.PlayMode;
      currentMapName = levelConfigs.MapConfiguration.MapName;
      levelDifficulty = levelConfigs.LevelDifficulty;

      currentEnemyConfiguration =
        EnemyFactoryConfigurations.GenerateEnemyConfiguration(currentPlayMode, currentMapName, levelDifficulty);
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          UpdateLocalData((LevelConfigurationState)state);
          UpdateUI((LevelConfigurationState)state);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void UpdateDescription(PlayModes playMode, string descriptionName, MonsterAttributeRatio ratios)
    {
      var description = "";
      description += playMode == PlayModes.CASUAL
        ? $"{I18nUtils.GetText("UI/FightReady/ModeDescription/CasualMode")}\n"
        : "";

      description += !string.IsNullOrEmpty(descriptionName)
        ? string.Format(I18nUtils.GetText(descriptionName)) + "\n"
        : "";

      description += string.Format(I18nUtils.GetText("UI/FightReady/AttributeDescription") + "\n",
        Mathf.RoundToInt(ratios.Physical * 100),
        Mathf.RoundToInt(ratios.Ice * 100),
        Mathf.RoundToInt(ratios.Fire * 100),
        Mathf.RoundToInt(ratios.Thunder * 100),
        Mathf.RoundToInt(ratios.Poison * 100)
      );

      switch (playMode)
      {
        case PlayModes.CASUAL:
          description += string.Format(I18nUtils.GetText("UI/FightReady/ModeDescription/BossAppearTime"),
            GeneralConfigurations.CasualModeDuration);
          break;
        case PlayModes.STANDARD:
          description += string.Format(I18nUtils.GetText("UI/FightReady/ModeDescription/BossAppearTime"),
            GeneralConfigurations.NormalModeDuration);
          break;
      }

      descriptionText.text = description;
    }

    private void UpdateUI(LevelConfigurationState levelConfigs)
    {
      var littleTex = Resources.Load<Texture2D>(levelConfigs.MapConfiguration.LittleBossAvatarName);
      var bigTex = Resources.Load<Texture2D>(levelConfigs.MapConfiguration.BigBossAvatarName);

      littleBossImage.sprite = Sprite.Create(littleTex, new Rect(0, 0, littleTex.width, littleTex.height),
        Vector2.one / 2);
      bigBossImage.sprite = Sprite.Create(bigTex, new Rect(0, 0, bigTex.width, bigTex.height), Vector2.one / 2);

      levelText.text = string.Format(I18nUtils.GetText("UI/FightReady/DifficultyNumber"), levelDifficulty + 1);

      var ratios = currentEnemyConfiguration.MonsterAttributeRatio;
      UpdateDescription(levelConfigs.PlayMode, $"<b>{currentEnemyConfiguration.MapDescriptionName}</b>", ratios);

      var gameData = storeManager.GetState<GameDataState>(StoreNames.GameDataStore);
      var currentMaxDifficulty = gameData.UnlockedDifficulties[currentMapName];

      if (currentMaxDifficulty < levelDifficulty)
      {
        storeManager.Commit(StoreNames.LevelConfigurationStore,
          StoreActions.LevelConfigurationStore_SET_LEVEL_DIFFICULTY, new LevelConfigurationData()
          {
            LevelDifficulty = currentMaxDifficulty
          });
        return;
      }

      unlockedLevelText.text = string.Format(I18nUtils.GetText("UI/FightReady/UnlockedDifficulty"),
        currentMaxDifficulty + 1, GeneralConfigurations.LevelDifficultyCount);

      decreaseDifficultyGO.SetActive(true);
      increaseDifficultyGO.SetActive(true);
      if (levelDifficulty <= 0)
      {
        decreaseDifficultyGO.SetActive(false);
      }

      if (levelDifficulty >= 9 || levelDifficulty >= currentMaxDifficulty)
      {
        increaseDifficultyGO.SetActive(false);
      }
    }

    public void DecreaseLevel()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);

      if (levelConfigs.LevelDifficulty > 0)
      {
        storeManager.Commit(StoreNames.LevelConfigurationStore,
          StoreActions.LevelConfigurationStore_SET_LEVEL_DIFFICULTY, new LevelConfigurationData()
          {
            LevelDifficulty = levelConfigs.LevelDifficulty - 1
          });
      }
    }

    public void IncreaseLevel()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      var localDifficulty = levelConfigs.LevelDifficulty;

      var gameData = storeManager.GetState<GameDataState>(StoreNames.GameDataStore);
      var currentMap = levelConfigs.MapConfiguration.MapName;

      if (localDifficulty < GeneralConfigurations.LevelDifficultyCount &&
          localDifficulty < gameData.UnlockedDifficulties[currentMap])
      {
        storeManager.Commit(StoreNames.LevelConfigurationStore,
          StoreActions.LevelConfigurationStore_SET_LEVEL_DIFFICULTY, new LevelConfigurationData()
          {
            LevelDifficulty = levelConfigs.LevelDifficulty + 1
          });
      }
    }

    public void GoBack()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      CloseWindow();
    }

    public void Play()
    {
      storeManager.Commit(StoreNames.LevelConfigurationStore,
        StoreActions.LevelConfigurationStore_SET_ENEMY_CONFIGURATION,
        new LevelConfigurationData()
        {
          EnemyConfiguration = currentEnemyConfiguration
        });

      ObjectPool.Instance.Reset();
      SceneManager.LoadScene("Battlefield");
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}