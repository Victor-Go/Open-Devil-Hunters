using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Player
{
  public class Footprint : PoolableAndPauseableGameObject
  {
    public float FootprintDuration = 1.5f;
    public float FootprintFadeOutDuration = 1f;

    private readonly string leftFootprintName = "Environment/Footprint/footprint_left";
    private readonly string rightFootprintName = "Environment/Footprint/footprint_right";

    private SpriteRenderer spriteRenderer;

    private Sprite leftFootprint;
    private Sprite rightFootprint;

    private float footprintElapsed;
    private float footprintFadeOutElapsed;
    private bool fadingOut;

    protected override void Awake()
    {
      base.Awake();

      spriteRenderer = GetComponent<SpriteRenderer>();

      var leftFp = Resources.Load<Texture2D>(leftFootprintName);
      var rightFp = Resources.Load<Texture2D>(rightFootprintName);
      leftFootprint = Sprite.Create(leftFp, new Rect(0, 0, leftFp.width, leftFp.height), Vector2.one / 2);
      rightFootprint = Sprite.Create(rightFp, new Rect(0, 0, rightFp.width, rightFp.height), Vector2.one / 2);
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      spriteRenderer.color = Color.white;
      footprintElapsed = 0;
      footprintFadeOutElapsed = 0;
      fadingOut = false;
    }

    public Footprint SetFootprintDirection(bool isRight)
    {
      switch (isRight)
      {
        case false: spriteRenderer.sprite = leftFootprint; break;
        case true: spriteRenderer.sprite = rightFootprint; break;
      }

      return this;
    }

    public Footprint SetMovementDirection(Vector2 direction)
    {
      var angle = Vector2.SignedAngle(Vector2.left, direction);
      transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

      return this;
    }

    private void Update()
    {
      if (paused) return;

      var dt = Time.deltaTime;
      if (!fadingOut)
      {
        footprintElapsed += dt;
        if (footprintElapsed > FootprintDuration)
        {
          fadingOut = true;
        }
      }
      else
      {
        footprintFadeOutElapsed += dt;
        var finalColor = new Color(0, 0, 0, 0);
        spriteRenderer.color = Color.Lerp(Color.white, finalColor, footprintFadeOutElapsed / FootprintFadeOutDuration);

        if (footprintFadeOutElapsed > FootprintFadeOutDuration)
        {
          fadingOut = false;
          objectPool.Recycle(ObjectName, gameObject);
        }
      }
    }
  }
}