using System;
using System.Collections.Generic;
using Code.Scripts.Src.Store.Level;
using UnityEngine;

namespace Code.Scripts.Src.Configurations
{
  public enum Version
  {
    DEMO,
    OFFICIAL,
  }

  [Serializable]
  public class FootstepClipNames
  {
    // Note: Dictionaries aren't directly serializable in Unity without custom wrappers, 
    // but typically we'd use a List of key-value pairs or Odin Serializer. 
    // For this template, we expose it as a standard class.
    public Dictionary<MapNames, string[]> Names = new()
    {
      { MapNames.FOREST, new[] { "Audio/Sound/Environment/Footstep/forest-footstep_0" } },
      { MapNames.DESERT, new[] { "Audio/Sound/Environment/Footstep/desert-footstep_0" } },
      { MapNames.GRAVEYARD, new[] { "Audio/Sound/Environment/Footstep/outdoor-footstep_0" } },
      { MapNames.DUNGEON, new[] { "Audio/Sound/Environment/Footstep/outdoor-footstep_0" } },
      { MapNames.HELL, new[] { "Audio/Sound/Environment/Footstep/outdoor-footstep_0" } }
    };
  }

  [Serializable]
  public struct AdditionalEffectLimits
  {
    public float MaximumPossibility;
    public float MinimumPossibility;
    public float MaximumHurtPercentagePerSecond;
    public float MinimumHurtPercentagePerSecond;
    public float MaximumLastForSeconds;
    public float MinimumLastForSeconds;
    public float MaximumStunningSeconds;
    public float MinimumStunningSeconds;
  }

  [Serializable]
  public struct PlayerInitData
  {
    public float ImmortalSeconds;
  }

  [CreateAssetMenu(fileName = "GeneralConfigurations", menuName = "DevilHunters/Configurations/General")]
  public class GeneralConfigurationAsset : ScriptableObject
  {
    [Header("Game Version")]
    public bool DebugUpgrade = false;
    public uint SteamId = 2306050;
    public Version Version = Version.OFFICIAL;
    public int CurrentSaveVersion = 1;

    [Header("Demo Limitations")]
    public List<PlayerNames> HeroesAvailableInDemoVersion = new()
    {
      PlayerNames.ARCHANGEL,
      PlayerNames.CAPTAIN_G,
      PlayerNames.WUKONG,
    };
    public List<MapNames> MapsAvailableInDemoVersion = new() { MapNames.FOREST };
    public List<PlayModes> PlayModesAvailableInDemoVersion = new() { PlayModes.STANDARD };
    public List<RuneTypes> RunesAvailableInDemoVersion = new()
    {
      RuneTypes.PICK_UP_DISTANCE, RuneTypes.INCREASE_HONOR, RuneTypes.EXP_BONUS,
      RuneTypes.TIME_STOP, RuneTypes.PUSH_AWAY, RuneTypes.INSTANT_RELOAD,
      RuneTypes.CLONED_PROJECTILE, RuneTypes.METEORITE_STRIKE, RuneTypes.THUNDER_STRIKE,
      RuneTypes.HAIL_STRIKE,
    };

    [Header("Enemy Spawning & Scaling")]
    public int MaxEnemyOnScreen = 50;
    public int InitialQuantityMaintainEnemy = 10;
    public int MaxQuantityMaintainEnemy = 40;
    public float MaximumPerSecondQuantity = 1.5f;
    public float InitialPerSecondQuantity = 0.25f;
    public float SpawnDistance = 10f;
    public int InfiniteEnemyGenerationLoopMinutes = 10;
    
    [Header("Boss Fight")]
    public int MinEnemiesInBossFight = 15;
    public int MaxEnemiesInBossFight = 30;
    public int MinEnemiesPerSecondInBossFight = 1;
    public int MaxEnemiesPerSecondInBossFight = 3;
    public float MultiBossDifficultyDecrement = 4f;

    [Header("Player & Match Limits")]
    public int MaximumPlayers = 2;
    public float MaximumPlayersHorizontalDistance = 12f;
    public PlayModes DefaultMode = PlayModes.STANDARD;
    public int ExpPointDuration = 45;
    public int CasualModeDuration = 10;
    public int NormalModeDuration = 15;

    [Header("Combat & Mechanics")]
    public float DefendTypeDecrement = 0.2f;
    public float CriticalHurtTypeIncrement = 0.2f;
    public float UpdateAimingTargetFrequency = 2.5f;
    public float JoystickAimingSensibility = 20f;
    public int MaximumHp = 1000000;
    public int MaximumAttackPerRound = 100;
    public float MaximumPickRadius = 5f;
    public float MaximumPlayerAttackingMovingSpeed = 5f;
    public float MinimumCoolingTime = 0.05f;
    public float MinimumReloadTime = 0.1f;
    public int MaximumSurroundSkills = 20;
    public float ImmortalSecondsAfterDamage = 1f;
    public int LevelDifficultyCount = 10;
    public Vector2 DefaultAutoAimingPosition = Vector2.right;
    public float InAttackingStateDuration = 0.5f;

