using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.I18n;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Main
{
    public class PropertiesDisplayer : MonoBehaviour, IStoreChangedHandler
    {
        public int PlayerNumber;

        public Text HeroName;
        public Text HeroDescription;

        private StoreManager storeManager;

        private void Awake()
        {
            storeManager = StoreManager.Instance;
        }

        private void Start()
        {
            storeManager.Subscribe(StoreNames.LevelConfigurationStore, this);
        }

        public void OnStoreChanged(StoreNames storeName, IState state)
        {
            switch (storeName)
            {
                case StoreNames.LevelConfigurationStore:
                    var levelState = (LevelConfigurationState)state;

                    HeroName.text = I18nUtils.GetText(levelState.PlayerConfigurations[PlayerNumber].PlayerNameIndicator);
                    HeroDescription.text = I18nUtils.GetText(levelState.PlayerConfigurations[PlayerNumber].DescriptionIndicator);
                    break;
                default:
                    throw new InvalidStoreEventException(storeName);
            }
        }

        private void OnDestroy()
        {
            storeManager.Unsubscribe(this);
        }
    }
}
