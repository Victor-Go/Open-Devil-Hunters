using System.Collections.Generic;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Src.Configurations
{
  public struct Range
  {
    public float Minimum { get; set; }
    public float Maximum { get; set; }
  }

  public struct GemGenerationConfiguration
  {
    public Dictionary<GemRarity, Range> Range { get; set; }
  }

  // Important: Gem cannot enable SkillBonusEffects.EnableRangeAttack.
  public static class GemConfigurations
  {
    public static Dictionary<string, GemGenerationConfiguration> MovementUpgradeConfigs { get; } = new()
    {
      {
        "AugmentMovingSpeedByValue",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.125f,
                Maximum = 0.175f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.175f,
                Maximum = 0.25f,
              }
            }
          }
        }
      },
      {
        "AugmentMovingSpeedByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.01f,
                Maximum = 0.03f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.02f,
                Maximum = 0.05f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            }
          }
        }
      },
      {
        "AugmentAttackingMovingSpeedByValue",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.125f,
                Maximum = 0.175f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.175f,
                Maximum = 0.25f,
              }
            }
          }
        }
      },
      {
        "AugmentAttackingMovingSpeedByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.01f,
                Maximum = 0.03f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.02f,
                Maximum = 0.05f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            }
          }
        }
      }
    };

    public static Dictionary<string, GemGenerationConfiguration> AttackUpgradeConfigs { get; } = new()
    {
      {
        "ReloadCountdownDecrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = -0.05f,
                Maximum = -0.075f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = -0.075f,
                Maximum = -0.1f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = -0.1f,
                Maximum = -0.125f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = -0.125f,
                Maximum = -0.15f,
              }
            }
          }
        }
      },
      {
        "CoolingCountdownDecrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = -0.05f,
                Maximum = -0.075f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = -0.075f,
                Maximum = -0.1f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = -0.1f,
                Maximum = -0.125f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = -0.125f,
                Maximum = -0.15f,
              }
            }
          }
        }
      }
    };

    public static Dictionary<string, GemGenerationConfiguration> BasicSkillUpgradeConfigs { get; } = new()
    {
      {
        "SkillCountIncrementByValue",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 1,
                Maximum = 1,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 1,
                Maximum = 2,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 1,
                Maximum = 3,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 2,
                Maximum = 3,
              }
            }
          }
        }
      },
      {
        "DispersionDecrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = -0.05f,
                Maximum = -0.075f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = -0.075f,
                Maximum = -0.1f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = -0.1f,
                Maximum = -0.125f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = -0.125f,
                Maximum = -0.15f,
              }
            }
          }
        }
      },
      {
        "SpeedIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.075f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.075f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.125f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.125f,
                Maximum = 0.15f,
              }
            }
          }
        }
      },
      {
        "SpeedIncrementByValue",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.25f,
                Maximum = 0.35f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.35f,
                Maximum = 0.55f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.55f,
                Maximum = 0.8f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.8f,
                Maximum = 1.2f,
              }
            }
          }
        }
      },
      {
        "RangeIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.2f,
                Maximum = 0.25f,
              }
            }
          }
        }
      },
      {
        "RangeIncrementByValue",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.25f,
                Maximum = 0.5f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.5f,
                Maximum = 0.75f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.75f,
                Maximum = 1f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 1f,
                Maximum = 1.25f,
              }
            }
          }
        }
      },
      {
        "PenetrationIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 1,
                Maximum = 1,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 1,
                Maximum = 1,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 1,
                Maximum = 2,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 1,
                Maximum = 2,
              }
            }
          }
        }
      },
      {
        "PenetrationIncrementByValue",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 1,
                Maximum = 1,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 1,
                Maximum = 2,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 1,
                Maximum = 3,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 2,
                Maximum = 3,
              }
            }
          }
        }
      },
      {
        "RepelForceIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.2f,
                Maximum = 0.25f,
              }
            }
          }
        }
      },
      {
        "RepelForceIncrementByValue",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.2f,
                Maximum = 0.25f,
              }
            }
          }
        }
      },
    };

    public static Dictionary<string, GemGenerationConfiguration> SkillHurtUpgrades { get; } = new()
    {
      {
        "PhysicalHurtIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.25f,
                Maximum = 0.3f,
              }
            }
          }
        }
      },
      {
        "PhysicalHurtIncrementByValue",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 1f,
                Maximum = 1.5f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 1.5f,
                Maximum = 2f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 2.5f,
                Maximum = 3f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 3f,
                Maximum = 4f,
              }
            }
          }
        }
      },
      {
        "MagicIceHurtIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.25f,
                Maximum = 0.3f,
              }
            }
          }
        }
      },
      {
        "MagicIceHurtIncrementByValue",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 1f,
                Maximum = 1.5f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 1.5f,
                Maximum = 2f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 2.5f,
                Maximum = 3f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 3f,
                Maximum = 4f,
              }
            }
          }
        }
      },
      {
        "MagicFireHurtIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.25f,
                Maximum = 0.3f,
              }
            }
          }
        }
      },
      {
        "MagicFireHurtIncrementByValue",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 1f,
                Maximum = 1.5f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 1.5f,
                Maximum = 2f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 2.5f,
                Maximum = 3f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 3f,
                Maximum = 4f,
              }
            }
          }
        }
      },
      {
        "MagicThunderHurtIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.25f,
                Maximum = 0.3f,
              }
            }
          }
        }
      },
      {
        "MagicThunderHurtIncrementByValue",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 1f,
                Maximum = 1.5f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 1.5f,
                Maximum = 2f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 2.5f,
                Maximum = 3f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 3f,
                Maximum = 4f,
              }
            }
          }
        }
      },
    };

    public static Dictionary<string, GemGenerationConfiguration> RangeAttackUpgradeConfigs { get; } = new()
    {
      {
        "DamageRangeIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.2f,
                Maximum = 0.25f,
              }
            }
          }
        }
      },
      {
        "DamageRangeIncrementByValue",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.2f,
                Maximum = 0.3f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.3f,
                Maximum = 0.4f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.4f,
                Maximum = 0.5f,
              }
            }
          }
        }
      },
    };

    public static Dictionary<string, GemGenerationConfiguration> AdditionalEffectUpgradeConfigs { get; } = new()
    {
      {
        "Possibility",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.03f,
                Maximum = 0.035f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.035f,
                Maximum = 0.04f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.04f,
                Maximum = 0.045f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.045f,
                Maximum = 0.05f,
              }
            }
          }
        }
      },
      {
        "PossibilityIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.25f,
                Maximum = 0.3f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.3f,
                Maximum = 0.35f,
              }
            }
          }
        }
      },
      {
        "BurnHurtPercentagePerSecond",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.075f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.075f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.075f,
                Maximum = 0.125f,
              }
            }
          }
        }
      },
      {
        "BurnHurtPercentagePerSecondIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.2f,
                Maximum = 0.25f,
              }
            }
          }
        }
      },
      {
        "BurnLastForSeconds",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 2f,
                Maximum = 3f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 3f,
                Maximum = 4f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 3f,
                Maximum = 4f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 4f,
                Maximum = 5f,
              }
            }
          }
        }
      },
      {
        "BurnLastForSecondsIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.2f,
                Maximum = 0.25f,
              }
            }
          }
        }
      },
      {
        "StunningSeconds",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 4f,
                Maximum = 5f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 5f,
                Maximum = 5.5f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 5.5f,
                Maximum = 6f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 6f,
                Maximum = 6.5f,
              }
            }
          }
        }
      },
      {
        "StunningSecondsIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.2f,
                Maximum = 0.25f,
              }
            }
          }
        }
      },
      {
        "FreezeDamagePerSecondIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.2f,
                Maximum = 0.25f,
              }
            }
          }
        }
      },
      {
        "FreezeDamagePerSecond",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 5f,
                Maximum = 10f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 10f,
                Maximum = 15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 15f,
                Maximum = 20f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 20f,
                Maximum = 25f,
              }
            }
          }
        }
      },
      {
        "FreezeLastForSeconds",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 2f,
                Maximum = 3f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 3f,
                Maximum = 4f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 3f,
                Maximum = 4f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 4f,
                Maximum = 5f,
              }
            }
          }
        }
      },
      {
        "FreezeLastForSecondsIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.2f,
                Maximum = 0.25f,
              }
            }
          }
        }
      },
      {
        "PoisonPossibility",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.03f,
                Maximum = 0.035f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.035f,
                Maximum = 0.04f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.04f,
                Maximum = 0.045f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.045f,
                Maximum = 0.05f,
              }
            }
          }
        }
      },
      {
        "PoisonPossibilityIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.25f,
                Maximum = 0.3f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.3f,
                Maximum = 0.35f,
              }
            }
          }
        }
      },
      {
        "PoisonHurtPercentagePerSecond",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.075f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.075f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.075f,
                Maximum = 0.125f,
              }
            }
          }
        }
      },
      {
        "PoisonHurtPercentagePerSecondIncrementByPercentage",
        new()
        {
          Range = new()
          {
            {
              GemRarity.RARE,
              new()
              {
                Minimum = 0.05f,
                Maximum = 0.1f,
              }
            },
            {
              GemRarity.SUPER_RARE,
              new()
              {
                Minimum = 0.1f,
                Maximum = 0.15f,
              }
            },
            {
              GemRarity.SUPER_SUPER_RARE,
              new()
              {
                Minimum = 0.15f,
                Maximum = 0.2f,
              }
            },
            {
              GemRarity.EXTREME_RARE,
              new()
              {
                Minimum = 0.2f,
                Maximum = 0.25f,
              }
            }
          }
        }
      }
    };
  }
}