using Code.Scripts.Src.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Behaviour.Skill
{
  public class ShootingSurroundSkill : PoolableAndPauseableGameObject, ISurroundSkill
  {
    protected BasicSkillConfigurations launchSkillConfigs;

    public void SetSkillConfigurations(BasicSkillConfigurations configurations)
    {
      launchSkillConfigs = configurations;
    }
  }
}