using Code.Scripts.Src.Store.Level;
using UnityEngine;

namespace Code.Scripts.Src.Utils
{
  public static class GeneralUtils
  {
    private static Camera _mainCamera;
    public static Camera MainCamera
    {
      get
      {
        if (_mainCamera == null) _mainCamera = Camera.main;
        return _mainCamera;
      }
    }
    public static Vector2 Rotate(Vector2 vector, float degrees)
    {
      float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
      float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

      float tx = vector.x;
      float ty = vector.y;
      vector.x = (cos * tx) - (sin * ty);
      vector.y = (sin * tx) + (cos * ty);
      return vector;
    }

    public static float ConvertAngle(float angle)
    {
      angle = angle % 360;
      angle = (angle + 360) % 360;
      if (angle > 180)
      {
        angle -= 360;
      }

      return angle;
    }

    public static float LinearConvert(float fromValue, float from0, float to0, float from1, float to1)
    {
      float normalizedValue = from0 < to0 ? Mathf.Clamp(fromValue, from0, to0) : Mathf.Clamp(fromValue, to0, from0);
      return Mathf.Abs(normalizedValue - from0) / Mathf.Abs(to0 - from0) * (to1 - from1) + from1;
    }

    /**
     * @param margin: Position within edges + margin will be counted as In Camera.
     */
    public static bool IsInCamera(Vector2 worldPosition, float margin = 0)
    {
      var topRightCornerWorldPosition = (Vector2)MainCamera.ViewportToWorldPoint(new Vector2(1, 1));
      var bottomLeftCornerWorldPosition = (Vector2)MainCamera.ViewportToWorldPoint(new Vector2(0, 0));

      if (worldPosition.x < topRightCornerWorldPosition.x + margin &&
          worldPosition.x > bottomLeftCornerWorldPosition.x - margin &&
          worldPosition.y < topRightCornerWorldPosition.y + margin &&
          worldPosition.y > bottomLeftCornerWorldPosition.y - margin)
      {
        return true;
      }

      return false;
    }

    public static Vector2 GetNearestPosition(Vector2 currentPosition, params Vector2[] positions)
    {
      var nearest = Vector2.zero;
      var sqrDistance = float.MaxValue;
      foreach (var position in positions)
      {
        var _sqrDistance = (currentPosition - position).sqrMagnitude;
        if (_sqrDistance < sqrDistance)
        {
          nearest = position;
          sqrDistance = _sqrDistance;
        }
      }

      return nearest;
    }

    public static int GetFirstAlivePlayerNumber(LevelConfigurationState levelConfigs)
    {
      for (var i = 0; i < levelConfigs.PlayerAlive.Length; i++)
      {
        if (levelConfigs.PlayerAlive[i])
        {
          return i;
        }
      }

      return -1;
    }
  }
}