using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy
{
  public class GeneralEnemy : PoolableEnemy
  {
    protected EnemyHurt enemyHurt;

    protected float colliderRadius;
    protected Vector2 direction;

    protected float directionUpdateInterval { get; } = 0.25f;
    protected float timeToUpdateDirection;

    protected float detectPlayerDistance { get; set; } = 1;

    protected override void Awake()
    {
      base.Awake();
      timeToUpdateDirection = Random.Range(0, directionUpdateInterval);
      animatorTransform = transform.Find("Animator");
      colliderRadius = GetComponent<CircleCollider2D>().radius;
      enemyHurt = GetComponentInChildren<EnemyHurt>();
    }

    private readonly RaycastHit2D[] raycastHitsCache = new RaycastHit2D[16];
    private static readonly int ObstaclesLayerMask = LayerMask.GetMask("CollidableEnvironment", "Enemy");

    protected bool HasObstacles(Vector2 position, Vector2 direction)
    {
      var count = Physics2D.RaycastNonAlloc(position, direction, raycastHitsCache, 1f, ObstaclesLayerMask);
      for (var i = 0; i < count; i++)
      {
        if (!ReferenceEquals(raycastHitsCache[i].collider.gameObject, gameObject))
        {
          return true;
        }
      }
      return false;
    }

    protected bool RaycastSector(Vector2 direction)
    {
      const int ray = 18;
      const float angle = 360f / ray;
      for (var i = 0; i < ray; i++)
      {
        Vector2 pos = transform.position +
                      Quaternion.AngleAxis(i / 2f * angle + 270, Vector3.forward) * direction * colliderRadius;
        if (HasObstacles(pos, direction))
        {
          return true;
        }
      }

      return false;
    }

    protected virtual void UpdateDirection()
    {
      var candidateDirection = (playerPositions[selectedPlayerNumber] - (Vector2)transform.position).normalized;

      var hasObstacles = RaycastSector(candidateDirection);
      if (hasObstacles)
      {
        float offset = 1;

        while (offset <= 170)
        {
          var cw = Quaternion.AngleAxis(offset, Vector3.forward) * candidateDirection;
          var cwHasObstacles = RaycastSector(cw);
          if (!cwHasObstacles)
          {
            direction = Quaternion.AngleAxis(offset + 30, Vector3.forward) * candidateDirection;
            return;
          }

          var ccw = Quaternion.AngleAxis(-offset, Vector3.forward) * candidateDirection;
          var ccwHasObstacles = RaycastSector(ccw);
          if (!ccwHasObstacles)
          {
            direction = Quaternion.AngleAxis(-offset - 30, Vector3.forward) * candidateDirection;
            return;
          }

          offset += 10;
        }
      }

      direction = candidateDirection;
    }

    protected override void Update()
    {
      base.Update();

      if (!paused)
      {
        timeToUpdateDirection -= Time.deltaTime;
        if (timeToUpdateDirection <= 0)
        {
          UpdateDirection();
          timeToUpdateDirection = directionUpdateInterval;
        }
      }
    }

    protected override void FixedUpdate()
    {
      if (paused)
      {
        rb.velocity = Vector2.zero;
        return;
      }

      if (stopHandlingMovement)
      {
        return;
      }

      var updatedPosition = rb.position + direction * (updatedEnemyConfigurations.Speed * Time.fixedDeltaTime);
      rb.MovePosition(updatedPosition);

      SetAnimatorDirection(direction);

      detectPlayerDistance -= Time.fixedDeltaTime;
      if (detectPlayerDistance <= 0)
      {
        detectPlayerDistance = 1;

        var playerPositionState = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore);
        var alive = playerPositionState.PlayerAlive;
        var positions = playerPositionState.PlayerPositions;

        var distance = GeneralConfigurations.SpawnDistance + 2;
        var sqrDistance = distance * distance;
        var atLeastNearbyOnePlayer = false;
        for (var i = 0; i < alive.Length; i++)
        {
          if (alive[i] && ((Vector2)transform.position - positions[i]).sqrMagnitude < sqrDistance)
          {
            atLeastNearbyOnePlayer = true;
            break;
          }
        }

        if (!atLeastNearbyOnePlayer)
        {
          OnEnemyRecycle();
          objectPool.Recycle(ObjectName, gameObject);
        }
      }
    }
  }
}