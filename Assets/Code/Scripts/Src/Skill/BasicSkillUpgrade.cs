using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Src.Skill
{
  [Serializable]
  public struct SkillHurtUpgrade
  {
    public float PhysicalHurtIncrementByPercentage { get; set; }

    public float PhysicalHurtIncrementByValue { get; set; }

    public float MagicIceHurtIncrementByPercentage { get; set; }

    public float MagicIceHurtIncrementByValue { get; set; }

    public float MagicFireHurtIncrementByPercentage { get; set; }

    public float MagicFireHurtIncrementByValue { get; set; }

    public float MagicThunderHurtIncrementByPercentage { get; set; }

    public float MagicThunderHurtIncrementByValue { get; set; }
  }

  [Serializable]
  public struct RangeAttackUpgrade
  {
    public bool EnableRangeAttack { get; set; }

    public float DamageRangeIncrementByPercentage { get; set; }

    public float DamageRangeIncrementByValue { get; set; }
  }

  [Serializable]
  public class BasicAdditionalEffectUpgrade
  {
    public float Possibility { get; set; }

    public float PossibilityIncrementByPercentage { get; set; }
  }

  [Serializable]
  public class FireAdditionalEffectUpgrade : BasicAdditionalEffectUpgrade
  {
    public float BurnHurtPercentagePerSecond { get; set; }

    public float BurnHurtPercentagePerSecondIncrementByPercentage { get; set; }

    public float BurnLastForSeconds { get; set; }

    public float BurnLastForSecondsIncrementByPercentage { get; set; }
  }

  [Serializable]
  public class ThunderAdditionalEffectUpgrade : BasicAdditionalEffectUpgrade
  {
    public float StunningSeconds { get; set; }

    public float StunningSecondsIncrementByPercentage { get; set; }
  }

  [Serializable]
  public class IceAdditionalEffectUpgrade : BasicAdditionalEffectUpgrade
  {
    public float FreezeDamagePerSecond { get; set; }

    public float FreezeDamagePerSecondIncrementByPercentage { get; set; }

    public float FreezeLastForSeconds { get; set; }

    public float FreezeLastForSecondsIncrementByPercentage { get; set; }
  }

  [Serializable]
  public class PoisonAdditionalEffectUpgrade
  {
    public float PoisonPossibility { get; set; }

    public float PoisonPossibilityIncrementByPercentage { get; set; }

    public float PoisonHurtPercentagePerSecond { get; set; }

    public float PoisonHurtPercentagePerSecondIncrementByPercentage { get; set; }
  }

  [Serializable]
  public class SkillAdditionalEffectUpgrade
  {
    public BasicAdditionalEffectTypes AddOrConvertToBasicAdditionalEffect { get; set; } = new();

    public BasicAdditionalEffectUpgrade BasicAdditionalEffectUpgrade { get; set; } = new();

    public bool AddPoisoningAdditionalEffect { get; set; }

    public PoisonAdditionalEffectUpgrade PoisonAdditionalEffectUpdate { get; set; } = new();
  }

  [Serializable]
  public class BasicSkillUpgrader : IBasicSkillUpgrader
  {
    public HashSet<SkillTag> ApplicableSkillTypes { get; set; } = new();
    public SkillHurtUpgrade SkillHurtUpgrade { get; set; } = new();
    public RangeAttackUpgrade RangeAttackUpgrade { get; set; } = new();
    public SkillAdditionalEffectUpgrade SkillAdditionalEffectUpgrade { get; set; } = new();
    public Action<BasicSkillHitContext> AddSkillHitInterceptor { get; set; }

    public int SkillCountIncrementByValue { get; set; }

    public float DispersionDecrementByPercentage { get; set; }

    public float SpeedIncrementByPercentage { get; set; }

    public float SpeedIncrementByValue { get; set; }

    public float RangeIncrementByPercentage { get; set; }

    public float RangeIncrementByValue { get; set; }

    public float PenetrationIncrementByPercentage { get; set; }

    public int PenetrationIncrementByValue { get; set; }

    public float RepelForceIncrementByPercentage { get; set; }

    public float RepelForceIncrementByValue { get; set; }

    public BasicSkillContainer UpdateBasicSkill(BasicSkillContainer skillContainer)
    {
      var skillTypes = skillContainer.SkillConfigurations.SkillTags;
      if (!ApplicableSkillTypes.Any(skillType => skillTypes.Contains(skillType)))
      {
        return skillContainer;
      }

      skillContainer.SkillCount += SkillCountIncrementByValue;

      var skillConfigs = skillContainer.SkillConfigurations;

      #region Skill Hit Interceptor

      if (AddSkillHitInterceptor != null)
      {
        skillConfigs.SkillHitInterceptors.Add(AddSkillHitInterceptor);
      }

      #endregion

      #region SkillHurts Update

      var skillHurt = skillConfigs.SkillHurt;
      var hurtPoint = skillHurt.HurtPoint;

      switch (skillHurt.HurtType)
      {
        case HurtTypes.PHYSICAL:
          skillHurt.HurtPoint = hurtPoint * (1 + SkillHurtUpgrade.PhysicalHurtIncrementByPercentage);
          skillHurt.HurtPoint = hurtPoint + SkillHurtUpgrade.PhysicalHurtIncrementByValue;
          break;
        case HurtTypes.MAGIC_ICE:
          skillHurt.HurtPoint = hurtPoint * (1 + SkillHurtUpgrade.MagicIceHurtIncrementByPercentage);
          skillHurt.HurtPoint = hurtPoint + SkillHurtUpgrade.MagicIceHurtIncrementByValue;
          break;
        case HurtTypes.MAGIC_THUNDER:
          skillHurt.HurtPoint = hurtPoint * (1 + SkillHurtUpgrade.MagicThunderHurtIncrementByPercentage);
          skillHurt.HurtPoint = hurtPoint + SkillHurtUpgrade.MagicThunderHurtIncrementByValue;
          break;
        case HurtTypes.MAGIC_FIRE:
          skillHurt.HurtPoint = hurtPoint * (1 + SkillHurtUpgrade.MagicFireHurtIncrementByPercentage);
          skillHurt.HurtPoint = hurtPoint + SkillHurtUpgrade.MagicFireHurtIncrementByValue;
          break;
        case HurtTypes.POISON:
          // FIXME: Should add poisonous skill upgrader
          break;
      }

      skillConfigs.SkillHurt = skillHurt;

      #endregion

      #region Range Attack

      if (RangeAttackUpgrade.EnableRangeAttack)
      {
        var rangeAttack = skillConfigs.RangeAttack;
        rangeAttack.Enabled = true;

        var damageRange = rangeAttack.DamageRange;
        damageRange *= (1 + RangeAttackUpgrade.DamageRangeIncrementByPercentage);
        damageRange += RangeAttackUpgrade.DamageRangeIncrementByValue;
        rangeAttack.DamageRange = damageRange;

        skillConfigs.RangeAttack = rangeAttack;
      }

      #endregion

      #region Skill Additional Effect

      var additionalEffect = skillConfigs.AdditionalEffect;

      #region Basic Additional Effects (Burn, Freeze, Stun)

      if (!SkillAdditionalEffectUpgrade.AddOrConvertToBasicAdditionalEffect.Equals(BasicAdditionalEffectTypes.NONE))
      {
        var effectUpdate = SkillAdditionalEffectUpgrade.BasicAdditionalEffectUpgrade;

        // Check if Additional Effect is new or has made a conversion.                 
        var isNewOrConvert = false;
        if (!SkillAdditionalEffectUpgrade.AddOrConvertToBasicAdditionalEffect.Equals(additionalEffect
              .BasicAdditionalEffectType))
        {
          isNewOrConvert = true;
          additionalEffect.BasicAdditionalEffectType = SkillAdditionalEffectUpgrade.AddOrConvertToBasicAdditionalEffect;

          switch (SkillAdditionalEffectUpgrade.AddOrConvertToBasicAdditionalEffect)
          {
            case BasicAdditionalEffectTypes.FREEZE:
              additionalEffect.BasicAdditionalEffect = new IceAdditionalEffect();
              break;
            case BasicAdditionalEffectTypes.BURN:
              additionalEffect.BasicAdditionalEffect = new FireAdditionalEffect();
              break;
            case BasicAdditionalEffectTypes.STUN:
              additionalEffect.BasicAdditionalEffect = new ThunderAdditionalEffect();
              break;
          }
        }

        if (isNewOrConvert || effectUpdate.Possibility != 0)
        {
          additionalEffect.BasicAdditionalEffect.Possibility = effectUpdate.Possibility;
        }
        else
        {
          additionalEffect.BasicAdditionalEffect.Possibility *= (1 + effectUpdate.PossibilityIncrementByPercentage);
        }

        additionalEffect.BasicAdditionalEffect.Possibility = Mathf.Clamp(
          additionalEffect.BasicAdditionalEffect.Possibility,
          GeneralConfigurations.AdditionalEffectLimits.MinimumPossibility,
          GeneralConfigurations.AdditionalEffectLimits.MaximumPossibility
        );

        switch (SkillAdditionalEffectUpgrade.AddOrConvertToBasicAdditionalEffect)
        {
          case BasicAdditionalEffectTypes.BURN:
            var fireEffectUpdate =
              (FireAdditionalEffectUpgrade)SkillAdditionalEffectUpgrade.BasicAdditionalEffectUpgrade;
            var fireAE = (FireAdditionalEffect)additionalEffect.BasicAdditionalEffect;

            if (isNewOrConvert || fireEffectUpdate.BurnHurtPercentagePerSecond != 0)
            {
              fireAE.HurtPercentagePerSecond = fireEffectUpdate.BurnHurtPercentagePerSecond;
            }
            else
            {
              fireAE.HurtPercentagePerSecond *= (1 + fireEffectUpdate.BurnHurtPercentagePerSecondIncrementByPercentage);
            }

            fireAE.HurtPercentagePerSecond = Mathf.Clamp(
              fireAE.HurtPercentagePerSecond,
              GeneralConfigurations.AdditionalEffectLimits.MinimumHurtPercentagePerSecond,
              GeneralConfigurations.AdditionalEffectLimits.MaximumHurtPercentagePerSecond
            );

            if (isNewOrConvert || fireEffectUpdate.BurnLastForSeconds != 0)
            {
              fireAE.LastForSeconds = fireEffectUpdate.BurnLastForSeconds;
            }
            else
            {
              fireAE.LastForSeconds *= (1 + fireEffectUpdate.BurnLastForSecondsIncrementByPercentage);
            }

            fireAE.LastForSeconds = Mathf.Clamp(
              fireAE.LastForSeconds,
              GeneralConfigurations.AdditionalEffectLimits.MinimumLastForSeconds,
              GeneralConfigurations.AdditionalEffectLimits.MaximumLastForSeconds
            );

            additionalEffect.BasicAdditionalEffect = fireAE;
            break;
          case BasicAdditionalEffectTypes.STUN:
            var thunderEffectUpdate =
              (ThunderAdditionalEffectUpgrade)SkillAdditionalEffectUpgrade.BasicAdditionalEffectUpgrade;
            var thunderAE = (ThunderAdditionalEffect)additionalEffect.BasicAdditionalEffect;

            if (isNewOrConvert || thunderEffectUpdate.StunningSeconds != 0)
            {
              thunderAE.StunningSeconds = thunderEffectUpdate.StunningSeconds;
            }
            else
            {
              thunderAE.StunningSeconds *= (1 + thunderEffectUpdate.StunningSecondsIncrementByPercentage);
            }

            thunderAE.StunningSeconds = Mathf.Clamp(
              thunderAE.StunningSeconds,
              GeneralConfigurations.AdditionalEffectLimits.MinimumStunningSeconds,
              GeneralConfigurations.AdditionalEffectLimits.MaximumStunningSeconds
            );

            additionalEffect.BasicAdditionalEffect = thunderAE;
            break;
          case BasicAdditionalEffectTypes.FREEZE:
            var iceEffectUpdate = (IceAdditionalEffectUpgrade)SkillAdditionalEffectUpgrade.BasicAdditionalEffectUpgrade;
            var iceAE = (IceAdditionalEffect)additionalEffect.BasicAdditionalEffect;

            if (isNewOrConvert || iceEffectUpdate.FreezeDamagePerSecond != 0)
            {
              iceAE.DamagePerSecond = iceEffectUpdate.FreezeDamagePerSecond;
            }
            else
            {
              iceAE.DamagePerSecond *= (1 + iceEffectUpdate.FreezeDamagePerSecondIncrementByPercentage);
            }

            if (isNewOrConvert || iceEffectUpdate.FreezeLastForSeconds != 0)
            {
              iceAE.LastForSeconds = iceEffectUpdate.FreezeLastForSeconds;
            }
            else
            {
              iceAE.LastForSeconds *= (1 + iceEffectUpdate.FreezeLastForSecondsIncrementByPercentage);
            }

            iceAE.LastForSeconds = Mathf.Clamp(
              iceAE.LastForSeconds,
              GeneralConfigurations.AdditionalEffectLimits.MinimumLastForSeconds,
              GeneralConfigurations.AdditionalEffectLimits.MaximumLastForSeconds
            );

            additionalEffect.BasicAdditionalEffect = iceAE;
            break;
        }
      }

      #endregion

      #region Poisoning Additional Effects

      if (SkillAdditionalEffectUpgrade.AddPoisoningAdditionalEffect)
      {
        var poisonAE = additionalEffect.PoisonAdditionalEffect;
        var poisonUpdate = SkillAdditionalEffectUpgrade.PoisonAdditionalEffectUpdate;

        if (poisonAE.Enable && poisonUpdate != null)
        {
          if (poisonUpdate.PoisonHurtPercentagePerSecond != 0)
          {
            poisonAE.HurtPercentagePerSecond = poisonUpdate.PoisonHurtPercentagePerSecond;
          }
          else
          {
            poisonAE.HurtPercentagePerSecond *= (1 + poisonUpdate.PoisonHurtPercentagePerSecondIncrementByPercentage);
          }

          poisonAE.HurtPercentagePerSecond = Mathf.Clamp(
            poisonAE.HurtPercentagePerSecond,
            GeneralConfigurations.AdditionalEffectLimits.MinimumHurtPercentagePerSecond,
            GeneralConfigurations.AdditionalEffectLimits.MaximumHurtPercentagePerSecond
          );

          if (poisonUpdate.PoisonPossibility != 0)
          {
            poisonAE.Possibility = poisonUpdate.PoisonPossibility;
          }
          else
          {
            poisonAE.Possibility *= (1 + poisonUpdate.PoisonPossibilityIncrementByPercentage);
          }

          poisonAE.Possibility = Mathf.Clamp(
            poisonAE.Possibility,
            GeneralConfigurations.AdditionalEffectLimits.MinimumPossibility,
            GeneralConfigurations.AdditionalEffectLimits.MaximumPossibility
          );
        }

        additionalEffect.PoisonAdditionalEffect = poisonAE;
      }

      #endregion

      skillConfigs.AdditionalEffect = additionalEffect;

      #endregion

      #region Other value updates

      skillConfigs.Dispersion *= (1 + DispersionDecrementByPercentage);

      var speed = skillConfigs.Speed;
      speed *= (1 + SpeedIncrementByPercentage);
      speed += SpeedIncrementByValue;
      skillConfigs.Speed = speed;

      var range = skillConfigs.Range;
      range *= (1 + RangeIncrementByPercentage);
      range += RangeIncrementByValue;
      skillConfigs.Range = range;

      var penetration = skillConfigs.Penetration;
      penetration = Mathf.RoundToInt(penetration * (1 + PenetrationIncrementByPercentage));
      penetration += PenetrationIncrementByValue;
      skillConfigs.Penetration = penetration;

      var repelForce = skillConfigs.RepelForce;
      repelForce *= (1 + RepelForceIncrementByPercentage);
      repelForce += RepelForceIncrementByValue;
      skillConfigs.RepelForce = repelForce;

      #endregion

      skillContainer.SkillConfigurations = skillConfigs;

      return skillContainer;
    }
  }
}