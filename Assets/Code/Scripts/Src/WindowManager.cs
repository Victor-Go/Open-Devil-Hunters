using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Src
{
  public enum WindowNames
  {
    InitialUI,
    OptionsUI,
    ConfirmBoxUI,
    GameOverUI,
    GemUI,
    InfoBoxUI,
    PauseUI,
    UpgradeUI,
    UpgradeCard,
    SelectLanguage,
    FightPreparation,
    PickRuneUI,
    PickGemUI,
    FightReady,
    ControlGuide,
    FightPreparationGuideUI,
    FightReadyGuideUI,
  }

  public enum OpenWindowActions
  {
    Show,
    CloseOthers,
    WaitForOthers,
  }


  /**
   * UIWindow only manage gameObject that is opened by either WindowManager.OpenWindow or UIWindow.OpenSubWindow since it needs context to perform window management.
   * Otherwise, call Initialize manually to set parent and rect.
   */
  public abstract class UIWindow : MonoBehaviour
  {
    public bool CloseOnBackClicked;

    // If current window is the root window, then _windowManager will be set, otherwise, _parentWindow will be set. 
    protected WindowManager _windowManager;
    private UIWindow _parentWindow;

    protected RectTransform canvasRt;
    protected readonly List<UIWindow> _subWindows = new(); // Opened by current UIWindow
    protected readonly List<UIWindow> _childrenWindows = new(); // Children of current transform

    private bool closingCurrentWindow;

    public virtual void Initialize(WindowManager windowManager, RectTransform canvasRt)
    {
      _windowManager = windowManager;
      this.canvasRt = canvasRt;
    }

    public virtual void Initialize(UIWindow parentWindow, RectTransform canvasRt)
    {
      _parentWindow = parentWindow;
      this.canvasRt = canvasRt;
    }

    protected void RegisterChildWindow(UIWindow childWindow)
    {
      if (childWindow == this) return;

      _childrenWindows.Add(childWindow);
      childWindow.Initialize(this, canvasRt);
    }

    public bool HasSubWindow()
    {
      return _subWindows.Any() || _childrenWindows.Any(window => window.HasSubWindow());
    }

    public virtual void OnWindowOpen()
    {
    }

    public virtual void CloseWindow()
    {
      closingCurrentWindow = true;

      if (_subWindows.Any())
      {
        foreach (var window in _subWindows)
        {
          window.CloseWindow();
        }
      }
      else
      {
        MakeCurrentWindowClose();
      }
    }

    // For normal window, use CloseWindow() instead!!
    protected virtual void MakeCurrentWindowClose()
    {
      OnCurrentWindowClosed();
    }

    protected virtual void OnCurrentWindowClosed()
    {
      ReportClosedToParentWindow();
      if (gameObject != null)
      {
        Destroy(gameObject);
      }
      else
      {
        Debug.LogWarning($"gameObject {gameObject.name} is null");
      }
    }

    public virtual void OnSubWindowClosed(UIWindow window)
    {
      if (_subWindows.Contains(window))
      {
        _subWindows.RemoveAll(win => win == window);
      }

      if (closingCurrentWindow && _subWindows.Count == 0)
      {
        MakeCurrentWindowClose();
      }
    }

    private void ReportClosedToParentWindow()
    {
      if (_parentWindow != null)
      {
        _parentWindow.OnSubWindowClosed(this);
      }

      if (_windowManager != null)
      {
        _windowManager.OnWindowClosed(this);
      }

      if (_parentWindow == null && _windowManager == null)
      {
        throw new NullReferenceException($"Both _parentWindow and _windowManager are null in {name}!");
      }
    }

    protected GameObject OpenSubWindow(WindowNames windowName)
    {
      var window = Instantiate(Resources.Load<GameObject>(WindowManager._windows[windowName]), canvasRt,
        false);
      var windowScript = window.GetComponent<UIWindow>();
      if (windowScript == null)
      {
        throw new Exception($"{window.name} does not have a UIWindow component!");
      }

      _subWindows.Add(windowScript);

      windowScript.Initialize(this, canvasRt);
      windowScript.OnWindowOpen();

      return window;
    }

    protected virtual void Update()
    {
      if (CloseOnBackClicked &&
          InputUtils.GetBackButton() && 
          !_subWindows.Any() &&
          !_childrenWindows.Any(w => w.HasSubWindow()))
      {
        CloseWindow();
      }
    }
  }

  public class WindowManager : SingletonMonoBehaviour<WindowManager>
  {
    private RectTransform _canvasRt;

    private readonly List<UIWindow> _activeWindows = new();
    private readonly Stack<UIWindow> _waitingWindows = new(); // For WaitForOthers case
    private readonly Stack<GameObject> _waitingWindowGOs = new();
    private UIWindow onGoingWindow; // For CloseOthers case: Call OnWindowOpen after all windows closed
    private GameObject onGoingWindowGO;

    public static readonly Dictionary<WindowNames, string> _windows = new()
    {
      { WindowNames.InitialUI, "UI/Main/InitialUi" },
      { WindowNames.SelectLanguage, "UI/Main/SelectLanguageUI" },
      { WindowNames.OptionsUI, "UI/Main/OptionsUI" },
      { WindowNames.FightPreparation, "UI/Main/FightPreparationUI" },
      { WindowNames.PickRuneUI, "UI/Main/PickRuneUI" },
      { WindowNames.PickGemUI, "UI/Main/PickGemUI" },
      { WindowNames.FightReady, "UI/Main/FightReadyUI" },
      { WindowNames.ControlGuide, "UI/ControlGuide/ControlGuide" },
      { WindowNames.ConfirmBoxUI, "UI/Main/ConfirmBoxUI" },
      { WindowNames.GameOverUI, "UI/GameOver/GameOverUI" },
      { WindowNames.InfoBoxUI, "UI/Main/InfoBoxUI" },
      { WindowNames.PauseUI, "UI/Pause/PauseUI" },
      { WindowNames.UpgradeUI, "UI/Upgrade/UpgradeUI" },
      { WindowNames.UpgradeCard, "UI/Upgrade/UpgradeCard" },
      { WindowNames.GemUI, "UI/PickedGem/GemUI" },
      { WindowNames.FightPreparationGuideUI, "UI/Main/FightPreparationGuideUI" },
      { WindowNames.FightReadyGuideUI, "UI/Main/FightReadyGuideUI" },
    };

    protected override void Awake()
    {
      base.Awake();
      _canvasRt = GetComponent<RectTransform>();
    }

    private void doOpenWindow(UIWindow windowScript, GameObject windowGO)
    {
      windowGO.SetActive(true);
      _activeWindows.Add(windowScript);
      windowScript.OnWindowOpen();
      windowGO.transform.SetParent(_canvasRt, false);
    }

    public GameObject OpenWindow(WindowNames windowName, OpenWindowActions action)
    {
      if (!_windows.TryGetValue(windowName, out var window))
      {
        throw new InvalidResourceNameException($"Invalid window name: {windowName}");
      }

      var windowGO = Instantiate(Resources.Load<GameObject>(window));

      var windowScript = windowGO.GetComponent<UIWindow>();
      if (windowScript == null)
      {
        throw new Exception($"{windowGO.name} does not inherit from UIWindow!");
      }

      windowScript.Initialize(this, _canvasRt);

      // Has blocking window, wait for blocking window to close
      if (action == OpenWindowActions.WaitForOthers && _activeWindows.Any())
      {
        _waitingWindows.Push(windowScript);
        _waitingWindowGOs.Push(windowGO);
        windowGO.SetActive(false);
        return windowGO;
      }

      if (action == OpenWindowActions.CloseOthers && _activeWindows.Any())
      {
        if (onGoingWindow != null)
        {
          _activeWindows.Add(onGoingWindow);
          onGoingWindow = null;
        }
        else
        {
          onGoingWindow = windowScript;
          onGoingWindowGO = windowGO;
        }

        new List<UIWindow>(_activeWindows).ForEach(activeWindow => activeWindow.CloseWindow());

        return windowGO;
      }

      doOpenWindow(windowScript, windowGO);

      return windowGO;
    }

    public void OnWindowClosed(UIWindow window)
    {
      if (_activeWindows.Contains(window))
      {
        _activeWindows.Remove(window);
      }

      if (!_activeWindows.Any())
      {
        if (onGoingWindow != null)
        {
          doOpenWindow(onGoingWindow, onGoingWindowGO);
          onGoingWindow = null;
          return;
        }

        if (_waitingWindows.Any())
        {
          var topWindow = _waitingWindows.Pop();
          var topWindowGO = _waitingWindowGOs.Pop();
          doOpenWindow(topWindow, topWindowGO);
        }
      }
    }
  }
}