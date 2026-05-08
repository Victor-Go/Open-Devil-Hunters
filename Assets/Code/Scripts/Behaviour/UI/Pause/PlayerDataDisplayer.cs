using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.UI.Pause;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Skill;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Pause
{
  public class PlayerDataDisplayer : MonoBehaviour, IStoreChangedHandler
  {
    [FormerlySerializedAs("playerNumber")] public int PlayerNumber;

    private Text hpRecoveryText; //HP恢复

    private float HpRecovery
    {
      set => hpRecoveryText.text = $"{Mathf.RoundToInt(value * 10000) / 100f}%{I18nUtils.GetText("Measure/PerSecond")}";
    }

    private Text resurrectionText; //复活

    private int Resurrection
    {
      set => resurrectionText.text = value.ToString();
    }

    private Text enemyKilledText; // 击杀数量

    private int EnemyKilled
    {
      set => enemyKilledText.text = value.ToString();
    }

    private Text movingSpeedText; //移动速度

    private float MovingSpeed
    {
      set => movingSpeedText.text = $"{Mathf.RoundToInt(value * 100) / 100f}";
    }

    private Text attackMovingSpeedText; //攻击移动速度

    private float AttackMovingSpeed
    {
      set => attackMovingSpeedText.text = $"{Mathf.RoundToInt(value * 100) / 100f}";
    }

    private Text pickUpRangeText; //拾取范围

    private float PickUpRange
    {
      set => pickUpRangeText.text = $"{Mathf.RoundToInt(value * 10) / 10f}";
    }

    private Text expBonusText; //经验加成
    private Text baseSkillHurtText; //基础技能伤害
    private Text baseSkillPropertyText; //基础技能属性

    private float BaseSkillHurt
    {
      set => baseSkillHurtText.text = $"{Mathf.RoundToInt(value * 10) / 10f}";
    }

    private Text projectileCountText; //发射技能数量

    private int ProjectileCount
    {
      set => projectileCountText.text = $"{value}";
    }

    private Text attackSpeedText; //攻击速度

    private float AttackSpeed
    {
      set => attackSpeedText.text = Mathf.RoundToInt(value * 100) / 100f + I18nUtils.GetText("Measure/PerSecond");
    }

    private Text reloadTimeText; //换弹时间

    private float ReloadTime
    {
      set => reloadTimeText.text = $"{Mathf.RoundToInt(value * 10) / 10f}";
    }

    private Text aETypeText; //附加效果
    private Text aEPossibilityText; //附加效果概率
    private Text aEEffectsText; //附加效果伤害
    private RectTransform upgradesScrollContent;

    private StoreManager storeManager;
    private SkillAndUpgradeController skillAndUpgradeController;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      skillAndUpgradeController = SkillAndUpgradeManager.Instance.SkillAndUpgradeControllers[PlayerNumber];

      // hpRecoveryText = transform.Find("HpRecovery/Value").GetComponent<Text>();  // Removed
      resurrectionText = transform.Find("Resurrection/Value").GetComponent<Text>();
      enemyKilledText = transform.Find("EnemyKilled/Value").GetComponent<Text>();
      movingSpeedText = transform.Find("MovingSpeed/Value").GetComponent<Text>();
      attackMovingSpeedText = transform.Find("AttackingMovingSpeed/Value").GetComponent<Text>();
      pickUpRangeText = transform.Find("PickUpDistance/Value").GetComponent<Text>();
      // expBonusText = transform.Find("ExpBonus/Value").GetComponent<Text>();  // Removed
      baseSkillHurtText = transform.Find("SkillHurtPoint/Value").GetComponent<Text>();
      baseSkillPropertyText = transform.Find("SkillProperty/Value").GetComponent<Text>();
      projectileCountText = transform.Find("ProjectileCount/Value").GetComponent<Text>();
      attackSpeedText = transform.Find("AttackSpeed/Value").GetComponent<Text>();
      reloadTimeText = transform.Find("ReloadTime/Value").GetComponent<Text>();
      aETypeText = transform.Find("AdditionalEffectType/Value").GetComponent<Text>();
      aEPossibilityText = transform.Find("AdditionalEffectPossibility/Value").GetComponent<Text>();
      aEEffectsText = transform.Find("AdditionalEffectEffects/Value").GetComponent<Text>();
      upgradesScrollContent = transform.Find("Upgrades/ScrollView/Viewport/Content").GetComponent<RectTransform>();
    }

    private void Start()
    {
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      if (PlayerNumber > levelConfigs.InitialNumberOfPlayers - 1 || !levelConfigs.PlayerAlive[PlayerNumber])
      {
        gameObject.SetActive(false);
        return;
      }

      // storeManager
      //   .Subscribe(StoreNames.PlayerStore, this)
      //   .Subscribe(StoreNames.AttackControlStore, this);

      var playerState = storeManager.GetState<PlayerState>(StoreNames.PlayerStore);
      HandlePlayerStateChanged(playerState);

      var attackState = storeManager.GetState<AttackControlState>(StoreNames.AttackControlStore);
      HandleAttackControlStateChanged(attackState);

      HandleLevelConfigsChanged(levelConfigs);
      ShowUpgraded();
      ShowSkillData();
      ShowResurrection();
      
      var battleData = storeManager.GetState<BattleDataState>(StoreNames.BattleDataStore);
      var enemyKilled = battleData.KilledEnemies.Count(e => e.KilledByPlayerNumber == PlayerNumber);
      EnemyKilled = enemyKilled;
    }

    private void ShowResurrection()
    {
      var levelGeneral = storeManager.GetState<LevelGeneralState>(StoreNames.LevelGeneralStore);
      Resurrection = levelGeneral.PlayLevelGeneral[PlayerNumber].ResurrectionCount;
    }

    private void ShowSkillData()
    {
      var type = skillAndUpgradeController.BasicSkill.SkillConfigurations.SkillHurt.HurtType;
      switch (type)
      {
        case HurtTypes.PHYSICAL: baseSkillPropertyText.text = I18nUtils.GetText("Physical"); break;
        case HurtTypes.POISON: baseSkillPropertyText.text = I18nUtils.GetText("Poison"); break;
        case HurtTypes.MAGIC_ICE: baseSkillPropertyText.text = I18nUtils.GetText("Ice"); break;
        case HurtTypes.MAGIC_FIRE: baseSkillPropertyText.text = I18nUtils.GetText("Fire"); break;
        case HurtTypes.MAGIC_THUNDER: baseSkillPropertyText.text = I18nUtils.GetText("Thunder"); break;
      }
    }

    private void HandlePlayerStateChanged(PlayerState state)
    {
      var data = state.PlayerDatas[PlayerNumber];

      MovingSpeed = data.MovingSpeed;
      AttackMovingSpeed = data.AttackingMovingSpeed;
      PickUpRange = data.PickUpRadius;
    }

    private void HandleAttackControlStateChanged(AttackControlState state)
    {
      var data = state.AttackControlDatas[PlayerNumber];

      AttackSpeed = 1 / data.AttackControlConfigurations.AttackCoolingTime;
      ReloadTime = data.AttackControlConfigurations.ReloadTime;
    }

    // This can refer to FightPreparation UI but this is actual values
    private void HandleLevelConfigsChanged(LevelConfigurationState levelConfigs)
    {
      if (PlayerNumber > levelConfigs.InitialNumberOfPlayers - 1 ||
          levelConfigs.FightPreparationShowingPlayerConfigurations == null) return;

      BaseSkillHurt = Mathf.RoundToInt(skillAndUpgradeController.BasicSkill.SkillConfigurations.SkillHurt.HurtPoint);
      ProjectileCount = skillAndUpgradeController.BasicSkill.SkillCount;


      #region Display AE related

      var aeType = "";
      var aePossibility = "";
      var aeEffects = "";

      var additionalEffect = skillAndUpgradeController.BasicSkill.SkillConfigurations.AdditionalEffect;
      if (additionalEffect.BasicAdditionalEffectType != BasicAdditionalEffectTypes.NONE)
      {
        aePossibility = $"{additionalEffect.BasicAdditionalEffect.Possibility * 100}%";
        switch (additionalEffect.BasicAdditionalEffectType)
        {
          case BasicAdditionalEffectTypes.FREEZE:
            aeType = I18nUtils.GetText("AdditionalEffect/Type/Freeze");

            var iceAe = (IceAdditionalEffect)additionalEffect.BasicAdditionalEffect;
            aeEffects = $"{iceAe.DamagePerSecond:F1}({iceAe.LastForSeconds:F1}s)";
            break;
          case BasicAdditionalEffectTypes.STUN:
            aeType = I18nUtils.GetText("AdditionalEffect/Type/Stun");

            var thunderAe = (ThunderAdditionalEffect)additionalEffect.BasicAdditionalEffect;
            aeEffects = $"{thunderAe.StunningSeconds:F1}s";
            break;
          case BasicAdditionalEffectTypes.BURN:
            aeType = I18nUtils.GetText("AdditionalEffect/Type/Burn");

            var fireAe = (FireAdditionalEffect)additionalEffect.BasicAdditionalEffect;
            aeEffects = $"{fireAe.HurtPercentagePerSecond:F0}%({fireAe.LastForSeconds:F1}s)";
            break;
        }
      }

      if (additionalEffect.PoisonAdditionalEffect.Enable)
      {
        aeType += $", {I18nUtils.GetText("AdditionalEffect/Type/Poison")}";
        aePossibility += $", {additionalEffect.PoisonAdditionalEffect.Possibility}%";
      }

      if (!string.IsNullOrEmpty(aeType))
      {
        aETypeText.text = aeType;
        aEPossibilityText.text = aePossibility;
        aEEffectsText.text = aeEffects;
      }
      else
      {
        aETypeText.text = "-";
        aEPossibilityText.text = "-";
        aEEffectsText.text = "-";
      }

      #endregion
    }

    private void ShowUpgraded()
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
      for (var vertical = 0; vertical < verticalCount; vertical++)
      {
        var horizontalCount = Mathf.Min(upgradeCards.Count - vertical * 10, 10);
        for (var horizontal = 0; horizontal < horizontalCount; horizontal++)
        {
          var upgradeCard = upgradeCards[vertical * 10 + horizontal];
          var rect = upgradeCard.GetComponent<RectTransform>();
          rect.SetParent(upgradesScrollContent, false);
          rect.localPosition = new Vector2(horizontal * 60, -60 * vertical);
        }
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.PlayerStore:
          HandlePlayerStateChanged((PlayerState)state);
          break;
        case StoreNames.AttackControlStore:
          HandleAttackControlStateChanged((AttackControlState)state);
          break;
        case StoreNames.LevelConfigurationStore:
          HandleLevelConfigsChanged((LevelConfigurationState)state);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}