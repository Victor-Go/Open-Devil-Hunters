using Code.Scripts.Src.Configurations;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using UnityEngine;

namespace Code.Scripts.Src.Types
{
  public abstract class Map : MonoBehaviour
  {
    protected SpriteRenderer spriteRenderer;

    protected List<Texture2D> backgroundTextures = new();
    protected List<BackgroundDecorationConfig> backgroundDecorationConfigs;
    protected int minimumDecorations;
    protected int maximumDecorations;

    protected ResourceManager resourceManager;
    protected Vector2 size;

    protected virtual void Awake()
    {
      resourceManager = ResourceManager.Instance;

      spriteRenderer = GetComponent<SpriteRenderer>();
      size = spriteRenderer.size;
    }

    protected virtual void Start()
    {
      SpawnDecorations();
    }

    public Map SetMapBackgroundNames(List<string> backgroundNames)
    {
      if (null == backgroundNames || !backgroundNames.Any())
      {
        throw new System.ArgumentNullException("No map background is found.");
      }

      foreach (var bgName in backgroundNames)
      {
        backgroundTextures.Add((Texture2D)resourceManager.GetResource(bgName));
      }

      return this;
    }

    public Map SetBackgroundDecorationConfigs(List<BackgroundDecorationConfig> bgDecorationConfigs)
    {
      backgroundDecorationConfigs = bgDecorationConfigs;
      return this;
    }

    public Map SetMinimumDecorations(int minimumDecorations)
    {
      this.minimumDecorations = minimumDecorations;
      return this;
    }

    public Map SetMaximumDecorations(int maximumDecorations)
    {
      this.maximumDecorations = maximumDecorations;
      return this;
    }

    public void Init()
    {
      int backgroundTexturesCount = backgroundTextures.Count;
      Texture2D texture = backgroundTextures[Random.Range(0, backgroundTexturesCount)];
      spriteRenderer.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
        new Vector2(0.5f, 0.5f), 100);
    }

    protected GameObject GetRandomDecoration()
    {
      int totalCoefficient = 0;
      foreach (var decoration in backgroundDecorationConfigs)
      {
        totalCoefficient += decoration.Coefficient;
      }

      int random;
      if (totalCoefficient == 0)
      {
        random = Random.Range(0, backgroundDecorationConfigs.Count);
        return Instantiate(resourceManager.GetResource(backgroundDecorationConfigs[random].PrefabName));
      }
      else
      {
        float elapsedCoefficient = 0;
        random = Random.Range(0, totalCoefficient);
        foreach (var decoration in backgroundDecorationConfigs)
        {
          if (elapsedCoefficient <= random && random < elapsedCoefficient + decoration.Coefficient)
          {
            return Instantiate(resourceManager.GetResource(decoration.PrefabName));
          }

          elapsedCoefficient += decoration.Coefficient;
        }
        
        throw new System.Exception("Random decoration error.");
      }
    }

    protected void SpawnDecorations()
    {
      if (backgroundDecorationConfigs.Count == 0)
      {
        return;
      }

      resourceManager = ResourceManager.Instance;

      Vector2 biggestDecorationSize = Vector2.zero;
      foreach (var backgroundDecorationConfig in backgroundDecorationConfigs)
      {
        GameObject decoration = resourceManager.GetResource(backgroundDecorationConfig.PrefabName);
        if (decoration == null)
        {
          Debug.LogErrorFormat("Unable to find prefab: {0}", backgroundDecorationConfig.PrefabName);
        }

        var sr = decoration.GetComponent<SpriteRenderer>();
        var c = decoration.GetComponent<BoxCollider2D>();
        var decorationSize = Vector2.one * 2;

        if (sr)
        {
          decorationSize = sr.size;
        }
        else if (c)
        {
          decorationSize = c.bounds.size;
        }
        else
        {
          var errMsg = "Decoration does not have an indicator to tell its size.";
          if (DebugConfigurations.DebugEnabled)
          {
            throw new System.Exception(errMsg);
          }
          else
          {
            Debug.LogWarning(errMsg);
          }
        }


        if (decorationSize.x >= biggestDecorationSize.y && decorationSize.y >= biggestDecorationSize.y)
        {
          biggestDecorationSize = decorationSize;
        }
      }

      Vector2 positioningSize = biggestDecorationSize * 1.5f; // Expand decoration size for random positoning.

      int xCount = (int)(size.x / positioningSize.x),
        yCount = (int)(size.y / positioningSize.y),
        totalCount = xCount * yCount;

      int min = minimumDecorations < 0 ? 0 : minimumDecorations,
        max = maximumDecorations <= totalCount ? maximumDecorations : totalCount;

      int quantity = Random.Range(min, max);

      List<bool> hasDecoration = new();
      for (int i = 0; i < totalCount; i++)
      {
        hasDecoration.Add(i < quantity);
      }

      hasDecoration = hasDecoration
        .Select(h => new
        {
          hasDecoration = h,
          randomKey = Random.Range(0, int.MaxValue),
        })
        .OrderBy(o => o.randomKey)
        .Select(o => o.hasDecoration)
        .ToList();

      int count = 0;
      for (int i = 0; i < xCount; i++)
      {
        for (int j = 0; j < yCount; j++)
        {
          if (hasDecoration[count++])
          {
            var plannedPosition = new Vector2(
              transform.position.x - size.x / 2 + positioningSize.x * i,
              transform.position.y - size.y / 2 + positioningSize.y * j);
            var randomizedPosition = new Vector2(
              biggestDecorationSize.x * Random.Range(-0.5f, 0.5f),
              biggestDecorationSize.y * Random.Range(-0.5f, 0.5f));
            plannedPosition += randomizedPosition;

            if (plannedPosition.sqrMagnitude >= 2)
            {
              GameObject decoration = GetRandomDecoration();
              decoration.transform.SetParent(transform);
              decoration.transform.position = (Vector3)plannedPosition;
              for (int k = 0; k < decoration.transform.childCount; k++)
              {
                var child = decoration.transform.GetChild(k);
                if (child.gameObject.layer == 15) // AKA CollidableEnvironment
                {
                  var pos = child.transform.position;
                  child.transform.position = new Vector3(pos.x, pos.y, 0);
                }
              }
            }
          }
        }
      }
    }
  }
}