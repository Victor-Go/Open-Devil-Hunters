using System;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.I18n;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI
{
  public class I18n : MonoBehaviour, IStoreChangedHandler
  {
    public string I18nIndicator;
    private StoreManager storeManager;
    private Text text;
    private TMPro.TextMeshProUGUI tmpText;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      text = GetComponentInChildren<Text>();
      tmpText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    private void Start()
    {
      storeManager.Subscribe(StoreNames.GameSettingStore, this);

      if (!string.IsNullOrEmpty(I18nIndicator))
      {
        var str = I18nUtils.GetText(I18nIndicator);
        if (text != null) text.text = str;
        else if (tmpText != null) tmpText.text = str;
        else throw new NullReferenceException($"Cannot find appropriate text component for {I18nIndicator}.");
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.GameSettingStore:
          GameSettingState gameSettingState = (GameSettingState)state;
          text.text = I18nUtils.GetText(gameSettingState.Language, I18nIndicator);
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