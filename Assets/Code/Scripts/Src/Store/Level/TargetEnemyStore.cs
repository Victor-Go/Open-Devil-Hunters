using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Types;
using System.Linq;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Src.Store.Level
{
    public struct TargetEnemyState : IState
    {
        public int NumberOfPlayers { get; set; }
        public long[] TargetEnemyUniqueIds { get; set; }
        public bool[] PlayerAlive { get; set; }
        public GameObject[] TargetEnemies { get; set; }
        public Vector2[] WorldSpaceControllerAimPositions { get; set; }
    }

    public struct TargetEnemyData : IActionData
    {
        public int NumberOfPlayers { get; set; }
        public int PlayerNumber { get; set; }
        public bool PlayerAlive { get; set; }
        public GameObject TargetEnemy { get; set; }
        public Vector2 WorldSpaceControllerAimPosition { get; set; }
    }

    public class TargetEnemyStore : IStore
    {
        public IState InitialState => new TargetEnemyState();

        public IState Commit(IState state, StoreActions action, IActionData data)
        {
            TargetEnemyState targetEnemyState = (TargetEnemyState)state;
            TargetEnemyData targetEnemyData = (TargetEnemyData)data;
            switch (action)
            {
                case StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS:
                    targetEnemyState.NumberOfPlayers = targetEnemyData.NumberOfPlayers;
                    targetEnemyState.PlayerAlive ??= new int[GeneralConfigurations.MaximumPlayers]
                        .Select((_, index) => index < targetEnemyData.NumberOfPlayers).ToArray();
                    targetEnemyState.TargetEnemyUniqueIds ??= new long[targetEnemyData.NumberOfPlayers];
                    targetEnemyState.TargetEnemies ??= new GameObject[targetEnemyData.NumberOfPlayers];
                    targetEnemyState.WorldSpaceControllerAimPositions ??= new Vector2[targetEnemyData.NumberOfPlayers];
                    return targetEnemyState;
                case StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE:
                    targetEnemyState.PlayerAlive[targetEnemyData.PlayerNumber] = targetEnemyData.PlayerAlive;
                    targetEnemyState.NumberOfPlayers = targetEnemyState.PlayerAlive.Count(p => p);
                    return targetEnemyState;
                case StoreActions.TargetEnemyStore_SET_TARGET_ENEMY:
                    var targetEnemy = targetEnemyData.TargetEnemy;
                    targetEnemyState.TargetEnemies[targetEnemyData.PlayerNumber] = targetEnemy;
                    targetEnemyState.TargetEnemyUniqueIds[targetEnemyData.PlayerNumber] = targetEnemy.GetComponent<Enemy>().EnemyUniqueId;
                    return targetEnemyState;
                case StoreActions.TargetEnemyStore_SET_CONTROLLER_AIM_POSITION:
                    targetEnemyState.WorldSpaceControllerAimPositions[targetEnemyData.PlayerNumber] = targetEnemyData.WorldSpaceControllerAimPosition;
                    return targetEnemyState;
                default:
                    throw new InvalidStoreActionException(action);
            }
        }
    }
}
