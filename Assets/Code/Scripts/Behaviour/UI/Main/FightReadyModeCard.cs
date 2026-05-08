using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class FightReadyModeCard : MonoBehaviour, IStoreChangedHandler
  {
    public PlayModes PlayMode;

    private StoreManager storeManager;
    private ResourceManager resourceManager;

    private GameObject selectedFrame;
    private GameObject notAvailableMask;

    private bool available;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      resourceManager = ResourceManager.Instance;

      selectedFrame = transform.Find("Selected").gameObject;
      notAvailableMask = transform.Find("NotAvailableInDemo").gameObject;
    }

    private void Start()
    {
      storeManager.Subscribe(StoreNames.LevelConfigurationStore, this);

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      UpdateLevelConfigs(levelConfigs);

      available = GeneralConfigurations.PlayModesAvailableInDemoVersion.Contains(PlayMode) ||
                  GeneralConfigurations.Version == Version.OFFICIAL;
      notAvailableMask.SetActive(!available);
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          UpdateLevelConfigs((LevelConfigurationState)state);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void UpdateLevelConfigs(LevelConfigurationState levelConfigs)
    {
      if (levelConfigs.PlayMode == PlayMode)
      {
        selectedFrame.SetActive(true);
      }
      else
      {
        selectedFrame.SetActive(false);
      }
    }

    public void SetAsCurrentPlayMode()
    {
      if (!available) return;

      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      storeManager.Commit(StoreNames.LevelConfigurationStore, StoreActions.LevelConfigurationStore_SET_PLAY_MODE,
        new LevelConfigurationData()
        {
          PlayMode = PlayMode,
        });
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}