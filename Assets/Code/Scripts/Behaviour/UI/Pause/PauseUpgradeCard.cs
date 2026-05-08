using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Pause
{
    public class PauseUpgradeCard : MonoBehaviour
    {
        private ResourceManager resourceManager;

        private Image image;
        private Text upgradeQuantityText;

        private UpgradeContainer upgrade;
        private int upgradeQuantity;

        private void Awake()
        {
            resourceManager = ResourceManager.Instance;
            image = transform.Find("Image").GetComponent<Image>();
            upgradeQuantityText = transform.Find("Quantity").GetComponent<Text>();
        }

        public void SetUpgrade(UpgradeContainer upgrade, int upgradeQuantity)
        {
            this.upgrade = upgrade;
            this.upgradeQuantity = upgradeQuantity;

            var tex = (Texture2D)resourceManager.GetResource(upgrade.ImageName);
            if (tex == null)
            {
                tex = (Texture2D)resourceManager.GetResource("UpgradeImage/default");
            }
            image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2);

            upgradeQuantityText.text = upgradeQuantity == 1 ? "" : upgradeQuantity.ToString();
        }
    }
}
