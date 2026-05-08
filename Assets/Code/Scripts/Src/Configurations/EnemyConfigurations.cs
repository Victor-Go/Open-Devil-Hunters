using System.Collections.Generic;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using JetBrains.Annotations;

namespace Code.Scripts.Src.Configurations
{
  [System.Serializable]
  public enum EnemyNames
  {
    // Normal enemies
    CUNNING_MOUSE,
    FIRE_LAMPREY,
    FLEA,
    LIGHTNING_DRAGON,
    LITTLE_DEMON,
    MUMMY,
    SCORPION,
    SIREN,
    SKELETON,
    WASP,

    // Forest
    ROCK_GIANT,
    ICE_TIGER,

    // Desert
    CELL_MONSTER,
    STELLARBOT,

    // Dungeon
    THUNDER_DEMON,
    WEREWOLF,

    // Graveyard
    WITCH,
    SPECTER,

    // Hell
    CANNON_DRAGON,
    DEVIL,

    // Summon enemies
    WOLF,
    WORM,

    // Abandoned
    LASERMON,
  }

  public struct EnemyUpgrades
  {
    public float IncreaseHpCoefficient { get; set; }
    public float IncreaseSpeedCoefficient { get; set; }
  }

  public class EnemyConfiguration
  {
    public EnemyNames EnemyId { get; set; }
    public string EnemyPrefabName { get; set; }
    public bool IsBoss { get; set; }
    public float Speed { get; set; }
    public float HealthPoint { get; set; }

    public float
      BaseNormalHitPoint
    {
      get;
      set;
    } // Consider GetCollisionHitPointOverStrength function that controls collision hit point multiple

    public float Fluctuation { get; set; }
    [CanBeNull] public List<HurtTypes> DefendTypes { get; set; }
    [CanBeNull] public List<HurtTypes> CriticalHurtTypes { get; set; }
    [CanBeNull] public List<BasicAdditionalEffectTypes> AdditionalEffectImmunity { get; set; }
    public bool PoisonImmunity { get; set; }
    public List<string> DeadSounds { get; set; } = new();
    public List<string> HurtSounds { get; set; } = new();
    public int QuantityFactor { get; set; } = 1; // It's int because it's limited by EnemyFactory

    public EnemyUpgrades EnemyStrengthUpgrade { get; set; }

    public EnemyUpgrades Enemy2PlayersUpgrade { get; set; }
    public float EnemyExpCoefficient { get; set; } = 1;
  }

