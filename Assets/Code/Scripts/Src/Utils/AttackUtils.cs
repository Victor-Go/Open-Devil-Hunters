using Code.Scripts.Behaviour.Level;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.Level;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Src.Utils
{
  public struct EnemyTuple
  {
    public GameObject Enemy { get; set; }
    public long EnemyUniqueId { get; set; }
  }

  public static class AttackUtils
  {
    private static readonly int EnemyLayerMask = LayerMask.GetMask("Enemy");
    private static readonly List<EnemyTuple> inScreenEnemyTuplesBuffer = new List<EnemyTuple>(128);
    private static readonly List<GameObject> inScreenEnemiesBuffer = new List<GameObject>(128);
    private static readonly List<GameObject> nearestEnemiesBuffer = new List<GameObject>(128);
    private static readonly List<GameObject> overlapEnemiesBuffer = new List<GameObject>(64);
    private static readonly Collider2D[] overlapCollidersBuffer = new Collider2D[64];
    /**
     * skillHurtType: The hurt type of incoming skill
     * defendType: The defend type of current object
     * criticalHurtType: The critical hurt type of current object
     * hurtPoint: Original hurt point
     */
    public static float GetHurtPointAfterDefenseAndCritical(
      HurtTypes skillHurtType,
      List<HurtTypes> defendTypes,
      List<HurtTypes> criticalHurtTypes,
      float hurtPoint
    )
    {
      if (defendTypes != null && defendTypes.Any(t => t == skillHurtType))
      {
        hurtPoint *= 1 - GeneralConfigurations.DefendTypeDecrement;
      }

      if (criticalHurtTypes != null && criticalHurtTypes.Any(t => t == skillHurtType))
      {
        hurtPoint *= 1 + GeneralConfigurations.CriticalHurtTypeIncrement;
      }

      return hurtPoint;
    }

    public static Vector2 GetTargetPosition(int playerNumber, StoreManager storeManager = null)
    {
      if (storeManager == null)
      {
        storeManager = StoreManager.Instance;
      }

      var targetEnemyState = storeManager.GetState<TargetEnemyState>(StoreNames.TargetEnemyStore);
      var aimingMode = storeManager.GetState<LevelSettingState>(StoreNames.LevelSettingStore).AimingModes[playerNumber];
      switch (aimingMode)
      {
        case AimingMode.AUTO_AIMING:
          var targetEnemy = targetEnemyState.TargetEnemies[playerNumber];
          if (targetEnemy)
          {
            return targetEnemy.transform.position;
          }
          else
          {
            var playerPositions = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore)
              .PlayerPositions;

            return playerPositions[playerNumber] + GeneralConfigurations.DefaultAutoAimingPosition;
          }
        case AimingMode.JOYSTICK:
          return targetEnemyState.WorldSpaceControllerAimPositions[playerNumber];
        case AimingMode.MOUSE:
          return GeneralUtils.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        default:
          return Vector2.zero;
      }
    }

    public static List<EnemyTuple> GetInScreenEnemyTuples(StoreManager storeManager = null)
    {
      storeManager ??= StoreManager.Instance;

      var levelState = storeManager.GetState<LevelState>(StoreNames.LevelStore);
      var mainCamera = GeneralUtils.MainCamera;
      Vector2 topRightCornerWorldPosition = (Vector2)mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
      Vector2 bottomLeftCornerWorldPosition = (Vector2)mainCamera.ViewportToWorldPoint(new Vector2(0, 0));

      inScreenEnemyTuplesBuffer.Clear();
      for (int i = 0; i < levelState.Enemies.Count; i++)
      {
        var enemy = levelState.Enemies[i];
        if (
          enemy != null &&
          enemy.activeSelf &&
          bottomLeftCornerWorldPosition.x <= enemy.transform.position.x &&
          enemy.transform.position.x < topRightCornerWorldPosition.x &&
          bottomLeftCornerWorldPosition.y <= enemy.transform.position.y &&
          enemy.transform.position.y <= topRightCornerWorldPosition.y
        )
        {
          inScreenEnemyTuplesBuffer.Add(new EnemyTuple
          {
            Enemy = enemy,
            EnemyUniqueId = levelState.EnemyUniqueIds[i]
          });
        }
      }

      return inScreenEnemyTuplesBuffer;
    }

    public static List<GameObject> GetInScreenEnemies(StoreManager storeManager = null)
    {
      storeManager ??= StoreManager.Instance;

      var levelState = storeManager.GetState<LevelState>(StoreNames.LevelStore);
      var mainCamera = GeneralUtils.MainCamera;
      Vector2 topRightCornerWorldPosition = (Vector2)mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
      Vector2 bottomLeftCornerWorldPosition = (Vector2)mainCamera.ViewportToWorldPoint(new Vector2(0, 0));

      inScreenEnemiesBuffer.Clear();
      for (int i = 0; i < levelState.Enemies.Count; i++)
      {
        var enemy = levelState.Enemies[i];
        if (
          enemy != null &&
          enemy.activeSelf &&
          bottomLeftCornerWorldPosition.x <= enemy.transform.position.x &&
          enemy.transform.position.x < topRightCornerWorldPosition.x &&
          bottomLeftCornerWorldPosition.y <= enemy.transform.position.y &&
          enemy.transform.position.y <= topRightCornerWorldPosition.y
        )
        {
          inScreenEnemiesBuffer.Add(enemy);
        }
      }

      return inScreenEnemiesBuffer;
    }

    public static List<GameObject> GetNearestEnemies(int playerNumber, int quantity, StoreManager storeManager = null)
    {
      storeManager ??= StoreManager.Instance;

      var candidateEnemies = GetInScreenEnemyTuples(storeManager);
      if (candidateEnemies.Count == 0)
      {
        var levelState = storeManager.GetState<LevelState>(StoreNames.LevelStore);
        for (int i = 0; i < levelState.Enemies.Count; i++)
        {
          candidateEnemies.Add(new EnemyTuple()
          {
            Enemy = levelState.Enemies[i],
            EnemyUniqueId = levelState.EnemyUniqueIds[i]
          });
        }
      }

      var playerPositionState = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore);
      var playerPosition = playerPositionState.PlayerPositions[playerNumber];

      nearestEnemiesBuffer.Clear();

      if (playerPositionState.NumberOfPlayers > 1)
      {
        var targetEnemyState = storeManager.GetState<TargetEnemyState>(StoreNames.TargetEnemyStore);
        int otherPlayerNumber = 1 ^ playerNumber;
        long otherPlayerTargetedEnemyUniqueId = targetEnemyState.TargetEnemyUniqueIds[otherPlayerNumber];

        for (int i = 0; i < candidateEnemies.Count; i++)
        {
            if (candidateEnemies[i].EnemyUniqueId != otherPlayerTargetedEnemyUniqueId)
            {
                nearestEnemiesBuffer.Add(candidateEnemies[i].Enemy);
            }
        }
      }
      else
      {
          for (int i = 0; i < candidateEnemies.Count; i++)
          {
              nearestEnemiesBuffer.Add(candidateEnemies[i].Enemy);
          }
      }

      nearestEnemiesBuffer.Sort((a, b) => {
          float distA = ((Vector2)a.transform.position - playerPosition).sqrMagnitude;
          float distB = ((Vector2)b.transform.position - playerPosition).sqrMagnitude;
          return distA.CompareTo(distB);
      });

      if (nearestEnemiesBuffer.Count > quantity)
      {
          nearestEnemiesBuffer.RemoveRange(quantity, nearestEnemiesBuffer.Count - quantity);
      }

      return nearestEnemiesBuffer;
    }

    public static GameObject GetNearestEnemy(int playerNumber, StoreManager storeManager = null)
    {
      storeManager ??= StoreManager.Instance;

      var enemies = GetNearestEnemies(playerNumber, 1, storeManager);
      return enemies.Count > 0 ? enemies[0] : null;
    }

    public static List<GameObject> GetNearestEnemiesAtPosition(Vector2 position, float range)
    {
      int count = Physics2D.OverlapCircleNonAlloc(position, range, overlapCollidersBuffer, EnemyLayerMask);
      overlapEnemiesBuffer.Clear();
      for (int i = 0; i < count; i++)
      {
        overlapEnemiesBuffer.Add(overlapCollidersBuffer[i].gameObject);
      }

      return overlapEnemiesBuffer;
    }

    public static GameObject GetNearestEnemyAtPosition(Vector2 position, float range)
    {
      int count = Physics2D.OverlapCircleNonAlloc(position, range, overlapCollidersBuffer, EnemyLayerMask);

      float nearest = float.MaxValue;

      if (count == 0)
      {
        return null;
      }

      GameObject enemyGameObject = overlapCollidersBuffer[0].gameObject;
      for (int i = 0; i < count; i++)
      {
        var enemy = overlapCollidersBuffer[i];
        var distance = (position - (Vector2)enemy.transform.position).sqrMagnitude;
        if (distance < nearest)
        {
          nearest = distance;
          enemyGameObject = enemy.gameObject;
        }
      }

      return enemyGameObject;
    }
  }
}