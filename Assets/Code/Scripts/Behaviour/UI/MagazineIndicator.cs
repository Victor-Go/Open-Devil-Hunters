using Code.Scripts.Src;
using Code.Scripts.Src.Store.Player;
using TMPro;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
  public class MagazineIndicator : MonoBehaviour, IStoreChangedHandler
  {
    public int PlayerNumber { get; set; }

    private StoreManager storeManager;
    private TextMeshPro textMesh;
    private TextMeshProUGUI textMeshUi;
    private RectTransform rectTransform;

    private int remainingRound;
    private int maxRound;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      storeManager.Subscribe(StoreNames.AttackControlStore, this);

      rectTransform = GetComponent<RectTransform>();
      textMesh = GetComponent<TextMeshPro>();
      textMeshUi = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
      var attackControlState = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore);
      SetData(attackControlState);
    }

    private void SetData(AttackControlState attackControlState)
    {
      remainingRound = attackControlState.AttackControlDatas[PlayerNumber].RemainingRoundCount;
      maxRound = attackControlState.AttackControlDatas[PlayerNumber].AttackControlConfigurations.AttackPerRound;

      if (remainingRound > 10000 || maxRound > 10000)
      {
        if (textMesh != null) textMesh.text = "";
        if (textMeshUi != null) textMeshUi.text = "";
      }
      else
      {
        if (textMesh != null) textMesh.text = $"{remainingRound}/{maxRound}";
        if (textMeshUi != null) textMeshUi.text = $"{remainingRound}/{maxRound}";
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

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}