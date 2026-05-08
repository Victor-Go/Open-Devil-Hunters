using System;
using System.Collections.Generic;

namespace Code.Scripts.Src.Configurations
{
  public struct BackgroundDecorationConfig
  {
    public string PrefabName;
    public int Coefficient;
  }

  [Serializable]
  public enum MapNames
  {
    FOREST,
    DESERT,
    DUNGEON,
    GRAVEYARD,
    HELL,
  }

  public class MapConfiguration
  {
    public MapNames MapName { get; set; }
    public string LittleBossAvatarName { get; set; }
    public string BigBossAvatarName { get; set; }
    public string MapPrefabName { get; set; }
    public List<string> MapBackgroundNames { get; set; }
    public List<BackgroundDecorationConfig> backgroundDecorationConfigs { get; set; }
    public int MaximumDecorations { get; set; }
    public int MinimumDecorations { get; set; }
  }

  public static class MapConfigurations
  {
    public static List<MapConfiguration> Configurations { get; } = new()
    {
      new()
      {
        MapName = MapNames.FOREST,
        MapPrefabName = "Map/Forest/Forest",
        LittleBossAvatarName = "EnemyAvatar/rock-giant",
        BigBossAvatarName = "EnemyAvatar/ice-tiger",
        MapBackgroundNames = new List<string>()
        {
          "Map/Forest/Terrains/forest-terrain_0",
        },
        backgroundDecorationConfigs = new List<BackgroundDecorationConfig>()
        {
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Environment/PickableGem",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 15,
            PrefabName = "Environment/Medicine",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration0",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration1",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration2",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration3",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration4",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration5",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration6",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration7",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration8",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration9",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration10",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration11",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration12",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration13",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecoration14",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecorationAnimation0",
          },
          // new BackgroundDecorationConfig()
          // {
          //     Coefficient = 10,
          //     PrefabName = "Map/Forest/Decorations/ForestDecorationAnimation1",
          // },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecorationAnimation2",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecorationAnimation3",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecorationAnimation4",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/ForestDecorationAnimation5",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Forest/Decorations/TreeMonster",
          },
        },
        MinimumDecorations = 6,
        MaximumDecorations = 12,
      },
      new()
      {
        MapName = MapNames.DESERT,
        MapPrefabName = "Map/Desert/Desert",
        LittleBossAvatarName = "EnemyAvatar/cell-monster",
        BigBossAvatarName = "EnemyAvatar/stellarbot",
        MapBackgroundNames = new List<string>()
        {
          "Map/Desert/Terrains/desert-terrain_0",
          "Map/Desert/Terrains/desert-terrain_1",
          "Map/Desert/Terrains/desert-terrain_2",
          "Map/Desert/Terrains/desert-terrain_3",
        },
        backgroundDecorationConfigs = new List<BackgroundDecorationConfig>()
        {
          new BackgroundDecorationConfig()
          {
            Coefficient = 9,
            PrefabName = "Environment/PickableGem",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 15,
            PrefabName = "Environment/Medicine",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 2,
            PrefabName = "Map/Desert/Decorations/DesertDecoration0",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration1",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration2",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration3",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration4",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration5",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration6",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration7",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration8",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration9",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration10",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration11",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration12",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration13",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration14",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecoration15",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecorationAnimation0",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Desert/Decorations/DesertDecorationAnimation1",
          },
        },
        MinimumDecorations = 8,
        MaximumDecorations = 12,
      },
      new()
      {
        MapName = MapNames.DUNGEON,
        MapPrefabName = "Map/Dungeon/Dungeon",
        LittleBossAvatarName = "EnemyAvatar/thunder-demon",
        BigBossAvatarName = "EnemyAvatar/werewolf",
        MapBackgroundNames = new List<string>()
        {
          "Map/Dungeon/Terrains/dungeon-terrain_0",
        },
        backgroundDecorationConfigs = new List<BackgroundDecorationConfig>()
        {
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Environment/PickableGem",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 15,
            PrefabName = "Environment/Medicine",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 2,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration0",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration1",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration2",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration3",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration4",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration5",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration6",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration7",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration8",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration9",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration10",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration11",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration12",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration13",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration14",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration15",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration16",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration17",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecoration18",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecorationAnimation0",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecorationAnimation1",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Dungeon/Decorations/DungeonDecorationAnimation2",
          },
        },
        MinimumDecorations = 6,
        MaximumDecorations = 12,
      },
      new()
      {
        MapName = MapNames.GRAVEYARD,
        MapPrefabName = "Map/Graveyard/Graveyard",
        LittleBossAvatarName = "EnemyAvatar/witch",
        BigBossAvatarName = "EnemyAvatar/specter",
        MapBackgroundNames = new List<string>()
        {
          "Map/Graveyard/Terrains/graveyard-terrain_0",
        },
        backgroundDecorationConfigs = new List<BackgroundDecorationConfig>()
        {
          new BackgroundDecorationConfig()
          {
            Coefficient = 9,
            PrefabName = "Environment/PickableGem",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 15,
            PrefabName = "Environment/Medicine",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 2,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration0",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration1",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration2",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration3",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration4",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration5",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration6",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration7",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration8",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration9",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration10",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration11",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration12",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecoration13",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecorationAnimation0",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecorationAnimation2",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecorationAnimation3",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecorationAnimation4",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Graveyard/Decorations/GraveyardDecorationAnimation5",
          },
        },
        MinimumDecorations = 6,
        MaximumDecorations = 10,
      },
      new()
      {
        MapName = MapNames.HELL,
        MapPrefabName = "Map/Hell/Hell",
        LittleBossAvatarName = "EnemyAvatar/cannon-dragon",
        BigBossAvatarName = "EnemyAvatar/devil",
        MapBackgroundNames = new List<string>()
        {
          "Map/Hell/Terrains/hell-terrain_0",
        },
        backgroundDecorationConfigs = new List<BackgroundDecorationConfig>()
        {
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Environment/PickableGem",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 20,
            PrefabName = "Environment/Medicine",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 2,
            PrefabName = "Map/Hell/Decorations/HellDecoration0",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Hell/Decorations/HellDecoration1",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Hell/Decorations/HellDecoration2",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Hell/Decorations/HellDecoration3",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Hell/Decorations/HellDecoration4",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Hell/Decorations/HellDecoration5",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Hell/Decorations/HellDecoration6",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Hell/Decorations/HellDecorationAnimation0",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Hell/Decorations/HellDecorationAnimation1",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Hell/Decorations/HellDecorationAnimation2",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Hell/Decorations/HellDecorationAnimation3",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Hell/Decorations/HellDecorationAnimation4",
          },
          new BackgroundDecorationConfig()
          {
            Coefficient = 10,
            PrefabName = "Map/Hell/Decorations/HellDecorationAnimation5",
          }
        },
        MinimumDecorations = 6,
        MaximumDecorations = 10,
      },
    };
  }
}