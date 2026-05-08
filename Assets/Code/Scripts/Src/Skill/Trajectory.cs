using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.Skill;
using Code.Scripts.Behaviour.Skill.WideRangeSkill;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Behaviour.Skill.Player;
using Code.Scripts.Behaviour.Skill.WideRangeSkill;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Scripts.Src.Skill
{
  public struct TrajectoryArguments : IEventData
  {
    [CanBeNull] public Vector2 PlayerPosition { get; set; }
    [CanBeNull] public Transform PlayerTransform { get; set; }
    [CanBeNull] public Vector2 LaunchPosition { get; set; }
    [CanBeNull] public Vector2 TargetPosition { get; set; }
    [CanBeNull] public List<GameObject> Targets { get; set; }
  }

  public enum TrajectoryTypes
  {
    NormalTrajectory,
    MineTrajectory,
    CircularTrajectory,
    CentralMultiTrajectory,
    SpiralTrajectory,
    JeanneDArcSkillTrajectory,
    HailTrajectory,
    MeteoriteTrajectory,
    ThunderTrajectory
  }

  public interface ITrajectory
  {
    public virtual void LaunchSkill(TrajectoryArguments trajectoryArguments, List<GameObject> skillObjects,
      float dispersion)
    {
    }
  }

  public class NormalTrajectory : ITrajectory
  {
    private static float ScatterRange { get; } = 1f;

    public static void LaunchSkill(TrajectoryArguments trajectoryArguments, List<GameObject> skillObjects,
      float dispersion)
    {
      var skillCount = skillObjects.Count;
      Vector2 launchPosition = trajectoryArguments.LaunchPosition,
        targetPosition = trajectoryArguments.TargetPosition;

      var lineOfSight = (targetPosition - launchPosition).normalized;
      var scatterPosition = Vector2.Perpendicular(lineOfSight).normalized * (ScatterRange / 2);
      var startPoint = scatterPosition - scatterPosition * (ScatterRange / skillCount);
      var segment = Vector2.Perpendicular(lineOfSight).normalized * ScatterRange / skillCount;

      for (var i = 0; i < skillCount; i++)
      {
        var skill = skillObjects[i];

        skill.transform.position = trajectoryArguments.LaunchPosition;

        var accurateDirection = lineOfSight + (startPoint - segment * i);

        var rotation = Random.Range(-dispersion / 2, dispersion / 2);
        var randomizedDirection = Quaternion.AngleAxis(rotation, Vector3.forward) * accurateDirection;

        var skillController = skill.GetComponent<PlayerBasicSkill>();
        skillController.SetTargetRelativeDirection(randomizedDirection);
      }
    }
  }

  public class MineTrajectory : ITrajectory
  {
    public static void LaunchSkill(TrajectoryArguments trajectoryArguments, List<GameObject> skillObjects,
      float dispersion)
    {
      if (skillObjects.Count == 1)
      {
        var mine = skillObjects.First();
        mine.transform.SetParent(null);
        mine.transform.position = trajectoryArguments.PlayerPosition;
      }
      else
      {
        var angle = 360 / skillObjects.Count;
        var initAngle = Random.Range(0, 360);
        for (int i = 0; i < skillObjects.Count; i++)
        {
          var mine = skillObjects[i];
          mine.transform.SetParent(null);
          mine.transform.position = trajectoryArguments.PlayerPosition +
                                    (Vector2)(Quaternion.AngleAxis(initAngle + angle * i, Vector3.forward) *
                                              Vector2.right);
        }
      }
    }
  }

  public class CircularTrajectory : ITrajectory
  {
    private static float spreadAngle { get; } = 90;

    public static void LaunchSkill(TrajectoryArguments trajectoryArguments, List<GameObject> skillObjects,
      float dispersion)
    {
      var skillCount = skillObjects.Count;
      Vector2 playerPosition = trajectoryArguments.PlayerPosition,
        targetPosition = trajectoryArguments.TargetPosition;
      var vectorFromPlayerToTarget = targetPosition - playerPosition;

      var startAngle = vectorFromPlayerToTarget.x < 0 ? 180 - spreadAngle / 2 : 0 - spreadAngle / 2;
      var segment = spreadAngle / (skillCount + 1);

      for (var i = 0; i < skillCount; i++)
      {
        var skill = skillObjects[i];

        skill.transform.position = trajectoryArguments.LaunchPosition;

        var circular = skill.GetComponent<CircularBullet>();
        circular.SetInitialDirection(Quaternion.AngleAxis(startAngle + (i + 1) * segment, Vector3.forward) *
                                     Vector2.right);
        circular.SetTargetPosition(targetPosition +
                                   (Vector2)(Quaternion.Euler(0, 0, Random.Range(0, 360f)) * Vector2.right *
                                             dispersion));
      }
    }
  }

  public class CentralMultiTrajectory : ITrajectory
  {
    public static void LaunchSkill(TrajectoryArguments trajectoryArguments, List<GameObject> skillObjects,
      float dispersion)
    {
      var skillCount = skillObjects.Count;
      Vector2 playerPosition = trajectoryArguments.PlayerPosition,
        targetPosition = trajectoryArguments.TargetPosition;
      var vectorFromPlayerToTarget = targetPosition - playerPosition;

      var startAngle = Vector2.SignedAngle(Vector2.right, vectorFromPlayerToTarget) +
                       Random.Range(-dispersion / 2, dispersion);
      var segment = 360 / skillCount;

      for (var i = 0; i < skillCount; i++)
      {
        var skill = skillObjects[i];

        skill.transform.position = playerPosition;

        skill.GetComponent<Bullet>()
          .SetTargetRelativeDirection(Quaternion.AngleAxis(startAngle + i * segment, Vector3.forward) * Vector2.right);
      }
    }
  }

  public class SpiralTrajectory : ITrajectory
  {
    public static void LaunchSkill(TrajectoryArguments trajectoryArguments, List<GameObject> skillObjects,
      float dispersion)
    {
      int skillCount = skillObjects.Count;
      Vector2 playerPosition = trajectoryArguments.PlayerPosition;

      var segment = 360 / skillCount;

      for (int i = 0; i < skillCount; i++)
      {
        var skill = skillObjects[i];

        skill.transform.position = playerPosition;

        skill.GetComponent<SpiralTimingSkill>().SetInitialAngle(i * segment);
      }
    }
  }

  public class JeanneDArcSkillTrajectory : ITrajectory
  {
    public static void LaunchSkill(TrajectoryArguments trajectoryArguments, List<GameObject> skillObjects,
      float dispersion)
    {
      var skillCount = skillObjects.Count;

      Vector2 playerPosition = trajectoryArguments.PlayerPosition,
        targetPosition = trajectoryArguments.TargetPosition,
        vectorFromPlayerToTarget = targetPosition - playerPosition;

      var size = skillObjects[0].GetComponent<BoxCollider2D>().size;
      var length = size.x / 2;
      var degree = 360f / skillCount;
      var angleToTarget = Vector2.SignedAngle(Vector2.right, vectorFromPlayerToTarget);

      for (var i = 0; i < skillCount; i++)
      {
        var skill = skillObjects[i];

        // If the skills are placed to far away, it may cause repel force problem (pushing enemies to the player).
        skill.transform.position = playerPosition +
                                   (Vector2)(Quaternion.AngleAxis(angleToTarget + i * degree, Vector3.forward) *
                                             (Vector2.right * length));
        skill.transform.rotation =
          Quaternion.Euler(0, 0, angleToTarget + degree * i);
      }
    }
  }

  public class HailTrajectory : ITrajectory
  {
    private const float range = 4;

    public static void LaunchSkill(TrajectoryArguments trajectoryArguments, List<GameObject> skillObjects,
      float dispersion)
    {
      var playerTransform = trajectoryArguments.PlayerTransform;
      var mainCamera = Camera.main;
      var bottomLeftCornerWorldPosition = (Vector2)mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
      var topRightCornerWorldPosition = (Vector2)mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
      var screenHeight = topRightCornerWorldPosition.y - bottomLeftCornerWorldPosition.y;

      foreach (var skillObject in skillObjects)
      {
        var targetPosition = (Vector2)(playerTransform.position +
                                       Quaternion.Euler(0, 0, Random.Range(0, 360f)) * Vector3.right *
                                       Random.Range(0.25f, range));
        skillObject.transform.position = targetPosition +
                                         (Vector2)(Quaternion.Euler(0, 0, Random.Range(60, 120f)) *
                                                   (Vector2.right * (screenHeight + 3)));

        skillObject.GetComponent<PlayerBasicSkill>().SetTargetPosition(targetPosition);
      }
    }
  }

  public class MeteoriteTrajectory : ITrajectory
  {
    private const float range = 4;

    public static void LaunchSkill(TrajectoryArguments trajectoryArguments, List<GameObject> skillObjects,
      float dispersion)
    {
      var playerTransform = trajectoryArguments.PlayerTransform;
      var mainCamera = Camera.main;
      var bottomLeftCornerWorldPosition = (Vector2)mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
      var topRightCornerWorldPosition = (Vector2)mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
      var screenHeight = topRightCornerWorldPosition.y - bottomLeftCornerWorldPosition.y;

      foreach (var skillObject in skillObjects)
      {
        var targetPosition = (Vector2)(playerTransform.position +
                                       Quaternion.Euler(0, 0, Random.Range(0, 360f)) * Vector3.right *
                                       Random.Range(0.25f, range));
        skillObject.transform.position = targetPosition +
                                         (Vector2)(Quaternion.Euler(0, 0, Random.Range(60, 120f)) *
                                                   (Vector2.right * (screenHeight + 3)));

        skillObject.GetComponent<PlayerBasicSkill>().SetTargetPosition(targetPosition);
      }
    }
  }

  public class ThunderTrajectory : ITrajectory
  {
    public static void LaunchSkill(TrajectoryArguments trajectoryArguments, List<GameObject> skillObjects,
      float dispersion)
    {
      if ((trajectoryArguments.Targets == null ||
           !trajectoryArguments.Targets.Count.Equals(skillObjects.Count)) &&
          DebugConfigurations.DebugEnabled)
      {
        throw new System.Exception("The number of targets should be the same as the number of skillObjects.");
      }

      var targets = trajectoryArguments.Targets;
      for (var i = 0; i < skillObjects.Count; i++)
      {
        var thunderSkill = skillObjects[i];

        var thunderController = thunderSkill.GetComponent<ThunderRangeSkill>();
        thunderController.SetTarget(targets[i]);
      }
    }
  }
}