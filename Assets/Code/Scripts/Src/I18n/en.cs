using System.Collections.Generic;

namespace Code.Scripts.Src.I18n
{
  public struct en : ILanguage
  {
    public static readonly Dictionary<string, string> locales = new()
    {
      { "HelloWorld", "Hello, World!" },
      { "Language", "English" },
      { "NotAvailableInDemo", "In Development" },
      { "Player1", "Player 1" },
      { "Player2", "Player 2" },

      #region Notification

      {
        "Notification/WelcomeNotification",
        "Thanks for purchasing our game!\nWe know our game is not perfect, that's why we need your feedback!\nStay tuned for continuous updates."
      },

      #endregion

      #region Measures

      { "Measure/PerSecond", "/sec" },

      #endregion

      #region UI

      {
        "UI/FightPreparationGuide/Page0/0",
        "There are 5 attributes for the attacks of heroes and enemies, namely physical, ice, fire, thunder, and poison."
      },
      {
        "UI/FightPreparationGuide/Page0/1",
        "Among them, ice attribute restrains fire attribute, fire attribute restrains thunder attribute, and thunder attribute restrains ice attribute."
      },
      {
        "UI/FightPreparationGuide/Page1/0",
        "Ice attribute may trigger freezing effect and cause numerical damage;\nFire attribute may trigger ignition effect and cause proportional damage;\nThunder attribute may trigger stun effect, but will not cause additional damage.\nAll the above attributes will be eliminated after a period of time.\nPoison attribute may trigger poisoning and continue to cause proportional damage. But bosses can often alleviate or even remove poisoning."
      },
      {
        "UI/FightPreparationGuide/Page1/1",
        "Some enemies have attribute defense, attribute effect immunity and attribute defense breaking. Making good use of the attributes that restrain the enemy can make it easier to defeat the opponent!"
      },

      {
        "UI/FightReadyGuide/Page0/0",
        "Here will show the attributes of the monsters in the current map. Remember these attributes and match your own strategy to defeat the monsters!"
      },
      {
        "UI/FightReadyGuide/Page1/0",
        "These two positions will show the bosses sitting in the map. Each boss has different combat skills and styles. Try to use different techniques to defeat different bosses!"
      },
      {
        "UI/FightReadyGuide/Page1/1",
        "Don't forget to explore the map more often, where you can collect different gems to strengthen your heroes.\nYou can also use the honors you get from battles to unlock new heroes and runes for better exploration in different maps!"
      },

      { "UI/ControlGuide/ActiveSkill", "Active Skill" },
      { "UI/ControlGuide/AutoFiring", "Auto-Firing" },
      { "UI/ControlGuide/Movement", "Movement" },
      { "UI/ControlGuide/Fire!", "Fire!" },
      { "UI/ControlGuide/AutoAiming", "Auto-Aiming" },
      { "UI/ControlGuide/Aim", "Aiming" },
      {
        "UI/ControlGuide/MoveToUseController", "Move the joystick to use controller aiming (Not in auto-aiming mode)."
      },
      { "UI/ControlGuide/MoveToUseMouse", "Move the mouse to use mouse aiming (Not in auto-aiming mode)." },
      { "UI/Control/AutoAiming", "Auto-Aiming" },
      { "UI/Control/MouseAiming", "Mouse Aiming" },
      { "UI/Control/ControllerAiming", "Controller Aiming" },
      { "UI/Control/AutoFiring", "Auto-Firing" },
      { "UI/Control/ManualFiring", "Manual Firing" },
      { "UI/Control/UseMouseSelectHero", "Use mouse to select a hero" },

      { "UI/Text/LevelUp!", "Level Up!" },
      { "UI/Button/Choose", "Choose" },
      { "UI/Start", "Start" },
      { "UI/Languages", "Languages" },
      { "UI/Options", "Options" },
      { "UI/Exit", "Exit" },
      { "UI/Name", "Name" },
      { "UI/Description", "Description" },
      { "UI/Properties", "Properties" },
      { "UI/Properties/AttacksPerRound", "Attacks per Round" },
      { "UI/Properties/RateOfFire", "Rate Of Fire" },
      { "UI/Properties/ReloadTime", "Reload Time" },
      { "UI/Properties/Projectiles", "Projectiles" },
      { "UI/Properties/FreezeDamagePerSecond", "Damage per Second" },
      { "UI/Properties/MovingSpeed", "Moving Speed" },
      { "UI/Properties/Physical", "Physical" },
      { "UI/Properties/Ice", "Ice" },
      { "UI/Properties/Fire", "Fire" },
      { "UI/Properties/Thunder", "Thunder" },
      { "UI/Properties/Poisoning", "Poisoning" },
      { "UI/GameOver", "Game Over" },
      { "UI/FinalScore", "Final Score" },
      { "UI/HonorGained", "Honor Gained" },
      { "UI/PlayerHonor", "Player Honor" },
      { "UI/PlayerScore", "Player Score" },
      { "UI/EnemyKilled", "Enemy Killed" },
      { "UI/ExperienceGained", "Experience Gained" },
      { "UI/FinalLevel", "Final Level" },
      { "UI/SurvivalTime", "Survival Time" },
      { "UI/Upgrades", "Upgrades" },
      { "UI/GameOver/Failed", "DEFEAT!" },
      { "UI/GameOver/Success", "LEVEL CLEARED!" },
      { "UI/GameOver/Aborted", "MISSION ABORTED" },

      { "UI/Achievements", "Achievements" },
      { "UI/Furnace", "Furnace" },
      { "UI/Gems", "Gems" },
      { "UI/Runes", "Runes" },
      { "UI/Difficulty", "Difficulty" },
      { "UI/Mode", "Play Mode" },
      { "UI/CasualMode", "Casual Mode" },
      { "UI/ChallengeMode", "ChallengeMode" },
      { "UI/InfiniteMode", "InfiniteMode" },
      { "UI/Play", "Play" },
      { "UI/Confirm", "Confirm" },
      { "UI/Cancel", "Cancel" },
      { "UI/GemList", "Gem List" },

      { "UI/HeroDescription/MaxHp", "Max HP" },
      { "UI/HeroDescription/HpRecover", "HP Recovery" },
      { "UI/HeroDescription/Resurrection", "Resurrection" },
      { "UI/HeroDescription/BasicSkillHurt", "Basic Skill Hurt" },
      { "UI/HeroDescription/BasicSkillProperty", "Basic Skill Attr." },
      { "UI/HeroDescription/Possibility", "AE Possibility" },
      { "UI/HeroDescription/AttackRange", "Attack Range" },
      { "UI/HeroDescription/AttackPerRound", "Attack Per Round" },
      { "UI/HeroDescription/AttackSpeed", "Attack Speed" },
      { "UI/HeroDescription/PickUpRange", "Pick Up Range" },
      { "UI/HeroDescription/MovingSpeed", "Moving Speed" },
      { "UI/HeroDescription/AttackMovingSpeed", "Attack Moving S." },
      { "UI/HeroDescription/BaseSkill", "Base Skill" },
      { "UI/HeroDescription/AdditionalEffect", "Additional Effect" },
      { "UI/HeroDescription/ProjectileCount", "Projectile Count" },
      { "UI/HeroDescription/AEHurt", "AE Hurt" },

      { "UI/FightPreparation/TwoPlayersUniqueHero", "Two players cannot choose the same hero" },
      { "UI/FightPreparation/BrowserHeroes", "Browser Heroes" },
      { "UI/FightPreparation/Back", "Back" },
      { "UI/FightPreparation/ApplyGems", "Wear Gems" },
      { "UI/FightPreparation/ApplyRunes", "Activate Runes" },
      { "UI/FightPreparation/SelectHero", "Select Hero" },
      { "UI/FightPreparation/Unlock", "Unlock" },

      {
        "UI/FightPreparation/AddPlayer",
        "Press <color=red>A</color> on gamepad or Right Ctrl on keyboard to add 2nd player"
      },
      { "UI/FightPreparation/RemovePlayer", "Remove" },

      { "UI/PickGemUI/PickGemTitle", "Wear Gems" },
      { "UI/PickGemUI/OneGemOnePlayer", "A gem can only be used by a single player" },

      { "UI/PickRuneUI/PickRuneTitle", "Activate Runes" },
      { "UI/PickRuneUI/Class", "Level" },
      { "UI/PickRuneUI/InspirationRune", "Inspiration Rune" },
      { "UI/PickRuneUI/DominationRune", "Domination Rune" },
      { "UI/PickRuneUI/ImmortalRune", "Immortal Rune" },
      { "UI/PickRuneUI/Upgrade", "Upgrade" },
      { "UI/PickRuneUI/Price", "Price" },
      { "UI/PickRuneUI/OneRunePerClass", "Only one rune can be chosen per level" },

      { "UI/Pause/PhysicalAttack", "Physical Attack" },
      { "UI/Pause/IceAttack", "Ice Attack" },
      { "UI/Pause/FireAttack", "Fire Attack" },
      { "UI/Pause/ThunderAttack", "Thunder Attack" },
      { "UI/Pause/Poisoning", "Poisoning" },
      { "UI/Pause/MovingSpeed", "Moving Speed" },
      { "UI/Pause/AttackingSpeed", "Speed when attacking" },
      { "UI/Pause/AttackSpeed", "Attack Speed" },
      { "UI/Pause/RecoverSpeed", "Recover Speed" },
      { "UI/Pause/ExpBonus", "Experience Bonus" },
      { "UI/Pause/RoundsPerSecond", " rounds/s" },
      { "UI/Pause/Resume", "Resume" },
      { "UI/Pause/ControlGuide", "Control Guide" },
      { "UI/Pause/GiveUp", "Give Up" },

      {
        "UI/Upgrade/DemoUpgradeUnlocked",
        "Upgrades already unlocked in the demo version: 96/188\nThere may be adjustments in the upgrade in the official version"
      },

      { "UI/GameOver/Quit", "Quit" },

      { "UI/FightReady/Difficulty", "Difficulty" },
      { "UI/FightReady/DifficultyNumber", "Level.{0}" },
      { "UI/FightReady/UnlockedDifficulty", "Unlocked {0}/{1}" },
      { "UI/FightReady/Mode", "Mode" },
      { "UI/FightReady/Map", "Map" },
      { "UI/FightReady/Description", "Description" },
      { "UI/FightReady/Casual", "Casual" },
      { "UI/FightReady/Standard", "Standard" },
      { "UI/FightReady/Infinite", "Infinite" },
      { "UI/FightReady/Forest", "Misty Forest" },
      { "UI/FightReady/Desert", "Scorching Desert" },
      { "UI/FightReady/Dungeon", "Dark Dungeon" },
      { "UI/FightReady/Graveyard", "Death Graveyard" },
      { "UI/FightReady/Hell", "Ultimate Hell" },
      {
        "UI/FightReady/ModeDescription/CasualMode",
        "The difficulty of monsters will be reduced in the Casual Mode."
      },
      { "UI/FightReady/ModeDescription/BossAppearTime", "The boss will appear after {0} minutes" },
      {
        "UI/FightReady/AttributeDescription",
        "Enemy Attribute Ratio: Physical - {0}%, Ice - {1}%, Fire - {2}%, Thunder - {3}%, Poison - {4}%"
      },

      { "UI/Prompt/UnlockRune", "Unlock rune <b>{0}</b> for {1}, continue?" },
      { "UI/Prompt/UnlockHero", "Unlock hero <b>{0}</b> for {1}, continue?" },
      { "UI/Info/Unlock/InsufficientBalance", "Sorry, insufficient balance to unlock. (Requirement: {0})" },
      { "UI/Info/Upgrade/InsufficientBalance", "Sorry, insufficient balance to upgrade." },
      { "UI/Info/RunePrerequisitesNotSatisfied", "Please unlock the rune of the previous level first." },

      { "UI/Audio/Overall", "Overall" },
      { "UI/Audio/Bgm", "BGM" },
      { "UI/Audio/Sfx", "SFX" },

      #endregion

      #region Property

      { "Physical", "Physical" },
      { "Ice", "Ice" },
      { "Thunder", "Thunder" },
      { "Fire", "Fire" },
      { "Poison", "Poison" },

      #endregion

      #region Heroes name

      { "Hero/Archangel", "Seraphiel" },
      { "Hero/CaptainG", "Bulletstorm" },
      { "Hero/Cutie", "Faye Spark" },
      { "Hero/Gumdam", "Comet Striker" },
      { "Hero/JeanneDArc", "Saint Jeanne" },
      { "Hero/MountainKing", "Thunderlord" },
      { "Hero/Paladin", "Iron Templar" },
      { "Hero/Ranger", "Venomstriker" },
      { "Hero/Witch", "Arcanist" },
      { "Hero/WuKong", "Sun Wukong" },
      { "Hero/ZhaoYun", "Zhao Yun" },

      #endregion

      #region Heroes description

      {
        "UI/Hero/Description/Archangel",
        "Seraphiel shoots icicles with a possibility to cause Freeze. She can recover some amount of health per second and is immune to Hell damage."
      },
      {
        "UI/Hero/Description/CaptainG",
        "Bulletstorm shoots 3 fire bullets with a possibility to cause Burning. Also, he is immune to Burn."
      },
      {
        "UI/Hero/Description/Cutie",
        "Faye Spark fires bullets with infinite penetration."
      },
      {
        "UI/Hero/Description/Gumdam",
        "Comet Striker fires 2 missiles from a bazooka that explodes, damaging all nearby enemies. But missiles may have a dead zone."
      },
      {
        "UI/Hero/Description/JeanneDArc",
        "Saint Jeanne attacks with her sword (Melee Attack), which can pierce through enemies. In addition, she does not need reload time."
      },
      {
        "UI/Hero/Description/MountainKing",
        "Thunderlord releases lightning bolts that may cause enemy Stun, and is also immune to Stun."
      },
      {
        "UI/Hero/Description/Paladin",
        "The Iron Templar can restore a small amount of health per second and is immune to the Hell damage."
      },
      {
        "UI/Hero/Description/Ranger",
        "Venomstriker fires 2 poisonous arrows that automatically circle the target. But arrows may have a dead zone."
      },
      {
        "UI/Hero/Description/Witch",
        "The Arcanist unleashes 2 balls of lightning to attack the enemy. She has one additional upgrade option while level up."
      },
      {
        "UI/Hero/Description/WuKong",
        "Sun Wukong can teleport to other places (press the button three times quickly to trigger)."
      },
      {
        "UI/Hero/Description/ZhaoYun",
        "Zhao Yun shoots 3 icy sword energies in all directions to attack the enemy. Also, he is immune to Freeze."
      },

      #endregion

      { "Level/Pause", "Paused" },
      { "Level/PressEscToResume", "Press Esc to resume" },

      #region Upgrade

      { "Upgrade/RecoverHp/Name", "Recover Lv{0}" },
      { "Upgrade/RecoverHp/Description", "Recover {0:F2}% of HP" },
      { "Upgrade/IncreaseMaxHp/Name", "MaxHP Lv{0}" },
      { "Upgrade/IncreaseMaxHp/Description", "Increase Max HP by {0:F2}%" },
      { "Upgrade/IncreaseMaxHpOnHurt/Name", "Dread Lv{0}" },
      {
        "Upgrade/IncreaseMaxHpOnHurt/Description",
        "Increase Max HP by {0:F2}% when the player is hurt. The maximum increment will be {1:F2}%."
      },
      { "Upgrade/IncreaseDefense/Name", "Shield Lv{0}" },
      { "Upgrade/IncreaseDefense/Description", "Increase defense by {0:F2}%" },

      { "Upgrade/IncreaseAllSpeed/Name", "Go Fast! Lv{0}" },
      {
        "Upgrade/IncreaseAllSpeed/Description", "Increase speed by {0:F2}% including normal speed and attacking speed."
      },
      { "Upgrade/IncreaseMovingSpeed/Name", "Running Shoe Lv{0}" },
      { "Upgrade/IncreaseMovingSpeed/Description", "Increase normal moving speed by {0:F2}%." },
      { "Upgrade/IncreaseAttackingMovingSpeed/Name", "Hit&Run Lv{0}" },
      { "Upgrade/IncreaseAttackingMovingSpeed/Description", "Increase attacking moving speed by {0:F2}%." },
      { "Upgrade/KillToIncreaseSpeed/Name", "Mercenary Lv{0}" },
      {
        "Upgrade/KillToIncreaseSpeed/Description",
        "Kill {0} enemies to increase {1:F2}% of speed and the maximum increment will be {2:F2}%. Speed increment will be reset after hurt."
      },

      { "Upgrade/ExpBonus/Name", "Nerd Lv{0}" },
      { "Upgrade/ExpBonus/Description", "Increases experience gained each time by {0:F2}%." },
      { "Upgrade/IncreasePickUp/Name", "Indiana Jones Lv{0}" },
      { "Upgrade/IncreasePickUp/Description", "Increase pick up radius by {0:F2}%." },
      { "Upgrade/IncreaseShootingRange/Name", "Barrett Lv{0}" },
      { "Upgrade/IncreaseShootingRange/Description", "Increase shooting range by {0:F2}%." },

      { "Upgrade/AddProjectile/Name", "Shotgun Lv{0}" },
      {
        "Upgrade/AddProjectile/Description",
        "Add {0} additional projectiles. However, the dispersion will increase slightly."
      },
      { "Upgrade/IncreaseDispersion/Name", "Scattering Lv{0}" },
      { "Upgrade/IncreaseDispersion/Description", "Increase dispersion by {0:F2}%." },
      { "Upgrade/ReduceDispersion/Name", "Sniper Rifle Lv{0}" },
      { "Upgrade/ReduceDispersion/Description", "Reduce dispersion by {0:F2}%." },
      { "Upgrade/IncreaseFiringRate/Name", "Rapid Fire Lv{0}" },
      { "Upgrade/IncreaseFiringRate/Description", "Increase firing rate by {0:F2}%." },
      { "Upgrade/BurstFire/Name", "Burst Fire Lv{0}" },
      { "Upgrade/BurstFire/Description", "Increases firing rate by {0:F2}% for {1:F2} seconds after taking damage." },
      { "Upgrade/IncreaseMagazineSize/Name", "Drum Magazine Lv{0}" },
      { "Upgrade/IncreaseMagazineSize/Description", "Increase magazine size by {0}." },
      { "Upgrade/ReduceReloadingTime/Name", "Quick Loader Lv{0}" },
      { "Upgrade/ReduceReloadingTime/Description", "Reduce reloading time by {0:F2}%." },
      { "Upgrade/BurstReload/Name", "Fear Loader Lv{0}" },
      { "Upgrade/BurstReload/Description", "Reduce reloading time by {0:F2}% for {1:F2} seconds after taking damage." },

      { "Upgrade/IncreaseHurtPercentage/Name", "Sharpening Lv{0}" },
      { "Upgrade/IncreaseHurtPercentage/Description", "Increase base skill hurt by {0:F2}%." },
      { "Upgrade/IncreaseIceAeDuration/Name", "Icy Lv{0}" },
      { "Upgrade/IncreaseIceAeDuration/Description", "Increase Frozen effect duration by {0:F2}%." },
      { "Upgrade/IncreaseFireAeDuration/Name", "Burn down Lv{0}" },
      { "Upgrade/IncreaseFireAeDuration/Description", "Increase Burning effect duration by {0:F2}%." },
      { "Upgrade/IncreaseThunderAeDuration/Name", "Thunderstorm Lv{0}" },
      { "Upgrade/IncreaseThunderAeDuration/Description", "Increase Stun effect duration by {0:F2}%." },

      { "Upgrade/ActivateDart/Name", "Ninja" },
      { "Upgrade/ActivateDart/Description", "Throws a dart that circles around you." },
      { "Upgrade/AddDart/Name", "Sensei Lv{0}" },
      { "Upgrade/AddDart/Description", "Throws {0} more darts around you." },
      { "Upgrade/PoisonDart/Name", "Poison Dart Lv{0}" },
      {
        "Upgrade/PoisonDart/Description",
        "Soak all darts in poison, giving them a {0:F2}% chance to poison the enemy and deal {1:F2}% damage per second (ineffective against bosses)."
      },
      { "Upgrade/ImproveDartHurt/Name", "Sharpened Lv{0}" },
      { "Upgrade/ImproveDartHurt/Description", "Sharpen all darts around you, improving {0:F2}% of hurts." },
      { "Upgrade/ActivateBoomerang/Name", "Aborigines" },
      { "Upgrade/ActivateBoomerang/Description", "Throws a boomerang that circles around you." },
      { "Upgrade/AddBoomerang/Name", "Manuel Schütz Lv{0}" },
      { "Upgrade/AddBoomerang/Description", "Throws {0} more boomerangs around you." },
      { "Upgrade/BurnBoomerang/Name", "Fire Boomerang Lv{0}" },
      {
        "Upgrade/BurnBoomerang/Description",
        "Gives the boomerang a {0:F2}% chance to set the enemy on fire for {1:F2} seconds, dealing {2:F2}% damage per second."
      },
      { "Upgrade/IncreaseBoomerangSpeed/Name", "Aerodynamics Lv{0}" },
      { "Upgrade/IncreaseBoomerangSpeed/Description", "Optimize aerodynamics to speed up the boomerang by {0:F2}%." },
      { "Upgrade/ImproveBoomerangHurt/Name", "Lead Filling Lv{0}" },
      {
        "Upgrade/ImproveBoomerangHurt/Description",
        "Charge the boomerang with lead, increasing its attack power by {0:F2}%"
      },

      { "Upgrade/ActivateIceTower/Name", "Frozen Tower" },
      {
        "Upgrade/ActivateIceTower/Description", "Fly an automatic Ice Tower around you that automatically fires hail."
      },
      { "Upgrade/AddIceTower/Name", "Winter Lv{0}" },
      { "Upgrade/AddIceTower/Description", "Add {0} more Ice Towers." },
      { "Upgrade/IncreaseIceTowerFiringRate/Name", "Skadi Lv{0}" },
      { "Upgrade/IncreaseIceTowerFiringRate/Description", "Increases the attack speed of Ice Tower by {0:F2}%." },
      { "Upgrade/ImproveIceTowerHurt/Name", "Ice Cube Lv{0}" },
      { "Upgrade/ImproveIceTowerHurt/Description", "Improve Ice Tower hurt by {0:F2}%." },
      { "Upgrade/ActivateFireTower/Name", "Fire Tower" },
      {
        "Upgrade/ActivateFireTower/Description",
        "Fly an automatic Fire Tower around you that automatically fires fire ball."
      },
      { "Upgrade/AddFireTower/Name", "Sultry Lv{0}" },
      { "Upgrade/AddFireTower/Description", "Add {0} more Fire Towers." },
      { "Upgrade/IncreaseFireTowerFiringRate/Name", "Apollo Lv{0}" },
      { "Upgrade/IncreaseFireTowerFiringRate/Description", "Increases the attack speed of Fire Tower by {0:F2}%." },
      { "Upgrade/ImproveFireTowerHurt/Name", "Flamethrower Lv{0}" },
      { "Upgrade/ImproveFireTowerHurt/Description", "Improve Fire Tower hurt by {0:F2}%." },
      { "Upgrade/ActivateThunderTower/Name", "Thunder Tower" },
      {
        "Upgrade/ActivateThunderTower/Description",
        "Fly an automatic Thunder Tower around you that automatically fires thunder."
      },
      { "Upgrade/AddThunderTower/Name", "Thunderbolt Lv{0}" },
      { "Upgrade/AddThunderTower/Description", "Add {0} more Thunder Towers." },
      { "Upgrade/IncreaseThunderTowerFiringRate/Name", "Thor Lv{0}" },
      {
        "Upgrade/IncreaseThunderTowerFiringRate/Description", "Increases the attack speed of Thunder Tower by {0:F2}%."
      },
      { "Upgrade/ImproveThunderTowerHurt/Name", "Tesla Coil" },
      { "Upgrade/ImproveThunderTowerHurt/Description", "Improve Thunder Tower hurt by {0:F2}%." },

      { "Upgrade/ActivateSpiral/Name", "Spiral" },
      {
        "Upgrade/ActivateSpiral/Description",
        "Every once in a while, the hero releases a spiral with random attributes to attack the enemy."
      },
      { "Upgrade/AddSpiral/Name", "Nautilidae Lv{0}" },
      { "Upgrade/AddSpiral/Description", "Add {0} more Spirals around you." },
      { "Upgrade/ReduceSpiralInterval/Name", "Rainstorm Lv{0}" },
      { "Upgrade/ReduceSpiralInterval/Description", "Reduce Spiral interval by {0:F2}%." },
      { "Upgrade/IncreaseSpiralHurt/Name", "Thunderstorm Lv{0}" },
      { "Upgrade/IncreaseSpiralHurt/Description", "Increase Spiral hurt by {0:F2}%." },

      { "Upgrade/ActivatePuppet/Name", "Puppet Fighter" },
      { "Upgrade/ActivatePuppet/Description", "Summon {0} puppets every {1:F2} seconds." },
      { "Upgrade/LevelUpPuppetToLv1/Name", "Armed Puppet" },
      {
        "Upgrade/LevelUpPuppetToLv1/Description",
        "Summon {0} level-upped puppets every {1:F2} seconds. However, the upgrades of current puppets are reset."
      },
      { "Upgrade/LevelUpPuppetToLv2/Name", "Saiya Puppet" },
      {
        "Upgrade/LevelUpPuppetToLv2/Description",
        "Summon {0} ultimate puppets every {1:F2} seconds. However, the upgrades of current puppets are reset."
      },
      { "Upgrade/ReducePuppetIntervalLv0/Name", "Team Lv{0}" },
      { "Upgrade/ReducePuppetIntervalLv0/Description", "Reduce puppet summon interval by {0:F2}%." },
      { "Upgrade/AddPuppetLv0/Name", "Shout Lv{0}" },
      {
        "Upgrade/AddPuppetLv0/Description",
        "Summon {0} more puppets for group fights."
      },
      { "Upgrade/IncreasePuppetHurtLv0/Name", "Knife Lv{0}" },
      { "Upgrade/IncreasePuppetHurtLv0/Description", "Increase all puppet hurt by {0:F2}%." },
      { "Upgrade/ReducePuppetIntervalLv1/Name", "Pager Lv{0}" },
      { "Upgrade/ReducePuppetIntervalLv1/Description", "Reduce puppet summon interval by {0:F2}%." },
      { "Upgrade/AddPuppetLv1/Name", "Mafia Lv{0}" },
      {
        "Upgrade/AddPuppetLv1/Description",
        "Summon {0} more puppets for group fights."
      },
      { "Upgrade/IncreasePuppetHurtLv1/Name", "Pistol Lv{0}" },
      { "Upgrade/IncreasePuppetHurtLv1/Description", "Increase all puppets hurt by {0:F2}%." },
      { "Upgrade/ReducePuppetIntervalLv2/Name", "Smartphone Lv{0}" },
      { "Upgrade/ReducePuppetIntervalLv2/Description", "Reduce puppet summon interval by {0:F2}%." },
      { "Upgrade/AddPuppetLv2/Name", "Army Lv{0}" },
      {
        "Upgrade/AddPuppetLv2/Description",
        "Summon {0} more puppets for group fights."
      },
      { "Upgrade/IncreasePuppetHurtLv2/Name", "Rifle Lv{0}" },
      { "Upgrade/IncreasePuppetHurtLv2/Description", "Increase all puppets hurt by {0:F2}%." },

      { "Upgrade/ActivateFlyingSword/Name", "Flying Sword" },
      {
        "Upgrade/ActivateFlyingSword/Description",
        "Use Qi to control swords, releasing {0} swords every {1:F2} seconds."
      },
      { "Upgrade/LevelUpFlyingSwordToLv1/Name", "Heavy Sword" },
      {
        "Upgrade/LevelUpFlyingSwordToLv1/Description",
        "Use heavier swords, releasing {0} swords every {1:F2} seconds. However, the upgrades of swords will be reset."
      },
      { "Upgrade/LevelUpFlyingSwordToLv2/Name", "Sharpened Sword" },
      {
        "Upgrade/LevelUpFlyingSwordToLv2/Description",
        "Use sharpened swords, releasing {0} swords every {1:F2} seconds. However, the upgrades of swords will be reset."
      },
      { "Upgrade/AddFlyingSwordLv0/Name", "Junior Lv{0}" },
      { "Upgrade/AddFlyingSwordLv0/Description", "Add {0} more flying swords." },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Name", "Apprentice Lv{0}" },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Description", "Reduce Swords releasing interval by {0:F2}%." },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Name", "Entry of Swordsman Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Description", "Increase Swords hurt by {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv1/Name", "Senior Lv{0}" },
      { "Upgrade/AddFlyingSwordLv1/Description", "Add {0} more flying swords." },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Name", "Artisan Lv{0}" },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Description", "Reduce Swords releasing interval by {0:F2}%." },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Name", "Senior of Swordsman Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Description", "Increase Swords hurt by {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv2/Name", "Shifu Lv{0}" },
      { "Upgrade/AddFlyingSwordLv2/Description", "Add {0} more flying swords." },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Name", "Sun Chaser Lv{0}" },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Description", "Reduce Swords releasing interval by {0:F2}%." },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Name", "Zongshi lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Description", "Increase Swords hurt by {0:F2}%." },

