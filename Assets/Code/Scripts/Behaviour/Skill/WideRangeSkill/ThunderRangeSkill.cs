using Code.Scripts.Behaviour.Character.Enemy;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.WideRangeSkill
{
  public class ThunderRangeSkill : PlayerBasicSkill
  {
    public float DisappearTimeout = 0.1f;

    private float height;
    private bool triggered;
    private float disappearCountdown;

    private void SetHeight()
    {
      SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
      Vector2 topRightCornerWorldPosition = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
      height = topRightCornerWorldPosition.y * 2;

      spriteRenderer.size = new Vector2(spriteRenderer.size.x, height);
    }

    protected override void Awake()
    {
      base.Awake();
      SetHeight();
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);
      triggered = false;
    }

    private void OnRectTransformDimensionsChange()
    {
      SetHeight();
    }

    public void SetTarget(GameObject enemy)
    {
      var enemyPosition = enemy.transform.position;
      transform.position = new Vector2(enemyPosition.x, enemyPosition.y);

      var hurtHandling = enemy.GetComponentInChildren<GeneralEnemyHurtHandling>();
      if (hurtHandling != null)
      {
        hurtHandling.TriggerEnemyHurt(PlayerNumber, this);
      }

      disappearCountdown = DisappearTimeout;
      triggered = true;
    }

    private void Update()
    {
      if (triggered && !paused)
      {
        disappearCountdown -= Time.deltaTime;
        if (disappearCountdown <= 0)
        {
          objectPool.Recycle(ObjectName, gameObject);
        }
      }
    }
  }
}