using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.WideRangeSkill
{
  public class HailRangeSkill : PlayerBasicSkill
  {
    private Rigidbody2D rb;
    private float speed = 6;

    protected override void Awake()
    {
      base.Awake();

      rb = GetComponent<Rigidbody2D>();
    }

    private void ShowAnimation()
    {
      var explosions = new[]
      {
        "Status/ActiveSkill/HailExplosion0",
        "Status/ActiveSkill/HailExplosion1",
      };

      string selectedExplosion = explosions[Random.Range(0, explosions.Length)];
      GameObject animation = objectPool.GetObject(selectedExplosion);
      animation.transform.SetParent(null);
      animation.transform.position = transform.position;
    }

    private void OnRecycle()
    {
      ShowAnimation();
      objectPool.Recycle(ObjectName, gameObject);
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer))
      {
        return;
      }

      OnRecycle();
    }

    private void FixedUpdate()
    {
      if (!paused)
      {
        rb.MovePosition(rb.position + relativeDirection * speed * Time.fixedDeltaTime);
        if ((rb.position - targetPosition).sqrMagnitude <= 0.1f)
        {
          Activated = true;
          OnRecycle();
        }
      }
    }
  }
}