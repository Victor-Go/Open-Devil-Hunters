using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.LittleBoss.Lasermon
{
  public class LasermonAnimator : MonoBehaviour
  {
    private Lasermon rootController;

    private void Awake()
    {
      rootController = GetComponentInParent<Lasermon>();
    }

    public void OnPrepareAttackAnimationFinished()
    {
      rootController.OnPrepareAttackAnimationFinished();
    }
  }
}