using System;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Src;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Store.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI
{
  public enum AimingTypes
  {
    Mouse,
    Automatic,
    Controller,
  }

  public class AmingModeChangedEvent : IEventData
  {
    public int PlayerNumber { get; set; }
    public AimingTypes AimingType { get; set; }
  }

  public class ControlIndicator : MonoBehaviour, IEventHandler, IStoreChangedHandler
  {
    public int PlayerNumber;

    private readonly StoreManager storeManager = StoreManager.Instance;
    private readonly EventManager eventManager = EventManager.Instance;

    private Text text;

    private string aiming;
    private string firing;

    private void Awake()
    {
      text = GetComponent<Text>();

      eventManager.AddEventHandler(Events.AIMING_CHANGED, this);
      storeManager
        .Subscribe(StoreNames.LevelConfigurationStore, this)
        .Subscribe(StoreNames.LevelSettingStore, this);
    }

    private void Start()
    {
      var levelSettings = storeManager.GetState<LevelSettingState>(StoreNames.LevelSettingStore);

      setFiring(levelSettings.FiringModes[PlayerNumber]);

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      if (!levelConfigs.PlayerAlive[PlayerNumber])
      {
        Destroy(gameObject);
      }
    }

    private void setFiring(FiringMode firingMode)
    {
      firing = firingMode switch
      {
        FiringMode.Manual => I18nUtils.GetText("UI/Control/ManualFiring") + (PlayerNumber == 0 ? " (F2)" : " (F11)"),
        FiringMode.Automatic => I18nUtils.GetText("UI/Control/AutoFiring") + (PlayerNumber == 0 ? " (F2)" : " (F11)"),
        _ => ""
      };
      text.text = $"{aiming}\n{firing}";
    }

    public void OnEvent(Events @event, IEventData data)
    {
      if (@event == Events.AIMING_CHANGED &&
          data is AmingModeChangedEvent eventData &&
          eventData.PlayerNumber == PlayerNumber)
      {
        aiming = eventData.AimingType switch
        {
          AimingTypes.Mouse => I18nUtils.GetText("UI/Control/MouseAiming") + (PlayerNumber == 0 ? " (F1)" : ""),
          AimingTypes.Automatic => I18nUtils.GetText("UI/Control/AutoAiming"),
          AimingTypes.Controller => I18nUtils.GetText("UI/Control/ControllerAiming"),
          _ => ""
        };

        text.text = $"{aiming}\n{firing}";
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          var levelConfigs = (LevelConfigurationState)state;
          if (!levelConfigs.PlayerAlive[PlayerNumber])
          {
            Destroy(gameObject);
          }

          break;

        case StoreNames.LevelSettingStore:
          var levelSettings = (LevelSettingState)state;
          setFiring(levelSettings.FiringModes[PlayerNumber]);
          break;
        default: throw new InvalidStoreEventException(storeName);
      }
    }

    private void OnDestroy()
    {
      eventManager.RemoveEventHandler(this);
      storeManager.Unsubscribe(this);
    }
  }
}