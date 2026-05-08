using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI
{
  public class ReloadingIndicatorUi : MonoBehaviour, IStoreChangedHandler
  {
    public int PlayerNumber { get; set; }

    private StoreManager storeManager;
    private Image progressBar;

    private void Awake()
    {
      storeManager = StoreManager.Instance;

      progressBar = GetComponent<Image>();
    }

    private void Start()
    {
      storeManager
        .Subscribe(StoreNames.LevelConfigurationStore, this)
        .Subscribe(StoreNames.AttackControlStore, this);

      var state = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore);
      HandleAttackControlState(state);
    }

    private void HandleAttackControlState(AttackControlState state)
    {
      if (state.AttackControlDatas[PlayerNumber].Reloading)
      {
        gameObject.SetActive(true);
        progressBar.fillAmount = 1 - (state.AttackControlDatas[PlayerNumber].ReloadCountdown /
                                      state.AttackControlDatas[PlayerNumber].AttackControlConfigurations.ReloadTime);
      }
      else
      {
        gameObject.SetActive(false);
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          if (!((LevelConfigurationState)state).PlayerAlive[PlayerNumber])
          {
            Destroy(gameObject);
          }

          break;
        case StoreNames.AttackControlStore:
          var attackState = (AttackControlState)state;
          HandleAttackControlState(attackState);
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