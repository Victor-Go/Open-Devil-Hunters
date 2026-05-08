using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.Animation;
using Code.Scripts.Behaviour.Character.Enemy;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.Animation;
using Code.Scripts.Behaviour.Character.Enemy;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Src.Types
{
  public enum EnemyRole
  {
    Normal,
    Boss,
  }

  public abstract class Enemy : PauseableGameObject
  {
    public EnemyRole EnemyRole;

    protected int numberOfPlayers;
    protected bool[] playerAlive;

    public EnemyNames EnemyConfigurationName;

    protected EnemyConfiguration initEnemyConfigurations;

    protected EnemyConfiguration
      updatedEnemyConfigurations; // This is updated when it's been picked from the object pool

    protected Color astonishedColor = new Color(153f / 255, 153f / 255, 153f / 255, 1);
    protected Color poisonedColor = new Color(0f / 255, 159f / 255, 14f / 255, 1);

    protected ResourceManager resourceManager;
    protected Scheduling scheduling;
    protected ObjectPool objectPool;

    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Material defaultMaterial;
    protected Material whiteOverlayMaterial;
    protected Material outlineMaterial;
    protected Transform animatorTransform;
    protected List<AudioClip> enemyDeadAudioClips = new();

    private EnemyCollisionHit enemyCollisionHit;
    public EnemyHurt enemyHurtController { get; set; }

    protected float initialMass;
    protected EnemyNames enemyId;
    protected bool blinking;
    protected Vector2 spriteSize;

    protected float
      totalStrength; // totalStrength: Enemy strength + level strength (Strength is calculated by f(difficulty)

    protected float baseStrength; // Base baseStrength defined by enemy configurations
    protected bool poisoned;
    protected bool stopHandlingMovement;
    protected float stopHandlingMovementTimeout;

    // Select a player to chase
    protected Vector2[] playerPositions;
    protected int selectedPlayerNumber;
    protected float selectPlayerInterval { get; } = 1;
    protected float selectPlayerTimeout;
    protected float chaseRandomThresholdDistance = 1;

    public long EnemyUniqueId { get; protected set; }
    protected Vector3 InitialScale;
    private static long enemyUniqueIdCounter = 0;

    protected override void Awake()
    {
      base.Awake();

      EnemyUniqueId = System.Threading.Interlocked.Increment(ref enemyUniqueIdCounter);

      rb = GetComponent<Rigidbody2D>();
      spriteRenderer = GetComponentInChildren<SpriteRenderer>();
      animatorTransform = transform.Find("Animator");

      resourceManager = ResourceManager.Instance;
      scheduling = Scheduling.Instance;
      objectPool = ObjectPool.Instance;

      spriteSize = spriteRenderer.size;
      initialMass = rb.mass;

      defaultMaterial = spriteRenderer.material;

      enemyCollisionHit = GetComponentInChildren<EnemyCollisionHit>();
      enemyHurtController = GetComponentInChildren<EnemyHurt>();
      if (EnemyConfigurations.Normal.TryGetValue(EnemyConfigurationName, out var value))
      {
        initEnemyConfigurations = ObjectCopier.Clone(value);
      }
      else if (EnemyConfigurations.Boss.TryGetValue(EnemyConfigurationName, value: out var value1))
      {
        initEnemyConfigurations = ObjectCopier.Clone(value1);
      }
      else if (EnemyConfigurations.Boss.TryGetValue(EnemyConfigurationName, out var value2))
      {
        initEnemyConfigurations = ObjectCopier.Clone(value2);
      }
      else
      {
        throw new System.Exception(
          $"EnemyConfigurationName {EnemyConfigurationName} not found in EnemyConfigurations (Node name: {name}).");
      }

      if (!enemyHurtController)
        throw new System.Exception(
          $"Enemy must have Hurt node to get skill hurt. Check the hierachy of {EnemyConfigurationName} (Node name: {name}).");
      if (!enemyCollisionHit)
        throw new System.Exception(
          $"Enemy must have Hit node to hurt player. Check the hierachy of {EnemyConfigurationName} (Node name: {name}).");

      enemyHurtController.SetConfigurations(initEnemyConfigurations);
      enemyCollisionHit.SetConfigurations(initEnemyConfigurations);

      updatedEnemyConfigurations = ObjectCopier.Clone(initEnemyConfigurations);
    }


    protected override void Start()
    {
      base.Start();
      InitialScale = transform.localScale;

      whiteOverlayMaterial = resourceManager.GetResource("Materials/WhiteOverlay");

      storeManager.Subscribe(StoreNames.LevelConfigurationStore, this);

      var playerPositionState = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore);
      playerPositions = playerPositionState.PlayerPositions;
      numberOfPlayers = playerPositionState.NumberOfPlayers;
      playerAlive = playerPositionState.PlayerAlive;

      var deadSounds = initEnemyConfigurations.DeadSounds;
      foreach (var descriptor in deadSounds)
      {
        AudioClip clip = resourceManager.GetResource(descriptor);
        if (clip)
        {
          enemyDeadAudioClips.Add(clip);
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

      SelectOnePlayerToChase();
    }

    public virtual void EnemyReset()
    {
      EnemyUniqueId = System.Threading.Interlocked.Increment(ref enemyUniqueIdCounter);

      updatedEnemyConfigurations = ObjectCopier.Clone(initEnemyConfigurations);
      poisoned = false;
      spriteRenderer.color = Color.white;
      paused = LevelUtils.EverythingCanNotMove(storeManager.GetState<GameState>(StoreNames.GameStateStore)
        .CurrentGameState);
      animator.speed = paused ? 0 : 1;

      ResetMass();
      enemyHurtController.EnemyReset();
      StartHandlingMovement();
    }

    protected Vector2 GetPlayersCenterPosition()
    {
      var sum = Vector2.zero;
      for (var i = 0; i < playerAlive.Length; i++)
      {
        if (!playerAlive[i])
        {
          continue;
        }

        sum += playerPositions[i];
      }

      return sum / playerAlive.Count(p => p);
    }

    public override void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          var levelConfigs = (LevelConfigurationState)state;
          numberOfPlayers = levelConfigs.NumberOfPlayers;
          playerAlive = levelConfigs.PlayerAlive;
          break;
        case StoreNames.GameStateStore:
          HandleGameStateChanged((GameState)state);
          break;
        case StoreNames.PlayerPositionStore:
          playerPositions = ((PlayerPositionState)state).PlayerPositions;
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    protected virtual void SelectOnePlayerToChase()
    {
      if (playerPositions == null)
      {
        return;
      }

      selectedPlayerNumber = 0;
      var nearest = float.MaxValue;
      for (var i = 0; i < playerAlive.Length; i++)
      {
        if (!playerAlive[i])
        {
          continue;
        }

        var distance = ((Vector2)transform.position - playerPositions[i]).sqrMagnitude;
        if (distance <= nearest ||
            (distance <= nearest + Mathf.Pow(chaseRandomThresholdDistance, 2) &&
             Random.Range(0, 1f) > 0.5f))
        {
          nearest = distance;
          selectedPlayerNumber = i;
        }
      }
    }

    protected virtual void SetAnimatorDirection(Vector2 direction)
    {
      if (direction.x > GeneralConfigurations.AnimationDirectionSettingThreshold)
      {
        animatorTransform.localScale = new Vector3(-InitialScale.x, InitialScale.y, InitialScale.z);
      }
      else if (direction.x < -GeneralConfigurations.AnimationDirectionSettingThreshold)
      {
        animatorTransform.localScale = InitialScale;
      }
    }

    public AnimatorDirection GetAnimatorDirection()
    {
      return animatorTransform.localScale.x > 0 ? AnimatorDirection.Left : AnimatorDirection.Right;
    }

    /**
     * Sets base strength of the enemy.
     * Warning: Enemy Strength is different from Level Strength.
     */
    public virtual void SetEnemyBaseStrength(float strength)
    {
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      var levelStrength = levelConfigs.LevelDifficulty;

      totalStrength = strength + levelStrength;
      baseStrength = strength;

      var StrengthUpgrade = initEnemyConfigurations.EnemyStrengthUpgrade;
      var twoPlayerUpgrades = initEnemyConfigurations.Enemy2PlayersUpgrade;

      updatedEnemyConfigurations.HealthPoint = initEnemyConfigurations.HealthPoint *
                                               Mathf.Pow(1 + StrengthUpgrade.IncreaseHpCoefficient,
                                                 totalStrength);
      updatedEnemyConfigurations.HealthPoint *= numberOfPlayers == 2
        ? 1 + twoPlayerUpgrades.IncreaseHpCoefficient
        : 1;

      updatedEnemyConfigurations.Speed *= initEnemyConfigurations.Speed *
                                          Mathf.Pow(1 + StrengthUpgrade.IncreaseSpeedCoefficient, totalStrength);
      updatedEnemyConfigurations.Speed *= numberOfPlayers == 2 ? 1 + twoPlayerUpgrades.IncreaseSpeedCoefficient : 1;
      updatedEnemyConfigurations.Speed =
        Mathf.Clamp(updatedEnemyConfigurations.Speed, 0.33f, GeneralConfigurations.EnemyMaxSpeed);

      enemyHurtController.SetConfigurations(updatedEnemyConfigurations);

      enemyCollisionHit.SetStrength(strength);
    }

    public virtual void StopHandlingMovementFor(float seconds)
    {
      stopHandlingMovement = true;
      stopHandlingMovementTimeout = Mathf.Max(seconds, stopHandlingMovementTimeout);

      animator.speed = 0;
    }

    public virtual void StartHandlingMovement()
    {
      animator.speed = 1;
      stopHandlingMovement = false;

      spriteRenderer.color = poisoned ? poisonedColor : Color.white;
    }

    public virtual void AddRepelForce(float force, Vector2 direction)
    {
      if (!(force > 0)) return;

      switch (EnemyRole)
      {
        case EnemyRole.Normal: StopHandlingMovementFor(Mathf.Clamp(-(1 / (2 * force + 1)) + 1, 0.1f, 1f)); break;
        case EnemyRole.Boss: StopHandlingMovementFor(Mathf.Clamp((-(1 / (2 * force + 1)) + 1) / 1.5f, 0.1f, 1f)); break;
      }

      rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    public virtual void Blink()
    {
      spriteRenderer.color = Color.white;
      spriteRenderer.material = whiteOverlayMaterial;
      blinking = true;

      scheduling.SetTimeout(() =>
      {
        spriteRenderer.material = outlineMaterial ?? defaultMaterial;
        blinking = false;
        if (stopHandlingMovement)
        {
          spriteRenderer.color = astonishedColor;
        }

        if (poisoned)
        {
          spriteRenderer.color = poisonedColor;
        }
      }, 0.2f);
    }

    public void SetOutline(Material material)
    {
      outlineMaterial = material;
      spriteRenderer.material = material;
    }

    public Vector2 GetSize()
    {
      return spriteSize;
    }

    public void ResetMass()
    {
      rb.mass = initialMass;
    }

    public virtual void OnEnemyPoisoned()
    {
      poisoned = true;
      if (!blinking)
      {
        spriteRenderer.color = poisonedColor;
      }
    }

    protected virtual void FixedUpdate()
    {
      if (paused)
      {
        rb.velocity = Vector2.zero;
        return;
      }

      if (stopHandlingMovement)
      {
        rb.velocity = Vector2.zero;
        return;
      }
    }

    protected virtual void Update()
    {
      if (!paused)
      {
        selectPlayerTimeout -= Time.deltaTime;
        if (selectPlayerTimeout <= 0)
        {
          selectPlayerTimeout = selectPlayerInterval;
          SelectOnePlayerToChase();
        }

        if (stopHandlingMovement)
        {
          stopHandlingMovementTimeout -= Time.deltaTime;
          if (stopHandlingMovementTimeout <= 0)
          {
            StartHandlingMovement();
          }
        }
      }
    }

    protected virtual void PlayDeadSound()
    {
      if (enemyDeadAudioClips != null && enemyDeadAudioClips.Any())
      {
        var selectedDeadClip = enemyDeadAudioClips[Random.Range(0, enemyDeadAudioClips.Count)];

        AudioWrapper.PlayClip(selectedDeadClip, transform.position);
      }
    }

    protected virtual void PlaceBlood()
    {
      var blood = objectPool.GetObject("Effect/Blood");
      blood.transform.SetParent(null);
      blood.transform.position = transform.position;
    }

    public virtual void OnEnemyRecycle()
    {
      enemyHurtController.RecycleAnimation();
      spriteRenderer.material = outlineMaterial ?? defaultMaterial;
      storeManager.Commit(StoreNames.LevelStore, StoreActions.LevelStore_REMOVE_ENEMY, new LevelData()
      {
        Enemy = gameObject
      });
    }

    public virtual void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      PlayDeadSound();
      PlaceBlood();

      spriteRenderer.material = outlineMaterial ?? defaultMaterial;

      var recoverOnKilled = storeManager.GetState<LevelGeneralState>(StoreNames.LevelGeneralStore)
        .PlayLevelGeneral[playerNumber].RecoverOnKilled;
      if (recoverOnKilled[playerNumber] > 0)
      {
        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_ADD_CURRENT_HP_PERCENTAGE,
          new PlayerActionData
          {
            PlayerNumber = playerNumber,
            AddCurrentHpPercentage = recoverOnKilled[playerNumber]
          });
      }

      var timePassed = storeManager.GetState<LevelTimePassedState>(StoreNames.LevelTimePassedStore).LevelTimePassed;
      storeManager.Commit(StoreNames.BattleDataStore, StoreActions.BattleDataStore_ADD_KILLED_ENEMY, new BattleData()
      {
        PlayerNumber = playerNumber,
        KilledEnemyId = enemyId,
        KilledTimestamp = timePassed,
      });

      storeManager.Commit(StoreNames.LevelStore, StoreActions.LevelStore_REMOVE_ENEMY, new LevelData()
      {
        Enemy = gameObject
      });
    }
  }

  public abstract class PoolableEnemy : Enemy, IPoolableGameObject
  {
    public string ObjectName { get; set; }

    protected override void Awake()
    {
      base.Awake();
      objectPool = ObjectPool.Instance;
      animator = GetComponentInChildren<Animator>();
    }

    public virtual void ObjectReset(Vector2 initialPosition)
    {
      base.EnemyReset();

      gameObject.SetActive(true);
      transform.position = initialPosition;
    }

    public override void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      base.OnEnemyDied(playerNumber, hurtType);

      objectPool.Recycle(ObjectName, gameObject);
    }
  }

  public abstract class PoolableBoss : PoolableEnemy
  {
    protected EventManager eventManager;

    protected override void Awake()
    {
      base.Awake();
      eventManager = EventManager.Instance;
    }

    protected override void Start()
    {
      base.Start();
      PublishAppearEvent();
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);
      PublishAppearEvent();
    }

    protected abstract void PublishAppearEvent();
  }

  public abstract class PoolableLittleBoss : BaseBossController
  {
    protected override void PublishAppearEvent()
    {
      eventManager.PublishEvent(Events.BOSS_APPEAR, new BossAppearEventData()
      {
        Enemy = this
      });
    }

    public override void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      base.OnEnemyDied(playerNumber, hurtType);

      eventManager.PublishEvent(Events.BOSS_DIED, new BossAppearEventData()
      {
        Enemy = this
      });
    }
  }

  public abstract class PoolableBigBoss : BaseBossController
  {
    protected string explosionScheduleId;
    protected bool died;

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      died = false;
    }

    protected override void PublishAppearEvent()
    {
      eventManager.PublishEvent(Events.BOSS_APPEAR, new BossAppearEventData()
      {
        Enemy = this
      });
    }

    public override void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      if (died) return;

      died = true;
      StopHandlingMovementFor(1);

      eventManager.PublishEvent(Events.BOSS_DIED, new BossAppearEventData()
      {
        Enemy = this
      });

      spriteRenderer.material = outlineMaterial ?? defaultMaterial;

      var timePassed = storeManager.GetState<LevelTimePassedState>(StoreNames.LevelTimePassedStore).LevelTimePassed;
      storeManager.Commit(StoreNames.BattleDataStore, StoreActions.BattleDataStore_ADD_KILLED_ENEMY, new BattleData()
      {
        PlayerNumber = playerNumber,
        KilledEnemyId = enemyId,
        KilledTimestamp = timePassed,
      });

      storeManager.Commit(StoreNames.LevelStore, StoreActions.LevelStore_REMOVE_ENEMY, new LevelData()
      {
        Enemy = gameObject
      });

      var angle = 0;
      const int count = 10;
      for (var i = 0; i < count; i++)
      {
        angle += 360 / count * i;
        var spiral = objectPool.GetObject("Enemy/SpiralDeadLight");
        spiral.transform.SetParent(null);

        var spiralController = spiral.GetComponent<SpiralDeadLight>();
        if (i % 2 == 0)
        {
          spiralController.SetHurtType(hurtType);
        }

        spiralController
          .SetPosition(transform.position)
          .InitialAngle = angle;
      }


      explosionScheduleId = scheduling.SetTimeout(() =>
      {
        var explosion = objectPool.GetObject("Effect/BossExplosion");
        var animationScript = explosion.GetComponent<SimpleAnimation>();
        if (animationScript != null)
        {
          animationScript.SetRecycleAnimationAfterFinished(true);
        }

        explosion.transform.SetParent(null);
        explosion.transform.position = transform.position;
        objectPool.Recycle(ObjectName, gameObject);
      }, 1);
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      scheduling.ClearSchedule(explosionScheduleId);
    }
  }
}