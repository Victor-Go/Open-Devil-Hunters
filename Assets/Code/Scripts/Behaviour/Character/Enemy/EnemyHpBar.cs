using System;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy
{
  public class EnemyHpBar : MonoBehaviour
  {
    private Transform fill;
    private SpriteRenderer fillSR;
    private float width;

    private void Awake()
    {
      fill = transform.Find("Fill");
      fillSR = fill.GetComponent<SpriteRenderer>();
      width = fillSR.size.x;
    }

    public void SetPercentage(float percentage)
    {
      percentage = Mathf.Clamp01(percentage);
      
      fill.localPosition = new Vector2(-width / 2 + percentage / 2 * width, 0);
      fillSR.size = new Vector2(width * percentage, fillSR.size.y);
    }
  }
}