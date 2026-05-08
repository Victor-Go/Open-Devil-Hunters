using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Animation
{
  public class RecycleOnFinishedAnimation : PoolableAndPauseableGameObject
  {
    public Animator Animator;

    protected override void Awake()
    {
      base.Awake();

      if (Animator != null)
      {
        animator = Animator;
      }
    }

    public void OnAnimationFinished()
    {
      objectPool.Recycle(ObjectName, gameObject);
    }
  }
}