using Code.Scripts.Src;
using UnityEngine;

namespace Code.Scripts.Src.Types
{
  public interface IPoolableGameObject
  {
    public string ObjectName { get; set; }
    public void ObjectReset(Vector2 initialPosition);
  }

  public abstract class PoolableAndPauseableGameObject : PauseableGameObject, IPoolableGameObject
  {
    public string ObjectName { get; set; }

    protected ObjectPool objectPool;

    protected override void Awake()
    {
      base.Awake();
      objectPool = ObjectPool.Instance;
    }

    public virtual void ObjectReset(Vector2 initialPosition)
    {
      gameObject.SetActive(true);
    }
  }
}