using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.UI.Pause;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.GameOver
{
  public class GameOverPlayerDataDisplayer : MonoBehaviour
  {
    public int PlayerNumber;

    private StoreManager storeManager;

    private Text playerNameText;
    private Image playerImage;
    private Text playerHonorText;
    private Text playerScoreText;
    private Text enemyKilledText;
    private Text expGainedText;
    private Text finalLevelText;
    private Text survivalTimeText;
    private RectTransform upgradesScrollContent;

    private void Awake()
    {
      storeManager = StoreManager.Instance;

      playerNameText = transform.Find("PlayerInfo/PlayerName").GetComponent<Text>();
      playerHonorText = transform.Find("PlayerInfo/PlayerHonor").GetComponent<Text>();
      playerScoreText = transform.Find("LevelData/PlayerScore/Value").GetComponent<Text>();
      enemyKilledText = transform.Find("LevelData/EnemyKilled/Value").GetComponent<Text>();
      expGainedText = transform.Find("LevelData/ExpGained/Value").GetComponent<Text>();
      finalLevelText = transform.Find("LevelData/FinalLevel/Value").GetComponent<Text>();
      survivalTimeText = transform.Find("LevelData/SurvivalTime/Value").GetComponent<Text>();

      playerImage = transform.Find("PlayerInfo/PlayerImage").GetComponent<Image>();

      upgradesScrollContent = transform.Find("Upgrades/ScrollView/Viewport/Content").GetComponent<RectTransform>();
    }

    private void Start()
    {
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      if (PlayerNumber > levelConfigs.InitialNumberOfPlayers - 1)
      {
        gameObject.SetActive(false);
        return;
      }

      ShowFinalData();
      ShowUpgrades();
    }

    private void ShowFinalData()
    {
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      var playerData = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
      var battleData = storeManager.GetState<BattleDataState>(StoreNames.BattleDataStore);

      var deadSeconds = playerData.PlayerDatas[PlayerNumber].DeadTime;
      var finalLevel = playerData.PlayerDatas[PlayerNumber].Level;
      var gainedExp = MathF.Round(playerData.PlayerDatas[PlayerNumber].CurrentExperience);
      var enemyKilled = battleData.KilledEnemies.Count(e => e.KilledByPlayerNumber == PlayerNumber);

      if (PlayerNumber > levelConfigs.InitialNumberOfPlayers - 1)
      {
        gameObject.SetActive(false);
        return;
      }

      playerNameText.text = I18nUtils.GetText(levelConfigs.PlayerConfigurations[PlayerNumber].PlayerNameIndicator);
      var tex = Resources.Load<Texture2D>(levelConfigs.PlayerConfigurations[PlayerNumber].AvatarImageIndicator);
      playerImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2);

      enemyKilledText.text = enemyKilled.ToString();

      var finalScore =
        GeneralConfigurations.GetFinalScore(levelConfigs.LevelDifficulty, deadSeconds, finalLevel, enemyKilled);
      playerScoreText.text = finalScore.ToString();

      var deadTime = TimeSpan.FromSeconds(deadSeconds);
      survivalTimeText.text = deadTime.ToString(@"hh\:mm\:ss");

      expGainedText.text = gainedExp.ToString();
      finalLevelText.text = finalLevel.ToString();

      playerHonorText.text =
        $"{I18nUtils.GetText("UI/PlayerHonor")}: +{GeneralConfigurations.GetFinalHonor(finalScore)}";
    }

    private void ShowUpgrades()
    {
      var playerUpgrades = storeManager.GetState<UpgradeState>(StoreNames.UpgradeStore).PlayerUpgrades[PlayerNumber]
        .Upgrades;

      var shownUpgradeIds = new List<string>();
      var upgradeCards = new List<GameObject>();
      foreach (var upgrade in playerUpgrades)
      {
        if (!shownUpgradeIds.Contains(upgrade.UpgradeId))
        {
          var currentUpgradeId = upgrade.UpgradeId;
          var upgradeCard = Instantiate(Resources.Load<GameObject>("UI/Pause/PauseUpgradeCard"));
          upgradeCard.GetComponent<PauseUpgradeCard>()
            .SetUpgrade(upgrade, playerUpgrades.Count(u => u.UpgradeId == currentUpgradeId));
          upgradeCards.Add(upgradeCard);
          shownUpgradeIds.Add(currentUpgradeId);
        }
      }


      int verticalCount = (int)Mathf.Ceil(upgradeCards.Count / 10f);
      var size = upgradesScrollContent.sizeDelta;
      size.y = 60 * verticalCount;
      upgradesScrollContent.sizeDelta = size;
      for (int vertical = 0; vertical < verticalCount; vertical++)
      {
        int horizontalCount = Mathf.Min(upgradeCards.Count - vertical * 10, 10);
        for (int horizontal = 0; horizontal < horizontalCount; horizontal++)
        {
          var upgradeCard = upgradeCards[vertical * 10 + horizontal];
          var rect = upgradeCard.GetComponent<RectTransform>();
          rect.SetParent(upgradesScrollContent, false);
          rect.localPosition = new Vector2(horizontal * 60, -60 * vertical);
        }
      }
    }
  }
}