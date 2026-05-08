using System;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.Stellarbot
{
  public class StellarbotAnimator : MonoBehaviour
  {
    private Stellarbot rootController;
    private AudioWrapper _audioWrapper;

    private void Awake()
    {
      rootController = GetComponentInParent<Stellarbot>();
      _audioWrapper = new AudioWrapper(new[]
        {
          "Audio/Sound/Enemy/Walk/stellarbot_walk_0",
          "Audio/Sound/Enemy/Walk/stellarbot_walk_1",
          "Audio/Sound/Enemy/Walk/stellarbot_walk_2",
          "Audio/Sound/Enemy/Walk/stellarbot_walk_3",
          "Audio/Sound/Enemy/Walk/stellarbot_walk_4",
        },
        transform
      );
    }

    public void OnAttackAnimationFinished()
    {
      rootController.OnPunchAnimationFinished();
    }

    public void PlayWalk()
    {
      _audioWrapper.PlayRandomly();
    }
  }
}