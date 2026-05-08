using Code.Scripts.Src;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Main
{
    public class HonorDisplayer : MonoBehaviour, IStoreChangedHandler
    {
        private StoreManager storeManager;
        private Text honorText;

        private void Awake()
        {
            storeManager = StoreManager.Instance;
            honorText = transform.Find("Honor").GetComponent<Text>();
        }

        private void Start()
        {
            storeManager.Subscribe(StoreNames.GameDataStore, this);

            honorText.text = storeManager.GetState<GameDataState>(StoreNames.GameDataStore).PlayerBalance.ToString();
        }
        public void OnStoreChanged(StoreNames storeName, IState state)
        {
            switch (storeName)
            {
                case StoreNames.GameDataStore:
                    honorText.text = ((GameDataState)state).PlayerBalance.ToString();
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