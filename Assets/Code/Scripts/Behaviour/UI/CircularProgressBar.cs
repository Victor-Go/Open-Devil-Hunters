using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
  public class CircularProgressBar : MonoBehaviour
  {
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
      spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetProgress(float percentage)
    {
      percentage %= 1;
      spriteRenderer.material.SetFloat("_Arc2", 360 * (1 - percentage));
    }
  }
}