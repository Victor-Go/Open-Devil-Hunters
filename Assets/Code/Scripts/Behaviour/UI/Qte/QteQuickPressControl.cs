using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI.Qte
{
  public enum QteType
  {
    FROZEN,
    BURNING
  }

  public class QteQuickPressControl : PauseableGameObject
  {
    public QteType qteType;

    private int playerNumber;

    public float PercentPerPress { get; set; }
    public float AutoDecreasePercentPerSecond { get; set; } = -0.1f;

    private GameObject slider;
    private SpriteRenderer sliderSprite;

    private float containerHeight;
    private float absInitialPosition;

    private SpriteRenderer[] allSR;

    private float percent;
    private bool handling = true;
    private bool zooming;

    private QteModeDisplayer qteMode;

    // Burning QTE
    private bool left;
    private bool whateverSide = true;

    private System.Action qteResolvedHandler;

    protected override void Awake()
    {
      base.Awake();

      slider = transform.Find("Slider").gameObject;

      sliderSprite = slider.GetComponent<SpriteRenderer>();

      containerHeight = GetComponent<SpriteRenderer>().size.y;
      absInitialPosition = 0.5f * containerHeight;

      allSR = GetComponentsInChildren<SpriteRenderer>(true);
      qteMode = GetComponentInChildren<QteModeDisplayer>();

      UpdateSlider();

      PercentPerPress = qteType == QteType.FROZEN ? 0.15f : 0.1f;
    }

    private void UpdateSlider()
    {
      var size = sliderSprite.size;
      sliderSprite.size = new Vector2(size.x, percent * containerHeight);
      slider.transform.localPosition = new Vector2(0, -absInitialPosition + sliderSprite.size.y / 2);
    }

    public void SetPlayerNumber(int playerNumber)
    {
      this.playerNumber = playerNumber;
      if (qteMode != null)
      {
        qteMode.SetPlayerNumber(playerNumber);
      }
    }

    public QteQuickPressControl SetQteResolvedHandler(System.Action action)
    {
      qteResolvedHandler = action;
      return this;
    }

    public void StartToDestroy()
    {
      handling = false;
      zooming = true;
    }

    private void Update()
    {
      if (paused)
      {
        return;
      }

      if (handling)
      {
        percent += AutoDecreasePercentPerSecond * Time.deltaTime;

        switch (qteType)
        {
          case QteType.FROZEN:
            if (InputUtils.GetButtonDown(playerNumber, InputButtonsDown.QTE1))
            {
              percent += PercentPerPress;
            }

            break;
          case QteType.BURNING:
            float horizontal = InputUtils.GetAxisRaw(playerNumber, InputAxises.LEFT_HORIZONTAL);
            bool triggeredLeft = horizontal < 0;
            bool triggeredRight = horizontal > 0;

            if (((left || whateverSide) && triggeredLeft) || ((!left || whateverSide) && triggeredRight))
            {
              whateverSide = false;
              left = !left;
              percent += PercentPerPress;
            }

            break;
        }

        percent = Mathf.Clamp(percent, 0, 1);
        UpdateSlider();

        if (percent >= 1)
        {
          qteResolvedHandler();
          StartToDestroy();
        }
      }

      if (zooming)
      {
        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(2, 2), Time.deltaTime * 10);
        foreach (var sr in allSR)
        {
          float alpha = 2 - transform.localScale.x;
          var color = sr.color;
          color.a = alpha;
          sr.color = color;
        }

        if (transform.localScale.x >= 2)
        {
          Destroy(gameObject);
        }
      }
    }
  }
}