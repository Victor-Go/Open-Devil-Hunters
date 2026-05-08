using System.Collections;
using System.Linq;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.UI.Animation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.Src.Types
{
  public abstract class AnimatedUI : UIWindow
  {
    protected UIEnterLeaveAnimation[] UIAnimations;
    protected int EnterAnimationCount;
    protected int ExitAnimationCount;

    protected bool enterAnimationFinished;

    protected virtual void Awake()
    {
      UIAnimations = GetComponentsInChildren<UIEnterLeaveAnimation>(false);
    }

    protected IEnumerator SetSelected(GameObject button)
    {
      yield return new WaitForEndOfFrame();
      EventSystem.current.SetSelectedGameObject(button);
    }

    public virtual void MakeAllUiEnter()
    {
      EnterAnimationCount = 0;

      if (UIAnimations.Any())
      {
        foreach (var animation in UIAnimations)
        {
          animation.MakeEnter(CountEnterAnimation);
          EnterAnimationCount++;
        }
      }
      else
      {
        OnAllUiEnterAnimationFinished();
      }
    }

    public override void OnWindowOpen()
    {
      base.OnWindowOpen();
      MakeAllUiEnter();
    }

    protected override void MakeCurrentWindowClose()
    {
      MakeExitAnimations();
    }

    protected void MakeExitAnimations()
    {
      ExitAnimationCount = 0;

      if (UIAnimations.Any())
      {
        foreach (var animation in UIAnimations)
        {
          animation.MakeLeave(CountExitAnimation);
          ExitAnimationCount++;
        }
      }
      else
      {
        OnAllUiExitAnimationFinished();
      }
    }

    private void CountEnterAnimation()
    {
      EnterAnimationCount--;
      if (EnterAnimationCount <= 0)
      {
        OnAllUiEnterAnimationFinished();
      }
    }

    private void CountExitAnimation()
    {
      ExitAnimationCount--;
      if (ExitAnimationCount <= 0)
      {
        OnAllUiExitAnimationFinished();
      }
    }

    protected virtual void OnAllUiEnterAnimationFinished()
    {
      enterAnimationFinished = true;
    }

    protected virtual void OnAllUiExitAnimationFinished()
    {
      OnCurrentWindowClosed();
    }

    protected override void Update()
    {
      if (
        enterAnimationFinished &&
        CloseOnBackClicked &&
        InputUtils.GetBackButton() &&
        !_subWindows.Any() &&
        !_childrenWindows.Any(w => w.HasSubWindow())
      )
      {
        CloseWindow();
      }
    }
  }
}