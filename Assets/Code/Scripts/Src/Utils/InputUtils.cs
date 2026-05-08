using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts.Src.Utils
{
  public enum InputAxises
  {
    LEFT_VERTICAL,
    LEFT_HORIZONTAL,
    RIGHT_VERTICAL,
    RIGHT_HORIZONTAL,
  }

  public enum InputButtons
  {
    GAMEPAD_X,
    GAMEPAD_Y,
    UP,
    DOWN,
    LEFT,
    RIGHT,
    FIRE,
    CONFIRM, // For FightPrepreration.
    MOUSE_FIRE, // For Xbox controller, it will trigger mouse left click when RT is deeply pressed. So, this effect should be eliminated.
  }

  public enum InputButtonsDown
  {
    QTE0,
    QTE1,
    AUTO_FIRING,
    ACTIVE_SKILL,
    TOGGLE_AUTO_AIMING,
  }

  public static class InputUtils
  {
    public static bool GetBackButton()
    {
      return Input.GetKeyDown(KeyCode.Escape)
             || Input.GetKeyDown(KeyCode.Joystick1Button1)
             || Input.GetKeyDown(KeyCode.Joystick1Button6)
             || Input.GetKeyDown(KeyCode.Joystick1Button10)
             || Input.GetKeyDown(KeyCode.Joystick1Button11)
             || Input.GetKeyDown(KeyCode.Joystick1Button17)
             || Input.GetKeyDown(KeyCode.Joystick2Button1)
             || Input.GetKeyDown(KeyCode.Joystick2Button6)
             || Input.GetKeyDown(KeyCode.Joystick2Button10)
             || Input.GetKeyDown(KeyCode.Joystick2Button11)
             || Input.GetKeyDown(KeyCode.Joystick2Button17);
    }

    public static float GetAxisRaw(int playerNumber, InputAxises inputAxis)
    {
      switch (inputAxis)
      {
        case InputAxises.LEFT_HORIZONTAL:
          return Input.GetAxisRaw($"Player{playerNumber}Horizontal");
        case InputAxises.LEFT_VERTICAL:
          return Input.GetAxisRaw($"Player{playerNumber}Vertical");
        case InputAxises.RIGHT_HORIZONTAL:
        {
          switch (Application.platform)
          {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXServer:
              return Input.GetAxisRaw($"Player{playerNumber}RightHorizontalMac");
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsServer:
            default:
              return Input.GetAxisRaw($"Player{playerNumber}RightHorizontal");
          }
        }
        case InputAxises.RIGHT_VERTICAL:
        {
          switch (Application.platform)
          {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXServer:
              return Input.GetAxisRaw($"Player{playerNumber}RightVerticalMac");
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsServer:
            default:
              return Input.GetAxisRaw($"Player{playerNumber}RightVertical");
          }
        }
      }

      throw new System.Exception($"Invalid player number {playerNumber}, axis {inputAxis}.");
    }

    public static bool GetButton(int playerNumber, InputButtons inputButton)
    {
      switch (inputButton)
      {
        case InputButtons.GAMEPAD_X:
          return playerNumber == 0
            ? Gamepad.all.Count > 0 && Gamepad.all[0].xButton.wasPressedThisFrame
            : Gamepad.all.Count > 1 && Gamepad.all[1].xButton.wasPressedThisFrame;
        case InputButtons.GAMEPAD_Y:
          return playerNumber == 0
            ? Gamepad.all.Count > 0 && Gamepad.all[0].yButton.wasPressedThisFrame
            : Gamepad.all.Count > 1 && Gamepad.all[1].yButton.wasPressedThisFrame;
        case InputButtons.FIRE:
          return Input.GetButton($"Player{playerNumber}Fire") || Input.GetAxisRaw(
            $"Player{playerNumber}RTFire") > 0;
        case InputButtons.CONFIRM:
          return playerNumber == 0
            ? Input.GetKeyDown(KeyCode.LeftControl) ||
              (Gamepad.all.Count > 0 && Gamepad.all[0].aButton.wasPressedThisFrame)
            : Input.GetKeyDown(KeyCode.RightControl) ||
              (Gamepad.all.Count > 1 && Gamepad.all[1].aButton.wasPressedThisFrame);
        case InputButtons.MOUSE_FIRE:
          return Input.GetButton("Player0MouseFire");
        case InputButtons.UP:
          return playerNumber == 0
            ? Input.GetKeyDown(KeyCode.W) || (Gamepad.all.Count > 0 && Gamepad.all[0].dpad.up.wasPressedThisFrame)
            : Input.GetKeyDown(KeyCode.UpArrow) ||
              (Gamepad.all.Count > 1 && Gamepad.all[1].dpad.up.wasPressedThisFrame);
        case InputButtons.DOWN:
          return playerNumber == 0
            ? Input.GetKeyDown(KeyCode.S) || (Gamepad.all.Count > 0 && Gamepad.all[0].dpad.down.wasPressedThisFrame)
            : Input.GetKeyDown(KeyCode.DownArrow) ||
              (Gamepad.all.Count > 1 && Gamepad.all[1].dpad.down.wasPressedThisFrame);
        case InputButtons.LEFT:
          return playerNumber == 0
            ? Input.GetKeyDown(KeyCode.A) || (Gamepad.all.Count > 0 && Gamepad.all[0].dpad.left.wasPressedThisFrame)
            : Input.GetKeyDown(KeyCode.LeftArrow) ||
              (Gamepad.all.Count > 1 && Gamepad.all[1].dpad.left.wasPressedThisFrame);
        case InputButtons.RIGHT:
          return playerNumber == 0
            ? Input.GetKeyDown(KeyCode.D) || (Gamepad.all.Count > 0 && Gamepad.all[0].dpad.right.wasPressedThisFrame)
            : Input.GetKeyDown(KeyCode.RightArrow) ||
              (Gamepad.all.Count > 1 && Gamepad.all[1].dpad.right.wasPressedThisFrame);
      }

      throw new System.Exception($"Invalid player number {playerNumber}, button {inputButton}.");
    }

    public static bool GetButtonDown(int playerNumber, InputButtonsDown inputButton)
    {
      switch (inputButton)
      {
        case InputButtonsDown.QTE0:
          return Input.GetButtonDown($"Player{playerNumber}QTE0");
        case InputButtonsDown.QTE1:
          return Input.GetButtonDown($"Player{playerNumber}QTE1");
        case InputButtonsDown.TOGGLE_AUTO_AIMING:
          return Input.GetButtonDown($"Player{playerNumber}AutoAiming");
        case InputButtonsDown.AUTO_FIRING:
          return Input.GetButtonDown($"Player{playerNumber}AutoFiring");
        case InputButtonsDown.ACTIVE_SKILL:
          return Input.GetButtonDown($"Player{playerNumber}ActiveSkill");
      }

      throw new System.Exception($"Invalid player number {playerNumber}, button down {inputButton}.");
    }
  }
}