using System;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Steam.Achievements;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scripts.Behaviour.Steam
{
  public class SteamIntegration : MonoBehaviour
  {
    private void Start()
    {
      if (Steamworks.SteamClient.IsValid) return;

      try
      {
        Steamworks.SteamClient.Init(GeneralConfigurations.SteamId);
        Debug.Log($"Steam name: {Steamworks.SteamClient.Name}");
      }
      catch (Exception e)
      {
        Debug.LogWarning(e);
      }
    }

    private void OnApplicationQuit()
    {
      Steamworks.SteamClient.Shutdown();
    }
  }
}