      { "Upgrade/ActivateMine/Name", "Landmine" },
      {
        "Upgrade/ActivateMine/Description",
        "Plant {0} mines that explodes and causes {1:F2} damage within a {2:F2} meter radius every {3:F2} seconds. Additionally, mines trigger the explosion of nearby mines."
      },
      { "Upgrade/LevelUpMineToLv1/Name", "Thunder Mine" },
      {
        "Upgrade/LevelUpMineToLv1/Description",
        "Plant {0} enhanced mines that explodes and causes {1:F2} damage within a {2:F2} meter radius every {3:F2} seconds."
      },
      { "Upgrade/LevelUpMineToLv2/Name", "Claymore Mine" },
      {
        "Upgrade/LevelUpMineToLv2/Description",
        "Plant {0} extremely lethal mines that explodes and causes {1:F2} damage within a {2:F2} meter radius every {3:F2} seconds."
      },
      { "Upgrade/AddMineLv0/Name", "Boy Scout Lv{0}" },
      { "Upgrade/AddMineLv0/Description", "Plant {0} more Landmines each time." },
      { "Upgrade/ReduceMineIntervalLv0/Name", "Pliers Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv0/Description", "Reduces the interval between mines by {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv0/Name", "Scratch Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv0/Description", "Increase Landmines hurt by {0:F2}%." },
      { "Upgrade/AddMineLv1/Name", "Minutemen Lv{0}" },
      { "Upgrade/AddMineLv1/Description", "Plant {0} more Thunder Mines each time." },
      { "Upgrade/ReduceMineIntervalLv1/Name", "Toolbox Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv1/Description", "Reduce the interval between mines by {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv1/Name", "Steel Ball Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv1/Description", "Increase Thunder Mines hurt by {0:F2}%." },
      { "Upgrade/AddMineLv2/Name", "Special Forces Lv{0}" },
      { "Upgrade/AddMineLv2/Description", "Plant {0} more Thunder Mines each time." },
      { "Upgrade/ReduceMineIntervalLv2/Name", "Mine System Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv2/Description", "Reduce the interval between mines by {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv2/Name", "Fragmentation Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv2/Description", "Increase Claymore Mines hurt by {0:F2}%." },

