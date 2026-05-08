using System.Collections.Generic;
using Code.Scripts.Src;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.ActiveSkill
{
  public class MeteoriteStrikeActiveSkillConfigurations : ActiveSkillConfigurations
  {
    public float Duration { get; set; }
    public int Quantity { get; set; }

    public MeteoriteStrikeActiveSkillConfigurations()
    {
    }

    public MeteoriteStrikeActiveSkillConfigurations(MeteoriteStrikeActiveSkillConfigurations other) : base(other)
    {
      Duration = other.Duration;
      Quantity = other.Quantity;
    }

    public override object Clone()
    {
      return new MeteoriteStrikeActiveSkillConfigurations(this);
    }
  }

  public class MeteoriteStrikeActiveSkill : PlayerActiveSkill
  {
    private const float skillBaseHurt = 100;

    private readonly List<string> launchAudioNames = new()
    {
      "Audio/Sound/Skill/ActiveSkill/meteorite-skill_0",
      "Audio/Sound/Skill/ActiveSkill/meteorite-skill_1",
      "Audio/Sound/Skill/ActiveSkill/meteorite-skill_2",
      "Audio/Sound/Skill/ActiveSkill/meteorite-skill_3",
    };

    private readonly List<AudioClip> audioClips = new();

    private readonly ObjectPool objectPool;
    private string strikeScheduleId;
    private string clearStrikeScheduleId;

    public MeteoriteStrikeActiveSkill()
    {
      objectPool = ObjectPool.Instance;

      foreach (var name in launchAudioNames)
      {
        audioClips.Add(Resources.Load<AudioClip>(name));
      }
    }

    private void LaunchSkill(int playerNumber)
    {
      #region Take player basic skill configs if it's of type Magic Fire.

      var skillConfigs = new BasicSkillConfigurations();
      var activeSkillAE = new SkillAdditionalEffect() { BasicAdditionalEffectType = BasicAdditionalEffectTypes.NONE };

      var playerBasicSkillConfigs =
        skillAndUpgradeManager.SkillAndUpgradeControllers[playerNumber].BasicSkill.SkillConfigurations;

      var fireHurt = skillBaseHurt; // Base active skill hurt.

      // If player basic skill is Fire Magic, then use its configs; otherwise, use default hurt.
      if (playerBasicSkillConfigs.SkillHurt.HurtType == HurtTypes.MAGIC_FIRE)
      {
        fireHurt = Mathf.Max(playerBasicSkillConfigs.SkillHurt.HurtPoint, fireHurt);
      }

      skillConfigs.SkillHurt = new SkillHurt() { HurtPoint = fireHurt, HurtType = HurtTypes.MAGIC_FIRE };

      if (playerBasicSkillConfigs.AdditionalEffect.BasicAdditionalEffectType.Equals(BasicAdditionalEffectTypes.BURN))
      {
        var playerBasicSkillAE = playerBasicSkillConfigs.AdditionalEffect;
        activeSkillAE.BasicAdditionalEffectType = BasicAdditionalEffectTypes.BURN;

        var fireAE = new FireAdditionalEffect();

        var playerBasicSkillBasicAE = (FireAdditionalEffect)playerBasicSkillAE.BasicAdditionalEffect;
        fireAE.Possibility = playerBasicSkillBasicAE.Possibility;
        fireAE.LastForSeconds = playerBasicSkillBasicAE.LastForSeconds;
        fireAE.HurtPercentagePerSecond = playerBasicSkillBasicAE.HurtPercentagePerSecond;

        activeSkillAE.BasicAdditionalEffect = fireAE;
      }

      skillConfigs.AdditionalEffect = activeSkillAE;

      #endregion

      var quantity = ((MeteoriteStrikeActiveSkillConfigurations)ActiveSkillConfigurations).Quantity;

      var meteoriteObjects = new[]
      {
        "Skill/Player/ActiveSkill/Meteorite0",
        "Skill/Player/ActiveSkill/Meteorite1"
      };

      List<GameObject> meteorites = new();
      for (var i = 0; i < quantity; i++)
      {
        var meteorite = objectPool.GetObject(meteoriteObjects[Random.Range(0, meteoriteObjects.Length)]);
        meteorite.transform.SetParent(null);

        var skillController = meteorite.GetComponent<PlayerBasicSkill>();
        skillController.SetSkillConfigurations(skillConfigs);

        meteorites.Add(meteorite);
      }

      var playerTransform = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore)
        .PlayerTransforms[playerNumber];
      MeteoriteTrajectory.LaunchSkill(new TrajectoryArguments
      {
        PlayerTransform = playerTransform,
      }, meteorites, 0);

      AudioWrapper.PlayClip(audioClips[Random.Range(0, audioClips.Count)], playerTransform.position);
    }

    public override void Launch(int playerNumber)
    {
      var configs = (MeteoriteStrikeActiveSkillConfigurations)ActiveSkillConfigurations;
      strikeScheduleId = scheduling.SetInterval(() => { LaunchSkill(playerNumber); }, 0.5f);

      clearStrikeScheduleId =
        scheduling.SetTimeout(() => { scheduling.ClearSchedule(strikeScheduleId); }, configs.Duration);
    }

    ~MeteoriteStrikeActiveSkill()
    {
      scheduling.ClearSchedule(strikeScheduleId);
      scheduling.ClearSchedule(clearStrikeScheduleId);
    }
  }
}