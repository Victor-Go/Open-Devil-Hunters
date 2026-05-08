using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.Werewolf
{
  public class WerewolfAnimator : MonoBehaviour
  {
    private Werewolf rootController;
    private AudioWrapper walks;
    private AudioWrapper howls;

    private void Awake()
    {
      rootController = GetComponentInParent<Werewolf>();

      walks = new AudioWrapper(new[]
      {
        "Audio/Sound/Enemy/Walk/animal-walk_3",
        "Audio/Sound/Enemy/Walk/animal-walk_4",
        "Audio/Sound/Enemy/Walk/animal-walk_5",
      }, transform);

      howls = new AudioWrapper(new[]
      {
        "Audio/Sound/Enemy/Skill/wolf-howl_0",
        "Audio/Sound/Enemy/Skill/wolf-howl_1",
        "Audio/Sound/Enemy/Skill/wolf-howl_2",
      }, transform);
    }

    public void OnAttackAnimationStart()
    {
      rootController.OnAttackAnimationStart();
    }

    public void OnAttackAnimationFinished()
    {
      rootController.OnAttackAnimationFinished();
    }

    public void OnInvokeAnimationFinished()
    {
      rootController.OnInvokeAnimationFinished();
    }

    public void PlayWalk()
    {
      walks.PlayRandomly();
    }

    public void PlayHowl()
    {
      howls.PlayRandomly();
    }
  }
}