using Code.Scripts.Src;
using Code.Scripts.Src.Store.Player;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
  public class PlayerHpAttachedDisplayer : MonoBehaviour, IStoreChangedHandler
  {
    public float Margin;
    public int PlayerNumber { get; set; }

    private StoreManager storeManager;

    private Transform fill;
    private SpriteRenderer fillSR;
    private float width;

    private void Awake()
    {
      storeManager = StoreManager.Instance;

      var container = transform.Find("Container").transform;

      width = container.GetComponent<SpriteRenderer>().size.x - 2 * Margin;
      fill = container.Find("Fill");
      fillSR = fill.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
      storeManager.Subscribe(StoreNames.PlayerStore, this);

      var playerState = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
      UpdateState(playerState);
    }

    private void UpdateState(PlayerState state)
    {
      var playerData = state.PlayerDatas[PlayerNumber];
      ShowPercentage(playerData.CurrentHp / playerData.MaximumHp);
    }

    private void ShowPercentage(float percentage)
    {
      percentage = Mathf.Clamp01(percentage);

      fill.localPosition = new Vector2(-width / 2 + percentage / 2 * width, 0);
      fillSR.size = new Vector2(width * percentage, fillSR.size.y);
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.PlayerStore:
          var playerState = (PlayerState)state;
          UpdateState(playerState);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}