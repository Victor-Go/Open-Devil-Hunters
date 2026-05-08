using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Level
{
  public class CameraManager : PauseableGameObject, IStoreChangedHandler, IEventHandler
  {
    private int numberOfPlayers;

    private List<Vector2> playerPositions = new();

    // Settings for camera size
    private float zoomSpeed { get; } = 1;
    private float cameraMinFieldOfView { get; } = 40;
    private float cameraMaxFieldOfView { get; } = 50;
    private float maxPlayerCameraDistance = 0.6f;
    private float minPlayerCameraDistance = 0.5f;
    private bool needToUpdateSize;

    private EventManager eventManager;
    private Camera mainCamera;

    private Vector2 offset;
    private float shakeCameraDuration { get; } = 0.5f;
    private float shakeCameraTimeout;

    private float cameraZ;
    private float targetCameraFieldOfView;
    private Vector3 targetPosition;
    private Vector3 stablePosition;

    protected override void Awake()
    {
      base.Awake();
      storeManager = StoreManager.Instance;
      eventManager = EventManager.Instance;

      mainCamera = GetComponent<Camera>();
      cameraZ = transform.position.z;

      stablePosition = transform.position;
    }

    protected override void Start()
    {
      base.Start();
      storeManager
        .Subscribe(StoreNames.LevelConfigurationStore, this)
        .Subscribe(StoreNames.PlayerPositionStore, this);
      eventManager.AddEventHandler(Events.PLAYER_HURT, this);

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      numberOfPlayers = levelConfigs.NumberOfPlayers;
    }

    private void UpdateCameraTargetPosition(Vector3 targetPosition)
    {
      this.targetPosition = targetPosition;

      if (!needToUpdateSize)
      {
        needToUpdateSize = IsNeedToUpdateCameraSize();
      }
    }

    private void UpdateCameraFieldOfView(float dt)
    {
      if (numberOfPlayers <= 1)
      {
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetCameraFieldOfView, dt);
        return;
      }

      #region Multiplayers

      var topRightCornerWorldPosition = (Vector2)mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
      var bottomLeftCornerWorldPosition = (Vector2)mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
      var size = new Vector2(topRightCornerWorldPosition.x - bottomLeftCornerWorldPosition.x,
        topRightCornerWorldPosition.y - bottomLeftCornerWorldPosition.y);
      float sizeDelta = 0;
      // Handle size shrink
      foreach (var playerPosition in playerPositions)
      {
        Vector2 distance = playerPosition - (Vector2)stablePosition;
        if (Mathf.Abs(distance.x) < size.x / 2 * minPlayerCameraDistance &&
            Mathf.Abs(distance.y) < size.y / 2 * minPlayerCameraDistance)
        {
          sizeDelta = -1;
          break;
        }
      }

      // Handle size expand
      foreach (var playerPosition in playerPositions)
      {
        Vector2 distance = playerPosition - (Vector2)stablePosition;
        if (Mathf.Abs(distance.x) > size.x / 2 * maxPlayerCameraDistance ||
            Mathf.Abs(distance.y) > size.y / 2 * maxPlayerCameraDistance)
        {
          sizeDelta = 1;
          break;
        }
      }

      float fieldOfView = Mathf.Clamp(mainCamera.fieldOfView + zoomSpeed * sizeDelta * dt, cameraMinFieldOfView,
        cameraMaxFieldOfView);
      mainCamera.fieldOfView = fieldOfView;

      #endregion
    }

    private bool IsNeedToUpdateCameraSize()
    {
      if (numberOfPlayers <= 1)
      {
        targetCameraFieldOfView = cameraMinFieldOfView;
        return Mathf.Abs(mainCamera.fieldOfView - targetCameraFieldOfView) > 0.5f;
      }

      var topRightCornerWorldPosition = (Vector2)mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
      var bottomLeftCornerWorldPosition = (Vector2)mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
      var size = new Vector2(topRightCornerWorldPosition.x - bottomLeftCornerWorldPosition.x,
        topRightCornerWorldPosition.y - bottomLeftCornerWorldPosition.y);

      foreach (var playerPosition in playerPositions)
      {
        Vector2 distance = playerPosition - (Vector2)stablePosition;
        if (Mathf.Abs(distance.x) > size.x / 2 * maxPlayerCameraDistance ||
            Mathf.Abs(distance.y) > size.y / 2 * maxPlayerCameraDistance ||
            Mathf.Abs(distance.x) < size.x / 2 * minPlayerCameraDistance ||
            Mathf.Abs(distance.y) < size.y / 2 * minPlayerCameraDistance)
        {
          return true;
        }
      }

      return false;
    }

    public override void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          var levelConfigs = (LevelConfigurationState)state;
          numberOfPlayers = levelConfigs.NumberOfPlayers;
          break;
        case StoreNames.PlayerPositionStore:
          PlayerPositionState positionState = (PlayerPositionState)state;
          playerPositions = new();
          for (int i = 0; i < positionState.NumberOfPlayers; i++)
          {
            if (positionState.PlayerAlive[i])
            {
              playerPositions.Add(positionState.PlayerPositions[i]);
            }
          }

          var tPos = positionState.CenterPosition;
          UpdateCameraTargetPosition(new Vector3(tPos.x, tPos.y, cameraZ));
          break;
        case StoreNames.GameStateStore:
          HandleGameStateChanged((GameState)state);
          break;
        default:
          throw new InvalidStoreEventException();
      }
    }

    private void EnableCameraShaking()
    {
      shakeCameraTimeout = shakeCameraDuration;
    }

    public void OnEvent(Events @event, IEventData data)
    {
      switch (@event)
      {
        case Events.PLAYER_HURT:
          EnableCameraShaking();
          break;
        default:
          throw new InvalidEventHandlingException(@event);
      }
    }

    private void Update()
    {
      if (!paused)
      {
        // Shake camera
        if (shakeCameraTimeout > 0)
        {
          shakeCameraTimeout -= Time.deltaTime;
          if (shakeCameraTimeout <= 0)
          {
            offset = Vector2.zero;
          }
          else
          {
            offset = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
          }
        }

        // Adjust camera size
        if (needToUpdateSize)
        {
          UpdateCameraFieldOfView(Time.deltaTime);
          needToUpdateSize = IsNeedToUpdateCameraSize();
        }

        // Adjust camera position
        stablePosition = Vector3.Lerp(stablePosition, new Vector3(targetPosition.x, targetPosition.y, cameraZ),
          Time.deltaTime);
        if (((Vector2)stablePosition - (Vector2)targetPosition).sqrMagnitude <= 0.01f) stablePosition = targetPosition;
        transform.position = stablePosition + (Vector3)offset;
      }
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      eventManager.RemoveEventHandler(this);
    }
  }
}