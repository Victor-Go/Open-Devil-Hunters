using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class FightPreparationPlayerDataDisplayer : UIWindow, IStoreChangedHandler
  {
    public int PlayerNumber;

    private bool initialized;

    private Image playerImage;
    private Text playerName;
    private Text playerDescriptionText;

    #region UI

    [SerializeField] private GameObject addPlayerButton;
    private GameObject playerInfoContainer;

    #endregion

    #region Player

    private float MaxHp
    {
      set => maxHpText.text = Mathf.CeilToInt(value).ToString();
    }

    private Text maxHpText; //最大生命值


    private float HpRecovery
    {
      set =>
        hpRecoveryText.text = $"{value * 100:F2}%{I18nUtils.GetText("Measure/PerSecond")}";
    }

    private Text hpRecoveryText; //HP恢复


    private int Resurrection
    {
      set => resurrectionText.text = value.ToString();
    }

    private Text resurrectionText; //复活

    #endregion

    #region Skill

    private float BaseSkillHurt
    {
      set => baseSkillHurtText.text = $"{Mathf.RoundToInt(value * 10) / 10f}";
    }

    private Text baseSkillHurtText; //基础技能伤害

    private Text baseSkillPropertyText; //基础技能属性

    private float basicAEPossibility, poisonAEPossibility;
    private Text skillPossibilityText;

    private Text reloadTimeText; //暂无：换弹时间

    private float ReloadTime
    {
      set => reloadTimeText.text = $"{Mathf.RoundToInt(value * 10) / 10f}";
    }

    private float AttackRange
    {
      set => attackRangeText.text = $"{Mathf.RoundToInt(value * 100) / 100f}";
    }

    private Text attackRangeText; //攻击范围


    private int ProjectileCount
    {
      set => projectileCountText.text = $"{value}";
    }

    private Text projectileCountText; // 投射数量

    // private int AttackPerRound
    // {
    //   set => attackPerRoundText.text = value > 100000 ? "-" : value.ToString();
    // }
    //
    // private Text attackPerRoundText; //每轮攻击


    private float AttackSpeed
    {
      set => attackSpeedText.text = Mathf.RoundToInt(value * 100) / 100f + I18nUtils.GetText("Measure/PerSecond");
    }

    private Text attackSpeedText; //攻击速度

    #endregion

    #region Others

    private float PickUpRange
    {
      set => pickUpRangeText.text = $"{Mathf.RoundToInt(value * 10) / 10f}";
    }

    private Text pickUpRangeText; //拾取范围


    private float MovingSpeed
    {
      set => movingSpeedText.text =
        $"{Mathf.Clamp(Mathf.RoundToInt(value * 100) / 100f, 0.1f, GeneralConfigurations.PlayerMaxSpeed)}";
    }

    private Text movingSpeedText; //移动速度


    private float AttackMovingSpeed
    {
      set => attackMovingSpeedText.text =
        $"{Mathf.Clamp(Mathf.RoundToInt(value * 100) / 100f, 0.1f, GeneralConfigurations.MaximumPlayerAttackingMovingSpeed)}";
    }

    private Text attackMovingSpeedText; //攻击移动速度

    #endregion

    private ResourceManager resourceManager;
    private StoreManager storeManager;

    private void Awake()
    {
      playerInfoContainer = transform.Find("PlayerInfoContainer").gameObject;

      playerDescriptionText = transform.Find("PlayerInfoContainer/PlayerDescription/Text").GetComponent<Text>();
      playerName = transform.Find("PlayerInfoContainer/PlayerName").GetComponent<Text>();
      playerImage = transform.Find("PlayerInfoContainer/PlayerImage").GetComponent<Image>();

      maxHpText = transform.Find("PlayerInfoContainer/PlayerData/MaxHp/Value").GetComponent<Text>();
      hpRecoveryText = transform.Find("PlayerInfoContainer/PlayerData/HpRecovery/Value").GetComponent<Text>();
      resurrectionText = transform.Find("PlayerInfoContainer/PlayerData/Resurrection/Value").GetComponent<Text>();

      projectileCountText = transform.Find("PlayerInfoContainer/PlayerData/ProjectileCount/Value").GetComponent<Text>();
      baseSkillHurtText = transform.Find("PlayerInfoContainer/PlayerData/BaseSkillHurt/Value").GetComponent<Text>();
      baseSkillPropertyText =
        transform.Find("PlayerInfoContainer/PlayerData/BaseSkillProperty/Value").GetComponent<Text>();
      skillPossibilityText =
        transform.Find("PlayerInfoContainer/PlayerData/BaseSkillPossibility/Value").GetComponent<Text>();
      attackRangeText = transform.Find("PlayerInfoContainer/PlayerData/AttackRange/Value").GetComponent<Text>();
      // attackPerRoundText = transform.Find("PlayerInfoContainer/PlayerData/AttackPerRound/Value").GetComponent<Text>();
      attackSpeedText = transform.Find("PlayerInfoContainer/PlayerData/AttackSpeed/Value").GetComponent<Text>();

      pickUpRangeText = transform.Find("PlayerInfoContainer/PlayerData/PickUpRange/Value").GetComponent<Text>();
      movingSpeedText = transform.Find("PlayerInfoContainer/PlayerData/MovingSpeed/Value").GetComponent<Text>();
      attackMovingSpeedText =
        transform.Find("PlayerInfoContainer/PlayerData/AttackMovingSpeed/Value").GetComponent<Text>();

      resourceManager = ResourceManager.Instance;
      storeManager = StoreManager.Instance;
    }

    private void Start()
    {
      storeManager
        .Subscribe(StoreNames.LevelConfigurationStore, this);

      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      SetAddPlayerUI(levelConfigs);
      OnLevelConfigurationChanged(levelConfigs);

      initialized = true;
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          if (initialized)
          {
            var levelConfigs = (LevelConfigurationState)state;
            SetAddPlayerUI(levelConfigs);
            OnLevelConfigurationChanged(levelConfigs);
          }

          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void SetAddPlayerUI(LevelConfigurationState levelConfigs)
    {
      if (PlayerNumber > levelConfigs.NumberOfPlayers - 1)
      {
        if (PlayerNumber == 1)
        {
          addPlayerButton.SetActive(true);
        }

        playerInfoContainer.SetActive(false);
      }
      else
      {
        if (PlayerNumber == 1)
        {
          addPlayerButton.SetActive(false);
        }

        playerInfoContainer.SetActive(true);
      }
    }

    public void OpenPickRuneUI()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      var pickRuneUi = OpenSubWindow(WindowNames.PickRuneUI);

      var pickRuneUiScript = pickRuneUi.GetComponent<PickRuneUI>();
      pickRuneUiScript.Initialize(this, canvasRt);
      pickRuneUiScript.SetPlayerNumber(PlayerNumber);
    }

    public void OpenPickGemUI()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      // var pickGemUi = OpenSubWindow(WindowNames.PickGemUi, true);
      var pickGemUi = OpenSubWindow(WindowNames.PickGemUI);
      pickGemUi.GetComponent<PickGemUI>().SetPlayerNumber(PlayerNumber);
    }

    private void OnLevelConfigurationChanged(LevelConfigurationState levelConfigs)
    {
      if (PlayerNumber > levelConfigs.InitialNumberOfPlayers - 1 ||
          levelConfigs.FightPreparationShowingPlayerConfigurations == null) return;

      var playerConfigs = levelConfigs.FightPreparationShowingPlayerConfigurations[PlayerNumber];
      var playerGems = levelConfigs.EquippedGems[PlayerNumber].Gems;
      var playerRunes = levelConfigs.RunesConfigurations[PlayerNumber].ActivatedRunes;

      if (playerConfigs == null) return;

      playerName.text = I18nUtils.GetText(playerConfigs.PlayerNameIndicator);
      var tex = Resources.Load<Texture2D>(playerConfigs.AvatarImageIndicator);
      playerImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2);

      playerDescriptionText.text = I18nUtils.GetText(playerConfigs.DescriptionIndicator);

      #region Calculate Max Hp

      var maxHp = playerConfigs.MaximumHp;
      maxHp += playerGems.Select(gem => gem.IncreaseMaxHp).Aggregate(0f, (a, b) => a + b);
      MaxHp = maxHp;

      #endregion

      #region Calculate Hp recovery

      var hpRecovery = playerConfigs.RecoverPerSecond;
      var recoveryRunes = playerRunes.FindAll(rune => rune.RuneType == RuneTypes.HP_RECOVERY);
      if (recoveryRunes.Any())
      {
        hpRecovery += recoveryRunes.Select(rune => 0.002f + 0.002f * rune.RuneLevel).Aggregate(0f, (a, b) => a + b);
      }

      // TODO: Difficult to implement, delayed
      // hpRecovery += playerGems.Select(gem => gem.HpRecovery).Aggregate(0f, (a, b) => a + b);

      HpRecovery = hpRecovery;

      #endregion

      #region Calculate resurrection quantity

      var resurrection = 0;
      if (playerRunes.Exists(rune => rune.RuneType == RuneTypes.RESURRECTION))
      {
        resurrection++;
      }

      resurrection += playerGems.Select(gem => gem.Resurrection).Aggregate(0, (a, b) => a + b);

      Resurrection = resurrection;

      #endregion

      #region Calculate skills

      var skillPreset = SkillPresets.PlayerBasicSkills[playerConfigs.DefaultSkillId];
      var skillConfigs = skillPreset.SkillConfigurations;
      BaseSkillHurt = skillConfigs.SkillHurt.HurtPoint +
                      playerGems.Select(gem =>
                        {
                          var hurtUpgrade = gem.BasicSkillUpgrader.SkillHurtUpgrade;
                          var hurt = skillConfigs.SkillHurt.HurtPoint;
                          return skillConfigs.SkillHurt.HurtType switch
                          {
                            HurtTypes.MAGIC_ICE => hurtUpgrade.MagicIceHurtIncrementByValue +
                                                   hurt * hurtUpgrade.MagicIceHurtIncrementByPercentage,
                            HurtTypes.MAGIC_FIRE => hurtUpgrade.MagicFireHurtIncrementByValue +
                                                    hurt * hurtUpgrade.MagicFireHurtIncrementByPercentage,
                            HurtTypes.MAGIC_THUNDER => hurtUpgrade.MagicThunderHurtIncrementByValue +
                                                       hurt * hurtUpgrade.MagicThunderHurtIncrementByPercentage,
                            HurtTypes.PHYSICAL => hurtUpgrade.PhysicalHurtIncrementByValue +
                                                  hurt * hurtUpgrade.PhysicalHurtIncrementByPercentage,
                            _ => 0
                          };
                        })
                        .Aggregate(0f, (a, b) => a + b);

      // Show base skill type
      var type = skillConfigs.SkillHurt.HurtType;
      baseSkillPropertyText.text = type switch
      {
        HurtTypes.PHYSICAL => I18nUtils.GetText("Physical"),
        HurtTypes.POISON => I18nUtils.GetText("Poison"),
        HurtTypes.MAGIC_ICE => I18nUtils.GetText("Ice"),
        HurtTypes.MAGIC_FIRE => I18nUtils.GetText("Fire"),
        HurtTypes.MAGIC_THUNDER => I18nUtils.GetText("Thunder"),
        _ => baseSkillPropertyText.text
      };

      // Show additional effect
      var possibility = "";
      var ae = skillConfigs.AdditionalEffect;
      if (ae.BasicAdditionalEffectType != BasicAdditionalEffectTypes.NONE)
      {
        var color = ae.BasicAdditionalEffectType switch
        {
          BasicAdditionalEffectTypes.BURN => "#ff7b00",
          BasicAdditionalEffectTypes.STUN => "#ffe600",
          BasicAdditionalEffectTypes.FREEZE => "#00f7ff",
          _ => ""
        };

        possibility +=
          $"<color={color}>{Mathf.RoundToInt(ae.BasicAdditionalEffect.Possibility * 1000) / 10f}%</color>/";
        basicAEPossibility = ae.BasicAdditionalEffect.Possibility;
      }
      else
      {
        possibility += "-/";
      }

      // Show poison additional effect
      if (ae.PoisonAdditionalEffect is { Enable: true })
      {
        possibility +=
          $"<color=#059e54>{Mathf.RoundToInt(ae.PoisonAdditionalEffect.Possibility * 1000) / 10f}%</color>";
        poisonAEPossibility = ae.PoisonAdditionalEffect.Possibility;
      }
      else
      {
        possibility += "<color=green>-</color>";
      }

      skillPossibilityText.text = possibility;

      PickUpRange = playerConfigs.PickUpRadius;

      ProjectileCount = skillPreset.DefaultQuantity +
                        playerGems
                          .Select(gem =>
                            gem.BasicSkillUpgrader.SkillCountIncrementByValue)
                          .Aggregate(0, (a, b) => a + b);
      AttackRange = skillConfigs.Range +
                    playerGems
                      .Select(gem =>
                        gem.BasicSkillUpgrader.RangeIncrementByValue +
                        skillConfigs.Range * gem.BasicSkillUpgrader.RangeIncrementByPercentage)
                      .Aggregate(0f, (a, b) => a + b);
      AttackSpeed = 1 / (
        skillPreset.AttackControlConfigurations.AttackCoolingTime +
        playerGems
          .Select(gem =>
            skillPreset.AttackControlConfigurations.AttackCoolingTime *
            gem.AttackControlUpgrade.CoolingCountdownDecrementByPercentage)
          .Aggregate(0f, (a, b) => a + b)
      );
      MovingSpeed = playerConfigs.MovingSpeed +
                    playerGems
                      .Select(gem =>
                        gem.MovementUpgrade.AugmentMovingSpeedByValue + playerConfigs.MovingSpeed *
                        gem.MovementUpgrade.AugmentMovingSpeedByPercentage
                      )
                      .Aggregate(0f, (a, b) => a + b);
      AttackMovingSpeed = playerConfigs.AttackingMovingSpeed +
                          playerGems
                            .Select(gem =>
                              gem.MovementUpgrade.AugmentAttackingMovingSpeedByValue +
                              playerConfigs.AttackingMovingSpeed *
                              gem.MovementUpgrade.AugmentAttackingMovingSpeedByPercentage)
                            .Aggregate(0f, (a, b) => a + b);

      #endregion
    }

    protected override void Update()
    {
      var xButton = InputUtils.GetButton(PlayerNumber, InputButtons.GAMEPAD_X);
      var yButton = InputUtils.GetButton(PlayerNumber, InputButtons.GAMEPAD_Y);

      if (xButton)
      {
        OpenPickGemUI();
      }
      else if (yButton)
      {
        OpenPickRuneUI();
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}