using System;
using Code.Scripts.Behaviour.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Behaviour.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Behaviour.Character.Player
{
  public class MovementControl : PauseableGameObject, Assets.Code.Scripts.Src.IObserver<PlayerActionInfo>,
    IStoreChangedHandler, IEventHandler
  {
    public bool AllowTeleportation;
    public float TeleportDistance = 3.5f;

    public float TeleportationCountdown = 15f;

    public bool IndependentWeapon; // Don't forget to change this value in AttackControl and AnimatorControl.
    public float FootprintDistance = 0.1f;

    public int PlayerNumber { get; set; }

    private int numberOfPlayers;

    private float footstepAudioDistance;

    private ObjectPool objectPool;
    private Rigidbody2D rb;
    private float symmetryAxisOffset;
    private EventManager eventManager;
    private Transform animatorTransform;
    private Transform launchPositionTransform;
    private float originalLaunchLocalHorizontalPosition;
    private ResourceManager resourceManager;
    private Transform footprintTransform;
    private AudioClip[] footprintClips;
    private bool footPrintIsRight;

    private float teleportationCountdown;

    private Vector2[] playerPositions;

    private Vector2 attackingLocalScale;
    private float canvasVhRatio;
    private float movingSpeed;
    private float attackingMovingSpeed;
    private bool inAttackingState;

    // For updating player position. Since player position may be moved accidentally (E.g. Pushed by enemy or by blast wind).
    private const float updatePlayerPositionInterval = 0.2f;
    private float updatePlayerPositionTimeout;

    private float footprintSqrDistanceElapsed;
    private float footstepAudioSqrDistanceElapsed;

    // Teleportation
    private const float operationTimeout = 0.25f;

    // False means no user input, true means has user input
    private readonly bool[] teleportOperations = { false, true, false, true, false, true };
    private int requiredOperationIndex;
    private float timeSinceLastOperation;
    private Vector2 teleportDirection;

    private GameObject teleportationIndicator;
    private Vector2 lastReportedPosition;

    private void TryCommitPosition()
    {
      var currentPosition = (Vector2)transform.position;
      if (lastReportedPosition == currentPosition) return;
      lastReportedPosition = currentPosition;

      // Bypass the generic event bus for raw position updates entirely to ensure zero allocations.
      playerPositions[PlayerNumber] = currentPosition;
    }

    protected override void Awake()
    {
      rb = GetComponentInParent<Rigidbody2D>();
      objectPool = ObjectPool.Instance;

      teleportationIndicator = transform.parent.Find("TeleportationIndicator")?.gameObject;
      if (AllowTeleportation && teleportationIndicator == null && DebugConfigurations.DebugEnabled)
      {
        throw new Exception("MovementControl indicates AllowTeleportation, but teleportation indicator not found.");
      }

      footprintTransform = transform.parent.Find("FootprintPosition");

      // Animator
      var animatorGameObject = transform.parent.Find("Animator").gameObject;
      animator = animatorGameObject.GetComponent<Animator>();
      animatorTransform = animatorGameObject.transform;
      symmetryAxisOffset = animatorTransform.localPosition.x -
                           transform.parent.Find("Symmetry").transform.localPosition.x;
      SetAnimatorHorizontalPosition(AnimatorDirection
        .Left); // Here will set animator to appropriate position relative toQuat symmetryAxisOffset.

      if (IndependentWeapon)
      {
        launchPositionTransform = animatorGameObject.transform.Find("Weapon/LaunchPosition");
      }
      else
      {
        launchPositionTransform = transform.parent.Find("LaunchPosition");
        originalLaunchLocalHorizontalPosition =
          launchPositionTransform.localPosition.x + animatorTransform.localPosition.x;
      }

      storeManager = StoreManager.Instance;
      eventManager = EventManager.Instance;
      resourceManager = ResourceManager.Instance;

      GameObject uiCanvas = resourceManager.GetResource("/UiCanvas");
      var canvasRect = uiCanvas.GetComponent<RectTransform>().rect;
      canvasVhRatio = canvasRect.height / canvasRect.width;

      footstepAudioDistance = FootprintDistance * 3;
    }

    private void Start()
    {
      storeManager
        .Subscribe(StoreNames.LevelConfigurationStore, this)
        .Subscribe(StoreNames.PlayerStore, this)
        .Subscribe(StoreNames.AttackControlStore, this)
        .Subscribe(StoreNames.PlayerPositionStore, this);

      eventManager.AddEventHandler(Events.UPDATE_PLAYER_DIRECTION, this);

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      numberOfPlayers = levelConfigs.NumberOfPlayers;

      var playerPositionState = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore);
      playerPositions = playerPositionState.PlayerPositions;

      var playerState = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
      attackingMovingSpeed = playerState.PlayerDatas[PlayerNumber].AttackingMovingSpeed;
      movingSpeed = playerState.PlayerDatas[PlayerNumber].MovingSpeed;

      var currentMapName = levelConfigs.MapConfiguration.MapName;
      footprintClips = new AudioClip[GeneralConfigurations.FootstepClipNames.Names[currentMapName].Length];
      for (var i = 0; i < footprintClips.Length; i++)
      {
        footprintClips[i] = Resources.Load<AudioClip>(GeneralConfigurations.FootstepClipNames.Names[currentMapName][i]);
      }
    }

    private void DoTeleport(Vector2 direction)
    {
      var disappear = objectPool.GetObject("Effect/TeleportationDisappear");
      disappear.transform.SetParent(null);
      disappear.transform.position = transform.position;

      var teleportPosition = (Vector2)transform.position + direction.normalized * TeleportDistance;
      var appear = objectPool.GetObject("Effect/TeleportationAppear");
      appear.transform.SetParent(null);
      appear.transform.position = teleportPosition;

      rb.position = teleportPosition;

      AudioWrapper.PlayClip(resourceManager.GetResource("Audio/Sound/Player/wukong-teleport_0"), teleportPosition);
    }

    private void ResetTeleport()
    {
      teleportDirection = Vector2.zero;
      requiredOperationIndex = 0;
      timeSinceLastOperation = 0;
    }

    public void OnNext(PlayerActionInfo playerAction)
    {
      var movement = Mathf.Clamp(playerAction.RawInputMovement.magnitude, 0, 1f) *
                     playerAction.RawInputMovement.normalized;

      #region Handle teleportation

      if (AllowTeleportation &&
          teleportationCountdown <= 0 &&
          ((movement.sqrMagnitude <= 0.05f && !teleportOperations[requiredOperationIndex]) ||
           (movement.sqrMagnitude > 0.05f &&
            Mathf.Abs(Vector2.SignedAngle(movement, teleportDirection)) <= 15 &&
            teleportOperations[requiredOperationIndex]))
         )
      {
        if (requiredOperationIndex == 1)
        {
          teleportDirection = movement;
        }

        timeSinceLastOperation = 0;
        requiredOperationIndex++;

        if (requiredOperationIndex >= teleportOperations.Length)
        {
          teleportationCountdown = TeleportationCountdown;
          teleportationIndicator.SetActive(false);

          DoTeleport(teleportDirection);
          ResetTeleport();
        }
      }
      // Reset when wrong direction
      else if (
        AllowTeleportation &&
        movement.sqrMagnitude > 0.05f &&
        Mathf.Abs(Vector2.SignedAngle(movement, teleportDirection)) > 15 &&
        teleportOperations[requiredOperationIndex]
      )
      {
        ResetTeleport();
      }

      #endregion

      var actualMovement = movement * Time.fixedDeltaTime * (inAttackingState ? attackingMovingSpeed : movingSpeed);
      var updatedPosition = rb.position + actualMovement;

      if (numberOfPlayers <= 1)
      {
        HandleFootprintDistanceChanged(actualMovement);
        rb.MovePosition(updatedPosition);
      }
      else
      {
        float maximumPlayersHorizontalDistance = GeneralConfigurations.MaximumPlayersHorizontalDistance,
          maximumPlayersVerticalDistance = maximumPlayersHorizontalDistance * canvasVhRatio;

        var otherPlayerPosition = playerPositions[PlayerNumber == 0 ? 1 : 0];
        var currentDistanceVector = otherPlayerPosition - rb.position;
        var previewDistanceVector = otherPlayerPosition - updatedPosition;

        if (Mathf.Abs(previewDistanceVector.x) > maximumPlayersHorizontalDistance &&
            Mathf.Abs(previewDistanceVector.x) > Mathf.Abs(currentDistanceVector.x))
        {
          actualMovement.x = 0;
        }

        if (Mathf.Abs(previewDistanceVector.y) > maximumPlayersVerticalDistance &&
            Mathf.Abs(previewDistanceVector.y) > Mathf.Abs(currentDistanceVector.y))
        {
          actualMovement.y = 0;
        }

        if (actualMovement != Vector2.zero)
        {
          HandleFootprintDistanceChanged(actualMovement);
          rb.MovePosition(rb.position + actualMovement);
        }
      }

      if (actualMovement != Vector2.zero)
      {
        TryCommitPosition();
      }

      animator.SetFloat("Speed", playerAction.RawInputMovement.magnitude);

      if (!inAttackingState)
      {
        if (movement.x < 0)
        {
          animatorTransform.localScale = new Vector2(1, 1);
          SetAnimatorHorizontalPosition(AnimatorDirection.Left);
          SetLaunchPosition(AnimatorDirection.Left);
        }
        else if (movement.x > 0)
        {
          animatorTransform.localScale = new Vector2(-1, 1);
          SetAnimatorHorizontalPosition(AnimatorDirection.Right);
          SetLaunchPosition(AnimatorDirection.Right);
        }
      }
      else
      {
        UpdateInAttackingStatePlayerDirection();
      }
    }

    private void HandleFootprintDistanceChanged(Vector2 movement)
    {
      var length = movement.magnitude;
      footprintSqrDistanceElapsed += length;
      footstepAudioSqrDistanceElapsed += length;

      if (footprintSqrDistanceElapsed >= FootprintDistance && footprintTransform != null)
      {
        footprintSqrDistanceElapsed = 0;
        var footprint = objectPool.GetObject("Environment/Footprint/Footprint");
        footprint.transform.SetParent(null);
        footprint.transform.position = footprintTransform.position;

        footprint.GetComponent<Footprint>()
          .SetFootprintDirection(footPrintIsRight)
          .SetMovementDirection(movement);

        footPrintIsRight = !footPrintIsRight;
      }

      if (footstepAudioSqrDistanceElapsed >= footstepAudioDistance * footstepAudioDistance &&
          footprintTransform != null)
      {
        footstepAudioSqrDistanceElapsed = 0;
        AudioWrapper.PlayClip(footprintClips[Random.Range(0, footprintClips.Length)], transform.position);
      }
    }

    private void UpdateInAttackingStatePlayerDirection()
    {
      animatorTransform.localScale = attackingLocalScale;
      if (attackingLocalScale.x > 0)
      {
        SetAnimatorHorizontalPosition(AnimatorDirection.Left);
        SetLaunchPosition(AnimatorDirection.Left);
      }
      else if (attackingLocalScale.x < 0)
      {
        SetAnimatorHorizontalPosition(AnimatorDirection.Right);
        SetLaunchPosition(AnimatorDirection.Right);
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          numberOfPlayers = ((LevelConfigurationState)state).NumberOfPlayers;
          break;
        case StoreNames.PlayerStore:
          var playerState = (PlayerState)state;
          movingSpeed = playerState.PlayerDatas[PlayerNumber].MovingSpeed;
          attackingMovingSpeed = playerState.PlayerDatas[PlayerNumber].AttackingMovingSpeed;
          break;
        case StoreNames.AttackControlStore:
          var attackControlState = (AttackControlState)state;

          // Receive message of whether the player is in attacking state fromQuat AttackControl.
          // Notice: AttackingState last for a moment, and will be managed by AttackControl.
          inAttackingState = attackControlState.AttackControlDatas[PlayerNumber].InAttackingState;

          if (inAttackingState) UpdateInAttackingStatePlayerDirection();
          break;
        case StoreNames.PlayerPositionStore:
          var playerPositionState = (PlayerPositionState)state;
          playerPositions = playerPositionState.PlayerPositions;
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void SetLaunchPosition(AnimatorDirection animatorDirection)
    {
      if (IndependentWeapon) return;

      var pos = launchPositionTransform.localPosition;

      switch (animatorDirection)
      {
        case AnimatorDirection.Left: pos.x = originalLaunchLocalHorizontalPosition; break;
        case AnimatorDirection.Right: pos.x = -originalLaunchLocalHorizontalPosition; break;
      }

      launchPositionTransform.localPosition = pos;
    }

    private void SetAnimatorHorizontalPosition(AnimatorDirection animatorDirection)
    {
      var lpos = animatorTransform.localPosition;
      switch (animatorDirection)
      {
        case AnimatorDirection.Left:
          animatorTransform.localPosition = new Vector2(symmetryAxisOffset, lpos.y);
          break;
        case AnimatorDirection.Right:
          animatorTransform.localPosition = new Vector2(-symmetryAxisOffset, lpos.y);
          break;
      }
    }

    private void SetPlayerDirection(PlayerDirection playerDirection)
    {
      switch (playerDirection)
      {
        case PlayerDirection.LEFT:
          attackingLocalScale = new Vector2(1, 1);
          break;
        case PlayerDirection.RIGHT:
          attackingLocalScale = new Vector2(-1, 1);
          break;
      }
    }

    public void OnEvent(Events @event, IEventData data)
    {
      switch (@event)
      {
        case Events.UPDATE_PLAYER_DIRECTION:
          var eventData = (UpdatePlayerDirectionEventData)data;
          if (eventData.PlayerNumber.Equals(PlayerNumber))
          {
            SetPlayerDirection(eventData.PlayerDirection);
          }

          break;
        default:
          throw new InvalidEventHandlingException(@event);
      }
    }

    private void Update()
    {
      if (paused)
      {
        return;
      }

      var dt = Time.deltaTime;

      if (AllowTeleportation && teleportationCountdown > 0)
      {
        teleportationCountdown -= dt;
        if (teleportationCountdown <= 0)
        {
          teleportationIndicator.SetActive(true);
        }
      }

      updatePlayerPositionTimeout -= dt;
      timeSinceLastOperation += dt;
      if (timeSinceLastOperation >= operationTimeout)
      {
        ResetTeleport();
      }

      if (updatePlayerPositionTimeout <= 0)
      {
        updatePlayerPositionTimeout = updatePlayerPositionInterval;
        TryCommitPosition();
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
      eventManager.RemoveEventHandler(this);
    }
  }
}