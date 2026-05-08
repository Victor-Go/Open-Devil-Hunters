namespace Code.Scripts.Src.Store.Level
{
    public struct HighPrecisionLevelTimeState : IState
    {
        public float LevelTimePassed { get; set; }
        public float LastDeltaTime { get; set; }
    }

    public struct HighPrecisionLevelTimeData : IActionData
    {
        public float LevelTimePassed { get; set; }
        public float DeltaTime { get; set; }
    }

    public class HighPrecisionGameLevelStore : IStore
    {
        public IState InitialState => new HighPrecisionLevelTimeState();

        public IState Commit(IState state, StoreActions action, IActionData data)
        {
            var highClockData = (HighPrecisionLevelTimeData)data;
            var highClockState = (HighPrecisionLevelTimeState)state;

            switch (action)
            {
                case StoreActions.HighPrecisionLevelTimeStore_SET_LEVEL_TIME_PASSED:
                    highClockState.LevelTimePassed = highClockData.LevelTimePassed;
                    highClockState.LastDeltaTime = highClockData.DeltaTime;
                    return highClockState;
                default:
                    throw new InvalidStoreActionException(action);
            }
        }
    }
}
