using Code.Scripts.Src;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
  public class SocialNetworks : MonoBehaviour, IStoreChangedHandler
  {
    private StoreManager storeManager;
    private ResourceManager resourceManager;

    private GameObject china;
    private GameObject outside;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      resourceManager = ResourceManager.Instance;

      china = transform.Find("China").gameObject;
      outside = transform.Find("Outside").gameObject;
    }

    void Start()
    {
      var gameSetting = storeManager.GetState<GameSettingState>(StoreNames.GameSettingStore);
      SetRegion(gameSetting);

      storeManager.Subscribe(StoreNames.GameSettingStore, this);
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.GameSettingStore:
          SetRegion((GameSettingState)state); break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void SetRegion(GameSettingState gameSetting)
    {
      switch (gameSetting.Language)
      {
        case Languages.zh_CHS:
          china.SetActive(true);
          outside.SetActive(false);
          break;
        default:
          outside.SetActive(true);
          china.SetActive(false);
          break;
      }
    }

    public void OpenTwitter()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      Application.OpenURL("https://twitter.com/DevilHuntersJeu");
    }

    public void OpenDiscord()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      Application.OpenURL("https://discord.com/channels/1069899190618038342/1074942294161104918");
    }

    public void OpenWeibo()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      Application.OpenURL("https://www.weibo.com/u/7814928438");
    }

    public void OpenQQ()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      Application.OpenURL("https://jq.qq.com/?_wv=1027&k=ujDEESht");
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}