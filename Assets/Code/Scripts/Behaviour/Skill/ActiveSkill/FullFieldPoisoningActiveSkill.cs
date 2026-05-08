using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Behaviour.Skill.ActiveSkill
{
  public class FullFieldPoisoningActiveSkill : PlayerActiveSkill
  {
    private string clearPoisonScheduleId;

    public static void AddPoisonEffect(int playerNumber, Enemy enemyScript)
    {
      enemyScript.enemyHurtController.HandleSkillAdditionalEffects(playerNumber, new()
      {
        PoisonAdditionalEffect = new()
        {
          Enable = true,
          HurtPercentagePerSecond = 0.05f,
          Possibility = 1f
        }
      });
    }

    public override void Launch(int playerNumber)
    {
      storeManager.Commit(StoreNames.LevelGeneralStore, StoreActions.LevelGeneralStore_SET_ENEMY_SPAWN_POISONED,
        new LevelGeneralData()
        {
          PoisonedOnSpawn = true
        });

      var enemies = storeManager.GetState<LevelState>(StoreNames.LevelStore).Enemies;
      foreach (var enemy in enemies)
      {
        AddPoisonEffect(playerNumber, enemy.GetComponent<Enemy>());
      }

      var configs = (PersistActiveSkillConfigurations)ActiveSkillConfigurations;
      clearPoisonScheduleId = scheduling.SetTimeout(ClearEffect, configs.Duration);
    }

    private void ClearEffect()
    {
      if (clearPoisonScheduleId != null)
      {
        scheduling.ClearSchedule(clearPoisonScheduleId);
        clearPoisonScheduleId = null;
      }

      storeManager.Commit(StoreNames.LevelGeneralStore, StoreActions.LevelGeneralStore_SET_ENEMY_SPAWN_POISONED,
        new LevelGeneralData()
        {
          PoisonedOnSpawn = false
        });
    }

    ~FullFieldPoisoningActiveSkill()
    {
      ClearEffect();
    }
  }
}