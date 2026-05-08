using System;
using System.Collections.Generic;
using Code.Scripts.Src;
using Code.Scripts.Src.Rune;
using Code.Scripts.Src.Skill;

namespace Code.Scripts.Src.Configurations
{
  public enum RuneTypes
  {
    CLONED_PROJECTILE,
    EXP_BONUS,
    FISSION,
    HAIL_STRIKE,
    HOLY_SHIELD,
    INCREASE_HONOR,
    INSTANT_KILL,
    INSTANT_RELOAD,
    KILL_AND_RECOVER,
    INCREASE_IMMORTAL_TIME,
    INCREASE_MAX_HP,
    METEORITE_STRIKE,
    PICK_UP_DISTANCE,
    POISONOUS,
    PUSH_AWAY,
    HP_RECOVERY,
    REDUCED_INJURY,
    RESURRECTION,
    THUNDER_STRIKE,
    TIME_STOP,
  }

  public abstract class BaseRune
  {
    protected int level; // From 0 to 4
    protected readonly SkillAndUpgradeManager skillAndUpgradeManager = SkillAndUpgradeManager.Instance;
    protected readonly StoreManager storeManager = StoreManager.Instance;

    public void SetRuneLevel(int level)
    {
      this.level = level;
    }

    public abstract void Apply(int playerNumber);
  }

  public class RunePreset
  {
    public RuneTypes RuneTypes { get; set; }
    public string TitleName { get; set; }
    public string DescriptionName { get; set; }
    public Type RuneControllerType { get; set; }
  }

