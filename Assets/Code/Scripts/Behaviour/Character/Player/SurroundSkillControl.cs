using System.Collections.Generic;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Player
{
  public enum SurroundSkillGroups
  {
    DART,
    BOOMERANG,
    ICE_TOWER,
    FIRE_TOWER,
    THUNDER_TOWER,
  }

  public class SurroundSkillControl : PauseableGameObject
  {
    public float RotationSpeed = 120;

    public int PlayerNumber { get; set; }

    private Dictionary<SurroundSkillGroups, int> groupSkillQuantity = new();
    private Dictionary<SurroundSkillGroups, List<GameObject>> surroundSkillGroups = new();
    private Dictionary<SurroundSkillGroups, List<Vector2>> desinatedPositionsGroups = new();
    private Dictionary<SurroundSkillGroups, bool> groupUpdatingPosition = new();
    private Dictionary<SurroundSkillGroups, float> groupDistances = new();

    public void AddSurroundSkill(SurroundSkillGroups group, GameObject skill, float distance)
    {
      if (!surroundSkillGroups.ContainsKey(group))
      {
        groupSkillQuantity[group] = 0;
        surroundSkillGroups[group] = new();
        desinatedPositionsGroups[group] = new();
        groupUpdatingPosition[group] = new();
        groupDistances[group] = distance;
      }

      if (groupSkillQuantity[group] >= GeneralConfigurations.MaximumSurroundSkills)
      {
        var errMsg = $"Only {GeneralConfigurations.MaximumAttackPerRound} surround skills can be added.";
        switch (DebugConfigurations.DebugEnabled)
        {
          case false:
            Debug.LogWarning(errMsg);
            return;
          case true: throw new System.Exception(errMsg);
        }
      }

      var finalQuantity = groupSkillQuantity[group] + 1;
      var angle = 360f / finalQuantity;

      skill.transform.SetParent(transform);
      skill.transform.localPosition = Quaternion.AngleAxis(groupSkillQuantity[group] * angle, Vector3.forward) *
                                      Vector2.up * groupDistances[group];

      surroundSkillGroups[group].Add(skill);
      desinatedPositionsGroups[group] = new();
      for (var i = 0; i < surroundSkillGroups[group].Count; i++)
      {
        desinatedPositionsGroups[group]
          .Add(Quaternion.AngleAxis(i * angle, Vector3.forward) * Vector2.up * groupDistances[group]);
      }

      groupSkillQuantity[group]++;
      groupUpdatingPosition[group] = true;
    }

    private void FixedUpdate()
    {
      if (paused) return;

      var dt = Time.fixedDeltaTime;
      foreach (var (group, _) in surroundSkillGroups)
      {
        if (groupUpdatingPosition[group])
        {
          bool finished = true;
          for (int i = 0; i < surroundSkillGroups[group].Count; i++)
          {
            var position = surroundSkillGroups[group][i].transform.localPosition;
            surroundSkillGroups[group][i].transform.localPosition =
              Vector2.Lerp(position, desinatedPositionsGroups[group][i], dt);
            finished &= ((Vector2)surroundSkillGroups[group][i].transform.localPosition -
                         desinatedPositionsGroups[group][i]).sqrMagnitude <= 0.01f;
          }

          if (finished)
          {
            groupUpdatingPosition[group] = false;
          }
        }
      }

      transform.rotation *= Quaternion.AngleAxis(RotationSpeed * dt, Vector3.forward);
    }
  }
}