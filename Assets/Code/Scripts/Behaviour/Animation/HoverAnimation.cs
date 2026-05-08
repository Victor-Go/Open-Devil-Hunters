using Code.Scripts.Src;
using Code.Scripts.Src;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.Behaviour.Animation
{
  public enum HoverStatus
  {
    EXPAND,
    STABLE,
    SHRINK
  }

  public class HoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    public float ExpandCoefficient = 1.25f;
    public bool PlayHoverSound = true;

    private ResourceManager resourceManager;

    private RectTransform targetRectTransform;

    private HoverStatus status = HoverStatus.STABLE;

    protected virtual void Awake()
    {
      resourceManager = ResourceManager.Instance;

      targetRectTransform = GetComponent<RectTransform>();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-hover"), Vector2.zero);
      status = HoverStatus.EXPAND;
      targetRectTransform.SetAsLastSibling();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
      status = HoverStatus.SHRINK;
      targetRectTransform.SetAsFirstSibling();
    }

    private void Update()
    {
      var dt = Time.deltaTime;
      switch (status)
      {
        case HoverStatus.EXPAND:
          targetRectTransform.localScale = Vector2.Lerp(targetRectTransform.localScale, Vector2.one * 1.25f, dt * 15);
          if (((Vector2)targetRectTransform.localScale - Vector2.one * ExpandCoefficient).magnitude < 0.05f)
          {
            status = HoverStatus.STABLE;
          }

          break;
        case HoverStatus.SHRINK:
          targetRectTransform.localScale = Vector2.Lerp(targetRectTransform.localScale, Vector2.one, dt * 15);
          if (((Vector2)targetRectTransform.localScale - Vector2.one).magnitude < 0.05f)
          {
            status = HoverStatus.STABLE;
          }

          break;
      }
    }
  }
}