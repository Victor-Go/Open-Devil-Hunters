using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Surround
{
  public class ThunderTower : MobileTower
  {
    protected override void Fire()
    {
      var bullet = objectPool.GetObject("Skill/Player/SurroundSkill/ThunderTowerBullet");

      var controller = bullet.GetComponent<Bullet>();
      controller.SetSkillConfigurations(launchSkillConfigs);
      controller.SetTargetRelativeDirection(transform.rotation * Vector2.down);
      bullet.transform.SetParent(null);
      bullet.transform.position = firePosition.position;
    }
  }
}