using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using System.Linq;
using Code.Scripts.Behaviour.UI.Main;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI
{
  public class SelectableGem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
    IStoreChangedHandler
  {
    public int PlayerNumber { get; set; }

    public bool Selected { get; set; }
    public int SelectedByPlayerNumber { get; private set; }

    private StoreManager storeManager;
    private ResourceManager resourceManager;

    private GameObject selectedByPlayer0;
    private GameObject selectedByPlayer1;
    private RectTransform rectTransform;
    private PickGemUI pickGemUI;
    private Image gemImage;
    private GemProperties gemProperties;
    private GameObject highlight;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      resourceManager = ResourceManager.Instance;

      selectedByPlayer0 = transform.Find("SelectedByPlayer0").gameObject;
      selectedByPlayer1 = transform.Find("SelectedByPlayer1").gameObject;
      rectTransform = GetComponent<RectTransform>();
      gemImage = transform.Find("Image").GetComponent<Image>();
      highlight = transform.Find("Highlight").gameObject;
    }

    private void Start()
    {
      storeManager.Subscribe(StoreNames.LevelConfigurationStore, this);

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      UpdateSelection(levelConfigs);
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          UpdateSelection((LevelConfigurationState)state);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void UpdateSelection(LevelConfigurationState levelConfigs)
    {
      selectedByPlayer0.SetActive(false);
      selectedByPlayer1.SetActive(false);
      for (int playerNumber = 0; playerNumber < levelConfigs.EquippedGems.Length; playerNumber++)
      {
        if (levelConfigs.EquippedGems[playerNumber].Gems.Where(g => g.UUID == gemProperties.UUID).Any())
        {
          Selected = true;
          SelectedByPlayerNumber = playerNumber;

          switch (playerNumber)
          {
            case 0: selectedByPlayer0.SetActive(true); break;
            case 1: selectedByPlayer1.SetActive(true); break;
          }

          return;
        }
      }

      Selected = false;
    }

    public SelectableGem SetGemProperties(GemProperties gemProperties)
    {
      this.gemProperties = gemProperties;

      Texture2D tex = Resources.Load<Texture2D>(GemUtils.GetGemSpritePath(gemProperties));
      gemImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2);

      return this;
    }

    public SelectableGem SetPickGemUI(PickGemUI pickGemUI)
    {
      this.pickGemUI = pickGemUI;
      return this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      rectTransform.SetAsFirstSibling();
      rectTransform.localScale = Vector2.one;
      highlight.SetActive(false);
      pickGemUI.HideGemProperties();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-hover"), Vector2.zero);
      rectTransform.SetAsLastSibling();
      rectTransform.localScale = Vector2.one * 1.25f;
      highlight.SetActive(true);
      pickGemUI.ShowGemProperties(gemProperties);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      AudioSource.PlayClipAtPoint(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      if (!Selected)
      {
        pickGemUI.TryAddGem(gemProperties);
      }
      else
      {
        storeManager.Commit(StoreNames.LevelConfigurationStore,
          StoreActions.LevelConfigurationStore_REMOVE_EQUIPPED_GEM, new LevelConfigurationData()
          {
            PlayerNumber = PlayerNumber,
            Gem = gemProperties,
          });
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}