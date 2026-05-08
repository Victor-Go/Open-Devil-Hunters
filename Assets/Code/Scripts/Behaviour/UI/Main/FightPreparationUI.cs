using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class FightPreparationUI : AnimatedUI
  {
    private ResourceManager _resourceManager;
    private List<UIWindow> _uiWindows;

    protected override void Awake()
    {
      base.Awake();
      _resourceManager = ResourceManager.Instance;
    }

    public override void Initialize(WindowManager windowManager, RectTransform canvasRt)
    {
      base.Initialize(windowManager, canvasRt);
      _uiWindows = GetComponentsInChildren<UIWindow>().ToList();
      _uiWindows.ForEach(RegisterChildWindow);
    }

    protected override void OnAllUiEnterAnimationFinished()
    {
      base.OnAllUiEnterAnimationFinished();

      if (!PlayerPrefs.HasKey("ShowedFightPreparationGuideUI") ||
          PlayerPrefs.GetInt("ShowedFightPreparationGuideUI") != 1)
      {
        OpenGuideWindow();
      }
    }

    public void OpenGuideWindow()
    {
      OpenSubWindow(WindowNames.FightPreparationGuideUI);
    }

    public override void Initialize(UIWindow parentWindow, RectTransform canvasRt)
    {
      base.Initialize(parentWindow, canvasRt);
      _uiWindows = GetComponentsInChildren<UIWindow>().ToList();
      _uiWindows.ForEach(window =>
      {
        if (window != this) window.Initialize(parentWindow, canvasRt);
      });

      OpenSubWindow(WindowNames.FightPreparationGuideUI);
    }

    protected override void OnCurrentWindowClosed()
    {
      base.OnCurrentWindowClosed();
      _windowManager.OpenWindow(WindowNames.InitialUI, OpenWindowActions.CloseOthers);
    }

    public void OpenFightReady()
    {
      AudioWrapper.PlayClip(_resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      OpenSubWindow(WindowNames.FightReady);
    }
  }
}