using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy
{
  public enum LasermonSkillRotationDirection
  {
    CLOCKWISE,
    COUNTER_CLOCKWISE,
  }

  public class LasermonSkill : EnemySkill
  {
    public float DebugFrom;
    public float DebugTo;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private Vector2 pivot;
    private LasermonSkillRotationDirection rotationDirection;

    private Quaternion fromQuat;
    private Quaternion toQuat;

    private LasermonSkillRotationDirection
      positiveRotationDirection; // Indicates the relation between fromAngle and toAngle

    private float speed;
    private float length;

    private float time;

    protected override void Awake()
    {
      base.Awake();

      spriteRenderer = GetComponent<SpriteRenderer>();
      boxCollider = GetComponent<BoxCollider2D>();

      rotationDirection = Random.Range(0, 2) == 0
        ? LasermonSkillRotationDirection.CLOCKWISE
        : LasermonSkillRotationDirection.COUNTER_CLOCKWISE;
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      transform.position = Vector2.zero;
      transform.rotation = Quaternion.identity;
    }

    public LasermonSkill SetLength(float length)
    {
      var size = spriteRenderer.size;
      size.x = length;
      spriteRenderer.size = size;

      size = boxCollider.size;
      size.x = length + 0.2f;
      boxCollider.size = size;

      this.length = length;

      return this;
    }

    public LasermonSkill SetPivot(Vector2 pivot)
    {
      if (length == 0) throw new System.Exception("Length not set.");

      this.pivot = pivot;
      transform.position = pivot + new Vector2(length / 2, 0);

      return this;
    }

    public LasermonSkill SetAngle(float angle)
    {
      if (pivot == Vector2.zero) Debug.LogError("Pivot is Vector2(0,0).");

      // FIXME: Here should reset rotation and rotate again.
      RotateTo(angle);
      return this;
    }

    public LasermonSkill SetAngleRange(float fromAngle, float toAngle)
    {
      if (pivot == Vector2.zero) Debug.LogError("Pivot is Vector2(0,0).");

      fromQuat = Quaternion.Euler(0, 0, fromAngle);
      toQuat = Quaternion.Euler(0, 0, toAngle);

      Quaternion f = Quaternion.Euler(0, 0, fromAngle), t = Quaternion.Euler(0, 0, toAngle);
      float r = (f * Quaternion.Inverse(t)).eulerAngles.z;
      positiveRotationDirection =
        r > 180 ? LasermonSkillRotationDirection.COUNTER_CLOCKWISE : LasermonSkillRotationDirection.CLOCKWISE;
      rotationDirection = positiveRotationDirection;

      RotateTo(fromAngle);

      //Set init angle between fromQuat and toQuat(Least side).
      //var from = Quaternion.Euler(0, 0, fromQuat);
      //var to = Quaternion.Euler(0, 0, toQuat);
      //RotateTo(Quaternion.Slerp(from, to, 0.5f).eulerAngles.z);

      return this;
    }

    public LasermonSkill SetSpeed(float speed)
    {
      this.speed = speed;
      return this;
    }

    private void RotateTo(float angle)
    {
      SetPivot(pivot);
      Rotate(angle);
    }

    private void Rotate(float angle)
    {
      transform.RotateAround(pivot, Vector3.forward, angle);
    }

    private void FixedUpdate()
    {
      if (paused) return;

      time += Time.fixedDeltaTime;
      var color = spriteRenderer.color;
      color.a = Mathf.Sin(5 * time) / 5 + 0.8f;
      spriteRenderer.color = color;

      if (speed > 0)
      {
        int direction = rotationDirection == LasermonSkillRotationDirection.CLOCKWISE ? -1 : 1;
        Rotate(direction * Time.fixedDeltaTime * speed);

        if (rotationDirection == LasermonSkillRotationDirection.CLOCKWISE &&
            positiveRotationDirection == LasermonSkillRotationDirection.CLOCKWISE &&
            (transform.rotation * Quaternion.Inverse(toQuat)).eulerAngles.z > 180)
        {
          rotationDirection = LasermonSkillRotationDirection.COUNTER_CLOCKWISE;
        }
        else if (rotationDirection == LasermonSkillRotationDirection.CLOCKWISE &&
                 positiveRotationDirection == LasermonSkillRotationDirection.COUNTER_CLOCKWISE &&
                 (transform.rotation * Quaternion.Inverse(fromQuat)).eulerAngles.z > 180)
        {
          rotationDirection = LasermonSkillRotationDirection.COUNTER_CLOCKWISE;
        }
        else if (rotationDirection == LasermonSkillRotationDirection.COUNTER_CLOCKWISE &&
                 positiveRotationDirection == LasermonSkillRotationDirection.CLOCKWISE &&
                 (Quaternion.Inverse(transform.rotation) * fromQuat).eulerAngles.z > 180)
        {
          rotationDirection = LasermonSkillRotationDirection.CLOCKWISE;
        }
        else if (rotationDirection == LasermonSkillRotationDirection.COUNTER_CLOCKWISE &&
                 positiveRotationDirection == LasermonSkillRotationDirection.COUNTER_CLOCKWISE &&
                 (Quaternion.Inverse(transform.rotation) * toQuat).eulerAngles.z > 180)
        {
          rotationDirection = LasermonSkillRotationDirection.CLOCKWISE;
        }
      }
    }
  }
}