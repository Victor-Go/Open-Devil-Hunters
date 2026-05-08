using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Src.Steam.Achievements
{
  public enum Achievements
  {
    FirstBlood,
    Rampage,
    Legendary,
    DevilHunter,
    WitchKiller,
    SpecterKiller,
    LasermonKiller,
    ThunderDemonKiller,
    CannonDragonKiller,
    StellarbotKiller,
    Ace,
    Brotherhood,
    NewHero,
    Gatling,
    Wustenfuchs,
    Buddha,
    DanteAlighitri,
    DungeonMaster,
    Tarzan,
  }

  public static class AchievementsManager
  {
    private static readonly Dictionary<Achievements, string> AchievementSteamApiNameMappings = new()
    {
      { Achievements.FirstBlood, "FIRST_BLOOD" },
      { Achievements.Rampage, "RAMPAGE" },
      { Achievements.Legendary, "LEGENDARY" },
      { Achievements.DevilHunter, "DEVIL_HUNTER" },
      { Achievements.WitchKiller, "WITCH_KILLER" },
      { Achievements.SpecterKiller, "SPECTER_KILLER" },
      { Achievements.LasermonKiller, "LASERMON_KILLER" },
      { Achievements.ThunderDemonKiller, "THUNDER_DEMON_KILLER" },
      { Achievements.CannonDragonKiller, "CANNON_DRAGON_KILLER" },
      { Achievements.StellarbotKiller, "STELLARBOT_KILLER" },
      { Achievements.Ace, "ACE" },
      { Achievements.Brotherhood, "BROTHERHOOD" },
      { Achievements.NewHero, "NEW_HERO" },
      { Achievements.Gatling, "GATLING" },
      { Achievements.Wustenfuchs, "WUSTENFUCHS" },
      { Achievements.Buddha, "BUDDHA" },
      { Achievements.DanteAlighitri, "DANTE_ALIGHITRI" },
      { Achievements.DungeonMaster, "DUNGEON_MASTER" },
      { Achievements.Tarzan, "TARZAN" },
    };

    public static bool GetAchievementIsUnlocked(Achievements achievement)
    {
      try
      {
        var ach = new Steamworks.Data.Achievement(AchievementSteamApiNameMappings[achievement]);
        Debug.Log($"Achievement {nameof(achievement)} status: {ach.State.ToString()}");
        return ach.State;
      }
      catch (Exception e)
      {
        Debug.LogWarning($"Failed to get achievement {nameof(achievement)}: {e}");
        return false;
      }
    }

    public static void UnlockAchievement(Achievements achievement)
    {
      try
      {
        var ach = new Steamworks.Data.Achievement(AchievementSteamApiNameMappings[achievement]);
        ach.Trigger();
        Debug.Log($"Achievement {nameof(achievement)} is unlocked");
      }
      catch (Exception e)
      {
        Debug.LogWarning($"Failed to unlock achievement {nameof(achievement)}: {e}");
      }
    }

    public static void ClearAchievements(Achievements achievement)
    {
      try
      {
        var ach = new Steamworks.Data.Achievement(AchievementSteamApiNameMappings[achievement]);
        ach.Clear();
        Debug.Log($"Achievement {nameof(achievement)} is cleared");
      }
      catch (Exception e)
      {
        Debug.LogWarning($"Failed to clear achievement {nameof(achievement)}: {e}");
        Debug.LogWarning(e);
      }
    }
  }
}