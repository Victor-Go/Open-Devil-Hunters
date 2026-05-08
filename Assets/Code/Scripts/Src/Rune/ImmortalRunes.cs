using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Src.Rune
{
  public class IncreaseImmortalTimeRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      var immortalSeconds = storeManager.GetState<PlayerState>(StoreNames.PlayerStore)
        .PlayerDatas[playerNumber]
        .ImmortalSecondAfterHurt;
      immortalSeconds += level * 0.5f;
      storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_IMMORTAL_SECONDS, new PlayerActionData()
      {
        PlayerNumber = playerNumber,
        ImmortalSecondAfterHurt = immortalSeconds
      });
    }
  }

  public class ReduceHurtRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .AddPlayerHurtAction((context) =>
        {
          context.HitPoint *= 1 - level * 0.05f;
          return context;
        });
    }
  }

  public class IncreaseMaximumHpRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      var maximumHp = storeManager.GetState<PlayerState>(StoreNames.PlayerStore)
        .PlayerDatas[playerNumber]
        .MaximumHp;
      storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_MAXIMUM_HP, new PlayerActionData()
      {
        PlayerNumber = playerNumber,
        MaximumHp = maximumHp * (1 + 0.1f * (level + 1))
      });
    }
  }

  public class AddHpRecoveryRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .AddTimingAction(1, () =>
        {
          var playerState = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
          var maxHp = playerState.PlayerDatas[playerNumber].MaximumHp;

          storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_ADD_CURRENT_HP, new PlayerActionData()
          {
            PlayerNumber = playerNumber,
            AddCurrentHp =
              maxHp * (0.002f + 0.002f *
                level) // If this value is updated, don't forget to update in FightPreparation and PauseUI.
          });
        });
    }
  }

  public class OneTimeResurrectionRune : BaseRune
  {
    private bool activated;
    private readonly ResourceManager resourceManager = ResourceManager.Instance;

    public override void Apply(int playerNumber)
    {
      storeManager.Commit(StoreNames.LevelGeneralStore, StoreActions.LevelGeneralStore_ADD_RESURRECTION,
        new LevelGeneralData()
        {
          PlayerNumber = playerNumber,
          AddResurrection = 1,
          ResurrectionPercentage = 0.25f + level * 0.15f,
        });
    }
  }

  public class HolyShieldActiveSkillRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .SetActiveSkill(ActiveSkillPresets.Configurations["HolyShieldActiveSkill"],
          new PersistActiveSkillConfigurations
          {
            Countdown = 120,
            Duration = 5 + level
          });
    }
  }

  public class KillAndRecoverActiveSkillRune : BaseRune
  {
    public override void Apply(int playerNumber)
    {
      skillAndUpgradeManager
        .SkillAndUpgradeControllers[playerNumber]
        .SetActiveSkill(ActiveSkillPresets.Configurations["KillAndRecoverActiveSkill"],
          new PersistActiveSkillConfigurations
          {
            Countdown = 120,
            Duration = 10 + 2 * level
          });
    }
  }
}