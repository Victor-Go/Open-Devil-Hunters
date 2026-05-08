using Code.Scripts.Src;
using Code.Scripts.Src.Store.Player;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Player
{
    public class PickUpControl : MonoBehaviour, IStoreChangedHandler
    {
        public int PlayerNumber { get; set; }
        private new CircleCollider2D collider;
        private StoreManager storeManager;

        private void Awake()
        {
            storeManager = StoreManager.Instance;
            storeManager.Subscribe(StoreNames.PlayerStore, this);

            collider = GetComponent<CircleCollider2D>();
        }

        private void Start()
        {
            PlayerState playerState = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
            collider.radius = playerState.PlayerDatas[PlayerNumber].PickUpRadius;
        }

        public void OnStoreChanged(StoreNames storeName, IState state)
        {
            switch (storeName)
            {
                case StoreNames.PlayerStore:
                    PlayerState playerState = (PlayerState)state;
                    collider.radius = playerState.PlayerDatas[PlayerNumber].PickUpRadius;
                    break;
                default:
                    throw new InvalidStoreEventException(storeName);
            }
        }

        public void OnDestroy()
        {
            storeManager.Unsubscribe(this);
        }
    }
}
