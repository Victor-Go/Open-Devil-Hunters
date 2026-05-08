using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using System.Collections;
using System.Linq;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
    public class ExperienceBar : MonoBehaviour, IStoreChangedHandler
    {
        public int PlayerNumber;
        public bool ForSinglePlayer;

        private int numberOfPlayers;
        private ProgressBarController progressBarController;
        private StoreManager storeManager;
        private Coroutine blink;
        private Color defaultColor;
        private bool isDefaultColor = true;

        private void Awake()
        {
            storeManager = StoreManager.Instance;

            progressBarController = GetComponentInChildren<ProgressBarController>();
        }

        private void Start()
        {
            storeManager
                .Subscribe(StoreNames.LevelConfigurationStore, this)
                .Subscribe(StoreNames.PlayerStore, this);

            defaultColor = progressBarController.GetFillColor();

            numberOfPlayers = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore).NumberOfPlayers;

            var playerState = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
            SetProgressBar(playerState);

            if (!ForSinglePlayer)
            {
                var width = transform.parent.GetComponent<RectTransform>().rect.width;
                var expBarRT = GetComponent<RectTransform>();
                var expBarSize = expBarRT.sizeDelta;
                expBarRT.sizeDelta = new Vector2(width / 2 - 10, expBarSize.y);
            }
        }

        private void SetLevel(int level)
        {
            progressBarController.SetText("Level." + level);
        }

        private void SetExperienceBarStyleToDefault()
        {
            progressBarController.SetFillColor(defaultColor);
            progressBarController.SetTextColor(Color.white);
        }

        public IEnumerator Blink()
        {
            WaitForSeconds wait = new WaitForSeconds(0.1f);
            while (true)
            {
                yield return wait;
                if (isDefaultColor)
                {
                    isDefaultColor = false;
                    progressBarController.SetFillColor(Color.white);
                    progressBarController.SetTextColor(defaultColor);
                }
                else
                {
                    isDefaultColor = true;
                    SetExperienceBarStyleToDefault();
                }
            }
        }

        private void SetProgressBar(PlayerState playerState)
        {
            var currentExp = playerState.PlayerDatas[PlayerNumber].CurrentExperience;
            var currentLevelExp = playerState.PlayerDatas[PlayerNumber].CurrentLevelExperience;
            var requiredExp = playerState.PlayerDatas[PlayerNumber].RequiredExperienceToLevelUp;
            var level = playerState.PlayerDatas[PlayerNumber].Level;
            SetLevel(level);

            var percentage = (float)(currentExp - currentLevelExp) / (requiredExp - currentLevelExp);
            progressBarController.SetPencentage(percentage);

            if (percentage >= 1)
            {
                if (blink == null)
                {
                    blink = StartCoroutine(Blink());
                }
            }
            else if (blink != null)
            {
                StopCoroutine(blink);
                blink = null;
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
                            SetProgressBar(playerState0);
                        }
                    }
                    break;
                case StoreNames.PlayerStore:
                    SetExperienceBarStyleToDefault();
                    PlayerState playerState1 = (PlayerState)state;
                    SetProgressBar(playerState1);
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
