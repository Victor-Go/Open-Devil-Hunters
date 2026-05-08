using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;

namespace Code.Scripts.Src.Store.Level
{
  public struct ActiveSkillState : IState
  {
    public int NumberOfPlayers { get; set; }
    public bool[] PlayerAlive { get; set; }
    public float[] CoolingCountdowns { get; set; }
    public string[] SkillImageIndicators { get; set; }
    public float BulletFissionPossibility { get; set; }
  }

  public struct ActiveSkillData : IActionData
  {
    public int NumberOfPlayers { get; set; }
    public int PlayerNumber { get; set; }
    public bool PlayerAlive { get; set; }
    public float CoolingCountdown { get; set; }
    public string SkillImageIndicator { get; set; }
    public float BulletFissionPossibility { get; set; }
  }

  public class ActiveSkillStore : IStore
  {
    public IState InitialState => new ActiveSkillState();

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      var activeSkillState = (ActiveSkillState)state;
      var activeSkillData = (ActiveSkillData)data;

      switch (action)
      {
        case StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS:
          var numberOfPlayers = activeSkillData.NumberOfPlayers;
          activeSkillState.NumberOfPlayers = numberOfPlayers;
          activeSkillState.PlayerAlive ??= new int[GeneralConfigurations.MaximumPlayers]
            .Select((_, index) => index < numberOfPlayers).ToArray();
          activeSkillState.CoolingCountdowns ??= new float[numberOfPlayers];
          activeSkillState.SkillImageIndicators ??= new string[numberOfPlayers];
          return activeSkillState;
        case StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE:
          activeSkillState.PlayerAlive[activeSkillData.PlayerNumber] = activeSkillData.PlayerAlive;
          activeSkillState.NumberOfPlayers = activeSkillState.PlayerAlive.Count(p => p);
          return activeSkillState;
        case StoreActions.ActiveSkillStore_SET_COOLING_COUNTDOWN:
          activeSkillState.CoolingCountdowns[activeSkillData.PlayerNumber] = activeSkillData.CoolingCountdown;
          return activeSkillState;
        case StoreActions.ActiveSkillStore_SET_ACTIVE_SKILL_ICON:
          activeSkillState.SkillImageIndicators[activeSkillData.PlayerNumber] = activeSkillData.SkillImageIndicator;
          return activeSkillState;
        case StoreActions.ActiveSkillStore_SET_BULLET_FISSION_POSSIBILITY:
          activeSkillState.BulletFissionPossibility = activeSkillData.BulletFissionPossibility;
          return activeSkillState;
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}