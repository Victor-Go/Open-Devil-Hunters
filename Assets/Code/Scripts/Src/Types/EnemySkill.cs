using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Src.Types
{
  public class EnemySkill : PauseableGameObject, IPoolableGameObject
  {
    public string SkillPresetName;
    public string ObjectName { get; set; }

    public BaseEnemySkillConfigurations SkillConfigurations { get; private set; }

    public float NormalHitPoint => SkillConfigurations.HitPoint * (SkillConfigurations.StrengthUpgrade != null
      ? Mathf.Pow(1 + SkillConfigurations.StrengthUpgrade.IncreaseHitPointCoefficient, hostStrength)
      : 1);

    protected Rigidbody2D rb;
    protected ObjectPool objectPool;

    protected AudioClip launchClip;
    protected TrailRenderer trailRenderer;

    protected float hostStrength;

    protected override void Awake()
    {
      base.Awake();
      objectPool = ObjectPool.Instance;
      rb = GetComponent<Rigidbody2D>();

      if (!string.IsNullOrEmpty(SkillPresetName))
      {
        if (SkillPresets.BossSkills.ContainsKey(SkillPresetName))
        {
          SetConfigurations(SkillPresets.BossSkills[SkillPresetName].SkillConfigurations);
        }
        else
        {
          throw new System.Exception($"Specific skill preset is not found for {SkillPresetName}.");
        }
      }

      trailRenderer = GetComponentInChildren<TrailRenderer>();

      InitSound();
    }

    protected void InitSound()
    {
      if (launchClip == null && SkillConfigurations != null)
      {
        var launchSoundEffectName = SkillConfigurations.LaunchSoundEffectName;
        if (!string.IsNullOrEmpty(launchSoundEffectName))
        {
          launchClip = Resources.Load<AudioClip>(launchSoundEffectName);
        }
      }
    }

    protected void PlayLaunchSound()
    {
      InitSound();
      if (launchClip)
      {
        AudioWrapper.PlayClip(launchClip, transform.position);
      }
    }

    protected override void Start()
    {
      base.Start();
      PlayLaunchSound();
    }

    public virtual void ObjectReset(Vector2 initialPosition)
    {
      gameObject.SetActive(true);
      transform.localScale = Vector3.one;
      transform.position = initialPosition;
      if (trailRenderer != null)
      {
        trailRenderer.Clear();
      }

      PlayLaunchSound();
    }

    public void SetHostStrength(float strength)
    {
      hostStrength = strength;
    }

    public virtual EnemySkill SetConfigurations(BaseEnemySkillConfigurations configs)
    {
      SkillConfigurations = configs;
      return this;
    }
  }
}