using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using UnityEngine;

namespace Code.Scripts.Src.Store.Level
{
  public class LevelState : IState
  {
    public List<GameObject> Enemies { get; set; } = new();
    public List<GameObject> OwnerlessExpPoints { get; set; } = new();

    public List<long> EnemyUniqueIds { get; set; } =
      new(); // EnemyUniqueIds corresponds to Enemies with index, so NEVER mess up their indexes.
  }

  public struct LevelData : IActionData
  {
    public List<GameObject> Enemies { get; set; }
    public GameObject Enemy { get; set; }
    public GameObject ExpPoint { get; set; }
  }

  public class LevelStore : IStore
  {
    public IState InitialState => new LevelState();

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      LevelState levelState = (LevelState)state;
      LevelData levelStoreData = (LevelData)data;
      switch (action)
      {
        case StoreActions.LevelStore_SET_ENEMIES:
          levelState.Enemies = levelStoreData.Enemies;
          return levelState;
        case StoreActions.LevelStore_ADD_ENEMY:
          levelState.Enemies.Add(levelStoreData.Enemy);
          levelState.EnemyUniqueIds.Add(levelStoreData.Enemy.GetComponent<Types.Enemy>().EnemyUniqueId);
          return levelState;
        case StoreActions.LevelStore_ADD_ENEMIES:
          levelState.Enemies = levelState.Enemies.Concat(levelStoreData.Enemies).ToList();
          levelState.EnemyUniqueIds = levelState.EnemyUniqueIds
            .Concat(levelStoreData.Enemies.Select(e => e.GetComponent<Types.Enemy>().EnemyUniqueId))
            .ToList();
          return levelState;
        case StoreActions.LevelStore_REMOVE_ENEMY:
          long enemyUniqueId = levelStoreData.Enemy.GetComponent<Types.Enemy>().EnemyUniqueId;
          levelState.Enemies.RemoveAll(e => ReferenceEquals(e, levelStoreData.Enemy));
          levelState.EnemyUniqueIds.RemoveAll(id => id == enemyUniqueId);
          return levelState;
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}