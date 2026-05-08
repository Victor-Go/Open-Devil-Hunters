using System;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.Character.Enemy;
using Code.Scripts.Src.Types;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Behaviour.Environment.Map
{
  public class GraveyardBurningSkull : EnvironmentMovableObject
  {
    private const float baseHitPoint = 10;

    protected override void Awake()
    {
      base.Awake();

      GetComponentInChildren<EnemyCollisionHit>().SetBaseHitPoint(baseHitPoint);

      var a = Random.Range(-5, 5);
      var b = Random.Range(-5, 5);
      var c = Random.Range(-5, 5);
      df = x => (Mathf.Sin(a * x) + Mathf.Cos(b * x) + Mathf.Sin(Mathf.Cos(c * x))) / 1.5f;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer))
      {
        return;
      }

      RecycleGameObject();
    }
  }
}