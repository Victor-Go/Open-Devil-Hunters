using System.Collections.Generic;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Behaviour.Skill;
using Code.Scripts.Behaviour.UI;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Behaviour.UI;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Player
{
  public class PlayerManager : PauseableGameObject, IStoreChangedHandler
  {
    private int PlayerNumber;

    public Vector2 PlayerSize { get; set; }

    private readonly Color astonishedColor = new Color(153f / 255, 153f / 255, 153f / 255, 1);
    private readonly Color poisonedColor = new Color(0f / 255, 159f / 255, 14f / 255, 1);
    private readonly Color frozenColor = new Color(88f / 255, 209f / 255, 255f / 255);

    private ResourceManager resourceManager;
    private AttackControl attackControl;
    private MovementControl movementControl;
    private PlayerAction playerAction;
    private SurroundSkillControl surroundSkillControl;
    private Scheduling scheduling;
    private PlayerHurtHandling playerHurtHandling;
    private IObservable<PlayerActionInfo> playerActionObservable;
    private SkillAndUpgradeManager skillAndUpgradeManager;

    private string holyShieldScheduleId;

    private Rigidbody2D rb;

    private Material redOverlayMaterial;
    private Material defaultMaterial;
    private SpriteRenderer playerImage;
    private bool isRedOverlay;

    private CharacterStatus playerStatus;
    private bool poisoned;

    private string immortalBlinkInterval;
    private bool blinking;

    private List<IDisposable> observerSubscriptions = new();

    protected override void Awake()
    {
      base.Awake();

      skillAndUpgradeManager = SkillAndUpgradeManager.Instance;

      PlayerControllers.Instance.PlayerManager = this;

      resourceManager = ResourceManager.Instance;
      scheduling = Scheduling.Instance;

      storeManager = StoreManager.Instance;

      playerAction = GetComponentInChildren<PlayerAction>();

      playerHurtHandling = GetComponentInChildren<PlayerHurtHandling>();
      attackControl = GetComponentInChildren<AttackControl>();
      movementControl = GetComponentInChildren<MovementControl>();
      surroundSkillControl = GetComponentInChildren<SurroundSkillControl>();
      animator = GetComponentInChildren<Animator>();
      rb = GetComponent<Rigidbody2D>();
      playerImage = transform.Find("Animator").GetComponent<SpriteRenderer>();
      defaultMaterial = playerImage.material;

      // Subscriptions
      playerActionObservable = GetComponent<IObservable<PlayerActionInfo>>();

      var subscriptions = new List<IDisposable>()
      {
        // The order is important since it will affect skill launch position.
        playerActionObservable.Subscribe(movementControl),
        playerActionObservable.Subscribe(attackControl),
      };

      subscriptions.ForEach(s => observerSubscriptions.Add(s));
    }

    protected override void Start()
    {
      base.Start();
      storeManager
        .Subscribe(StoreNames.GameStateStore, this)
        .Subscribe(StoreNames.PlayerStore, this);

      skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].SetPlayerManager(this);

      redOverlayMaterial = resourceManager.GetResource("Materials/RedOverlay");
    }

    public void Blink()
    {
      blinking = true;
      playerImage.color = Color.white;
      playerImage.material = redOverlayMaterial;

      if (immortalBlinkInterval != null && !immortalBlinkInterval.Equals(""))
      {
        scheduling.ClearSchedule(immortalBlinkInterval);
        immortalBlinkInterval = null;
      }

      immortalBlinkInterval = scheduling.SetInterval(() =>
      {
        if (!blinking) // This check is necessary for multiple attacks in a short period of time.
        {
          scheduling.ClearSchedule(immortalBlinkInterval);
          immortalBlinkInterval = null;
          playerImage.material = defaultMaterial;
          return;
        }

        playerImage.material = isRedOverlay ? defaultMaterial : redOverlayMaterial;
        isRedOverlay = !isRedOverlay;
        if (!isRedOverlay)
        {
          SetPlayerColor();
        }
        else
        {
          playerImage.color = Color.white;
        }
      }, 0.15f);
    }

    public void TriggerHolyShield(float duration)
    {
      playerHurtHandling.TriggerImmortal(duration);

      GameObject holyShield = Instantiate(resourceManager.GetResource("Status/HolyShield"));
      holyShield.transform.SetParent(transform, false);
      holyShield.transform.localPosition = Vector2.zero;

      holyShieldScheduleId = scheduling.SetTimeout(() => { holyShield.GetComponent<HolyShield>().FadeOut(); },
        Mathf.Max(0, duration - 1));
    }

    public void StopBlinking()
    {
      if (immortalBlinkInterval != null && !immortalBlinkInterval.Equals(""))
      {
        scheduling.ClearSchedule(immortalBlinkInterval);
      }

      playerImage.material = defaultMaterial;
      isRedOverlay = false;
      blinking = false;
      SetPlayerColor();
    }

    public PlayerManager SetPlayerNumber(int playerNumber)
    {
      PlayerNumber = playerNumber;
      SetPlayerNumberToSubComponents();

      return this;
    }

    private void SetPlayerNumberToSubComponents()
    {
      var pickUpControl = GetComponentInChildren<PickUpControl>();
      var reloadDisplayer = GetComponentInChildren<ReloadDisplayer>();
      var hpAttachedDisplayer = GetComponentInChildren<PlayerHpAttachedDisplayer>();

      pickUpControl.PlayerNumber = PlayerNumber;
      reloadDisplayer.PlayerNumber = PlayerNumber;
      playerAction.PlayerNumber = PlayerNumber;
      attackControl.PlayerNumber = PlayerNumber;
      playerHurtHandling.PlayerNumber = PlayerNumber;
      movementControl.PlayerNumber = PlayerNumber;
      surroundSkillControl.PlayerNumber = PlayerNumber;
      hpAttachedDisplayer.PlayerNumber = PlayerNumber;
    }

    public void AddSurroundSkill(SurroundSkillGroups group, GameObject skill, float distance)
    {
      if (surroundSkillControl == null)
      {
        return;
      }

      surroundSkillControl.AddSurroundSkill(group, skill, distance);
    }

    public PlayerManager InitPlayerConfigurations(PlayerConfiguration playerConfig)
    {
      // Player movement and pick up.
      var initData = new PlayerActionData()
      {
        PlayerNumber = PlayerNumber,
        PickUpRadius = playerConfig.PickUpRadius,
        MovingSpeed = playerConfig.MovingSpeed,
        AttackingMovingSpeed = playerConfig.AttackingMovingSpeed,
        MaximumHp = playerConfig.MaximumHp,
      };

      storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_INITIALIZE, initData);

      // Player attack.
      attackControl
        .InitializeAttackControl(
          SkillPresets.PlayerBasicSkills[playerConfig.DefaultSkillId].AttackControlConfigurations,
          playerConfig.PlayerFireAudios,
          playerConfig.ReadyAudios
        );

      var defaultSkillId = playerConfig.DefaultSkillId;
      var defaultSkillPreset = SkillPresets.PlayerBasicSkills[playerConfig.DefaultSkillId];
      skillAndUpgradeManager.SkillAndUpgradeControllers[PlayerNumber].SetBasicSkill(
        defaultSkillId,
        defaultSkillPreset.SkillPrefabName,
        defaultSkillPreset.DefaultQuantity,
        defaultSkillPreset.SkillConfigurations
      );

      PlayerSize = playerImage.size;

      playerHurtHandling.SetImmunity(playerConfig.AdditionalEffectImmunity, playerConfig.PoisonImmunity);

      return this;
    }

    private void SetPlayerColor()
    {
      switch (playerStatus)
      {
        case CharacterStatus.FROZEN:
          playerImage.color = frozenColor;
          animator.speed = 0;
          break;
        case CharacterStatus.STUNNING:
          playerImage.color = astonishedColor;
          animator.speed = 0;
          break;
        case CharacterStatus.NORMAL:
          playerImage.color = Color.white;
          animator.speed = 1;
          break;
      }

      if (poisoned)
      {
        playerImage.color = poisonedColor;
      }
    }

    public override void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.GameStateStore:
          var gameState = (GameState)state;
          HandleGameStateChanged(gameState);
          break;
        case StoreNames.PlayerStore:
          var playerState = (PlayerState)state;
          playerStatus = playerState.PlayerDatas[PlayerNumber].PlayerStatus;
          poisoned = playerState.PlayerDatas[PlayerNumber].Poisoned;

          if (!blinking)
          {
            SetPlayerColor();
          }

          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void Update()
    {
      if (paused)
      {
        rb.velocity = Vector2.zero;
      }
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      observerSubscriptions.ForEach(s => s.Dispose());
      scheduling.ClearSchedule(holyShieldScheduleId);
      scheduling.ClearSchedule(immortalBlinkInterval);
    }
  }
}