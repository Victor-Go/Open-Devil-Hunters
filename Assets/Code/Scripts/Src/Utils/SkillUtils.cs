using System;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Src.Utils
{
  public static class SkillUtils
  {
    public static HurtTypes GetHurtTypeFromEnemyStatusType(CharacterStatus enemyStatus)
    {
      switch (enemyStatus)
      {
        case CharacterStatus.NORMAL:
          return HurtTypes.PHYSICAL;
        case CharacterStatus.STUNNING:
          return HurtTypes.MAGIC_THUNDER;
        case CharacterStatus.FROZEN:
          return HurtTypes.MAGIC_ICE;
        case CharacterStatus.BURNING:
          return HurtTypes.MAGIC_FIRE;
        default:
          throw new Exception("Unhandled EnemyStatus.");
      }
    }

    public static Func<int, bool> SkillIsOfHurtTypeGenerator(HurtTypes hurtType)
    {
      return (playerNumber) =>
      {
        var playerSkill = SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber].BasicSkill;
        return playerSkill.SkillConfigurations.SkillHurt.HurtType == hurtType;
      };
    }

    public static Func<int, bool> IsBasicAdditionalEffectTypeGenerator(BasicAdditionalEffectTypes type)
    {
      return (playerNumber) =>
      {
        var playerAE = SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[playerNumber].BasicSkill
          .SkillConfigurations.AdditionalEffect;
        return playerAE.BasicAdditionalEffectType == type;
      };
    }

    public static Func<int, bool> IsNotPlayerNameGenerator(PlayerNames playerName)
    {
      return (playerNumber) =>
      {
        var levelConfgs = StoreManager.Instance.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
        return levelConfgs.PlayerConfigurations[playerNumber].PlayerName != playerName;
      };
    }

    public static bool HasAdditionalEffect(int playerNumber)
    {
      var ae = SkillAndUpgradeManager
        .Instance
        .SkillAndUpgradeControllers[playerNumber]
        .BasicSkill
        .SkillConfigurations
        .AdditionalEffect;

      return ae != null &&
             (ae.BasicAdditionalEffectType != BasicAdditionalEffectTypes.NONE ||
              ae.PoisonAdditionalEffect.Enable == true);
    }
  }
}