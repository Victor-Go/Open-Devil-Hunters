using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Player.Base
{
  public class JeanneDArcSkill : PlayerBasicSkill
  {
    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      transform.localScale = Vector2.one;
    }

    public void OnAnimationFinished()
    {
      objectPool.Recycle(ObjectName, gameObject);
    }
  }
}