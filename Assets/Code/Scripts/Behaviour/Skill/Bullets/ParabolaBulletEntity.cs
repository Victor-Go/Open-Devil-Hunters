using Code.Scripts.Behaviour.Skill.Bullets;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Behaviour.Skill
{
    public class ParabolaBulletEntity : EnemySkill
    {
        public ParabolaBullet ParabolaBullet { get; set; }

        public void OnAnimationFinished()
        {
            ParabolaBullet.OnAnimationFinished();
        }
    }
}