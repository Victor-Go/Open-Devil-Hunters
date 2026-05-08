using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class SmeltingUI : AnimatedUI
  {
    public float SpeedCoefficient = 5;
    public GameObject DefaultButton;

    public GameObject LeftPart;
    public GameObject RightPart;

    private RectTransform leftRt;
    private RectTransform rightRt;

    private Vector2 leftInitialPosition;
    private Vector2 rightInitialPosition;

    private Vector2 leftDefaultPosition;
    private Vector2 rightDefaultPosition;

    private bool entering;
    private bool exiting;

    private string targetUiName;

    protected override void Awake()
    {
      base.Awake();

      leftRt = LeftPart.GetComponent<RectTransform>();
      rightRt = RightPart.GetComponent<RectTransform>();

      leftDefaultPosition = leftRt.anchoredPosition;
      rightDefaultPosition = rightRt.anchoredPosition;
    }

    public override void MakeAllUiEnter()
    {
      entering = true;

      leftInitialPosition = new Vector2(
        -canvasRt.rect.width / 2 - 100,
        0
      );
      leftRt.anchoredPosition = leftInitialPosition;

      rightInitialPosition = new Vector2(
        canvasRt.rect.width / 2 + 100,
        0
      );
      rightRt.anchoredPosition = rightInitialPosition;

      EventSystem.current.SetSelectedGameObject(null);
      StartCoroutine(SetSelected(DefaultButton));
    }

    public void OpenFightPreparation()
    {
      if (exiting)
      {
        return;
      }

      entering = false;
      exiting = true;
      targetUiName = "FightPreparation";
    }

    private void Update()
    {
      if (entering)
      {
        leftRt.anchoredPosition =
          Vector2.Lerp(leftRt.anchoredPosition, leftDefaultPosition, Time.deltaTime * SpeedCoefficient);
        rightRt.anchoredPosition = Vector2.Lerp(rightRt.anchoredPosition, rightDefaultPosition,
          Time.deltaTime * SpeedCoefficient);
        if ((leftRt.anchoredPosition - leftDefaultPosition).sqrMagnitude <= 1)
        {
          entering = false;
        }
      }
      else if (exiting)
      {
        leftRt.anchoredPosition =
          Vector2.Lerp(leftRt.anchoredPosition, leftInitialPosition, Time.deltaTime * SpeedCoefficient);
        rightRt.anchoredPosition = Vector2.Lerp(rightRt.anchoredPosition, rightInitialPosition,
          Time.deltaTime * SpeedCoefficient);

        if ((leftRt.anchoredPosition - leftInitialPosition).sqrMagnitude <= 30)
        {
          OpenSubWindow(WindowNames.FightPreparation);
          exiting = false;
          Destroy(gameObject);
        }
      }

      bool esc = InputUtils.GetBackButton();
      if (esc)
      {
        OpenFightPreparation();
      }
    }
  }
}