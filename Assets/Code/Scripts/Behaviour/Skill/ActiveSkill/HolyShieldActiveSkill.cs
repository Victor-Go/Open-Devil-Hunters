using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Behaviour.Skill.ActiveSkill
{
    public class HolyShieldActiveSkill : PlayerActiveSkill
    {
        private PlayerControllers playerControllers = PlayerControllers.Instance;

        public override void Launch(int playerNumber)
        {
            var configs = (PersistActiveSkillConfigurations)ActiveSkillConfigurations;
            float immortalSeconds = configs.Duration;
            playerControllers.PlayerManager.TriggerHolyShield(immortalSeconds);
        }
    }
}