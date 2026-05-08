using System;
using System.Collections.Generic;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Src
{
  public static class PreloadResources
  {
    public static readonly Dictionary<string, string> PreloadAudioClips = new()
    {
      { "AudioClip/buy", "Audio/Sound/UI/buy" },
      { "AudioClip/ui-button_0", "Audio/Sound/UI/ui-button_0" },
      { "AudioClip/ui-hover", "Audio/Sound/UI/ui-hover" },
      { "AudioClip/player-hurt_0", "Audio/Sound/Player/player-hurt_0" },
      { "AudioClip/success_0", "Audio/Sound/Event/success_0" },
      { "AudioClip/success_1", "Audio/Sound/Event/success_1" },
    };
  }

  public class InvalidResourceNameException : Exception
  {
    public InvalidResourceNameException() : base()
    {
    }

    public InvalidResourceNameException(string resourceName) : base($"Invalid Resource Name: {resourceName}")
    {
      Console.WriteLine("Invalid Resource Name: {0}", resourceName);
    }
  }

  public class ResourceManager : Singleton<ResourceManager>
  {
    private readonly Dictionary<string, dynamic> resources = new();

    protected ResourceManager() : base()
    {
      Init();
    }

    private void Init()
    {
      foreach (var (clipName, clipPath) in PreloadResources.PreloadAudioClips)
      {
        resources.Add(clipName, Resources.Load<AudioClip>(clipPath));
      }
    }

    public ResourceManager SetResource(string resourceName, dynamic resource)
    {
      resources.Add(resourceName, resource);
      return this;
    }

    public ResourceManager RemoveResource(string resourceName)
    {
      resources.Remove(resourceName);
      return this;
    }

    public dynamic GetResource(string resourceName)
    {
      return !resources.TryGetValue(resourceName, out var resource) ? Resources.Load(resourceName) : resource;
    }

    public void ClearResources()
    {
      resources.Clear();
      Init();
    }
  }
}