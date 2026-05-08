using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Environment
{
  public class CircularTrail : PauseableGameObject
  {
    public int PlayerNumber { get; set; }

    protected float Speed { get; set; } = 3;
    protected float AnglePerSecond { get; set; } = 480;

    protected Rigidbody2D rb;
    protected GameObject trail;

    protected Vector2 targetPosition;
    protected Vector2 lastDirection;

    private bool _triggered;

    protected override void Awake()
    {
      base.Awake();

      rb = GetComponent<Rigidbody2D>();
      var _go = transform.Find("Trail");
      if (_go != null)
      {
        trail = _go.gameObject;
      }
    }

    protected override void Start()
    {
      base.Start();
      storeManager.Subscribe(StoreNames.PlayerPositionStore, this);
    }

    public override void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.GameStateStore:
          HandleGameStateChanged((GameState)state);
          break;
        case StoreNames.PlayerPositionStore:
          targetPosition = ((PlayerPositionState)state).PlayerPositions[PlayerNumber];
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    protected virtual void OnTrailFinished()
    {
      if (trail != null)
      {
        trail.transform.SetParent(null);
        Scheduling.Instance.SetTimeout(() => 
        { 
          if (trail != null)
          {
            Destroy(trail); 
          }
        }, 1);
      }

      Destroy(gameObject);
    }

    protected void TriggerCircularTrail(Vector2 initialDirection)
    {
      _triggered = true;

      lastDirection = initialDirection;
      var playerPosition = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore)
        .PlayerPositions[PlayerNumber];
      targetPosition = playerPosition;

      storeManager.Subscribe(StoreNames.PlayerPositionStore, this);
    }

    protected virtual void MoveObject()
    {
      var dt = Time.fixedDeltaTime;
      var dMaxAngle = dt * AnglePerSecond;

      var angle = Vector2.SignedAngle(targetPosition - (Vector2)transform.position, lastDirection);
      if (Mathf.Abs(angle) > dMaxAngle)
      {
        angle = angle < 0 ? -dMaxAngle : dMaxAngle;
      }

      Vector2 direction = Quaternion.AngleAxis(angle, Vector3.back) * lastDirection;
      lastDirection = direction;

      Vector2 movement = direction * Speed * dt;
      rb.MovePosition(movement + rb.position);

      if ((rb.position - targetPosition).magnitude <= 0.5f)
      {
        OnTrailFinished();
      }
    }

    protected virtual void FixedUpdate()
    {
      if (paused || !_triggered)
      {
        return;
      }

      MoveObject();
    }
  }
}