    [Header("Physics & Motion")]
    public float EnemyMaxSpeed = 1.75f;
    public float PlayerMaxSpeed = 3f;
    public float BulletMaximumSpeed = 10f;
    public float AttackNotBeingPressedToReloadThreshold = 1f;
    public float AnimationDirectionSettingThreshold = 0.005f;

    [Header("Data Sets")]
    public List<PlayerNames> DefaultUnlockedPlayers = new()
    {
      PlayerNames.ARCHANGEL,
      PlayerNames.CAPTAIN_G,
      PlayerNames.WUKONG,
    };

    public PlayerInitData PlayerInitSettings = new PlayerInitData { ImmortalSeconds = 1f };
    public FootstepClipNames FootstepClipNames = new FootstepClipNames();

    public AdditionalEffectLimits AdditionalEffectLimits = new AdditionalEffectLimits
    {
      MaximumHurtPercentagePerSecond = 0.1f,
      MinimumHurtPercentagePerSecond = 0.01f,
      MaximumLastForSeconds = 5f,
      MinimumLastForSeconds = 1f,
      MaximumPossibility = 0.5f,
      MinimumPossibility = 0.01f,
      MaximumStunningSeconds = 7.5f,
      MinimumStunningSeconds = 1f,
    };

    // Note: The static mathematical delegates (GetEnemyDifficultyToTimePoint, GetStrengthByDifficulty, etc.) 
    // should ideally be moved to a dedicated MathUtility static class, as logic delegates cannot be serialized via Unity's Inspector.
  }

  /// <summary>
  /// A proxy class that allows the rest of the codebase to continue referencing 
  /// GeneralConfigurations without throwing compile errors during the migration.
  /// This bridges the static dependencies to the newly created ScriptableObject.
  /// </summary>
  public static class GeneralConfigurations
  {
    private static GeneralConfigurationAsset _instance;
    public static GeneralConfigurationAsset Instance
    {
      get
      {
        if (_instance == null)
        {
          // Attempt to load from Resources. Ensure a 'GeneralConfigurations' asset exists in a Resources folder!
          _instance = Resources.Load<GeneralConfigurationAsset>("GeneralConfigurations");
          
          if (_instance == null)
          {
            Debug.LogWarning("GeneralConfigurations Asset not found in Resources! Creating a default runtime instance.");
            _instance = ScriptableObject.CreateInstance<GeneralConfigurationAsset>();
          }
        }
        return _instance;
      }
    }

    public static bool DebugUpgrade => Instance.DebugUpgrade;
    public static uint SteamId => Instance.SteamId;
    public static Version Version => Instance.Version;
    public static int CurrentSaveVersion => Instance.CurrentSaveVersion;
    public static List<PlayerNames> HeroesAvailableInDemoVersion => Instance.HeroesAvailableInDemoVersion;
    public static List<MapNames> MapsAvailableInDemoVersion => Instance.MapsAvailableInDemoVersion;
    public static List<PlayModes> PlayModesAvailableInDemoVersion => Instance.PlayModesAvailableInDemoVersion;
    public static List<RuneTypes> RunesAvailableInDemoVersion => Instance.RunesAvailableInDemoVersion;

    public static int MaxEnemyOnScreen => Instance.MaxEnemyOnScreen;
    public static int InfiniteEnemyGenerationLoopMinutes => Instance.CasualModeDuration; // Using CasualModeDuration as originally mapped
    public static float MaximumPerSecondQuantity => Instance.MaximumPerSecondQuantity;
    public static float InitialPerSecondQuantity => Instance.InitialPerSecondQuantity;
    public const float pPerSecond = 1.5f;

    public static int CasualModeDuration => Instance.CasualModeDuration;
    public static int NormalModeDuration => Instance.NormalModeDuration;
    public static int ExpPointDuration => Instance.ExpPointDuration;
    
    public static float SpawnDistance => Instance.SpawnDistance;
    public static float MultiBossDifficultyDecrement => Instance.MultiBossDifficultyDecrement;

    public static int MinEnemiesInBossFight => Instance.MinEnemiesInBossFight;
    public static int MaxEnemiesInBossFight => Instance.MaxEnemiesInBossFight;
    public static int MinEnemiesPerSecondInBossFight => Instance.MinEnemiesPerSecondInBossFight;
    public static int MaxEnemiesPerSecondInBossFight => Instance.MaxEnemiesPerSecondInBossFight;

    public static int MaximumPlayers => Instance.MaximumPlayers;
    public static List<PlayerNames> DefaultUnlockedPlayers => Instance.DefaultUnlockedPlayers;
    public static float MaximumPlayersHorizontalDistance => Instance.MaximumPlayersHorizontalDistance;
    public static PlayerInitData PlayerInitData => Instance.PlayerInitSettings;
    public static FootstepClipNames FootstepClipNames => Instance.FootstepClipNames;
    public static PlayModes DefaultMode => Instance.DefaultMode;

