using System.Linq;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.WideRangeSkill
{
  public class MeteoriteRangeSkill : PlayerBasicSkill
  {
    private Rigidbody2D rb;
    private const float speed = 6;

    protected override void Awake()
    {
      base.Awake();

      Activated = false;
      rb = GetComponent<Rigidbody2D>();
    }

    public override void ObjectReset(Vector2 initPosition)
    {
      base.ObjectReset(initPosition);
      Activated = false;
    }

    private void ShowAnimation()
    {
      var explosions = new[]
      {
        "Status/ActiveSkill/MeteoriteExplosion0",
        "Status/ActiveSkill/MeteoriteExplosion1",
        "Status/ActiveSkill/MeteoriteExplosion2",
        "Status/ActiveSkill/MeteoriteExplosion3"
      };

      string selectedExplosion;
      var random = Random.Range(0, 100);
      selectedExplosion = random switch
      {
        < 15 => explosions[0],
        < 40 => explosions[1],
        < 70 => explosions[2],
        _ => explosions[3]
      };

      var animation = objectPool.GetObject(selectedExplosion);
      animation.transform.SetParent(null);
      animation.transform.position = transform.position;
    }

    private void MakeDamage()
    {
      var colliders = Physics2D.OverlapCircleAll(rb.position, 0.5f).ToList();
      colliders = colliders.Where(collider => TriggerGroup.CanInteract(gameObject.layer, collider.gameObject.layer))
        .ToList();

      if (colliders.Count > 0)
      {
        foreach (var collider in colliders)
        {
          var enemyHurt = collider.gameObject.GetComponent<EnemyHurt>();
          if (enemyHurt != null)
          {
            enemyHurt.TriggerEnemyHurt(PlayerNumber, this, transform.position);
          }
        }
      }
    }

    private void FixedUpdate()
    {
      if (!paused)
      {
        rb.MovePosition(rb.position + relativeDirection * speed * Time.fixedDeltaTime);
        if ((rb.position - targetPosition).sqrMagnitude <= 0.1f)
        {
          Activated = true;

          ShowAnimation();
          MakeDamage();

          objectPool.Recycle(ObjectName, gameObject);
        }
      }
    }
  }
}