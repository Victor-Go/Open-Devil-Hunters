using Code.Scripts.Src.Types;
using Code.Scripts.Src.Store.Player;

namespace Code.Scripts.Src.Upgrade
{
  public class KillEnemyToIncreaseSpeed : IEnemyDieInterceptor, IPlayerHurtInterceptor
  {
    private readonly StoreManager storeManager;

    private readonly int playerNumber;
    private readonly float initialSpeed;
    private readonly float initialASpeed;
    private readonly float increasePercentage;
    private readonly float maxPercentage;
    private float increasedSpeed;
    private float increasedASpeed;
    private int killedEnemy;
    private readonly int killRequirement;

    public KillEnemyToIncreaseSpeed(int playerNumber, int killRequirement, float increasePercentage,
      float maxPercentage)
    {
      storeManager = StoreManager.Instance;

      var playerData = storeManager.GetState<PlayerState>(StoreNames.PlayerStore).PlayerDatas[playerNumber];
      initialSpeed = playerData.MovingSpeed;
      initialASpeed = playerData.AttackingMovingSpeed;

      this.killRequirement = killRequirement;
      this.playerNumber = playerNumber;
      this.increasePercentage = increasePercentage;
      this.maxPercentage = maxPercentage;
    }

    public EnemyDieContext OnEnemyDie(EnemyDieContext enemyDieContext)
    {
      killedEnemy++;

      if (enemyDieContext.PlayerNumber == playerNumber && killedEnemy >= killRequirement &&
          increasedSpeed / initialSpeed < maxPercentage && increasedASpeed / initialASpeed < maxPercentage)
      {
        killedEnemy = 0;

        var playerData = storeManager.GetState<PlayerState>(StoreNames.PlayerStore).PlayerDatas[playerNumber];
        var currentSpeed = playerData.MovingSpeed;
        var currentASpeed = playerData.AttackingMovingSpeed;

        var incSpeed = initialSpeed * increasePercentage;
        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_MOVING_SPEED, new PlayerActionData()
        {
          PlayerNumber = playerNumber,
          MovingSpeed = currentSpeed + incSpeed
        });

        var incASpeed = initialASpeed * increasePercentage;
        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_ATTACKING_MOVING_SPEED,
          new PlayerActionData()
          {
            PlayerNumber = playerNumber,
            AttackingMovingSpeed = currentASpeed + incASpeed
          });

        increasedSpeed += incSpeed;
        increasedASpeed += incASpeed;
      }

      return enemyDieContext;
    }

    public PlayerHurtContext OnPlayerHurt(PlayerHurtContext playerHurtContext)
    {
      if (playerNumber == playerHurtContext.PlayerNumber)
      {
        var playerData = storeManager.GetState<PlayerState>(StoreNames.PlayerStore).PlayerDatas[playerNumber];
        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_MOVING_SPEED, new PlayerActionData()
        {
          PlayerNumber = playerNumber,
          MovingSpeed = playerData.MovingSpeed - increasedSpeed,
        });

        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_ATTACKING_MOVING_SPEED,
          new PlayerActionData()
          {
            PlayerNumber = playerNumber,
            AttackingMovingSpeed = playerData.AttackingMovingSpeed - increasedASpeed,
          });

        increasedSpeed = 0;
        increasedASpeed = 0;
        killedEnemy = 0;
      }

      return playerHurtContext;
    }
  }
}