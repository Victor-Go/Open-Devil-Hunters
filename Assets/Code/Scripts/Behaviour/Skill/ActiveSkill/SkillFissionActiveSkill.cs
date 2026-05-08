using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Behaviour.Skill.ActiveSkill
{
  public class SkillFissionActiveSkillConfigurations : ActiveSkillConfigurations
  {
    public float PossibilityOfFission { get; set; }

    public SkillFissionActiveSkillConfigurations()
    {
    }

    public SkillFissionActiveSkillConfigurations(SkillFissionActiveSkillConfigurations other) : base(
      other)
    {
      PossibilityOfFission = other.PossibilityOfFission;
    }

    public override object Clone()
    {
      return new SkillFissionActiveSkillConfigurations(this);
    }
  }

  public class SkillFissionActiveSkill : PlayerActiveSkill
  {
    private string clearFissionScheduleId;

    public override void Launch(int playerNumber)
    {
      var possibility = ((SkillFissionActiveSkillConfigurations)ActiveSkillConfigurations).PossibilityOfFission;

      storeManager.Commit(StoreNames.ActiveSkillStore, StoreActions.ActiveSkillStore_SET_BULLET_FISSION_POSSIBILITY,
        new ActiveSkillData
        {
          BulletFissionPossibility = possibility,
        });

      clearFissionScheduleId = scheduling.SetTimeout(() =>
      {
        storeManager.Commit(StoreNames.ActiveSkillStore, StoreActions.ActiveSkillStore_SET_BULLET_FISSION_POSSIBILITY,
          new ActiveSkillData
          {
            BulletFissionPossibility = 0,
          });
      }, 10);
    }

    ~SkillFissionActiveSkill()
    {
      scheduling.ClearSchedule(clearFissionScheduleId);
    }
  }
}