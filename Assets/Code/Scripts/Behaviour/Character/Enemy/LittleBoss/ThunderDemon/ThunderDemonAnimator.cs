using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.LittleBoss.ThunderDemon
{
  public class ThunderDemonAnimator : MonoBehaviour
  {
    private ThunderDemon rootController;
    private AudioWrapper walks;

    private void Awake()
    {
      walks = new AudioWrapper(new[]
        {
          "Audio/Sound/Enemy/Walk/thunder-demon_walk_0",
          "Audio/Sound/Enemy/Walk/thunder-demon_walk_1",
        },
        transform
      );
      rootController = GetComponentInParent<ThunderDemon>();
    }

    public void OnAttackAnimationFinished()
    {
      rootController.OnAttackAnimationFinished();
    }

    public void PlayWalk()
    {
      walks.PlayRandomly();
    }
  }
}