      #endregion

      #region Map

      { "MapIndicator/Forest", "Misty Forest" },
      { "MapIndicator/Desert", "Scorching Desert" },
      { "MapIndicator/Dungeon", "Dark Dungeon" },
      { "MapIndicator/Graveyard", "Death Graveyard" },
      { "MapIndicator/Hell", "Ultimate Hell" },

      {
        "MapDescription/Forest",
        "A foggy forest, where bullets often shoot out from nowhere in the seemingly peaceful trees."
      },
      {
        "MapDescription/Desert",
        "It seems that many monsters that can survive in the harsh desert are immune to attributes."
      },
      {
        "MapDescription/Dungeon",
        "In the dungeon, there are often the howling of wolves and the sound of hammers swinging."
      },
      {
        "MapDescription/Graveyard",
        "Watch out for the elusive skulls in the cemetery! You may get hurt if you run into them!"
      },
      {
        "MapDescription/Hell",
        "Hell is enchanted by the devil, causing more or less damage to heroes every 10 seconds! But it seems that some heroes don't care about this at all."
      },

      #endregion

      #region Additional Effect

      { "AdditionalEffect/Type/Freeze", "Freeze" },
      { "AdditionalEffect/Type/Stun", "Stun" },
      { "AdditionalEffect/Type/Burn", "Burn" },
      { "AdditionalEffect/Type/Poison", "Poison" },

