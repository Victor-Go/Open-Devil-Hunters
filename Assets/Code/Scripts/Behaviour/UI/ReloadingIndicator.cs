using Code.Scripts.Behaviour.UI;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Player;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
  public class ReloadingIndicator : MonoBehaviour, IStoreChangedHandler
  {
    public int PlayerNumber { get; set; }

    private StoreManager storeManager;
    private CircularProgressBar circularProgressBar;

    private void Awake()
    {
      storeManager = StoreManager.Instance;

      circularProgressBar = GetComponentInChildren<CircularProgressBar>();
    }

    private void Start()
    {
      storeManager.Subscribe(StoreNames.AttackControlStore, this);

      var state = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore);
      HandleAttackControlState(state);
    }

    private void HandleAttackControlState(AttackControlState state)
    {
      if (state.AttackControlDatas[PlayerNumber].Reloading)
      {
        gameObject.SetActive(true);
        circularProgressBar.SetProgress(1 - (state.AttackControlDatas[PlayerNumber].ReloadCountdown /
                                             state.AttackControlDatas[PlayerNumber].AttackControlConfigurations
                                               .ReloadTime));
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