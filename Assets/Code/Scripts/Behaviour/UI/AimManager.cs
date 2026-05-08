using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
  public class AimManager : MonoBehaviour, IStoreChangedHandler
  {
    private int playerNumber;

    private StoreManager storeManager;

    private MagazineIndicator magazineIndicator;
    private MagazineIndicatorUi magazineIndicatorUi;
    private ReloadingIndicator reloadingIndicator;
    private ReloadingIndicatorUi reloadingIndicatorUi;

    private void Awake()
    {
      storeManager = StoreManager.Instance;

      magazineIndicator = GetComponentInChildren<MagazineIndicator>();
      magazineIndicatorUi = GetComponentInChildren<MagazineIndicatorUi>();
      reloadingIndicator = GetComponentInChildren<ReloadingIndicator>();
      reloadingIndicatorUi = GetComponentInChildren<ReloadingIndicatorUi>();
    }

    private void Start()
    {
      storeManager.Subscribe(StoreNames.LevelConfigurationStore, this);
    }

    public void SetPlayerNumber(int playerNumber)
    {
      this.playerNumber = playerNumber;

      if (magazineIndicator != null)
      {
        magazineIndicator.PlayerNumber = playerNumber;
      }

      if (magazineIndicatorUi != null)
      {
        magazineIndicatorUi.PlayerNumber = playerNumber;
      }

      if (reloadingIndicator != null)
      {
        reloadingIndicator.PlayerNumber = playerNumber;
      }

      if (reloadingIndicatorUi != null)
      {
        reloadingIndicatorUi.PlayerNumber = playerNumber;
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          if (!((LevelConfigurationState)state).PlayerAlive[playerNumber])
          {
            Destroy(gameObject);
          }

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