  public static class RunePresets
  {
    public static Dictionary<RuneTypes, RunePreset> Presets { get; } = new()
    {
      {
        RuneTypes.CLONED_PROJECTILE,
        new()
        {
          TitleName = "Rune/Title/ClonedProjectile",
          DescriptionName = "Rune/Description/ClonedProjectile",
          RuneTypes = RuneTypes.CLONED_PROJECTILE,
          RuneControllerType = typeof(ClonedProjectileRune),
        }
      },
      {
        RuneTypes.EXP_BONUS,
        new()
        {
          TitleName = "Rune/Title/ExpBonus",
          DescriptionName = "Rune/Description/ExpBonus",
          RuneTypes = RuneTypes.EXP_BONUS,
          RuneControllerType = typeof(IncreaseExperienceRune),
        }
      },
      {
        RuneTypes.FISSION,
        new()
        {
          TitleName = "Rune/Title/Fission",
          DescriptionName = "Rune/Description/Fission",
          RuneTypes = RuneTypes.FISSION,
          RuneControllerType = typeof(SkillFissionActiveSkillRune),
        }
      },
      {
        RuneTypes.HAIL_STRIKE,
        new()
        {
          TitleName = "Rune/Title/HailStrike",
          DescriptionName = "Rune/Description/HailStrike",
          RuneTypes = RuneTypes.HAIL_STRIKE,
          RuneControllerType = typeof(HailStrikeActiveSkillRune),
        }
      },
      {
        RuneTypes.HOLY_SHIELD,
        new()
        {
          TitleName = "Rune/Title/HolyShield",
          DescriptionName = "Rune/Description/HolyShield",
          RuneTypes = RuneTypes.HOLY_SHIELD,
          RuneControllerType = typeof(HolyShieldActiveSkillRune),
        }
      },
      {
        RuneTypes.INCREASE_HONOR,
        new()
        {
          TitleName = "Rune/Title/IncreaseHonor",
          DescriptionName = "Rune/Description/IncreaseHonor",
          RuneTypes = RuneTypes.INCREASE_HONOR,
          RuneControllerType = typeof(IncreaseHonorRune),
        }
      },
      {
        RuneTypes.INSTANT_KILL,
        new()
        {
          TitleName = "Rune/Title/InstantKill",
          DescriptionName = "Rune/Description/InstantKill",
          RuneTypes = RuneTypes.INSTANT_KILL,
          RuneControllerType = typeof(InstantKillRune),
        }
      },
      {
        RuneTypes.INSTANT_RELOAD,
        new()
        {
          TitleName = "Rune/Title/InstantReload",
          DescriptionName = "Rune/Description/InstantReload",
          RuneTypes = RuneTypes.INSTANT_RELOAD,
          RuneControllerType = typeof(InstantReloadRune),
        }
      },
      {
        RuneTypes.KILL_AND_RECOVER,
        new()
        {
          TitleName = "Rune/Title/KillAndRecover",
          DescriptionName = "Rune/Description/KillAndRecover",
          RuneTypes = RuneTypes.KILL_AND_RECOVER,
          RuneControllerType = typeof(KillAndRecoverActiveSkillRune),
        }
      },
      {
        RuneTypes.INCREASE_IMMORTAL_TIME,
        new()
        {
          TitleName = "Rune/Title/IncreaseImmortalTime",
          DescriptionName = "Rune/Description/IncreaseImmortalTime",
          RuneTypes = RuneTypes.INCREASE_IMMORTAL_TIME,
          RuneControllerType = typeof(IncreaseImmortalTimeRune),
        }
      },
      {
        RuneTypes.INCREASE_MAX_HP,
        new()
        {
          TitleName = "Rune/Title/IncreaseMaxHp",
          DescriptionName = "Rune/Description/IncreaseMaxHp",
          RuneTypes = RuneTypes.INCREASE_MAX_HP,
          RuneControllerType = typeof(IncreaseMaximumHpRune),
        }
      },
      {
        RuneTypes.METEORITE_STRIKE,
        new()
        {
          TitleName = "Rune/Title/MeteoriteStrike",
          DescriptionName = "Rune/Description/MeteoriteStrike",
          RuneTypes = RuneTypes.METEORITE_STRIKE,
          RuneControllerType = typeof(MeteoriteStrikeActiveSkillRune),
        }
      },
      {
        RuneTypes.PICK_UP_DISTANCE,
        new()
        {
          TitleName = "Rune/Title/PickUpDistance",
          DescriptionName = "Rune/Description/PickUpDistance",
          RuneTypes = RuneTypes.PICK_UP_DISTANCE,
          RuneControllerType = typeof(IncreasePickUpRune),
        }
      },
      {
        RuneTypes.POISONOUS,
        new()
        {
          TitleName = "Rune/Title/Poisonous",
          DescriptionName = "Rune/Description/Poisonous",
          RuneTypes = RuneTypes.POISONOUS,
          RuneControllerType = typeof(FullFieldPoisoningActiveSkillRune),
        }
      },
      {
        RuneTypes.PUSH_AWAY,
        new()
        {
          TitleName = "Rune/Title/PushAway",
          DescriptionName = "Rune/Description/PushAway",
          RuneTypes = RuneTypes.PUSH_AWAY,
          RuneControllerType = typeof(PushAwayEnemiesRune),
        }
      },
      {
        RuneTypes.HP_RECOVERY,
        new()
        {
          TitleName = "Rune/Title/HpRecovery",
          DescriptionName = "Rune/Description/HpRecovery",
          RuneTypes = RuneTypes.HP_RECOVERY,
          RuneControllerType = typeof(AddHpRecoveryRune),
        }
      },
      {
        RuneTypes.REDUCED_INJURY,
        new()
        {
          TitleName = "Rune/Title/ReducedInjuery",
          DescriptionName = "Rune/Description/ReducedInjuery",
          RuneTypes = RuneTypes.REDUCED_INJURY,
          RuneControllerType = typeof(ReduceHurtRune),
        }
      },
      {
        RuneTypes.RESURRECTION,
        new()
        {
          TitleName = "Rune/Title/Resurrection",
          DescriptionName = "Rune/Description/Resurrection",
          RuneTypes = RuneTypes.RESURRECTION,
          RuneControllerType = typeof(OneTimeResurrectionRune),
        }
      },
      {
        RuneTypes.THUNDER_STRIKE,
        new()
        {
          TitleName = "Rune/Title/ThunderStrike",
          DescriptionName = "Rune/Description/ThunderStrike",
          RuneTypes = RuneTypes.THUNDER_STRIKE,
          RuneControllerType = typeof(ThunderStrikeActiveSkillRune),
        }
      },
      {
        RuneTypes.TIME_STOP,
        new()
        {
          TitleName = "Rune/Title/TimeStop",
          DescriptionName = "Rune/Description/TimeStop",
          RuneTypes = RuneTypes.TIME_STOP,
          RuneControllerType = typeof(TimeStopActiveSkillRune)
        }
      },
    };
  }

  public class PriceToUnlockRuneLevels
  {
    public int[] Price { get; set; }
  }

