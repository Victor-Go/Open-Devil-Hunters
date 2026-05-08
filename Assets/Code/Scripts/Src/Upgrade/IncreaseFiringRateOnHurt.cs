using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Store.Player;

namespace Code.Scripts.Src.Upgrade
{
  public class IncreaseFiringRateOnHurt : IPlayerHurtInterceptor
  {
    private readonly Scheduling scheduling;
    private readonly StoreManager storeManager;

    private readonly int playerNumber;
    private readonly float percentage;
    private readonly float resetTimeout;

    private float coolingTimeDecrement;
    private string resetScheduleId;

    public IncreaseFiringRateOnHurt(int playerNumber, float percentage, float resetTimeout)
    {
      this.playerNumber = playerNumber;
      this.percentage = percentage;
      this.resetTimeout = resetTimeout;

      scheduling = Scheduling.Instance;
      storeManager = StoreManager.Instance;
    }

    public PlayerHurtContext OnPlayerHurt(PlayerHurtContext playerHurtContext)
    {
      if (playerHurtContext.PlayerNumber == playerNumber)
      {
        if (coolingTimeDecrement == 0)
        {
          var attackControl = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore)
            .AttackControlDatas[playerNumber];
          var coolingTime = attackControl.AttackControlConfigurations.AttackCoolingTime;

          coolingTimeDecrement = coolingTime - coolingTime / (1 + percentage);

          storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_COOLING_TIME,
            new AttackControlActionData()
            {
              PlayerNumber = playerNumber,
              CoolingTime = coolingTime - coolingTimeDecrement,
            });
        }

        scheduling.ClearSchedule(resetScheduleId);
        resetScheduleId = scheduling.SetTimeout(ResetFiringSpeed, resetTimeout);
      }

      return playerHurtContext;
    }

    private void ResetFiringSpeed()
    {
      var attackControl = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore)
        .AttackControlDatas[playerNumber];
      var coolingTime = attackControl.AttackControlConfigurations.AttackCoolingTime;
      storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_COOLING_TIME,
        new AttackControlActionData()
        {
          PlayerNumber = playerNumber,
          CoolingTime = coolingTime + coolingTimeDecrement,
        });

      coolingTimeDecrement = 0;
    }

    ~IncreaseFiringRateOnHurt()
    {
      scheduling.ClearSchedule(resetScheduleId);
    }
  }
}