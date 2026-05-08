using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.IceTiger
{
  public class IceTigerAnimator : MonoBehaviour
  {
    private IceTiger rootController;
    private AudioWrapper walks;
    private AudioWrapper roars;
    private AudioWrapper skills;

    private void Awake()
    {
      rootController = GetComponentInParent<IceTiger>();

      walks = new(new[]
      {
        "Audio/Sound/Enemy/Walk/animal-walk_0",
        "Audio/Sound/Enemy/Walk/animal-walk_1",
        "Audio/Sound/Enemy/Walk/animal-walk_2",
      }, transform);

      roars = new(new[]
      {
        "Audio/Sound/Enemy/Skill/icetiger-soar_0",
        "Audio/Sound/Enemy/Skill/icetiger-soar_1",
      }, transform);

      skills = new(new[]
      {
        "Audio/Sound/Enemy/Skill/icetiger-skill_0",
        "Audio/Sound/Enemy/Skill/icetiger-skill_1",
        "Audio/Sound/Enemy/Skill/icetiger-skill_2",
        "Audio/Sound/Enemy/Skill/icetiger-skill_3",
      }, transform);
    }

    public void OnAttackAnimationFinished()
    {
      rootController.OnAttackAnimationFinished();
      skills.PlayRandomly();
    }

    public void PlayWalk()
    {
      walks.PlayRandomly();
    }

    public void PlayRoar()
    {
      roars.PlayRandomly();
    }
  }
}