      #endregion

      #region Gems

      { "Gem/GemSynthesis", "Gem Synthesis" },
      { "Gem/Rarity/R", "Rare" },
      { "Gem/Rarity/SR", "Super Rare" },
      { "Gem/Rarity/SSR", "Super Super Rare" },
      { "Gem/Rarity/XR", "Extremely Rare" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByPercentage", "Attacking Moving Speed {0}{1}%" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByValue", "Attacking Moving Speed {0}{1}" },
      { "Gem/Upgrade/AugmentMovingSpeedByPercentage", "Moving Speed {0}{1}%" },
      { "Gem/Upgrade/AugmentMovingSpeedByValue", "Moving Speed {0}{1}" },
      { "Gem/Upgrade/CoolingCountdownDecrementByPercentage", "Basic Skill Cooling Time {0}{1}%" },
      { "Gem/Upgrade/ReloadCountdownDecrementByPercentage", "Basic Skill Reload Time {0}{1}%" },
      { "Gem/Upgrade/DispersionDecrementByPercentage", "Basic Skill Dispersion {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByPercentage", "Basic Skill Penetration {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByValue", "Basic Skill Penetration {0}{1}" },
      { "Gem/Upgrade/RangeIncrementByPercentage", "Basic Skill Range {0}{1}%" },
      { "Gem/Upgrade/RangeIncrementByValue", "Basic Skill Range {0}{1}" },
      { "Gem/Upgrade/RepelForceIncrementByPercentage", "Basic Skill Repel Force {0}{1}%" },
      { "Gem/Upgrade/RepelForceIncrementByValue", "Basic Skill Repel Force {0}{1}" },
      { "Gem/Upgrade/SkillCountIncrementByValue", "Basic Skill projectile {0}{1}" },
      { "Gem/Upgrade/SpeedIncrementByPercentage", "Basic Skill Speed {0}{1}%" },
      { "Gem/Upgrade/SpeedIncrementByValue", "Basic Skill Speed {0}{1}" },
      { "Gem/Upgrade/EnableRangeAttack", "Enable range attack for basic skill" },
      { "Gem/Upgrade/AugmentDamageRangeByValue", "Basic Skill Damage Range {0}{1}" },
      { "Gem/Upgrade/AugmentDamageRangeByPercentage", "Basic Skill Damge Range {0}{1}%" },
      { "Gem/Upgrade/AddPoisoningAdditionalEffect", "Add Poisoning Additional Effect" },
      { "Gem/Upgrade/SetPoisoningPossibility", "Set Poisoning Possibility to {0}%" },
      { "Gem/Upgrade/SetPoisoningHurtPercentage", "Set Poisoning Hurt Percentage to {0}%" },
      { "Gem/Upgrade/AugmentPoisoningPossibility", "Skill Poisoning Possibility {0}{1}%" },
      { "Gem/Upgrade/AugmentPoisoningHurtPercentage", "Skill Poisoning Hurt Percentage {0}{1}%" },
      { "Gem/Upgrade/AddOrConvertBasicAdditionalEffect", "Add or convert Additional Effect Attribute to <b>{0}</b>" },
      { "Gem/Upgrade/SetBasicAdditionalEffectPossibility", "Set Skill Additional Effect Possibility to {0}%" },
      { "Gem/Upgrade/AugmentBasicAdditionalEffectPossibility", "Skill Additional Effect Possibility {0}{1}%" },
      { "Gem/Upgrade/AugmentPhysicalHurtByValue", "Physical Hurt {0}{1}" },
      { "Gem/Upgrade/AugmentPhysicalHurtByPercentage", "Physical Hurt {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicIceHurtByValue", "Ice Magic Hurt {0}{1}" },
      { "Gem/Upgrade/AugmentMagicIceHurtByPercentage", "Ice Magic Hurt {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicFireHurtByValue", "Fire Magic Hurt {0}{1}" },
      { "Gem/Upgrade/AugmentMagicFireHurtByPercentage", "Fire Magic Hurt {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicThunderByValue", "Thunder Magic Hurt {0}{1}" },
      { "Gem/Upgrade/Augment/MagicThunderByPercentage", "Thunder Magic Hurt {0}{1}%" },
      { "Gem/Upgrade/Augment/Resurrection", "Resurrection count {0}{1}" },
      { "Gem/Upgrade/Augment/IncreaseMaxHp", "Max Hp {0}{1}" },

