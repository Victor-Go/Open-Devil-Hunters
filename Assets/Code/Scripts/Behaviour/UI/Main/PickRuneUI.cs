using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.UI;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class PickRuneUI : UIAnimation, IStoreChangedHandler
  {
    private int _playerNumber;

    private StoreManager storeManager;
    private ResourceManager resourceManager;

    private GameObject player0Image;
    private GameObject player1Image;
    private Image runeImage;
    private GameObject maxLevelGO;
    private GameObject levelGO;
    private GameObject upgradeButton;
    private GameObject priceGO;
    private Text runeTitle;
    private Text runeDescription;

    private UIRuneInfo currentShowingRune;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      resourceManager = ResourceManager.Instance;

      targetTransform = transform.Find("PickRune").GetComponent<RectTransform>();

      runeImage = targetTransform.Find("Bottom/RuneImage").GetComponent<Image>();

      maxLevelGO = targetTransform.Find("Bottom/MaxLevel").gameObject;
      levelGO = targetTransform.Find("Bottom/LevelAndUpgrade/Level").gameObject;
      upgradeButton = targetTransform.Find("Bottom/LevelAndUpgrade/Upgrade").gameObject;
      priceGO = targetTransform.Find("Bottom/LevelAndUpgrade/Price").gameObject;

      runeTitle = targetTransform.Find("Bottom/RuneTitle").GetComponent<Text>();
      runeDescription = targetTransform.Find("Bottom/RuneDescription").GetComponent<Text>();
      player0Image = targetTransform.Find("Player0Image").gameObject;
      player1Image = targetTransform.Find("Player1Image").gameObject;
    }

    protected override void Start()
    {
      base.Start();

      storeManager
        .Subscribe(StoreNames.GameDataStore, this);
    }

    public void SetPlayerNumber(int playerNumber)
    {
      _playerNumber = playerNumber;

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      var tex = Resources.Load<Texture2D>(levelConfigs.PlayerConfigurations[playerNumber].AvatarImageIndicator);
      var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2);

      switch (playerNumber)
      {
        case 0:
          player1Image.SetActive(false);
          player0Image.GetComponent<Image>().sprite = sprite;
          break;
        case 1:
          player0Image.SetActive(false);
          player1Image.GetComponent<Image>().sprite = sprite;
          break;
      }

      var selectableRunes = GetComponentsInChildren<SelectableRune>();
      foreach (var rune in selectableRunes)
      {
        rune.PlayerNumber = playerNumber;
        rune.SetPickRuneUI(this);
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.GameDataStore:
          var unlockedRunes = ((GameDataState)state).UnlockedRunes;
          UpdateShowingRuneInfo(unlockedRunes);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    public void TryPickRune(RuneTypes runeType)
    {
      var rune = storeManager.GetState<GameDataState>(StoreNames.GameDataStore).UnlockedRunes
        .Find(rune => rune.RuneType == runeType);

      storeManager.Commit(StoreNames.LevelConfigurationStore, StoreActions.LevelConfigurationStore_SET_RUNE,
        new LevelConfigurationData()
        {
          PlayerNumber = _playerNumber,
          RuneType = runeType,
          RuneLevel = rune.RuneLevel
        });
    }

    private void ShowInfoBox(string title)
    {
      var infoBox = OpenSubWindow(WindowNames.InfoBoxUI);
      infoBox.GetComponent<InfoBoxUI>().SetTitle(title);
    }

    public void TryUnlockRune(RuneTypes runeType)
    {
      var unlockedRunes = storeManager.GetState<GameDataState>(StoreNames.GameDataStore).UnlockedRunes
        .Select(r => r.RuneType);
      if (!RuneConfigurations.Prerequisites[runeType].Any() ||
          RuneConfigurations.Prerequisites[runeType].Any(r => unlockedRunes.Contains(r)))
      {
        var gameData = storeManager.GetState<GameDataState>(StoreNames.GameDataStore);
        var @class = RuneUtils.GetRuneClass(runeType);

        var price = RuneConfigurations.PriceToUnlockRuneLevels[@class].Price[0];
        if (gameData.PlayerBalance >= price)
        {
          var confirmBox = OpenSubWindow(WindowNames.ConfirmBoxUI);
          confirmBox.GetComponent<ConfirmBoxUI>()
            .SetTitle(string.Format(I18nUtils.GetText("UI/Prompt/UnlockRune"),
              I18nUtils.GetText(RunePresets.Presets[runeType].TitleName), price))
            .SetConfirmAction(UnlockGenerator(runeType, price));
        }
        else
        {
          ShowInfoBox(string.Format(I18nUtils.GetText("UI/Info/Unlock/InsufficientBalance"), price));
        }
      }
      else
      {
        ShowInfoBox(I18nUtils.GetText("UI/Info/RunePrerequisitesNotSatisfied"));
      }
    }

    private Action UnlockGenerator(RuneTypes runeType, int price)
    {
      return () =>
      {
        storeManager.Commit(StoreNames.GameDataStore, StoreActions.GameDataStore_DECREASE_PLAYER_HONOR,
          new GameDataActionData()
          {
            PlayerHonor = price
          });

        storeManager.Commit(StoreNames.GameDataStore, StoreActions.GameDataStore_UNLOCK_RUNE, new GameDataActionData()
        {
          RuneType = runeType
        });

        SaveSystem.SaveGame();
      };
    }

    public void SetShowingRune(UIRuneInfo rune)
    {
      currentShowingRune = rune;

      runeImage.sprite = rune.RuneImage;

      runeTitle.text = I18nUtils.GetText(rune.RuneTitleName);
      runeDescription.text = I18nUtils.GetText(rune.RuneDescriptionName);

      var gameData = storeManager.GetState<GameDataState>(StoreNames.GameDataStore);
      UpdateShowingRuneInfo(gameData.UnlockedRunes);
    }

    private void UpdateShowingRuneInfo(List<RuneData> unlockedRunes)
    {
      maxLevelGO.SetActive(false);
      levelGO.SetActive(false);
      upgradeButton.SetActive(false);
      priceGO.SetActive(false);

      var rune = unlockedRunes.FirstOrDefault(r => r.RuneType == currentShowingRune.RuneType);
      if (rune == default) return;

      if (rune.RuneLevel < 4)
      {
        levelGO.SetActive(true);
        levelGO.GetComponent<Text>().text = string.Format("Level.{0}", rune.RuneLevel + 1);

        int @class = RuneUtils.GetRuneClass(rune.RuneType);
        priceGO.SetActive(true);
        priceGO.GetComponent<Text>().text =
          RuneConfigurations.PriceToUnlockRuneLevels[@class].Price[rune.RuneLevel].ToString();

        upgradeButton.SetActive(true);
      }
      else
      {
        maxLevelGO.SetActive(true);
        maxLevelGO.GetComponent<Text>().text = $"Level.{rune.RuneLevel + 1}";
      }
    }

    public void UpgradeCurrentShowingRune()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      var unlockedRunes = storeManager.GetState<GameDataState>(StoreNames.GameDataStore).UnlockedRunes;
      var rune = unlockedRunes.FirstOrDefault(r => r.RuneType == currentShowingRune.RuneType);

      if (rune == default) return;

      var @class = RuneUtils.GetRuneClass(rune.RuneType);

      var price = RuneConfigurations.PriceToUnlockRuneLevels[@class].Price[rune.RuneLevel];
      var gameData = storeManager.GetState<GameDataState>(StoreNames.GameDataStore);
      var balance = gameData.PlayerBalance;

      if (balance >= price)
      {
        storeManager.Commit(StoreNames.GameDataStore, StoreActions.GameDataStore_DECREASE_PLAYER_HONOR,
          new GameDataActionData()
          {
            PlayerHonor = price
          });

        storeManager.Commit(StoreNames.GameDataStore, StoreActions.GameDataStore_UPGRADE_RUNE, new GameDataActionData()
        {
          RuneType = currentShowingRune.RuneType,
        });

        SaveSystem.SaveGame();
      }
      else
      {
        ShowInfoBox(I18nUtils.GetText("UI/Info/Upgrade/InsufficientBalance"));
      }
    }

    public void Confirm()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      CloseWindow();
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}