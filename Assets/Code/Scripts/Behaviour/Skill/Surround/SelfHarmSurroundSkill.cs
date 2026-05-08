using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Behaviour.Skill
{
    public class SelfHarmSurroundSkill : PoolableAndPauseableGameObject, ISurroundSkill
    {
        private PlayerBasicSkill skillController;

        protected override void Awake()
        {
            base.Awake();
            skillController = GetComponent<PlayerBasicSkill>();
        }

        public void SetSkillConfigurations(BasicSkillConfigurations configurations)
        {
            skillController.SetSkillConfigurations(configurations);
        }
    }
}
