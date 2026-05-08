using Code.Scripts.Src;
using Code.Scripts.Behaviour.Skill.Bullets;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Environment.Map
{
  public class ParabolaEnemy : PauseableGameObject
  {
    public string SkillProfileName;
    public float LaunchBulletInterval;
    public Transform LaunchPosition;
    public int BulletQuantity;

    private ObjectPool objectPool;

    private EnemySkillPreset skillPreset;
    private float launchBulletTimeout;

    protected override void Awake()
    {
      base.Awake();

      objectPool = ObjectPool.Instance;

      launchBulletTimeout = LaunchBulletInterval;
    }

    protected override void Start()
    {
      base.Start();

      skillPreset = SkillPresets.EnvironmentEnemySkills[SkillProfileName];
    }

    private void DoLaunchBullet()
    {
      var bullet = objectPool.GetObject(skillPreset.SkillPrefabName,
        LaunchPosition == null ? transform.position : LaunchPosition.position);
      var bulletController = bullet.GetComponent<ParabolaBullet>();
      bulletController.SetConfigurations(skillPreset.SkillConfigurations);
      bulletController.SetHostStrength(0);  // Environment trees' bullets will not be upgraded

      bullet.transform.SetParent(null);
    }

    public void TriggerLaunchBullet()
    {
      animator.SetTrigger("Launch");
    }

    public void LaunchAnimationFinished()
    {
      animator.ResetTrigger("Launch");
      for (int i = 0; i < BulletQuantity; i++)
      {
        DoLaunchBullet();
      }
    }

    private void FixedUpdate()
    {
      if (paused)
      {
        return;
      }

      launchBulletTimeout -= Time.fixedDeltaTime;
      if (launchBulletTimeout <= 0)
      {
        launchBulletTimeout = LaunchBulletInterval;

        TriggerLaunchBullet();
      }
    }
  }
}