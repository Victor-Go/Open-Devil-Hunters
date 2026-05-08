using System;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Upgrade;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Upgrade
{
  public class UpgradeCard : UIWindow
  {
    public int PlayerNumber { get; set; }

    private Image image;
    private UpgradeReducerContainer upgrade;
    private Text nameText;
    private Text descriptionText;
    private Action<int> chooseAction;
    private UpgradeUI upgradeUI;

    private void Awake()
    {
      image = transform.Find("ImageContainer/Image").GetComponent<Image>();
      nameText = transform.Find("Name").GetComponent<Text>();
      descriptionText = transform.Find("Description").GetComponent<Text>();
    }

    public UpgradeCard SetUpgrade(UpgradeReducerContainer upgrade)
    {
      this.upgrade = upgrade;

      var tex = (Texture2D)ResourceManager.Instance.GetResource(upgrade.ImageName);
      if (tex == null)
      {
        tex = (Texture2D)ResourceManager.Instance.GetResource("UpgradeImage/default");
      }

      image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2);
      nameText.text =
        I18nUtils.GetText(upgrade.UpgradeNameIndicator ?? upgrade.UpgradeNameText);
      descriptionText.text =
        I18nUtils.GetText(upgrade.UpgradeDescriptionIndicator ??
                          upgrade.UpgradeDescriptionText);
      chooseAction = upgrade.UpgradeReducer;

      return this;
    }

    public void SetUpgradeUI(UpgradeUI upgradeUI)
    {
      this.upgradeUI = upgradeUI;
    }

    public void Choose()
    {
      if (DebugConfigurations.DebugEnabled)
      {
        Debug.LogFormat("Chosen upgrade {0}.", upgrade.UpgradeId);
      }

      AudioWrapper.PlayClip(ResourceManager.Instance.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      chooseAction(PlayerNumber);

      StoreManager.Instance.Commit(StoreNames.UpgradeStore, StoreActions.UpgradeStore_ADD_UPGRADE,
        new UpgradedSkillData
        {
          PlayerNumber = PlayerNumber,
          UpgradeId = upgrade.UpgradeId,
          Category = upgrade.Category,
          UpgradeNameIndicator = upgrade.UpgradeNameIndicator,
          UpgradeDescriptionIndicator = upgrade.UpgradeDescriptionIndicator,

          ImageName = upgrade.ImageName,
        });

      upgradeUI.CloseWindow();
    }
  }
}