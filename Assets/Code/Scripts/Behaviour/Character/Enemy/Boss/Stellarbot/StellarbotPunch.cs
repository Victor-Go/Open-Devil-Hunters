using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy.Boss.Stellarbot
{
  public enum StellarbotPunchDirection
  {
    Left,
    Right
  }

  public class StellarbotPunch : EnemySkill
  {
    [SerializeField] private float maxRange;

    private Vector2 _direction;
    private Vector3 _initialScale;

    private Vector2 _startPosition;
    private AudioWrapper _audioWrapper;

    protected override void Awake()
    {
      base.Awake();
      _initialScale = transform.localScale;

      _audioWrapper = new(new[]
        {
          "Audio/Sound/Enemy/Skill/stellarbot_skill_0",
          "Audio/Sound/Enemy/Skill/stellarbot_skill_1",
          "Audio/Sound/Enemy/Skill/stellarbot_skill_2",
          "Audio/Sound/Enemy/Skill/stellarbot_skill_3",
          "Audio/Sound/Enemy/Skill/stellarbot_skill_4",
          "Audio/Sound/Enemy/Skill/stellarbot_skill_5",
        },
        transform
      );
    }

    protected override void Start()
    {
      base.Start();

      _startPosition = transform.position;
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);
      _startPosition = initialPosition;
      rb.position = initialPosition;

      _audioWrapper.PlayRandomly();
    }

    public void SetDirection(StellarbotPunchDirection direction)
    {
      switch (direction)
      {
        case StellarbotPunchDirection.Left:
          _direction = Vector2.left;
          transform.localScale = _initialScale;
          break;
        case StellarbotPunchDirection.Right:
          _direction = Vector2.right;
          transform.localScale = new Vector3(-_initialScale.x, _initialScale.y, _initialScale.z);
          break;
      }
    }

    private void FixedUpdate()
    {
      if (paused)
      {
        return;
      }

      if ((rb.position - _startPosition).sqrMagnitude > maxRange * maxRange)
      {
        objectPool.Recycle(gameObject);
        return;
      }

      var movement = _direction * (Time.fixedDeltaTime * SkillConfigurations.Speed);
      rb.MovePosition(movement + rb.position);
    }
  }
}