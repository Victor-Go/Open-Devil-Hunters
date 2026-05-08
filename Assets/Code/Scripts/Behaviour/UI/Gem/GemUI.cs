using System.Linq;
using Code.Scripts.Behaviour.UI;
using Code.Scripts.Behaviour.UI.Gem;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Gem
{
  public class GemUI : UIAnimation
  {
    private StoreManager storeManager;
    private ResourceManager resourceManager;

    private RoseRibbon[] roseRibbons;

    private GameObject rarityContainer;
    private Text descriptionText;
    private Image gemImage;
    private Button confirmButton;

    private GemProperties gemProperties;
    
    private GameStates initGameState;
    
    private void Awake()
    {
      storeManager = StoreManager.Instance;
      resourceManager = ResourceManager.Instance;

      rarityContainer = transform.Find("UIContainer/GemContainer/RarityContainer").gameObject;
      descriptionText = transform.Find("UIContainer/Description/PropertyContainer").GetComponentInChildren<Text>();
      gemImage = transform.Find("UIContainer/GemContainer/Gem").GetComponent<Image>();
      roseRibbons = transform.Find("UIContainer/GemContainer/Ribbon").GetComponentsInChildren<RoseRibbon>(true);
      confirmButton = transform.Find("UIContainer/Description/Confirm").GetComponent<Button>();
    }

    private void Start()
    {
      resourceManager.GetResource("/UiCanvas").GetComponent<RectTransform>();
      initGameState = storeManager.GetState<GameState>(StoreNames.GameStateStore).CurrentGameState;
      
      storeManager.Commit(StoreNames.GameStateStore, StoreActions.GameStateStore_SET_GAME_STATE, new GameStateData()
      {
        GameState = GameStates.PICKED_GEM,
      });

      storeManager.Commit(StoreNames.BackgroundCanvas, StoreActions.BackgroundCanvas_SET_ACTIVE,
        new BackgroundCanvasActionData()
        {
          Active = true
        });
    }

    protected override void OnWindowOpened()
    {
      base.OnWindowOpened();
      
      EventSystem.current.SetSelectedGameObject(confirmButton.gameObject);
    }

    public void SetGem(GemProperties gem)
    {
      gemProperties = gem;

      SetGemImage();
      SetGemRarity();
      SetGemDescription();

      storeManager.Commit(StoreNames.LevelCollectionStore, StoreActions.LevelCollectionStore_ADD_GEM,
        new LevelCollectionData()
        {
          GemProperties = gem
        });

      storeManager.Commit(StoreNames.GameDataStore, StoreActions.GameDataStore_ADD_GEM, new GameDataActionData()
      {
        Gem = gem
      });

      SaveSystem.SaveGame();
    }

    private void SetRibbonEquationK(int k)
    {
      foreach (var ribbon in roseRibbons)
      {
        ribbon.K = k;
      }
    }

    private void SetGemImage()
    {
      string gemPicResourceName = "Gem/gem-";

      switch (gemProperties.Rarity)
      {
        case GemRarity.RARE:
          gemPicResourceName += "lozenge_";
          SetRibbonEquationK(3);
          break;
        case GemRarity.SUPER_RARE:
          gemPicResourceName += "hexagon_";
          SetRibbonEquationK(5);
          break;
        case GemRarity.SUPER_SUPER_RARE:
          gemPicResourceName += "octagon_";
          SetRibbonEquationK(7);
          break;
        case GemRarity.EXTREME_RARE:
          gemPicResourceName += "dimond_";
          SetRibbonEquationK(9);
          break;
      }

      switch (gemProperties.Color)
      {
        case GemColors.RED:
          gemPicResourceName += "red";
          break;
        case GemColors.BLUE:
          gemPicResourceName += "blue";
          break;
        case GemColors.GREEN:
          gemPicResourceName += "green";
          break;
        case GemColors.YELLOW:
          gemPicResourceName += "yellow";
          break;
        case GemColors.LEMON:
          gemPicResourceName += "lemon";
          break;
        case GemColors.PURPLE:
          gemPicResourceName += "purple";
          break;
      }

      var texture = Resources.Load<Texture2D>(gemPicResourceName);
      gemImage.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f),
        100f);
    }

    private void SetGemRarity()
    {
      System.Collections.Generic.List<GameObject> rarities = new();

      switch (gemProperties.Rarity)
      {
        case GemRarity.RARE:
          rarities.Add(GetRarityContainer("UI/PickedGem/R"));
          break;
        case GemRarity.SUPER_RARE:
          rarities.Add(GetRarityContainer("UI/PickedGem/S"));
          rarities.Add(GetRarityContainer("UI/PickedGem/R"));
          break;
        case GemRarity.SUPER_SUPER_RARE:
          rarities.Add(GetRarityContainer("UI/PickedGem/S"));
          rarities.Add(GetRarityContainer("UI/PickedGem/S"));
          rarities.Add(GetRarityContainer("UI/PickedGem/R"));
          break;
        case GemRarity.EXTREME_RARE:
          rarities.Add(GetRarityContainer("UI/PickedGem/X"));
          rarities.Add(GetRarityContainer("UI/PickedGem/R"));
          break;
      }

      var initialPoint = -rarities.Sum(r => r.GetComponent<RectTransform>().rect.width) / 2;
      float elapsed = 0;
      foreach (var r in rarities)
      {
        var width = r.GetComponent<RectTransform>().rect.width;
        r.GetComponent<RectTransform>().anchoredPosition = new Vector2(initialPoint + elapsed, 0);
        elapsed += width;
      }

      return;

      GameObject GetRarityContainer(string path)
      {
        GameObject go = Instantiate(resourceManager.GetResource("UI/PickedGem/GemRarity"));
        go.transform.SetParent(rarityContainer.transform, false);

        var texture = Resources.Load<Texture2D>(path);
        go.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
          new Vector2(0.5f, 0.5f), 100f);

        return go;
      }
    }

    public void SetGemDescription()
    {
      var descriptions = GemUtils.GetGemDescription(gemProperties);

      descriptionText.text = string.Join("\n", descriptions.ToArray());
    }

    public void Confirm()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      CloseWindow();
    }
    
    protected override void OnCurrentWindowClosed()
    {
      // State change should be put advanced since OnCurrentWindowClosed will show potential next GemUI that commit game state change
      storeManager.Commit(StoreNames.BackgroundCanvas, StoreActions.BackgroundCanvas_SET_ACTIVE,
        new BackgroundCanvasActionData()
        {
          Active = false
        });
      
      storeManager.Commit(StoreNames.GameStateStore, StoreActions.GameStateStore_SET_GAME_STATE, new GameStateData()
      {
        GameState = initGameState,
      });

      base.OnCurrentWindowClosed();
    }
  }
}