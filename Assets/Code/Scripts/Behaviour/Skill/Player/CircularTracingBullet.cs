using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Player
{
  public class CircularTracingBullet : CircularBullet
  {
    private GameObject targetEnemy;

    public override void SetTargetPosition(Vector2 targetPosition)
    {
      base.SetTargetPosition(targetPosition);
      TrySetTargetEnemy(targetPosition);
    }

    private void TrySetTargetEnemy(Vector2 position)
    {
      targetEnemy = AttackUtils.GetNearestEnemyAtPosition(position, 1);
      if (targetEnemy != null)
        return;

      targetEnemy = AttackUtils.GetNearestEnemy(PlayerNumber, storeManager);
      if (targetEnemy != null)
        return;
    }

    protected override void FixedUpdate()
    {
      if (paused) return;

      if (targetEnemy == null || !targetEnemy.activeSelf)
        TrySetTargetEnemy(transform.position);
      if (targetEnemy != null && targetEnemy.activeSelf)
        targetPosition = targetEnemy.transform.position;

      base.FixedUpdate();
    }
  }
}