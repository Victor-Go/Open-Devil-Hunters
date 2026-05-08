using System;
using System.Collections.Generic;
using Code.Scripts.Src.Types;
using JetBrains.Annotations;

namespace Code.Scripts.Src.Configurations
{
  // The order of the enum must not be changed for game save since enum in binary is an int
  [Serializable]
  public enum PlayerNames
  {
    ARCHANGEL,
    CAPTAIN_G,
    CUTIE,
    GUMDAM,
    JEANNE_D_ARC,
    MOUNTAIN_KING,
    PALADIN,
    RANGER,
    WITCH,
    WUKONG,
    ZHAOYUN,
  }

  public class PlayerConfiguration
  {
    public PlayerNames PlayerName { get; set; }
    public int HonorRequirement { get; set; }
    public string PlayerNameIndicator { get; set; }
    public string PlayerPrefabName { get; set; }
    public string AvatarImageIndicator { get; set; }
    public string DescriptionIndicator { get; set; }
    public string AnimatorName { get; set; }
    public float MaximumHp { get; set; }
    public float AttackingMovingSpeed { get; set; }
    public float MovingSpeed { get; set; }
    public float PickUpRadius { get; set; }
    public string DefaultSkillId { get; set; }
    public float RecoverPerSecond { get; set; }
    public string[] PlayerFireAudios { get; set; }
    public string[] ReadyAudios { get; set; }
    [CanBeNull] public List<BasicAdditionalEffectTypes> AdditionalEffectImmunity { get; set; }
    public bool PoisonImmunity { get; set; }
  }

