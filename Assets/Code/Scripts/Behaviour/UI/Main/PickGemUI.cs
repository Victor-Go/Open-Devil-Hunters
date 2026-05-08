using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.UI;
using Code.Scripts.Behaviour.UI.Main;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class PickGemUI : UIAnimation
  {
    private int playerNumber;

    private StoreManager storeManager;

    private GameObject player0GO;
    private GameObject player1GO;
    private Image gem0Image;
    private Image gem1Image;
    private Image gem2Image;
    private RectTransform scrollContainer;
    private ScrollRect scrollRect;
    private Text gemDescription;
    private GameObject confirmButton;

    private void Awake()
    {
      storeManager = StoreManager.Instance;

      targetTransform = transform.Find("PickGem").GetComponent<RectTransform>();
      scrollRect = targetTransform.GetComponentInChildren<ScrollRect>();
      player0GO = targetTransform.Find("Player0Image").gameObject;
      player1GO = targetTransform.Find("Player1Image").gameObject;
      gem0Image = targetTransform.Find("HeaderBar/Gem0/Image").GetComponent<Image>();
      gem1Image = targetTransform.Find("HeaderBar/Gem1/Image").GetComponent<Image>();
      gem2Image = targetTransform.Find("HeaderBar/Gem2/Image").GetComponent<Image>();
      gemDescription = targetTransform.Find("GemDescription").GetComponent<Text>();
      scrollContainer = targetTransform.Find("ScrollView/Viewport/Content").GetComponent<RectTransform>();
      confirmButton = targetTransform.Find("Confirm").gameObject;
    }

    protected override void Start()
    {
      base.Start();

      StartCoroutine(UIUtils.SetSelectedGameObject(confirmButton));
    }

    private IEnumerator StopScrollRolling()
    {
      yield return new WaitForEndOfFrame();
      scrollRect.StopMovement();
      scrollContainer.anchoredPosition = Vector2.zero;
    }

    public void SetPlayerNumber(int playerNumber)
    {
      this.playerNumber = playerNumber;

      // Set PlayerNumber for EquippedGemDisplayer.
      var gemDisplayers = GetComponentsInChildren<EquippedGemDisplayer>();
      foreach (var displayer in gemDisplayers)
      {
        displayer.PlayerNumber = playerNumber;
      }

      // Show Hero Avatar.
      var levelConfig = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      var tex = Resources.Load<Texture2D>(levelConfig.PlayerConfigurations[playerNumber].AvatarImageIndicator);
      switch (playerNumber)
      {
        case 0:
          player1GO.SetActive(false);
          player0GO.GetComponent<Image>().sprite =
            Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2);
          break;
        case 1:
          player0GO.SetActive(false);
          player1GO.GetComponent<Image>().sprite =
            Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2);
          break;
      }


      // Show Gems.
      var gameData = storeManager.GetState<GameDataState>(StoreNames.GameDataStore);
      var gems = gameData.OwnedeGems.OrderByDescending(g => g.Rarity).ToList();

      var gemTransforms = new List<Transform>();
      foreach (var gem in gems)
      {
        {
          {
            var _gem = Instantiate(Resources.Load<GameObject>("UI/Main/SelectableGem"), scrollContainer, false);
            gemTransforms.Add(_gem.transform);
            _gem.GetComponent<SelectableGem>()
              .SetGemProperties(gem)
              .SetPickGemUI(this);
          }
        }
      }

      var verticalCount = (int)Mathf.Ceil(gemTransforms.Count / 10f);
      var size = scrollContainer.sizeDelta;
      size.y = 70 * verticalCount + 20;
      scrollContainer.sizeDelta = size;
      StartCoroutine(StopScrollRolling());
      for (var vertical = 0; vertical < verticalCount; vertical++)
      {
        var horizontalCount = Mathf.Min(gemTransforms.Count - vertical * 10, 10);
        for (var horizontal = 0; horizontal < horizontalCount; horizontal++)
        {
          var gem = gemTransforms[vertical * 10 + horizontal];
          gem.localPosition = new Vector2(45 + horizontal * 70, -45 - 70 * vertical);
        }
      }
    }

    public void ShowGemProperties(GemProperties gemProperties)
    {
      var rarity = gemProperties.Rarity switch
      {
        GemRarity.RARE => $"<color=#fcf03f>{I18nUtils.GetText("Gem/Rarity/R")}</color>",
        GemRarity.SUPER_RARE => $"<color=#3bff6f>{I18nUtils.GetText("Gem/Rarity/SR")}</color>",
        GemRarity.SUPER_SUPER_RARE => $"<color=#009dff>{I18nUtils.GetText("Gem/Rarity/SSR")}</color>",
        GemRarity.EXTREME_RARE => $"<color=#ff4fb5>{I18nUtils.GetText("Gem/Rarity/XR")}</color>",
        _ => ""
      };

      var description = $"<b>{rarity}</b>\n";

      description += string.Join("\n", GemUtils.GetGemDescription(gemProperties));
      gemDescription.text = description;
    }

    public void HideGemProperties()
    {
      gemDescription.text = string.Empty;
    }

    public void TryAddGem(GemProperties gemProperties)
    {
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      if (levelConfigs.EquippedGems[playerNumber].Gems.Count < 3)
      {
        storeManager.Commit(StoreNames.LevelConfigurationStore, StoreActions.LevelConfigurationStore_ADD_EQUIPPED_GEM,
          new LevelConfigurationData
          {
            PlayerNumber = playerNumber,
            Gem = gemProperties,
          });
      }
    }

    public void Confirm()
    {
      AudioWrapper.PlayClip(ResourceManager.Instance.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      CloseWindow();
    }
  }
}