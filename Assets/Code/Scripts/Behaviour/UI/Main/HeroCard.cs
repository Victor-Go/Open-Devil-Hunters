using Code.Scripts.Behaviour.UI;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Steam.Achievements;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Main
{
  public enum HeroCardStatus
  {
    STABLE,
    EXPAND,
    SHRINK,
  }

  public class HeroCard : UIWindow, IStoreChangedHandler, IEventHandler, IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler
  {
    public HeroCardNavigation Navigation { get; set; }
    public bool AvailableInTrailVersion { get; private set; }
    public bool Unlocked { get; set; }
    public bool Selected { get; private set; }
    public int SelectedByPlayerNumber { get; private set; } // [0,1]
    public bool Hovered { get; private set; }
    public int HoveredByPlayerNumber { get; private set; }

    private EventManager eventManager;
    private StoreManager storeManager;
    private ResourceManager resourceManager;

    private Image heroImage;
    private GameObject unlock;
    private GameObject trailVersion;

    private RectTransform rectTransform;
    private PlayerConfiguration playerConfiguration;

    private GameObject selectionSign;
    private GameObject hoverSign;

    private HeroCardStatus status = HeroCardStatus.STABLE;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      eventManager = EventManager.Instance;
      resourceManager = ResourceManager.Instance;

      heroImage = transform.Find("Avatar").GetComponent<Image>();
      unlock = transform.Find("Unlock").gameObject;
      trailVersion = transform.Find("TrailVersion").gameObject;

      rectTransform = GetComponent<RectTransform>();

      SetUnlockAndAvailableUi();
    }

    private void Start()
    {
      storeManager.Subscribe(StoreNames.GameDataStore, this);

      eventManager
        .AddEventHandler(Events.PLAYER_SELECT_HERO, this)
        .AddEventHandler(Events.PLAYER_HOVER_HERO, this);
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.GameDataStore:
          HandleGameDataChanged((GameDataState)state);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    public void OnEvent(Events @event, IEventData data)
    {
      switch (@event)
      {
        case Events.PLAYER_SELECT_HERO:
          var playerSelectData = (PlayerSelectOrHoverHeroEventData)data;
          if (Selected && playerSelectData.PlayerNumber == SelectedByPlayerNumber &&
              playerSelectData.PlayerName != playerConfiguration.PlayerName)
          {
            UnselectCurrentHero();
          }

          break;
        case Events.PLAYER_HOVER_HERO:
          var playerHoverData = (PlayerSelectOrHoverHeroEventData)data;
          if (Hovered && playerHoverData.PlayerNumber == HoveredByPlayerNumber &&
              playerHoverData.PlayerName != playerConfiguration.PlayerName)
          {
            UnhoverCurrentHero(playerHoverData.PlayerNumber);
          }

          break;
        default:
          throw new InvalidEventHandlingException(@event);
      }
    }

    public void SetHeroConfiguration(PlayerConfiguration config)
    {
      playerConfiguration = ObjectCopier.Clone(config);
      var texture = ResourceManager.Instance.GetResource(config.AvatarImageIndicator) as Texture2D;
      heroImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

      var gameData = storeManager.GetState<GameDataState>(StoreNames.GameDataStore);
      HandleGameDataChanged(gameData);

      AvailableInTrailVersion =
        GeneralConfigurations.HeroesAvailableInDemoVersion.Contains(playerConfiguration.PlayerName);

      SetUnlockAndAvailableUi();
    }

    private void HandleGameDataChanged(GameDataState state)
    {
      Unlocked = state.UnlockedPlayers.Contains(playerConfiguration.PlayerName);
      SetUnlockAndAvailableUi();
    }

    private void SetUnlockAndAvailableUi()
    {
      trailVersion.SetActive(!AvailableInTrailVersion && GeneralConfigurations.Version == Version.DEMO);
      unlock.SetActive(!AvailableInTrailVersion && !Unlocked);
    }

    public void SetNavigation(HeroCardNavigation navigation)
    {
      Navigation = navigation;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      InteractWithCurrentHero();
    }

    public void TryUnlock()
    {
      if (Unlocked)
      {
        return;
      }

      UnhoverCurrentHero(HoveredByPlayerNumber);

      var gameData = storeManager.GetState<GameDataState>(StoreNames.GameDataStore);
      if (gameData.PlayerBalance >= playerConfiguration.HonorRequirement)
      {
        var confirmBox = OpenSubWindow(WindowNames.ConfirmBoxUI);
        confirmBox
          .GetComponent<ConfirmBoxUI>()
          .SetTitle(string.Format(I18nUtils.GetText("UI/Prompt/UnlockHero"),
            I18nUtils.GetText(playerConfiguration.PlayerNameIndicator), playerConfiguration.HonorRequirement))
          .SetConfirmAction(Unlock);
      }
      else
      {
        var infoBox = OpenSubWindow(WindowNames.InfoBoxUI);
        infoBox
          .GetComponent<InfoBoxUI>()
          .SetTitle(string.Format(I18nUtils.GetText("UI/Info/Unlock/InsufficientBalance"),
            playerConfiguration.HonorRequirement));
      }
    }

    private void Unlock()
    {
      storeManager.Commit(StoreNames.GameDataStore, StoreActions.GameDataStore_DECREASE_PLAYER_HONOR,
        new GameDataActionData()
        {
          PlayerHonor = playerConfiguration.HonorRequirement,
        });

      storeManager.Commit(StoreNames.GameDataStore, StoreActions.GameDataStore_UNLOCK_HERO, new GameDataActionData()
      {
        PlayerName = playerConfiguration.PlayerName,
      });

      AchievementsManager.UnlockAchievement(Achievements.NewHero);

      SaveSystem.SaveGame();
    }

    public void HandleHover(int playerNumber)
    {
      if (Hovered && HoveredByPlayerNumber != playerNumber) return;

      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-hover"), Vector2.zero);

      Hovered = true;
      HoveredByPlayerNumber = playerNumber;

      rectTransform.SetAsLastSibling();
      status = HeroCardStatus.EXPAND;

      if (hoverSign) Destroy(hoverSign);
      switch (playerNumber)
      {
        case 0: hoverSign = Instantiate(Resources.Load<GameObject>("UI/Main/Player0Hover")); break;
        case 1: hoverSign = Instantiate(Resources.Load<GameObject>("UI/Main/Player1Hover")); break;
      }

      hoverSign.transform.SetParent(transform, false);

      storeManager.Commit(StoreNames.LevelConfigurationStore,
        StoreActions.LevelConfigurationStore_SET_FIGHT_PREPARATION_SHOWING_PLAYER_CONFIGURATIONS,
        new LevelConfigurationData()
        {
          PlayerNumber = playerNumber,
          PlayerConfiguration = playerConfiguration,
        });

      eventManager.PublishEvent(Events.PLAYER_HOVER_HERO, new PlayerSelectOrHoverHeroEventData()
      {
        PlayerNumber = playerNumber,
        PlayerName = playerConfiguration.PlayerName,
      });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      int playerNumber = 0;

      HandleHover(playerNumber);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      UnhoverCurrentHero(0);
    }

    public void UnhoverCurrentHero(int playerNumber)
    {
      if (!Hovered || playerNumber != HoveredByPlayerNumber) return;

      Hovered = false;
      if (hoverSign)
      {
        Destroy(hoverSign);
      }

      hoverSign = null;

      rectTransform.SetAsFirstSibling();
      status = HeroCardStatus.SHRINK;

      var playerConfig = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore)
        .PlayerConfigurations[playerNumber];
      storeManager.Commit(StoreNames.LevelConfigurationStore,
        StoreActions.LevelConfigurationStore_SET_FIGHT_PREPARATION_SHOWING_PLAYER_CONFIGURATIONS,
        new LevelConfigurationData()
        {
          PlayerNumber = HoveredByPlayerNumber,
          PlayerConfiguration = playerConfig
        });
    }

    public void UnselectCurrentHero()
    {
      Selected = false;
      if (selectionSign)
      {
        Destroy(selectionSign);
      }

      selectionSign = null;
    }

    public void InteractWithCurrentHero(int playerNumber = 0)
    {
      if (!AvailableInTrailVersion && GeneralConfigurations.Version == Version.DEMO)
      {
        return;
      }

      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      if (!Unlocked)
      {
        TryUnlock();
        return;
      }

      if (Selected || (Hovered && HoveredByPlayerNumber != playerNumber)) return;

      eventManager.PublishEvent(Events.PLAYER_SELECT_HERO, new PlayerSelectOrHoverHeroEventData()
      {
        PlayerNumber = playerNumber,
        PlayerName = playerConfiguration.PlayerName
      });

      SelectedByPlayerNumber = playerNumber;
      Selected = true;

      rectTransform.SetAsLastSibling();

      if (selectionSign) Destroy(selectionSign);
      switch (playerNumber)
      {
        case 0: selectionSign = Instantiate(Resources.Load<GameObject>("UI/Main/Player0Selection")); break;
        case 1: selectionSign = Instantiate(Resources.Load<GameObject>("UI/Main/Player1Selection")); break;
      }

      selectionSign.transform.SetParent(transform, false);

      storeManager.Commit(StoreNames.LevelConfigurationStore,
        StoreActions.LevelConfigurationStore_SET_PLAYER_CONFIGURATIONS, new LevelConfigurationData()
        {
          PlayerNumber = playerNumber,
          PlayerConfiguration = playerConfiguration,
        });
      storeManager.Commit(StoreNames.LevelConfigurationStore,
        StoreActions.LevelConfigurationStore_SET_FIGHT_PREPARATION_SHOWING_PLAYER_CONFIGURATIONS,
        new LevelConfigurationData()
        {
          PlayerNumber = playerNumber,
          PlayerConfiguration = playerConfiguration
        });
    }

    private void Update()
    {
      var dt = Time.deltaTime;
      switch (status)
      {
        case HeroCardStatus.EXPAND:
          rectTransform.localScale = Vector2.Lerp(rectTransform.localScale, Vector2.one * 1.25f, dt * 15);
          if (((Vector2)rectTransform.localScale - Vector2.one * 1.25f).magnitude < 0.05f)
          {
            status = HeroCardStatus.STABLE;
          }

          break;
        case HeroCardStatus.SHRINK:
          rectTransform.localScale = Vector2.Lerp(rectTransform.localScale, Vector2.one, dt * 15);
          if (((Vector2)rectTransform.localScale - Vector2.one).magnitude < 0.05f)
          {
            status = HeroCardStatus.STABLE;
          }

          break;
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
      eventManager.RemoveEventHandler(this);
    }
  }
}