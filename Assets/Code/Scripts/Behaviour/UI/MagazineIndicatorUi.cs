using Code.Scripts.Src;
using Code.Scripts.Src.Store.Player;
using TMPro;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
  public class MagazineIndicatorUi : MonoBehaviour, IStoreChangedHandler
  {
    public int PlayerNumber { get; set; }

    private StoreManager storeManager;
    private TextMeshProUGUI textMeshUi;
    private RectTransform rectTransform;
    private GameObject canvasGameObject;

    private int remainingRound;
    private int maxRound;

    private void Awake()
    {
      storeManager = StoreManager.Instance;

      rectTransform = GetComponent<RectTransform>();
      textMeshUi = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
      storeManager.Subscribe(StoreNames.AttackControlStore, this);

      var attackControlState = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore);
      SetData(attackControlState);
    }

    private void SetData(AttackControlState attackControlState)
    {
      remainingRound = attackControlState.AttackControlDatas[PlayerNumber].RemainingRoundCount;
      maxRound = attackControlState.AttackControlDatas[PlayerNumber].AttackControlConfigurations.AttackPerRound;
      if (remainingRound > 10000 || maxRound > 10000)
      {
        textMeshUi.text = "";
      }
      else
      {
        textMeshUi.text = $"{remainingRound}/{maxRound}";
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.AttackControlStore:
          var attackControlState = (AttackControlState)state;
          SetData(attackControlState);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void Update()
    {
      Vector2 mousePosition = Input.mousePosition / GetComponentInParent<Canvas>().scaleFactor;
      rectTransform.anchoredPosition = mousePosition;
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}