  public static class PlayerConfigurations
  {
    public static Dictionary<PlayerNames, PlayerConfiguration> Players { get; } = new()
    {
      {
        PlayerNames.ARCHANGEL,
        new PlayerConfiguration()
        {
          PlayerName = PlayerNames.ARCHANGEL,
          HonorRequirement = 10000,
          PlayerNameIndicator = "Hero/Archangel",
          PlayerPrefabName = "Player/Archangel",
          DescriptionIndicator = "UI/Hero/Description/Archangel",
          AvatarImageIndicator = "PlayerAvatar/archangel",
          AnimatorName = "Player/Archangel",
          DefaultSkillId = "ArchangelSkill",
          MaximumHp = 90,
          PickUpRadius = 1.75f,
          MovingSpeed = 1.9f,
          AttackingMovingSpeed = 1.2f,
          RecoverPerSecond = 0.005f,
        }
      },
      {
        PlayerNames.CAPTAIN_G,
        new PlayerConfiguration()
        {
          PlayerName = PlayerNames.CAPTAIN_G,
          HonorRequirement = 10000,
          PlayerNameIndicator = "Hero/CaptainG",
          PlayerPrefabName = "Player/CaptainG",
          DescriptionIndicator = "UI/Hero/Description/CaptainG",
          AvatarImageIndicator = "PlayerAvatar/captain-g",
          AnimatorName = "Player/CaptainG",
          DefaultSkillId = "CaptainGSkill",
          AdditionalEffectImmunity = new()
          {
            BasicAdditionalEffectTypes.BURN,
          },
          MaximumHp = 190,
          PickUpRadius = 1.5f,
          MovingSpeed = 1f,
          AttackingMovingSpeed = 1f,
          RecoverPerSecond = 0f,
          ReadyAudios = new[]
          {
            "Audio/Sound/Player/Ready/captain-ready_0",
            "Audio/Sound/Player/Ready/captain-ready_1",
            "Audio/Sound/Player/Ready/captain-ready_2",
          }
        }
      },
      {
        PlayerNames.CUTIE,
        new PlayerConfiguration()
        {
          PlayerName = PlayerNames.CUTIE,
          HonorRequirement = 10000,
          PlayerNameIndicator = "Hero/Cutie",
          PlayerPrefabName = "Player/Cutie",
          DescriptionIndicator = "UI/Hero/Description/Cutie",
          AvatarImageIndicator = "PlayerAvatar/cutie",
          AnimatorName = "Player/Cutie",
          DefaultSkillId = "CutieSkill",
          MaximumHp = 135,
          PickUpRadius = 3.5f,
          MovingSpeed = 1.9f,
          AttackingMovingSpeed = 1.2f,
          RecoverPerSecond = 0f,
          ReadyAudios = new[]
          {
            "Audio/Sound/Player/Ready/cutie-ready_0",
            "Audio/Sound/Player/Ready/cutie-ready_1",
            "Audio/Sound/Player/Ready/cutie-ready_2",
            "Audio/Sound/Player/Ready/cutie-ready_3",
          }
        }
      },
      {
        PlayerNames.GUMDAM,
        new PlayerConfiguration()
        {
          PlayerName = PlayerNames.GUMDAM,
          HonorRequirement = 10000,
          PlayerNameIndicator = "Hero/Gumdam",
          PlayerPrefabName = "Player/Gumdam",
          DescriptionIndicator = "UI/Hero/Description/Gumdam",
          AvatarImageIndicator = "PlayerAvatar/gumdam",
          AnimatorName = "Player/Gumdam",
          DefaultSkillId = "GumdamSkill",
          MaximumHp = 160,
          PickUpRadius = 2.5f,
          MovingSpeed = 1.5f,
          AttackingMovingSpeed = 1.2f,
          RecoverPerSecond = 0f,
          ReadyAudios = new[]
          {
            "Audio/Sound/Player/Ready/gumdam-ready_0",
            "Audio/Sound/Player/Ready/gumdam-ready_1",
            "Audio/Sound/Player/Ready/gumdam-ready_2",
          }
        }
      },
      {
        PlayerNames.JEANNE_D_ARC,
        new PlayerConfiguration()
        {
          PlayerName = PlayerNames.JEANNE_D_ARC,
          HonorRequirement = 10000,
          PlayerNameIndicator = "Hero/JeanneDArc",
          PlayerPrefabName = "Player/JeanneDArc",
          DescriptionIndicator = "UI/Hero/Description/JeanneDArc",
          AvatarImageIndicator = "PlayerAvatar/jeanne-d-arc",
          AnimatorName = "Player/JeanneDArc",
          DefaultSkillId = "JeanneDArcSkill",
          MaximumHp = 200,
          PickUpRadius = 1.5f,
          MovingSpeed = 1.8f,
          AttackingMovingSpeed = 1.5f,
          RecoverPerSecond = 0f,
        }
      },
      {
        PlayerNames.MOUNTAIN_KING,
        new PlayerConfiguration()
        {
          PlayerName = PlayerNames.MOUNTAIN_KING,
          HonorRequirement = 10000,
          PlayerNameIndicator = "Hero/MountainKing",
          PlayerPrefabName = "Player/MountainKing",
          DescriptionIndicator = "UI/Hero/Description/MountainKing",
          AvatarImageIndicator = "PlayerAvatar/mountain-king",
          AnimatorName = "Player/MountainKing",
          DefaultSkillId = "MountainKingSkill",
          AdditionalEffectImmunity = new()
          {
            BasicAdditionalEffectTypes.STUN,
          },
          MaximumHp = 160,
          PickUpRadius = 1.75f,
          MovingSpeed = 1.5f,
          AttackingMovingSpeed = 1.2f,
          RecoverPerSecond = 0f,
        }
      },
      {
        PlayerNames.PALADIN,
        new PlayerConfiguration()
        {
          PlayerName = PlayerNames.PALADIN,
          HonorRequirement = 10000,
          PlayerNameIndicator = "Hero/Paladin",
          PlayerPrefabName = "Player/Paladin",
          DescriptionIndicator = "UI/Hero/Description/Paladin",
          AvatarImageIndicator = "PlayerAvatar/paladin",
          AnimatorName = "Player/Paladin",
          DefaultSkillId = "PaladinSkill",
          MaximumHp = 120,
          PickUpRadius = 2f,
          MovingSpeed = 1.5f,
          AttackingMovingSpeed = 1.2f,
          RecoverPerSecond = 0.0025f,
        }
      },
      {
        PlayerNames.RANGER,
        new PlayerConfiguration()
        {
          PlayerName = PlayerNames.RANGER,
          HonorRequirement = 10000,
          PlayerNameIndicator = "Hero/Ranger",
          PlayerPrefabName = "Player/Ranger",
          DescriptionIndicator = "UI/Hero/Description/Ranger",
          AvatarImageIndicator = "PlayerAvatar/ranger",
          AnimatorName = "Player/Ranger",
          DefaultSkillId = "RangerSkill",
          MaximumHp = 190,
          PickUpRadius = 2.5f,
          MovingSpeed = 2,
          AttackingMovingSpeed = 1.5f,
          RecoverPerSecond = 0f,
          PoisonImmunity = true,
          ReadyAudios = new[]
          {
            "Audio/Sound/Player/Ready/ranger-ready_0",
            "Audio/Sound/Player/Ready/ranger-ready_1",
            "Audio/Sound/Player/Ready/ranger-ready_2",
            "Audio/Sound/Player/Ready/ranger-ready_3",
          }
        }
      },
      {
        PlayerNames.WITCH,
        new PlayerConfiguration()
        {
          PlayerName = PlayerNames.WITCH,
          HonorRequirement = 10000,
          PlayerNameIndicator = "Hero/Witch",
          PlayerPrefabName = "Player/Witch",
          DescriptionIndicator = "UI/Hero/Description/Witch",
          AvatarImageIndicator = "PlayerAvatar/witch",
          AnimatorName = "Player/Witch",
          DefaultSkillId = "WitchSkill",
          MaximumHp = 165,
          PickUpRadius = 2f,
          MovingSpeed = 1.5f,
          AttackingMovingSpeed = 1.2f,
          RecoverPerSecond = 0f,
        }
      },
      {
        PlayerNames.WUKONG,
        new PlayerConfiguration()
        {
          PlayerName = PlayerNames.WUKONG,
          HonorRequirement = 10000,
          PlayerNameIndicator = "Hero/WuKong",
          PlayerPrefabName = "Player/WuKong",
          DescriptionIndicator = "UI/Hero/Description/WuKong",
          AvatarImageIndicator = "PlayerAvatar/wukong",
          AnimatorName = "Player/WuKong",
          DefaultSkillId = "WuKongSkill",
          MaximumHp = 150,
          PickUpRadius = 1.75f,
          MovingSpeed = 1.3f,
          AttackingMovingSpeed = 1.1f,
          RecoverPerSecond = 0f,
          PlayerFireAudios = new[]
          {
            "Audio/Sound/Player/FireSkill/wukong-fire_0",
            "Audio/Sound/Player/FireSkill/wukong-fire_1",
          }
        }
      },
      {
        PlayerNames.ZHAOYUN,
        new PlayerConfiguration()
        {
          PlayerName = PlayerNames.ZHAOYUN,
          HonorRequirement = 10000,
          PlayerNameIndicator = "Hero/ZhaoYun",
          PlayerPrefabName = "Player/ZhaoYun",
          DescriptionIndicator = "UI/Hero/Description/ZhaoYun",
          AvatarImageIndicator = "PlayerAvatar/zhaoyun",
          AnimatorName = "Player/ZhaoYun",
          DefaultSkillId = "ZhaoYunSkill",
          AdditionalEffectImmunity = new()
          {
            BasicAdditionalEffectTypes.FREEZE,
          },
          MaximumHp = 190,
          PickUpRadius = 2f,
          MovingSpeed = 1.5f,
          AttackingMovingSpeed = 1.2f,
          RecoverPerSecond = 0f,
          PlayerFireAudios = new[]
          {
            "Audio/Sound/Player/FireSkill/zhaoyun-fire_0",
            "Audio/Sound/Player/FireSkill/zhaoyun-fire_1",
          }
        }
      }
    };
  }
}