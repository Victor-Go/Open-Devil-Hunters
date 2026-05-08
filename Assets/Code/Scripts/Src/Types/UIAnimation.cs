using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Src.Types
{
  public enum FadeAnimationStatus
  {
    Appearing,
    Stable,
    Disappearing,
  }

  public class UIAnimation : UIWindow
  {
    public float AnimationDuration = 0.25f;

    protected Image[] images;
    protected FadeAnimationStatus status = FadeAnimationStatus.Stable;
    protected float timeElapsed;

    [SerializeField] protected RectTransform targetTransform;

    protected virtual void Start()
    {
      Init();
    }

    protected virtual void Init()
    {
      if (targetTransform == null)
      {
        Debug.LogWarning("UIAnimation: targetTransform is null");
        return;
      }

      var _images = targetTransform.GetComponentsInChildren<Image>();
      if (_images != null)
      {
        images = _images.Where(t => !t.tag.Equals("ExcludeUIFading")).ToArray();
      }

      targetTransform.localScale = Vector3.zero;
      SetImagesOpacity(0);

      timeElapsed = 0;
    }

    protected void SetImagesOpacity(float opacity)
    {
      if (images == null) return;
      foreach (var image in images)
      {
        var color = image.color;
        color.a = opacity;
        image.color = color;
      }
    }

    private void MakeAppear()
    {
      timeElapsed = 0;
      status = FadeAnimationStatus.Appearing;
    }

    public override void OnWindowOpen()
    {
      base.OnWindowOpen();
      MakeAppear();
    }

    protected override void MakeCurrentWindowClose()
    {
      base.MakeCurrentWindowClose();
      MakeDisappear();
    }

    private void MakeDisappear()
    {
      timeElapsed = 0;
      status = FadeAnimationStatus.Disappearing;
    }

    protected virtual void OnWindowOpened()
    {
    }

    protected override void Update()
    {
      base.Update();

      var dt = Time.deltaTime;
      switch (status)
      {
        case FadeAnimationStatus.Appearing:
          timeElapsed += dt;
          SetImagesOpacity(Mathf.Lerp(0, 1, timeElapsed / AnimationDuration));
          if (targetTransform != null)
            targetTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timeElapsed / AnimationDuration);
          break;
        case FadeAnimationStatus.Disappearing:
          timeElapsed += dt;
          SetImagesOpacity(Mathf.Lerp(1, 0, timeElapsed / AnimationDuration));
          if (targetTransform != null)
            targetTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timeElapsed / AnimationDuration);
          break;
      }

      if (status != FadeAnimationStatus.Stable && (targetTransform == null || timeElapsed > AnimationDuration))
      {
        if (status == FadeAnimationStatus.Disappearing)
        {
          OnCurrentWindowClosed();
          return;
        }

        status = FadeAnimationStatus.Stable;
        timeElapsed = 0;
        OnWindowOpened();
      }
    }
  }
}