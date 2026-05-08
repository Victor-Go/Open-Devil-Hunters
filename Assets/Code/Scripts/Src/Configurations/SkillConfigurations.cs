using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.Character.Enemy.LittleBoss.Witch;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Src.Configurations
{
  public class SkillPreset
  {
    public string SkillPrefabName { get; set; }
    public AttackControlConfigurations AttackControlConfigurations { get; set; } = new();
    public BasicSkillConfigurations SkillConfigurations { get; set; } = new();
    public int DefaultQuantity { get; set; }

    public SkillPreset Clone()
    {
      var @new = (SkillPreset)MemberwiseClone();
      @new.AttackControlConfigurations = @new.AttackControlConfigurations.Clone();
      @new.SkillConfigurations = (BasicSkillConfigurations)@new.SkillConfigurations.Clone();
      return @new;
    }
  }

  public class TimingSkillPreset
  {
    public string SkillPrefabName { get; set; }
    public BasicSkillConfigurations SkillConfigurations { get; set; } = new();
    public float DefaultInterval { get; set; }
    public int DefaultQuantity { get; set; }

    public TimingSkillPreset Clone()
    {
      return (TimingSkillPreset)MemberwiseClone();
    }
  }

  public class SpiralTimingSkillConfiguration : BasicSkillConfigurations
  {
    public float AdditionalEffectPossibility { get; set; }
    public float LastForSeconds { get; set; }
    public float HurtPercentagePerSecond { get; set; }
    public float DamagePerSecond { get; set; }

    public SpiralTimingSkillConfiguration()
    {
    }

    public SpiralTimingSkillConfiguration(SpiralTimingSkillConfiguration other) : base(other)
    {
      AdditionalEffect = other.AdditionalEffect;
      LastForSeconds = other.LastForSeconds;
      HurtPercentagePerSecond = other.HurtPercentagePerSecond;
      DamagePerSecond = other.DamagePerSecond;
    }

    public override object Clone()
    {
      return new SpiralTimingSkillConfiguration(this);
    }
  }

  public class EnemySkillStrengthUpgrade
  {
    public float IncreaseHitPointCoefficient { get; set; }

    public EnemySkillStrengthUpgrade(float increaseHitPoint)
    {
      IncreaseHitPointCoefficient = increaseHitPoint;
    }
  }

  public class EnemySkillPreset
  {
    public string SkillPrefabName { get; set; }
    public BaseEnemySkillConfigurations SkillConfigurations { get; set; } = new();
  }

  public static class SkillPresets
  {
    public static readonly Dictionary<string, SkillPreset> PlayerBasicSkills = new()
    {
      {
        "ArchangelSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/BaseSkill/ArchangelSkill",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 5,
            AttackCoolingTime = 0.5f,
            ReloadTime = 1.25f,
          },
          SkillConfigurations = new()
          {
            LaunchSoundEffectNames = new()
            {
              "Audio/Sound/Skill/PlayerBaseSkill/archangel-skill"
            },
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_ICE,
              HurtPoint = 22.5f,
            },
            AdditionalEffect = new()
            {
              BasicAdditionalEffect = new IceAdditionalEffect()
              {
                LastForSeconds = 3,
                Possibility = 0.15f,
                DamagePerSecond = 4,
              },
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.FREEZE,
            },
            SkillTags = new()
            {
              SkillTag.BASE_SKILL,
              SkillTag.ICE_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 3.5f,
            Range = 2.25f,
            RepelForce = 0.1f,
            Penetration = 1,
            Dispersion = 20,
          },
          DefaultQuantity = 1,
        }
      },
      {
        "CaptainGSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/BaseSkill/CaptainGSkill",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 3,
            AttackCoolingTime = 0.4f,
            ReloadTime = 0.8f,
          },
          SkillConfigurations = new()
          {
            LaunchSoundEffectNames = new()
            {
              "Audio/Sound/Skill/PlayerBaseSkill/shotgun_0",
              "Audio/Sound/Skill/PlayerBaseSkill/shotgun_1",
            },
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_FIRE,
              HurtPoint = 5,
            },
            AdditionalEffect = new()
            {
              BasicAdditionalEffect = new FireAdditionalEffect()
              {
                LastForSeconds = 3,
                Possibility = 0.15f,
                HurtPercentagePerSecond = 0.05f,
              },
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.BURN,
            },
            SkillTags = new()
            {
              SkillTag.BASE_SKILL,
              SkillTag.FIRE_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 4.5f,
            Range = 1.5f,
            RepelForce = 0.25f,
            Penetration = 1,
            Dispersion = 50,
          },
          DefaultQuantity = 5,
        }
      },
      {
        "CutieSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/BaseSkill/CutieSkill",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 5,
            AttackCoolingTime = 0.3f,
            ReloadTime = 1.25f,
          },
          SkillConfigurations = new()
          {
            LaunchSoundEffectNames = new()
            {
              "Audio/Sound/Skill/PlayerBaseSkill/handgun_0",
            },
            SkillHurt = new()
            {
              HurtType = HurtTypes.PHYSICAL,
              HurtPoint = 12,
            },
            SkillTags = new()
            {
              SkillTag.BASE_SKILL,
              SkillTag.PHYSICAL_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 5f,
            Range = 2.5f,
            RepelForce = 0.5f,
            Penetration = int.MaxValue,
            Dispersion = 5,
          },
          DefaultQuantity = 1,
        }
      },
      {
        "GumdamSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/BaseSkill/GumdamSkill",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 4,
            AttackCoolingTime = 1f,
            ReloadTime = 1.5f,
          },
          SkillConfigurations = new CircularSkillConfigurations()
          {
            LaunchSoundEffectNames = new()
            {
              "Audio/Sound/Skill/PlayerBaseSkill/gumdam-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/gumdam-skill_1",
              "Audio/Sound/Skill/PlayerBaseSkill/gumdam-skill_2"
            },
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_FIRE,
              HurtPoint = 15,
            },
            SkillTags = new()
            {
              SkillTag.BASE_SKILL,
              SkillTag.PHYSICAL_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.CircularTrajectory,
            Speed = 3.5f,
            Range = 5,
            RepelForce = 0.25f,
            Penetration = 1,
            Dispersion = 0.75f, // Dispersion means the random target bias length for CircularTrajectory only 
            AnglePerSecond = 180,
            AnglePerSecondIncrementPerSecond = 180,
          },
          DefaultQuantity = 2,
        }
      },
      {
        "JeanneDArcSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/BaseSkill/JeanneDArcSkill",
          AttackControlConfigurations = new()
          {
            AttackPerRound = int.MaxValue,
            AttackCoolingTime = 0.8f,
            ReloadTime = 0,
          },
          SkillConfigurations = new()
          {
            LaunchSoundEffectNames = new()
            {
              "Audio/Sound/Skill/PlayerBaseSkill/jeanne-d-arc-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/jeanne-d-arc-skill_1"
            },
            SkillHurt = new()
            {
              HurtType = HurtTypes.PHYSICAL,
              HurtPoint = 20,
            },
            SkillTags = new()
            {
              SkillTag.BASE_SKILL,
              SkillTag.PHYSICAL_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.JeanneDArcSkillTrajectory,
            Speed = 0,
            Range = 0,
            RepelForce = 0.35f,
            Penetration = int.MaxValue,
            Dispersion = 20,
          },
          DefaultQuantity = 2
        }
      },
      {
        "MountainKingSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/BaseSkill/MountainKingSkill",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 5,
            AttackCoolingTime = 0.65f,
            ReloadTime = 1.25f,
          },
          SkillConfigurations = new()
          {
            LaunchSoundEffectNames = new()
            {
              "Audio/Sound/Skill/PlayerBaseSkill/mountain-king-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/mountain-king-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/mountain-king-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/mountain-king-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/mountain-king-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/mountain-king-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/mountain-king-skill_1",
            },
            AdditionalLaunchSoundEffectNames = new()
            {
              "Audio/Sound/Skill/PlayerBaseSkill/thunder-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/thunder-skill_1",
              "Audio/Sound/Skill/PlayerBaseSkill/thunder-skill_2",
              "Audio/Sound/Skill/PlayerBaseSkill/thunder-skill_3",
              "Audio/Sound/Skill/PlayerBaseSkill/thunder-skill_4",
              "Audio/Sound/Skill/PlayerBaseSkill/thunder-skill_5",
            },
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_THUNDER,
              HurtPoint = 30,
            },
            AdditionalEffect = new()
            {
              BasicAdditionalEffect = new ThunderAdditionalEffect()
              {
                Possibility = 0.15f,
              },
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.STUN,
            },
            SkillTags = new()
            {
              SkillTag.BASE_SKILL,
              SkillTag.THUNDER_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 4f,
            Range = 2.25f,
            RepelForce = 0.25f,
            Penetration = 1,
            Dispersion = 75,
          },
          DefaultQuantity = 1,
        }
      },
      {
        "PaladinSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/BaseSkill/PaladinSkill",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 5,
            AttackCoolingTime = 0.4f,
            ReloadTime = 1f,
          },
          SkillConfigurations = new()
          {
            LaunchSoundEffectNames = new()
            {
              "Audio/Sound/Skill/PlayerBaseSkill/paladin-skill_0"
            },
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_FIRE,
              HurtPoint = 22,
            },
            SkillTags = new()
            {
              SkillTag.BASE_SKILL,
              SkillTag.PHYSICAL_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 4f,
            Range = 2f,
            RepelForce = 0.25f,
            Penetration = 1,
            Dispersion = 30,
          },
          DefaultQuantity = 1,
        }
      },
      {
        "RangerSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/BaseSkill/RangerSkill",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 5,
            AttackCoolingTime = 0.25f,
            ReloadTime = 1.25f,
          },
          SkillConfigurations = new CircularSkillConfigurations()
          {
            LaunchSoundEffectNames = new()
            {
              "Audio/Sound/Skill/PlayerBaseSkill/ranger-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/ranger-skill_1",
              "Audio/Sound/Skill/PlayerBaseSkill/ranger-skill_2",
            },
            SkillHurt = new()
            {
              HurtType = HurtTypes.POISON,
              HurtPoint = 8,
            },
            SkillTags = new()
            {
              SkillTag.BASE_SKILL,
              SkillTag.POISON_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            AdditionalEffect = new()
            {
              PoisonAdditionalEffect = new PoisonAdditionalEffect()
              {
                Enable = true,
                Possibility = 0.3f,
                HurtPercentagePerSecond = 0.2f,
              },
            },
            TrajectoryType = TrajectoryTypes.CircularTrajectory,
            Speed = 4f,
            Range = 4.5f,
            RepelForce = 0.05f,
            Penetration = 1,
            Dispersion = 30,
            AnglePerSecond = 240,
            AnglePerSecondIncrementPerSecond = 180,
          },
          DefaultQuantity = 2,
        }
      },
      {
        "WitchSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/BaseSkill/WitchSkill",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 3,
            AttackCoolingTime = 0.3f,
            ReloadTime = 0.75f,
          },
          SkillConfigurations = new()
          {
            LaunchSoundEffectNames = new()
            {
              "Audio/Sound/Skill/PlayerBaseSkill/witch-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/witch-skill_1",
              "Audio/Sound/Skill/PlayerBaseSkill/witch-skill_2",
              "Audio/Sound/Skill/PlayerBaseSkill/witch-skill_3",
              "Audio/Sound/Skill/PlayerBaseSkill/witch-skill_4",
            },
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_THUNDER,
              HurtPoint = 15,
            },
            SkillTags = new()
            {
              SkillTag.BASE_SKILL,
              SkillTag.PHYSICAL_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 4,
            Range = 2.25f,
            RepelForce = 0.25f,
            Penetration = 1,
            Dispersion = 30,
          },
          DefaultQuantity = 2,
        }
      },
      {
        "WuKongSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/BaseSkill/WuKongSkill",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 5,
            AttackCoolingTime = 0.25f,
            ReloadTime = 1f,
          },
          SkillConfigurations = new()
          {
            LaunchSoundEffectNames = new()
            {
              "Audio/Sound/Skill/PlayerBaseSkill/wukong-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/wukong-skill_1",
              "Audio/Sound/Skill/PlayerBaseSkill/wukong-skill_2",
              "Audio/Sound/Skill/PlayerBaseSkill/wukong-skill_3",
              "Audio/Sound/Skill/PlayerBaseSkill/wukong-skill_4",
              "Audio/Sound/Skill/PlayerBaseSkill/wukong-skill_5",
            },
            SkillHurt = new()
            {
              HurtType = HurtTypes.PHYSICAL,
              HurtPoint = 14 * (DebugConfigurations.DebugEnabled ? 100 : 1),
            },
            SkillTags = new()
            {
              SkillTag.BASE_SKILL,
              SkillTag.PHYSICAL_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 4,
            Range = 2.25f,
            RepelForce = 0.18f,
            Penetration = 1,
            Dispersion = 30,
          },
          DefaultQuantity = 1,
        }
      },
      {
        "ZhaoYunSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/BaseSkill/ZhaoYunSkill",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 4,
            AttackCoolingTime = 0.5f,
            ReloadTime = 1f,
          },
          SkillConfigurations = new()
          {
            LaunchSoundEffectNames = new()
            {
              "Audio/Sound/Skill/PlayerBaseSkill/zhaoyun-skill_0",
              "Audio/Sound/Skill/PlayerBaseSkill/zhaoyun-skill_1",
            },
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_ICE,
              HurtPoint = 18,
            },
            SkillTags = new()
            {
              SkillTag.BASE_SKILL,
              SkillTag.PHYSICAL_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.CentralMultiTrajectory,
            Speed = 4,
            Range = 2.25f,
            RepelForce = 0.25f,
            Penetration = 2,
            Dispersion = 15,
          },
          DefaultQuantity = 3,
        }
      },
    };

    public static List<string> PlayerBasicSkillPrefabNames
    {
      get { return PlayerBasicSkills.Values.Select(s => s.SkillPrefabName).ToList(); }
    }

    public static Dictionary<string, SkillPreset> PlayerSkills { get; } = new()
    {
      #region Surround Skill

      {
        "DartSurroundSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/SurroundSkill/Dart",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtType = HurtTypes.PHYSICAL,
              HurtPoint = 10,
            },
            SkillTags = new()
            {
              SkillTag.PHYSICAL_SKILL,
              SkillTag.SURROUND_SKILL,
            },
            RepelForce = 0,
          }
        }
      },
      {
        "BoomerangSurroundSkill",
        new()
        {
          SkillPrefabName = "Skill/Player/SurroundSkill/Boomerang",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_FIRE,
              HurtPoint = 12,
            },
            SkillTags = new()
            {
              SkillTag.FIRE_SKILL,
              SkillTag.SURROUND_SKILL,
            },
            RepelForce = 0.25f,
          }
        }
      },
      {
        "IceTowerBullet",
        new()
        {
          SkillPrefabName = "Skill/Player/SurroundSkill/IceTowerBullet",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_ICE,
              HurtPoint = 18,
            },
            Speed = 6,
            RepelForce = 0.25f,
            Range = 5,
          }
        }
      },
      {
        "FireTowerBullet",
        new()
        {
          SkillPrefabName = "Skill/Player/SurroundSkill/FireTowerBullet",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_FIRE,
              HurtPoint = 18,
            },
            Speed = 6,
            RepelForce = 0.25f,
            Range = 5,
          }
        }
      },
      {
        "ThunderTowerBullet",
        new()
        {
          SkillPrefabName = "Skill/Player/SurroundSkill/ThunderTowerBullet",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_THUNDER,
              HurtPoint = 18,
            },
            Speed = 6,
            RepelForce = 0.25f,
            Range = 5,
          }
        }
      },

      #endregion

      {
        "GeneralBullet",
        new()
        {
          SkillPrefabName = "Skill/Player/GeneralBullet",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 2,
            AttackCoolingTime = 0.5f,
            ReloadTime = 1f,
          },
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtType = HurtTypes.PHYSICAL,
              HurtPoint = 10
            },
            SkillTags = new()
            {
              SkillTag.PHYSICAL_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 9f,
            Range = 6f,
            RepelForce = 0.5f,
            Penetration = 1,
            Dispersion = 10,
          },
          DefaultQuantity = 1,
        }
      },
      {
        "ArtilleryShell",
        new()
        {
          SkillPrefabName = "Skill/Player/ArtilleryShell",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 3,
            AttackCoolingTime = 0.5f,
            ReloadTime = 1f,
          },
          SkillConfigurations = new()
          {
            SkillTags = new()
            {
              SkillTag.PHYSICAL_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            SkillHurt = new()
            {
              HurtType = HurtTypes.PHYSICAL,
              HurtPoint = 10
            },
            AdditionalEffect = new()
            {
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.BURN,
              BasicAdditionalEffect = new FireAdditionalEffect()
              {
                Possibility = 0.03f,
                HurtPercentagePerSecond = 0.02f,
                LastForSeconds = 3,
              },
              PoisonAdditionalEffect = new()
              {
                Enable = true,
                HurtPercentagePerSecond = 0.05f,
                Possibility = 0.5f,
              }
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 3f,
            Range = 5f,
            RepelForce = 1.5f,
            Penetration = 1,
            Dispersion = 10,
          },
          DefaultQuantity = 1,
        }
      },
      {
        "FireBullet",
        new()
        {
          SkillPrefabName = "Skill/Player/FireBullet",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 10,
            AttackCoolingTime = 0.25f,
            ReloadTime = 1f,
          },
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_FIRE,
              HurtPoint = 10,
            },
            SkillTags = new()
            {
              SkillTag.FIRE_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 9f,
            Range = 6f,
            RepelForce = 0.75f,
            Penetration = 2,
            Dispersion = 10,
          },
          DefaultQuantity = 1,
        }
      },
      {
        "ThunderBullet",
        new()
        {
          SkillPrefabName = "Skill/Player/ThunderBullet",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 10,
            AttackCoolingTime = 0.25f,
            ReloadTime = 1f,
          },
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_THUNDER,
              HurtPoint = 10,
            },
            SkillTags = new()
            {
              SkillTag.THUNDER_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 15f,
            Range = 8f,
            RepelForce = 1,
            Penetration = 2,
            Dispersion = 10,
          },
          DefaultQuantity = 1,
        }
      },
      {
        "IceArrowBullet",
        new()
        {
          SkillPrefabName = "Skill/Player/Hail0",
          AttackControlConfigurations = new()
          {
            AttackPerRound = 10,
            AttackCoolingTime = 0.25f,
            ReloadTime = 1f,
          },
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtType = HurtTypes.MAGIC_ICE,
              HurtPoint = 10,
            },
            SkillTags = new()
            {
              SkillTag.THUNDER_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 15f,
            Range = 8f,
            RepelForce = 1.25f,
            Penetration = 2,
            Dispersion = 10,
          },
          DefaultQuantity = 1,
        }
      },
    };

    public static Dictionary<string, TimingSkillPreset> PlayerTimingSkills { get; } = new()
    {
      #region Spiral

      {
        "Spiral",
        new()
        {
          DefaultInterval = 15,
          SkillPrefabName = "Skill/Player/TimingSkill/Spiral",
          SkillConfigurations = new SpiralTimingSkillConfiguration()
          {
            HurtPercentagePerSecond = 0.01f,
            DamagePerSecond = 10,
            AdditionalEffectPossibility = 0.5f,
            LastForSeconds = 5,
            SkillHurt = new()
            {
              HurtPoint = 32,
              HurtType = HurtTypes.PHYSICAL
            },
            SkillTags = new()
            {
              SkillTag.TIMING_SKILL,
              SkillTag.MUTABLE_BULLET,
              SkillTag.MUTABLE_HURT_TYPE,
            },
            TrajectoryType = TrajectoryTypes.SpiralTrajectory,
            Speed = 120,
            Range = 12,
            RepelForce = 0.25f,
            Penetration = int.MaxValue,
            Dispersion = 0,
          }
        }
      },

      #endregion

      #region Puppet

      {
        "PuppetLv0",
        new()
        {
          DefaultInterval = 15,
          SkillPrefabName = "Skill/Player/TimingSkill/Puppet/PuppetLv0",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtPoint = 15,
              HurtType = HurtTypes.PHYSICAL,
            },
            SkillTags = new()
            {
              SkillTag.TIMING_SKILL,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 2,
            Penetration = int.MaxValue,
            Dispersion = 20,
          }
        }
      },
      {
        "PuppetLv1",
        new()
        {
          DefaultInterval = 12.5f,
          SkillPrefabName = "Skill/Player/TimingSkill/Puppet/PuppetLv1",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtPoint = 25,
              HurtType = HurtTypes.PHYSICAL,
            },
            SkillTags = new()
            {
              SkillTag.TIMING_SKILL,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 1.75f,
            Penetration = int.MaxValue,
            Dispersion = 20,
          }
        }
      },
      {
        "PuppetLv2",
        new()
        {
          DefaultInterval = 10,
          SkillPrefabName = "Skill/Player/TimingSkill/Puppet/PuppetLv2",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtPoint = 40,
              HurtType = HurtTypes.MAGIC_THUNDER,
            },
            SkillTags = new()
            {
              SkillTag.TIMING_SKILL,
            },
            TrajectoryType = TrajectoryTypes.NormalTrajectory,
            Speed = 1.5f,
            Penetration = int.MaxValue,
            Dispersion = 20,
          }
        }
      },

      #endregion

      #region Flying Sword

      {
        "FlyingSwordLv0",
        new()
        {
          DefaultInterval = 10,
          SkillPrefabName = "Skill/Player/TimingSkill/FlyingSword/FlyingSwordLv0",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtPoint = 25,
              HurtType = HurtTypes.PHYSICAL,
            },
            SkillTags = new()
            {
              SkillTag.TIMING_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.CentralMultiTrajectory,
            Speed = 6,
            Penetration = int.MaxValue,
            Dispersion = 0,
            Range = 6,
          }
        }
      },
      {
        "FlyingSwordLv1",
        new()
        {
          DefaultInterval = 12.5f,
          SkillPrefabName = "Skill/Player/TimingSkill/FlyingSword/FlyingSwordLv1",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtPoint = 35,
              HurtType = HurtTypes.PHYSICAL,
            },
            SkillTags = new()
            {
              SkillTag.TIMING_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            TrajectoryType = TrajectoryTypes.CentralMultiTrajectory,
            Speed = 6.5f,
            Penetration = int.MaxValue,
            Dispersion = 0,
            Range = 8,
          }
        }
      },
      {
        "FlyingSwordLv2",
        new()
        {
          DefaultInterval = 15,
          SkillPrefabName = "Skill/Player/TimingSkill/FlyingSword/FlyingSwordLv2",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtPoint = 50,
              HurtType = HurtTypes.MAGIC_FIRE,
            },
            SkillTags = new()
            {
              SkillTag.TIMING_SKILL,
              SkillTag.MUTABLE_BULLET,
            },
            AdditionalEffect = new()
            {
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.BURN,
              BasicAdditionalEffect = new FireAdditionalEffect
              {
                LastForSeconds = 5,
                Possibility = 0.25f,
                HurtPercentagePerSecond = 0.05f,
              },
            },
            TrajectoryType = TrajectoryTypes.CentralMultiTrajectory,
            Speed = 7,
            Penetration = int.MaxValue,
            Dispersion = 0,
            Range = 8,
          }
        }
      },

      #endregion

      #region Mine

      {
        "MineLv0",
        new()
        {
          DefaultInterval = 10,
          SkillPrefabName = "Skill/Player/TimingSkill/Mine/FireMine/FireMineLv0",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtPoint = 30,
              HurtType = HurtTypes.PHYSICAL,
            },
            SkillTags = new()
            {
              SkillTag.TIMING_SKILL,
            },
            AdditionalEffect = new()
            {
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.STUN,
              BasicAdditionalEffect = new ThunderAdditionalEffect()
              {
                Possibility = 0.3f,
                StunningSeconds = 1,
              },
            },
            TrajectoryType = TrajectoryTypes.MineTrajectory,
            Penetration = int.MaxValue,
            Dispersion = 0,
            Range = 1.5f,
          }
        }
      },
      {
        "MineLv1",
        new()
        {
          DefaultInterval = 12.5f,
          SkillPrefabName = "Skill/Player/TimingSkill/Mine/FireMine/FireMineLv1",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtPoint = 45,
              HurtType = HurtTypes.PHYSICAL,
            },
            SkillTags = new()
            {
              SkillTag.TIMING_SKILL,
            },
            AdditionalEffect = new()
            {
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.STUN,
              BasicAdditionalEffect = new ThunderAdditionalEffect()
              {
                Possibility = 0.3f,
                StunningSeconds = 3,
              },
            },
            TrajectoryType = TrajectoryTypes.MineTrajectory,
            Penetration = int.MaxValue,
            Dispersion = 0,
            Range = 1.75f,
          }
        }
      },
      {
        "MineLv2",
        new()
        {
          DefaultInterval = 15f,
          SkillPrefabName = "Skill/Player/TimingSkill/Mine/FireMine/FireMineLv2",
          SkillConfigurations = new()
          {
            SkillHurt = new()
            {
              HurtPoint = 65,
              HurtType = HurtTypes.PHYSICAL,
            },
            SkillTags = new()
            {
              SkillTag.TIMING_SKILL,
            },
            AdditionalEffect = new()
            {
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.STUN,
              BasicAdditionalEffect = new ThunderAdditionalEffect()
              {
                Possibility = 0.4f,
                StunningSeconds = 6,
              },
            },
            TrajectoryType = TrajectoryTypes.MineTrajectory,
            Penetration = int.MaxValue,
            Dispersion = 0,
            Range = 2f,
          }
        }
      },

      #endregion
    };

    public static Dictionary<string, EnemySkillPreset> BossSkills { get; } = new()
    {
      {
        "WitchBullet",
        new EnemySkillPreset
        {
          SkillPrefabName = "Enemy/LittleBoss/Witch/WitchBullet",
          SkillConfigurations = new WitchBulletConfigurations
          {
            HitPoint = 40,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.1f),
            Speed = 3.5f,
            LifeTime = 6,
            AnglePerSecond = 135,
            AdditionalEffect = new()
            {
              PoisonAdditionalEffect = new PoisonAdditionalEffect
              {
                Enable = true,
                HurtPercentagePerSecond = 0.05f,
                Possibility = 0.5f,
              }
            }
          }
        }
      },
      {
        "LasermonSkillPurple",
        new EnemySkillPreset
        {
          SkillPrefabName = "Enemy/LittleBoss/Lasermon/LasermonSkillPurple",
          SkillConfigurations = new WitchBulletConfigurations
          {
            HitPoint = 50,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.1f),
            Speed = 3,
            LifeTime = 5,
            AnglePerSecond = 120,
            AdditionalEffect = new SkillAdditionalEffect
            {
              BasicAdditionalEffect = new IceAdditionalEffect()
              {
                LastForSeconds = 5,
                Possibility = 0.2f,
                DamagePerSecond = 10,
              },
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.FREEZE,
            }
          }
        }
      },
      {
        "LasermonSkillGreen",
        new EnemySkillPreset
        {
          SkillPrefabName = "Enemy/LittleBoss/Lasermon/LasermonSkillGreen",
          SkillConfigurations = new WitchBulletConfigurations
          {
            HitPoint = 45,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.1f),
            Speed = 3,
            LifeTime = 5,
            AnglePerSecond = 120,
            AdditionalEffect = new SkillAdditionalEffect
            {
              PoisonAdditionalEffect = new PoisonAdditionalEffect
              {
                Enable = true,
                HurtPercentagePerSecond = 0.05f,
                Possibility = 0.2f,
              }
            }
          }
        }
      },
      {
        "WerewolfClaw",
        new EnemySkillPreset
        {
          SkillConfigurations = new BaseEnemySkillConfigurations()
          {
            HitPoint = 65,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.125f),
            AdditionalEffect = new SkillAdditionalEffect
            {
              BasicAdditionalEffect = new ThunderAdditionalEffect
              {
                StunningSeconds = 3,
                Possibility = 0.5f
              },
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.STUN,
            }
          }
        }
      },
      {
        "SpecterSkill",
        new EnemySkillPreset
        {
          SkillConfigurations = new BaseEnemySkillConfigurations
          {
            HitPoint = 15,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.125f),
          }
        }
      },
      {
        "DevilBurningBat",
        new EnemySkillPreset
        {
          SkillPrefabName = "Enemy/Boss/Devil/DevilBurningBat",
          SkillConfigurations = new LinearEnemySkillConfigurations()
          {
            LaunchSoundEffectName = "Audio/Sound/Enemy/Boss/devil-skill_0",
            df = t => Mathf.Cos(3 * t),
            HitPoint = 55,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.135f),
            Speed = 4,
            AdditionalEffect = new SkillAdditionalEffect
            {
              BasicAdditionalEffect = new FireAdditionalEffect
              {
                LastForSeconds = 6,
                Possibility = 0.5f,
                HurtPercentagePerSecond = 0.1f,
              },
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.BURN,
            }
          }
        }
      },
      {
        "DevilPoisonSkull",
        new EnemySkillPreset
        {
          SkillPrefabName = "Enemy/Boss/Devil/DevilPoisonSkull",
          SkillConfigurations = new LinearEnemySkillConfigurations
          {
            LaunchSoundEffectName = "Audio/Sound/Enemy/Boss/devil-skill_1",
            df = t => 4 * Mathf.Cos(10 * t) / (Mathf.PI * Mathf.Sqrt(-Mathf.Pow(Mathf.Sin(10 * t), 2) + 1)),
            HitPoint = 62,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.15f),
            Speed = 3.5f,
            AdditionalEffect = new SkillAdditionalEffect
            {
              PoisonAdditionalEffect = new PoisonAdditionalEffect
              {
                Enable = true,
                HurtPercentagePerSecond = 0.075f,
                Possibility = 0.45f,
              }
            }
          }
        }
      },
      {
        "DevilShockWave",
        new EnemySkillPreset
        {
          SkillPrefabName = "Enemy/Boss/Devil/DevilShockWave",
          SkillConfigurations = new LinearEnemySkillConfigurations()
          {
            LaunchSoundEffectName = "Audio/Sound/Enemy/Boss/devil-skill_2",
            df = t => 0,
            HitPoint = 48,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.125f),
            Speed = 4.5f,
            AdditionalEffect = new SkillAdditionalEffect
            {
              BasicAdditionalEffect = new IceAdditionalEffect
              {
                LastForSeconds = 7,
                Possibility = 0.6f,
                DamagePerSecond = 60,
              },
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.FREEZE,
            }
          }
        }
      },
      {
        "ThunderDemonSkill",
        new EnemySkillPreset
        {
          SkillPrefabName = "Enemy/LittleBoss/ThunderDemon/ThunderDemonSkill",
          SkillConfigurations = new LinearEnemySkillConfigurations()
          {
            df = t => 0,
            HitPoint = 45,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.1f),
            Speed = 3.75f,
            AdditionalEffect = new()
            {
              BasicAdditionalEffect = new ThunderAdditionalEffect()
              {
                StunningSeconds = 1.5f,
                Possibility = 0.8f
              },
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.STUN,
            }
          }
        }
      },
      {
        "IceTigerSkill",
        new EnemySkillPreset
        {
          SkillPrefabName = "Enemy/LittleBoss/ThunderDemon/ThunderDemonSkill",
          SkillConfigurations = new LinearEnemySkillConfigurations()
          {
            df = t => 0,
            HitPoint = 55,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.1f),
            Speed = 1,
            AdditionalEffect = new()
            {
              BasicAdditionalEffect = new IceAdditionalEffect()
              {
                LastForSeconds = 7,
                Possibility = 0.55f,
                DamagePerSecond = 50,
              },
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.FREEZE,
            }
          }
        }
      },
      {
        "CannonDragonSkill",
        new EnemySkillPreset
        {
          SkillPrefabName = "Enemy/LittleBoss/ThunderDemon/ThunderDemonSkill",
          SkillConfigurations = new LinearEnemySkillConfigurations()
          {
            df = t => 0,
            HitPoint = 55,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.1f),
            Speed = 5f,
            AdditionalEffect = new()
            {
              BasicAdditionalEffect = new ThunderAdditionalEffect
              {
                StunningSeconds = 2f,
                Possibility = 0.45f
              },
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.STUN,
            }
          }
        }
      },
      {
        "RockGiantSkill",
        new EnemySkillPreset
        {
          SkillPrefabName = "Enemy/Boss/RockGiant/RockGiantSkill",
          SkillConfigurations = new LinearEnemySkillConfigurations()
          {
            df = t => 0,
            HitPoint = 30,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.1f),
            Speed = 3.5f,
            AdditionalEffect = new()
            {
              BasicAdditionalEffect = new ThunderAdditionalEffect()
              {
                StunningSeconds = 2.5f,
                Possibility = 0.75f
              },
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.STUN,
            }
          }
        }
      },
      {
        "StellarbotPunch",
        new EnemySkillPreset
        {
          SkillPrefabName = "Enemy/Boss/Stellarbot/StellarbotPunch",
          SkillConfigurations = new LinearEnemySkillConfigurations()
          {
            df = t => 0,
            HitPoint = 60,
            StrengthUpgrade = new EnemySkillStrengthUpgrade(0.1f),
            Speed = 6,
            AdditionalEffect = new()
            {
              BasicAdditionalEffect = new ThunderAdditionalEffect()
              {
                StunningSeconds = 2f,
                Possibility = 0.85f
              },
              BasicAdditionalEffectType = BasicAdditionalEffectTypes.STUN,
            }
          }
        }
      }
    };

    public static Dictionary<string, EnemySkillPreset> EnvironmentEnemySkills { get; } = new()
    {
      {
        "ForestBullet2",
        new()
        {
          SkillPrefabName = "Skill/Enemy/ForestBullet2",
          SkillConfigurations = new()
          {
            HitPoint = 10,
            Speed = 1f,
          }
        }
      },
      {
        "ForestBullet4",
        new()
        {
          SkillPrefabName = "Skill/Enemy/ForestBullet4",
          SkillConfigurations = new()
          {
            HitPoint = 10,
            Speed = 1.5f,
          }
        }
      },
      {
        "TreeMonsterBullet",
        new()
        {
          SkillPrefabName = "Skill/Enemy/TreeMonsterBullet",
          SkillConfigurations = new RangingEnemySkillConfigurations()
          {
            Range = 5,
            HitPoint = 15,
            Speed = 1f,
          }
        }
      }
    };
  }
}