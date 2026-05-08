using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.LittleBoss.Witch
{
  public class WitchAnimator : MonoBehaviour
  {
    private Witch rootController;
    private AudioWrapper skills;


    private static readonly string[] skillPaths = new[]
    {
      "Audio/Sound/Enemy/Skill/witch-laugh_0",
      "Audio/Sound/Enemy/Skill/witch-laugh_1",
      "Audio/Sound/Enemy/Skill/witch-laugh_2",
      "Audio/Sound/Enemy/Skill/witch-laugh_3",
    };

    private void Awake()
    {
      rootController = GetComponentInParent<Witch>();
      skills = new(skillPaths, transform);
    }

    public void OnAttack()
    {
      rootController.OnAttack();
      skills.PlayRandomly();
    }
  }
}