using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Animation
{
  public class ResurrectionAnimation : PauseableAnimation
  {
    private GameStates gameState;
    private Transform playerTransform;

    private static readonly int EnemyLayerMask = LayerMask.GetMask("Enemy");
    private static readonly Collider2D[] overlapCollidersBuffer = new Collider2D[32];

    public void SetPlayerTransform(Transform transform)
    {
      playerTransform = transform;
    }

    protected override void Start()
    {
      base.Start();
      var gameState = storeManager.GetState<GameState>(StoreNames.GameStateStore).CurrentGameState;
      this.gameState = gameState;

      storeManager.Commit(StoreNames.GameStateStore, StoreActions.GameStateStore_SET_GAME_STATE, new GameStateData()
      {
        GameState = GameStates.ONLY_ANIMATION_CAN_PLAY
      });
    }

    private void PushAwayEnemies(Vector2 playerPosition)
    {
      // Animation
      GameObject pushAway = ObjectPool.Instance.GetObject("Status/PushAway");
      pushAway.transform.SetParent(null);
      pushAway.transform.position = playerPosition;

      // Add force
      const int range = 3;
      const int sqrRange = range * range;
      const int  force = 3;

      var count = Physics2D.OverlapCircleNonAlloc(playerPosition, range, overlapCollidersBuffer, EnemyLayerMask);
      for (int i = 0; i < count; i++)
      {
        var enemy = overlapCollidersBuffer[i];
        var sqrMag = ((Vector2)enemy.transform.position - playerPosition).sqrMagnitude;
        enemy.GetComponent<Enemy>().AddRepelForce(force * (1 - sqrMag / sqrRange),
          (Vector2)enemy.transform.position - playerPosition);
      }
    }

    public void OnResurrectionAnimationFinished()
    {
      storeManager.Commit(StoreNames.GameStateStore, StoreActions.GameStateStore_SET_GAME_STATE, new GameStateData()
      {
        GameState = gameState
      });

      var playerPosition = playerTransform.position;

      PushAwayEnemies(playerPosition);
      Destroy(gameObject);
    }

    protected override void HandleGameStateChanged(GameState state)
    {
      GameStates gameState = state.CurrentGameState;
      paused = !LevelUtils.PlayerAnimationCanPlay(gameState);
      if (animator != null)
      {
        animator.speed = paused ? 0 : 1;
      }
    }
  }
}