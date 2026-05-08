using Code.Scripts.Src.Configurations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Code.Scripts.Behaviour.Character.Enemy
{
  public class EnemyCollisionHit : MonoBehaviour
  {
    public float NormalHitPoint => GeneralConfigurations.GetCollisionHitPointOverStrength(strength, baseHitPoint);

    private float baseHitPoint;
    private float strength;

    public void SetConfigurations(EnemyConfiguration config)
    {
      baseHitPoint = config.BaseNormalHitPoint;
    }

    public void SetBaseHitPoint(float hitPoint)
    {
      baseHitPoint = hitPoint;
    }

    public void SetStrength(float strength)
    {
      this.strength = strength;
    }
  }
}