    public static float DefendTypeDecrement => Instance.DefendTypeDecrement;
    public static float CriticalHurtTypeIncrement => Instance.CriticalHurtTypeIncrement;
    public static float UpdateAimingTargetFrequency => Instance.UpdateAimingTargetFrequency;
    public static float JoystickAimingSensibility => Instance.JoystickAimingSensibility;
    public static int MaximumHp => Instance.MaximumHp;
    public static int MaximumAttackPerRound => Instance.MaximumAttackPerRound;
    public static float MaximumPickRadius => Instance.MaximumPickRadius;
    public static float MaximumPlayerAttackingMovingSpeed => Instance.MaximumPlayerAttackingMovingSpeed;
    public static float MinimumCoolingTime => Instance.MinimumCoolingTime;
    public static float MinimumReloadTime => Instance.MinimumReloadTime;
    public static int MaximumSurroundSkills => Instance.MaximumSurroundSkills;
    public static float ImmortalSecondsAfterDamage => Instance.ImmortalSecondsAfterDamage;
    public static int LevelDifficultyCount => Instance.LevelDifficultyCount;
    public static Vector2 DefaultAutoAimingPosition => Instance.DefaultAutoAimingPosition;
    public static float InAttackingStateDuration => Instance.InAttackingStateDuration;

    public static float EnemyMaxSpeed => Instance.EnemyMaxSpeed;
    public static float PlayerMaxSpeed => Instance.PlayerMaxSpeed;
    public static float BulletMaximumSpeed => Instance.BulletMaximumSpeed;
    public static float AttackNotBeingPressedToReloadThreshold => Instance.AttackNotBeingPressedToReloadThreshold;
    public static float AnimationDirectionSettingThreshold => Instance.AnimationDirectionSettingThreshold;

    public static AdditionalEffectLimits AdditionalEffectLimits => Instance.AdditionalEffectLimits;

    // --- Mathematical Delegates (Untouched to prevent massive logic breaks) ---
    private const float difficultyIncrementBySeconds = 30f;
    private const float levelDifficultyFactor = 2f;

    public static readonly Func<int, float, float> GetEnemyDifficultyToTimePoint = (timePointInSeconds, difficulty) =>
      Mathf.Clamp(timePointInSeconds / difficultyIncrementBySeconds + difficulty * levelDifficultyFactor, 0, 50);

    public static readonly Func<int, float> GetMinQuantityGenerationPerSecond = seconds =>
      seconds switch { <= 60 => 0, <= 180 => 0.175f, <= 300 => 0.25f, _ => 0.325f };

    public static readonly Func<float, float, bool, float> GetStrengthByDifficulty = (difficulty, a, harderFirst) =>
    {
      a = Mathf.Clamp(a, 0.75f, 1.5f);
      return 1.5f * Mathf.Sin(a * difficulty * (harderFirst ? 1 : -1)) / a + difficulty;
    };

    public static readonly Func<float, float, float> GetCollisionHitPointOverStrength =
      (strength, baseHit) => baseHit * (1 + strength * 0.1f);

    public static readonly Func<int, int> GetInfiniteLoopDifficultyIncrementToLoopCount =
      loopCount => InfiniteEnemyGenerationLoopMinutes * loopCount;

    public static readonly Func<int, int> GetEnemyGenerationMaintainQuantity = seconds => Mathf.Min(
      Mathf.RoundToInt(
        (Instance.MaxQuantityMaintainEnemy - Instance.InitialQuantityMaintainEnemy) *
        Mathf.Pow((float)seconds / (NormalModeDuration * 60), 0.7f) + Instance.InitialQuantityMaintainEnemy
      ), MaxEnemyOnScreen);

    public static readonly Func<int, float> GetEnemyGenerationPerSecond = seconds => Mathf.Clamp(
      (MaximumPerSecondQuantity - InitialPerSecondQuantity) *
      Mathf.Pow((float)seconds / NormalModeDuration, pPerSecond) +
      Instance.InitialQuantityMaintainEnemy,
      InitialPerSecondQuantity, MaximumPerSecondQuantity);

    public static readonly Func<PlayModes, int> GetMinutesToBoss = playMode => playMode == PlayModes.CASUAL
      ? CasualModeDuration : NormalModeDuration;

    public static Func<int, int, int, int, int> GetFinalScore { get; } =
      (levelDifficulty, duration, finalLevel, killed) =>
        (1 + levelDifficulty / 3) * (duration * 5 + finalLevel * 100 + killed * 2);

    public static Func<int, int> GetFinalHonor { get; } = finalScore =>
      Mathf.CeilToInt(finalScore * 0.33f);
  }
}