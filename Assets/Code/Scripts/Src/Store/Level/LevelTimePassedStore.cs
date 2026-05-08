using Code.Scripts.Src;

namespace Code.Scripts.Src.Store.Level
{
  public struct LevelTimePassedState : IState
  {
    public int LevelTimePassed { get; set; }
  }

  public struct LevelTimePassedData : IActionData
  {
    public int LevelTimePassed { get; set; }
  }

  public class LevelTimePassedStore : IStore
  {
    public IState InitialState => new LevelTimePassedState();

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      LevelTimePassedState levelTimePassedState = (LevelTimePassedState)state;
      LevelTimePassedData levelTimePassedStoreData = (LevelTimePassedData)data;
      switch (action)
      {
        case StoreActions.LevelTimePassedStore_SET_LEVEL_TIME_PASSED:
          levelTimePassedState.LevelTimePassed = levelTimePassedStoreData.LevelTimePassed;
          return levelTimePassedState;
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}