using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI
{
    public class HpDisplayer : MonoBehaviour, IStoreChangedHandler
    {
        public int PlayerNumber;
        public bool ForSinglePlayer;

        private int numberOfPlayers;
        private StoreManager storeManager;
        private ProgressBarController progressBarController;
        private float currentHp, maximumHp;
        private Image heart;
        private bool heartBlinking;
        private bool showHeart;
        private float blinkTimeout;
        private float blinkInterval = 0.5f;

        private void Awake()
        {
            storeManager = StoreManager.Instance;
            storeManager.Subscribe(StoreNames.PlayerStore, this);

            progressBarController = GetComponent<ProgressBarController>();

            heart = transform.Find("Heart").GetComponent<Image>();
        }

        private void Start()
        {
            storeManager.Subscribe(StoreNames.LevelConfigurationStore, this);

            numberOfPlayers = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore).NumberOfPlayers;

            PlayerState playerState = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
            SetHpDisplay(playerState.PlayerDatas[PlayerNumber].CurrentHp, playerState.PlayerDatas[PlayerNumber].MaximumHp);
        }

        private void SetHpDisplay(float currentHp, float maximumHp)
        {
            if (this.currentHp == currentHp && this.maximumHp == maximumHp)
            {
                return;
            }

            this.currentHp = currentHp;
            this.maximumHp = maximumHp;

            progressBarController
                .SetPencentage(currentHp / maximumHp)
                .SetText((0 < currentHp && currentHp < 1 ? "~" : "") + (int)currentHp + "/" + (int)maximumHp);
        }

        private void SetHeart(float percentage)
        {
            heartBlinking = percentage <= 0.3f;

            if (!heartBlinking)
            {
                heart.color = Color.white;
            }
            else
            {
                showHeart = false;
                blinkInterval = Mathf.Max(0.1f, Mathf.Min(0.5f, percentage / 0.3f * 0.5f));
            }
        }

        public void OnStoreChanged(StoreNames storeName, IState state)
        {
            switch (storeName)
            {
                case StoreNames.LevelConfigurationStore:
                    var levelConfigs = (LevelConfigurationState)state;
                    if (ForSinglePlayer && levelConfigs.NumberOfPlayers < numberOfPlayers)
                    {
                        numberOfPlayers = levelConfigs.NumberOfPlayers;
                        var playerNumber = GeneralUtils.GetFirstAlivePlayerNumber(levelConfigs);

                        if (playerNumber >= 0)
                        {
                            PlayerNumber = playerNumber;
                            var playerState0 = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
                            SetHpDisplay(playerState0.PlayerDatas[PlayerNumber].CurrentHp, playerState0.PlayerDatas[PlayerNumber].MaximumHp);
                        }
                    }
                    break;
                case StoreNames.PlayerStore:
                    PlayerState playerState1 = (PlayerState)state;
                    SetHpDisplay(playerState1.PlayerDatas[PlayerNumber].CurrentHp, playerState1.PlayerDatas[PlayerNumber].MaximumHp);
                    SetHeart(playerState1.PlayerDatas[PlayerNumber].CurrentHp / playerState1.PlayerDatas[PlayerNumber].MaximumHp);
                    break;
                default:
                    throw new InvalidStoreEventException(storeName);
            }
        }

        private void Update()
        {
            if (heartBlinking)
            {
                blinkTimeout -= Time.deltaTime;
                if (blinkTimeout <= 0)
                {
                    blinkTimeout = blinkInterval;
                    var color = heart.color;
                    color.a = showHeart ? 1 : 0;
                    heart.color = color;
                    showHeart = !showHeart;
                }
            }
        }

        private void OnDestroy()
        {
            storeManager.Unsubscribe(this);
        }
    }
}
