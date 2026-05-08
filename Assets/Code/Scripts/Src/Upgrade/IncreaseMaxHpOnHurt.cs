using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Store.Player;

namespace Code.Scripts.Src.Upgrade
{
  public class IncreaseMaxHpOnHurt : IPlayerHurtInterceptor
  {
    private StoreManager storeManager;

    private float _maxIncrementPercentage;
    private float _increasePercentagePerHurt;
    private float initialMaxHp;
    private float increased;

    public IncreaseMaxHpOnHurt(float currentMaxHp, float increasePercentagePerHurt, float maxIncrementPercentage)
    {
      storeManager = StoreManager.Instance;

      initialMaxHp = currentMaxHp;
      _increasePercentagePerHurt = increasePercentagePerHurt;
      _maxIncrementPercentage = maxIncrementPercentage;
    }

    public PlayerHurtContext OnPlayerHurt(PlayerHurtContext playerHurtContext)
    {
      if (_increasePercentagePerHurt < _maxIncrementPercentage)
      {
        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_AUGMENT_MAXIMUM_HP, new PlayerActionData()
        {
          PlayerNumber = playerHurtContext.PlayerNumber,
          AugmentMaximumHp = initialMaxHp * _increasePercentagePerHurt,
          DoNotAugmentCurrentHp = true,
        });
        _increasePercentagePerHurt += _increasePercentagePerHurt;
      }

      return playerHurtContext;
    }
  }
}