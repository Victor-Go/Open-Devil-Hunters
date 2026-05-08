using Code.Scripts.Behaviour.Character;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.UI;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Player
{
  public enum FiringMode
  {
    Automatic,
    Manual,
  }

  public class AttackControl : PauseableGameObject, IObserver<PlayerActionInfo>, IObserver<AnimatorFinishedActionInfo>
  {
    [SerializeField] private float playFireAudioInterval = 2;

    public bool IndependentWeapon; // Don't forget to change this value in MovementControl and AnimatorControl.
    public int PlayerNumber { get; set; }

    private SkillAndUpgradeManager skillAndUpgradeManager;

    private float attackNotBeingPressedDuration; // Detects if should reload automatically
    private AttackControlConfigurations attackControlConfigurations;
    private float resetInAttackingStateTimeout;
    private FiringMode currentFiringMode;
    private Transform launchPositionTransform;
    private Transform attackRotationAxis;
    private Transform walkAttackRotationAxis;
    private AnimatorControl animatorController;
    private GameObject weaponGameObject;
    private AudioWrapper fireWrapper;
    private float playFireAudioCountdown;
    private AudioWrapper reloadWrapper;
    private bool localReloading; // Is used to play reload sound effects

    protected override void Awake()
    {
      base.Awake();
      skillAndUpgradeManager = SkillAndUpgradeManager.Instance;
      var animatorGameObject = transform.parent.Find("Animator");
      animator = animatorGameObject.GetComponent<Animator>();
      animatorController = animatorGameObject.GetComponent<AnimatorControl>();
      animatorController.Subscribe(this);

      if (IndependentWeapon)
      {
        var atf = animator.transform;
        attackRotationAxis = atf.Find("AttackRotationAxis");
        walkAttackRotationAxis = atf.Find("WalkAttackRotationAxis");

        weaponGameObject = transform.parent.Find("Animator/Weapon").gameObject;
        launchPositionTransform = weaponGameObject.transform.Find("LaunchPosition");
      }
      else
      {
        launchPositionTransform = transform.parent.Find("LaunchPosition");
      }
    }

    protected override void Start()
    {
      base.Start();
      storeManager
        .Subscribe(StoreNames.AttackControlStore, this)
        .Subscribe(StoreNames.LevelSettingStore, this);
    }

    public void OnNext(AnimatorFinishedActionInfo _)
    {
      DoLaunchSkill();
    }

    public override void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.AttackControlStore:
          var attackControlState = (AttackControlState)state;
          attackControlConfigurations = attackControlState.AttackControlDatas[PlayerNumber].AttackControlConfigurations;
          SetAttackAnimationSpeed(attackControlState.AttackControlDatas[PlayerNumber].AttackControlConfigurations
            .AttackCoolingTime);
          break;
        case StoreNames.LevelSettingStore:
          var levelSetting = (LevelSettingState)state;
          currentFiringMode = levelSetting.FiringModes[PlayerNumber];
          break;
        case StoreNames.GameStateStore:
          HandleGameStateChanged((GameState)state);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void SetAttackAnimationSpeed(float coolingTime)
    {
      var attackPerSecond = 1 / (coolingTime * 0.8f);

      const int minSampleSpeed = 6;
      // IMPORTANT!: This is a constant value depends on the minimum attack animation sample speed decided by the frames needed to launch a skill. This should also be changed when the value of sample speed is changed.

      animatorController.SetAttackAnimationSpeed(Mathf.Max(1, attackPerSecond / minSampleSpeed));
    }

    /**
     * This is called when the attack animation has finished.
     */
    private void DoLaunchSkill()
    {
      animatorController.ResetAttackAnimation();

      var attackControlState = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore);
      if (attackControlState.AttackControlDatas[PlayerNumber].Reloading ||
          attackControlState.AttackControlDatas[PlayerNumber].Cooling)
      {
        return;
      }

      #region Handle remaining round and reloading

      int newRemainingRoundCount = attackControlState.AttackControlDatas[PlayerNumber].RemainingRoundCount - 1;

      storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_ATTACK_CONTROL_STATE,
        new AttackControlActionData()
        {
          PlayerNumber = PlayerNumber,
          CoolingCountdown = attackControlConfigurations.AttackCoolingTime,
          ReloadCountdown = attackControlConfigurations.ReloadTime,
          RemainingRoundCount = newRemainingRoundCount,
          Reloading = false,
          Cooling = true,
        });

      if (newRemainingRoundCount <= 0)
      {
        TriggerReload();
      }

      #endregion

      #region Let SkillAndUpgradeController to launch skill

      var launchPosition = launchPositionTransform.position;
      var targetPosition = AttackUtils.GetTargetPosition(PlayerNumber, storeManager);

      var trajectoryArgs = new TrajectoryArguments()
      {
        PlayerPosition = transform.position,
        LaunchPosition = launchPosition,
        TargetPosition = targetPosition,
      };

      skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber]
        .LaunchBasicSkill(trajectoryArgs, launchPositionTransform.position);
      if (playFireAudioCountdown <= 0)
      {
        playFireAudioCountdown = playFireAudioInterval;
        fireWrapper.PlayRandomly();
      }

      #endregion
    }

    private void TryToFire()
    {
      var attackControlState = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore);

      if (attackControlState.AttackControlDatas[PlayerNumber].Reloading ||
          attackControlState.AttackControlDatas[PlayerNumber].Cooling) return;

      // Set InAttackingState toQuat true and notify MovementControl toQuat update animator direction.
      resetInAttackingStateTimeout = GeneralConfigurations.InAttackingStateDuration;
      storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_IN_ATTACKING_STATE,
        new AttackControlActionData
        {
          PlayerNumber = PlayerNumber,
          InAttackingState = true
        });

      if (IndependentWeapon)
      {
        var targetPosition = AttackUtils.GetTargetPosition(PlayerNumber, storeManager);
        animatorController.SetTargetPosition(attackRotationAxis.position, targetPosition);
      }

      // Wait for animation finish and do launch skill.
      animatorController.PlayAttackAnimation();
    }

    public void OnNext(PlayerActionInfo playerAction)
    {
      if (currentFiringMode != FiringMode.Manual) return;

      var attackControlState = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore);

      var remainingRoundCount = attackControlState.AttackControlDatas[PlayerNumber].RemainingRoundCount;
      var attackPerRound = attackControlState.AttackControlDatas[PlayerNumber].AttackControlConfigurations
        .AttackPerRound;

      if (playerAction.Attacking)
      {
        attackNotBeingPressedDuration = 0;
      }

      if (playerAction.Attacking)
      {
        TryToFire();
      }
      else if (!playerAction.Attacking && remainingRoundCount < attackPerRound) // Auto reloading
      {
        attackNotBeingPressedDuration += playerAction.FixedDeltaTime;
        if (attackNotBeingPressedDuration >= GeneralConfigurations.AttackNotBeingPressedToReloadThreshold)
        {
          attackNotBeingPressedDuration = 0;
          TriggerReload();
        }
      }
    }

    private void TriggerReload()
    {
      var attackControlState = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore);

      if (attackControlState.AttackControlDatas[PlayerNumber].Reloading)
      {
        return;
      }

      var reloadContext = new PlayerReloadContext
      {
        CoolingTime = attackControlConfigurations.AttackCoolingTime,
        ReloadTime = attackControlConfigurations.ReloadTime,
        AttackPerRound = attackControlConfigurations.AttackPerRound,
        PlayerPosition = transform.position,
      };

      var reloadInterceptors = skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].PlayerReloadInterceptors;
      foreach (var interceptor in reloadInterceptors)
      {
        reloadContext = interceptor.OnPlayerReload(reloadContext);
      }

      var reloadActions = skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].PlayerReloadActions;
      foreach (var action in reloadActions)
      {
        reloadContext = action(reloadContext);
      }

      storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_ATTACK_CONTROL_STATE,
        new AttackControlActionData
        {
          PlayerNumber = PlayerNumber,
          CoolingCountdown = reloadContext.CoolingTime,
          ReloadCountdown = reloadContext.ReloadTime,
          RemainingRoundCount = attackControlState.AttackControlDatas[PlayerNumber].RemainingRoundCount,
          Reloading = true,
          Cooling = false,
        });
      localReloading = true;
    }

    public AttackControl InitializeAttackControl(
      AttackControlConfigurations attackControlConfigurations,
      string[] playerFireAudios,
      string[] reloadAudios
    )
    {
      // Warning: When upgrade attack control settings, countdowns will be reset.
      storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_ATTACK_CONTROL_STATE,
        new AttackControlActionData
        {
          PlayerNumber = PlayerNumber,
          AttackControlConfigurations = attackControlConfigurations,
          CoolingCountdown = attackControlConfigurations.AttackCoolingTime,
          ReloadCountdown = attackControlConfigurations.ReloadTime,
          RemainingRoundCount = attackControlConfigurations.AttackPerRound,
          Reloading = false,
          Cooling = false,
        });
      localReloading = false;

      fireWrapper = new AudioWrapper(playerFireAudios, transform);
      reloadWrapper = new AudioWrapper(reloadAudios, transform);
      return this;
    }

    private void Update()
    {
      if (paused)
      {
        return;
      }

      #region Handle switch auto-firing

      var switchAutoFiring = InputUtils.GetButtonDown(PlayerNumber, InputButtonsDown.AUTO_FIRING);
      if (switchAutoFiring)
      {
        var levelSetting = storeManager.GetState<LevelSettingState>(StoreNames.LevelSettingStore);
        var nextFiringMode = levelSetting.FiringModes[PlayerNumber].Equals(FiringMode.Automatic)
          ? FiringMode.Manual
          : FiringMode.Automatic;

        storeManager.Commit(StoreNames.LevelSettingStore, StoreActions.LevelSettingStore_SET_FIRING_MODE,
          new LevelSettingData()
          {
            PlayerNumber = PlayerNumber,
            FiringMode = nextFiringMode,
          });
      }

      #endregion

      #region Handle auto-firing

      if (currentFiringMode == FiringMode.Automatic)
      {
        TryToFire();
      }

      #endregion

      var deltaTime = Time.deltaTime;
      var attackControlState = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore);
      playFireAudioCountdown -= deltaTime;

      #region Handle reloading

      if (attackControlState.AttackControlDatas[PlayerNumber].Reloading)
      {
        var reloadCountdown = attackControlState.AttackControlDatas[PlayerNumber].ReloadCountdown - deltaTime;
        var reloading = reloadCountdown > 0;

        var actionData = new AttackControlActionData
        {
          PlayerNumber = PlayerNumber,
          CoolingCountdown = attackControlState.AttackControlDatas[PlayerNumber].CoolingCountdown,
          ReloadCountdown = reloadCountdown > 0 ? reloadCountdown : 0,
          RemainingRoundCount = attackControlState.AttackControlDatas[PlayerNumber].RemainingRoundCount,
          Reloading = reloading,
          Cooling = attackControlState.AttackControlDatas[PlayerNumber].Cooling,
        };

        if (reloading)
        {
          storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_ATTACK_CONTROL_STATE,
            actionData);
          localReloading = true;
        }
        else
        {
          if (localReloading)
          {
            reloadWrapper.PlayRandomly();
          }

          localReloading = false;

          actionData.RemainingRoundCount = attackControlState.AttackControlDatas[PlayerNumber]
            .AttackControlConfigurations.AttackPerRound;
          storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_ATTACK_CONTROL_STATE,
            actionData);
        }
      }

      #endregion

      #region Handle cooling

      else if (attackControlState.AttackControlDatas[PlayerNumber].Cooling)
      {
        var coolingCountdown = attackControlState.AttackControlDatas[PlayerNumber].CoolingCountdown - deltaTime;
        var cooling = coolingCountdown > 0;

        storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_ATTACK_CONTROL_STATE,
          new AttackControlActionData()
          {
            PlayerNumber = PlayerNumber,
            CoolingCountdown = coolingCountdown > 0 ? coolingCountdown : 0,
            ReloadCountdown = attackControlState.AttackControlDatas[PlayerNumber].ReloadCountdown,
            RemainingRoundCount = attackControlState.AttackControlDatas[PlayerNumber].RemainingRoundCount,
            Reloading = attackControlState.AttackControlDatas[PlayerNumber].Reloading,
            Cooling = cooling,
          });
      }

      #endregion

      #region Handle InAttackingState

      if (resetInAttackingStateTimeout > 0)
      {
        resetInAttackingStateTimeout -= deltaTime;
        if (resetInAttackingStateTimeout <= 0)
        {
          storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_IN_ATTACKING_STATE,
            new AttackControlActionData()
            {
              PlayerNumber = PlayerNumber,
              InAttackingState = false
            });
        }
      }

      #endregion
    }
  }
}