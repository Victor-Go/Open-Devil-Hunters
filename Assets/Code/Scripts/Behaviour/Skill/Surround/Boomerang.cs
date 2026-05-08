using System;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.Surround
{
  public class Boomerang : PlayerBasicSkill, IStoreChangedHandler
  {
    public float Speed = 60;

    private readonly Func<float, float> r = theta => 1.5f * (MathF.Sin(1.25f * theta) + 2f) * 0.25f;

    private Scheduling scheduling;

    private AudioSource audioSource;

    private string resetUuidScheduleId;
    private Vector2 playerPosition;
    private float currentAngle;

    private Rigidbody2D rb;

    protected override void Awake()
    {
      base.Awake();

      scheduling = Scheduling.Instance;
      audioSource = GetComponent<AudioSource>();

      rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
      base.Start();

      storeManager.Subscribe(StoreNames.PlayerPositionStore, this);
      playerPosition = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore)
        .PlayerPositions[PlayerNumber];

      resetUuidScheduleId = scheduling.SetInterval(() => { GenerateUniqueId(); }, 1);
    }

    public override void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.PlayerPositionStore:
          playerPosition = ((PlayerPositionState)state).PlayerPositions[PlayerNumber];
          break;
        case StoreNames.GameStateStore:
          HandleGameStateChanged((GameState)state);
          break;
      }
    }

    protected override void HandleGameStateChanged(GameState state)
    {
      base.HandleGameStateChanged(state);

      switch (paused)
      {
        case true: audioSource.Pause(); break;
        case false: audioSource.Play(); break;
      }
    }

    private void Polar(float angle)
    {
      var theta = angle * Mathf.Deg2Rad;

      rb.MovePosition(playerPosition +
                      (Vector2)(Quaternion.AngleAxis(currentAngle, Vector3.forward) * Vector2.right * r(theta)));
    }

    private void FixedUpdate()
    {
      if (paused) return;

      Polar(currentAngle);

      currentAngle += Time.fixedDeltaTime * Speed;
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      scheduling.ClearSchedule(resetUuidScheduleId);
    }
  }
}