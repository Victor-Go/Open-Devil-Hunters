using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Src.Types
{
  public class EnemyBullet : EnemySkill
  {
    protected Vector2 direction;

    public EnemyBullet SetDirection(Vector2 direction)
    {
      this.direction = direction;
      return this;
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer))
      {
        return;
      }

      objectPool.Recycle(ObjectName, gameObject);
    }
  }
}