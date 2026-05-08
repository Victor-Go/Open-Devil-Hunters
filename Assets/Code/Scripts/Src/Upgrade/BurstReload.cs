using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Store.Player;

namespace Code.Scripts.Src.Upgrade
{
  public class BurstReloadOnHurt : IPlayerHurtInterceptor
  {
    private readonly Scheduling scheduling;
    private readonly StoreManager storeManager;

    private readonly int playerNumber;
    private readonly float incrementPercentage;
    private readonly float resetTimeout;

    private float reloadTimeDecrement;
    private string resetScheduleId;

    public BurstReloadOnHurt(int playerNumber, float incrementPercentage, float resetTimeout)
    {
      this.playerNumber = playerNumber;
      this.incrementPercentage = incrementPercentage;
      this.resetTimeout = resetTimeout;

      scheduling = Scheduling.Instance;
      storeManager = StoreManager.Instance;
    }

    public PlayerHurtContext OnPlayerHurt(PlayerHurtContext playerHurtContext)
    {
      if (playerHurtContext.PlayerNumber == playerNumber)
      {
        if (reloadTimeDecrement != 0)
        {
          var attackControl = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore)
            .AttackControlDatas[playerNumber];
          var reloadTime = attackControl.AttackControlConfigurations.ReloadTime;

          reloadTimeDecrement = reloadTime * incrementPercentage;

          storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_COOLING,
            new AttackControlActionData()
            {
              PlayerNumber = playerNumber,
              CoolingTime = reloadTime * (1 - incrementPercentage),
            });
        }

        scheduling.ClearSchedule(resetScheduleId);
        resetScheduleId = scheduling.SetTimeout(ResetReloadTime, resetTimeout);
      }

      return playerHurtContext;
    }

    private void ResetReloadTime()
    {
      var attackControl = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore)
        .AttackControlDatas[playerNumber];
      var reloadTime = attackControl.AttackControlConfigurations.ReloadTime;

      storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_COOLING,
        new AttackControlActionData()
        {
          PlayerNumber = playerNumber,
          CoolingTime = reloadTime + reloadTimeDecrement,
        });

      reloadTimeDecrement = 0;
    }

    ~BurstReloadOnHurt()
    {
      scheduling.ClearSchedule(resetScheduleId);
    }
  }
}