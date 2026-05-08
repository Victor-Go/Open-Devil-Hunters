using Code.Scripts.Behaviour.Skill;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Environment.Map
{
  public class TreeMonsterEnemy : PauseableGameObject
  {
    public float LaunchBulletInterval;
    public Transform LaunchPosition;
    public int BulletQuantity;


    private ObjectPool objectPool;
    private Scheduling scheduling;

    private string scheduleId;

    private EnemySkillPreset skillPreset;
    private float launchBulletTimeout;

    protected override void Awake()
    {
      base.Awake();

      objectPool = ObjectPool.Instance;
      scheduling = Scheduling.Instance;

      launchBulletTimeout = LaunchBulletInterval;
    }

    protected override void Start()
    {
      base.Start();

      skillPreset = SkillPresets.EnvironmentEnemySkills["TreeMonsterBullet"];
    }

    private void DoLaunchBullet(float angle)
    {
      var bullet = objectPool.GetObject(skillPreset.SkillPrefabName,
        LaunchPosition == null ? transform.position : LaunchPosition.position);
      bullet.GetComponent<TreeMonsterBullet>()
        .SetDirection(Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right)
        .SetConfigurations(skillPreset.SkillConfigurations)
        .SetHostStrength(0);  // Environment trees' bullets will not be upgraded
      bullet.transform.SetParent(null);
    }

    private void TriggerLaunchBullet()
    {
      animator.SetTrigger("GoToLaunch");
      float segment = 360 / BulletQuantity;
      float offset = Random.Range(0, 360);
      scheduleId = scheduling.SetTimeout(() =>
      {
        for (int i = 0; i < BulletQuantity; i++)
        {
          DoLaunchBullet(i * segment + offset);
        }

        animator.SetTrigger("GoToShake");
      }, 1);
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

    protected override void OnDestroy()
    {
      base.OnDestroy();

      scheduling.ClearSchedule(scheduleId);
    }
  }
}