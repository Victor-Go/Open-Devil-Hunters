using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Level;
using System.Linq;
using Code.Scripts.Behaviour.UI.Main;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI
{
  public class UIRuneInfo
  {
    public bool Unlocked { get; set; }
    public bool Activated { get; set; }
    public RuneTypes RuneType { get; set; }
    public Sprite RuneImage { get; set; }
    public int Level { get; set; }
    public string RuneTitleName { get; set; }
    public string RuneDescriptionName { get; set; }
  }

  public class SelectableRune : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IStoreChangedHandler
  {
    public RuneTypes RuneType;
    public int PlayerNumber { get; set; }
    public UIRuneInfo RuneInfo { get; set; }

    private StoreManager storeManager;
    private ResourceManager resourceManager;

    private RectTransform rectTransform;
    private GameObject activatedByPlayer0Frame;
    private GameObject activatedByPlayer1Frame;
    private GameObject unlockMask;
    private GameObject notAvailableMask;
    private Text levelText;
    private PickRuneUI pickRuneUI;

    private bool available;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      resourceManager = ResourceManager.Instance;

      rectTransform = GetComponent<RectTransform>();
      activatedByPlayer0Frame = transform.Find("ActivatedByPlayer0Frame").gameObject;
      activatedByPlayer1Frame = transform.Find("ActivatedByPlayer1Frame").gameObject;
      notAvailableMask = transform.Find("NotAvailableInDemo").gameObject;
      levelText = transform.Find("Level").GetComponent<Text>();
      unlockMask = transform.Find("UnlockMask").gameObject;

      RuneInfo = new();
      RuneInfo.RuneType = RuneType;
      RuneInfo.RuneImage = transform.Find("Image").GetComponent<Image>().sprite;
      RuneInfo.RuneTitleName = RunePresets.Presets[RuneType].TitleName;
      RuneInfo.RuneDescriptionName = RunePresets.Presets[RuneType].DescriptionName;
    }

    private void Start()
    {
      storeManager
        .Subscribe(StoreNames.LevelConfigurationStore, this)
        .Subscribe(StoreNames.GameDataStore, this);

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      UpdateLevelInfo(levelConfigs);

      var gameData = storeManager.GetState<GameDataState>(StoreNames.GameDataStore);
      UpdateGameData(gameData);

      available = GeneralConfigurations.RunesAvailableInDemoVersion.Contains(RuneType) ||
                  GeneralConfigurations.Version == Version.OFFICIAL;
      notAvailableMask.SetActive(!available);
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          UpdateLevelInfo((LevelConfigurationState)state);
          break;
        case StoreNames.GameDataStore:
          UpdateGameData((GameDataState)state);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void UpdateLevelInfo(LevelConfigurationState levelConfigs)
    {
      var runes = levelConfigs.RunesConfigurations[PlayerNumber].ActivatedRunes;
      if (runes.Any(r => r.RuneType == RuneInfo.RuneType))
      {
        new[]
        {
          activatedByPlayer0Frame,
          activatedByPlayer1Frame
        }[PlayerNumber].SetActive(true);
      }
      else
      {
        new[]
        {
          activatedByPlayer0Frame,
          activatedByPlayer1Frame
        }[PlayerNumber].SetActive(false);
      }
    }

    private void UpdateGameData(GameDataState gameData)
    {
      var currentRune = gameData.UnlockedRunes.FirstOrDefault(r => r.RuneType == RuneInfo.RuneType);
      if (currentRune != default)
      {
        unlockMask.SetActive(false);
        RuneInfo.Level = currentRune.RuneLevel;
        RuneInfo.Unlocked = true;
        levelText.text = (RuneInfo.Level + 1).ToString();
      }
      else
      {
        unlockMask.SetActive(true);
        RuneInfo.Unlocked = false;
        levelText.text = string.Empty;
      }
    }

    public void SetPickRuneUI(PickRuneUI pickRuneUI)
    {
      this.pickRuneUI = pickRuneUI;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      if (!available) return;

      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      if (RuneInfo.Unlocked)
      {
        pickRuneUI.TryPickRune(RuneInfo.RuneType);
      }
      else
      {
        pickRuneUI.TryUnlockRune(RuneInfo.RuneType);
      }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      pickRuneUI.SetShowingRune(RuneInfo);
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}