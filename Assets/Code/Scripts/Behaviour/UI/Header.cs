using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
  internal enum HeaderStatus
  {
    APPEARING,
    STABLE,
    DISAPPEARING,
  }

  public class Header : MonoBehaviour, MultiplayerUI
  {
    public int PlayerNumber;
    public float FinalY;
    public float AnimationTime;

    private float initY;

    private HeaderStatus status = HeaderStatus.STABLE;
    private RectTransform rectTransform;

    private float timePassed;

    private void Awake()
    {
      rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
      initY = rectTransform.anchoredPosition.y;
    }

    public void Appear()
    {
      if (Mathf.Approximately(rectTransform.anchoredPosition.y, FinalY)) return;

      status = HeaderStatus.APPEARING;
      timePassed = 0;
    }

    public void Disappear()
    {
      if (Mathf.Approximately(rectTransform.anchoredPosition.y, initY)) return;

      status = HeaderStatus.DISAPPEARING;
      timePassed = 0;
    }

    private void DoAppear(float dt)
    {
      timePassed += dt;
      rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(initY, FinalY, timePassed / AnimationTime));
      if (timePassed > AnimationTime)
      {
        status = HeaderStatus.STABLE;
      }
    }

    private void DoDisappear(float dt)
    {
      timePassed += dt;
      rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(FinalY, initY, timePassed / AnimationTime));
      if (timePassed > AnimationTime)
      {
        status = HeaderStatus.STABLE;
      }
    }

    private void Update()
    {
      switch (status)
      {
        case HeaderStatus.APPEARING:
          DoAppear(Time.deltaTime);
          break;
        case HeaderStatus.DISAPPEARING:
          DoDisappear(Time.deltaTime);
          break;
      }
    }
  }
}