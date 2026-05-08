using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Behaviour.UI;
using Code.Scripts.Src.Store.Player;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
    public class ReloadDisplayer : MonoBehaviour, IStoreChangedHandler
    {
        public int PlayerNumber { get; set; }

        private StoreManager storeManager;
        private CircularProgressBar progressBar;

        private void Awake()
        {
            storeManager = StoreManager.Instance;
            progressBar = GetComponent<CircularProgressBar>();
        }

        private void Start()
        {
            storeManager
                .Subscribe(StoreNames.LevelConfigurationStore, this)
                .Subscribe(StoreNames.AttackControlStore, this);

            var attackState = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore);
            HandleReloadChange(attackState);
        }

        private void HandleReloadChange(AttackControlState state)
        {
            gameObject.SetActive(state.AttackControlDatas[PlayerNumber].Reloading);
            progressBar.SetProgress(1 - (state.AttackControlDatas[PlayerNumber].ReloadCountdown / state.AttackControlDatas[PlayerNumber].AttackControlConfigurations.ReloadTime));
        }

        public void OnStoreChanged(StoreNames storeName, IState state)
        {
            switch (storeName)
            {
                case StoreNames.LevelConfigurationStore:
                    var levelConfigs = (LevelConfigurationState)state;
                    if (!levelConfigs.PlayerAlive[PlayerNumber])
                    {
                        Destroy(gameObject);
                    }
                    break;
                case StoreNames.AttackControlStore:
                    var attackState = (AttackControlState)state;
                    HandleReloadChange(attackState);
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
