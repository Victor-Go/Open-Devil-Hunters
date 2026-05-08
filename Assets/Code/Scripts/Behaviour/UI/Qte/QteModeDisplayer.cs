using Code.Scripts.Behaviour.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Behaviour.Level;
using Code.Scripts.Src.Store.Level;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI.Qte
{
  public class QteModeDisplayer : MonoBehaviour, IStoreChangedHandler
  {
    private int playerNumber;

    private StoreManager storeManager;

    private GameObject keyboard;
    private GameObject controller;

    private void Awake()
    {
      keyboard = transform.Find("KeyboardButton").gameObject;
      controller = transform.Find("ControllerButton").gameObject;

      storeManager = StoreManager.Instance;
    }

    public void SetPlayerNumber(int playerNumber)
    {
      this.playerNumber = playerNumber;

      storeManager.Subscribe(StoreNames.LevelSettingStore, this);

      var levelSetting = storeManager.GetState<LevelSettingState>(StoreNames.LevelSettingStore);
      HandleAimingMode(levelSetting);
    }

    private void HandleAimingMode(LevelSettingState levelSetting)
    {
      keyboard.SetActive(false);
      controller.SetActive(false);

      switch (levelSetting.AimingModes[playerNumber])
      {
        case AimingMode.NONE:
        case AimingMode.AUTO_AIMING:
        case AimingMode.MOUSE:
          keyboard.SetActive(true);
          break;
        case AimingMode.JOYSTICK:
          keyboard.SetActive(true);
          break;
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelSettingStore:
          HandleAimingMode((LevelSettingState)state);
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