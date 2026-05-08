using UnityEngine;

namespace Code.Scripts.Src.Configurations
{
  public static class DebugConfigurations
  {
    public static bool DebugEnabled { get; } = Application.isEditor;
  }
}