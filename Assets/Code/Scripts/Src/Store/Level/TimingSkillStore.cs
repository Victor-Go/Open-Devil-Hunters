using System.Collections.Generic;
using Code.Scripts.Src;
using Code.Scripts.Src.Skill;

namespace Code.Scripts.Src.Store.Level
{
  public class PlayerTimingSkills
  {
    public List<TimingSkillContainer> Skills { get; set; } = new();
  }

  public class TimingSkillState : IState
  {
    public int NumberOfPlayers { get; set; }
    public PlayerTimingSkills[] PlayerSkills { get; set; }
  }

  public class TimingSkillActionData : IActionData
  {
    public int NumberOfPlayers { get; set; }
    public int PlayerNumber { get; set; }
    public int SkillIndex { get; set; }
    public string SkillId { get; set; }
    public float TimeToLaunch { get; set; }
    public TimingSkillContainer TimingSkillContainer { get; set; }
    public float DeltaTime { get; set; }
  }

  public class TimingSkillStore : IStore
  {
    public IState InitialState => new TimingSkillState();

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      var timingSkillState = (TimingSkillState)state;
      var actionData = (TimingSkillActionData)data;

      var playerNumber = actionData.PlayerNumber;

      switch (action)
      {
        case StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS:
          timingSkillState.NumberOfPlayers = actionData.NumberOfPlayers;
          timingSkillState.PlayerSkills = new PlayerTimingSkills[actionData.NumberOfPlayers];
          for (int i = 0; i < timingSkillState.PlayerSkills.Length; i++)
          {
            timingSkillState.PlayerSkills[i] = new PlayerTimingSkills();
          }

          return timingSkillState;
        case StoreActions.TimingSkillStore_SET_TIMING_SKILL:
          var skillContainer = actionData.TimingSkillContainer;
          timingSkillState.PlayerSkills[playerNumber].Skills.Add(skillContainer);
          return timingSkillState;
        case StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL:
          timingSkillState.PlayerSkills[playerNumber].Skills
            .RemoveAll(container => container.SkillId == actionData.SkillId);
          return timingSkillState;
        case StoreActions.TimingSkillStore_UPDATE_DELTA_TIME:
          foreach (var timingSkill in timingSkillState.PlayerSkills)
          {
            foreach (var timingSkillContainer in timingSkill.Skills)
            {
              timingSkillContainer.TimeToLaunch -= actionData.DeltaTime;
            }
          }

          return timingSkillState;
        case StoreActions.TimingSkillStore_SET_TIME_TO_LAUNCH:
          var skillIndex = actionData.SkillIndex;
          var timeToLaunch = actionData.TimeToLaunch;
          timingSkillState.PlayerSkills[playerNumber].Skills[skillIndex].TimeToLaunch = timeToLaunch;
          return timingSkillState;
        case StoreActions.TimingSkillStore_RESET:
          timingSkillState.PlayerSkills[playerNumber]
            .Skills
            .Clear();
          return timingSkillState;
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}