      #endregion

      #region Runes

      { "Rune/Description/ClonedProjectile", "Increase the number of basic skills. Each upgrade adds a projectile." },
      {
        "Rune/Description/ExpBonus",
        "Increases experience gained by heroes by 10%. Experience gained by 7% each time the hero is leveled up."
      },
      {
        "Rune/Description/Fission",
        "After the hero's basic skills hit the enemy, there is a certain chance to split. The initial split probability is 10%, and it increases by 2% each time it is upgraded."
      },
      {
        "Rune/Description/HailStrike",
        "active skills. When triggered, summons hailstones to attack enemies within visible range. The skill lasts for 7 seconds and increases by 2 seconds with each upgrade."
      },
      {
        "Rune/Description/HolyShield",
        "active skills. After triggering, the hero gains 10 seconds of invincibility time, and each upgrade increases by 2 seconds."
      },
      {
        "Rune/Description/IncreaseHonor",
        "At the end of the game, the honor value earned by the player increases. The initial increase is 10%, and each upgrade increases by 10%."
      },
      {
        "Rune/Description/InstantKill",
        "When the hero's basic skills hit the enemy, there is a certain probability that the enemy will die instantly. The initial probability is 1%, and each upgrade increases by 0.5%."
      },
      {
        "Rune/Description/InstantReload",
        "There is a 10% chance to reload instantly when the ammo runs out. Each upgrade increases the chance by 5%."
      },
      {
        "Rune/Description/KillAndRecover",
        "active skills. Every time the hero kills an enemy within 10 seconds, the hero can recover 1% of his health. The duration of each upgrade is increased by 2 seconds."
      },
      {
        "Rune/Description/IncreaseImmortalTime",
        "Increases invulnerability time after taking damage. The initial state increases by 0.25 seconds, and each upgrade increases by 0.15 seconds."
      },
      {
        "Rune/Description/IncreaseMaxHp",
        "Increases the hero's maximum health by 10%, and increases by 5% for each upgrade."
      },
      {
        "Rune/Description/MeteoriteStrike",
        "active skills. Summons meteorites to attack enemies within visible range for 10 seconds. Initially, 10 meteorites fall per second, and each upgrade increases by 5."
      },
      {
        "Rune/Description/PickUpDistance",
        "The hero's pick-up distance increases by 10%, and each upgrade increases by 10%."
      },
      {
        "Rune/Description/Poisonous",
        "active skills. Deals poison damage to enemies within visible range every second for 10 seconds. Each level increases the duration by 2 seconds."
      },
      {
        "Rune/Description/PushAway",
        "Pushes away nearby enemies whenever the hero runs out of ammo (10 second cooldown). Each upgrade increases thrust by 20%."
      },
      {
        "Rune/Description/HpRecovery",
        "Recovers 0.2% hero HP per second, and increases the recovery amount by 0.2% per level."
      },
      {
        "Rune/Description/ReducedInjuery",
        "The hero's injury is reduced by 5%, and an additional 5% is reduced for each level."
      },
      {
        "Rune/Description/Resurrection",
        "When a hero dies, he is instantly revived with 25% of his health restored. After resurrection, HP increases by 15% each time it is upgraded."
      },
      {
        "Rune/Description/ThunderStrike",
        "active skills. Summon lightning to attack enemies within visible range with precision. The skill lasts for 10 seconds. Initially attack 10% of enemies per second, each upgrade will attack an additional 3% of enemies"
      },
      {
        "Rune/Description/TimeStop",
        "Pauses all enemy actions for 5 seconds. Each upgrade increases the length of the time pause by 2 seconds."
      },

