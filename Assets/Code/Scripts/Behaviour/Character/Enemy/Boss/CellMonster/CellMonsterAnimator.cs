using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.CellMonster
{
  public class CellMonsterAnimator : MonoBehaviour
  {
    private CellMonster rootController;
    private AudioWrapper walks;
    private AudioWrapper sprays;

    private static readonly string[] walksPaths = new[]
    {
      "Audio/Sound/Enemy/Walk/enemy-walk_0",
      "Audio/Sound/Enemy/Walk/enemy-walk_1",
      "Audio/Sound/Enemy/Walk/enemy-walk_2",
    };

    private static readonly string[] spraysPaths = new[]
    {
      "Audio/Sound/Enemy/Skill/cell-monster-spray_0",
      "Audio/Sound/Enemy/Skill/cell-monster-spray_1",
      "Audio/Sound/Enemy/Skill/cell-monster-spray_2",
    };

    private void Awake()
    {
      rootController = GetComponentInParent<CellMonster>();

      walks = new(walksPaths, transform);

      sprays = new(spraysPaths, transform);
    }

    private void OnInvokeAnimationForSpray()
    {
      rootController.OnInvokeAnimationForSpray();

      sprays.PlayRandomly();
    }

    public void PlayWalk()
    {
      walks.PlayRandomly();
    }
  }
}