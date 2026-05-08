using System.Collections.Generic;
using Code.Scripts.Src;
using Code.Scripts.Src.Upgrade;

namespace Code.Scripts.Src.Store.Level
{
  public struct UpgradeContainer
  {
    public string UpgradeId { get; set; }
    public UpgradeCategory Category { get; set; }
    public string UpgradeNameIndicator { get; set; }
    public string UpgradeDescriptionIndicator { get; set; }
    public string ImageName { get; set; }
  }

  public class PlayerUpgrades
  {
    public List<UpgradeContainer> Upgrades { get; set; } = new();
  }

  public class UpgradeState : IState
  {
    public PlayerUpgrades[] PlayerUpgrades { get; set; }
  }

  public struct UpgradedSkillData : IActionData
  {
    public int NumberOfPlayers { get; set; }
    public int PlayerNumber { get; set; }
    public string UpgradeId { get; set; }
    public UpgradeCategory Category { get; set; }
    public string UpgradeNameIndicator { get; set; }
    public string UpgradeDescriptionIndicator { get; set; }
    public string ImageName { get; set; }
  }

  public class UpgradeStore : IStore
  {
    public IState InitialState => new UpgradeState();

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      var upgradedSkillState = (UpgradeState)state;
      var upgradedSkillData = (UpgradedSkillData)data;

      var playerNumber = upgradedSkillData.PlayerNumber;

      switch (action)
      {
        case StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS:
          upgradedSkillState.PlayerUpgrades = new PlayerUpgrades[upgradedSkillData.NumberOfPlayers];
          for (var i = 0; i < upgradedSkillState.PlayerUpgrades.Length; i++)
          {
            upgradedSkillState.PlayerUpgrades[i] = new PlayerUpgrades();
          }

          return upgradedSkillState;
        case StoreActions.UpgradeStore_ADD_UPGRADE:
          upgradedSkillState.PlayerUpgrades[playerNumber].Upgrades.Add(new()
          {
            UpgradeId = upgradedSkillData.UpgradeId,
            Category = upgradedSkillData.Category,
            UpgradeNameIndicator = upgradedSkillData.UpgradeNameIndicator,
            UpgradeDescriptionIndicator = upgradedSkillData.UpgradeDescriptionIndicator,
            ImageName = upgradedSkillData.ImageName,
          });
          return upgradedSkillState;
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}