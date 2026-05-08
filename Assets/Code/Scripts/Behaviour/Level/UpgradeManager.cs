using Code.Scripts.Src;
using Code.Scripts.Behaviour.UI.Upgrade;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Player;
using UnityEngine;

namespace Code.Scripts.Behaviour.Level
{
  public class UpgradeManager : MonoBehaviour, IStoreChangedHandler
  {
    private StoreManager storeManager;
    private EventManager eventManager;
    private ResourceManager resourceManager;
    private UpgradeUI upgradeUi;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      eventManager = EventManager.Instance;
      resourceManager = ResourceManager.Instance;
    }

    private void Start()
    {
      storeManager
        .Subscribe(StoreNames.PlayerStore, this);
    }

    private void ShowUpgradeUi(int playerNumber)
    {
      var window = WindowManager.Instance.OpenWindow(WindowNames.UpgradeUI, OpenWindowActions.WaitForOthers);
      upgradeUi = window.GetComponent<UpgradeUI>();
      upgradeUi.SetPlayerNumber(playerNumber);
    }

    private void SetPlayerNewExperience(int playerNumber)
    {
      var playerState = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
      var currentLevel = playerState.PlayerDatas[playerNumber].Level;
      var nextLevelRequiredExperience = PlayerExperienceConfigurations.GetLevelUpRequiredExperience(currentLevel + 1);
      var currentLevelExperience = PlayerExperienceConfigurations.GetLevelUpRequiredExperience(currentLevel);

      storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_LEVEL_UP_DATA, new PlayerActionData
      {
        PlayerNumber = playerNumber,
        Level = currentLevel + 1,
        CurrentLevelExperience = currentLevelExperience,
        RequiredExperienceToLevelUp = nextLevelRequiredExperience,
      });
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.PlayerStore:
          var playerState = (PlayerState)state;

          for (var playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
          {
            if (playerState.PlayerDatas[playerNumber] == null)
            {
              break;
            }

            float currentExp = playerState.PlayerDatas[playerNumber].CurrentExperience,
              requiredExp = playerState.PlayerDatas[playerNumber].RequiredExperienceToLevelUp;

            if (currentExp >= requiredExp)
            {
              SetPlayerNewExperience(playerNumber);
              ShowUpgradeUi(playerNumber);
            }
          }

          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }
  }
}