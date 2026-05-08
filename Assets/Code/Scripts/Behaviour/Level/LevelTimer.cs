using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Store.Level;
using UnityEngine;

namespace Code.Scripts.Behaviour.Level
{
    public class LevelTimer : MonoBehaviour, IStoreChangedHandler
    {
        private StoreManager storeManager;
        private bool paused;
        private float accurateTimePassed;

        private void Awake()
        {
            storeManager = StoreManager.Instance;
        }

        private void Start()
        {
            storeManager.Subscribe(StoreNames.GameStateStore, this);
        }

        public void OnStoreChanged(StoreNames storeName, IState state)
        {
            switch (storeName)
            {
                case StoreNames.GameStateStore:
                    GameStates gameState = ((GameState)state).CurrentGameState;
                    paused = !LevelUtils.PlayerCanMove(gameState);
                    break;
                default:
                    throw new InvalidStoreEventException(storeName);
            }
        }

        private void Update()
        {
            if (!paused)
            {
                accurateTimePassed += Time.deltaTime;

                var levelState = storeManager.GetState<LevelTimePassedState>(StoreNames.LevelTimePassedStore);
                int timePassed = levelState.LevelTimePassed;
                int newTime = (int)accurateTimePassed;

                if (newTime > timePassed)   // Reduce function call
                {
                    storeManager.Commit(StoreNames.LevelTimePassedStore, StoreActions.LevelTimePassedStore_SET_LEVEL_TIME_PASSED, new LevelTimePassedData()
                    {
                        LevelTimePassed = newTime,
                    });
                }
            }
        }

        private void OnDestroy()
        {
            storeManager.Unsubscribe(this);
        }
    }
}