  public static class RuneConfigurations
  {
    public static readonly List<List<RuneTypes>> RuneClasses = new()
    {
      new()
      {
        RuneTypes.PICK_UP_DISTANCE,
        RuneTypes.PUSH_AWAY,
        RuneTypes.INCREASE_IMMORTAL_TIME,
      },
      new()
      {
        RuneTypes.INCREASE_HONOR,
        RuneTypes.INSTANT_RELOAD,
        RuneTypes.REDUCED_INJURY,
        RuneTypes.INCREASE_MAX_HP,
        RuneTypes.HP_RECOVERY,
      },
      new()
      {
        RuneTypes.EXP_BONUS,
        RuneTypes.INSTANT_KILL,
        RuneTypes.CLONED_PROJECTILE,
        RuneTypes.RESURRECTION,
      },
      new()
      {
        RuneTypes.TIME_STOP,
        RuneTypes.FISSION,
        RuneTypes.METEORITE_STRIKE,
        RuneTypes.THUNDER_STRIKE,
        RuneTypes.HAIL_STRIKE,
        RuneTypes.POISONOUS,
        RuneTypes.HOLY_SHIELD,
        RuneTypes.KILL_AND_RECOVER
      }
    };

    public static Dictionary<RuneTypes, List<RuneTypes>> Prerequisites =
      new() // List<RuneTypes> indicates conditions to unlock the Rune. Only one of the conditions needs to be met.
      {
        {
          RuneTypes.PICK_UP_DISTANCE,
          new() { }
        },
        {
          RuneTypes.INCREASE_HONOR,
          new()
          {
            RuneTypes.PICK_UP_DISTANCE,
          }
        },
        {
          RuneTypes.EXP_BONUS,
          new()
          {
            RuneTypes.INCREASE_HONOR,
          }
        },
        {
          RuneTypes.TIME_STOP,
          new()
          {
            RuneTypes.EXP_BONUS,
          }
        },
        {
          RuneTypes.PUSH_AWAY,
          new() { }
        },
        {
          RuneTypes.INSTANT_RELOAD,
          new()
          {
            RuneTypes.PUSH_AWAY,
          }
        },
        {
          RuneTypes.CLONED_PROJECTILE,
          new()
          {
            RuneTypes.INSTANT_RELOAD,
          }
        },
        {
          RuneTypes.INSTANT_KILL,
          new()
          {
            RuneTypes.CLONED_PROJECTILE,
          }
        },
        {
          RuneTypes.METEORITE_STRIKE,
          new()
          {
            RuneTypes.CLONED_PROJECTILE,
          }
        },
        {
          RuneTypes.THUNDER_STRIKE,
          new()
          {
            RuneTypes.CLONED_PROJECTILE,
          }
        },
        {
          RuneTypes.HAIL_STRIKE,
          new()
          {
            RuneTypes.CLONED_PROJECTILE,
          }
        },
        {
          RuneTypes.POISONOUS,
          new()
          {
            RuneTypes.CLONED_PROJECTILE,
          }
        },
        {
          RuneTypes.FISSION,
          new()
          {
            RuneTypes.INSTANT_KILL,
          }
        },
        {
          RuneTypes.INCREASE_IMMORTAL_TIME,
          new() { }
        },
        {
          RuneTypes.REDUCED_INJURY,
          new()
          {
            RuneTypes.INCREASE_IMMORTAL_TIME,
          }
        },
        {
          RuneTypes.INCREASE_MAX_HP,
          new()
          {
            RuneTypes.INCREASE_IMMORTAL_TIME,
          }
        },
        {
          RuneTypes.HP_RECOVERY,
          new()
          {
            RuneTypes.INCREASE_IMMORTAL_TIME,
          }
        },
        {
          RuneTypes.RESURRECTION,
          new()
          {
            RuneTypes.REDUCED_INJURY,
            RuneTypes.INCREASE_MAX_HP,
            RuneTypes.HP_RECOVERY,
          }
        },
        {
          RuneTypes.HOLY_SHIELD,
          new()
          {
            RuneTypes.RESURRECTION,
          }
        },
        {
          RuneTypes.KILL_AND_RECOVER,
          new()
          {
            RuneTypes.RESURRECTION,
          }
        }
      };

    public static readonly PriceToUnlockRuneLevels[] PriceToUnlockRuneLevels = new PriceToUnlockRuneLevels[4]
    {
      // Class 1
      new()
      {
        Price = new[] { 3000, 4000, 5000, 6000, 7000 }
      },
      // Class 2
      new()
      {
        Price = new[] { 5000, 6000, 7000, 8000, 9000 }
      },
      // Class 3
      new()
      {
        Price = new[] { 7000, 8000, 9000, 10000, 11000 }
      },
      // Class 4
      new()
      {
        Price = new[] { 9000, 10000, 11000, 12000, 13000 }
      }
    };
  }
}