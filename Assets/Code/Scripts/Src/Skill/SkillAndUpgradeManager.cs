using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Scripts.Src.Skill
{
  [Serializable]
  public class InvalidSkillIdException : Exception
  {
    public InvalidSkillIdException() : base()
    {
    }

    public InvalidSkillIdException(string skillId)
      : base($"Invalid PlayerSkill Id: {skillId}")
    {
      Console.WriteLine("Invalid PlayerSkill Id: {0}", skillId);
    }
  }

  public class BasicSkillContainer
  {
    public string SkillId { get; set; }
    public List<SkillTag> SkillTags { get; set; }
    public string SkillPrefabName { get; set; }
    public int SkillCount { get; set; }
    public BasicSkillConfigurations SkillConfigurations { get; set; }
  }

  public class TimingSkillContainer : BasicSkillContainer
  {
    public string ImageName { get; set; }
    public float Interval { get; set; }
    public float TimeToLaunch { get; set; }
  }

  public class SurroundSkillContainer
  {
    public string SkillId { get; set; }
    public List<SkillTag> SkillTags { get; set; }
    public BasicSkillConfigurations SkillConfigurations { get; set; }
    public ISurroundSkill SkillController { get; set; }
    public GameObject SkillObject { get; set; }
  }

  public interface IBasicSkillUpgrader
  {
    public BasicSkillContainer UpdateBasicSkill(BasicSkillContainer previousSkillContainer);
  }

  public struct TimingAction
  {
    public string ScheduleId { get; set; }
  }

  public class SkillAndUpgradeController : IStoreChangedHandler
  {
    public int PlayerNumber { get; set; }

    private readonly ObjectPool objectPool;
    private readonly StoreManager storeManager;
    private readonly Scheduling scheduling;
    private PlayerManager playerManager;

    #region Skills

    public BasicSkillContainer BasicSkill { get; set; } = new();
    public List<SurroundSkillContainer> SurroundSkills { get; set; } = new();

    #endregion

    // Timing Actions.
    private readonly List<TimingAction> timingActions = new();

    // Active Skill.
    public PlayerActiveSkill ActiveSkill { get; set; }
    public ActiveSkillConfigurations ActiveSkillConfigurations { get; set; }

    #region Interceptros

    public List<IObtainExperienceInterceptor> ObtainExperienceInterceptors { get; set; } = new();
    public List<Func<ObtainExperienceContext, ObtainExperienceContext>> ObtainExperienceActions { get; set; } = new();

    public List<IPlayerReloadInterceptor> PlayerReloadInterceptors { get; set; } = new();
    public List<Func<PlayerReloadContext, PlayerReloadContext>> PlayerReloadActions { get; set; } = new();

    public List<IPlayerHurtInterceptor> PlayerHurtInterceptors { get; set; } = new();
    public List<Func<PlayerHurtContext, PlayerHurtContext>> PlayerHurtActions { get; set; } = new();

    public List<IPlayerDieInterceptor> PlayerDieInterceptors { get; set; } = new();
    public List<Func<PlayerDieContext, PlayerDieContext>> PlayerDieActions { get; set; } = new();

    public List<IEnemyHurtInterceptor> EnemyDirectHurtInterceptors { get; set; } = new();
    public List<Func<EnemyHurtContext, EnemyHurtContext>> EnemyDirectHurtActions { get; set; } = new();

    public List<IEnemyDieInterceptor> EnemyDieInterceptors { get; set; } = new();
    public List<Func<EnemyDieContext, EnemyDieContext>> EnemyDieActions { get; set; } = new();

    #endregion

    public SkillAndUpgradeController()
    {
      objectPool = ObjectPool.Instance;
      storeManager = StoreManager.Instance;
      scheduling = Scheduling.Instance;

      storeManager
        .Subscribe(StoreNames.TimingSkillStore, this)
        .Subscribe(StoreNames.HighPrecisionLevelTimeStore, this);
    }

    public void SetPlayerManager(PlayerManager playerManager)
    {
      this.playerManager = playerManager;
    }

    #region Basic Skill

    public void SetBasicSkill(string skillId, string skillPrefabName, int count,
      BasicSkillConfigurations skillConfigurations)
    {
      skillConfigurations.SkillId = skillId;
      var skillContainer = new BasicSkillContainer
      {
        SkillId = skillId,
        SkillTags = skillConfigurations.SkillTags,
        SkillPrefabName = skillPrefabName,
        SkillCount = count,
        SkillConfigurations = (BasicSkillConfigurations)skillConfigurations.Clone()
      };

      BasicSkill = skillContainer;
    }

    public void UpgradeBasicSkill(IBasicSkillUpgrader skillUpgrader)
    {
      if (skillUpgrader == null)
      {
        return;
      }

      BasicSkill = skillUpgrader.UpdateBasicSkill(BasicSkill);
    }

    public void UpgradeBasicSkill(Func<BasicSkillContainer, BasicSkillContainer> upgrader)
    {
      BasicSkill = upgrader(BasicSkill);
    }

    // Note: If changed launch skill logics, should also change in EnemyHurt.OnTriggerStay2D since it will create skill fission. 
    public void LaunchBasicSkill(TrajectoryArguments trajectoryArguments, Vector2 resetPosition)
    {
      List<GameObject> skillObjects = new();
      for (var i = 0; i < BasicSkill.SkillCount; i++)
      {
        var skillObject = objectPool.GetObject(BasicSkill.SkillPrefabName, resetPosition);

        skillObject.transform.SetParent(null);

        var skillController = skillObject.GetComponent<PlayerBasicSkill>();
        skillController.SetSkillConfigurations(BasicSkill.SkillConfigurations);
        skillController.CanMoveWhenTimeStopped = true;
        skillController.PlayerNumber = PlayerNumber;

        if (i == 0)
        {
          skillController.PlayLaunchSound();
        }

        skillObjects.Add(skillObject);
      }

      var trajectoryLaunch = SkillAndUpgradeManager.GetTrajectory(BasicSkill.SkillConfigurations.TrajectoryType);
      trajectoryLaunch(trajectoryArguments, skillObjects, BasicSkill.SkillConfigurations.Dispersion);
    }

    #endregion

    #region Active Skill

    public void SetActiveSkill(ActiveSkillPreset preset, ActiveSkillConfigurations skillConfigurations)
    {
      ActiveSkill = (PlayerActiveSkill)Activator.CreateInstance(preset.ActiveSkillType);
      ActiveSkillConfigurations = (ActiveSkillConfigurations)skillConfigurations.Clone();

      storeManager.Commit(StoreNames.ActiveSkillStore, StoreActions.ActiveSkillStore_SET_ACTIVE_SKILL_ICON,
        new ActiveSkillData()
        {
          PlayerNumber = PlayerNumber,
          SkillImageIndicator = preset.ImageIndicator
        });
    }

    public void LaunchActiveSkill()
    {
      if (ActiveSkill == null)
      {
        return;
      }

      ActiveSkill.SetSkillConfigurations(ActiveSkillConfigurations);
      ActiveSkill.Launch(PlayerNumber);
    }

    #endregion

    #region Timing Skill

    private void LaunchTimingSkill(TimingSkillContainer timingSkillContainer, TrajectoryArguments trajectoryArguments,
      Vector2 resetPosition)
    {
      List<GameObject> skillObjects = new();
      for (int i = 0; i < timingSkillContainer.SkillCount; i++)
      {
        GameObject skillObject = objectPool.GetObject(timingSkillContainer.SkillPrefabName, resetPosition);

        skillObject.transform.SetParent(null);

        var skillController = skillObject.GetComponent<PlayerBasicSkill>();
        skillController.SetSkillConfigurations(timingSkillContainer.SkillConfigurations);
        skillController.CanMoveWhenTimeStopped = true;
        skillController.PlayerNumber = PlayerNumber;

        skillObjects.Add(skillObject);
      }

      var trajectoryLaunch =
        SkillAndUpgradeManager.GetTrajectory(timingSkillContainer.SkillConfigurations.TrajectoryType);
      trajectoryLaunch(trajectoryArguments, skillObjects, timingSkillContainer.SkillConfigurations.Dispersion);
    }

    public void SetTimingSkill(string skillId, string skillPrefabName, int skillCount, float interval,
      string skillImageName, BasicSkillConfigurations skillConfigurations, bool launchImmediately = true)
    {
      if (string.IsNullOrEmpty(skillId))
      {
        throw new NullReferenceException($"{skillPrefabName}");
      }

      var skillConfigs = (BasicSkillConfigurations)skillConfigurations.Clone();
      skillConfigs.SkillId = skillId;

      var container = new TimingSkillContainer()
      {
        SkillId = skillId,
        SkillTags = skillConfigurations.SkillTags,
        SkillConfigurations = skillConfigs,
        SkillPrefabName = skillPrefabName,
        SkillCount = skillCount,
        ImageName = skillImageName,
        Interval = interval,
        TimeToLaunch = launchImmediately ? 0 : interval,
      };

      storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_SET_TIMING_SKILL,
        new TimingSkillActionData()
        {
          PlayerNumber = PlayerNumber,
          TimingSkillContainer = container,
        });
    }

    public void RemoveTimingSkill(string skillId)
    {
      storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
        new TimingSkillActionData()
        {
          PlayerNumber = PlayerNumber,
          SkillId = skillId,
        });
    }

    #endregion

    #region Surround Skill

    public void AddSurroundSkill(
      SurroundSkillGroups group,
      float distance,
      string skillId,
      string skillPrefabName,
      BasicSkillConfigurations skillConfigurations
    )
    {
      var skillObject = objectPool.GetObject(skillPrefabName);
      var controller = skillObject.GetComponent<ISurroundSkill>();
      controller.SetSkillConfigurations(new(skillConfigurations));

      var skillConfigs = new BasicSkillConfigurations(skillConfigurations)
      {
        SkillId = skillId
      };

      var container = new SurroundSkillContainer()
      {
        SkillId = skillId,
        SkillTags = skillConfigurations.SkillTags,
        SkillConfigurations = skillConfigs,
        SkillController = controller,
        SkillObject = skillObject,
      };

      SurroundSkills.Add(container);

      playerManager.AddSurroundSkill(group, skillObject, distance);
    }

    public void UpgradeSurroundSkill(Func<SurroundSkillContainer, SurroundSkillContainer> upgrader)
    {
      for (var i = 0; i < SurroundSkills.Count; i++)
      {
        SurroundSkills[i] = upgrader(SurroundSkills[i]);
        SurroundSkills[i].SkillController.SetSkillConfigurations(SurroundSkills[i].SkillConfigurations);
      }
    }

    #endregion

    #region Timing Action

    public string AddTimingAction(float interval, Action action)
    {
      var scheduleId = scheduling.SetInterval(() => action(), interval);
      timingActions.Add(new()
      {
        ScheduleId = scheduleId
      });
      return scheduleId;
    }

    public void RemoveTimingAction(string scheduleId)
    {
      var schedules =
        timingActions.Where(s => s.ScheduleId.Equals(scheduleId)).Select(s => s.ScheduleId).ToList();

      foreach (var schedule in schedules)
      {
        scheduling.ClearSchedule(schedule);
      }

      timingActions.RemoveAll(s => s.ScheduleId.Equals(scheduleId));
    }

    #endregion

    #region Interceptors

    public void AddObtainExperienceInterceptor(IObtainExperienceInterceptor obtainExperienceInterceptor)
    {
      ObtainExperienceInterceptors.Add(obtainExperienceInterceptor);
    }

    public void AddObtainExperienceAction(Func<ObtainExperienceContext, ObtainExperienceContext> func)
    {
      ObtainExperienceActions.Add(func);
    }

    public void AddPlayerReloadInterceptor(IPlayerReloadInterceptor playerReloadInterceptor)
    {
      PlayerReloadInterceptors.Add(playerReloadInterceptor);
    }

    public void AddPlayerReloadAction(Func<PlayerReloadContext, PlayerReloadContext> func)
    {
      PlayerReloadActions.Add(func);
    }

    public void AddPlayerHurtInterceptor(IPlayerHurtInterceptor playerHurtInterceptor)
    {
      PlayerHurtInterceptors.Add(playerHurtInterceptor);
    }

    public void AddPlayerHurtAction(Func<PlayerHurtContext, PlayerHurtContext> func)
    {
      PlayerHurtActions.Add(func);
    }

    public void AddPlayerDieInterceptor(IPlayerDieInterceptor playerDieInterceptor)
    {
      PlayerDieInterceptors.Add(playerDieInterceptor);
    }

    public void AddPlayerDieAction(Func<PlayerDieContext, PlayerDieContext> func)
    {
      PlayerDieActions.Add(func);
    }

    public void AddEnemyDirectHurtInterceptor(IEnemyHurtInterceptor enemyHurtInterceptor)
    {
      EnemyDirectHurtInterceptors.Add(enemyHurtInterceptor);
    }

    public void AddEnemyDirectHurtAction(Func<EnemyHurtContext, EnemyHurtContext> func)
    {
      EnemyDirectHurtActions.Add(func);
    }

    public void AddEnemyDieInterceptor(IEnemyDieInterceptor enemyDieInterceptor)
    {
      EnemyDieInterceptors.Add(enemyDieInterceptor);
    }

    public void AddEnemyDieAction(Func<EnemyDieContext, EnemyDieContext> func)
    {
      EnemyDieActions.Add(func);
    }

    #endregion

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.TimingSkillStore:
          var timingState = (TimingSkillState)state;
          if (PlayerNumber < timingState.NumberOfPlayers)
          {
            var timingSkills = timingState.PlayerSkills[PlayerNumber].Skills;
            HandleTimingSkillChanged(timingSkills);
          }

          break;
        case StoreNames.HighPrecisionLevelTimeStore:
          var dt = ((HighPrecisionLevelTimeState)state).LastDeltaTime;
          Update(dt);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void HandleTimingSkillChanged(List<TimingSkillContainer> timingSkillContainers)
    {
      for (var index = 0; index < timingSkillContainers.Count; index++)
      {
        var container = timingSkillContainers[index];
        var playerPosition = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore)
          .PlayerPositions[PlayerNumber];
        
        if (container.TimeToLaunch <= 0)
        {
          var trajectoryArgs = new TrajectoryArguments()
          {
            LaunchPosition = playerPosition,
            PlayerPosition = playerPosition,
            TargetPosition = AttackUtils.GetTargetPosition(PlayerNumber, storeManager)
          };
          LaunchTimingSkill(container, trajectoryArgs, playerPosition);

          storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_SET_TIME_TO_LAUNCH,
            new TimingSkillActionData()
            {
              PlayerNumber = PlayerNumber,
              SkillIndex = index,
              TimeToLaunch = container.Interval,
            });
        }
      }
    }

    private void Update(float dt)
    {
      if (PlayerNumber == 0) // It only needs to be updated once.
      {
        storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_UPDATE_DELTA_TIME,
          new TimingSkillActionData()
          {
            DeltaTime = dt
          });
      }
    }

    public void Reset()
    {
      ActiveSkill = null;

      storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_RESET,
        new TimingSkillActionData()
        {
          PlayerNumber = PlayerNumber,
        });
      foreach (var timing in timingActions)
      {
        scheduling.ClearSchedule(timing.ScheduleId);
      }

      timingActions.Clear();

      foreach (var surroundSkill in SurroundSkills)
      {
        Object.Destroy(surroundSkill.SkillObject);
      }

      SurroundSkills.Clear();

      ObtainExperienceInterceptors.Clear();
      ObtainExperienceActions.Clear();
      PlayerReloadInterceptors.Clear();
      PlayerReloadActions.Clear();
      PlayerHurtInterceptors.Clear();
      PlayerHurtActions.Clear();
      PlayerDieInterceptors.Clear();
      PlayerDieActions.Clear();
      EnemyDieInterceptors.Clear();
      EnemyDieActions.Clear();

      storeManager.Unsubscribe(this);
    }

    ~SkillAndUpgradeController()
    {
      Reset();
    }
  }

  public class SkillAndUpgradeManager : Singleton<SkillAndUpgradeManager>
  {
    public SkillAndUpgradeController[] SkillAndUpgradeControllers { get; } =
      new SkillAndUpgradeController[GeneralConfigurations.MaximumPlayers]; // Each controller represents one player.

    public static Action<TrajectoryArguments, List<GameObject>, float> GetTrajectory(
      TrajectoryTypes trajectoryType)
    {
      switch (trajectoryType)
      {
        case TrajectoryTypes.NormalTrajectory: return NormalTrajectory.LaunchSkill;
        case TrajectoryTypes.CircularTrajectory: return CircularTrajectory.LaunchSkill;
        case TrajectoryTypes.SpiralTrajectory: return SpiralTrajectory.LaunchSkill;
        case TrajectoryTypes.CentralMultiTrajectory: return CentralMultiTrajectory.LaunchSkill;
        case TrajectoryTypes.JeanneDArcSkillTrajectory: return JeanneDArcSkillTrajectory.LaunchSkill;
        case TrajectoryTypes.HailTrajectory: return HailTrajectory.LaunchSkill;
        case TrajectoryTypes.MeteoriteTrajectory: return MeteoriteTrajectory.LaunchSkill;
        case TrajectoryTypes.ThunderTrajectory: return ThunderTrajectory.LaunchSkill;
        case TrajectoryTypes.MineTrajectory: return MineTrajectory.LaunchSkill;
      }

      return NormalTrajectory.LaunchSkill;
    }

    private void Init()
    {
      for (var playerNumber = 0; playerNumber < SkillAndUpgradeControllers.Length; playerNumber++)
      {
        var controller = new SkillAndUpgradeController
        {
          PlayerNumber = playerNumber
        };
        SkillAndUpgradeControllers[playerNumber] = controller;
      }
    }

    protected SkillAndUpgradeManager()
    {
      Init();
    }

    public void Reset()
    {
      Init();
    }
  }
}