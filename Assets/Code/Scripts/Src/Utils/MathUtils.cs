using UnityEngine;

namespace Code.Scripts.Src.Utils
{
  public static class MathUtils
  {
    // x starts from 0
    public static float GetExponential(float exponential, float maxValue, int maxX, float initialValue, float x)
    {
      return Mathf.Clamp((maxValue - initialValue) * Mathf.Pow(x / maxX, exponential) + initialValue, 0, maxValue);
    }

    // x starts from 0
    public static float GetPiecewise(float exponential, float maxValue, int maxX, float initialValue, float x)
    {
      return x <= maxX
        ? GetExponential(exponential, maxValue, maxX, initialValue, x)
        : maxValue * Mathf.Pow(x / maxX, -exponential);
    }

    public static float GetRandom(float value, float range)
    {
      return value * Random.Range(1 - range, 1 + range);
    }
  }
}