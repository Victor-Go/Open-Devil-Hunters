using System;
using UnityEngine;

namespace Code.Scripts.Src.Configurations
{
  public static class PlayerExperienceConfigurations
  {
    public static float GetExpPossibility => 0.1f;

    private const int a1 = 5;
    private const float q = 1.125f;

    public static int GetLevelUpRequiredExperience(int level)
    {
      return (int)(a1 * (1 - Math.Pow(q, level + 1)) / (1 - q));
    }
  }
}