using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Main
{
    public class EquippedGemDisplayer : MonoBehaviour, IPointerClickHandler, IStoreChangedHandler
    {
        public bool AllowRemove;
        public int PlayerNumber;
        public int ItemNumber;

        private StoreManager storeManager;

        private Image gemImage;

        private GemProperties gemProperties;
        private Sprite emptySprite;

        private void Awake()
        {
            storeManager = StoreManager.Instance;
            gemImage = transform.Find("Image").GetComponent<Image>();
            emptySprite = gemImage.sprite;
        }

        private void Start()
        {
            storeManager.Subscribe(StoreNames.LevelConfigurationStore, this);

            var levelConfigs = StoreManager.Instance.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
            SetGemImage(levelConfigs);
        }

        public void OnStoreChanged(StoreNames storeName, IState state)
        {
            switch (storeName)
            {
                case StoreNames.LevelConfigurationStore:
                    SetGemImage((LevelConfigurationState)state);
                    break;
                default:
                    throw new InvalidStoreEventException(storeName);
            }
        }

        private void SetGemImage(LevelConfigurationState levelConfigs)
        {
            var gems = levelConfigs.EquippedGems[PlayerNumber].Gems;
            if (ItemNumber >= gems.Count)
            {
                gemImage.sprite = emptySprite;
            }
            else
            {
                var currentGem = gems[ItemNumber];
                gemProperties = currentGem;
                var tex = Resources.Load<Texture2D>(GemUtils.GetGemSpritePath(currentGem));
                gemImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (AllowRemove)
            {
                storeManager.Commit(StoreNames.LevelConfigurationStore, StoreActions.LevelConfigurationStore_REMOVE_EQUIPPED_GEM, new LevelConfigurationData()
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
