using UnityEngine;
using System.Collections;
using Code.Scripts.Src.Types;
using Code.Scripts.Src;
using System.Linq;
using System.Collections.Generic;
using Code.Scripts.Behaviour.Level;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Behaviour.Skill
{
  public class PhysicalActiveSkillConfigurations : ActiveSkillConfigurations
  {
    public float Duration { get; set; }
  }

  public struct PhysicalBonus
  {
    public string SkillId { get; set; }
    public float Bonus { get; set; }
  }

  // Not used in current version
  public class PhysicalActiveSkill : PlayerActiveSkill
  {
    private List<PhysicalBonus> physicalBonusList;

    public PhysicalActiveSkill()
    {
      skillAndUpgradeManager = SkillAndUpgradeManager.Instance;
      scheduling = Scheduling.Instance;
    }

    public override void Launch(int playerNumber)
    {
      var effectCountdown = ((PhysicalActiveSkillConfigurations)ActiveSkillConfigurations).Duration;
      scheduling.SetTimeout(RemovePhysicalHurtsBonus, effectCountdown);

      AddPhysicalHurtsBonus();
    }

    private void AddPhysicalHurtsBonus()
    {
      // FIXME: This should be upgraded to be compatible with multiplayers.
      //physicalBonusList = skillAndUpgradeManager.BasicSkills.Select(c => new PhysicalBonus()
      //{
      //    SkillId = c.SkillId,
      //    Bonus = c.SkillConfigurations.SkillHurts.PhysicalHurt.HurtPoint * 1 // 100%
      //}).ToList();

      //for (int i = 0; i < physicalBonusList.Count; i++)
      //{
      //    var phyHurt = skillAndUpgradeManager.BasicSkills[i].SkillConfigurations.SkillHurts.PhysicalHurt;
      //    phyHurt.HurtPoint += physicalBonusList[i].Bonus;
      //    skillAndUpgradeManager.BasicSkills[i].SkillConfigurations.SkillHurts.PhysicalHurt = phyHurt;
      //}
    }

    private void RemovePhysicalHurtsBonus()
    {
      // FIXME: This should be upgraded to be compatible with multiplayers.
      //foreach (var skill in skillAndUpgradeManager.BasicSkills)
      //{
      //    var bonus = physicalBonusList.Where(pb => pb.SkillId.Equals(skill.SkillId)).ToList();
      //    if (bonus.Any())
      //    {
      //        var phyHurt = skill.SkillConfigurations.SkillHurts.PhysicalHurt;
      //        phyHurt.HurtPoint -= bonus[0].Bonus;
      //        skill.SkillConfigurations.SkillHurts.PhysicalHurt = phyHurt;
      //    }
      //}
    }
  }
}