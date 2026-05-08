using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src.I18n;

namespace Code.Scripts.Src.Store.Game
{
    [Serializable]
    public class GameSettingState : IState
    {
        public Languages Language { get; set; }
        public List<string> ShownNotificationIds { get; set; } = new();
    }

    public struct GameSettingData : IActionData
    {
        public GameSettingState SavedState { get; set; }
        public Languages Language { get; set; }
        public string NotificationId { get; set; }
    }

    public class GameSettingStore : IStore
    {
        public IState InitialState =>
                 new GameSettingState() { Language = Languages.auto };


        public IState Commit(IState state, StoreActions action, IActionData data)
        {
            GameSettingData gameSettingData = (GameSettingData)data;
            GameSettingState gameSettingState = (GameSettingState)state;

            switch (action)
            {
                case StoreActions.GameSettingStore_LOAD_FROM_SAVE:
                    gameSettingState = gameSettingData.SavedState;
                    return gameSettingState;
                case StoreActions.GameSettingStore_SET_LOCALE:
                    gameSettingState.Language = gameSettingData.Language;
                    return gameSettingState;
                case StoreActions.GameSettingStore_ADD_SHOWN_NOTIFICATION:
                    var notificationId = gameSettingData.NotificationId;
                    if (!gameSettingState.ShownNotificationIds.Contains(notificationId))
                    {
                        gameSettingState.ShownNotificationIds.Add(notificationId);
                    }
                    return gameSettingState;
                default:
                    throw new InvalidStoreActionException(action);
            }
        }
    }
}
