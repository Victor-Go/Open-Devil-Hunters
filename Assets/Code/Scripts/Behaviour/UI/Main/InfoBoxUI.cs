using Code.Scripts.Src.Utils;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class InfoBoxUI : UIAnimation
  {
    private GameObject confirmGameObject;

    private Text title;

    private void Awake()
    {
      confirmGameObject = transform.Find("InfoBox/Confirm").gameObject;
      targetTransform = transform.Find("InfoBox").GetComponent<RectTransform>();
      title = transform.Find("InfoBox/Title").GetComponent<Text>();
    }

    protected override void Start()
    {
      base.Start();

      EventSystem.current.SetSelectedGameObject(null);
      StartCoroutine(UIUtils.SetSelectedGameObject(confirmGameObject));
    }

    public InfoBoxUI SetTitle(string title)
    {
      this.title.text = title;
      return this;
    }

    public void OnConfirmClicked()
    {
      AudioWrapper.PlayClip(ResourceManager.Instance.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      CloseWindow();
    }
  }
}