using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Player;
using UnityEngine;

namespace Code.Scripts.Src.Rune
{
  // Not used in current version
  public class EnemyExplosionAfterDeadRune : BaseRune
  {
    private static readonly Collider2D[] overlapResults = new Collider2D[32];

    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager.SkillAndUpgradeControllers[playerNumber].AddEnemyDieAction((context) =>
      {
        int hitCount = Physics2D.OverlapCircleNonAlloc(context.EnemyPosition, 1f, overlapResults);
        for(int i = 0; i < hitCount; i++)
        {
          var collider = overlapResults[i];
          if (TriggerGroup.CanInteract("", LayerMask.LayerToName(1)))
          {
            // FIXME: Hurt directly.
          }
        }

        return context;
      });
    }
  }

  // Not used in current version
  public class IncreaseAttackingVelocityRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      var coolingTime = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore)
        .AttackControlDatas[playerNumber].CoolingCountdown;
      storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_COOLING_COUNTDOWN,
        new AttackControlActionData()
        {
          PlayerNumber = playerNumber,
          CoolingCountdown = coolingTime * (1 - 0.05f * (level + 1))
        });
    }
  }

  // Not used in current version
  public class PoisonNearbyOnEnemyDieRune : BaseRune
  {
    private static readonly Collider2D[] overlapResults = new Collider2D[32];

    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager.SkillAndUpgradeControllers[playerNumber].AddEnemyDieAction((context) =>
      {
        int hitCount = Physics2D.OverlapCircleNonAlloc(context.EnemyPosition, 1f, overlapResults);
        for(int i = 0; i < hitCount; i++)
        {
          var collider = overlapResults[i];
          if (TriggerGroup.CanInteract("", LayerMask.LayerToName(1)))
          {
            // FIXME: Add poisoning effect.
          }
        }

        return context;
      });
    }
  }
}