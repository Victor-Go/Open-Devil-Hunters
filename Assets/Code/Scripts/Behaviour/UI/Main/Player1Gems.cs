using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
    public class Player1Gems : MonoBehaviour, IStoreChangedHandler
    {
        private StoreManager storeManager;

        private void Awake()
        {
            storeManager = StoreManager.Instance;
        }

        private void Start()
        {
            storeManager.Subscribe(StoreNames.LevelConfigurationStore, this);

            var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
            SetLevelConfigs(levelConfigs);
        }

        public void OnStoreChanged(StoreNames storeName, IState state)
        {
            switch (storeName)
            {
                case StoreNames.LevelConfigurationStore:
                    var levelConfigs = (LevelConfigurationState)state;
                    SetLevelConfigs(levelConfigs);
                    break;
                default:
                    throw new InvalidStoreEventException(storeName);
            }
        }

        private void SetLevelConfigs(LevelConfigurationState levelConfigs)
        {
            switch (levelConfigs.InitialNumberOfPlayers)
            {
                case 1: gameObject.SetActive(false); break;
                case 2: gameObject.SetActive(true); break;
            }
        }

        private void OnDestroy()
        {
            storeManager.Unsubscribe(this);
        }
    }
}