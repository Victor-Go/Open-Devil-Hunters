using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class SelectLanguageUI : UIAnimation
  {
    private void Awake()
    {
      targetTransform = transform.Find("SelectLanguage").GetComponent<RectTransform>();
    }

    public void Confirm()
    {
      AudioWrapper.PlayClip(ResourceManager.Instance.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      CloseWindow();
    }
  }
}