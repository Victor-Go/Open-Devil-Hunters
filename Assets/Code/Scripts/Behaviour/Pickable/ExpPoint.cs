using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Pickable
{
  public class ExpPoint : PoolableGameObject, IStoreChangedHandler
  {
    public float Speed = 10;
    public float Threshold = 0.25f;
    public float OutgoingDistance = 1;
    public float BaseExperience = 1;

    private ObjectPool objectPool;
    private StoreManager storeManager;
    private Scheduling scheduling;
    private Animator animator;
    private SkillAndUpgradeManager skillAndUpgradeManager;
    private GameObject trail;
    private TrailRenderer trailRenderer;
    private AudioClip audioClip;

    private float existDuration = 0;
    private int triggerPlayerNumber;
    private string trailSchedulingId;
    private bool paused;
    private bool triggered;
    private bool outgoing = true;
    private Vector2 outgoingMovement = Vector2.zero;
    private float outgoingDistance;
    private Vector2 movement = Vector2.zero;
    private Vector2 playerPosition;

    private void Awake()
    {
      skillAndUpgradeManager = SkillAndUpgradeManager.Instance;
      scheduling = Scheduling.Instance;
      objectPool = ObjectPool.Instance;
      storeManager = StoreManager.Instance;

      trail = transform.Find("Trail").gameObject;
      trailRenderer = trail.GetComponent<TrailRenderer>();
      animator = GetComponent<Animator>();
      audioClip = (AudioClip)Resources.Load("Audio/Sound/Environment/get-exp_0");
    }

    private void Start()
    {
      storeManager
        .Subscribe(StoreNames.GameStateStore, this);
      trailRenderer.Clear();
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      if (trailSchedulingId != null && !trailSchedulingId.Equals(""))
      {
        scheduling.ClearSchedule(trailSchedulingId);
        trailSchedulingId = null;
      }

      trail.transform.SetParent(transform, false);
      trail.transform.localPosition = Vector2.zero;
      trailRenderer.Clear();
      trail.SetActive(true);

      triggered = false;
      outgoing = true;
      outgoingDistance = 0;
      existDuration = 0;
      storeManager.Unsubscribe(this);
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.GameStateStore:
          GameStates gameState = ((GameState)state).CurrentGameState;
          paused = LevelUtils.EverythingCanNotMove(gameState);
          animator.speed = paused ? 0 : 1;
          break;
        case StoreNames.PlayerPositionStore:
          if (triggered)
          {
            var positionState = (PlayerPositionState)state;
            playerPosition = positionState.PlayerPositions[triggerPlayerNumber];
            movement = (playerPosition - (Vector2)transform.position).normalized * Speed;
          }

          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer) || triggered)
      {
        return;
      }

      triggerPlayerNumber = other.GetComponent<PickUpControl>().PlayerNumber;
      Trigger(triggerPlayerNumber);
    }

    public void Trigger(int playerNumber)
    {
      triggered = true;
      playerPosition = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore)
        .PlayerPositions[triggerPlayerNumber];
      outgoingMovement = ((Vector2)transform.position - playerPosition).normalized * Speed;
      storeManager.Subscribe(StoreNames.PlayerPositionStore, this);
    }

    private void AddExperience()
    {
      AudioWrapper.PlayClip(audioClip, transform.position);

      var expContext = new ObtainExperienceContext
      {
        Experience = BaseExperience
      };

      foreach (var interceptor in skillAndUpgradeManager.SkillAndUpgradeControllers[triggerPlayerNumber]
                 .ObtainExperienceInterceptors)
      {
        expContext = interceptor.OnObtainExperience(expContext);
      }

      foreach (var action in skillAndUpgradeManager.SkillAndUpgradeControllers[triggerPlayerNumber]
                 .ObtainExperienceActions)
      {
        expContext = action(expContext);
      }

      storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_ADD_CURRENT_EXPERIENCE,
        new PlayerActionData()
        {
          PlayerNumber = triggerPlayerNumber,
          Experience = expContext.Experience
        });
    }

    private void FixedUpdate()
    {
      if (paused)
      {
        return;
      }

      existDuration += Time.fixedDeltaTime;
      if (existDuration >= GeneralConfigurations.ExpPointDuration)
      {
        trail.transform.SetParent(null);
        trailRenderer.Clear();
        trail.SetActive(false);
        objectPool.Recycle(ObjectName, gameObject);
        return;
      }

      // Detect if it's been absorbed
      if ((playerPosition - (Vector2)transform.position).sqrMagnitude <= Threshold && !outgoing)
      {
        AddExperience();
        trail.transform.SetParent(null);
        trailSchedulingId = scheduling.SetTimeout(() => { trail.SetActive(false); }, 1);
        objectPool.Recycle(ObjectName, gameObject);
      }

      if (triggered && !outgoing)
      {
        var actualMovement = movement * Time.fixedDeltaTime;
        transform.position = (Vector2)transform.position + actualMovement;
      }
      else if (triggered && outgoing)
      {
        var actualMovement = outgoingMovement * Time.fixedDeltaTime;
        outgoingDistance += actualMovement.magnitude;
        transform.position = (Vector2)transform.position + actualMovement;
        if (outgoingDistance >= OutgoingDistance)
        {
          outgoing = false;
        }
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}