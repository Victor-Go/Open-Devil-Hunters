using Code.Scripts.Behaviour.UI;
using Code.Scripts.Src;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI.Qte
{
  public class QtePreciseControl : PauseableGameObject
  {
    private int playerNumber;

    private const float speed = 1f;

    private Scheduling scheduling;

    private GameObject target;
    private GameObject left;
    private GameObject right;
    private GameObject selector;

    private QteModeDisplayer qteMode;
    private SpriteRenderer targetSprite;
    private SpriteRenderer leftSprite;
    private SpriteRenderer rightSprite;

    private float containerWidth;
    private Vector2 defaultLocalPosition;

    private float targetPercent = 0.2f;
    private float absInitialPosition;
    private bool moveSelector = true;
    private bool zooming;

    private string shakeScheduleId;
    private string clearShakeScheduleId;

    private SpriteRenderer[] allSR;

    private System.Action QteResolvedHandler;

    protected override void Awake()
    {
      base.Awake();
      scheduling = Scheduling.Instance;

      qteMode = GetComponentInChildren<QteModeDisplayer>();

      target = transform.Find("Target").gameObject;
      left = transform.Find("Left").gameObject;
      right = transform.Find("Right").gameObject;
      selector = transform.Find("Selector").gameObject;

      targetSprite = target.GetComponent<SpriteRenderer>();
      leftSprite = left.GetComponent<SpriteRenderer>();
      rightSprite = right.GetComponent<SpriteRenderer>();

      containerWidth = GetComponent<SpriteRenderer>().size.x;

      absInitialPosition = 0.5f * containerWidth;
      selector.transform.localPosition = new Vector2(-absInitialPosition, 0);

      allSR = GetComponentsInChildren<SpriteRenderer>(true);
    }

    protected override void Start()
    {
      base.Start();
      defaultLocalPosition = transform.localPosition;
    }

    public void SetPlayerNumber(int playerNumber)
    {
      this.playerNumber = playerNumber;
      qteMode.SetPlayerNumber(playerNumber);
    }

    public QtePreciseControl SetQteResolvedHandler(System.Action action)
    {
      QteResolvedHandler = action;
      return this;
    }

    public void StartToDestroy()
    {
      zooming = true;
    }

    public QtePreciseControl SetTargetPercent(float percent)
    {
      percent = Mathf.Clamp(percent, 0.15f, 0.35f);
      targetPercent = percent;
      var size = targetSprite.size;
      targetSprite.size = new Vector2(containerWidth * percent, size.y);

      float absPosition = containerWidth * ((1 + percent) / 4);
      leftSprite.size = new Vector2(containerWidth * (1 - percent) * 0.5f, size.y);
      left.transform.localPosition = new Vector2(-absPosition, 0);
      rightSprite.size = new Vector2(containerWidth * (1 - percent) * 0.5f, size.y);
      right.transform.localPosition = new Vector2(absPosition, 0);
      return this;
    }

    private void Shake()
    {
      selector.gameObject.SetActive(false);
      selector.transform.localPosition = new Vector2(-absInitialPosition, 0);

      ClearSchedule();

      shakeScheduleId =
        scheduling.SetInterval(
          () =>
          {
            transform.localPosition =
              defaultLocalPosition + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
          }, 0.05f);

      clearShakeScheduleId = scheduling.SetTimeout(() =>
      {
        scheduling.ClearSchedule(shakeScheduleId);
        shakeScheduleId = null;
        clearShakeScheduleId = null;

        selector.gameObject.SetActive(true);
        transform.localPosition = defaultLocalPosition;
        selector.transform.localPosition = new Vector2(-absInitialPosition, 0);
        moveSelector = true;
      }, 0.5f);
    }

    private void Update()
    {
      if (paused)
      {
        return;
      }

      if (moveSelector)
      {
        var position = selector.transform.localPosition;
        selector.transform.localPosition = new Vector2(position.x + Time.deltaTime * speed, 0);
        if (selector.transform.localPosition.x >= absInitialPosition)
        {
          selector.transform.localPosition = new Vector2(-absInitialPosition, 0);
        }

        if (InputUtils.GetButtonDown(playerNumber, InputButtonsDown.QTE0))
        {
          moveSelector = false;
          var percent = (selector.transform.localPosition.x + absInitialPosition) / containerWidth;
          if (0.5f - targetPercent / 2 < percent && percent < 0.5f + targetPercent / 2)
          {
            QteResolvedHandler();
            StartToDestroy();
          }
          else
          {
            moveSelector = false;
            Shake();
          }
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

    private void ClearSchedule()
    {
      if (shakeScheduleId != null && !shakeScheduleId.Equals(""))
      {
        scheduling.ClearSchedule(shakeScheduleId);
      }

      if (clearShakeScheduleId != null && !clearShakeScheduleId.Equals(""))
      {
        scheduling.ClearSchedule(clearShakeScheduleId);
      }
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      ClearSchedule();
    }
  }
}