  public static class EnemyConfigurations
  {
    // For bosses and its invoke monsters, strength is related to boss's strength
    public static Dictionary<EnemyNames, EnemyConfiguration> Normal { get; } = new()
    {
      {
        EnemyNames.CUNNING_MOUSE,
        new()
        {
          EnemyId = EnemyNames.CUNNING_MOUSE,
          EnemyPrefabName = "Enemy/Normal/CunningMouse",
          EnemyExpCoefficient = 6.5f,
          Speed = 0.65f,
          HealthPoint = 26,
          BaseNormalHitPoint = 35,
          Fluctuation = 0.2f,
          DefendTypes = new() { HurtTypes.PHYSICAL },
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.FIRE_LAMPREY,
        new()
        {
          EnemyId = EnemyNames.FIRE_LAMPREY,
          EnemyPrefabName = "Enemy/Normal/FireLamprey",
          EnemyExpCoefficient = 2.5f,
          Speed = 0.85f,
          HealthPoint = 17,
          BaseNormalHitPoint = 20,
          Fluctuation = 0.35f,
          DefendTypes = new() { HurtTypes.MAGIC_FIRE },
          CriticalHurtTypes = new() { HurtTypes.MAGIC_ICE },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.BURN, },
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.5f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          },
        }
      },
      {
        EnemyNames.FLEA,
        new()
        {
          EnemyId = EnemyNames.FLEA,
          EnemyPrefabName = "Enemy/Normal/Flea",
          EnemyExpCoefficient = 1.5f,
          Speed = 0.8f,
          HealthPoint = 13,
          BaseNormalHitPoint = 18,
          Fluctuation = 0.25f,
          DefendTypes = new(),
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.LIGHTNING_DRAGON,
        new()
        {
          EnemyId = EnemyNames.LIGHTNING_DRAGON,
          EnemyPrefabName = "Enemy/Normal/LightningDragon",
          EnemyExpCoefficient = 3f,
          Speed = 0.7f,
          HealthPoint = 20,
          BaseNormalHitPoint = 25,
          Fluctuation = 0.3f,
          DefendTypes = new() { HurtTypes.MAGIC_THUNDER },
          CriticalHurtTypes = new() { HurtTypes.MAGIC_FIRE },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.STUN },
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.5f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.LITTLE_DEMON,
        new()
        {
          EnemyId = EnemyNames.LITTLE_DEMON,
          EnemyPrefabName = "Enemy/Normal/LittleDemon",
          EnemyExpCoefficient = 2f,
          Speed = 0.95f,
          HealthPoint = 10,
          BaseNormalHitPoint = 25,
          Fluctuation = 0.2f,
          DefendTypes = new(),
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.4f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.MUMMY,
        new()
        {
          EnemyId = EnemyNames.MUMMY,
          EnemyPrefabName = "Enemy/Normal/Mummy",
          EnemyExpCoefficient = 1.5f,
          Speed = 0.7f,
          HealthPoint = 14,
          BaseNormalHitPoint = 20,
          Fluctuation = 0.2f,
          DefendTypes = new() { HurtTypes.MAGIC_ICE },
          CriticalHurtTypes = new() { HurtTypes.MAGIC_THUNDER },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.FREEZE },
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.45f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.SCORPION,
        new()
        {
          EnemyId = EnemyNames.SCORPION,
          EnemyPrefabName = "Enemy/Normal/Scorpion",
          EnemyExpCoefficient = 4f,
          Speed = 1f,
          HealthPoint = 22,
          BaseNormalHitPoint = 20,
          Fluctuation = 0.85f,
          DefendTypes = new() { HurtTypes.POISON },
          PoisonImmunity = true,
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.6f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.SIREN,
        new()
        {
          EnemyId = EnemyNames.SIREN,
          EnemyPrefabName = "Enemy/Normal/Siren",
          EnemyExpCoefficient = 1.5f,
          Speed = 0.85f,
          HealthPoint = 17,
          BaseNormalHitPoint = 10,
          Fluctuation = 0.25f,
          DefendTypes = new(),
          AdditionalEffectImmunity = new()
            { BasicAdditionalEffectTypes.BURN, BasicAdditionalEffectTypes.FREEZE, BasicAdditionalEffectTypes.STUN },
          PoisonImmunity = true,
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.6f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/female-enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.SKELETON,
        new()
        {
          QuantityFactor = 3,
          EnemyId = EnemyNames.SKELETON,
          EnemyPrefabName = "Enemy/Normal/Skeleton",
          EnemyExpCoefficient = 0.5f,
          Speed = 0.75f,
          HealthPoint = 9,
          BaseNormalHitPoint = 121,
          Fluctuation = 0.2f,
          DefendTypes = new(),
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.WASP,
        new()
        {
          EnemyId = EnemyNames.WASP,
          EnemyPrefabName = "Enemy/Normal/Wasp",
          EnemyExpCoefficient = 1.5f,
          Speed = 0.95f,
          HealthPoint = 13,
          BaseNormalHitPoint = 15,
          Fluctuation = 0.2f,
          DefendTypes = new(),
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.5f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
    };

    public static Dictionary<EnemyNames, EnemyConfiguration> Boss { get; } = new()
    {
      {
        EnemyNames.WITCH,
        new()
        {
          EnemyId = EnemyNames.WITCH,
          EnemyPrefabName = "Enemy/LittleBoss/Witch/Witch",
          EnemyExpCoefficient = 100f,
          IsBoss = true,
          Speed = 1.25f,
          HealthPoint = 2805,
          BaseNormalHitPoint = 37,
          Fluctuation = 0.15f,
          DefendTypes = new() { HurtTypes.MAGIC_ICE, HurtTypes.POISON },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.FREEZE },
          CriticalHurtTypes = new() { HurtTypes.MAGIC_THUNDER },
          PoisonImmunity = true,
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },

          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/witch-died_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.SPECTER,
        new()
        {
          EnemyId = EnemyNames.SPECTER,
          EnemyExpCoefficient = 100f,
          EnemyPrefabName = "Enemy/LittleBoss/Specter/Specter",
          IsBoss = true,
          Speed = 1,
          HealthPoint = 4650,
          BaseNormalHitPoint = 35,
          Fluctuation = 0.2f,
          DefendTypes = new() { HurtTypes.PHYSICAL },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.STUN },
          PoisonImmunity = true,
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.LASERMON,
        new()
        {
          EnemyId = EnemyNames.LASERMON,
          EnemyExpCoefficient = 100f,
          EnemyPrefabName = "Enemy/LittleBoss/Lasermon/Lasermon",
          IsBoss = true,
          Speed = 1f,
          HealthPoint = 12500,
          BaseNormalHitPoint = 30,
          Fluctuation = 0.2f,
          DefendTypes = new() { HurtTypes.PHYSICAL },
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.THUNDER_DEMON,
        new()
        {
          EnemyId = EnemyNames.THUNDER_DEMON,
          EnemyExpCoefficient = 100f,
          EnemyPrefabName = "Enemy/LittleBoss/ThunderDemon/ThunderDemon",
          IsBoss = true,
          Speed = 1.1f,
          HealthPoint = 3570,
          BaseNormalHitPoint = 30,
          Fluctuation = 0.2f,
          DefendTypes = new() { HurtTypes.MAGIC_THUNDER },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.STUN },
          CriticalHurtTypes = new() { HurtTypes.MAGIC_FIRE },
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.CANNON_DRAGON,
        new()
        {
          EnemyId = EnemyNames.CANNON_DRAGON,
          EnemyExpCoefficient = 100f,
          EnemyPrefabName = "Enemy/LittleBoss/CannonDragon/CannonDragon",
          IsBoss = true,
          Speed = 1f,
          HealthPoint = 3570,
          BaseNormalHitPoint = 30,
          Fluctuation = 0.2f,
          DefendTypes = new() { HurtTypes.MAGIC_FIRE, HurtTypes.MAGIC_THUNDER },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.BURN, BasicAdditionalEffectTypes.STUN },
          CriticalHurtTypes = new() { HurtTypes.MAGIC_ICE },
          PoisonImmunity = true,
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.ROCK_GIANT,
        new()
        {
          EnemyId = EnemyNames.ROCK_GIANT,
          EnemyExpCoefficient = 100f,
          EnemyPrefabName = "Enemy/Boss/RockGiant/RockGiant",
          IsBoss = true,
          Speed = 0.8f,
          HealthPoint = 3825,
          BaseNormalHitPoint = 30,
          Fluctuation = 0.3f,
          DefendTypes = new() { HurtTypes.PHYSICAL },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.FREEZE },
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/boss-dead_0",
            "Audio/Sound/Enemy/Dead/boss-dead_1",
            "Audio/Sound/Enemy/Dead/boss-dead_2",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.ICE_TIGER,
        new()
        {
          EnemyId = EnemyNames.ICE_TIGER,
          EnemyExpCoefficient = 100f,
          EnemyPrefabName = "Enemy/Boss/IceTiger/IceTiger",
          IsBoss = true,
          Speed = 1.15f,
          HealthPoint = 4150,
          BaseNormalHitPoint = 6,
          Fluctuation = 0.3f,
          DefendTypes = new() { HurtTypes.MAGIC_ICE },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.FREEZE },
          CriticalHurtTypes = new() { HurtTypes.MAGIC_THUNDER },
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/boss-dead_1",
            "Audio/Sound/Enemy/Dead/boss-dead_2",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.WEREWOLF,
        new()
        {
          EnemyId = EnemyNames.WEREWOLF,
          EnemyExpCoefficient = 100f,
          EnemyPrefabName = "Enemy/Boss/Werewolf/Werewolf",
          IsBoss = true,
          Speed = 1.1f,
          HealthPoint = 4650,
          BaseNormalHitPoint = 25,
          Fluctuation = 0.3f,
          DefendTypes = new() { HurtTypes.PHYSICAL },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.FREEZE },
          PoisonImmunity = true,
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/boss-dead_1",
            "Audio/Sound/Enemy/Dead/boss-dead_2",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.CELL_MONSTER,
        new()
        {
          EnemyId = EnemyNames.CELL_MONSTER,
          EnemyExpCoefficient = 100f,
          EnemyPrefabName = "Enemy/Boss/CellMonster/CellMonster",
          IsBoss = true,
          Speed = 1f,
          HealthPoint = 3570,
          BaseNormalHitPoint = 25,
          Fluctuation = 0.2f,
          DefendTypes = new() { HurtTypes.MAGIC_THUNDER },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.STUN },
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/boss-dead_1",
            "Audio/Sound/Enemy/Dead/boss-dead_2",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          },
        }
      },
      {
        EnemyNames.STELLARBOT,
        new()
        {
          EnemyId = EnemyNames.STELLARBOT,
          EnemyExpCoefficient = 100f,
          EnemyPrefabName = "Enemy/Boss/Stellarbot/Stellarbot",
          IsBoss = true,
          Speed = 1.25f,
          HealthPoint = 4450,
          BaseNormalHitPoint = 30,
          Fluctuation = 0.3f,
          DefendTypes = new() { HurtTypes.MAGIC_ICE },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.FREEZE },
          PoisonImmunity = true,
          CriticalHurtTypes = new() { HurtTypes.PHYSICAL },
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/boss-dead_1",
            "Audio/Sound/Enemy/Dead/boss-dead_2",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          },
        }
      },
      {
        EnemyNames.DEVIL,
        new()
        {
          EnemyId = EnemyNames.DEVIL,
          EnemyExpCoefficient = 100f,
          EnemyPrefabName = "Enemy/Boss/Devil/Devil",
          IsBoss = true,
          Speed = 1.25f,
          HealthPoint = 5000,
          BaseNormalHitPoint = 35,
          Fluctuation = 0.25f,
          DefendTypes = new() { HurtTypes.MAGIC_ICE, HurtTypes.MAGIC_THUNDER, HurtTypes.POISON, },
          AdditionalEffectImmunity = new() { BasicAdditionalEffectTypes.FREEZE, BasicAdditionalEffectTypes.STUN },
          PoisonImmunity = true,
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/boss-dead_0",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.WOLF,
        new()
        {
          EnemyId = EnemyNames.WOLF,
          EnemyPrefabName = "Enemy/Boss/Werewolf/Wolf",
          EnemyExpCoefficient = 0.25f,
          IsBoss = false,
          Speed = 0.95f,
          HealthPoint = 25,
          BaseNormalHitPoint = 35,
          Fluctuation = 0.2f,
          DefendTypes = new(),
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.5f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      },
      {
        EnemyNames.WORM,
        new()
        {
          EnemyId = EnemyNames.WORM,
          EnemyPrefabName = "Enemy/Boss/CellMonster/Worm",
          EnemyExpCoefficient = 0.25f,
          IsBoss = false,
          Speed = 0.8f,
          HealthPoint = 35,
          BaseNormalHitPoint = 30,
          Fluctuation = 0.2f,
          DefendTypes = new() { HurtTypes.POISON },
          EnemyStrengthUpgrade = new()
          {
            IncreaseHpCoefficient = 0.07f,
            IncreaseSpeedCoefficient = 0.0135f,
          },
          Enemy2PlayersUpgrade = new()
          {
            IncreaseHpCoefficient = 0.65f,
            IncreaseSpeedCoefficient = 0.2f,
          },
          DeadSounds =
          {
            "Audio/Sound/Enemy/Dead/enemy-dead_0",
            "Audio/Sound/Enemy/Dead/enemy-dead_1",
            "Audio/Sound/Enemy/Dead/enemy-dead_2",
            "Audio/Sound/Enemy/Dead/enemy-dead_3",
            "Audio/Sound/Enemy/Dead/enemy-dead_4",
            "Audio/Sound/Enemy/Dead/enemy-dead_5",
            "Audio/Sound/Enemy/Dead/enemy-dead_6",
            "Audio/Sound/Enemy/Dead/enemy-dead_7",
            "Audio/Sound/Enemy/Dead/enemy-dead_8",
            "Audio/Sound/Enemy/Dead/enemy-dead_9",
            "Audio/Sound/Enemy/Dead/enemy-dead_10",
          },
          HurtSounds =
          {
            "Audio/Sound/Enemy/Hurt/enemy-hurt_0",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_1",
            "Audio/Sound/Enemy/Hurt/enemy-hurt_2",
          }
        }
      }
    };
  }
}