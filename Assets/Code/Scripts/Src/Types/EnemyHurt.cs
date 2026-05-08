using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.Animation;
using Code.Scripts.Behaviour.Character.Enemy;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.Animation;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Src.Types
{
  public abstract class EnemyHurt : PauseableGameObject
  {
    public float HealthPoint { get; set; }

    [CanBeNull] protected List<HurtTypes> DefendTypes { get; set; }
    [CanBeNull] protected List<HurtTypes> CriticalHurtTypes { get; set; }
    [CanBeNull] protected List<BasicAdditionalEffectTypes> AdditionalEffectImmunity { get; set; }
    protected bool PoisonImmunity { get; set; }

    public float FluctuationPercentage { get; set; }
    public float InitialHealthPoint { get; set; }
    public bool IsBoss { get; set; }

    protected ObjectPool objectPool;
    protected SkillAndUpgradeManager skillAndUpgradeManager;
    protected ResourceManager resourceManager;
    protected readonly List<string> hurtBy = new();
    protected CharacterStatus enemyStatus = CharacterStatus.NORMAL;
    protected GameObject rootGameObject;
    protected Enemy rootController;
    protected Rigidbody2D rootRb;
    protected readonly List<AudioClip> enemyHurtAudioClips = new();
    protected Transform animatorTransform;
    protected SpriteRenderer animatorSpriteRenderer;

    protected EnemyConfiguration enemyConfiguration;

    protected GameStates gameState;

    protected SimpleAnimation frozenAnimation;
    protected SimpleAnimation burningAnimation;
    protected SimpleAnimation stunningAnimation;
    protected SimpleAnimation poisonedAnimation;

    protected bool isPoisoned;
    protected float statusTimeout;
    protected float statusHurtTimeout; // This is used for taking hurt once per second.
    protected float statusHurtPoint;
    protected float statusHurtPercentage;
    protected float poisonedHurtPercentage;
    protected int persistentHurtPlayerNumber;

    private long uniqueId;
    private static long uniqueIdCounter = 0;

    private float playerSkillFissionPossibility;

    protected bool died;

    protected override void Awake()
    {
      base.Awake();
      resourceManager = ResourceManager.Instance;
      objectPool = ObjectPool.Instance;
      skillAndUpgradeManager = SkillAndUpgradeManager.Instance;

      rootGameObject = gameObject;
      while (!rootGameObject.CompareTag("RootNode"))
      {
        rootGameObject = rootGameObject.transform.parent.gameObject;
      }

      rootController = rootGameObject.GetComponent<Enemy>();

      if (!rootController)
        throw new System.Exception(
          $"Enemy must indicate the tag <RootNode> where contains the root controller. Check the hierarchy of {name}.");

      animatorTransform = rootGameObject.transform.Find("Animator");
      animator = animatorTransform.GetComponent<Animator>();

      rootRb = rootGameObject.GetComponent<Rigidbody2D>();
      animatorSpriteRenderer = animatorTransform.GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
      base.Start();

      var hurtSounds = enemyConfiguration.HurtSounds;

      foreach (var descriptor in hurtSounds)
      {
        AudioClip clip = resourceManager.GetResource(descriptor);
        if (clip)
        {
          enemyHurtAudioClips.Add(clip);
        }
        else if (DebugConfigurations.DebugEnabled)
        {
          throw new System.Exception($"Sound {descriptor} not found.");
        }
        else
        {
          Debug.LogWarning($"Sound {descriptor} not found.");
        }
      }

      storeManager.Subscribe(StoreNames.ActiveSkillStore, this);
    }

    public override void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.GameStateStore:
          HandleGameStateChanged((GameState)state);
          break;
        case StoreNames.ActiveSkillStore:
          playerSkillFissionPossibility = ((ActiveSkillState)state).BulletFissionPossibility;
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    public virtual void EnemyReset()
    {
      HealthPoint = InitialHealthPoint;
      enemyStatus = CharacterStatus.NORMAL;

      statusHurtPercentage = 0;
      poisonedHurtPercentage = 0;
      statusHurtPoint = 0;

      isPoisoned = false;

      died = false;

      hurtBy.Clear();

      RecycleAnimation();
    }

    public EnemyHurt()
    {
      GenerateUniqueId();
    }

    protected override void HandleGameStateChanged(GameState state)
    {
      var gameState = state.CurrentGameState;
      this.gameState = gameState;
      paused = CanMoveWhenTimeStopped
        ? !LevelUtils.PlayerCanMove(gameState)
        : LevelUtils.EverythingCanNotMove(gameState);
      if (animator != null)
      {
        animator.speed = paused ? 0 : 1;
      }
    }

    public void SetConfigurations(EnemyConfiguration config)
    {
      enemyConfiguration = config;

      IsBoss = config.IsBoss;

      HealthPoint = config.HealthPoint;
      InitialHealthPoint = config.HealthPoint;

      FluctuationPercentage = config.Fluctuation;
      DefendTypes = config.DefendTypes;
      CriticalHurtTypes = config.CriticalHurtTypes;
      AdditionalEffectImmunity = config.AdditionalEffectImmunity;
      PoisonImmunity = config.PoisonImmunity;
    }

    public void GenerateUniqueId()
    {
      uniqueId = System.Threading.Interlocked.Increment(ref uniqueIdCounter);
    }

    protected float GetRandomizedRelativeHurtPoint(HurtTypes hurtType, float hurtPoint)
    {
      hurtPoint = AttackUtils.GetHurtPointAfterDefenseAndCritical(hurtType, DefendTypes, CriticalHurtTypes, hurtPoint);
      hurtPoint *= Random.Range(1 - FluctuationPercentage, 1 + FluctuationPercentage);

      return Mathf.Max(hurtPoint, 1);
    }

    protected virtual void PlayOnDieAnimation(HurtTypes hurtType)
    {
      var animationMapping = new Dictionary<HurtTypes, string[]>()
      {
        {
          HurtTypes.MAGIC_ICE,
          new[]
          {
            "Status/EnemyDied/EnemyIceDied0",
            "Status/EnemyDied/EnemyIceDied1",
          }
        },
        {
          HurtTypes.MAGIC_FIRE,
          new[]
          {
            "Status/EnemyDied/EnemyFireDied0",
            "Status/EnemyDied/EnemyFireDied1",
          }
        },
        {
          HurtTypes.MAGIC_THUNDER,
          new[]
          {
            "Status/EnemyDied/EnemyThunderDied0",
            "Status/EnemyDied/EnemyThunderDied1",
          }
        },
        {
          HurtTypes.POISON,
          new[]
          {
            "Status/EnemyDied/EnemyPoisonDied0",
          }
        }
      };

      var bloodSplash = objectPool.GetObject("Effect/BloodSplash");
      bloodSplash.transform.SetParent(null);
      bloodSplash.transform.position = transform.position;

      if (animationMapping.TryGetValue(hurtType, out var effectNames))
      {
        var effectName = effectNames[Random.Range(0, effectNames.Length)];
        var explosion = objectPool.GetObject(effectName);
        explosion.transform.SetParent(null);
        explosion.transform.position = rootGameObject.transform.position;
        explosion.GetComponent<SimpleAnimation>().SetRecycleAnimationAfterFinished(true);
      }
      else
      {
        var dissolve = objectPool.GetObject("Enemy/DissolveEnemy");
        dissolve.transform.position = transform.position;
        dissolve.GetComponent<DissolveEnemy>()
          .SetSprite(animatorSpriteRenderer.sprite)
          .SetDirection(animatorTransform.localScale.x > 0
            ? DissolveEnemyDirection.LEFT
            : DissolveEnemyDirection.RIGHT);
      }
    }

    protected virtual void PlayOnHurtAnimation(HurtTypes hurtType)
    {
      var animationMapping = new Dictionary<HurtTypes, string[]>()
      {
        {
          HurtTypes.PHYSICAL,
          new[]
          {
            "Status/EnemyHurt/EnemyPhysicalHurt0",
          }
        },
        {
          HurtTypes.MAGIC_ICE,
          new[]
          {
            "Status/EnemyHurt/EnemyIceHurt0",
            "Status/EnemyHurt/EnemyIceHurt1",
          }
        },
        {
          HurtTypes.MAGIC_FIRE,
          new[]
          {
            "Status/EnemyHurt/EnemyFireHurt0",
            "Status/EnemyHurt/EnemyFireHurt1",
          }
        },
        {
          HurtTypes.MAGIC_THUNDER,
          new[]
          {
            "Status/EnemyHurt/EnemyThunderHurt0",
            "Status/EnemyHurt/EnemyThunderHurt1",
          }
        },
        {
          HurtTypes.POISON,
          new[]
          {
            "Status/EnemyHurt/EnemyPoisonHurt0",
          }
        }
      };

      var effectNames = animationMapping[hurtType];
      var effectName = effectNames[Random.Range(0, effectNames.Length)];
      var explosion = objectPool.GetObject(effectName);
      explosion.transform.SetParent(null);
      explosion.transform.position = rootGameObject.transform.position;
      explosion.GetComponent<SimpleAnimation>().SetRecycleAnimationAfterFinished(true);
    }

    protected void FadeOutStatusAnimation()
    {
      rootController.ResetMass();
      if (frozenAnimation != null)
      {
        frozenAnimation.FadeOutAnimation();
        frozenAnimation = null;
      }

      if (burningAnimation != null)
      {
        burningAnimation.FadeOutAnimation();
        burningAnimation = null;
      }

      if (stunningAnimation != null)
      {
        stunningAnimation.FadeOutAnimation();
        stunningAnimation = null;
      }
    }

    public void RecycleAnimation()
    {
      if (frozenAnimation != null)
      {
        frozenAnimation.RecycleAnimation();
        frozenAnimation = null;
      }

      if (burningAnimation != null)
      {
        burningAnimation.RecycleAnimation();
        burningAnimation = null;
      }

      if (stunningAnimation != null)
      {
        stunningAnimation.RecycleAnimation();
        stunningAnimation = null;
      }

      if (poisonedAnimation != null)
      {
        poisonedAnimation.RecycleAnimation();
        poisonedAnimation = null;
      }
    }

    protected virtual void HandleHurt(int playerNumber, float hurtPoint, HurtTypes hurtType,
      bool playHurtAnimation = true)
    {
      HealthPoint -= hurtPoint;

      if (HealthPoint <= 0)
      {
        PlayOnDieAnimation(hurtType);
        HandleEnemyDied(playerNumber, hurtType);
      }
      else
      {
        if (playHurtAnimation)
        {
          PlayOnHurtAnimation(hurtType);
        }

        HandleEnemyHurt(hurtType);
      }
    }

    protected virtual void HandleEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      if (Random.Range(0, 1000) >
          Mathf.Clamp(
            (1 - PlayerExperienceConfigurations.GetExpPossibility * enemyConfiguration.EnemyExpCoefficient) * 1000,
            0,
            1000)
         )
      {
        if (IsBoss)
        {
          var expPoint = objectPool.GetObject("Environment/BossExpPoint");
          expPoint.transform.position = rootGameObject.transform.position;
        }
        else
        {
          var expPoint = objectPool.GetObject("Environment/ExpPoint");
          expPoint.transform.position = rootGameObject.transform.position;
        }
      }

      OnEnemyDied(playerNumber, hurtType);

      var dieContext = new EnemyDieContext
      {
        PlayerNumber = playerNumber,
        EnemyPosition = transform.position
      };
      var enemyDieInterceptors =
        skillAndUpgradeManager.SkillAndUpgradeControllers[playerNumber].EnemyDieInterceptors;
      foreach (var dieInterceptor in enemyDieInterceptors)
      {
        dieContext = dieInterceptor.OnEnemyDie(dieContext);
      }

      var enemyDieActions = skillAndUpgradeManager.SkillAndUpgradeControllers[playerNumber].EnemyDieActions;
      foreach (var action in enemyDieActions)
      {
        dieContext = action(dieContext);
      }
    }

    protected virtual void HandleEnemyHurt(HurtTypes hurtType)
    {
      if (enemyHurtAudioClips != null && enemyHurtAudioClips.Any())
      {
        AudioWrapper.PlayClip(enemyHurtAudioClips[Random.Range(0, enemyHurtAudioClips.Count)],
          transform.position);
        rootController.Blink();
      }
    }

    protected virtual void HandleDirectHurt(int playerNumber, BaseSkillConfigurations skillConfigurations,
      float hurtPoint)
    {
      var hurtContext = new EnemyHurtContext()
      {
        PlayerNumber = playerNumber,
        HitPoint = hurtPoint,
        CurrentPosition = transform.position,
        IsBoss = IsBoss,
        SkillConfigurations = skillConfigurations,
      };

      var enemyHurtInterceptors = skillAndUpgradeManager.SkillAndUpgradeControllers[playerNumber]
        .EnemyDirectHurtInterceptors;
      foreach (var hurtInterceptor in enemyHurtInterceptors)
      {
        hurtContext = hurtInterceptor.OnEnemyHurt(hurtContext);
      }

      var enemyHurtActions =
        skillAndUpgradeManager.SkillAndUpgradeControllers[playerNumber].EnemyDirectHurtActions;
      foreach (var action in enemyHurtActions)
      {
        hurtContext = action(hurtContext);
      }

      HandleHurt(playerNumber, hurtContext.HitPoint, skillConfigurations.SkillHurt.HurtType);
    }

    protected virtual void ShowHurtValue(float hurtPoint, HurtTypes hurtType)
    {
      GameObject hurtValue = objectPool.GetObject("Environment/HurtValueTMP");
      HurtValue hurtValueController = hurtValue.GetComponent<HurtValue>();

      hurtValueController
        .SetDefaultPosition((Vector2)rootGameObject.transform.position +
                            GeneralUtils.Rotate(Vector2.up * 0.3f, Random.Range(-90, 90)))
        .SetText(hurtPoint)
        .SetHurtType(hurtType);
    }

    protected virtual void HandleFreezeAdditionalEffect(IceAdditionalEffect additionalEffect)
    {
      statusTimeout = !IsBoss ? additionalEffect.LastForSeconds : Random.Range(0.5f, 1);
      statusHurtPoint = additionalEffect.DamagePerSecond;
      statusHurtTimeout = 1;
      rootRb.mass *= 10;
      rootController.StopHandlingMovementFor(additionalEffect.LastForSeconds + 0.5f);

      if (enemyStatus.Equals(CharacterStatus.FROZEN))
      {
        return;
      }

      FadeOutStatusAnimation();
      enemyStatus = CharacterStatus.FROZEN;
      var frozenList = new[]
      {
        "Status/Frozen0",
        "Status/Frozen1",
        "Status/Frozen2",
      };

      var frozen = objectPool.GetObject(frozenList[Random.Range(0, frozenList.Length)]);
      frozen.transform.SetParent(rootGameObject.transform);
      frozen.transform.localPosition = Vector2.zero;

      frozenAnimation = frozen.GetComponent<SimpleAnimation>();
    }

    protected virtual void HandleBurnAdditionalEffect(FireAdditionalEffect additionalEffect)
    {
      if (enemyStatus.Equals(CharacterStatus
            .FROZEN)) // If transit fromQuat Frozen status to burning status, enemy should move again.
      {
        rootController.StartHandlingMovement();
      }

      FadeOutStatusAnimation();
      enemyStatus = CharacterStatus.BURNING;
      statusTimeout = additionalEffect.LastForSeconds;
      statusHurtPercentage =
        IsBoss ? additionalEffect.HurtPercentagePerSecond : additionalEffect.HurtPercentagePerSecond / 5f;
      statusHurtTimeout = 1;

      var burningList = new[]
      {
        "Status/Burning0",
        "Status/Burning1"
      };

      var burning = objectPool.GetObject(burningList[Random.Range(0, burningList.Length)]);
      burning.transform.SetParent(rootGameObject.transform);
      burning.transform.localPosition = Vector2.zero;

      burningAnimation = burning.GetComponent<SimpleAnimation>();
    }

    protected virtual void HandleStunAdditionalEffect(ThunderAdditionalEffect additionalEffect)
    {
      statusTimeout = !IsBoss ? additionalEffect.StunningSeconds : additionalEffect.StunningSeconds / 2f;
      statusHurtTimeout = 1;
      rootController.StopHandlingMovementFor(additionalEffect.StunningSeconds + 0.5f);

      if (enemyStatus.Equals(CharacterStatus.STUNNING))
      {
        return;
      }

      FadeOutStatusAnimation();
      enemyStatus = CharacterStatus.STUNNING;

      var stunList = new[]
      {
        "Status/Stunning0"
      };

      var stunning = objectPool.GetObject(stunList[Random.Range(0, stunList.Length)]);
      stunning.transform.SetParent(rootGameObject.transform);
      stunning.transform.localPosition = new Vector2(0, rootController.GetSize().y / 2 + 0.1f);

      stunningAnimation = stunning.GetComponent<SimpleAnimation>();
    }

    protected virtual void HandlePoisonAdditionalEffects(PoisonAdditionalEffect additionalEffect)
    {
      poisonedHurtPercentage = additionalEffect.HurtPercentagePerSecond;
      rootController.OnEnemyPoisoned();

      if (!isPoisoned)
      {
        isPoisoned = true;

        var poisonedList = new[]
        {
          "Status/Poisoned"
        };

        var poisoned = objectPool.GetObject(poisonedList[Random.Range(0, poisonedList.Length)]);
        poisoned.transform.SetParent(rootGameObject.transform);
        poisonedAnimation = poisoned.GetComponent<SimpleAnimation>();
        var size = rootController.GetSize();
        poisoned.transform.localPosition = new Vector2(size.x / 3, size.y / 2 + 0.1f);
      }
    }

    public virtual void HandleSkillAdditionalEffects(int playerNumber, SkillAdditionalEffect additionalEffect)
    {
      if (additionalEffect.BasicAdditionalEffect != null)
      {
        var possibility = additionalEffect.BasicAdditionalEffect.Possibility;
        if (possibility >= Random.Range(0, 1f))
        {
          persistentHurtPlayerNumber = playerNumber;
          switch (additionalEffect.BasicAdditionalEffectType)
          {
            case BasicAdditionalEffectTypes.FREEZE:
              if (AdditionalEffectImmunity == null ||
                  AdditionalEffectImmunity.All(i => i != BasicAdditionalEffectTypes.FREEZE))
              {
                HandleFreezeAdditionalEffect((IceAdditionalEffect)additionalEffect.BasicAdditionalEffect);
              }

              break;
            case BasicAdditionalEffectTypes.BURN:
              if (AdditionalEffectImmunity == null ||
                  AdditionalEffectImmunity.All(i => i != BasicAdditionalEffectTypes.BURN))
              {
                HandleBurnAdditionalEffect((FireAdditionalEffect)additionalEffect.BasicAdditionalEffect);
              }

              break;
            case BasicAdditionalEffectTypes.STUN:
              if (AdditionalEffectImmunity == null ||
                  AdditionalEffectImmunity.All(i => i != BasicAdditionalEffectTypes.STUN))
              {
                HandleStunAdditionalEffect((ThunderAdditionalEffect)additionalEffect.BasicAdditionalEffect);
              }

              break;
          }
        }
      }

      if (!PoisonImmunity && additionalEffect.PoisonAdditionalEffect.Enable)
      {
        var possibility = additionalEffect.PoisonAdditionalEffect.Possibility;
        if (possibility >= Random.Range(0, 1f))
        {
          persistentHurtPlayerNumber = playerNumber;
          HandlePoisonAdditionalEffects(additionalEffect.PoisonAdditionalEffect);
        }
      }
    }

    public virtual void TriggerEnemyStun(float seconds)
    {
      statusTimeout = !IsBoss ? seconds : Random.Range(0.5f, 1);
      statusHurtTimeout = 1;
      rootController.StopHandlingMovementFor(seconds + 0.5f);

      if (enemyStatus.Equals(CharacterStatus.STUNNING))
      {
        return;
      }

      FadeOutStatusAnimation();
      enemyStatus = CharacterStatus.STUNNING;

      var stunList = new[]
      {
        "Status/Stunning0"
      };

      var stunning = objectPool.GetObject(stunList[Random.Range(0, stunList.Length)]);
      stunning.transform.SetParent(rootGameObject.transform);
      stunning.transform.localPosition = new Vector2(0, rootController.GetSize().y / 2 + 0.1f);

      stunningAnimation = stunning.GetComponent<SimpleAnimation>();
    }

    /**
     * If triggerPosition is present, will add a repel force to enemy.
     */
    public virtual void TriggerEnemyHurt(int playerNumber, PlayerSkill playerSkill, Vector2 triggerPosition)
    {
      if (died)
      {
        return;
      }

      var direction = ((Vector2)transform.position - triggerPosition).normalized;
      rootController.AddRepelForce(playerSkill.SkillConfigurations.RepelForce, direction);

      TriggerEnemyHurt(playerNumber, playerSkill);
    }

    public virtual void TriggerEnemyHurt(int playerNumber, PlayerSkill playerSkill)
    {
      if (hurtBy.Contains(playerSkill.UniqueId) || died)
      {
        return;
      }

      hurtBy.Add(playerSkill.UniqueId);

      #region Calc total hurt.

      float totalHurtPoint = 0;

      var hurt = playerSkill.SkillConfigurations.SkillHurt;

      var relativeHurtPoint = GetRandomizedRelativeHurtPoint(hurt.HurtType, hurt.HurtPoint);
      totalHurtPoint += relativeHurtPoint;

      totalHurtPoint = Mathf.RoundToInt(totalHurtPoint);

      #endregion

      HandleDirectHurt(playerNumber, playerSkill.SkillConfigurations, totalHurtPoint);
      ShowHurtValue(totalHurtPoint, hurt.HurtType);

      var additionalEffect = playerSkill.SkillConfigurations.AdditionalEffect;
      HandleSkillAdditionalEffects(playerNumber, additionalEffect);
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
      if (died)
      {
        return;
      }

      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer))
      {
        return;
      }

      var playerSkill = other.gameObject.GetComponent<PlayerSkill>();
      if (playerSkill == null || !playerSkill.Activated)
      {
        return;
      }

      var playerNumber = playerSkill.PlayerNumber;

      // Refer these logics from SkillAndUpgradeManager
      if (!hurtBy.Contains(playerSkill.UniqueId) &&
          SkillPresets.PlayerBasicSkillPrefabNames.Contains(playerSkill.ObjectName) &&
          Random.Range(0, 1f) <= playerSkillFissionPossibility)
      {
        var number = Random.Range(2, 5);
        var originSkillConfigs = (BasicSkillConfigurations)playerSkill.SkillConfigurations;

        for (var i = 0; i < number; i++)
        {
          var newSkill = objectPool.GetObject(other.GetComponent<IPoolableGameObject>().ObjectName);
          newSkill.transform.SetParent(null);
          newSkill.transform.position = transform.position;

          var newSkillController = newSkill.GetComponent<PlayerBasicSkill>();
          newSkillController.SetSkillConfigurations(originSkillConfigs);
          newSkillController.CanMoveWhenTimeStopped = true;
          newSkillController.PlayerNumber = playerNumber;

          hurtBy.Add(newSkillController.UniqueId);

          var trajectoryArguments = new TrajectoryArguments()
          {
            PlayerPosition = transform.position,
            TargetPosition = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector3.forward,
            LaunchPosition = transform.position,
          };
          var trajectoryLaunch = SkillAndUpgradeManager.GetTrajectory(
            ((BasicSkillConfigurations)newSkillController.SkillConfigurations)
            .TrajectoryType);
          trajectoryLaunch(trajectoryArguments, new() { newSkill }, Random.Range(0, 100));
        }
      }

      TriggerEnemyHurt(playerNumber, playerSkill, other.gameObject.transform.position);
    }

    protected float GetRandomizedHurtPoint(float hurtPoint)
    {
      return hurtPoint * Random.Range(1 - FluctuationPercentage, 1 + FluctuationPercentage);
    }

    protected virtual void Update()
    {
      if ((!enemyStatus.Equals(CharacterStatus.NORMAL) || isPoisoned) && !paused)
      {
        if (!gameState.Equals(GameStates
              .ONLY_PLAYER_CAN_MOVE)) // If game state equals to TIME_PAUSED_SKILL, then should not calculate effect countdown.
        {
          statusTimeout -= Time.deltaTime;
          statusHurtTimeout -= Time.deltaTime;
        }

        // Calculate status hurt.
        if (statusHurtTimeout <= 0)
        {
          statusHurtTimeout = 1;

          if (!enemyStatus.Equals(CharacterStatus.NORMAL))
          {
            var hurtType = SkillUtils.GetHurtTypeFromEnemyStatusType(enemyStatus);

            var totalNormalHurtPoint = GetRandomizedRelativeHurtPoint(hurtType,
              statusHurtPoint +
              statusHurtPercentage *
              InitialHealthPoint);

            HandleHurt(persistentHurtPlayerNumber, totalNormalHurtPoint, hurtType, false);
            ShowHurtValue(totalNormalHurtPoint, hurtType);
          }

          if (isPoisoned)
          {
            var poisonHurtPoint = GetRandomizedHurtPoint(poisonedHurtPercentage * InitialHealthPoint);

            HandleHurt(persistentHurtPlayerNumber, poisonHurtPoint, HurtTypes.POISON, false);
            ShowHurtValue(poisonHurtPoint, HurtTypes.POISON);
          }
        }

        if (statusTimeout <= 0)
        {
          enemyStatus = CharacterStatus.NORMAL;
          FadeOutStatusAnimation();
        }
      }
    }

    public virtual void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      if (died)
      {
        return;
      }

      died = true;
      RecycleAnimation();
      rootController.OnEnemyDied(playerNumber, hurtType);
    }

    public string UniqueId => uniqueId;
  }
}