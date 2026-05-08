using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Src.Utils
{
  public class GemGeneratorContainer
  {
    public readonly float PossibilityFactor;
    public readonly Func<GemProperties, GemRarity, float, GemProperties> GemGenerator;
    public readonly GemRarity MinGemRarity;

    public GemGeneratorContainer(
      Func<GemProperties, GemRarity, float, GemProperties> gemGenerator,
      GemRarity minGemRarity = GemRarity.RARE,
      float possibilityFactor = 1)
    {
      GemGenerator = gemGenerator;
      MinGemRarity = minGemRarity;
      PossibilityFactor = possibilityFactor;
    }
  }

  public static class GemUtils
  {
    public static string GetGemSpritePath(GemProperties gemProperties)
    {
      var gemPicResourceName = "Gem/gem-";

      switch (gemProperties.Rarity)
      {
        case GemRarity.RARE:
          gemPicResourceName += "lozenge_";
          break;
        case GemRarity.SUPER_RARE:
          gemPicResourceName += "hexagon_";
          break;
        case GemRarity.SUPER_SUPER_RARE:
          gemPicResourceName += "octagon_";
          break;
        case GemRarity.EXTREME_RARE:
          gemPicResourceName += "dimond_";
          break;
      }

      switch (gemProperties.Color)
      {
        case GemColors.RED:
          gemPicResourceName += "red";
          break;
        case GemColors.BLUE:
          gemPicResourceName += "blue";
          break;
        case GemColors.GREEN:
          gemPicResourceName += "green";
          break;
        case GemColors.YELLOW:
          gemPicResourceName += "yellow";
          break;
        case GemColors.LEMON:
          gemPicResourceName += "lemon";
          break;
        case GemColors.PURPLE:
          gemPicResourceName += "purple";
          break;
      }

      return gemPicResourceName;
    }

    public static GemProperties GenerateGemProperties()
    {
      var randomRarity = Random.Range(0, 1001);
      var rarity = randomRarity switch
      {
        < 85_0 => GemRarity.RARE, // => 80%
        < 95_0 => GemRarity.SUPER_RARE, // => 15%
        < 99_0 => GemRarity.SUPER_SUPER_RARE, // => 4%
        _ => GemRarity.EXTREME_RARE // => 1%
      };

      return GenerateGemProperties(rarity);
    }

    public static GemProperties GenerateGemProperties(GemRarity rarity)
    {
      // Award function
      var getAward = new Func<float, float>(x => Mathf.Pow(x, 0.6f));

      var gemProperties = new GemProperties
      {
        UUID = Guid.NewGuid().ToString(),
        Rarity = rarity
      };

      // Randomize gem color
      var gemColors = Enum.GetValues(typeof(GemColors));
      gemProperties.Color = (GemColors)gemColors.GetValue(Random.Range(0, gemColors.Length));

      var gemGeneratorContainers = new List<GemGeneratorContainer>();

      // Increase Max HP
      gemGeneratorContainers.Add(new GemGeneratorContainer((properties, _, rarityFactor) =>
      {
        properties.IncreaseMaxHp = getAward(rarityFactor) * 50;
        return properties;
      }));

      // Resurrection
      gemGeneratorContainers.Add(new GemGeneratorContainer((properties, _, _) =>
      {
        properties.Resurrection = 1;
        return properties;
      }, GemRarity.SUPER_RARE, 0.5f));

      // Movement upgrade generator
      gemGeneratorContainers.Add(new GemGeneratorContainer((properties, _, rarityFactor) =>
      {
        var upgradeItem = Random.Range(0, 4);
        var movementUpgrade = new MovementUpgrade();

        switch (upgradeItem)
        {
          case 0: movementUpgrade.AugmentMovingSpeedByPercentage = getAward(rarityFactor) * 0.3f; break;
          case 1: movementUpgrade.AugmentMovingSpeedByValue = getAward(rarityFactor) * 0.5f; break;
          case 2: movementUpgrade.AugmentAttackingMovingSpeedByPercentage = getAward(rarityFactor) * 0.3f; break;
          case 3: movementUpgrade.AugmentAttackingMovingSpeedByValue = getAward(rarityFactor) * 0.5f; break;
        }

        properties.MovementUpgrade = movementUpgrade;
        return properties;
      }, GemRarity.RARE, 6f));

      // Attack control upgrade
      gemGeneratorContainers.Add(new GemGeneratorContainer((properties, _, rarityFactor) =>
      {
        var upgradeItem = Random.Range(0, 2);
        var attackControlUpgrade = new AttackControlUpgrade();

        switch (upgradeItem)
        {
          case 0: attackControlUpgrade.CoolingCountdownDecrementByPercentage = -getAward(rarityFactor) * 0.3f; break;
          case 1: attackControlUpgrade.ReloadCountdownDecrementByPercentage = -getAward(rarityFactor) * 0.5f; break;
        }

        properties.AttackControlUpgrade = attackControlUpgrade;
        return properties;
      }));

      #region Skill upgrades

      // Skill hurt upgrade
      gemGeneratorContainers.Add(new GemGeneratorContainer((_properties, _, rarityFactor) =>
      {
        _properties.BasicSkillUpgrader.ApplicableSkillTypes = new((SkillTag[])Enum.GetValues(typeof(SkillTag)));

        var skillHurtUpgrade = new SkillHurtUpgrade();

        switch (Random.Range(0, 8))
        {
          case 0: skillHurtUpgrade.PhysicalHurtIncrementByPercentage = getAward(rarityFactor) * 0.3f; break;
          case 1:
            skillHurtUpgrade.PhysicalHurtIncrementByValue = getAward(rarityFactor) * 15;
            break;
          case 2:
            skillHurtUpgrade.MagicIceHurtIncrementByPercentage = getAward(rarityFactor) * 0.3f;
            break;
          case 3:
            skillHurtUpgrade.MagicIceHurtIncrementByValue = getAward(rarityFactor) * 15;
            break;
          case 4:
            skillHurtUpgrade.MagicFireHurtIncrementByPercentage = getAward(rarityFactor) * 0.3f;
            break;
          case 5:
            skillHurtUpgrade.MagicFireHurtIncrementByValue = getAward(rarityFactor) * 15;
            break;
          case 6:
            skillHurtUpgrade.MagicThunderHurtIncrementByPercentage = getAward(rarityFactor) * 0.3f;
            break;
          case 7:
            skillHurtUpgrade.MagicThunderHurtIncrementByValue = getAward(rarityFactor) * 15;
            break;
        }

        _properties.BasicSkillUpgrader.SkillHurtUpgrade = skillHurtUpgrade;
        return _properties;
      }));

      // Projectile count upgrade
      gemGeneratorContainers.Add(new GemGeneratorContainer((_properties, localRarity, rarityFactor) =>
      {
        _properties.BasicSkillUpgrader.ApplicableSkillTypes = new((SkillTag[])Enum.GetValues(typeof(SkillTag)));

        var value = localRarity switch
        {
          GemRarity.SUPER_SUPER_RARE => 2,
          GemRarity.EXTREME_RARE => 3,
          _ => 1
        };

        _properties.BasicSkillUpgrader.SkillCountIncrementByValue = value;
        return _properties;
      }, GemRarity.SUPER_RARE));

      // Skill property upgrade
      gemGeneratorContainers.Add(new GemGeneratorContainer((_properties, _, rarityFactor) =>
      {
        _properties.BasicSkillUpgrader.ApplicableSkillTypes = new((SkillTag[])Enum.GetValues(typeof(SkillTag)));

        var option = Random.Range(0, 3);
        switch (option)
        {
          case 0:
            _properties.BasicSkillUpgrader.SpeedIncrementByPercentage = getAward(rarityFactor) * 0.5f; break;
          case 1:
            _properties.BasicSkillUpgrader.DispersionDecrementByPercentage = -getAward(rarityFactor) * 0.5f; break;
          case 2:
            _properties.BasicSkillUpgrader.RangeIncrementByPercentage = getAward(rarityFactor) * 0.5f; break;
        }

        return _properties;
      }));

      // Penetration upgrade
      gemGeneratorContainers.Add(new GemGeneratorContainer((_properties, _, rarityFactor) =>
      {
        _properties.BasicSkillUpgrader.ApplicableSkillTypes = new((SkillTag[])Enum.GetValues(typeof(SkillTag)));

        _properties.BasicSkillUpgrader.PenetrationIncrementByValue =
          Mathf.Max(Mathf.CeilToInt(getAward(rarityFactor) * 2), 1);
        return _properties;
      }, GemRarity.SUPER_RARE));

      // Repel force upgrade
      gemGeneratorContainers.Add(new GemGeneratorContainer((_properties, _, rarityFactor) =>
      {
        _properties.BasicSkillUpgrader.ApplicableSkillTypes = new((SkillTag[])Enum.GetValues(typeof(SkillTag)));

        _properties.BasicSkillUpgrader.RepelForceIncrementByPercentage = getAward(rarityFactor) * 0.5f;
        return _properties;
      }));

      // Skill poisoning additional effect upgrade
      gemGeneratorContainers.Add(new GemGeneratorContainer((_properties, _, rarityFactor) =>
      {
        _properties.BasicSkillUpgrader.ApplicableSkillTypes = new((SkillTag[])Enum.GetValues(typeof(SkillTag)));

        _properties.BasicSkillUpgrader.SkillAdditionalEffectUpgrade = new()
        {
          AddPoisoningAdditionalEffect = true,
          PoisonAdditionalEffectUpdate = new PoisonAdditionalEffectUpgrade
          {
            PoisonPossibility = getAward(rarityFactor) * 0.2f,
            PoisonHurtPercentagePerSecond = getAward(rarityFactor) * 0.5f,
          }
        };
        return _properties;
      }));

      #endregion


      // Pick random generators and apply gem effects
      gemGeneratorContainers
        .Where(c => rarity >= c.MinGemRarity)
        .Select(obj => new
        {
          obj,
          randomKey = Random.Range(0, 10000) * obj.PossibilityFactor,
        })
        .OrderBy(a => a.randomKey)
        .Select(a => a.obj)
        .Take(rarity switch
        {
          GemRarity.RARE => Random.Range(0, 100) < 80 ? 1 : 2,
          GemRarity.SUPER_RARE => 2,
          GemRarity.SUPER_SUPER_RARE => 3,
          GemRarity.EXTREME_RARE => 3,
          _ => 1
        })
        .ToList()
        .ForEach(g => g.GemGenerator(gemProperties,
          rarity,
          rarity switch
          {
            GemRarity.RARE => Random.Range(0, 0.4f),
            GemRarity.SUPER_RARE => Random.Range(0, 0.5f),
            GemRarity.SUPER_SUPER_RARE => Random.Range(0.5f, 0.75f),
            GemRarity.EXTREME_RARE => Random.Range(0.75f, 1f),
            _ => Random.Range(0, 0.5f),
          }));

      return gemProperties;
    }

    private static string GetDescription(string i18nDescriptor, float value, bool isPercentage = true,
      bool increment = true)
    {
      var desc = string.Format(I18nUtils.GetText(i18nDescriptor), value < 0 ? "" : "+",
        isPercentage ? Mathf.Round(value * 100) : $"{value:0.##}");
      var positive = (increment && value > 0) || (!increment && value < 0);
      return (positive ? "<color=#0f0>" : "<color=#f00>") + desc + "</color>";
    }

    private static string GetDescription(string i18nDescriptor, int value, bool isPercentage = true,
      bool increment = true)
    {
      var desc = string.Format(I18nUtils.GetText(i18nDescriptor), value < 0 ? "" : "+",
        isPercentage ? Mathf.Round(value * 100) : $"{value:0.##}");
      var positive = (increment && value > 0) || (!increment && value < 0);
      return (positive ? "<color=#0f0>" : "<color=#f00>") + desc + "</color>";
    }

    public static List<string> GetGemDescription(GemProperties gem)
    {
      var descriptions = new List<string>();

      if (gem.Resurrection > 0)
      {
        descriptions.Add(GetDescription("Gem/Upgrade/Augment/Resurrection", gem.Resurrection, false));
      }

      if (gem.IncreaseMaxHp > 0)
      {
        descriptions.Add(
          GetDescription("Gem/Upgrade/Augment/IncreaseMaxHp", Mathf.RoundToInt(gem.IncreaseMaxHp), false));
      }


      if (gem.MovementUpgrade != null)
      {
        var movementUpgrade = gem.MovementUpgrade;

        if (movementUpgrade.AugmentAttackingMovingSpeedByPercentage != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/AugmentAttackingMovingSpeedByPercentage",
            movementUpgrade.AugmentAttackingMovingSpeedByPercentage));
        }

        if (movementUpgrade.AugmentAttackingMovingSpeedByValue != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/AugmentAttackingMovingSpeedByValue",
            movementUpgrade.AugmentAttackingMovingSpeedByValue, false));
        }

        if (movementUpgrade.AugmentMovingSpeedByPercentage != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/AugmentMovingSpeedByPercentage",
            movementUpgrade.AugmentMovingSpeedByPercentage));
        }

        if (movementUpgrade.AugmentMovingSpeedByValue != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/AugmentMovingSpeedByValue",
            movementUpgrade.AugmentMovingSpeedByValue, false));
        }
      }

      if (gem.AttackControlUpgrade != null)
      {
        var attackControlUpgrade = gem.AttackControlUpgrade;

        if (attackControlUpgrade.CoolingCountdownDecrementByPercentage != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/CoolingCountdownDecrementByPercentage",
            attackControlUpgrade.CoolingCountdownDecrementByPercentage, true, false));
        }

        if (attackControlUpgrade.ReloadCountdownDecrementByPercentage != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/ReloadCountdownDecrementByPercentage",
            attackControlUpgrade.ReloadCountdownDecrementByPercentage, true, false));
        }
      }

      if (gem.BasicSkillUpgrader != null)
      {
        var basicSkillUpgrade = gem.BasicSkillUpgrader;

        if (basicSkillUpgrade.DispersionDecrementByPercentage != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/DispersionDecrementByPercentage",
            basicSkillUpgrade.DispersionDecrementByPercentage, true, false));
        }

        if (basicSkillUpgrade.PenetrationIncrementByPercentage != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/PenetrationIncrementByPercentage",
            basicSkillUpgrade.PenetrationIncrementByPercentage));
        }

        if (basicSkillUpgrade.PenetrationIncrementByValue != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/PenetrationIncrementByValue",
            basicSkillUpgrade.PenetrationIncrementByValue, false));
        }

        var rangeAttackUpgrade = basicSkillUpgrade.RangeAttackUpgrade;
        if (rangeAttackUpgrade.EnableRangeAttack &&
            (rangeAttackUpgrade.DamageRangeIncrementByValue != 0 ||
             rangeAttackUpgrade.DamageRangeIncrementByPercentage != 0))
        {
          var value = rangeAttackUpgrade.DamageRangeIncrementByValue;
          var percentage = rangeAttackUpgrade.DamageRangeIncrementByPercentage;
          var description = I18nUtils.GetText("Gem/Upgrade/EnableRangeAttack") + "\n";
          if (value != 0) description += GetDescription("Gem/Upgrade/AugmentDamageRangeByValue", value, false);
          if (percentage != 0)
            description += GetDescription("Gem/Upgrade/AugmentDamageRangeByPercentage", percentage, false);

          descriptions.Add(description);
        }

        if (basicSkillUpgrade.RangeIncrementByPercentage != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/RangeIncrementByPercentage",
            basicSkillUpgrade.RangeIncrementByPercentage));
        }

        if (basicSkillUpgrade.RangeIncrementByValue != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/RangeIncrementByValue", basicSkillUpgrade.RangeIncrementByValue,
            false));
        }

        if (basicSkillUpgrade.RepelForceIncrementByPercentage != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/RepelForceIncrementByPercentage",
            basicSkillUpgrade.RepelForceIncrementByPercentage));
        }

        if (basicSkillUpgrade.RepelForceIncrementByValue != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/RepelForceIncrementByValue",
            basicSkillUpgrade.RepelForceIncrementByValue, false));
        }

        var skillAEUpgrade = basicSkillUpgrade.SkillAdditionalEffectUpgrade;
        if (skillAEUpgrade.AddPoisoningAdditionalEffect)
        {
          var _descriptions = new List<string>
          {
            I18nUtils.GetText("Gem/Upgrade/AddPoisoningAdditionalEffect")
          };

          var poisonUpgrade = skillAEUpgrade.PoisonAdditionalEffectUpdate;
          if (poisonUpgrade.PoisonPossibility != 0)
            _descriptions.Add(string.Format(I18nUtils.GetText("Gem/Upgrade/SetPoisoningPossibility"),
              Mathf.Round(100 * poisonUpgrade.PoisonPossibility)));
          if (poisonUpgrade.PoisonHurtPercentagePerSecond != 0)
            _descriptions.Add(string.Format(I18nUtils.GetText("Gem/Upgrade/SetPoisoningHurtPercentage"),
              Mathf.Round(100 * poisonUpgrade.PoisonHurtPercentagePerSecond)));
          if (poisonUpgrade.PoisonPossibilityIncrementByPercentage != 0)
            _descriptions.Add(GetDescription("Gem/Upgrade/AugmentPoisoningPossibility",
              poisonUpgrade.PoisonPossibilityIncrementByPercentage));
          if (poisonUpgrade.PoisonHurtPercentagePerSecondIncrementByPercentage != 0)
            _descriptions.Add(GetDescription("Gem/Upgrade/AugmentPoisoningHurtPercentage",
              poisonUpgrade.PoisonHurtPercentagePerSecondIncrementByPercentage));

          descriptions.Add(string.Join("\n", _descriptions.ToArray()));
        }

        if (skillAEUpgrade.AddOrConvertToBasicAdditionalEffect != BasicAdditionalEffectTypes.NONE)
        {
          var attribute = skillAEUpgrade.AddOrConvertToBasicAdditionalEffect switch
          {
            BasicAdditionalEffectTypes.FREEZE => I18nUtils.GetText("AdditionalEffect/Type/Freeze"),
            BasicAdditionalEffectTypes.BURN => I18nUtils.GetText("AdditionalEffect/Type/Burn"),
            BasicAdditionalEffectTypes.STUN => I18nUtils.GetText("AdditionalEffect/Type/Stun"),
            _ => ""
          };

          var _descriptions = new List<string>
          {
            string.Format(I18nUtils.GetText("Gem/Upgrade/AddOrConvertBasicAdditionalEffect"), attribute)
          };

          var basicAE = skillAEUpgrade.BasicAdditionalEffectUpgrade;
          if (basicAE.Possibility != 0)
            _descriptions.Add(string.Format(I18nUtils.GetText("Gem/Upgrade/SetBasicAdditionalEffectPossibility"),
              Mathf.Round(100 * basicAE.Possibility)));
          if (basicAE.PossibilityIncrementByPercentage != 0)
            _descriptions.Add(GetDescription("Gem/Upgrade/AugmentBasicAdditionalEffectPossibility",
              basicAE.PossibilityIncrementByPercentage));

          descriptions.Add(string.Join("\n", _descriptions.ToArray()));
        }

        if (basicSkillUpgrade.SkillCountIncrementByValue != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/SkillCountIncrementByValue",
            basicSkillUpgrade.SkillCountIncrementByValue, false));
        }

        #region Skill Hurt Upgrades

        var skillHurtUpgrades = basicSkillUpgrade.SkillHurtUpgrade;
        if (skillHurtUpgrades.PhysicalHurtIncrementByValue != 0)
          descriptions.Add(GetDescription("Gem/Upgrade/AugmentPhysicalHurtByValue",
            skillHurtUpgrades.PhysicalHurtIncrementByValue, false));
        if (skillHurtUpgrades.PhysicalHurtIncrementByPercentage != 0)
          descriptions.Add(GetDescription("Gem/Upgrade/AugmentPhysicalHurtByPercentage",
            skillHurtUpgrades.PhysicalHurtIncrementByPercentage));
        if (skillHurtUpgrades.MagicIceHurtIncrementByValue != 0)
          descriptions.Add(GetDescription("Gem/Upgrade/AugmentMagicIceHurtByValue",
            skillHurtUpgrades.MagicIceHurtIncrementByValue, false));
        if (skillHurtUpgrades.MagicIceHurtIncrementByPercentage != 0)
          descriptions.Add(GetDescription("Gem/Upgrade/AugmentMagicIceHurtByPercentage",
            skillHurtUpgrades.MagicIceHurtIncrementByPercentage));
        if (skillHurtUpgrades.MagicFireHurtIncrementByValue != 0)
          descriptions.Add(GetDescription("Gem/Upgrade/AugmentMagicFireHurtByValue",
            skillHurtUpgrades.MagicFireHurtIncrementByValue, false));
        if (skillHurtUpgrades.MagicFireHurtIncrementByPercentage != 0)
          descriptions.Add(GetDescription("Gem/Upgrade/AugmentMagicFireHurtByPercentage",
            skillHurtUpgrades.MagicFireHurtIncrementByPercentage));
        if (skillHurtUpgrades.MagicThunderHurtIncrementByValue != 0)
          descriptions.Add(GetDescription("Gem/Upgrade/AugmentMagicThunderByValue",
            skillHurtUpgrades.MagicThunderHurtIncrementByValue, false));
        if (skillHurtUpgrades.MagicThunderHurtIncrementByPercentage != 0)
          descriptions.Add(GetDescription("Gem/Upgrade/Augment/MagicThunderByPercentage",
            skillHurtUpgrades.MagicThunderHurtIncrementByPercentage));

        #endregion

        if (basicSkillUpgrade.SpeedIncrementByPercentage != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/SpeedIncrementByPercentage",
            basicSkillUpgrade.SpeedIncrementByPercentage));
        }

        if (basicSkillUpgrade.SpeedIncrementByValue != 0)
        {
          descriptions.Add(GetDescription("Gem/Upgrade/SpeedIncrementByValue", basicSkillUpgrade.SpeedIncrementByValue,
            false));
        }
      }

      return descriptions;
    }
  }
}