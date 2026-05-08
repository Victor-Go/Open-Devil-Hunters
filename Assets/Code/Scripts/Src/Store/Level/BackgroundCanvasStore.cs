namespace Code.Scripts.Src.Store.Level
{
    public class BackgroundCanvasState : IState
    {
        public bool Active { get; set; }
    }

    public struct BackgroundCanvasActionData : IActionData
    {
        public bool Active { get; set; }
    }

    public class BackgroundCanvasStore : IStore
    {
        IState IStore.InitialState => new BackgroundCanvasState();

        public IState Commit(IState state, StoreActions action, IActionData data)
        {
            var actionData = (BackgroundCanvasActionData)data;
            var backgroundState = (BackgroundCanvasState)state;

            switch (action)
            {
                case StoreActions.BackgroundCanvas_SET_ACTIVE:
                    backgroundState.Active = actionData.Active;
                    return backgroundState;
                default:
                    throw new InvalidStoreActionException(action);
            }
        }
    }
}
