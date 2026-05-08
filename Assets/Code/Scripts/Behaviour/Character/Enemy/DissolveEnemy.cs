using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy
{
  public enum DissolveEnemyDirection
  {
    LEFT,
    RIGHT
  }

  public class DissolveEnemy : PoolableAndPauseableGameObject
  {
    public float DisappearTimeout;

    private SpriteRenderer spriteRenderer;
    private Material dissolveMaterial;
    private float progress;
    private Vector3 initialScale;
    private static readonly int fadePropertyId = Shader.PropertyToID("_Fade");

    protected override void Awake()
    {
      base.Awake();

      spriteRenderer = GetComponent<SpriteRenderer>();
      dissolveMaterial = spriteRenderer.material;

      initialScale = transform.localScale;
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      progress = 0;
      dissolveMaterial.SetFloat(fadePropertyId, 1 - progress);
    }

    public DissolveEnemy SetSprite(Sprite sprite)
    {
      spriteRenderer.sprite = sprite;
      return this;
    }

    public DissolveEnemy SetDirection(DissolveEnemyDirection direction)
    {
      switch (direction)
      {
        case DissolveEnemyDirection.LEFT:
          transform.localScale = initialScale;
          break;
        case DissolveEnemyDirection.RIGHT:
          transform.localScale = new Vector3(-initialScale.x, initialScale.y, 1);
          break;
      }

      return this;
    }

    private void Update()
    {
      if (paused) return;

      var speed = 1 / DisappearTimeout;
      progress += speed * Time.deltaTime;
      dissolveMaterial.SetFloat(fadePropertyId, 1 - progress);

      if (progress >= 1)
      {
        objectPool.Recycle(ObjectName, gameObject);
      }
    }
  }
}