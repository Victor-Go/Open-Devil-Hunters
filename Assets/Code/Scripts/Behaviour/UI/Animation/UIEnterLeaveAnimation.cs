using System;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts.Behaviour.UI.Animation
{
  public enum UIAnimationStatus
  {
    Entering,
    Stable,
    Leaving,
  }

  public struct InitialPositionParameters
  {
    public RectTransform CanvasRt { get; set; }
    public Vector2 CurrentRectSize { get; set; }
    public Vector2 CurrentDefaultPosition { get; set; }
  }

  public class UIEnterLeaveAnimation : MonoBehaviour
  {
    [FormerlySerializedAs("initialPositionFunctionId")]
    public string InitialPositionFunctionId;

    public Vector2 InitialScale = Vector2.one;

    private const float enterSpeedCoefficient = 5;
    private const float leaveSpeedCoefficient = 5;
    private const float threshold = 1;

    private UIAnimationStatus _status = UIAnimationStatus.Stable;

    private RectTransform _canvasRt;
    private RectTransform _rectTransform;
    private Func<InitialPositionParameters, Vector2> _initialPositionFunction;
    private Vector2 _defaultPosition;
    private Vector2 _initialPosition;
    private Vector2 _defaultScale;
    private Action _enterFinishedCallback;
    private Action _leaveFinishedCallback;

    private void Awake()
    {
      _rectTransform = GetComponent<RectTransform>();
      _defaultPosition = _rectTransform.anchoredPosition;
      _defaultScale = _rectTransform.localScale;

      if (!string.IsNullOrEmpty(InitialPositionFunctionId))
      {
        _initialPositionFunction = UIAnimationConfigurations.InitialPositionFunctions[InitialPositionFunctionId];
      }
    }

    private void Start()
    {
      GameObject canvasNode = ResourceManager.Instance.GetResource("/Canvas");
      _canvasRt = canvasNode.GetComponent<RectTransform>();

      if (_initialPositionFunction == null)
      {
        throw new Exception("Must provide initialPositionFunction in Awake!");
      }

      if (_canvasRt == null)
      {
        throw new Exception("Failed to get canvasRt from ResourceManager!");
      }

      _initialPosition = _initialPositionFunction(new InitialPositionParameters
      {
        CanvasRt = _canvasRt,
        CurrentRectSize = _rectTransform.rect.size,
        CurrentDefaultPosition = _defaultPosition,
      });
      _rectTransform.anchoredPosition = _initialPosition;

      transform.localScale = InitialScale;
    }

    public void MakeEnter(Action finishedCallback = null)
    {
      _status = UIAnimationStatus.Entering;
      _enterFinishedCallback = finishedCallback;
    }

    public void MakeLeave(Action finishedCallback = null)
    {
      _status = UIAnimationStatus.Leaving;
      _leaveFinishedCallback = finishedCallback;
    }

    private void Update()
    {
      var dt = Time.deltaTime;

      switch (_status)
      {
        case UIAnimationStatus.Entering:
          _rectTransform.anchoredPosition =
            Vector2.Lerp(_rectTransform.anchoredPosition, _defaultPosition, dt * enterSpeedCoefficient);
          _rectTransform.localScale =
            Vector2.Lerp(_rectTransform.localScale, _defaultScale, dt * leaveSpeedCoefficient);
          if ((_rectTransform.anchoredPosition - _defaultPosition).sqrMagnitude <= 1 &&
              ((Vector2)transform.localScale - _defaultScale).sqrMagnitude <= threshold * threshold)
          {
            _status = UIAnimationStatus.Stable;
            _enterFinishedCallback?.Invoke();
          }

          break;
        case UIAnimationStatus.Leaving:
          _rectTransform.anchoredPosition =
            Vector2.Lerp(_rectTransform.anchoredPosition, _initialPosition, dt * leaveSpeedCoefficient);
          _rectTransform.localScale = Vector2.Lerp(_rectTransform.localScale, InitialScale, dt * leaveSpeedCoefficient);

          if ((_rectTransform.anchoredPosition - _initialPosition).sqrMagnitude <= threshold * threshold &&
              ((Vector2)transform.localScale - InitialScale).sqrMagnitude <= threshold * threshold)
          {
            _status = UIAnimationStatus.Stable;
            if (_leaveFinishedCallback != null) _leaveFinishedCallback();
          }

          break;
      }
    }
  }
}