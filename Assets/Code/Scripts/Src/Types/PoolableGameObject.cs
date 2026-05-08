using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Src.Types
{
  public class PoolableGameObject : MonoBehaviour, IPoolableGameObject
  {
    public string ObjectName { get; set; }

    public virtual void ObjectReset(Vector2 initialPosition)
    {
      gameObject.SetActive(true);
    }
  }
}