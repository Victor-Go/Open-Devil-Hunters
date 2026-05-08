using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.RockGiant
{
  public class RockGiantAnimator : MonoBehaviour
  {
    private RockGiant rootController;
    private AudioWrapper walks;

    private static readonly string[] walkPaths = new[]
    {
      "Audio/Sound/Enemy/Walk/rock-giant_walk_0",
      "Audio/Sound/Enemy/Walk/rock-giant_walk_1",
      "Audio/Sound/Enemy/Walk/rock-giant_walk_2",
      "Audio/Sound/Enemy/Walk/rock-giant_walk_3",
      "Audio/Sound/Enemy/Walk/rock-giant_walk_4",
    };

    private void Awake()
    {
      rootController = GetComponentInParent<RockGiant>();
      walks = new AudioWrapper(walkPaths, transform);
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