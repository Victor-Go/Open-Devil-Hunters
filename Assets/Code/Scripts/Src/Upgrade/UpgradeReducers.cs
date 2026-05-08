using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Upgrade;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Behaviour.Skill.Surround;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Src.Upgrade
{
  public class UpgradeReducerContainer
  {
    public string UpgradeId { get; set; }
    public UpgradeCategory Category { get; set; }
    public List<string> PrerequisiteUpgradeIds { get; set; }
    public List<string> SatisfyAnyIds { get; set; }
    public List<string> NoTheseUpgradeIds { get; set; }
    public int MaximumUpgradeCount { get; set; }
    public string UpgradeNameIndicator { get; set; }
    public string UpgradeNameText { get; set; } // Translated name
    public string UpgradeDescriptionIndicator { get; set; }
    public string UpgradeDescriptionText { get; set; } // Translated description 
    public string ImageName { get; set; }

    public Action<int> UpgradeReducer { get; set; }

    /*
     * Value should be in the following range: [0,1]
     */
    public float Possibility { get; set; }

    /*
     * Should return a value in the following range: [0,1]
     */
    public Func<List<UpgradeContainer>, int, float> GetPossibilityIndependentVariable { get; set; }
    public Func<int, bool> IsAvailable { get; set; }
  }

  public enum UpgradeCategory
  {
    Health,
    Speed,
    Miscellaneous,
    Projectile,
    Magazine,
    BaseSkill,
    SurroundSkill,
    SpiralSkill,
    Puppet,
    FlyingSword,
    Mine,
  }

  public static class UpgradeReducers
  {
    private const float maxPossibilityMultiple = 2.5f;

    // The more player upgrade, the more possibility to have the upgrade
    private const float relationCoefficient = 0.175f;

    private static string getText(string indicator, params float[] args)
    {
      return args.Length switch
      {
        // 0 => I18nUtils.GetText(indicator),
        1 => string.Format(I18nUtils.GetText(indicator), args[0]),
        2 => string.Format(I18nUtils.GetText(indicator), args[0], args[1]),
        3 => string.Format(I18nUtils.GetText(indicator), args[0], args[1], args[2]),
        4 => string.Format(I18nUtils.GetText(indicator), args[0], args[1], args[2], args[3]),
        _ => string.Format(I18nUtils.GetText(indicator))
      };
    }

    private static string getText(string indicator, params int[] args)
    {
      try
      {
        return args.Length switch
        {
          1 => string.Format(I18nUtils.GetText(indicator), args[0]),
          2 => string.Format(I18nUtils.GetText(indicator), args[0], args[1]),
          3 => string.Format(I18nUtils.GetText(indicator), args[0], args[1], args[2]),
          4 => string.Format(I18nUtils.GetText(indicator), args[0], args[1], args[2], args[3]),
          _ => string.Format(I18nUtils.GetText(indicator))
        };
      }
      catch (Exception e)
      {
        Debug.LogWarning($"Failed to get text: {indicator}, {args.Length}");
        return "";
      }
    }

    private static string getText(string indicator)
    {
      return I18nUtils.GetText(indicator);
    }

    public static List<UpgradeReducerContainer> GetCandidateUpgrades(int playerNumber, int quantity)
    {
      var existingUpgrades = StoreManager.Instance.GetState<UpgradeState>(StoreNames.UpgradeStore)
        .PlayerUpgrades[playerNumber]
        .Upgrades;

      var filteredUpgrades = fixedUpgradeReducers
        .Concat(GenerateDynamicUpgrades(playerNumber, existingUpgrades))
        .Where(
          c =>
            (c.IsAvailable == null || c.IsAvailable(playerNumber)) &&
            (c.NoTheseUpgradeIds == null || !existingUpgrades.Any(u => c.NoTheseUpgradeIds.Contains(u.UpgradeId))) &&
            (c.PrerequisiteUpgradeIds == null || !c.PrerequisiteUpgradeIds.Any() ||
             c.PrerequisiteUpgradeIds.All(prerequisiteId =>
               existingUpgrades.Select(u => u.UpgradeId).Contains(prerequisiteId))) &&
            (c.SatisfyAnyIds == null || !c.SatisfyAnyIds.Any() ||
             c.SatisfyAnyIds.Any(satisfyId => existingUpgrades.Select(u => u.UpgradeId).Contains(satisfyId))) &&
            (c.MaximumUpgradeCount <= 0 ||
             existingUpgrades.Count(u => u.UpgradeId == c.UpgradeId) < c.MaximumUpgradeCount)
        ).ToList();

      var seen = new HashSet<UpgradeCategory>();

      return filteredUpgrades
        .Select(upgrade => new
        {
          Upgrade = upgrade,
          RandomKey = upgrade.GetPossibilityIndependentVariable != null
            ? Random.Range(0,
              Mathf.Clamp(
                maxPossibilityMultiple *
                Mathf.Pow(upgrade.GetPossibilityIndependentVariable(existingUpgrades, playerNumber), 0.85f),
                1,
                maxPossibilityMultiple))
            : Random.Range(0,
              Mathf.Clamp(
                upgrade.Possibility == 0 ? 1 : maxPossibilityMultiple * upgrade.Possibility,
                1,
                maxPossibilityMultiple)
            )
        })
        .OrderByDescending(x => x.RandomKey)
        .Select(x => x.Upgrade)
        .Where(upgrade => seen.Add(upgrade.Category))
        .Take(quantity <= filteredUpgrades.Count ? quantity : filteredUpgrades.Count)
        .Select(upgrade => new
        {
          Upgrade = upgrade,
          RandomKey = Random.Range(0f, 1f)
        })
        .OrderBy(x => x.RandomKey)
        .Select(x => x.Upgrade)
        .ToList();
    }

    private static List<UpgradeReducerContainer> GenerateDynamicUpgrades(int playerNumber,
      List<UpgradeContainer> existingUpgrades)
    {
      var autoGenerateReducers = new List<UpgradeReducerContainer>();

      foreach (var generator in dynamicUpgradeReducerGenerators)
      {
        autoGenerateReducers.AddRange(generator(playerNumber, existingUpgrades));
      }

      return autoGenerateReducers;
    }

    private static readonly List<Func<int, List<UpgradeContainer>, List<UpgradeReducerContainer>>>
      dynamicUpgradeReducerGenerators =
        new()
        {
          #region Health

          (_playerNumber, existingUpgrades) =>
          {
            // Recover HP
            var _playerData = StoreManager.Instance.GetState<PlayerState>(StoreNames.PlayerStore)
              .PlayerDatas[_playerNumber];
            var _currentHp = _playerData.CurrentHp;
            var _maxHp = _playerData.MaximumHp;
            var recoverRatio = 1 - _currentHp / _maxHp;
            var hpPercentage = Mathf.Clamp01(Random.Range(recoverRatio * 0.5f, recoverRatio));

            // Increase Max HP%
            var maxHpPercentage =
              MathUtils.GetRandom(MathUtils.GetPiecewise(1.5f,
                  0.5f,
                  5,
                  0.125f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseMaxHp")),
                0.1f);

            // Increase Max HP% on hurt
            var maxHpPercentageOnHurtMaxIncrement = MathUtils.GetRandom(MathUtils.GetPiecewise(1.5f,
                0.4f,
                5,
                0.15f,
                existingUpgrades.Count(u => u.UpgradeId == "IncreaseMaxHpOnHurt")),
              0.1f);
            var maxHpPercentageOnHurt = maxHpPercentageOnHurtMaxIncrement / 10f;

            // Increase defense%
            var defensePercentage =
              MathUtils.GetRandom(MathUtils.GetPiecewise(1.5f,
                  0.25f,
                  4,
                  0.05f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseDefense")),
                0.1f);

            return new List<UpgradeReducerContainer>
            {
              new()
              {
                UpgradeId = "RecoverHp",
                Category = UpgradeCategory.Health,
                IsAvailable = playerNumber =>
                {
                  var playerData = StoreManager.Instance.GetState<PlayerState>(StoreNames.PlayerStore)
                    .PlayerDatas[playerNumber];
                  var currentHp = playerData.CurrentHp;
                  var maxHp = playerData.MaximumHp;
                  return currentHp < 0.5f * maxHp;
                },
                GetPossibilityIndependentVariable = (_, playerNumber) =>
                {
                  var playerData = StoreManager.Instance.GetState<PlayerState>(StoreNames.PlayerStore)
                    .PlayerDatas[playerNumber];
                  var currentHp = playerData.CurrentHp;
                  var maxHp = playerData.MaximumHp;
                  var hpRatio = currentHp / maxHp;
                  return Mathf.Pow(1 - hpRatio, 0.8f) * maxPossibilityMultiple;
                },
                UpgradeNameText = getText("Upgrade/RecoverHp/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "RecoverHp") + 1),
                UpgradeDescriptionText = getText("Upgrade/RecoverHp/Description", hpPercentage * 100),
                ImageName = "UpgradeImage/instant-recover",
                UpgradeReducer = playerNumber =>
                {
                  StoreManager.Instance.Commit(StoreNames.PlayerStore,
                    StoreActions.PlayerStore_ADD_CURRENT_HP_PERCENTAGE,
                    new PlayerActionData
                    {
                      PlayerNumber = playerNumber,
                      AddCurrentHpPercentage = hpPercentage,
                    });
                }
              },
              new()
              {
                UpgradeId = "IncreaseMaxHp",
                Category = UpgradeCategory.Health,
                UpgradeNameText = getText("Upgrade/IncreaseMaxHp/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseMaxHp") + 1),
                UpgradeDescriptionText = getText("Upgrade/IncreaseMaxHp/Description", maxHpPercentage * 100),
                ImageName = "UpgradeImage/increase-max-hp",
                Possibility = 0.15f,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var currentMaxHp = storeManager.GetState<PlayerState>(StoreNames.PlayerStore)
                    .PlayerDatas[playerNumber]
                    .MaximumHp;

                  storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_AUGMENT_MAXIMUM_HP,
                    new PlayerActionData
                    {
                      PlayerNumber = playerNumber,
                      AugmentMaximumHp = currentMaxHp * maxHpPercentage
                    });
                }
              },
              new()
              {
                UpgradeId = "IncreaseMaxHpOnHurt",
                Category = UpgradeCategory.Health,
                UpgradeNameText = getText("Upgrade/IncreaseMaxHpOnHurt/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseMaxHpOnHurt") + 1),
                UpgradeDescriptionText = getText("Upgrade/IncreaseMaxHpOnHurt/Description", maxHpPercentageOnHurt * 100,
                  maxHpPercentageOnHurtMaxIncrement * 100),
                ImageName = "UpgradeImage/increase-max-hp-on-hurt",
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Health) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var maxHp = StoreManager.Instance.GetState<PlayerState>(StoreNames.PlayerStore)
                    .PlayerDatas[playerNumber]
                    .MaximumHp;

                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .AddPlayerHurtInterceptor(new IncreaseMaxHpOnHurt(
                      maxHp,
                      maxHpPercentageOnHurtMaxIncrement,
                      maxHpPercentageOnHurt
                    ));
                }
              },
              new()
              {
                UpgradeId = "IncreaseDefense",
                Category = UpgradeCategory.Health,
                UpgradeNameText = getText("Upgrade/IncreaseDefense/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseDefense") + 1),
                UpgradeDescriptionText = getText("Upgrade/IncreaseDefense/Description", defensePercentage * 100),
                ImageName = "UpgradeImage/increase-defense",
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Health) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .AddPlayerHurtAction(context =>
                    {
                      context.HitPoint *= 1 / (1 + defensePercentage);
                      return context;
                    });
                }
              },
            };
          },

          #endregion

          #region Speed

          (_playerNumber, existingUpgrades) =>
          {
            // All speed
            var increaseAllSpeedPercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.35f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseAllSpeed")), 0.1f);

            // Speed
            var increaseMovingSpeedPercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.4f, 5, 0.15f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseMovingSpeed")), 0.1f);
            var increaseAttackMovingSpeedPercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.4f, 5, 0.15f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseAttackingMovingSpeed")), 0.1f);

            // Kill to go fast
            const int killRequirement = 150;
            var maxIncrement =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.4f, 5, 0.15f,
                  existingUpgrades.Count(u => u.UpgradeId == "KillToIncreaseSpeed")), 0.1f);
            var incrementPer500 = maxIncrement * 5;

            return new List<UpgradeReducerContainer>
            {
              new()
              {
                UpgradeId = "IncreaseAllSpeed",
                Category = UpgradeCategory.Speed,
                UpgradeNameText = getText("Upgrade/IncreaseAllSpeed/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseAllSpeed") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseAllSpeed/Description", increaseAllSpeedPercentage * 100),
                ImageName = "UpgradeImage/increase-speed",
                IsAvailable = playerNumber =>
                {
                  var playerData = StoreManager.Instance.GetState<PlayerState>(StoreNames.PlayerStore)
                    .PlayerDatas[playerNumber];
                  return playerData.MovingSpeed < GeneralConfigurations.PlayerMaxSpeed &&
                         playerData.AttackingMovingSpeed < GeneralConfigurations.PlayerMaxSpeed;
                },
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Speed) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var playerData = storeManager.GetState<PlayerState>(StoreNames.PlayerStore).PlayerDatas[playerNumber];

                  var speed = playerData.MovingSpeed;
                  var aSpeed = playerData.AttackingMovingSpeed;

                  storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_MOVING_SPEED,
                    new PlayerActionData
                    {
                      PlayerNumber = playerNumber,
                      MovingSpeed = speed * (1 + increaseMovingSpeedPercentage)
                    });

                  storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_ATTACKING_MOVING_SPEED,
                    new PlayerActionData()
                    {
                      PlayerNumber = playerNumber,
                      AttackingMovingSpeed = aSpeed * (1 + increaseMovingSpeedPercentage)
                    });
                }
              },
              new()
              {
                UpgradeId = "IncreaseMovingSpeed",
                Category = UpgradeCategory.Speed,
                UpgradeNameText = getText("Upgrade/IncreaseMovingSpeed/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseMovingSpeed") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseMovingSpeed/Description", increaseMovingSpeedPercentage * 100),
                ImageName = "UpgradeImage/increase-speed",
                IsAvailable = playerNumber =>
                {
                  var playerData = StoreManager.Instance.GetState<PlayerState>(StoreNames.PlayerStore)
                    .PlayerDatas[playerNumber];
                  return playerData.MovingSpeed < GeneralConfigurations.PlayerMaxSpeed;
                },
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Speed) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var playerData = storeManager.GetState<PlayerState>(StoreNames.PlayerStore).PlayerDatas[playerNumber];

                  var speed = playerData.MovingSpeed;

                  storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_MOVING_SPEED,
                    new PlayerActionData
                    {
                      PlayerNumber = playerNumber,
                      MovingSpeed = speed * (1 + increaseMovingSpeedPercentage)
                    });
                }
              },
              new()
              {
                UpgradeId = "IncreaseAttackingMovingSpeed",
                Category = UpgradeCategory.Speed,
                UpgradeNameText = getText("Upgrade/IncreaseAttackingMovingSpeed/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseAttackingMovingSpeed") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseAttackingMovingSpeed/Description",
                    increaseAttackMovingSpeedPercentage * 100),
                ImageName = "UpgradeImage/increase-speed",
                IsAvailable = playerNumber =>
                {
                  var playerData = StoreManager.Instance.GetState<PlayerState>(StoreNames.PlayerStore)
                    .PlayerDatas[playerNumber];
                  return playerData.AttackingMovingSpeed < GeneralConfigurations.PlayerMaxSpeed;
                },
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Speed) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var playerData = storeManager.GetState<PlayerState>(StoreNames.PlayerStore).PlayerDatas[playerNumber];

                  var aSpeed = playerData.AttackingMovingSpeed;

                  storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_ATTACKING_MOVING_SPEED,
                    new PlayerActionData()
                    {
                      PlayerNumber = playerNumber,
                      AttackingMovingSpeed = aSpeed * (1 + increaseAttackMovingSpeedPercentage)
                    });
                }
              },
              new()
              {
                UpgradeId = "KillToIncreaseSpeed",
                Category = UpgradeCategory.Speed,
                UpgradeNameText = getText("Upgrade/KillToIncreaseSpeed/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "KillToIncreaseSpeed") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/KillToIncreaseSpeed/Description", killRequirement, incrementPer500,
                    maxIncrement * 100),
                ImageName = "UpgradeImage/increase-speed",
                IsAvailable = playerNumber =>
                {
                  var playerData = StoreManager.Instance.GetState<PlayerState>(StoreNames.PlayerStore)
                    .PlayerDatas[playerNumber];
                  return playerData.MovingSpeed < GeneralConfigurations.PlayerMaxSpeed &&
                         playerData.AttackingMovingSpeed < GeneralConfigurations.PlayerMaxSpeed;
                },
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Speed) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var controller =
                    new KillEnemyToIncreaseSpeed(playerNumber, killRequirement, incrementPer500, maxIncrement);

                  var skillController = SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];

                  skillController.AddPlayerHurtInterceptor(controller);
                  skillController.AddEnemyDieInterceptor(controller);
                }
              },
            };
          },

          #endregion

          #region Micellaneous (Pickup, Exp, Shooting Range)

          (_playerNumber, existingUpgrades) =>
          {
            var expBonusPercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.25f, 4, 0.075f, existingUpgrades.Count(u => u.UpgradeId == "ExpBonus")),
                0.1f);
            var pickUpRangePercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.4f, 2, 0.125f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreasePickUp")), 0.1f);
            var shootingRangePercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.35f, 2, 0.05f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseShootingRange")), 0.1f);

            return new List<UpgradeReducerContainer>()
            {
              new()
              {
                UpgradeId = "ExpBonus",
                Category = UpgradeCategory.Miscellaneous,
                UpgradeNameText = getText("Upgrade/ExpBonus/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "ExpBonus") + 1),
                UpgradeDescriptionText = getText("Upgrade/ExpBonus/Description", expBonusPercentage * 100),
                ImageName = "UpgradeImage/exp-bonus",
                MaximumUpgradeCount = 7,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.Miscellaneous) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .AddObtainExperienceAction(context =>
                    {
                      if (Random.Range(0, 1f) < 0.5f)
                      {
                        context.Experience *= 1 + expBonusPercentage;
                      }

                      return context;
                    });
                }
              },
              new()
              {
                UpgradeId = "IncreasePickUp",
                Category = UpgradeCategory.Miscellaneous,
                UpgradeNameText = getText("Upgrade/IncreasePickUp/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreasePickUp") + 1),
                UpgradeDescriptionText = getText("Upgrade/IncreasePickUp/Description", pickUpRangePercentage * 100),
                ImageName = "UpgradeImage/increase-pick-up",
                MaximumUpgradeCount = 10,
                IsAvailable = playerNumber =>
                {
                  var playerData = StoreManager.Instance.GetState<PlayerState>(StoreNames.PlayerStore)
                    .PlayerDatas[playerNumber];
                  return playerData.PickUpRadius < GeneralConfigurations.MaximumPickRadius;
                },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.Miscellaneous) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var currentPickUp = storeManager.GetState<PlayerState>(StoreNames.PlayerStore)
                    .PlayerDatas[playerNumber]
                    .PickUpRadius;
                  storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_PICK_UP_RADIUS,
                    new PlayerActionData()
                    {
                      PickUpRadius = currentPickUp * (1 + pickUpRangePercentage)
                    });
                }
              },
              new()
              {
                UpgradeId = "IncreaseShootingRange",
                Category = UpgradeCategory.Miscellaneous,
                UpgradeNameText = getText("Upgrade/IncreaseShootingRange/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseShootingRange") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseShootingRange/Description", shootingRangePercentage * 100),
                ImageName = "UpgradeImage/increase-shooting-range",
                MaximumUpgradeCount = 7,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.Miscellaneous) * relationCoefficient,
                IsAvailable = playerNumber =>
                {
                  var playerName = StoreManager.Instance
                    .GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore)
                    .PlayerConfigurations[playerNumber].PlayerName;

                  return playerName != PlayerNames.GUMDAM &&
                         playerName != PlayerNames.JEANNE_D_ARC &&
                         playerName != PlayerNames.RANGER;
                },
                UpgradeReducer = playerNumber =>
                {
                  var skillConfigs = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .BasicSkill
                    .SkillConfigurations;

                  var currentRange = skillConfigs.Range;
                  skillConfigs.Range = currentRange * (1 + shootingRangePercentage);
                }
              },
            };
          },

          #endregion

          #region Projectile

          (_playerNumber, existingUpgrades) =>
          {
            var numberOfAddProjectile = existingUpgrades.Count(u => u.UpgradeId == "AddProjectile");

            var addProjectile = numberOfAddProjectile <= 3 ? numberOfAddProjectile + 1 : 1;
            var dispersionPercentage = MathUtils.GetRandom(
              MathUtils.GetPiecewise(2, 0.55f, 3, 0.175f,
                existingUpgrades.Count(u =>
                  u.UpgradeId is "IncreaseDispersion" or "ReduceDispersion"
                )), 0.1f);
            var increaseFireRatePercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.45f, 4, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseFiringRate")), 0.1f);
            var burstFirePercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 1.5f, 2, 0.3f, existingUpgrades.Count(u => u.UpgradeId == "BurstFire")),
                0.1f);
            var increaseFireRateOnHurtDuration = Random.Range(5, 10f);

            return new()
            {
              new()
              {
                UpgradeId = "AddProjectile",
                Category = UpgradeCategory.Projectile,
                UpgradeNameText = getText("Upgrade/AddProjectile/Name", numberOfAddProjectile + 1),
                UpgradeDescriptionText = getText("Upgrade/AddProjectile/Description", addProjectile),
                ImageName = "UpgradeImage/add-projectile",
                MaximumUpgradeCount = 8,
                Possibility = 0.1f,
                UpgradeReducer = playerNumber =>
                {
                  var basicSkill = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .BasicSkill;

                  basicSkill.SkillCount += addProjectile;
                  basicSkill.SkillConfigurations.Dispersion *= 1.1f;
                },
              },
              new()
              {
                UpgradeId = "IncreaseDispersion",
                Category = UpgradeCategory.Projectile,
                UpgradeNameText = getText("Upgrade/IncreaseDispersion/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseDispersion") + 1),
                UpgradeDescriptionText = getText("Upgrade/IncreaseDispersion/Description", dispersionPercentage * 100),
                ImageName = "UpgradeImage/dispersion_percentage",
                IsAvailable = playerNumber =>
                {
                  var basicSkill = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .BasicSkill;

                  return basicSkill.SkillConfigurations.Dispersion <= 180;
                },
                MaximumUpgradeCount = 5,
                Possibility = 0.1f,
                UpgradeReducer = playerNumber =>
                {
                  var basicSkill = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .BasicSkill;

                  basicSkill.SkillConfigurations.Dispersion *= 1 + dispersionPercentage;
                },
              },
              new()
              {
                UpgradeId = "ReduceDispersion",
                Category = UpgradeCategory.Projectile,
                UpgradeNameText = getText("Upgrade/ReduceDispersion/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "ReduceDispersion") + 1),
                UpgradeDescriptionText = getText("Upgrade/ReduceDispersion/Description", dispersionPercentage * 100),
                ImageName = "UpgradeImage/dispersion_percentage",
                IsAvailable = playerNumber =>
                {
                  var basicSkill = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .BasicSkill;

                  var playerName = StoreManager.Instance
                    .GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore)
                    .PlayerConfigurations[playerNumber].PlayerName;

                  return playerName != PlayerNames.GUMDAM
                    ? basicSkill.SkillConfigurations.Dispersion >= 5
                    : basicSkill.SkillConfigurations.Dispersion >= 0.1f;
                },
                MaximumUpgradeCount = 5,
                Possibility = 0.1f,
                UpgradeReducer = playerNumber =>
                {
                  var basicSkill = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .BasicSkill;

                  basicSkill.SkillConfigurations.Dispersion *= 1 - dispersionPercentage;
                },
              },
              new()
              {
                UpgradeId = "IncreaseFiringRate",
                Category = UpgradeCategory.Projectile,
                UpgradeNameText = getText("Upgrade/IncreaseFiringRate/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseFiringRate") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseFiringRate/Description", increaseFireRatePercentage * 100),
                ImageName = "UpgradeImage/increase-firing-rate",
                MaximumUpgradeCount = 10,
                IsAvailable = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var attackControl = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore)
                    .AttackControlDatas[playerNumber];
                  var coolingTime = attackControl.AttackControlConfigurations.AttackCoolingTime;

                  return coolingTime > GeneralConfigurations.MinimumCoolingTime;
                },
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Projectile) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var attackControl = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore)
                    .AttackControlDatas[playerNumber];
                  var coolingTime = attackControl.AttackControlConfigurations.AttackCoolingTime;

                  storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_COOLING_TIME,
                    new AttackControlActionData()
                    {
                      PlayerNumber = playerNumber,
                      CoolingTime = coolingTime / (1 + increaseFireRatePercentage)
                    });
                }
              },
              new()
              {
                UpgradeId = "BurstFire",
                Category = UpgradeCategory.Projectile,
                UpgradeNameText = getText("Upgrade/BurstFire/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "BurstFire") + 1),
                UpgradeDescriptionText = getText("Upgrade/BurstFire/Description",
                  burstFirePercentage * 100,
                  increaseFireRateOnHurtDuration),
                MaximumUpgradeCount = 3,
                ImageName = "UpgradeImage/increase-firing-rate",
                PrerequisiteUpgradeIds = new() { "Projectile/IncreaseFiringRate" },
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Projectile) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var controller = new IncreaseFiringRateOnHurt(playerNumber, burstFirePercentage,
                    increaseFireRateOnHurtDuration);
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .AddPlayerHurtInterceptor(controller);
                }
              },
            };
          },

          #endregion

          #region Magazine

          (_playerNumber, existingUpgrades) =>
          {
            var numberOfMagazine = existingUpgrades.Count(u => u.UpgradeId == "IncreaseMagazineSize");
            var increaseMagazine = numberOfMagazine <= 6
              ? numberOfMagazine + 1
              : 1;
            var reduceReloadTimePercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.55f, 4, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "ReduceReloadingTime")), 0.1f);

            var burstReloadPercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.65f, 4, 0.35f,
                  existingUpgrades.Count(u => u.UpgradeId == "BurstReload")), 0.1f);
            var burstReloadDuration = Random.Range(10f, 15f);

            return new List<UpgradeReducerContainer>()
            {
              new()
              {
                UpgradeId = "IncreaseMagazineSize",
                Category = UpgradeCategory.Magazine,
                UpgradeNameText = getText("Upgrade/IncreaseMagazineSize/Name", numberOfMagazine + 1),
                UpgradeDescriptionText = getText("Upgrade/IncreaseMagazineSize/Description", increaseMagazine),
                ImageName = "UpgradeImage/increase-magazine-size",
                MaximumUpgradeCount = 6,
                IsAvailable = playerNumber =>
                {
                  var playerName = StoreManager.Instance
                    .GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore)
                    .PlayerConfigurations[playerNumber].PlayerName;
                  return playerName != PlayerNames.JEANNE_D_ARC;
                },
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Magazine) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var attackControl = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore)
                    .AttackControlDatas[playerNumber];
                  var attackPerRound = attackControl.AttackControlConfigurations.AttackPerRound;
                  storeManager.Commit(StoreNames.AttackControlStore,
                    StoreActions.AttackControlStore_SET_ATTACK_PER_ROUND,
                    new AttackControlActionData()
                    {
                      PlayerNumber = playerNumber,
                      AttackPerRound = attackPerRound + increaseMagazine,
                    });
                }
              },
              new()
              {
                UpgradeId = "ReduceReloadingTime",
                Category = UpgradeCategory.Magazine,
                UpgradeNameText = getText("Upgrade/ReduceReloadingTime/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "ReduceReloadingTime") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/ReduceReloadingTime/Description", reduceReloadTimePercentage * 100),
                ImageName = "UpgradeImage/reduce-reload-time",
                MaximumUpgradeCount = 7,
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Magazine) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var attackControl = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore)
                    .AttackControlDatas[playerNumber];
                  var reloadTime = attackControl.AttackControlConfigurations.ReloadTime;
                  storeManager.Commit(StoreNames.AttackControlStore, StoreActions.AttackControlStore_SET_RELOAD_TIME,
                    new AttackControlActionData()
                    {
                      PlayerNumber = playerNumber,
                      ReloadTime = reloadTime * (1 - reduceReloadTimePercentage),
                    });
                }
              },
              new()
              {
                UpgradeId = "BurstReload",
                Category = UpgradeCategory.Magazine,
                UpgradeNameText = getText("Upgrade/BurstReload/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "BurstReload") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/BurstReload/Description", burstReloadPercentage * 100, burstReloadDuration),
                ImageName = "UpgradeImage/reduce-reload-time",
                MaximumUpgradeCount = 3,
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Magazine) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var controller = new BurstReloadOnHurt(playerNumber, burstReloadPercentage, burstReloadDuration);
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .AddPlayerHurtInterceptor(controller);
                }
              },
            };
          },

          #endregion

          #region Base Skill

          (_playerNumber, existingUpgrades) =>
          {
            var increaseHurtPercentage =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.35f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseHurtPercentage")), 0.1f);
            var increaseAeDurationPercentage =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.35f, 5, 0.05f, existingUpgrades.Count(u =>
                  u.UpgradeId == "IncreaseIceAeDuration" ||
                  u.UpgradeId == "IncreaseFireAeDuration" ||
                  u.UpgradeId == "IncreaseThunderAeDuration")
                ), 0.1f);

            return new List<UpgradeReducerContainer>()
            {
              new()
              {
                UpgradeId = "IncreaseHurtPercentage",
                Category = UpgradeCategory.BaseSkill,
                UpgradeNameText = getText("Upgrade/IncreaseHurtPercentage/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseHurtPercentage") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseHurtPercentage/Description", increaseHurtPercentage * 100),
                ImageName = "UpgradeImage/increase-base-hurt",
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.BaseSkill) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var skillConfigs = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .BasicSkill
                    .SkillConfigurations;

                  var skillHurt = skillConfigs.SkillHurt;
                  skillHurt.HurtPoint *= 1 + increaseHurtPercentage;
                  skillConfigs.SkillHurt = skillHurt;
                }
              },
              new()
              {
                UpgradeId = "IncreaseIceAeDuration",
                Category = UpgradeCategory.BaseSkill,
                IsAvailable = playerNumber => SkillUtils.HasAdditionalEffect(playerNumber) &&
                                              SkillUtils.IsBasicAdditionalEffectTypeGenerator(BasicAdditionalEffectTypes
                                                .FREEZE)(playerNumber),
                UpgradeNameText = getText("Upgrade/IncreaseIceAeDuration/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseIceAeDuration") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseIceAeDuration/Description", increaseAeDurationPercentage * 100),
                ImageName = "UpgradeImage/increase-ice-ae-duration",
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.BaseSkill) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var ae = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .BasicSkill
                    .SkillConfigurations
                    .AdditionalEffect;

                  var iceAe = (IceAdditionalEffect)ae.BasicAdditionalEffect;
                  iceAe.LastForSeconds *= 1 + increaseAeDurationPercentage;
                  ae.BasicAdditionalEffect = iceAe;
                }
              },
              new()
              {
                UpgradeId = "IncreaseFireAeDuration",
                Category = UpgradeCategory.BaseSkill,
                IsAvailable = playerNumber => SkillUtils.HasAdditionalEffect(playerNumber) &&
                                              SkillUtils.IsBasicAdditionalEffectTypeGenerator(BasicAdditionalEffectTypes
                                                .BURN)(playerNumber),
                UpgradeNameText = getText("Upgrade/IncreaseFireAeDuration/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseFireAeDuration") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseFireAeDuration/Description", increaseAeDurationPercentage * 100),
                ImageName = "UpgradeImage/increase-fire-ae-duration",
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.BaseSkill) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var ae = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .BasicSkill
                    .SkillConfigurations
                    .AdditionalEffect;

                  var fireAe = (FireAdditionalEffect)ae.BasicAdditionalEffect;
                  fireAe.LastForSeconds *= 1 + increaseAeDurationPercentage;
                  ae.BasicAdditionalEffect = fireAe;
                }
              },
              new()
              {
                UpgradeId = "IncreaseThunderAeDuration",
                Category = UpgradeCategory.BaseSkill,
                IsAvailable = playerNumber => SkillUtils.HasAdditionalEffect(playerNumber) &&
                                              SkillUtils.IsBasicAdditionalEffectTypeGenerator(BasicAdditionalEffectTypes
                                                .STUN)(playerNumber),
                UpgradeNameText = getText("Upgrade/IncreaseThunderAeDuration/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseThunderAeDuration") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseThunderAeDuration/Description", increaseAeDurationPercentage * 100),
                ImageName = "UpgradeImage/increase-thunder-ae-duration",
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.BaseSkill) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var ae = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .BasicSkill
                    .SkillConfigurations
                    .AdditionalEffect;

                  var thunderAe = (ThunderAdditionalEffect)ae.BasicAdditionalEffect;
                  thunderAe.StunningSeconds *= 1 + increaseAeDurationPercentage;
                  ae.BasicAdditionalEffect = thunderAe;
                }
              },
            };
          },

          #endregion

          #region Surround Skill (Dart, Boomerang)

          // Dart
          (_playerNumber, existingUpgrades) =>
          {
            var numberOfAddDart = existingUpgrades.Count(u => u.UpgradeId == "AddDart");

            var addDart = numberOfAddDart <= 5 ? numberOfAddDart + 1 : 1;
            var poisonPossibility =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.5f, 5, 0.05f,
                  existingUpgrades.Count(u => u.UpgradeId == "PoisonDart")), 0.1f);
            var poisonHurtPercentage =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.15f, 5, 0.01f,
                  existingUpgrades.Count(u => u.UpgradeId == "PoisonDart")), 0.1f);
            var increaseHurtPercentage =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.35f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "ImproveDartHurt")), 0.1f);

            return new List<UpgradeReducerContainer>()
            {
              new()
              {
                UpgradeId = "ActivateDart",
                Category = UpgradeCategory.SurroundSkill,
                UpgradeNameText = getText("Upgrade/ActivateDart/Name"),
                UpgradeDescriptionText = getText("Upgrade/ActivateDart/Description"),
                ImageName = "UpgradeImage/activate-dart-surround",
                MaximumUpgradeCount = 1,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("Dart")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var dartConfigs = SkillPresets.PlayerSkills["DartSurroundSkill"];
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .AddSurroundSkill(SurroundSkillGroups.DART, 1.5f, "DartSurroundSkill", dartConfigs.SkillPrefabName,
                      dartConfigs.SkillConfigurations);
                }
              },
              new()
              {
                UpgradeId = "AddDart",
                Category = UpgradeCategory.SurroundSkill,
                IsAvailable = playerNumber =>
                {
                  var skillAndUpgrade = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber];

                  return skillAndUpgrade
                    .SurroundSkills
                    .Count <= GeneralConfigurations.MaximumSurroundSkills;
                },
                MaximumUpgradeCount = 5,
                UpgradeNameText = getText("Upgrade/AddDart/Name", numberOfAddDart + 1),
                UpgradeDescriptionIndicator = getText("Upgrade/AddDart/Description", addDart),
                ImageName = "UpgradeImage/add-dart-surround",
                PrerequisiteUpgradeIds = new List<string> { "ActivateDart" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("Dart")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var manager = SkillAndUpgradeManager.Instance;

                  for (var i = 0; i < addDart; i++)
                  {
                    var dartConfigs = SkillPresets.PlayerSkills["DartSurroundSkill"].Clone();
                    manager.SkillAndUpgradeControllers[playerNumber]
                      .AddSurroundSkill(SurroundSkillGroups.DART, 1.5f, "DartSurroundSkill",
                        dartConfigs.SkillPrefabName,
                        dartConfigs.SkillConfigurations);
                  }
                }
              },
              new()
              {
                UpgradeId = "PoisonDart",
                Category = UpgradeCategory.SurroundSkill,
                UpgradeNameText = getText("Upgrade/PoisonDart/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "PoisonDart") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/PoisonDart/Description", poisonPossibility, poisonHurtPercentage * 100),
                ImageName = "UpgradeImage/activate-dart-surround",
                PrerequisiteUpgradeIds = new List<string> { "ActivateDart" },
                MaximumUpgradeCount = 6,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("Dart")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .UpgradeSurroundSkill(ssc =>
                    {
                      if (ssc.SkillId != "DartSurroundSkill") return ssc;

                      var skillHurt = ssc.SkillConfigurations.SkillHurt;
                      skillHurt.HurtType = HurtTypes.POISON;
                      ssc.SkillConfigurations.SkillHurt = skillHurt;

                      ssc.SkillConfigurations.AdditionalEffect.PoisonAdditionalEffect = new PoisonAdditionalEffect
                      {
                        Enable = true,
                        Possibility = poisonPossibility,
                        HurtPercentagePerSecond = poisonHurtPercentage,
                      };

                      return ssc;
                    });
                }
              },
              new()
              {
                UpgradeId = "ImproveDartHurt",
                Category = UpgradeCategory.SurroundSkill,
                UpgradeNameText = getText("Upgrade/ImproveDartHurt/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "ImproveDartHurt") + 1),
                UpgradeDescriptionText = getText("Upgrade/ImproveDartHurt/Description", increaseHurtPercentage * 100),
                ImageName = "UpgradeImage/activate-dart-surround",
                PrerequisiteUpgradeIds = new List<string> { "ActivateDart" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.SurroundSkill) * relationCoefficient,
                MaximumUpgradeCount = 6,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .UpgradeSurroundSkill(ssc =>
                    {
                      if (ssc.SkillId != "DartSurroundSkill") return ssc;

                      var skillHurt = ssc.SkillConfigurations.SkillHurt;
                      skillHurt.HurtPoint *= 1 + increaseHurtPercentage;

                      ssc.SkillConfigurations.SkillHurt = skillHurt;
                      return ssc;
                    });
                }
              },
            };
          },
          // Boomerang
          (_playerNumber, existingUpgrades) =>
          {
            var numberOfAddBoomerang = existingUpgrades.Count(u => u.UpgradeId == "AddBoomerang");
            var addBoomerang = numberOfAddBoomerang <= 5 ? numberOfAddBoomerang + 1 : 1;

            var burnPossibility =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.6f, 5, 0.05f,
                  existingUpgrades.Count(u => u.UpgradeId == "BurnBoomerang")), 0.1f);
            var burnHurtPercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.2f, 5, 0.01f,
                  existingUpgrades.Count(u => u.UpgradeId == "BurnBoomerang")), 0.1f);
            var burnDuration =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 10, 10, 2, existingUpgrades.Count(u => u.UpgradeId == "BurnBoomerang")),
                0.1f);

            var increaseSpeedPercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.35f, 5, 0.05f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseBoomerangSpeed")), 0.1f);

            var increaseHurtPercentage =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.35f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "ImproveBoomerangHurt")), 0.1f);

            return new List<UpgradeReducerContainer>
            {
              new()
              {
                UpgradeId = "ActivateBoomerang",
                Category = UpgradeCategory.SurroundSkill,
                UpgradeNameText = getText("Upgrade/ActivateBoomerang/Name"),
                UpgradeDescriptionText = getText("Upgrade/ActivateBoomerang/Description"),
                ImageName = "UpgradeImage/activate-boomerang-surround",
                MaximumUpgradeCount = 1,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("Boomerang")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var boomerangConfigs = SkillPresets.PlayerSkills["BoomerangSurroundSkill"];
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .AddSurroundSkill(SurroundSkillGroups.BOOMERANG, 1, "BoomerangSurroundSkill",
                      boomerangConfigs.SkillPrefabName, boomerangConfigs.SkillConfigurations);
                }
              },
              new()
              {
                UpgradeId = "AddBoomerang",
                Category = UpgradeCategory.SurroundSkill,
                IsAvailable = playerNumber =>
                {
                  var skillAndUpgrade = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber];

                  return skillAndUpgrade
                    .SurroundSkills
                    .Count <= GeneralConfigurations.MaximumSurroundSkills;
                },
                UpgradeNameText = getText("Upgrade/AddBoomerang/Name", numberOfAddBoomerang + 1),
                UpgradeDescriptionText = getText("Upgrade/AddBoomerang/Description", addBoomerang),
                ImageName = "UpgradeImage/add-boomerang-surround",
                PrerequisiteUpgradeIds = new List<string> { "ActivateBoomerang" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("Boomerang")) * relationCoefficient,
                MaximumUpgradeCount = 5,
                UpgradeReducer = playerNumber =>
                {
                  var manager = SkillAndUpgradeManager.Instance;

                  for (var i = 0; i < addBoomerang; i++)
                  {
                    var dartConfigs = SkillPresets.PlayerSkills["BoomerangSurroundSkill"];
                    manager.SkillAndUpgradeControllers[playerNumber]
                      .AddSurroundSkill(SurroundSkillGroups.BOOMERANG, 1, "BoomerangSurroundSkill",
                        dartConfigs.SkillPrefabName,
                        dartConfigs.SkillConfigurations);
                  }

                  manager.SkillAndUpgradeControllers[playerNumber]
                    .UpgradeSurroundSkill(ssc =>
                    {
                      if (ssc.SkillId == "BoomerangSurroundSkill")
                      {
                        ssc.SkillObject.GetComponent<Boomerang>().Speed *= 1.05f;
                      }

                      return ssc;
                    });
                }
              },
              new()
              {
                UpgradeId = "BurnBoomerang",
                Category = UpgradeCategory.SurroundSkill,
                UpgradeNameText = getText("Upgrade/BurnBoomerang/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "BurnBoomerang") + 1),
                UpgradeDescriptionText = getText("Upgrade/BurnBoomerang/Description", burnPossibility * 100,
                  burnDuration,
                  burnHurtPercentage * 100),
                ImageName = "UpgradeImage/activate-boomerang-surround",
                PrerequisiteUpgradeIds = new() { "ActivateBoomerang" },
                MaximumUpgradeCount = 6,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("Boomerang")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .UpgradeSurroundSkill(ssc =>
                    {
                      if (ssc.SkillId == "BoomerangSurroundSkill")
                      {
                        ssc.SkillConfigurations.AdditionalEffect.BasicAdditionalEffect = new FireAdditionalEffect
                        {
                          Possibility = burnPossibility,
                          HurtPercentagePerSecond = burnHurtPercentage,
                          LastForSeconds = burnDuration,
                        };

                        ssc.SkillConfigurations.AdditionalEffect.BasicAdditionalEffectType =
                          BasicAdditionalEffectTypes.BURN;
                      }

                      return ssc;
                    });
                }
              },
              new()
              {
                UpgradeId = "IncreaseBoomerangSpeed",
                Category = UpgradeCategory.SurroundSkill,
                UpgradeNameText = getText("Upgrade/IncreaseBoomerangSpeed/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseBoomerangSpeed") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseBoomerangSpeed/Description", increaseSpeedPercentage * 100),
                ImageName = "UpgradeImage/activate-boomerang-surround",
                PrerequisiteUpgradeIds = new() { "ActivateBoomerang" },
                MaximumUpgradeCount = 6,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("Boomerang")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .UpgradeSurroundSkill(ssc =>
                    {
                      if (ssc.SkillId == "BoomerangSurroundSkill")
                      {
                        ssc.SkillObject.GetComponent<Boomerang>().Speed *= increaseSpeedPercentage;
                      }

                      return ssc;
                    });
                }
              },
              new()
              {
                UpgradeId = "ImproveBoomerangHurt",
                Category = UpgradeCategory.SurroundSkill,
                UpgradeNameText = getText("Upgrade/ImproveBoomerangHurt/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "ImproveBoomerangHurt") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/ImproveBoomerangHurt/Description", increaseHurtPercentage * 100),
                ImageName = "UpgradeImage/add-boomerang-surround",
                PrerequisiteUpgradeIds = new() { "ActivateBoomerang" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.SurroundSkill) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .UpgradeSurroundSkill(ssc =>
                    {
                      if (ssc.SkillId == "BoomerangSurroundSkill")
                      {
                        var skillHurt = ssc.SkillConfigurations.SkillHurt;
                        skillHurt.HurtPoint *= 1 + increaseHurtPercentage;
                        ssc.SkillConfigurations.SkillHurt = skillHurt;
                      }

                      return ssc;
                    });
                }
              },
            };
          },
          // Ice Tower
          (_playerNumber, existingUpgrades) =>
          {
            const float distance = 2.5f;

            var numberOfAddTower = existingUpgrades.Count(u => u.UpgradeId == "AddIceTower");

            var addTower = numberOfAddTower <= 5 ? numberOfAddTower + 1 : 1;

            var increaseFiringRatePercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.45f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseIceTowerFiringRate")), 0.1f);

            var increaseHurtPercentage =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.35f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "ImproveIceTowerHurt")), 0.1f);

            return new List<UpgradeReducerContainer>()
            {
              new()
              {
                UpgradeId = "ActivateIceTower",
                Category = UpgradeCategory.SurroundSkill,
                NoTheseUpgradeIds = new List<string> { "ActivateThunderTower", "ActivateFireTower" },
                IsAvailable = playerNumber =>
                {
                  var skillAndUpgrade = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber];

                  var quantityLimit = skillAndUpgrade
                    .SurroundSkills
                    .Count <= GeneralConfigurations.MaximumSurroundSkills;

                  var isIce = skillAndUpgrade
                    .BasicSkill
                    .SkillConfigurations
                    .SkillHurt
                    .HurtType == HurtTypes.MAGIC_ICE;

                  return !isIce && quantityLimit;
                },
                UpgradeNameText = getText("Upgrade/ActivateIceTower/Name"),
                UpgradeDescriptionText = getText("Upgrade/ActivateIceTower/Description"),
                ImageName = "UpgradeImage/activate-ice-tower",
                MaximumUpgradeCount = 1,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("IceTower")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var iceTowerBulletConfigs = SkillPresets.PlayerSkills["IceTowerBullet"].SkillConfigurations;
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .AddSurroundSkill(SurroundSkillGroups.ICE_TOWER, distance, "IceTower",
                      "Skill/Player/SurroundSkill/IceTower",
                      iceTowerBulletConfigs);
                }
              },
              new()
              {
                UpgradeId = "AddIceTower",
                Category = UpgradeCategory.SurroundSkill,
                IsAvailable = playerNumber =>
                {
                  var skillAndUpgrade = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber];

                  var quantityLimit = skillAndUpgrade
                    .SurroundSkills
                    .Count <= GeneralConfigurations.MaximumSurroundSkills;

                  var isIce = skillAndUpgrade
                    .BasicSkill
                    .SkillConfigurations
                    .SkillHurt
                    .HurtType == HurtTypes.MAGIC_ICE;

                  return !isIce && quantityLimit;
                },
                UpgradeNameText = getText("Upgrade/AddIceTower/Name", numberOfAddTower + 1),
                UpgradeDescriptionText = getText("Upgrade/AddIceTower/Description", addTower),
                ImageName = "UpgradeImage/activate-ice-tower",
                PrerequisiteUpgradeIds = new List<string> { "ActivateIceTower" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("IceTower")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  for (var i = 0; i < addTower; i++)
                  {
                    var iceTowerBulletConfigs = SkillPresets.PlayerSkills["IceTowerBullet"].SkillConfigurations;
                    SkillAndUpgradeManager
                      .Instance
                      .SkillAndUpgradeControllers[playerNumber]
                      .AddSurroundSkill(SurroundSkillGroups.ICE_TOWER, distance, "IceTower",
                        "Skill/Player/SurroundSkill/IceTower",
                        iceTowerBulletConfigs);
                  }
                }
              },
              new()
              {
                UpgradeId = "IncreaseIceTowerFiringRate",
                Category = UpgradeCategory.SurroundSkill,
                PrerequisiteUpgradeIds = new List<string> { "ActivateIceTower", },
                UpgradeNameText = getText("Upgrade/IncreaseIceTowerFiringRate/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseIceTowerFiringRate") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseIceTowerFiringRate/Description", increaseFiringRatePercentage * 100),
                ImageName = "UpgradeImage/activate-ice-tower",
                MaximumUpgradeCount = 5,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("IceTower")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .UpgradeSurroundSkill(ssc =>
                    {
                      if (ssc.SkillId == "IceTower")
                      {
                        ssc.SkillObject.GetComponent<IceTower>().FireInterval *= 1 / (1 + increaseFiringRatePercentage);
                      }

                      return ssc;
                    });
                }
              },
              new()
              {
                UpgradeId = "ImproveIceTowerHurt",
                Category = UpgradeCategory.SurroundSkill,
                UpgradeNameText = getText("Upgrade/ImproveIceTowerHurt/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "ImproveIceTowerHurt") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/ImproveIceTowerHurt/Description", increaseHurtPercentage * 100),
                ImageName = "UpgradeImage/activate-ice-tower",
                PrerequisiteUpgradeIds = new() { "ActivateIceTower" },
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .UpgradeSurroundSkill(ssc =>
                    {
                      if (ssc.SkillId == "IceTower")
                      {
                        var skillHurt = ssc.SkillConfigurations.SkillHurt;
                        skillHurt.HurtPoint *= 1 + increaseHurtPercentage;
                        ssc.SkillConfigurations.SkillHurt = skillHurt;
                      }

                      return ssc;
                    });
                }
              },
            };
          },
          // Fire Tower
          (_playerNumber, existingUpgrades) =>
          {
            const float distance = 2.5f;

            var numberOfAddTower = existingUpgrades.Count(u => u.UpgradeId == "AddFireTower");

            var addTower = numberOfAddTower <= 5 ? numberOfAddTower + 1 : 1;

            var increaseFiringRatePercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.45f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseFireTowerFiringRate")), 0.1f);

            var increaseHurtPercentage =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.35f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "ImproveFireTowerHurt")), 0.1f);

            return new List<UpgradeReducerContainer>()
            {
              new()
              {
                UpgradeId = "ActivateFireTower",
                Category = UpgradeCategory.SurroundSkill,
                NoTheseUpgradeIds = new List<string> { "ActivateThunderTower", "ActivateFireTower" },
                IsAvailable = playerNumber =>
                {
                  var skillAndUpgrade = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber];

                  var quantityLimit = skillAndUpgrade
                    .SurroundSkills
                    .Count <= GeneralConfigurations.MaximumSurroundSkills;

                  var isFire = skillAndUpgrade
                    .BasicSkill
                    .SkillConfigurations
                    .SkillHurt
                    .HurtType == HurtTypes.MAGIC_FIRE;

                  return !isFire && quantityLimit;
                },
                UpgradeNameText = getText("Upgrade/ActivateFireTower/Name"),
                UpgradeDescriptionText = getText("Upgrade/ActivateFireTower/Description"),
                ImageName = "UpgradeImage/activate-fire-tower",
                MaximumUpgradeCount = 1,
                UpgradeReducer = playerNumber =>
                {
                  var fireTowerBulletConfigs = SkillPresets.PlayerSkills["FireTowerBullet"].SkillConfigurations;
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .AddSurroundSkill(SurroundSkillGroups.FIRE_TOWER, distance, "FireTower",
                      "Skill/Player/SurroundSkill/FireTower",
                      fireTowerBulletConfigs);
                }
              },
              new()
              {
                UpgradeId = "AddFireTower",
                Category = UpgradeCategory.SurroundSkill,
                IsAvailable = playerNumber =>
                {
                  var skillAndUpgrade = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber];

                  var quantityLimit = skillAndUpgrade
                    .SurroundSkills
                    .Count <= GeneralConfigurations.MaximumSurroundSkills;

                  var isFire = skillAndUpgrade
                    .BasicSkill
                    .SkillConfigurations
                    .SkillHurt
                    .HurtType == HurtTypes.MAGIC_FIRE;

                  return !isFire && quantityLimit;
                },
                UpgradeNameText = getText("Upgrade/AddFireTower/Name", numberOfAddTower),
                UpgradeDescriptionText = getText("Upgrade/AddFireTower/Description", addTower),
                ImageName = "UpgradeImage/activate-fire-tower",
                PrerequisiteUpgradeIds = new List<string> { "ActivateFireTower" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("IceTower")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  for (var i = 0; i < addTower; i++)
                  {
                    var fireTowerBulletConfigs = SkillPresets.PlayerSkills["FireTowerBullet"].SkillConfigurations;
                    SkillAndUpgradeManager
                      .Instance
                      .SkillAndUpgradeControllers[playerNumber]
                      .AddSurroundSkill(SurroundSkillGroups.FIRE_TOWER, distance, "FireTower",
                        "Skill/Player/SurroundSkill/FireTower",
                        fireTowerBulletConfigs);
                  }
                }
              },
              new()
              {
                UpgradeId = "IncreaseFireTowerFiringRate",
                Category = UpgradeCategory.SurroundSkill,
                PrerequisiteUpgradeIds = new List<string> { "ActivateFireTower", },
                UpgradeNameText = getText("Upgrade/IncreaseFireTowerFiringRate/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseFireTowerFiringRate") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseFireTowerFiringRate/Description", increaseFiringRatePercentage * 100),
                ImageName = "UpgradeImage/activate-fire-tower",
                MaximumUpgradeCount = 5,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("IceTower")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .UpgradeSurroundSkill(ssc =>
                    {
                      if (ssc.SkillId == "FireTower")
                      {
                        ssc.SkillObject.GetComponent<FireTower>().FireInterval *=
                          1 / (1 + increaseFiringRatePercentage);
                      }

                      return ssc;
                    });
                }
              },
              new()
              {
                UpgradeId = "ImproveFireTowerHurt",
                Category = UpgradeCategory.SurroundSkill,
                UpgradeNameText = getText("Upgrade/ImproveFireTowerHurt/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "ImproveFireTowerHurt") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/ImproveFireTowerHurt/Description", increaseHurtPercentage * 100),
                ImageName = "UpgradeImage/activate-fire-tower",
                PrerequisiteUpgradeIds = new() { "ActivateFireTower" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.SurroundSkill) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .UpgradeSurroundSkill(ssc =>
                    {
                      if (ssc.SkillId == "FireTower")
                      {
                        var skillHurt = ssc.SkillConfigurations.SkillHurt;
                        skillHurt.HurtPoint *= 1 + increaseHurtPercentage;
                        ssc.SkillConfigurations.SkillHurt = skillHurt;
                      }

                      return ssc;
                    });
                }
              },
            };
          },
          // Thunder Tower
          (_playerNumber, existingUpgrades) =>
          {
            const float distance = 2.5f;

            var numberOfAddTower = existingUpgrades.Count(u => u.UpgradeId == "AddThunderTower");

            var addTower = numberOfAddTower <= 5 ? numberOfAddTower + 1 : 1;

            var increaseFiringRatePercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.45f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseThunderTowerFiringRate")), 0.1f);

            var increaseHurtPercentage =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.35f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "ImproveThunderTowerHurt")), 0.1f);

            return new List<UpgradeReducerContainer>()
            {
              new()
              {
                UpgradeId = "ActivateThunderTower",
                NoTheseUpgradeIds = new List<string> { "ActivateIceTower", "ActivateFireTower" },
                Category = UpgradeCategory.SurroundSkill,
                IsAvailable = playerNumber =>
                {
                  var skillAndUpgrade = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber];

                  var quantityLimit = skillAndUpgrade
                    .SurroundSkills
                    .Count <= GeneralConfigurations.MaximumSurroundSkills;

                  var isThunder = skillAndUpgrade
                    .BasicSkill
                    .SkillConfigurations
                    .SkillHurt
                    .HurtType == HurtTypes.MAGIC_THUNDER;

                  return !isThunder && quantityLimit;
                },
                UpgradeNameText = getText("Upgrade/ActivateThunderTower/Name"),
                UpgradeDescriptionText = getText("Upgrade/ActivateThunderTower/Description"),
                ImageName = "UpgradeImage/activate-thunder-tower",
                MaximumUpgradeCount = 1,
                UpgradeReducer = playerNumber =>
                {
                  var thunderTowerBulletConfigs = SkillPresets.PlayerSkills["ThunderTowerBullet"].SkillConfigurations;
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .AddSurroundSkill(SurroundSkillGroups.THUNDER_TOWER, distance, "ThunderTower",
                      "Skill/Player/SurroundSkill/ThunderTower",
                      thunderTowerBulletConfigs);
                }
              },
              new()
              {
                UpgradeId = "AddThunderTower",
                Category = UpgradeCategory.SurroundSkill,
                IsAvailable = playerNumber =>
                {
                  var skillAndUpgrade = SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber];

                  var quantityLimit = skillAndUpgrade
                    .SurroundSkills
                    .Count <= GeneralConfigurations.MaximumSurroundSkills;

                  var isThunder = skillAndUpgrade
                    .BasicSkill
                    .SkillConfigurations
                    .SkillHurt
                    .HurtType == HurtTypes.MAGIC_THUNDER;

                  return !isThunder && quantityLimit;
                },
                UpgradeNameText = getText("Upgrade/AddThunderTower/Name", numberOfAddTower + 1),
                UpgradeDescriptionText = getText("Upgrade/AddThunderTower/Description", addTower),
                ImageName = "UpgradeImage/activate-thunder-tower",
                PrerequisiteUpgradeIds = new List<string> { "ActivateThunderTower" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("ThunderTower")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  for (var i = 0; i < addTower; i++)
                  {
                    var thunderTowerBulletConfigs = SkillPresets.PlayerSkills["ThunderTowerBullet"].SkillConfigurations;
                    SkillAndUpgradeManager
                      .Instance
                      .SkillAndUpgradeControllers[playerNumber]
                      .AddSurroundSkill(SurroundSkillGroups.THUNDER_TOWER, distance, "ThunderTower",
                        "Skill/Player/SurroundSkill/ThunderTower",
                        thunderTowerBulletConfigs);
                  }
                }
              },
              new()
              {
                UpgradeId = "IncreaseThunderTowerFiringRate",
                Category = UpgradeCategory.SurroundSkill,
                PrerequisiteUpgradeIds = new List<string> { "ActivateThunderTower", },
                UpgradeNameText = getText("Upgrade/IncreaseThunderTowerFiringRate/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseThunderTowerFiringRate") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseThunderTowerFiringRate/Description", increaseFiringRatePercentage * 100),
                ImageName = "UpgradeImage/activate-thunder-tower",
                MaximumUpgradeCount = 5,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("ThunderTower")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .UpgradeSurroundSkill(ssc =>
                    {
                      if (ssc.SkillId == "ThunderTower")
                      {
                        ssc.SkillObject.GetComponent<ThunderTower>().FireInterval *=
                          1 / (1 + increaseFiringRatePercentage);
                      }

                      return ssc;
                    });
                }
              },
              new()
              {
                UpgradeId = "ImproveThunderTowerHurt",
                Category = UpgradeCategory.SurroundSkill,
                UpgradeNameText = getText("Upgrade/ImproveThunderTowerHurt/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "ImproveThunderTowerHurt") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/ImproveThunderTowerHurt/Description", increaseHurtPercentage * 100),
                ImageName = "UpgradeImage/activate-thunder-tower",
                PrerequisiteUpgradeIds = new() { "ActivateThunderTower" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.UpgradeId.Contains("ThunderTower")) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .UpgradeSurroundSkill(ssc =>
                    {
                      if (ssc.SkillId == "ThunderTower")
                      {
                        var skillHurt = ssc.SkillConfigurations.SkillHurt;
                        skillHurt.HurtPoint *= 1 + increaseHurtPercentage;
                        ssc.SkillConfigurations.SkillHurt = skillHurt;
                      }

                      return ssc;
                    });
                }
              },
            };
          },

          #endregion

          #region Spirals

          (_playerNumber, existingUpgrades) =>
          {
            const int minInterval = 5;

            var numberOfAddSpiral = existingUpgrades.Count(u => u.UpgradeId == "AddSpiral");
            var addSpiral = numberOfAddSpiral <= 5 ? numberOfAddSpiral + 1 : 1;

            var reduceIntervalPercentage =
              MathUtils.GetRandom(
                MathUtils.GetPiecewise(1.5f, 0.25f, 4, 0.05f,
                  existingUpgrades.Count(u => u.UpgradeId == "ReduceSpiralInterval")), 0.1f);

            var increaseSpiralHurtPercentage =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.35f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseSpiralHurt")), 0.1f);

            return new List<UpgradeReducerContainer>()
            {
              new()
              {
                UpgradeId = "ActivateSpiral",
                Category = UpgradeCategory.SpiralSkill,
                UpgradeNameText = getText("Upgrade/ActivateSpiral/Name"),
                UpgradeDescriptionText = getText("Upgrade/ActivateSpiral/Description"),
                ImageName = "UpgradeImage/activate-spiral",
                MaximumUpgradeCount = 1,
                UpgradeReducer = playerNumber =>
                {
                  var spiral = SkillPresets.PlayerTimingSkills["Spiral"];
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .SetTimingSkill("Spiral", spiral.SkillPrefabName, 1, spiral.DefaultInterval,
                      "UpgradeImage/activate-spiral",
                      spiral.SkillConfigurations);
                }
              },
              new()
              {
                UpgradeId = "AddSpiral",
                Category = UpgradeCategory.SpiralSkill,
                UpgradeNameText = getText("Upgrade/AddSpiral/Name", numberOfAddSpiral + 1),
                UpgradeDescriptionText = getText("Upgrade/AddSpiral/Description", addSpiral),
                ImageName = "UpgradeImage/add-spiral",
                MaximumUpgradeCount = 5,
                PrerequisiteUpgradeIds = new List<string> { "ActivateSpiral" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.SpiralSkill) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentSpiral = timingSkills.FirstOrDefault(container => container.SkillId == "Spiral");

                  if (currentSpiral != null)
                  {
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentSpiral.SkillId,
                      });

                    var interval = currentSpiral.Interval;
                    var quantity = currentSpiral.SkillCount;
                    var skillConfigs = currentSpiral.SkillConfigurations;

                    skillAndUpgradeController.SetTimingSkill("Spiral", currentSpiral.SkillPrefabName,
                      quantity + addSpiral,
                      interval,
                      "UpgradeImage/activate-spiral", skillConfigs);
                  }
                }
              },
              new()
              {
                UpgradeId = "ReduceSpiralInterval",
                Category = UpgradeCategory.SpiralSkill,
                UpgradeNameText = getText("Upgrade/ReduceSpiralInterval/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "ReduceSpiralInterval") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/ReduceSpiralInterval/Description", reduceIntervalPercentage * 100),
                ImageName = "UpgradeImage/activate-spiral",
                MaximumUpgradeCount = 5,
                IsAvailable = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentSpiral = timingSkills.FirstOrDefault(container => container.SkillId == "Spiral");

                  return currentSpiral == null || currentSpiral.Interval > minInterval;
                },
                PrerequisiteUpgradeIds = new() { "ActivateSpiral" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.SpiralSkill) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentSpiral = timingSkills.FirstOrDefault(container => container.SkillId == "Spiral");

                  if (currentSpiral != null)
                  {
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData()
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentSpiral.SkillId,
                      });

                    var interval = currentSpiral.Interval * (1 - reduceIntervalPercentage);
                    var quantity = currentSpiral.SkillCount;
                    var skillConfigs = currentSpiral.SkillConfigurations;

                    var newInterval = Mathf.Max(interval, minInterval);

                    skillAndUpgradeController.SetTimingSkill("Spiral", currentSpiral.SkillPrefabName, quantity,
                      newInterval,
                      "UpgradeImage/activate-spiral", skillConfigs);
                  }
                }
              },
              new()
              {
                UpgradeId = "IncreaseSpiralHurt",
                Category = UpgradeCategory.SpiralSkill,
                UpgradeNameText = getText("Upgrade/IncreaseSpiralHurt/Name",
                  existingUpgrades.Count(u => u.UpgradeId == "IncreaseSpiralHurt") + 1),
                UpgradeDescriptionText =
                  getText("Upgrade/IncreaseSpiralHurt/Description", increaseSpiralHurtPercentage * 100),
                ImageName = "UpgradeImage/activate-spiral",
                PrerequisiteUpgradeIds = new() { "ActivateSpiral" },
                MaximumUpgradeCount = 5,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.SpiralSkill) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentSpiral = timingSkills.FirstOrDefault(container => container.SkillId == "Spiral");

                  if (currentSpiral != null)
                  {
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData()
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentSpiral.SkillId,
                      });

                    var interval = currentSpiral.Interval;
                    var quantity = currentSpiral.SkillCount;
                    var skillConfigs = (BasicSkillConfigurations)currentSpiral.SkillConfigurations.Clone();

                    skillConfigs.SkillHurt = new SkillHurt
                    {
                      HurtPoint = skillConfigs.SkillHurt.HurtPoint * (1 + increaseSpiralHurtPercentage),
                      HurtType = skillConfigs.SkillHurt.HurtType,
                    };

                    skillAndUpgradeController.SetTimingSkill("Spiral", currentSpiral.SkillPrefabName, quantity,
                      interval,
                      "UpgradeImage/activate-spiral", skillConfigs);
                  }
                }
              },
            };
          },

          #endregion

          #region Puppet

          (_playerNumber, existingUpgrades) =>
          {
            const int minInterval = 5;

            const int lv0Count = 3,
              lv1Count = 2,
              lv2Count = 2;

            var puppetLv0 = SkillPresets.PlayerTimingSkills["PuppetLv0"];
            var puppetLv1 = SkillPresets.PlayerTimingSkills["PuppetLv1"];
            var puppetLv2 = SkillPresets.PlayerTimingSkills["PuppetLv2"];

            var reduceIntervalPercentage = MathUtils.GetRandom(
              MathUtils.GetPiecewise(1.5f, 0.25f, 4, 0.05f,
                existingUpgrades.Count(u => u.UpgradeId.Contains("ReducePuppetIntervalLv"))), 0.1f);

            var increaseHurtPercentage =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.35f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId.Contains("IncreasePuppetHurtLv"))), 0.1f);

            var _timingSkills = StoreManager.Instance.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
              .PlayerSkills[_playerNumber].Skills;

            var _currentPuppet =
              _timingSkills.FirstOrDefault(container => container.SkillId.Contains("PuppetLv"));

            var puppetImage = "UpgradeImage/puppet-lv0";
            var addPuppetName = "AddPuppetLv0";
            var reducePuppetIntervalName = "ReducePuppetIntervalLv0";
            var increasePuppetHurtName = "IncreasePuppetHurtLv0";
            if (_currentPuppet != null)
            {
              var regex = new Regex(@"PuppetLv(\d+)");
              var match = regex.Match(_currentPuppet.SkillId);
              var level = match.Success ? int.Parse(match.Groups[1].Value) : 0;

              puppetImage = $"UpgradeImage/puppet-lv{level}";
              addPuppetName = $"AddPuppetLv{level}";
              reducePuppetIntervalName = $"ReducePuppetIntervalLv{level}";
              increasePuppetHurtName = $"IncreasePuppetHurtLv{level}";
            }

            var numberOfAddPuppet = existingUpgrades.Count(u => u.UpgradeId.Contains(addPuppetName));
            var addPuppet = numberOfAddPuppet <= 5 ? numberOfAddPuppet + 1 : 0;

            return new List<UpgradeReducerContainer>
            {
              new()
              {
                UpgradeId = "ActivatePuppet",
                Category = UpgradeCategory.Puppet,
                UpgradeNameText = getText("Upgrade/ActivatePuppet/Name"),
                UpgradeDescriptionText =
                  getText("Upgrade/ActivatePuppet/Description", lv0Count, puppetLv0.DefaultInterval),
                ImageName = "UpgradeImage/puppet-lv0",
                MaximumUpgradeCount = 1,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .SetTimingSkill(
                      "PuppetLv0",
                      puppetLv0.SkillPrefabName,
                      lv0Count,
                      puppetLv0.DefaultInterval,
                      "UpgradeImage/puppet-lv0",
                      puppetLv0.SkillConfigurations
                    );
                }
              },
              new()
              {
                UpgradeId = "LevelUpPuppetToLv1",
                Category = UpgradeCategory.Puppet,
                UpgradeNameText = getText("Upgrade/LevelUpPuppetToLv1/Name"),
                UpgradeDescriptionText = getText("Upgrade/LevelUpPuppetToLv1/Description", lv1Count,
                  puppetLv1.DefaultInterval),
                ImageName = "UpgradeImage/puppet-lv1",
                MaximumUpgradeCount = 1,
                PrerequisiteUpgradeIds = new List<string> { "ActivatePuppet" },
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentPuppets =
                    timingSkills
                      .FirstOrDefault(container => container.SkillId.Contains("PuppetLv"));
                  if (currentPuppets != null)
                    storeManager.Commit(StoreNames.TimingSkillStore,
                      StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData()
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentPuppets.SkillId,
                      });
                  skillAndUpgradeController
                    .SetTimingSkill(
                      "PuppetLv1",
                      puppetLv1.SkillPrefabName,
                      lv1Count,
                      puppetLv1.DefaultInterval,
                      "UpgradeImage/puppet-lv1",
                      puppetLv1.SkillConfigurations
                    );
                }
              },
              new()
              {
                UpgradeId = "LevelUpPuppetToLv2",
                Category = UpgradeCategory.Puppet,
                UpgradeNameIndicator = getText("Upgrade/LevelUpPuppetToLv2/Name"),
                UpgradeDescriptionIndicator = getText("Upgrade/LevelUpPuppetToLv2/Description", lv2Count,
                  puppetLv2.DefaultInterval),
                ImageName = "UpgradeImage/puppet-lv2",
                MaximumUpgradeCount = 1,
                PrerequisiteUpgradeIds = new List<string> { "LevelUpPuppetToLv1" },
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentPuppets =
                    timingSkills
                      .FirstOrDefault(container => container.SkillId.Contains("PuppetLv"));
                  if (currentPuppets != null)
                    storeManager.Commit(StoreNames.TimingSkillStore,
                      StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData()
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentPuppets.SkillId,
                      });
                  skillAndUpgradeController
                    .SetTimingSkill(
                      "PuppetLv2",
                      puppetLv1.SkillPrefabName,
                      lv2Count,
                      puppetLv2.DefaultInterval,
                      "UpgradeImage/puppet-lv2",
                      puppetLv2.SkillConfigurations
                    );
                }
              },
              new()
              {
                UpgradeId = addPuppetName,
                Category = UpgradeCategory.Puppet,
                UpgradeNameText = getText($"Upgrade/{addPuppetName}/Name", numberOfAddPuppet + 1),
                UpgradeDescriptionText = getText($"Upgrade/{addPuppetName}/Description", addPuppet),
                ImageName = puppetImage,
                MaximumUpgradeCount = 5,
                PrerequisiteUpgradeIds = new List<string> { "ActivatePuppet" },
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Puppet) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var puppet =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("PuppetLv"));

                  if (puppet != null)
                  {
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData
                      {
                        PlayerNumber = playerNumber,
                        SkillId = puppet.SkillId,
                      });

                    var interval = puppet.Interval;
                    var quantity = puppet.SkillCount + addPuppet;
                    var skillConfigs = puppet.SkillConfigurations;

                    skillAndUpgradeController
                      .SetTimingSkill(
                        puppet.SkillId,
                        puppet.SkillPrefabName,
                        quantity,
                        interval,
                        puppetImage,
                        skillConfigs
                      );
                  }
                }
              },
              new()
              {
                UpgradeId = reducePuppetIntervalName,
                Category = UpgradeCategory.Puppet,
                UpgradeNameText = getText($"Upgrade/{reducePuppetIntervalName}/Name",
                  existingUpgrades.Count(u => u.UpgradeId == reducePuppetIntervalName) + 1),
                UpgradeDescriptionText =
                  getText($"Upgrade/{reducePuppetIntervalName}/Description", reduceIntervalPercentage * 100),
                ImageName = puppetImage,
                MaximumUpgradeCount = 5,
                IsAvailable = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentPuppet =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("PuppetLv"));

                  return currentPuppet == null || currentPuppet.Interval > minInterval;
                },
                PrerequisiteUpgradeIds = new List<string> { "ActivatePuppet" },
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Puppet) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentPuppet =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("PuppetLv"));

                  if (currentPuppet != null)
                  {
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentPuppet.SkillId,
                      });

                    var interval = currentPuppet.Interval * (1 - reduceIntervalPercentage);
                    var quantity = currentPuppet.SkillCount;
                    var skillConfigs = currentPuppet.SkillConfigurations;

                    var newInterval = Mathf.Max(interval, minInterval);

                    skillAndUpgradeController.SetTimingSkill(
                      currentPuppet.SkillId,
                      currentPuppet.SkillPrefabName,
                      quantity,
                      newInterval,
                      puppetImage,
                      skillConfigs
                    );
                  }
                }
              },
              new()
              {
                UpgradeId = increasePuppetHurtName,
                Category = UpgradeCategory.Puppet,
                UpgradeNameText = getText($"Upgrade/{increasePuppetHurtName}/Name",
                  existingUpgrades.Count(u => u.UpgradeId == increasePuppetHurtName) + 1),
                UpgradeDescriptionText =
                  getText($"Upgrade/{increasePuppetHurtName}/Description", increaseHurtPercentage * 100),
                ImageName = puppetImage,
                PrerequisiteUpgradeIds = new() { "ActivatePuppet" },
                MaximumUpgradeCount = 5,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.Puppet) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentPuppet =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("PuppetLv"));

                  if (currentPuppet != null)
                  {
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData()
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentPuppet.SkillId,
                      });

                    var interval = currentPuppet.Interval;
                    var quantity = currentPuppet.SkillCount;
                    var skillConfigs = (BasicSkillConfigurations)currentPuppet.SkillConfigurations.Clone();

                    skillConfigs.SkillHurt = new SkillHurt
                    {
                      HurtPoint = skillConfigs.SkillHurt.HurtPoint * (1 + increaseHurtPercentage),
                      HurtType = skillConfigs.SkillHurt.HurtType,
                    };

                    skillAndUpgradeController.SetTimingSkill(currentPuppet.SkillId, currentPuppet.SkillPrefabName,
                      quantity,
                      interval,
                      puppetImage,
                      skillConfigs
                    );
                  }
                }
              },
            };
          },

          #endregion

          #region Flying Sword

          (_playerNumber, existingUpgrades) =>
          {
            const int minInterval = 5;

            const int lv0Count = 3,
              lv1Count = 2,
              lv2Count = 2;

            var swordLv0 = SkillPresets.PlayerTimingSkills["FlyingSwordLv0"];
            var swordLv1 = SkillPresets.PlayerTimingSkills["FlyingSwordLv1"];
            var swordLv2 = SkillPresets.PlayerTimingSkills["FlyingSwordLv2"];

            var reduceIntervalPercentage = MathUtils.GetRandom(
              MathUtils.GetPiecewise(1.5f, 0.25f, 4, 0.05f,
                existingUpgrades.Count(u => u.UpgradeId.Contains("ReduceFlyingSwordIntervalLv"))), 0.1f);

            var increaseHurtPercentage =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.35f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId.Contains("IncreaseFlyingSwordHurtLv"))), 0.1f);

            var _timingSkills = StoreManager.Instance.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
              .PlayerSkills[_playerNumber].Skills;

            var _currentSword =
              _timingSkills.FirstOrDefault(container => container.SkillId.Contains("FlyingSwordLv"));

            var swordImage = "UpgradeImage/flying-sword-lv0";
            var addSwordName = "AddFlyingSwordLv0";
            var reduceSwordIntervalName = "ReduceFlyingSwordIntervalLv0";
            var increaseSwordHurtName = "IncreaseFlyingSwordHurtLv0";
            if (_currentSword != null)
            {
              var regex = new Regex(@"FlyingSwordLv(\d+)");
              var match = regex.Match(_currentSword.SkillId);
              var level = match.Success ? int.Parse(match.Groups[1].Value) : 0;

              swordImage = $"UpgradeImage/flying-sword-lv{level}";
              addSwordName = $"AddFlyingSwordLv{level}";
              reduceSwordIntervalName = $"ReduceFlyingSwordIntervalLv{level}";
              increaseSwordHurtName = $"IncreaseFlyingSwordHurtLv{level}";
            }

            var numberOfAddSword = existingUpgrades.Count(u => u.UpgradeId == addSwordName);
            var addSword = numberOfAddSword <= 5 ? numberOfAddSword + 1 : 1;

            return new List<UpgradeReducerContainer>()
            {
              new()
              {
                UpgradeId = "ActivateFlyingSword",
                Category = UpgradeCategory.FlyingSword,
                UpgradeNameText = getText("Upgrade/ActivateFlyingSword/Name"),
                UpgradeDescriptionText = getText("Upgrade/ActivateFlyingSword/Description", lv0Count,
                  swordLv0.DefaultInterval),
                ImageName = "UpgradeImage/flying-sword-lv0",
                MaximumUpgradeCount = 1,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .SetTimingSkill(
                      "FlyingSwordLv0",
                      swordLv0.SkillPrefabName,
                      lv0Count,
                      swordLv0.DefaultInterval,
                      "UpgradeImage/flying-sword-lv0",
                      swordLv0.SkillConfigurations
                    );
                }
              },
              new()
              {
                UpgradeId = "LevelUpFlyingSwordToLv1",
                Category = UpgradeCategory.FlyingSword,
                UpgradeNameText = getText("Upgrade/LevelUpFlyingSwordToLv1/Name"),
                UpgradeDescriptionText = getText("Upgrade/LevelUpFlyingSwordToLv1/Description", lv1Count,
                  swordLv1.DefaultInterval),
                ImageName = "UpgradeImage/flying-sword-lv1",
                MaximumUpgradeCount = 1,
                PrerequisiteUpgradeIds = new List<string> { "ActivateFlyingSword" },
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentFlyingSword =
                    timingSkills
                      .FirstOrDefault(container => container.SkillId.Contains("FlyingSwordLv"));
                  if (currentFlyingSword != null)
                    storeManager.Commit(StoreNames.TimingSkillStore,
                      StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData()
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentFlyingSword.SkillId,
                      });
                  skillAndUpgradeController
                    .SetTimingSkill(
                      "FlyingSwordLv1",
                      swordLv1.SkillPrefabName,
                      lv1Count,
                      swordLv1.DefaultInterval,
                      "UpgradeImage/flying-sword-lv1",
                      swordLv1.SkillConfigurations
                    );
                }
              },
              new()
              {
                UpgradeId = "LevelUpFlyingSwordToLv2",
                Category = UpgradeCategory.FlyingSword,
                UpgradeNameText = getText("Upgrade/LevelUpFlyingSwordToLv2/Name"),
                UpgradeDescriptionText = getText("Upgrade/LevelUpFlyingSwordToLv2/Description", lv2Count,
                  swordLv2.DefaultInterval),
                ImageName = "UpgradeImage/flying-sword-lv2",
                MaximumUpgradeCount = 1,
                PrerequisiteUpgradeIds = new List<string> { "LevelUpFlyingSwordToLv1" },
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;
                  var currentFlyingSword = timingSkills
                    .FirstOrDefault(container => container.SkillId.Contains("FlyingSwordLv"));
                  if (currentFlyingSword != null)
                    storeManager.Commit(StoreNames.TimingSkillStore,
                      StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData()
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentFlyingSword.SkillId,
                      });
                  skillAndUpgradeController
                    .SetTimingSkill(
                      "FlyingSwordLv2",
                      swordLv2.SkillPrefabName,
                      lv2Count,
                      swordLv2.DefaultInterval,
                      "UpgradeImage/flying-sword-lv2",
                      swordLv2.SkillConfigurations
                    );
                }
              },
              new()
              {
                UpgradeId = addSwordName,
                Category = UpgradeCategory.FlyingSword,
                UpgradeNameText = getText($"Upgrade/{addSwordName}/Name", numberOfAddSword + 1),
                UpgradeDescriptionText = getText($"Upgrade/{addSwordName}/Description", addSword),
                ImageName = swordImage,
                MaximumUpgradeCount = 5,
                PrerequisiteUpgradeIds = new List<string> { "ActivateFlyingSword" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.FlyingSword) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentSword =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("FlyingSwordLv"));

                  if (currentSword != null)
                  {
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentSword.SkillId,
                      });

                    var interval = currentSword.Interval;
                    var quantity = currentSword.SkillCount + addSword;
                    var skillConfigs = currentSword.SkillConfigurations;

                    skillAndUpgradeController.SetTimingSkill(
                      skillConfigs.SkillId,
                      currentSword.SkillPrefabName,
                      quantity + addSword,
                      interval,
                      swordImage,
                      skillConfigs);
                  }
                }
              },
              new()
              {
                UpgradeId = reduceSwordIntervalName,
                Category = UpgradeCategory.FlyingSword,
                UpgradeNameText = getText($"Upgrade/{reduceSwordIntervalName}/Name",
                  existingUpgrades.Count(u => u.UpgradeId == reduceSwordIntervalName) + 1),
                UpgradeDescriptionText =
                  getText($"Upgrade/{reduceSwordIntervalName}/Description", reduceIntervalPercentage * 100),
                ImageName = swordImage,
                MaximumUpgradeCount = 5,
                IsAvailable = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentSword =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("FlyingSwordLv"));

                  return currentSword == null || currentSword.Interval > minInterval;
                },
                PrerequisiteUpgradeIds = new() { "ActivateFlyingSword" },
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.FlyingSword) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentSword =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("FlyingSwordLv"));

                  if (currentSword != null)
                  {
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentSword.SkillId,
                      });

                    var interval = currentSword.Interval * (1 - reduceIntervalPercentage);
                    var quantity = currentSword.SkillCount;
                    var skillConfigs = currentSword.SkillConfigurations;

                    var newInterval = Mathf.Max(interval, minInterval);

                    skillAndUpgradeController.SetTimingSkill(currentSword.SkillId, currentSword.SkillPrefabName,
                      quantity,
                      newInterval,
                      swordImage,
                      skillConfigs
                    );
                  }
                }
              },
              new()
              {
                UpgradeId = increaseSwordHurtName,
                Category = UpgradeCategory.FlyingSword,
                UpgradeNameText = getText($"Upgrade/{increaseSwordHurtName}/Name",
                  existingUpgrades.Count(u => u.UpgradeId == increaseSwordHurtName) + 1),
                UpgradeDescriptionText =
                  getText($"Upgrade/{increaseSwordHurtName}/Description", increaseHurtPercentage * 100),
                ImageName = swordImage,
                PrerequisiteUpgradeIds = new() { "ActivateFlyingSword" },
                MaximumUpgradeCount = 5,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.FlyingSword) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentSword =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("FlyingSwordLv"));

                  if (currentSword != null)
                  {
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData()
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentSword.SkillId,
                      });

                    var interval = currentSword.Interval;
                    var quantity = currentSword.SkillCount;
                    var skillConfigs = (BasicSkillConfigurations)currentSword.SkillConfigurations.Clone();

                    skillConfigs.SkillHurt = new SkillHurt
                    {
                      HurtPoint = skillConfigs.SkillHurt.HurtPoint * (1 + increaseHurtPercentage),
                      HurtType = skillConfigs.SkillHurt.HurtType,
                    };

                    skillAndUpgradeController.SetTimingSkill(currentSword.SkillId, currentSword.SkillPrefabName,
                      quantity,
                      interval,
                      swordImage,
                      skillConfigs
                    );
                  }
                }
              },
            };
          },

          #endregion

          #region Mine

          (_playerNumber, existingUpgrades) =>
          {
            const int minInterval = 5;

            const int lv0Count = 4,
              lv1Count = 3,
              lv2Count = 3;

            var mineLv0 = SkillPresets.PlayerTimingSkills["MineLv0"];
            var mineLv1 = SkillPresets.PlayerTimingSkills["MineLv1"];
            var mineLv2 = SkillPresets.PlayerTimingSkills["MineLv2"];

            var reduceIntervalPercentage = MathUtils.GetRandom(
              MathUtils.GetPiecewise(1.5f, 0.25f, 4, 0.05f,
                existingUpgrades.Count(u => u.UpgradeId.Contains("ReduceMineIntervalLv"))), 0.1f);

            var increaseHurtPercentage =
              MathUtils.GetRandom(
                MathUtils.GetExponential(1.5f, 0.35f, 5, 0.1f,
                  existingUpgrades.Count(u => u.UpgradeId.Contains("IncreaseMineHurtLv"))), 0.1f);

            var _timingSkills = StoreManager.Instance.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
              .PlayerSkills[_playerNumber].Skills;

            var _currentMine = _timingSkills.FirstOrDefault(container => container.SkillId.Contains("MineLv"));

            var mineImage = "UpgradeImage/mine-lv0";
            var addMineName = "AddMineLv0";
            var reduceMineIntervalName = "ReduceMineIntervalLv0";
            var increaseMineHurtName = "IncreaseMineHurtLv0";
            if (_currentMine != null)
            {
              var regex = new Regex(@"MineLv(\d+)");
              var match = regex.Match(_currentMine.SkillId);
              var level = match.Success ? int.Parse(match.Groups[1].Value) : 0;

              mineImage = $"UpgradeImage/mine-lv{level}";
              addMineName = $"AddMineLv{level}";
              reduceMineIntervalName = $"ReduceMineIntervalLv{level}";
              increaseMineHurtName = $"IncreaseMineHurtLv{level}";
            }

            var numberOfAddMine = existingUpgrades.Count(u => u.UpgradeId.Contains(addMineName));
            var addMine = numberOfAddMine <= 5 ? numberOfAddMine + 1 : 0;

            return new List<UpgradeReducerContainer>()
            {
              new()
              {
                UpgradeId = "ActivateMine",
                Category = UpgradeCategory.Mine,
                UpgradeNameIndicator = getText("Upgrade/ActivateMine/Name"),
                UpgradeDescriptionIndicator =
                  getText("Upgrade/ActivateMine/Description",
                    lv0Count,
                    mineLv0.SkillConfigurations.SkillHurt.HurtPoint,
                    mineLv0.SkillConfigurations.Range,
                    mineLv0.DefaultInterval
                  ),
                ImageName = "UpgradeImage/mine-lv0",
                MaximumUpgradeCount = 1,
                UpgradeReducer = playerNumber =>
                {
                  SkillAndUpgradeManager
                    .Instance
                    .SkillAndUpgradeControllers[playerNumber]
                    .SetTimingSkill(
                      "MineLv0",
                      mineLv0.SkillPrefabName,
                      lv0Count,
                      mineLv0.DefaultInterval,
                      "UpgradeImage/mine-lv0",
                      mineLv0.SkillConfigurations);
                }
              },
              new()
              {
                UpgradeId = "LevelUpMineToLv1",
                Category = UpgradeCategory.Mine,
                UpgradeNameIndicator = getText("Upgrade/LevelUpMineToLv1/Name"),
                UpgradeDescriptionIndicator =
                  getText("Upgrade/LevelUpMineToLv1/Description",
                    lv1Count,
                    mineLv1.SkillConfigurations.SkillHurt.HurtPoint,
                    mineLv1.SkillConfigurations.Range,
                    mineLv1.DefaultInterval
                  ),
                ImageName = "UpgradeImage/mine-lv1",
                MaximumUpgradeCount = 1,
                PrerequisiteUpgradeIds = new List<string> { "ActivateMine" },
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;
                  var currentMine =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("MineLv"));
                  if (currentMine != null)
                    storeManager.Commit(StoreNames.TimingSkillStore,
                      StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentMine.SkillId,
                      });
                  skillAndUpgradeController
                    .SetTimingSkill(
                      "MineLv1",
                      mineLv1.SkillPrefabName,
                      lv1Count,
                      mineLv1.DefaultInterval,
                      "UpgradeImage/mine-lv1",
                      mineLv1.SkillConfigurations);
                }
              },
              new()
              {
                UpgradeId = "LevelUpMineToLv2",
                Category = UpgradeCategory.Mine,
                UpgradeNameIndicator = getText("Upgrade/LevelUpMineToLv2/Name"),
                UpgradeDescriptionIndicator =
                  getText("Upgrade/LevelUpMineToLv2/Description",
                    lv2Count,
                    mineLv2.SkillConfigurations.SkillHurt.HurtPoint,
                    mineLv2.SkillConfigurations.Range,
                    mineLv2.DefaultInterval
                  ),
                ImageName = "UpgradeImage/mine-lv2",
                MaximumUpgradeCount = 1,
                PrerequisiteUpgradeIds = new List<string> { "LevelUpMineToLv1" },
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;
                  var CurrentFireMine =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("MineLv"));
                  if (CurrentFireMine != null)
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData
                      {
                        PlayerNumber = playerNumber,
                        SkillId = CurrentFireMine.SkillId,
                      });
                  skillAndUpgradeController
                    .SetTimingSkill(
                      "MineLv2",
                      mineLv2.SkillPrefabName,
                      lv2Count,
                      mineLv2.DefaultInterval,
                      "UpgradeImage/mine-lv2",
                      mineLv2.SkillConfigurations
                    );
                }
              },
              new()
              {
                UpgradeId = addMineName,
                Category = UpgradeCategory.Mine,
                UpgradeNameText = getText($"Upgrade/{addMineName}/Name", numberOfAddMine + 1),
                UpgradeDescriptionText = getText($"Upgrade/{addMineName}/Description", addMine + 1),
                ImageName = mineImage,
                MaximumUpgradeCount = 3,
                PrerequisiteUpgradeIds = new List<string> { "ActivateMine" },
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Mine) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var mine =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("MineLv"));

                  if (mine != null)
                  {
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData
                      {
                        PlayerNumber = playerNumber,
                        SkillId = mine.SkillId,
                      });

                    var interval = mine.Interval;
                    var quantity = mine.SkillCount + addMine;
                    var skillConfigs = mine.SkillConfigurations;

                    skillAndUpgradeController
                      .SetTimingSkill(
                        mine.SkillId,
                        mine.SkillPrefabName,
                        quantity,
                        interval,
                        mineImage,
                        skillConfigs
                      );
                  }
                }
              },
              new()
              {
                UpgradeId = reduceMineIntervalName,
                Category = UpgradeCategory.Mine,
                UpgradeNameText = getText($"Upgrade/{reduceMineIntervalName}/Name",
                  existingUpgrades.Count(u => u.UpgradeId == reduceMineIntervalName) + 1),
                UpgradeDescriptionText =
                  getText($"Upgrade/{reduceMineIntervalName}/Description", reduceIntervalPercentage * 100),
                ImageName = mineImage,
                MaximumUpgradeCount = 5,
                IsAvailable = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentMine =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("MineLv"));

                  return currentMine == null || currentMine.Interval > minInterval;
                },
                PrerequisiteUpgradeIds = new List<string> { "ActivateMine" },
                GetPossibilityIndependentVariable =
                  (luc, _) => luc.Count(uc => uc.Category == UpgradeCategory.Mine) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentMine =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("MineLv"));

                  if (currentMine != null)
                  {
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentMine.SkillId,
                      });

                    var interval = currentMine.Interval * (1 - reduceIntervalPercentage);
                    var quantity = currentMine.SkillCount;
                    var skillConfigs = currentMine.SkillConfigurations;

                    var newInterval = Mathf.Max(interval, minInterval);

                    skillAndUpgradeController.SetTimingSkill(
                      currentMine.SkillId,
                      currentMine.SkillPrefabName,
                      quantity,
                      newInterval,
                      mineImage,
                      skillConfigs
                    );
                  }
                }
              },
              new()
              {
                UpgradeId = increaseMineHurtName,
                Category = UpgradeCategory.Mine,
                UpgradeNameText = getText($"Upgrade/{increaseMineHurtName}/Name",
                  existingUpgrades.Count(u => u.UpgradeId == increaseMineHurtName) + 1),
                UpgradeDescriptionText =
                  getText($"Upgrade/{increaseMineHurtName}/Description", increaseHurtPercentage * 100),
                ImageName = mineImage,
                PrerequisiteUpgradeIds = new() { "ActivateMine" },
                MaximumUpgradeCount = 5,
                GetPossibilityIndependentVariable = (luc, _) =>
                  luc.Count(uc => uc.Category == UpgradeCategory.Mine) * relationCoefficient,
                UpgradeReducer = playerNumber =>
                {
                  var storeManager = StoreManager.Instance;
                  var skillAndUpgradeController =
                    SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber];
                  var timingSkills = storeManager.GetState<TimingSkillState>(StoreNames.TimingSkillStore)
                    .PlayerSkills[playerNumber].Skills;

                  var currentMine =
                    timingSkills.FirstOrDefault(container => container.SkillId.Contains("MineLv"));

                  if (currentMine != null)
                  {
                    storeManager.Commit(StoreNames.TimingSkillStore, StoreActions.TimingSkillStore_REMOVE_TIMING_SKILL,
                      new TimingSkillActionData()
                      {
                        PlayerNumber = playerNumber,
                        SkillId = currentMine.SkillId,
                      });

                    var interval = currentMine.Interval;
                    var quantity = currentMine.SkillCount;
                    var skillConfigs = (BasicSkillConfigurations)currentMine.SkillConfigurations.Clone();

                    skillConfigs.SkillHurt = new SkillHurt
                    {
                      HurtPoint = skillConfigs.SkillHurt.HurtPoint * (1 + increaseHurtPercentage),
                      HurtType = skillConfigs.SkillHurt.HurtType,
                    };

                    skillAndUpgradeController.SetTimingSkill(currentMine.SkillId, currentMine.SkillPrefabName,
                      quantity,
                      interval,
                      mineImage,
                      skillConfigs
                    );
                  }
                }
              },
            };
          },

          #endregion
        };

    private static List<UpgradeReducerContainer> fixedUpgradeReducers { get; } = new();
  }
}