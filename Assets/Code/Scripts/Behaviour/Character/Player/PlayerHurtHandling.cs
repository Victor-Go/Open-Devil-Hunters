using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.Animation;
using Code.Scripts.Behaviour.Character.Enemy;
using Code.Scripts.Behaviour.UI.Qte;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Player
{
  public class PlayerHurtHandling : PauseableGameObject, IEventHandler
  {
    public int PlayerNumber { get; set; }

    private readonly float onHurtImmortalSeconds = GeneralConfigurations.ImmortalSecondsAfterDamage;

    private SkillAndUpgradeManager skillAndUpgradeManager;
    private Scheduling scheduling;
    private ObjectPool objectPool;
    private ResourceManager resourceManager;
    private EventManager eventManager;

    private GameObject rootGameObject;
    private PlayerManager rootController;
    private Rigidbody2D rootRb;

    private bool immortal;
    private float initialMass;

    private SimpleAnimation burningAnimation;
    private SimpleAnimation frozenAnimation;
    private SimpleAnimation stunningAnimation;
    private SimpleAnimation poisonedAnimation;

    private QtePreciseControl poisonQteControl;
    private QteQuickPressControl frozenQteControl;

    private float maximumHp;
    private bool isPoisoned;
    private float statusTimeout;
    private float statusHurtTimeout; // This is used for taking hurt once per second.
    private float statusHurtPoint;
    private float statusHurtPercentage;
    private float poisonedTimeout;
    private float poisonedHurtPercentage;
    private CharacterStatus playerStatus;

    private string resetImmortalTimeoutId;

    [CanBeNull] private List<BasicAdditionalEffectTypes> additionalEffectImmunity;
    private bool poisonImmunity;

    protected override void Awake()
    {
      base.Awake();

      PlayerControllers.Instance.PlayerHurtHandling = this;

      resourceManager = ResourceManager.Instance;
      skillAndUpgradeManager = SkillAndUpgradeManager.Instance;
      eventManager = EventManager.Instance;
      scheduling = Scheduling.Instance;
      objectPool = ObjectPool.Instance;

      rootGameObject = gameObject;
      while (!rootGameObject.tag.Equals("RootNode"))
      {
        rootGameObject = rootGameObject.transform.parent.gameObject;
      }

      rootRb = rootGameObject.GetComponent<Rigidbody2D>();
      rootController = rootGameObject.GetComponent<PlayerManager>();
      initialMass = rootRb.mass;
    }

    protected override void Start()
    {
      base.Start();
      storeManager.Subscribe(StoreNames.PlayerStore, this);

      var playerState = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
      maximumHp = playerState.PlayerDatas[PlayerNumber].MaximumHp;
      playerStatus = playerState.PlayerDatas[PlayerNumber].PlayerStatus;

      eventManager.AddEventHandler(Events.GAME_OVER, this);
    }

    public void SetImmunity([CanBeNull] List<BasicAdditionalEffectTypes> additionalEffectImmunity, bool poisonImmunity)
    {
      this.additionalEffectImmunity = additionalEffectImmunity;
      this.poisonImmunity = poisonImmunity;
    }

    public override void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.GameStateStore:
          HandleGameStateChanged((GameState)state);
          break;
        case StoreNames.PlayerStore:
          var playerState = (PlayerState)state;
          maximumHp = playerState.PlayerDatas[PlayerNumber].MaximumHp;
          playerStatus = playerState.PlayerDatas[PlayerNumber].PlayerStatus;
          isPoisoned = playerState.PlayerDatas[PlayerNumber].Poisoned;
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    public void OnEvent(Events @event, IEventData data)
    {
      switch (@event)
      {
        case Events.GAME_OVER:
          immortal = true;
          break;
        default:
          throw new InvalidEventHandlingException(@event);
      }
    }

    public void TriggerOnHurtImmortal()
    {
      TriggerImmortal(onHurtImmortalSeconds);
    }

    public void TriggerImmortal(float seconds)
    {
      immortal = true;

      if (resetImmortalTimeoutId != null && !resetImmortalTimeoutId.Equals(""))
      {
        scheduling.ClearSchedule(resetImmortalTimeoutId);
      }

      resetImmortalTimeoutId = scheduling.SetTimeout(() =>
      {
        immortal = false;
        rootController.StopBlinking();
        resetImmortalTimeoutId = null;
      }, seconds);
    }

    private void CommitHpChange(float hitPoint)
    {
      var playerState = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
      var currentHp = playerState.PlayerDatas[PlayerNumber].CurrentHp;
      var newHp = currentHp - hitPoint;

      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/player-hurt_0"), transform.position);

      if (newHp > 0)
      {
        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_CURRENT_HP, new PlayerActionData()
        {
          PlayerNumber = PlayerNumber,
          CurrentHp = newHp,
        });
        eventManager.PublishEvent(Events.PLAYER_HURT, null);
      }
      else
      {
        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_CURRENT_HP, new PlayerActionData()
        {
          PlayerNumber = PlayerNumber,
          CurrentHp = 0,
        });
        HandlePlayerDie();
      }
    }

    public void TriggerNormalHit(float hitPoint)
    {
      HandleNormalHit(hitPoint);
    }

    private void HandleNormalHit(float hitPoint)
    {
      hitPoint *= Random.Range(0.9f, 1.1f);

      var hurtContext = new PlayerHurtContext()
      {
        PlayerNumber = PlayerNumber,
        HitPoint = hitPoint,
      };

      var playerHurtInterceptors =
        skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].PlayerHurtInterceptors;
      foreach (var hurtInterceptor in playerHurtInterceptors)
      {
        hurtContext = hurtInterceptor.OnPlayerHurt(hurtContext);
      }

      var playerHurtActions = skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].PlayerHurtActions;
      foreach (var action in playerHurtActions)
      {
        hurtContext = action(hurtContext);
      }

      CommitHpChange(hurtContext.HitPoint);
    }

    private void HandlePlayerDie()
    {
      var dieContext = new PlayerDieContext()
      {
        AllowPlayerDie = true,
        PlayerPosition = transform.position
      };

      var playerDieInterceptors = skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].PlayerDieInterceptors;
      foreach (var dieInterceptor in playerDieInterceptors)
      {
        dieContext = dieInterceptor.OnPlayerDie(dieContext);
      }

      // Handle die actions
      var playerDieActions = skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].PlayerDieActions;
      foreach (var action in playerDieActions)
      {
        dieContext = action(dieContext);
      }

      // Handle resurrection
      var levelGeneral = storeManager.GetState<LevelGeneralState>(StoreNames.LevelGeneralStore);
      var playerLevelGeneral = levelGeneral.PlayLevelGeneral[PlayerNumber];
      var resurrection = playerLevelGeneral.ResurrectionCount;
      if (dieContext.AllowPlayerDie && resurrection > 0)
      {
        dieContext.AllowPlayerDie = false;
        storeManager.Commit(StoreNames.LevelGeneralStore, StoreActions.LevelGeneralStore_REMOVE_RESURRECTION,
          new LevelGeneralData()
          {
            PlayerNumber = PlayerNumber,
            RemoveResurrection = 1,
          });

        var maxHp = storeManager.GetState<PlayerState>(StoreNames.PlayerStore)
          .PlayerDatas[PlayerNumber]
          .MaximumHp;
        var hp = maxHp * playerLevelGeneral.ResurrectionPercentage;
        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_CURRENT_HP, new PlayerActionData()
        {
          PlayerNumber = PlayerNumber,
          CurrentHp = hp
        });

        // Play animation.

        GameObject _animation = Instantiate(resourceManager.GetResource("Status/Resurrection"));

        var playerPositionState = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore);
        _animation
          .GetComponent<ResurrectionAnimation>()
          .SetPlayerTransform(playerPositionState.PlayerTransforms[PlayerNumber]);
        _animation.transform.SetParent(null);
        _animation.transform.position = transform.position;
      }

      if (dieContext.AllowPlayerDie)
      {
        eventManager.PublishEvent(Events.PLAYER_DIED, new PlayerDiedEventData()
        {
          PlayerNumber = PlayerNumber
        });
        GameObject playerDiedAnim = resourceManager.GetResource("Status/PlayerDied0");
        var anim = Instantiate(playerDiedAnim, null, true);
        anim.transform.position = transform.position;
        
        skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].Reset();
        
        Destroy(rootGameObject);
      }
    }

    public void ResetMass()
    {
      rootRb.mass = initialMass;
    }

    private void FadeOutStatusAnimation()
    {
      ResetMass();
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

    private void FadeOutPoisonedAnimation()
    {
      if (poisonedAnimation != null)
      {
        poisonedAnimation.FadeOutAnimation();
        poisonedAnimation = null;
      }
    }

    #region Additional Effect

    private void HandleBurnAdditionalEffect(FireAdditionalEffect additionalEffect)
    {
      FadeOutStatusAnimation();

      storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_PLAYER_STATUS, new PlayerActionData()
      {
        PlayerNumber = PlayerNumber,
        PlayerStatus = CharacterStatus.BURNING
      });

      statusTimeout = additionalEffect.LastForSeconds;
      statusHurtPercentage = additionalEffect.HurtPercentagePerSecond;
      statusHurtTimeout = 1;

      var burningList = new[]
      {
        "Status/Burning0",
        "Status/Burning1"
      };

      GameObject burning = objectPool.GetObject(burningList[Random.Range(0, burningList.Length)]);
      burning.transform.SetParent(rootGameObject.transform);
      burning.transform.localPosition = Vector2.zero;
      burningAnimation = burning.GetComponent<SimpleAnimation>();

      var playerSize = rootController.PlayerSize;
      GameObject qte = Instantiate(resourceManager.GetResource("UI/Qte/QteBurning"));
      qte.transform.SetParent(rootGameObject.transform);
      qte.transform.localPosition = new Vector2(-playerSize.x / 2 - 0.5f, 0);

      frozenQteControl = qte.GetComponent<QteQuickPressControl>();
      frozenQteControl.SetQteResolvedHandler(OnStatusResolved);
      frozenQteControl.SetPlayerNumber(PlayerNumber);
    }

    private void HandleFreezeAdditionalEffect(IceAdditionalEffect additionalEffect)
    {
      statusTimeout = additionalEffect.LastForSeconds;
      statusHurtPoint = additionalEffect.DamagePerSecond;
      statusHurtTimeout = 1;
      rootRb.mass *= 10;

      if (playerStatus.Equals(CharacterStatus.FROZEN))
      {
        return;
      }

      FadeOutStatusAnimation();
      storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_PLAYER_STATUS, new PlayerActionData()
      {
        PlayerNumber = PlayerNumber,
        PlayerStatus = CharacterStatus.FROZEN
      });
      var frozenList = new[]
      {
        "Status/Frozen0",
        "Status/Frozen1",
        "Status/Frozen2",
      };

      GameObject frozen = objectPool.GetObject(frozenList[Random.Range(0, frozenList.Length)]);
      var effectSize = frozen.GetComponent<SpriteRenderer>().size;
      var playerSize = rootController.PlayerSize;
      float scale = Mathf.Max(playerSize.x / effectSize.x, playerSize.y / effectSize.y, 1);
      frozen.transform.localScale = new Vector2(scale, scale);
      frozen.transform.SetParent(rootGameObject.transform);
      frozen.transform.localPosition = Vector2.zero;
      frozenAnimation = frozen.GetComponent<SimpleAnimation>();

      GameObject qte = Instantiate(resourceManager.GetResource("UI/Qte/QteFrozen"));
      qte.transform.SetParent(rootGameObject.transform);
      qte.transform.localPosition = new Vector2(playerSize.x / 2 + 0.5f, 0);

      frozenQteControl = qte.GetComponent<QteQuickPressControl>();
      frozenQteControl.SetQteResolvedHandler(OnStatusResolved);
      frozenQteControl.SetPlayerNumber(PlayerNumber);
    }

    private void OnStatusResolved()
    {
      statusHurtPercentage = 0;
      statusHurtPoint = 0;

      storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_PLAYER_STATUS, new PlayerActionData()
      {
        PlayerNumber = PlayerNumber,
        PlayerStatus = CharacterStatus.NORMAL,
      });

      if (frozenQteControl != null)
      {
        frozenQteControl.StartToDestroy();
        frozenQteControl = null;
      }

      FadeOutStatusAnimation();
    }

    private void HandleStunAdditionalEffect(ThunderAdditionalEffect additionalEffect)
    {
      statusTimeout = additionalEffect.StunningSeconds;
      statusHurtTimeout = 1;

      if (playerStatus.Equals(CharacterStatus.STUNNING))
      {
        return;
      }

      FadeOutStatusAnimation();
      storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_PLAYER_STATUS, new PlayerActionData()
      {
        PlayerNumber = PlayerNumber,
        PlayerStatus = CharacterStatus.STUNNING
      });

      var stunList = new[]
      {
        "Status/Stunning0"
      };

      GameObject stunning = objectPool.GetObject(stunList[Random.Range(0, stunList.Length)]);
      var effectSize = stunning.GetComponent<SpriteRenderer>().size;
      var playerSize = rootController.PlayerSize;
      float scale = Mathf.Max(playerSize.x / effectSize.x, 1);
      stunning.transform.localScale = new Vector2(scale, scale);
      stunning.transform.SetParent(rootGameObject.transform);
      stunning.transform.localPosition = new Vector2(0, rootController.PlayerSize.y / 2 + 0.1f);

      stunningAnimation = stunning.GetComponent<SimpleAnimation>();
    }

    private void HandlePoisonAdditionalEffect(PoisonAdditionalEffect additionalEffect)
    {
      poisonedHurtPercentage = additionalEffect.HurtPercentagePerSecond;
      poisonedTimeout = Mathf.Max(poisonedTimeout, 5);

      if (!isPoisoned)
      {
        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_PLAYER_POISONED, new PlayerActionData()
        {
          PlayerNumber = PlayerNumber,
          Poisoned = true,
        });

        var poisonedList = new[]
        {
          "Status/Poisoned"
        };

        GameObject poisoned = objectPool.GetObject(poisonedList[Random.Range(0, poisonedList.Length)]);
        poisoned.transform.SetParent(rootGameObject.transform);
        poisonedAnimation = poisoned.GetComponent<SimpleAnimation>();
        Vector2 size = rootController.PlayerSize;
        poisoned.transform.localPosition = new Vector2(size.x / 3, size.y / 2 + 0.1f);

        GameObject qte = Instantiate(resourceManager.GetResource("UI/Qte/QtePoison"));
        qte.transform.SetParent(rootGameObject.transform);
        qte.transform.localPosition = new Vector2(0, -size.y / 2 - 0.5f);

        poisonQteControl = qte.GetComponent<QtePreciseControl>();
        poisonQteControl.SetTargetPercent(Random.Range(0.2f, 0.3f)).SetQteResolvedHandler(OnPoisonResolved);
        poisonQteControl.SetPlayerNumber(PlayerNumber);
      }
    }

    private void OnPoisonResolved()
    {
      poisonedHurtPercentage = 0;
      storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_SET_PLAYER_POISONED, new PlayerActionData()
      {
        PlayerNumber = PlayerNumber,
        Poisoned = false
      });

      if (poisonQteControl != null)
      {
        poisonQteControl.StartToDestroy();
        poisonQteControl = null;
      }

      FadeOutPoisonedAnimation();
    }

    #endregion

    private void HandleEnemySkillHit(EnemySkill enemySkill)
    {
      var configs = enemySkill.SkillConfigurations;
      var additionalEffect = configs.AdditionalEffect;

      // Handle basic AE.
      switch (additionalEffect.BasicAdditionalEffectType)
      {
        case BasicAdditionalEffectTypes.BURN:
          if (additionalEffectImmunity != null &&
              additionalEffectImmunity.Any(i => i == BasicAdditionalEffectTypes.BURN))
          {
            HandleBurnAdditionalEffect((FireAdditionalEffect)additionalEffect.BasicAdditionalEffect);
          }

          break;
        case BasicAdditionalEffectTypes.FREEZE:
          if (additionalEffectImmunity != null &&
              additionalEffectImmunity.Any(i => i == BasicAdditionalEffectTypes.FREEZE))
          {
            HandleFreezeAdditionalEffect((IceAdditionalEffect)additionalEffect.BasicAdditionalEffect);
          }

          break;
        case BasicAdditionalEffectTypes.STUN:
          if (additionalEffectImmunity != null &&
              additionalEffectImmunity.Any(i => i == BasicAdditionalEffectTypes.STUN))
          {
            HandleStunAdditionalEffect((ThunderAdditionalEffect)additionalEffect.BasicAdditionalEffect);
          }

          break;
      }

      // Handle poison AE.
      if (additionalEffect.PoisonAdditionalEffect.Enable && !poisonImmunity)
      {
        HandlePoisonAdditionalEffect(additionalEffect.PoisonAdditionalEffect);
      }

      HandleNormalHit(enemySkill.NormalHitPoint);
    }

    private float GetRandomizedHurtPoint(float hurtPoint)
    {
      return hurtPoint * Random.Range(0.85f, 1.1f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer) || immortal)
      {
        return;
      }

      TriggerOnHurtImmortal();
      rootController.Blink();

      var collisionLayerName = LayerMask.LayerToName(other.gameObject.layer);

      if (collisionLayerName.Equals("EnemyHit"))
      {
        var generalEnemyHit = other.gameObject.GetComponent<EnemyCollisionHit>();
        var hitPoint = generalEnemyHit.NormalHitPoint;
        HandleNormalHit(hitPoint);
      }
      else if (collisionLayerName.Equals("EnemySkill"))
      {
        var enemySkill = other.GetComponent<EnemySkill>();
        HandleEnemySkillHit(enemySkill);
      }
    }

    public void TriggerThunderHurtManually(float hitPoint, float stunningSeconds)
    {
      TriggerOnHurtImmortal();
      rootController.Blink();

      HandleNormalHit(hitPoint);
      HandleStunAdditionalEffect(new ThunderAdditionalEffect
      {
        StunningSeconds = stunningSeconds,
        Possibility = 1,
      });
    }

    private void Update()
    {
      if (paused) return;

      if (!playerStatus.Equals(CharacterStatus.NORMAL) || isPoisoned)
      {
        statusTimeout -= Time.deltaTime;
        statusHurtTimeout -= Time.deltaTime;

        // Calculate status hurt.
        if (statusHurtTimeout <= 0)
        {
          statusHurtTimeout = 1;

          var totalHurtPoint = GetRandomizedHurtPoint(statusHurtPoint + statusHurtPercentage * maximumHp);
          totalHurtPoint += GetRandomizedHurtPoint(poisonedHurtPercentage * maximumHp);

          if (totalHurtPoint != 0)
          {
            TriggerOnHurtImmortal();
            CommitHpChange(totalHurtPoint);
          }
        }

        if (statusTimeout <= 0)
        {
          OnStatusResolved();
        }
      }

      if (isPoisoned)
      {
        poisonedTimeout -= Time.deltaTime;

        if (poisonedTimeout <= 0)
        {
          OnPoisonResolved();
        }
      }
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      scheduling.ClearSchedule(resetImmortalTimeoutId);
    }
  }
}