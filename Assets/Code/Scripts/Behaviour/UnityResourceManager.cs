using Code.Scripts.Src;
using System;
using System.Collections.Generic;
using Code.Scripts.Src;
using UnityEngine;

namespace Code.Scripts.Behaviour
{
  [Serializable]
  public struct PrefabDictionary
  {
    public string PrefabName;
    public GameObject Prefab;
  }

  [Serializable]
  public struct MaterialDictionary
  {
    public string MaterialName;
    public Material Material;
  }

  [Serializable]
  public struct ComponentDictionary
  {
    public string ComponentName;
    public GameObject Component;
  }

  [Serializable]
  public struct AnimatorDictionary
  {
    public string AnimatorName;
    public RuntimeAnimatorController AnimatorController;
  }

  [Serializable]
  public struct TextureDictionary
  {
    public string TextureName;
    public Texture2D Texture;
  }

  public class UnityResourceManager : MonoBehaviour
  {
    public List<PrefabDictionary> Prefabs;
    public List<MaterialDictionary> Materials;
    public List<ComponentDictionary> Components;
    public List<AnimatorDictionary> Animators;
    public List<TextureDictionary> Textures;

    private readonly Dictionary<string, GameObject> prefabCache = new();
    private readonly Dictionary<string, Material> materialCache = new();
    private readonly Dictionary<string, GameObject> componentCache = new();
    private readonly Dictionary<string, RuntimeAnimatorController> animatorCache = new();
    private readonly Dictionary<string, Texture2D> textureCache = new();

    private void Awake()
    {
      resourceManager = ResourceManager.Instance;

      foreach (var prefab in Prefabs)
      {
        prefabCache[prefab.PrefabName] = prefab.Prefab;
        resourceManager.SetResource(prefab.PrefabName, prefab.Prefab);
      }

      foreach (var material in Materials)
      {
        materialCache[material.MaterialName] = material.Material;
        resourceManager.SetResource(material.MaterialName, material.Material);
      }

      foreach (var component in Components)
      {
        componentCache[component.ComponentName] = component.Component;
        resourceManager.SetResource(component.ComponentName, component.Component);
      }

      foreach (var animator in Animators)
      {
        animatorCache[animator.AnimatorName] = animator.AnimatorController;
        resourceManager.SetResource(animator.AnimatorName, animator.AnimatorController);
      }

      foreach (var texture in Textures)
      {
        textureCache[texture.TextureName] = texture.Texture;
        resourceManager.SetResource(texture.TextureName, texture.Texture);
      }
    }

    public GameObject GetPrefab(string name) => prefabCache.TryGetValue(name, out var val) ? val : null;
    public Material GetMaterial(string name) => materialCache.TryGetValue(name, out var val) ? val : null;
    public GameObject GetComponent(string name) => componentCache.TryGetValue(name, out var val) ? val : null;
    public RuntimeAnimatorController GetAnimator(string name) => animatorCache.TryGetValue(name, out var val) ? val : null;
    public Texture2D GetTexture(string name) => textureCache.TryGetValue(name, out var val) ? val : null;

    private void OnDestroy()
    {
      foreach (var prefab in Prefabs)
      {
        resourceManager.RemoveResource(prefab.PrefabName);
      }

      foreach (var material in Materials)
      {
        resourceManager.RemoveResource(material.MaterialName);
      }

      foreach (var component in Components)
      {
        resourceManager.RemoveResource(component.ComponentName);
      }

      foreach (var animator in Animators)
      {
        resourceManager.RemoveResource(animator.AnimatorName);
      }

      foreach (var texture in Textures)
      {
        resourceManager.RemoveResource(texture.TextureName);
      }
    }
  }
}