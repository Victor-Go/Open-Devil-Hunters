using Code.Scripts.Behaviour.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.ActiveSkill
{
  public class ActiveSkillManager : PauseableGameObject, IEventHandler
  {
    public int PlayerNumber;
    public bool ForSinglePlayer;

    private int numberOfPlayers;
    private SkillAndUpgradeManager skillAndUpgradeManager;
    private SkillImage skillImageController;
    private GameObject skillImageGameObject;
    private EventManager eventManager;

    private float countdown;

    private bool activated;

    protected override void Awake()
    {
      base.Awake();
      skillAndUpgradeManager = SkillAndUpgradeManager.Instance;
      eventManager = EventManager.Instance;

      eventManager.AddEventHandler(Events.LEVEL_INITIALIZED, this);
    }

    public void OnEvent(Events @event, IEventData data)
    {
      switch (@event)
      {
        case Events.LEVEL_INITIALIZED:
          OnLevelInitialized();
          break;
        default:
          throw new InvalidEventHandlingException(@event);
      }
    }

    private void OnLevelInitialized()
    {
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      activated = PlayerNumber < levelConfigs.NumberOfPlayers;
      if (!activated)
      {
        return;
      }

      numberOfPlayers = levelConfigs.NumberOfPlayers;

      storeManager
        .Subscribe(StoreNames.LevelConfigurationStore, this)
        .Subscribe(StoreNames.ActiveSkillStore, this);

      var activeSkillState = storeManager.GetState<ActiveSkillState>(StoreNames.ActiveSkillStore);
      SetActiveSkill(activeSkillState.SkillImageIndicators[PlayerNumber]);
    }

    public override void OnStoreChanged(StoreNames storeName, IState state)
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
              var activeSkillState0 = storeManager.GetState<ActiveSkillState>(StoreNames.ActiveSkillStore);
              SetActiveSkill(activeSkillState0.SkillImageIndicators[PlayerNumber]);
            }
          }

          break;
        case StoreNames.GameStateStore:
          HandleGameStateChanged((GameState)state);
          break;
        case StoreNames.ActiveSkillStore:
          var activeSkillState1 = (ActiveSkillState)state;
          UpdateCountdown(activeSkillState1.CoolingCountdowns[PlayerNumber]);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void SetActiveSkill(string skillImageIndicator)
    {
      if (string.IsNullOrEmpty(skillImageIndicator))
      {
        return;
      }

      var resourceManager = ResourceManager.Instance;
      if (skillImageGameObject == null)
      {
        skillImageGameObject = Instantiate(resourceManager.GetResource("UI/Header/SkillImage"));
        skillImageGameObject.transform.SetParent(transform, false);
        skillImageGameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        skillImageController = skillImageGameObject.GetComponent<SkillImage>();
      }

      skillImageController.SetImage(resourceManager.GetResource(skillImageIndicator));
    }

    public void UpdateCountdown(float coolingCountdown)
    {
      if (skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].ActiveSkill == null)
      {
        return;
      }

      var skillConfig = skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].ActiveSkillConfigurations;
      skillImageController.SetProgress(1 - (coolingCountdown / skillConfig.Countdown));
    }


    private void Update()
    {
      if (paused || !activated)
      {
        return;
      }

      var triggerSkill = InputUtils.GetButtonDown(PlayerNumber, InputButtonsDown.ACTIVE_SKILL);
      if (triggerSkill && skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].ActiveSkill != null)
      {
        var activeSkillState = storeManager.GetState<ActiveSkillState>(StoreNames.ActiveSkillStore);
        if (activeSkillState.CoolingCountdowns[PlayerNumber] <= 0)
        {
          skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].LaunchActiveSkill();

          var skillConfig = skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].ActiveSkillConfigurations;

          countdown = skillConfig.Countdown;
          storeManager.Commit(StoreNames.ActiveSkillStore, StoreActions.ActiveSkillStore_SET_COOLING_COUNTDOWN,
            new ActiveSkillData
            {
              PlayerNumber = PlayerNumber,
              CoolingCountdown = skillConfig.Countdown
            });
        }
      }

      if (countdown > 0)
      {
        countdown -= Time.deltaTime;
        storeManager.Commit(StoreNames.ActiveSkillStore, StoreActions.ActiveSkillStore_SET_COOLING_COUNTDOWN,
          new ActiveSkillData
          {
            PlayerNumber = PlayerNumber,
            CoolingCountdown = countdown
          });
      }
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();

      eventManager.RemoveEventHandler(this);
    }
  }
}