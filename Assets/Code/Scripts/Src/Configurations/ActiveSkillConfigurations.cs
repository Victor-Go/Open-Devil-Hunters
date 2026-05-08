using System;
using System.Collections.Generic;
using Code.Scripts.Behaviour.Skill;
using Code.Scripts.Src.Types;
using Code.Scripts.Behaviour.Skill.ActiveSkill;

namespace Code.Scripts.Src.Configurations
{
  public class ActiveSkillPreset
  {
    public string ImageIndicator { get; set; }
    public Type ActiveSkillType { get; set; }
  }

  public static class ActiveSkillPresets
  {
    public static Dictionary<string, ActiveSkillPreset> Configurations { get; } = new()
    {
      {
        "TimeStopActiveSkill",
        new()
        {
          ImageIndicator = "Rune/time-stop",
          ActiveSkillType = typeof(TimeStopActiveSkill)
        }
      },
      {
        "HolyShieldActiveSkill",
        new()
        {
          ImageIndicator = "Rune/holy-shield",
          ActiveSkillType = typeof(HolyShieldActiveSkill)
        }
      },
      {
        "ThunderStrikeActiveSkill",
        new()
        {
          ImageIndicator = "Rune/thunder-strike",
          ActiveSkillType = typeof(ThunderStrikeActiveSkill)
        }
      },
      {
        "MeteoriteStrikeActiveSkill",
        new()
        {
          ImageIndicator = "Rune/meteorite-strike",
          ActiveSkillType = typeof(MeteoriteStrikeActiveSkill)
        }
      },
      {
        "HailStrikeActiveSkill",
        new()
        {
          ImageIndicator = "Rune/hail-strike",
          ActiveSkillType = typeof(HailStrikeActiveSkill)
        }
      },
      {
        "FullFieldPoisoningActiveSkill",
        new()
        {
          ImageIndicator = "Rune/poisonous",
          ActiveSkillType = typeof(FullFieldPoisoningActiveSkill)
        }
      },
      {
        "KillAndRecoverActiveSkill",
        new()
        {
          ImageIndicator = "Rune/kill-and-recover",
          ActiveSkillType = typeof(KillAndRecoverActiveSkill)
        }
      },
      {
        "SkillFissionActiveSkill",
        new()
        {
          ImageIndicator = "Rune/fission",
          ActiveSkillType = typeof(SkillFissionActiveSkill)
        }
      }
    };
  }
}