using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
  internal enum DisplayerStatus
  {
    APPEARING,
    STABLE,
    DISAPPEARING,
  }

  public class TimingSkillDisplayer : MonoBehaviour, IStoreChangedHandler
  {
    public int PlayerNumber;
    public float FinalX;
    private const float AnimationTime = 0.5f;

    private readonly StoreManager storeManager = StoreManager.Instance;

    private RectTransform rectTransform;

    private DisplayerStatus status = DisplayerStatus.STABLE;
    private float timePassed;

    private float initX;

    private bool appeared;

    private void Awake()
    {
      rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
      initX = rectTransform.anchoredPosition.x;

      storeManager.Subscribe(StoreNames.LevelConfigurationStore, this);

      var numberOfPlayers = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore)
        .NumberOfPlayers;
      if (PlayerNumber <= numberOfPlayers - 1)
      {
        Appear();
      }
    }

    private void Appear()
    {
      if (Mathf.Approximately(FinalX, rectTransform.anchoredPosition.y)) return;

      status = DisplayerStatus.APPEARING;
      timePassed = 0;
    }

    private void DoAppear(float dt)
    {
      timePassed += dt;
      rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(initX, FinalX, timePassed / AnimationTime), 0);
      if (timePassed > AnimationTime)
      {
        status = DisplayerStatus.STABLE;
        appeared = true;
      }
    }

    private void DoDisappear(float dt)
    {
      timePassed += dt;
      rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(FinalX, initX, timePassed / AnimationTime), 0);
      if (timePassed > AnimationTime)
      {
        status = DisplayerStatus.STABLE;
        appeared = false;
      }
    }

    private void Disappear()
    {
      if (Mathf.Approximately(initX, rectTransform.anchoredPosition.y)) return;

      status = DisplayerStatus.DISAPPEARING;
      timePassed = 0;
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          var levelConfigs = (LevelConfigurationState)state;
          var playerNumber = GeneralUtils.GetFirstAlivePlayerNumber(levelConfigs);
          var newNumberOfPlayers = ((LevelConfigurationState)state).NumberOfPlayers;

          if (newNumberOfPlayers <= 1 && appeared && playerNumber != PlayerNumber)
          {
            Disappear();
          }

          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void Update()
    {
      switch (status)
      {
        case DisplayerStatus.APPEARING:
          DoAppear(Time.deltaTime);
          break;
        case DisplayerStatus.DISAPPEARING:
          DoDisappear(Time.deltaTime);
          break;
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}