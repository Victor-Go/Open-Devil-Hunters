using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.ActiveSkill
{
  public class ThunderStrikeActiveSkillConfigurations : ActiveSkillConfigurations
  {
    public float Duration { get; set; }
    public float PercentageOfEnemies { get; set; }

    public ThunderStrikeActiveSkillConfigurations()
    {
    }

    public ThunderStrikeActiveSkillConfigurations(ThunderStrikeActiveSkillConfigurations other) : base(other)
    {
      Duration = other.Duration;
      PercentageOfEnemies = other.PercentageOfEnemies;
    }

    public override object Clone()
    {
      return new ThunderStrikeActiveSkillConfigurations(this);
    }
  }

  public class ThunderStrikeActiveSkill : PlayerActiveSkill
  {
    private readonly float skillBaseHurt = 100;

    private readonly List<string> launchAudioNames = new()
    {
      "Audio/Sound/Skill/ActiveSkill/thunder-skill_0",
      "Audio/Sound/Skill/ActiveSkill/thunder-skill_1",
      "Audio/Sound/Skill/ActiveSkill/thunder-skill_2",
    };

    private readonly List<AudioClip> audioClips = new();

    private readonly ObjectPool objectPool;
    private string strikeScheduleId;
    private string clearStrikeScheduleId;

    public ThunderStrikeActiveSkill()
    {
      objectPool = ObjectPool.Instance;

      foreach (var name in launchAudioNames)
      {
        audioClips.Add(Resources.Load<AudioClip>(name));
      }
    }

    private void LaunchSkill(int playerNumber, float percentage)
    {
      #region Take player basic skill configs if it's of type Magic Thunder.

      var skillConfigs = new BasicSkillConfigurations();
      var activeSkillAE = new SkillAdditionalEffect() { BasicAdditionalEffectType = BasicAdditionalEffectTypes.NONE };

      var playerBasicSkillConfigs =
        skillAndUpgradeManager.SkillAndUpgradeControllers[playerNumber].BasicSkill.SkillConfigurations;

      var thunderHurt = skillBaseHurt; // Base active skill hurt.

      // If player basic skill is Thunder Magic, then use its configs; otherwise, use default hurt.
      if (playerBasicSkillConfigs.SkillHurt.HurtType == HurtTypes.MAGIC_THUNDER)
      {
        thunderHurt = Mathf.Max(playerBasicSkillConfigs.SkillHurt.HurtPoint, thunderHurt);
      }

      skillConfigs.SkillHurt = new SkillHurt() { HurtPoint = thunderHurt, HurtType = HurtTypes.MAGIC_THUNDER };

      if (playerBasicSkillConfigs.AdditionalEffect.BasicAdditionalEffectType.Equals(BasicAdditionalEffectTypes.STUN))
      {
        var playerBasicSkillAE = playerBasicSkillConfigs.AdditionalEffect;
        activeSkillAE.BasicAdditionalEffectType = BasicAdditionalEffectTypes.STUN;

        var thunderAE = new ThunderAdditionalEffect();

        var playerBasicSkillBasicAE = (ThunderAdditionalEffect)playerBasicSkillAE.BasicAdditionalEffect;
        thunderAE.Possibility = playerBasicSkillBasicAE.Possibility;
        thunderAE.StunningSeconds = playerBasicSkillBasicAE.StunningSeconds;

        activeSkillAE.BasicAdditionalEffect = thunderAE;
      }

      skillConfigs.AdditionalEffect = activeSkillAE;

      #endregion

      var inScreenEnemies = AttackUtils.GetInScreenEnemies();
      var enemiesToStrike = inScreenEnemies.Where(_ => Random.Range(0, 1f) < percentage).ToList();
      List<GameObject> thunders = new();
      for (var i = 0; i < enemiesToStrike.Count; i++)
      {
        var thunder = objectPool.GetObject("Skill/Player/ActiveSkill/ThunderRangeAttack");
        thunder.transform.SetParent(null);

        var skillController = thunder.GetComponent<PlayerBasicSkill>();
        skillController.SetSkillConfigurations(skillConfigs);

        thunders.Add(thunder);
      }

      ThunderTrajectory.LaunchSkill(new()
      {
        Targets = enemiesToStrike,
      }, thunders, 0);

      var position = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore).CenterPosition;
      AudioWrapper.PlayClip(audioClips[Random.Range(0, audioClips.Count)], position);
    }

    private int currentPlayerNumber;

    private void DoStrike()
    {
      var configs = (ThunderStrikeActiveSkillConfigurations)ActiveSkillConfigurations;
      LaunchSkill(currentPlayerNumber, configs.PercentageOfEnemies);
    }

    private void DoClearStrike()
    {
      scheduling.ClearSchedule(strikeScheduleId);
    }

    public override void Launch(int playerNumber)
    {
      currentPlayerNumber = playerNumber;
      var configs = (ThunderStrikeActiveSkillConfigurations)ActiveSkillConfigurations;
      
      strikeScheduleId = scheduling.SetInterval(DoStrike, 0.25f);
      clearStrikeScheduleId = scheduling.SetTimeout(DoClearStrike, configs.Duration);
    }

    ~ThunderStrikeActiveSkill()
    {
      scheduling.ClearSchedule(strikeScheduleId);
      scheduling.ClearSchedule(clearStrikeScheduleId);
    }
  }
}