using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.LittleBoss.Specter
{
  public class SpecterAnimator : MonoBehaviour
  {
    private Specter rootController;
    private AudioWrapper appears;
    private AudioWrapper disappears;

    private static readonly string[] appearPaths = new[]
    {
      "Audio/Sound/Enemy/Skill/appear_0",
      "Audio/Sound/Enemy/Skill/appear_1",
    };

    private static readonly string[] disappearPaths = new[]
    {
      "Audio/Sound/Enemy/Skill/disappear_0",
      "Audio/Sound/Enemy/Skill/disappear_1",
      "Audio/Sound/Enemy/Skill/disappear_2",
    };

    private void Awake()
    {
      rootController = GetComponentInParent<Specter>();
      appears = new AudioWrapper(appearPaths, transform);

      disappears = new AudioWrapper(disappearPaths, transform);
    }

    public void OnAppearAnimationFinished()
    {
      rootController.OnAppearAnimationFinished();
    }

    public void OnDisappearAnimationFinished()
    {
      rootController.OnDisappearAnimationFinished();
    }

    public void PlayAppear()
    {
      appears.PlayRandomly();
    }

    public void PlayDisappear()
    {
      disappears.PlayRandomly();
    }
  }
}