using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using System.Collections;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill
{
  public class SpiralTimingSkill : PlayerBasicSkill
  {
    public float InitialAngle;

    private const float e = 2.7182818f;

    private readonly System.Func<float, float> r = theta => 0.8f * Mathf.Pow(e, 0.15f * theta);

    private TrailRenderer[] trailRenderers;
    private GameObject fireTrails;
    private GameObject iceTrails;
    private GameObject thunderTrails;
    private GameObject poisonTrails;

    private Vector2 basePosition;
    private float currentAngle;

    protected override void Awake()
    {
      base.Awake();

      trailRenderers = transform.GetComponentsInChildren<TrailRenderer>(true);

      fireTrails = transform.Find("FireTrails").gameObject;
      iceTrails = transform.Find("IceTrails").gameObject;
      thunderTrails = transform.Find("ThunderTrails").gameObject;
      poisonTrails = transform.Find("PoisonTrails").gameObject;
    }

    protected override void Start()
    {
      base.Start();
      basePosition = transform.position;

      Init();
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);
      basePosition = initialPosition;
      currentAngle = 0;

      Init();
    }

    private void Init()
    {
      StartCoroutine(ClearTrails());

      fireTrails.SetActive(false);
      iceTrails.SetActive(false);
      thunderTrails.SetActive(false);
      poisonTrails.SetActive(false);

      var skillHurt = SkillConfigurations.SkillHurt;
      var skillConfigs = (SpiralTimingSkillConfiguration)SkillConfigurations;
      int type = Random.Range(0, 4);
      switch (type)
      {
        case 0:
          fireTrails.SetActive(true);
          skillHurt.HurtType = HurtTypes.MAGIC_FIRE;
          SkillConfigurations.SkillHurt = skillHurt;
          SkillConfigurations.AdditionalEffect = new()
          {
            BasicAdditionalEffectType = BasicAdditionalEffectTypes.BURN,
            BasicAdditionalEffect = new FireAdditionalEffect()
            {
              Possibility = skillConfigs.AdditionalEffectPossibility,
              HurtPercentagePerSecond = skillConfigs.HurtPercentagePerSecond,
              LastForSeconds = skillConfigs.LastForSeconds,
            }
          };
          break;
        case 1:
          iceTrails.SetActive(true);
          skillHurt.HurtType = HurtTypes.MAGIC_ICE;
          SkillConfigurations.SkillHurt = skillHurt;
          SkillConfigurations.AdditionalEffect = new()
          {
            BasicAdditionalEffectType = BasicAdditionalEffectTypes.FREEZE,
            BasicAdditionalEffect = new IceAdditionalEffect()
            {
              Possibility = skillConfigs.AdditionalEffectPossibility,
              DamagePerSecond = skillConfigs.DamagePerSecond,
              LastForSeconds = skillConfigs.LastForSeconds,
            }
          };
          break;
        case 2:
          thunderTrails.SetActive(true);
          skillHurt.HurtType = HurtTypes.MAGIC_THUNDER;
          SkillConfigurations.SkillHurt = skillHurt;
          SkillConfigurations.AdditionalEffect = new()
          {
            BasicAdditionalEffectType = BasicAdditionalEffectTypes.STUN,
            BasicAdditionalEffect = new ThunderAdditionalEffect()
            {
              Possibility = skillConfigs.AdditionalEffectPossibility,
              StunningSeconds = skillConfigs.LastForSeconds * 1.25f,
            }
          };
          break;
        case 3:
          poisonTrails.SetActive(true);
          skillHurt.HurtType = HurtTypes.POISON;
          SkillConfigurations.SkillHurt = skillHurt;
          SkillConfigurations.AdditionalEffect = new()
          {
            PoisonAdditionalEffect = new()
            {
              Enable = true,
              HurtPercentagePerSecond = skillConfigs.HurtPercentagePerSecond,
              Possibility = skillConfigs.AdditionalEffectPossibility,
            }
          };
          break;
      }
    }

    public void SetInitialAngle(float angle)
    {
      InitialAngle = angle;
    }

    private IEnumerator ClearTrails()
    {
      yield return new WaitForEndOfFrame();
      foreach (var tr in trailRenderers)
      {
        tr.Clear();
      }
    }

    private void Polar(float angle)
    {
      float theta = angle * Mathf.Deg2Rad;

      transform.position = basePosition + (Vector2)(Quaternion.AngleAxis(InitialAngle + currentAngle, Vector3.forward) *
                                                    Vector2.right * r(theta));
    }

    private void FixedUpdate()
    {
      if (paused) return;

      Polar(currentAngle);

      var speed = SkillConfigurations.Speed;
      currentAngle += Time.fixedDeltaTime * speed;

      if (!GeneralUtils.IsInCamera(transform.position, 5))
      {
        objectPool.Recycle(ObjectName, gameObject);
      }
    }
  }
}