using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Src.Types
{
  public enum HurtTypes
  {
    PHYSICAL,
    MAGIC_ICE,
    MAGIC_FIRE,
    MAGIC_THUNDER,
    POISON,
  }

  public struct SkillHurt
  {
    public HurtTypes HurtType { get; set; }
    public float HurtPoint { get; set; }
  }

  #region Additional Effect

  [Serializable]
  public enum BasicAdditionalEffectTypes
  {
    NONE,
    FREEZE,
    BURN,
    STUN,
  }

  public abstract class BasicAdditionalEffect
  {
    public float Possibility { get; set; }

    public BasicAdditionalEffect()
    {
    }

    public BasicAdditionalEffect(BasicAdditionalEffect other)
    {
      Possibility = other.Possibility;
    }
  }

  public class FireAdditionalEffect : BasicAdditionalEffect
  {
    public float HurtPercentagePerSecond { get; set; }
    public float LastForSeconds { get; set; }

    public FireAdditionalEffect()
    {
    }

    public FireAdditionalEffect(FireAdditionalEffect other) : base(other)
    {
      HurtPercentagePerSecond = other.HurtPercentagePerSecond;
      LastForSeconds = other.LastForSeconds;
    }
  }

  public class ThunderAdditionalEffect : BasicAdditionalEffect
  {
    public float StunningSeconds { get; set; }

    public ThunderAdditionalEffect()
    {
    }

    public ThunderAdditionalEffect(ThunderAdditionalEffect other) : base(other)
    {
      StunningSeconds = other.StunningSeconds;
    }
  }

  public class IceAdditionalEffect : BasicAdditionalEffect
  {
    public float DamagePerSecond { get; set; }
    public float LastForSeconds { get; set; }

    public IceAdditionalEffect()
    {
    }

    public IceAdditionalEffect(IceAdditionalEffect other) : base(other)
    {
      DamagePerSecond = other.DamagePerSecond;
      LastForSeconds = other.LastForSeconds;
    }
  }

  public class PoisonAdditionalEffect
  {
    // Indicate to EnemyHurt if it needs to be handled since in AdditionalEffect, PoisonAE will be assigned automatically which is not a good practice
    public bool Enable { get; set; }

    public float HurtPercentagePerSecond { get; set; }
    public float Possibility { get; set; }

    public object Clone()
    {
      return ObjectCopier.Clone(this);
    }
  }

  public class SkillAdditionalEffect
  {
    /**
     * BasicAdditionalEffectTypes indicates which type it is. Since BasicAdditionalEffect will always use its derived type (Fire/Ice/ThunderAdditionalEffect).
     */
    public BasicAdditionalEffectTypes BasicAdditionalEffectType { get; set; }

    public BasicAdditionalEffect BasicAdditionalEffect { get; set; }
    public PoisonAdditionalEffect PoisonAdditionalEffect { get; set; } = new();
  }

  #endregion

  [Serializable]
  public enum SkillTag
  {
    BASE_SKILL,
    TIMING_SKILL,
    SURROUND_SKILL,

    MUTABLE_BULLET,

    MUTABLE_HURT_TYPE,
    PHYSICAL_SKILL,
    ICE_SKILL,
    FIRE_SKILL,
    THUNDER_SKILL,
    POISON_SKILL,
  }

  [Serializable]
  public struct RangeAttack
  {
    public bool Enabled { get; set; }
    public float DamageRange { get; set; }

    public RangeAttack Clone()
    {
      return (RangeAttack)MemberwiseClone();
    }
  }

  [Serializable]
  public struct BasicSkillHitContext
  {
    public ObjectPool ObjectPool { get; set; }
    public Vector2 CurrentPosition { get; set; }
    public BasicSkillConfigurations SkillConfigurations { get; set; }
  }

  #region Skill Configurations

  [Serializable]
  public class BaseSkillConfigurations : ICloneable
  {
    // FIXME: This is a bad design
    // This will be set automatically in SkillAndUpgradeManager. It's used to keep skill identity.
    public string SkillId { get; set; }

    public List<SkillTag> SkillTags { get; set; } = new();
    public SkillHurt SkillHurt { get; set; } = new();
    public float Speed { get; set; }
    public float RepelForce { get; set; }
    public SkillAdditionalEffect AdditionalEffect { get; set; } = new();
    public List<Action<BasicSkillHitContext>> SkillHitInterceptors { get; set; } = new();
    public List<string> LaunchSoundEffectNames { get; set; } = new();
    public List<string> AdditionalLaunchSoundEffectNames { get; set; } = new();

    public BaseSkillConfigurations()
    {
    }

    public BaseSkillConfigurations(BaseSkillConfigurations other)
    {
      SkillId = other.SkillId;
      SkillTags = other.SkillTags.Select(t => t).ToList();
      SkillHurt = ObjectCopier.Clone(other.SkillHurt);
      Speed = other.Speed;
      RepelForce = other.RepelForce;

      var ae = new SkillAdditionalEffect
      {
        BasicAdditionalEffectType = other.AdditionalEffect.BasicAdditionalEffectType
      };
      ae.BasicAdditionalEffect = other.AdditionalEffect.BasicAdditionalEffectType switch
      {
        BasicAdditionalEffectTypes.FREEZE => new IceAdditionalEffect(
          (IceAdditionalEffect)other.AdditionalEffect.BasicAdditionalEffect),
        BasicAdditionalEffectTypes.BURN => new FireAdditionalEffect(
          (FireAdditionalEffect)other.AdditionalEffect.BasicAdditionalEffect),
        BasicAdditionalEffectTypes.STUN => new ThunderAdditionalEffect(
          (ThunderAdditionalEffect)other.AdditionalEffect.BasicAdditionalEffect),
        _ => ae.BasicAdditionalEffect
      };

      ae.PoisonAdditionalEffect = new PoisonAdditionalEffect();
      if (other.AdditionalEffect.PoisonAdditionalEffect.Enable)
      {
        ae.PoisonAdditionalEffect.Enable = true;
        ae.PoisonAdditionalEffect = (PoisonAdditionalEffect)other.AdditionalEffect.PoisonAdditionalEffect.Clone();
      }

      AdditionalEffect = ae;

      SkillHitInterceptors = other.SkillHitInterceptors.Select(t => (Action<BasicSkillHitContext>)t.Clone()).ToList();
      LaunchSoundEffectNames = other.LaunchSoundEffectNames.Select(s => s).ToList();
      AdditionalLaunchSoundEffectNames = other.AdditionalLaunchSoundEffectNames.Select(s => s).ToList();
    }

    public virtual object Clone()
    {
      return new BaseSkillConfigurations(this);
    }
  }

  [Serializable]
  public class BasicSkillConfigurations : BaseSkillConfigurations
  {
    public RangeAttack RangeAttack { get; set; }
    public TrajectoryTypes TrajectoryType { get; set; }

    // Dispersion is defined as follows: The toRange of bias rad for normalized vector.
    public float Dispersion { get; set; }

    public float Range { get; set; }
    public int Penetration { get; set; }

    public BasicSkillConfigurations()
    {
    }

    public BasicSkillConfigurations(BasicSkillConfigurations other) : base(other)
    {
      RangeAttack = other.RangeAttack.Clone();
      TrajectoryType = other.TrajectoryType;
      Dispersion = other.Dispersion;
      Range = other.Range;
      Penetration = other.Penetration;
    }

    public override object Clone()
    {
      return new BasicSkillConfigurations(this);
    }
  }

  public class CircularSkillConfigurations : BasicSkillConfigurations
  {
    public float AnglePerSecond { get; set; }
    public float AnglePerSecondIncrementPerSecond { get; set; }

    public CircularSkillConfigurations()
    {
    }

    public CircularSkillConfigurations(CircularSkillConfigurations other) : base(other)
    {
      AnglePerSecond = other.AnglePerSecond;
      AnglePerSecondIncrementPerSecond = other.AnglePerSecondIncrementPerSecond;
    }

    public override object Clone()
    {
      return new CircularSkillConfigurations(this);
    }
  }

  public class ActiveSkillConfigurations : ICloneable
  {
    public float Countdown { get; set; }

    public ActiveSkillConfigurations()
    {
    }

    public ActiveSkillConfigurations(ActiveSkillConfigurations other)
    {
      Countdown = other.Countdown;
    }

    public virtual object Clone()
    {
      return new ActiveSkillConfigurations(this);
    }
  }

  public class PersistActiveSkillConfigurations : ActiveSkillConfigurations
  {
    public float Duration { get; set; }

    public PersistActiveSkillConfigurations()
    {
    }

    public PersistActiveSkillConfigurations(PersistActiveSkillConfigurations other) : base(other)
    {
      Duration = other.Duration;
    }

    public override object Clone()
    {
      return new PersistActiveSkillConfigurations(this);
    }
  }

  public class BaseEnemySkillConfigurations
  {
    public float HitPoint { get; set; }
    public EnemySkillStrengthUpgrade StrengthUpgrade { get; set; }
    public float Speed { get; set; }
    public SkillAdditionalEffect AdditionalEffect { get; set; } = new();
    public string LaunchSoundEffectName { get; set; }
  }

  public class RangingEnemySkillConfigurations : BaseEnemySkillConfigurations
  {
    public float Range { get; set; }
  }

  public class LinearEnemySkillConfigurations : BaseEnemySkillConfigurations
  {
    public Func<float, float> df { get; set; } // The derivative y relative to lifetime.
  }

  #endregion

  #region Player Skill

  public abstract class PlayerSkill : PauseableGameObject, IPoolableGameObject
  {
    public bool PlaySoundWhileInit = true;
    public int PlayerNumber { get; set; }
    public string ObjectName { get; set; }

    public bool Activated { get; set; } = true;

    public BaseSkillConfigurations SkillConfigurations { get; protected set; }

    protected Vector3 initialEulerAngles;
    protected ObjectPool objectPool;
    protected ResourceManager resourceManager;

    protected Vector2 relativeDirection = Vector2.right; // Direction relative to current position.
    protected Vector2 targetPosition; // targetPosition does not garentee it would be accurate target position.

    protected AudioWrapper launchWrapper;
    protected AudioWrapper additionalLaunchWrapper;

    private string uniqueId;

    protected override void Awake()
    {
      base.Awake();
      GenerateUniqueId();
      objectPool = ObjectPool.Instance;
      resourceManager = ResourceManager.Instance;
      initialEulerAngles = transform.rotation.eulerAngles;
    }

    protected override void Start()
    {
      base.Start();

      if (PlaySoundWhileInit)
      {
        PlayLaunchSound();
      }
    }

    protected void InitSounds()
    {
      launchWrapper = new AudioWrapper(SkillConfigurations.LaunchSoundEffectNames.ToArray(), transform);
      additionalLaunchWrapper =
        new AudioWrapper(SkillConfigurations.AdditionalLaunchSoundEffectNames.ToArray(), transform);
    }

    public virtual void ObjectReset(Vector2 initialPosition)
    {
      GenerateUniqueId();
      gameObject.SetActive(true);

      transform.position = initialPosition;

      if (PlaySoundWhileInit)
      {
        PlayLaunchSound();
      }
    }

    public void PlayLaunchSound()
    {
      if (launchWrapper == null || additionalLaunchWrapper == null)
      {
        InitSounds();
      }

      launchWrapper.PlayRandomly();
      additionalLaunchWrapper.PlayRandomly();
    }

    protected override void HandleGameStateChanged(GameState state)
    {
      GameStates gameState = state.CurrentGameState;
      paused = !LevelUtils.PlayerCanMove(gameState);
      if (animator != null)
      {
        animator.speed = paused ? 0 : 1;
      }
    }

    public void GenerateUniqueId()
    {
      uniqueId = Guid.NewGuid().ToString();
    }

    public string UniqueId => uniqueId;
  }

  public abstract class PlayerBasicSkill : PlayerSkill
  {
    public virtual void SetSkillConfigurations(BasicSkillConfigurations skillConfigurations)
    {
      SkillConfigurations = skillConfigurations;
    }

    /**
     * Direction that relative to firing position.
     */
    public virtual void SetTargetRelativeDirection(Vector2 relativeDirection)
    {
      this.relativeDirection = relativeDirection.normalized;
      targetPosition = (Vector2)transform.position + relativeDirection;
      transform.eulerAngles = new Vector3(initialEulerAngles.x, initialEulerAngles.y,
        Vector2.SignedAngle(Vector2.left, relativeDirection));
    }

    /**
     * Set Target position.
     * Be careful, target position represents the actual designated target position while relative direction represents direction relative to firing position.
     */
    public virtual void SetTargetPosition(Vector2 targetPosition)
    {
      this.targetPosition = targetPosition;
      relativeDirection = (targetPosition - (Vector2)transform.position).normalized;
      transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.left, relativeDirection));
    }
  }

  public abstract class PlayerActiveSkill
  {
    protected readonly StoreManager storeManager = StoreManager.Instance;
    protected Scheduling scheduling = Scheduling.Instance;
    protected SkillAndUpgradeManager skillAndUpgradeManager = SkillAndUpgradeManager.Instance;

    protected ActiveSkillConfigurations ActiveSkillConfigurations;

    public abstract void Launch(int playerNumber);

    public virtual PlayerActiveSkill SetSkillConfigurations(ActiveSkillConfigurations activeSkillConfigurations)
    {
      ActiveSkillConfigurations = activeSkillConfigurations;
      return this;
    }
  }

  #endregion
}