using Code.Scripts.Src;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Behaviour.Level;
using Code.Scripts.Src.Configurations;

namespace Code.Scripts.Src.Store.Level
{
  public class LevelSettingState : IState
  {
    public AimingMode[] AimingModes { get; set; } = new AimingMode[GeneralConfigurations.MaximumPlayers];
    public FiringMode[] FiringModes { get; set; } = new FiringMode[GeneralConfigurations.MaximumPlayers];
  }

  public struct LevelSettingData : IActionData
  {
    public int PlayerNumber { get; set; }
    public AimingMode AimingMode { get; set; }
    public FiringMode FiringMode { get; set; }
  }

  public class LevelSettingStore : IStore
  {
    public IState InitialState
    {
      get
      {
        var levelState = new LevelSettingState();

        for (var i = 0; i < 2; i++)
        {
          levelState.AimingModes[i] = AimingMode.JOYSTICK;
        }

        return levelState;
      }
    }

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      var levelSettingState = (LevelSettingState)state;
      var levelSettingData = (LevelSettingData)data;
      var playerNumber = levelSettingData.PlayerNumber;

      switch (action)
      {
        case StoreActions.LevelSettingStore_SET_AIMING_MODE:
          levelSettingState.AimingModes[playerNumber] = levelSettingData.AimingMode;
          return levelSettingState;
        case StoreActions.LevelSettingStore_SET_FIRING_MODE:
          levelSettingState.FiringModes[playerNumber] = levelSettingData.FiringMode;
          return levelSettingState;
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}