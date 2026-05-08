using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.LittleBoss.CannonDragon
{
  public class CannonDragonAnimator : MonoBehaviour
  {
    private CannonDragon rootController;
    private AudioWrapper walks;

    private void Awake()
    {
      rootController = GetComponentInParent<CannonDragon>();
      
      walks = new(new[]
      {
        "Audio/Sound/Enemy/Walk/enemy-walk_3",
        "Audio/Sound/Enemy/Walk/enemy-walk_4",
        "Audio/Sound/Enemy/Walk/enemy-walk_5",
      }, transform);
    }
    
    public void PlayWalk()
    {
      walks.PlayRandomly();
    }

    public void OnAttackAnimationFinished()
    {
      rootController.OnAttackAnimationFinished();
    }
  }
}