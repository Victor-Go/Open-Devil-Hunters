using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Animation
{
  public class RecycleParticleSystemOnStop : PoolableGameObject
  {
    public bool PlayOnStart = true;
    public string[] PlaySoundOnStart;

    private ObjectPool objectPool;
    private new ParticleSystem particleSystem;

    private AudioWrapper audioWrapper;

    private void Awake()
    {
      objectPool = ObjectPool.Instance;

      particleSystem = GetComponentInChildren<ParticleSystem>();

      if (PlaySoundOnStart != null && PlaySoundOnStart.Length > 0)
      {
        audioWrapper = new AudioWrapper(PlaySoundOnStart, transform);
      }
    }

    private void Start()
    {
      PlaySound();
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      particleSystem.Play();
      PlaySound();
    }

    private void PlaySound()
    {
      if (audioWrapper != null)
      {
        audioWrapper.PlayRandomly();
      }
    }

    public void OnParticleSystemStopped()
    {
      objectPool.Recycle(ObjectName, gameObject);
    }
  }
}