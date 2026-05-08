using System;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class ConfirmBoxUI : UIAnimation
  {
    private ResourceManager resourceManager;

    private Action confirmAction;
    private Action cancelAction;

    private GameObject confirmGameObject;

    private Text title;

    private void Awake()
    {
      resourceManager = ResourceManager.Instance;

      confirmGameObject = transform.Find("ConfirmBox/Confirm").gameObject;
      targetTransform = transform.Find("ConfirmBox").GetComponent<RectTransform>();
      title = transform.Find("ConfirmBox/Title").GetComponent<Text>();
    }

    protected override void Start()
    {
      base.Start();

      EventSystem.current.SetSelectedGameObject(null);
      StartCoroutine(UIUtils.SetSelectedGameObject(confirmGameObject));
    }

    public ConfirmBoxUI SetTitle(string title)
    {
      this.title.text = title;
      return this;
    }

    public ConfirmBoxUI SetConfirmAction(Action confirmAction)
    {
      this.confirmAction = confirmAction;
      return this;
    }

    public ConfirmBoxUI SetCancelAction(Action cancelAction)
    {
      this.cancelAction = cancelAction;
      return this;
    }

    public void OnConfirmClicked()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      confirmAction?.Invoke();
      CloseWindow();
    }

    public void OnCancelClicked()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      cancelAction?.Invoke();
      CloseWindow();
    }
  }
}