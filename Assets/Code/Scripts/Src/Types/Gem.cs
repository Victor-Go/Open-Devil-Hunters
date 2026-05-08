using System;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Player;

namespace Code.Scripts.Src.Types
{
  [Serializable]
  public enum GemRarity
  {
    RARE,
    SUPER_RARE,
    SUPER_SUPER_RARE,
    EXTREME_RARE,
  }

  [Serializable]
  public enum GemColors
  {
    RED,
    GREEN,
    BLUE,
    YELLOW,
    LEMON,
    PURPLE,
  }

  [Serializable]
  public class GemProperties
  {
    public string UUID { get; set; }
    public GemRarity Rarity { get; set; }
    public GemColors Color { get; set; }
    public MovementUpgrade MovementUpgrade { get; set; } = new();
    public AttackControlUpgrade AttackControlUpgrade { get; set; } = new();
    public BasicSkillUpgrader BasicSkillUpgrader { get; set; } = new();

    public int Resurrection { get; set; }

    // public float HpRecovery { get; set; }
    public float IncreaseMaxHp { get; set; }
  }
}