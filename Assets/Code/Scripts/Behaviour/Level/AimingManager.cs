using System.Collections.Generic;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.UI;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.Level
{
  public enum AimingMode
  {
    MOUSE,
    AUTO_AIMING,
    JOYSTICK,
    NONE,
  }

  public enum PlayerDirection
  {
    LEFT,
    RIGHT
  }

  public class AimingManager : MonoBehaviour, IStoreChangedHandler
  {
    public List<Color> AimColors;

    private readonly float targetUpdateSpeed = GeneralConfigurations.UpdateAimingTargetFrequency;

    private int numberOfPlayers;
    private bool[] playerAlive;

    private ResourceManager resourceManager;
    private StoreManager storeManager;
    private EventManager eventManager;
    private float lastAimingElapsed;

    // Only player 0 can use mouse to aim
    private GameObject mouseAim;
    private GameObject[] autoAims;
    private GameObject[] controllerAims;
    private RectTransform[] controllerAimRectTransforms;
    private PlayerDirection[] lastPlayerDirections;
    private Vector2 controllerAimSize;
    private bool paused;
    private float scaleFactor;
    private Vector2 canvasSize;
    private bool[] inAttackingStates;

    private AimingMode[] previousValidAimingModes;
    private AimingMode[] currentAimingModes;

    private GameObject[] currentAimingTargets;

    private void Awake()
    {
      resourceManager = ResourceManager.Instance;
      storeManager = StoreManager.Instance;
      eventManager = EventManager.Instance;
    }

    private void Start()
    {
      storeManager
        .Subscribe(StoreNames.LevelConfigurationStore, this)
        .Subscribe(StoreNames.GameStateStore, this)
        .Subscribe(StoreNames.LevelStore, this)
        .Subscribe(StoreNames.AttackControlStore, this)
        .Subscribe(StoreNames.PlayerPositionStore, this);

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      numberOfPlayers = levelConfigs.NumberOfPlayers;
      playerAlive = levelConfigs.PlayerAlive;

      Cursor.lockState = CursorLockMode.Confined;
      GameObject uiCanvas = resourceManager.GetResource("/UiCanvas");

      var canvas = uiCanvas.GetComponent<Canvas>();
      var canvasRect = uiCanvas.GetComponent<RectTransform>().rect;

      canvasSize = new Vector2(canvasRect.width, canvasRect.height);
      scaleFactor = canvas.scaleFactor;

      previousValidAimingModes = new AimingMode[numberOfPlayers];
      currentAimingModes = new AimingMode[numberOfPlayers];
      controllerAimRectTransforms = new RectTransform[numberOfPlayers];
      inAttackingStates = new bool[numberOfPlayers];
      currentAimingTargets = new GameObject[numberOfPlayers];

      lastPlayerDirections = new PlayerDirection[numberOfPlayers];
      for (var playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
      {
        if (!playerAlive[playerNumber])
        {
          continue;
        }

        lastPlayerDirections[playerNumber] = GeneralConfigurations.DefaultAutoAimingPosition.x > 0
          ? PlayerDirection.RIGHT
          : PlayerDirection.LEFT;
      }

      mouseAim = Instantiate(resourceManager.GetResource("UI/Aiming/MouseAim"));
      mouseAim.transform.SetParent(uiCanvas.transform, false);

      autoAims = new GameObject[numberOfPlayers];
      controllerAims = new GameObject[numberOfPlayers];
      for (var playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
      {
        if (!playerAlive[playerNumber])
        {
          continue;
        }

        #region Auto aim

        GameObject autoAim = Instantiate(resourceManager.GetResource("UI/Aiming/AutoAim"));
        autoAim.GetComponent<SpriteRenderer>().color = AimColors[playerNumber];
        autoAim.GetComponentInChildren<AimManager>().SetPlayerNumber(playerNumber);
        autoAim.SetActive(false);
        autoAims[playerNumber] = autoAim;

        #endregion

        #region Controller aim

        GameObject controllerAim = Instantiate(resourceManager.GetResource("UI/Aiming/ControllerAim"));
        controllerAim.GetComponent<Image>().color = AimColors[playerNumber];
        controllerAim.GetComponentInChildren<AimManager>().SetPlayerNumber(playerNumber);
        controllerAim.transform.SetParent(uiCanvas.transform, false);
        controllerAims[playerNumber] = controllerAim;

        var controllerRT = controllerAims[playerNumber].GetComponent<RectTransform>();
        controllerAimRectTransforms[playerNumber] = controllerRT;
        controllerAimSize = new Vector2(controllerRT.rect.width, controllerRT.rect.height);

        InitControllerAim(playerNumber);

        #endregion

        // Auto aim as default aiming
        // if (numberOfPlayers <= 1)
        // {
        //   SetAimingMode(playerNumber, AimingMode.MOUSE);
        // }
        // else
        // {
        SetAimingMode(playerNumber, AimingMode.AUTO_AIMING);
        // }
      }

      // var gameState = storeManager.GetState<GameState>(StoreNames.GameStateStore);
      // HandleGameStateChanged(gameState.CurrentGameState);
    }

    private void InitControllerAim(int playerNumber)
    {
      SetControllerAimPosition(playerNumber, canvasSize / 2); // Move nothing, but set related data accordingly.
    }

    private void UpdatePlayerDirectionAccordingToTarget(int playerNumber)
    {
      if (!inAttackingStates[playerNumber] || !currentAimingModes[playerNumber].Equals(AimingMode.AUTO_AIMING))
      {
        return;
      }

      var playerPositions = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore).PlayerPositions;

      PlayerDirection direction;
      if (currentAimingTargets[playerNumber] != null && currentAimingTargets[playerNumber].activeSelf)
      {
        direction = ((Vector2)currentAimingTargets[playerNumber].transform.position - playerPositions[playerNumber]).x >
                    0
          ? PlayerDirection.RIGHT
          : PlayerDirection.LEFT;
      }
      else // If there's no target, then set player direction to last direction.
      {
        direction = lastPlayerDirections[playerNumber];
      }

      SetPlayerDirection(playerNumber, direction);
    }

    private void SetAimToTarget(int playerNumber, GameObject target)
    {
      if (target == null || !currentAimingModes[playerNumber].Equals(AimingMode.AUTO_AIMING))
      {
        autoAims[playerNumber].SetActive(false);
        return;
      }

      currentAimingTargets[playerNumber] = target;

      autoAims[playerNumber].transform.SetParent(target.transform);
      autoAims[playerNumber].SetActive(true);
      autoAims[playerNumber].transform.localPosition = Vector2.zero;
      storeManager.Commit(StoreNames.TargetEnemyStore, StoreActions.TargetEnemyStore_SET_TARGET_ENEMY,
        new TargetEnemyData()
        {
          PlayerNumber = playerNumber,
          TargetEnemy = target
        });

      UpdatePlayerDirectionAccordingToTarget(playerNumber);
    }

    private void UnsetAim(int playerNumber)
    {
      autoAims[playerNumber].transform.SetParent(transform);
      autoAims[playerNumber].SetActive(false);
    }

    private void SetToPreviousValidAimingMode(int playerNumber)
    {
      SetAimingMode(playerNumber, previousValidAimingModes[playerNumber]);
    }

    private void SetAimingMode(int playerNumber, AimingMode aimingMode)
    {
      if (!currentAimingModes[playerNumber].Equals(AimingMode.NONE))
      {
        previousValidAimingModes[playerNumber] = currentAimingModes[playerNumber];
      }

      currentAimingModes[playerNumber] = aimingMode;
      storeManager.Commit(StoreNames.LevelSettingStore, StoreActions.LevelSettingStore_SET_AIMING_MODE,
        new LevelSettingData()
        {
          PlayerNumber = playerNumber,
          AimingMode = aimingMode
        });

      switch (aimingMode)
      {
        case AimingMode.JOYSTICK:
          EnableJoystickAiming(playerNumber);
          break;
        case AimingMode.MOUSE:
          if (playerNumber == 0) // Only player 0 can use mouse aiming
          {
            EnableMouseAiming(playerNumber);
          }

          break;
        case AimingMode.AUTO_AIMING:
          EnableAutoAiming(playerNumber);
          break;
        case AimingMode.NONE:
          EnableMouse(playerNumber);
          break;
      }
    }

    private void EnableMouse(int playerNumber)
    {
      UnsetAim(playerNumber);
      controllerAims[playerNumber].SetActive(false);

      if (playerNumber == 0)
      {
        mouseAim.SetActive(false);
      }

      Cursor.visible = true;
    }

    // Only player 0 can use mouse aiming
    private void EnableMouseAiming(int playerNumber)
    {
      if (playerNumber != 0) return;

      UnsetAim(playerNumber);
      controllerAims[playerNumber].SetActive(false);
      mouseAim.SetActive(true);

      Cursor.visible = false;

      eventManager.PublishEvent(Events.AIMING_CHANGED, new AmingModeChangedEvent
      {
        PlayerNumber = playerNumber,
        AimingType = AimingTypes.Mouse,
      });
    }

    private void EnableJoystickAiming(int playerNumber)
    {
      UnsetAim(playerNumber);

      if (playerNumber == 0)
      {
        mouseAim.SetActive(false);
      }

      controllerAims[playerNumber].SetActive(true);

      Cursor.visible = false;
      eventManager.PublishEvent(Events.AIMING_CHANGED, new AmingModeChangedEvent
      {
        PlayerNumber = playerNumber,
        AimingType = AimingTypes.Controller,
      });
    }

    private void EnableAutoAiming(int playerNumber)
    {
      controllerAims[playerNumber].SetActive(false);

      if (playerNumber == 0)
      {
        mouseAim.SetActive(false);
      }

      autoAims[playerNumber].SetActive(true);

      Cursor.visible = false;

      var nearestEnemy = AttackUtils.GetNearestEnemy(playerNumber, storeManager);
      SetAimToTarget(playerNumber, nearestEnemy);
      eventManager.PublishEvent(Events.AIMING_CHANGED, new AmingModeChangedEvent
      {
        PlayerNumber = playerNumber,
        AimingType = AimingTypes.Automatic,
      });
    }

    /**
    * Publish an event to indicate the facing of a player during attack
    * Notice: It just tells MovementControl the facing of a player during attack. But it lets MovementControl to decide whether to use. Because when the player is walking, appearently it should follow the movingDirection of movement.
    */
    private void SetPlayerDirection(int playerNumber, PlayerDirection direction)
    {
      lastPlayerDirections[playerNumber] = direction;
      eventManager.PublishEvent(Events.UPDATE_PLAYER_DIRECTION, new UpdatePlayerDirectionEventData()
      {
        PlayerNumber = playerNumber,
        PlayerDirection = direction
      });
    }

    private void HandleGameStateChanged(GameStates gameState)
    {
      paused = !LevelUtils.PlayerCanMove(gameState);
      if (paused || gameState == GameStates.PAUSED_OTHERS)
      {
        for (var playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
        {
          if (!playerAlive[playerNumber])
          {
            continue;
          }

          SetAimingMode(playerNumber, AimingMode.NONE);
        }
      }
      else
      {
        for (var playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
        {
          if (!playerAlive[playerNumber])
          {
            continue;
          }

          SetToPreviousValidAimingMode(playerNumber);
        }
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          var levelConfigs = (LevelConfigurationState)state;
          numberOfPlayers = levelConfigs.NumberOfPlayers;
          playerAlive = levelConfigs.PlayerAlive;
          break;
        case StoreNames.GameStateStore:
          var gameState = (GameState)state;
          HandleGameStateChanged(gameState.CurrentGameState);

          break;
        case StoreNames.LevelStore: // Enemy list changed.
          for (var playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
          {
            if (!playerAlive[playerNumber])
            {
              continue;
            }

            var target = AttackUtils.GetNearestEnemy(playerNumber, storeManager);
            SetAimToTarget(playerNumber, target);
          }

          break;
        case StoreNames.AttackControlStore:
          var attackControlState = (AttackControlState)state;
          for (var playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
          {
            if (!playerAlive[playerNumber])
            {
              continue;
            }

            inAttackingStates[playerNumber] = attackControlState.AttackControlDatas[playerNumber].InAttackingState;
          }

          break;
        case StoreNames.PlayerPositionStore:
          var playerPositions = ((PlayerPositionState)state).PlayerPositions;
          for (var playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
          {
            if (!playerAlive[playerNumber])
            {
              continue;
            }

            /**
             * When is in controller aiming mode, the controller aim may change world position when another player moves.
             * Thus, need to update aim world position and player direction in this case.
             */
            if (currentAimingModes[playerNumber] == AimingMode.JOYSTICK)
            {
              ControllerAimSetPlayerDirection(playerNumber, playerPositions);
              ControllerAimUpdateScreenToWorldPoint(playerNumber);
            }
          }

          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void ControllerAimSetPlayerDirection(int playerNumber, Vector2[] playerPositions)
    {
      if (currentAimingModes[playerNumber] != AimingMode.JOYSTICK) return;

      var controllerAimWorldPosition =
        Camera.main.ScreenToWorldPoint(controllerAimRectTransforms[playerNumber].anchoredPosition);
      var controllerAimPlayerDirection = controllerAimWorldPosition.x > playerPositions[playerNumber].x
        ? PlayerDirection.RIGHT
        : PlayerDirection.LEFT;
      SetPlayerDirection(playerNumber, controllerAimPlayerDirection);
    }

    /**
     * Updating the ScreenToWorldPoint is necessary for controller aiming since when there are multiple players, when another player moves, it will also change the ScreenToWorldPoint position for current player.
     */
    private void ControllerAimUpdateScreenToWorldPoint(int playerNumber)
    {
      if (currentAimingModes[playerNumber] != AimingMode.JOYSTICK) return;

      storeManager.Commit(StoreNames.TargetEnemyStore, StoreActions.TargetEnemyStore_SET_CONTROLLER_AIM_POSITION,
        new TargetEnemyData()
        {
          PlayerNumber = playerNumber,
          WorldSpaceControllerAimPosition =
            Camera.main.ScreenToWorldPoint(controllerAimRectTransforms[playerNumber].anchoredPosition)
        });
    }

    private void TryToggleAutoAiming(int playerNumber)
    {
      if (paused)
      {
        return;
      }

      if (currentAimingModes[playerNumber].Equals(AimingMode.AUTO_AIMING))
      {
        if (playerNumber == 0)
        {
          SetAimingMode(playerNumber, AimingMode.MOUSE);
        }
      }
      else
      {
        SetAimingMode(playerNumber, AimingMode.AUTO_AIMING);
      }
    }

    private void OnApplicationFocus(bool focus)
    {
      if (focus)
      {
        for (var playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
        {
          if (!playerAlive[playerNumber])
          {
            continue;
          }

          if (currentAimingModes[playerNumber].Equals(AimingMode.NONE))
          {
            Cursor.visible = true;
            return;
          }
        }

        Cursor.visible = false;
      }
      else
      {
        Cursor.visible = true;
      }
    }

    private void SetControllerAimPosition(int playerNumber, Vector2 rawControllerAimAnchoredPosition)
    {
      controllerAimRectTransforms[playerNumber].anchoredPosition =
        new Vector2(
          Mathf.Clamp(rawControllerAimAnchoredPosition.x, controllerAimSize.x, canvasSize.x - controllerAimSize.x),
          Mathf.Clamp(rawControllerAimAnchoredPosition.y, controllerAimSize.x, canvasSize.y - controllerAimSize.y));

      storeManager.Commit(StoreNames.TargetEnemyStore, StoreActions.TargetEnemyStore_SET_CONTROLLER_AIM_POSITION,
        new TargetEnemyData()
        {
          PlayerNumber = playerNumber,
          WorldSpaceControllerAimPosition =
            Camera.main.ScreenToWorldPoint(controllerAimRectTransforms[playerNumber].anchoredPosition)
        });

      // As controller aim moves, should update player movingDirection.
      var playerPositions = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore).PlayerPositions;
      ControllerAimSetPlayerDirection(playerNumber, playerPositions);
    }

    private void MoveControllerAimPosition(int playerNumber, Vector2 controllerAimMovement)
    {
      var screenSqrMag = (canvasSize / 2).sqrMagnitude;

      var playerPositions = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore).PlayerPositions;
      var playerScreenPosition = (Vector2)Camera.main.WorldToScreenPoint(playerPositions[playerNumber]);

      var aimSpeedCoefficient =
        ((controllerAimRectTransforms[playerNumber].anchoredPosition) - playerScreenPosition).sqrMagnitude /
        screenSqrMag + 0.3f;
      aimSpeedCoefficient = aimSpeedCoefficient > 1 ? 1 : aimSpeedCoefficient;

      var anchoredPosition = controllerAimRectTransforms[playerNumber].anchoredPosition + controllerAimMovement *
        (GeneralConfigurations.JoystickAimingSensibility * aimSpeedCoefficient);

      // Correct controller aim anchoredPosition.
      SetControllerAimPosition(playerNumber, anchoredPosition);
    }

    private void Update()
    {
      if (paused)
      {
        return;
      }

      for (var playerNumber = 0; playerNumber < GeneralConfigurations.MaximumPlayers; playerNumber++)
      {
        if (!playerAlive[playerNumber])
        {
          continue;
        }

        var aimingMode = currentAimingModes[playerNumber];
        if (aimingMode.Equals(AimingMode.JOYSTICK) ||
            aimingMode.Equals(AimingMode.MOUSE) ||
            aimingMode.Equals(AimingMode.AUTO_AIMING))
        {
          var toggleAutoAiming = InputUtils.GetButtonDown(playerNumber, InputButtonsDown.TOGGLE_AUTO_AIMING);

          var controllerAimMovement = Vector2.zero;
          controllerAimMovement.x = InputUtils.GetAxisRaw(playerNumber, InputAxises.RIGHT_HORIZONTAL);
          controllerAimMovement.y = InputUtils.GetAxisRaw(playerNumber, InputAxises.RIGHT_VERTICAL);

          Vector2 mouseMovement;
          mouseMovement.x = Input.GetAxis("Mouse X");
          mouseMovement.y = Input.GetAxis("Mouse Y");

          #region Set aiming mode according to joystick movement.

          // Set aiming mode to auto-aiming.
          if (toggleAutoAiming && playerNumber == 0)
          {
            TryToggleAutoAiming(playerNumber);
          }

          // Set aiming mode to joystick.
          if (controllerAimMovement.sqrMagnitude >= 0.1f &&
              mouseMovement.sqrMagnitude == 0 &&
              !currentAimingModes[playerNumber].Equals(AimingMode.JOYSTICK))
          {
            SetAimingMode(playerNumber, AimingMode.JOYSTICK);
          }

          // Set aiming mode to mouse.
          if (mouseMovement.sqrMagnitude >= 0.1f &&
              !currentAimingModes[0].Equals(AimingMode.MOUSE))
          {
            SetAimingMode(0, AimingMode.MOUSE);
          }

          #endregion

          #region Handle joystick.

          if (controllerAimMovement.sqrMagnitude != 0 && currentAimingModes[playerNumber].Equals(AimingMode.JOYSTICK))
          {
            MoveControllerAimPosition(playerNumber, controllerAimMovement);
          }

          #endregion

          #region Handle auto-aiming.

          if (currentAimingModes[playerNumber].Equals(AimingMode.AUTO_AIMING))
          {
            lastAimingElapsed += Time.deltaTime;
            if (lastAimingElapsed >= (1 / targetUpdateSpeed))
            {
              lastAimingElapsed = 0;
              GameObject nearestEnemy = AttackUtils.GetNearestEnemy(playerNumber, storeManager);
              SetAimToTarget(playerNumber, nearestEnemy);
            }
            else
            {
              UpdatePlayerDirectionAccordingToTarget(playerNumber);
            }
          }

          #endregion

          #region Handle mouse.

          if (playerNumber == 0 && currentAimingModes[0].Equals(AimingMode.MOUSE))
          {
            Vector2 mousePosition = Input.mousePosition / scaleFactor;
            SetPlayerDirection(playerNumber,
              mousePosition.x - canvasSize.x / 2 > 0 ? PlayerDirection.RIGHT : PlayerDirection.LEFT);
          }

          #endregion
        }
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
      Cursor.lockState = CursorLockMode.None;
    }
  }
}