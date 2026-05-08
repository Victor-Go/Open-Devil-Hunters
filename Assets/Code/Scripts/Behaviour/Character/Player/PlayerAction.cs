using System.Collections.Generic;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Player
{
  public struct PlayerActionInfo
  {
    public float FixedDeltaTime { get; set; }
    public Vector2 CurrentGlobalPosition { get; set; }
    public Vector2 RawInputMovement { get; set; }
    public bool Attacking { get; set; }
  }

  public class PlayerAction : PauseableGameObject, IObservable<PlayerActionInfo>, IStoreChangedHandler
  {
    public int PlayerNumber { get; set; }
    private int numberOfPlayers { get; set; }

    private Vector2 movement;
    private bool attacking;

    private bool frozenOrStunningNotMovable;

    private float generalNotMovableTimeout;

    private readonly List<IObserver<PlayerActionInfo>> observers = new();

    private CharacterStatus playerStatus;

    protected override void Awake()
    {
      base.Awake();
      storeManager = StoreManager.Instance;
    }

    protected override void Start()
    {
      base.Start();

      storeManager
        .Subscribe(StoreNames.LevelConfigurationStore, this)
        .Subscribe(StoreNames.GameStateStore, this)
        .Subscribe(StoreNames.PlayerStore, this);

      numberOfPlayers = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore)
        .NumberOfPlayers;

      storeManager.Commit(StoreNames.PlayerPositionStore, StoreActions.PlayerPositionStore_SET_PLAYER_TRANSFORM,
        new PlayerPositionData()
        {
          PlayerNumber = PlayerNumber,
          PlayerTransform = gameObject.transform
        });

      storeManager.Commit(StoreNames.PlayerPositionStore, StoreActions.PlayerPositionStore_SET_PLAYER_POSITION,
        new PlayerPositionData()
        {
          PlayerNumber = PlayerNumber,
          ReportedPlayerPosition = gameObject.transform.position
        });
    }

    public override void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          numberOfPlayers = ((LevelConfigurationState)state).NumberOfPlayers;
          break;
        case StoreNames.GameStateStore:
          HandleGameStateChanged((GameState)state);
          break;
        case StoreNames.PlayerStore:
          playerStatus = ((PlayerState)state).PlayerDatas[PlayerNumber].PlayerStatus;
          HandlePlayerStatusChanged();
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void HandlePlayerStatusChanged()
    {
      switch (playerStatus)
      {
        case CharacterStatus.FROZEN:
        case CharacterStatus.STUNNING:
          TriggerFrozenOrStunningNotMovable();
          break;
        case CharacterStatus.NORMAL:
          ResolveFrozenOrStunningNotMovable();
          break;
      }
    }

    private void TriggerFrozenOrStunningNotMovable()
    {
      frozenOrStunningNotMovable = true;
      movement = Vector2.zero;
      attacking = false;
    }

    public void StopHandlingMovementAndAttackFor(float seconds)
    {
      movement = Vector2.zero;
      attacking = false;
      generalNotMovableTimeout = Mathf.Max(seconds, generalNotMovableTimeout);
    }

    public void ResolveFrozenOrStunningNotMovable()
    {
      frozenOrStunningNotMovable = false;
    }

    private void Update()
    {
      if (!frozenOrStunningNotMovable && generalNotMovableTimeout <= 0)
      {
        movement.x = InputUtils.GetAxisRaw(PlayerNumber, InputAxises.LEFT_HORIZONTAL);
        movement.y = InputUtils.GetAxisRaw(PlayerNumber, InputAxises.LEFT_VERTICAL);
        attacking = InputUtils.GetButton(PlayerNumber, InputButtons.FIRE);

        if (PlayerNumber == 0)
        {
          attacking |= InputUtils.GetButton(0, InputButtons.MOUSE_FIRE);
        }
      }

      if (generalNotMovableTimeout > 0) generalNotMovableTimeout -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
      if (paused) return;

      var actionInfo = new PlayerActionInfo()
      {
        FixedDeltaTime = Time.fixedDeltaTime,
        CurrentGlobalPosition = transform.position,
        Attacking = attacking,
        RawInputMovement =
          movement, // Be careful, since max value of raw input would be Vector2(1,1) and so on. So that the magnitude would be sqrt(2) instead of 1.
      };

      foreach (var observer in observers)
      {
        observer.OnNext(actionInfo);
      }
    }

    public IDisposable Subscribe(IObserver<PlayerActionInfo> observer)
    {
      if (!observers.Contains(observer))
      {
        observers.Add(observer);
      }

      return new Unsubscriber<PlayerActionInfo>(observers, observer);
    }
  }
}