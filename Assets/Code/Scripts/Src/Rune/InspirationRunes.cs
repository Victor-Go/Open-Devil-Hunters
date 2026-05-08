using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Src.Rune
{
  public class IncreasePickUpRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      var playerState = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
      var pickUpRadius = playerState.PlayerDatas[playerNumber].PickUpRadius;
      storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_PICK_UP_RADIUS, new PlayerActionData()
      {
        PlayerNumber = playerNumber,
        PickUpRadius = pickUpRadius * (1 + ((level + 1) * 0.1f))
      });
    }
  }

  public class IncreaseHonorRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      storeManager.Commit(StoreNames.LevelGeneralStore, StoreActions.LevelGeneralStore_SET_SET_HONOR_BONUS,
        new LevelGeneralData
        {
          HonorBonus = 0.1f * (1 + level)
        });
    }
  }

  public class IncreaseExperienceRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .AddObtainExperienceAction(context =>
        {
          context.Experience *= 1.1f + level * 0.07f;
          return context;
        });
    }
  }

  public class TimeStopActiveSkillRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .SetActiveSkill(ActiveSkillPresets.Configurations["TimeStopActiveSkill"],
          new PersistActiveSkillConfigurations
          {
            Countdown = 60,
            Duration = 5 + level * 2,
          });
    }
  }
}