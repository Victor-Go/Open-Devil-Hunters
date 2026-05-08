using System.Collections.Generic;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.ActiveSkill
{
  public class HailStrikeActiveSkillConfigurations : ActiveSkillConfigurations
  {
    public float Duration { get; set; }
    public int Quantity { get; set; }

    public HailStrikeActiveSkillConfigurations()
    {
    }

    public HailStrikeActiveSkillConfigurations(HailStrikeActiveSkillConfigurations other) : base(other)
    {
      Duration = other.Duration;
      Quantity = other.Quantity;
    }

    public override object Clone()
    {
      return new HailStrikeActiveSkillConfigurations(this);
    }
  }

  public class HailStrikeActiveSkill : PlayerActiveSkill
  {
    private readonly float skillBaseHurt = 100;

    private readonly List<string> launchAudioNames = new()
    {
      "Audio/Sound/Skill/ActiveSkill/hail-skill_0",
      "Audio/Sound/Skill/ActiveSkill/hail-skill_1",
      "Audio/Sound/Skill/ActiveSkill/hail-skill_2",
    };

    private readonly List<AudioClip> audioClips = new();

    private readonly ObjectPool objectPool;
    private string strikeScheduleId;
    private string clearStrikeScheduleId;

    public HailStrikeActiveSkill()
    {
      objectPool = ObjectPool.Instance;

      foreach (var name in launchAudioNames)
      {
        audioClips.Add(Resources.Load<AudioClip>(name));
      }
    }

    private void LaunchSkill(int playerNumber)
    {
      #region Take player basic skill configs if it's of type Magic Ice.

      var skillConfigs = new BasicSkillConfigurations();
      var activeSkillAE = new SkillAdditionalEffect() { BasicAdditionalEffectType = BasicAdditionalEffectTypes.NONE };

      var playerBasicSkillConfigs =
        skillAndUpgradeManager.SkillAndUpgradeControllers[playerNumber].BasicSkill.SkillConfigurations;

      var iceHurt = skillBaseHurt; // Base active skill hurt.

      // If player basic skill is Ice Magic, then use its configs; otherwise, use default hurt.
      if (playerBasicSkillConfigs.SkillHurt.HurtType == HurtTypes.MAGIC_ICE)
      {
        iceHurt = Mathf.Max(playerBasicSkillConfigs.SkillHurt.HurtPoint, iceHurt);
      }

      skillConfigs.SkillHurt = new SkillHurt() { HurtPoint = iceHurt, HurtType = HurtTypes.MAGIC_ICE };

      if (playerBasicSkillConfigs.AdditionalEffect.BasicAdditionalEffectType.Equals(BasicAdditionalEffectTypes.FREEZE))
      {
        var playerBasicSkillAE = playerBasicSkillConfigs.AdditionalEffect;
        activeSkillAE.BasicAdditionalEffectType = BasicAdditionalEffectTypes.FREEZE;

        var iceAE = new IceAdditionalEffect();

        var playerBasicSkillBasicAE = (IceAdditionalEffect)playerBasicSkillAE.BasicAdditionalEffect;
        iceAE.Possibility = playerBasicSkillBasicAE.Possibility;
        iceAE.DamagePerSecond = playerBasicSkillBasicAE.DamagePerSecond;
        iceAE.LastForSeconds = playerBasicSkillBasicAE.LastForSeconds;

        activeSkillAE.BasicAdditionalEffect = iceAE;
      }

      skillConfigs.AdditionalEffect = activeSkillAE;

      #endregion

      var quantity = ((HailStrikeActiveSkillConfigurations)ActiveSkillConfigurations).Quantity;

      var meteoriteObjects = new[]
      {
        "Skill/Player/Hail0"
      };

      List<GameObject> hails = new();
      for (var i = 0; i < quantity; i++)
      {
        var hail = objectPool.GetObject(meteoriteObjects[Random.Range(0, meteoriteObjects.Length)]);
        hail.transform.SetParent(null);

        var skillController = hail.GetComponent<PlayerBasicSkill>();
        skillController.SetSkillConfigurations(skillConfigs);

        hails.Add(hail);
      }

      var playerTransform = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore)
        .PlayerTransforms[playerNumber];
      HailTrajectory.LaunchSkill(new TrajectoryArguments
      {
        PlayerTransform = playerTransform,
      }, hails, 0);

      var position = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore).CenterPosition;
      AudioWrapper.PlayClip(audioClips[Random.Range(0, audioClips.Count)], position);
    }

    public override void Launch(int playerNumber)
    {
      var configs = (HailStrikeActiveSkillConfigurations)ActiveSkillConfigurations;
      strikeScheduleId = scheduling.SetInterval(() => { LaunchSkill(playerNumber); }, 0.2f);

      clearStrikeScheduleId =
        scheduling.SetTimeout(() => { scheduling.ClearSchedule(strikeScheduleId); }, configs.Duration);
    }

    ~HailStrikeActiveSkill()
    {
      scheduling.ClearSchedule(strikeScheduleId);
      scheduling.ClearSchedule(clearStrikeScheduleId);
    }
  }
}