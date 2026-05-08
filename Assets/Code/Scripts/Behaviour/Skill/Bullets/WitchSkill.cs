using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Bullets
{
  public class WitchSkill : GeneralBullet
  {
    protected override void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer))
      {
        return;
      }

      var enemyController = other.gameObject.GetComponent<EnemyHurt>();
      if (!touchedEnemies.Contains(enemyController.UniqueId))
      {
        touchedEnemies.Add(enemyController.UniqueId);
        penetrated++;

        var skillHitContext = new BasicSkillHitContext()
        {
          ObjectPool = objectPool,
          CurrentPosition = transform.position,
          SkillConfigurations = (BasicSkillConfigurations)SkillConfigurations,
        };
        foreach (var interceptor in SkillConfigurations.SkillHitInterceptors)
        {
          interceptor(skillHitContext);
        }

        if (penetrated >= ((BasicSkillConfigurations)SkillConfigurations).Penetration)
        {
          RecycleBullet();
        }
      }
    }
  }
}