      { "Rune/Title/ClonedProjectile", "Cloned Projectile" },
      { "Rune/Title/ExpBonus", "Book of Experience" },
      { "Rune/Title/Fission", "Fission" },
      { "Rune/Title/HailStrike", "Hail" },
      { "Rune/Title/HolyShield", "Holy Shield" },
      { "Rune/Title/IncreaseHonor", "Champion" },
      { "Rune/Title/InstantKill", "One Shot One Kill" },
      { "Rune/Title/InstantReload", "Multi Magazine" },
      { "Rune/Title/KillAndRecover", "Bloodthirsty" },
      { "Rune/Title/IncreaseImmortalTime", "Knight Helmet" },
      { "Rune/Title/IncreaseMaxHp", "Strong Arm" },
      { "Rune/Title/MeteoriteStrike", "Meteorite" },
      { "Rune/Title/PickUpDistance", "Bounty Hunter" },
      { "Rune/Title/Poisonous", "Gaz Zone" },
      { "Rune/Title/PushAway", "Leave Alone" },
      { "Rune/Title/HpRecovery", "Red Cross" },
      { "Rune/Title/ReducedInjuery", "Armored Knight" },
      { "Rune/Title/Resurrection", "Miracle" },
      { "Rune/Title/ThunderStrike", "Thunder Strike" },
      { "Rune/Title/TimeStop", "Time Machine" },

      #endregion

      #region Exception

      { "Exception/CorruptedSaveFile", "Save file is corrupted and has been backed up to {0}." },

      #endregion
    };
  }
}