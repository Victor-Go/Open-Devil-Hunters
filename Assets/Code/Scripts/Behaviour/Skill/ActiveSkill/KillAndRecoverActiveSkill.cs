using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Behaviour.Skill.ActiveSkill
{
  public class KillAndRecoverActiveSkill : PlayerActiveSkill
  {
    private int playerNumber;
    private string clearScheduleId;

    public override void Launch(int playerNumber)
    {
      this.playerNumber = playerNumber;

      storeManager.Commit(StoreNames.LevelGeneralStore, StoreActions.LevelGeneralStore_SET_RECOVER_ON_KILLED,
        new LevelGeneralData
        {
          PlayerNumber = playerNumber,
          RecoverPercentageOnKilled = 0.01f,
        });

      var configs = (PersistActiveSkillConfigurations)ActiveSkillConfigurations;
      clearScheduleId = scheduling.SetTimeout(ClearEffect, configs.Duration);
    }

    private void ClearEffect()
    {
      if (clearScheduleId != null)
      {
        scheduling.ClearSchedule(clearScheduleId);
        clearScheduleId = null;
      }

      storeManager.Commit(StoreNames.LevelGeneralStore, StoreActions.LevelGeneralStore_SET_RECOVER_ON_KILLED,
        new LevelGeneralData
        {
          PlayerNumber = playerNumber,
          RecoverPercentageOnKilled = 0.01f,
        });
    }

    ~KillAndRecoverActiveSkill()
    {
      ClearEffect();
    }
  }
}