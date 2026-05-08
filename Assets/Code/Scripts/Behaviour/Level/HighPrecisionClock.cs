using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Level
{
    public class HighPrecisionClock : PauseableGameObject
    {
        private float gameTime;

        protected override void HandleGameStateChanged(GameState state)
        {
            GameStates gameState = state.CurrentGameState;
            paused = !LevelUtils.PlayerCanMove(gameState);
            if (animator != null)
            {
                animator.speed = paused ? 0 : 1;
            }
        }

        private void Update()
        {
            if (!paused)
            {
                var dt = Time.deltaTime;
                gameTime += dt;

                storeManager.Commit(StoreNames.HighPrecisionLevelTimeStore, StoreActions.HighPrecisionLevelTimeStore_SET_LEVEL_TIME_PASSED, new HighPrecisionLevelTimeData()
                {
                    LevelTimePassed = gameTime,
                    DeltaTime = dt,
                });
            }
        }
    }
}
