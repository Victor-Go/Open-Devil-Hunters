using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy
{
  public class EnemyLineAim : PoolableAndPauseableGameObject
  {
    private SpriteRenderer spriteRenderer;
    private float length;
    private float x;

    protected override void Awake()
    {
      base.Awake();

      spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      x = 0;
    }

    public EnemyLineAim SetLength(float length)
    {
      var size = spriteRenderer.size;
      size.x = length;
      spriteRenderer.size = size;

      this.length = length;

      return this;
    }

    public EnemyLineAim SetAngle(float angle)
    {
      transform.position = (Vector2)transform.position + new Vector2(length / 2, 0);
      transform.rotation = Quaternion.identity;
      transform.RotateAround((Vector2)transform.position - new Vector2(length / 2, 0), Vector3.forward, angle);

      return this;
    }

    private void FixedUpdate()
    {
      if (paused) return;

      x += Time.fixedDeltaTime;

      var color = spriteRenderer.color;
      color.a = Mathf.Cos(5 * x) / 5 + 0.3f;
      spriteRenderer.color = color;
    }
  }
}