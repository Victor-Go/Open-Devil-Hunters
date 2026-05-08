using System.Collections.Generic;

namespace Code.Scripts.Src.I18n
{
  public struct de : ILanguage
  {
    public static readonly Dictionary<string, string> locales = new()
    {
      { "HelloWorld", "Hallo Welt!" },
      { "Language", "Deutsch" },
      { "NotAvailableInDemo", "In Entwicklung" },
      { "Player1", "Spieler 1" },
      { "Player2", "Spieler 2" },
      {
        "Notification/WelcomeNotification",
        "Vielen Dank für den Kauf unseres Spiels!Wir wissen, dass unser Spiel nicht perfekt ist, deshalb brauchen wir dein Feedback!Bleib dran für kontinuierliche Updates."
      },
      { "Measure/PerSecond", "/sek" },
      {
        "UI/FightPreparationGuide/Page0/0",
        "Es gibt 5 Attribute für die Angriffe von Helden und Feinden, nämlich physisch, Eis, Feuer, Donner und Gift."
      },
      {
        "UI/FightPreparationGuide/Page0/1",
        "Unter ihnen schwächt das Eisattribut das Feuerattribut, das Feuerattribut das Donnerattribut und das Donnerattribut das Eisattribut."
      },
      {
        "UI/FightPreparationGuide/Page1/0",
        "Das Eisattribut kann den Gefriereffekt auslösen und numerischen Schaden verursachen;\nDas Feuerattribut kann den Entzündungseffekt auslösen und proportionalen Schaden verursachen;\nDas Donnerattribut kann den Betäubungseffekt auslösen, aber keinen zusätzlichen Schaden verursachen.\nAlle oben genannten Attribute werden nach einer gewissen Zeit eliminiert.\nDas Giftattribut kann eine Vergiftung auslösen und weiterhin proportionalen Schaden verursachen. Bosse können die Vergiftung jedoch oft lindern oder sogar aufheben."
      },
      {
        "UI/FightPreparationGuide/Page1/1",
        "Einige Gegner haben Attributverteidigung, Immunität gegen Attributeffekte und Durchbrechen der Attributverteidigung. Wenn du die Attribute, die den Gegner schwächen, gut einsetzt, kannst du den Gegner leichter besiegen!"
      },
      {
        "UI/FightReadyGuide/Page0/0",
        "Hier werden die Attribute der Monster auf der aktuellen Karte angezeigt. Merke dir diese Attribute und passe deine eigene Strategie an, um die Monster zu besiegen!"
      },
      {
        "UI/FightReadyGuide/Page1/0",
        "Diese beiden Positionen zeigen die Bosse, die auf der Karte sitzen. Jeder Boss hat unterschiedliche Kampffähigkeiten und -stile. Versuche, verschiedene Techniken anzuwenden, um verschiedene Bosse zu besiegen!"
      },
      {
        "UI/FightReadyGuide/Page1/1",
        "Vergiss nicht, die Karte öfter zu erkunden, wo du verschiedene Edelsteine sammeln kannst, um deine Helden zu stärken.Du kannst auch die Ehrenpunkte, die du in Kämpfen erhältst, verwenden, um neue Helden und Runen freizuschalten, um verschiedene Karten besser zu erkunden!"
      },
      { "UI/ControlGuide/ActiveSkill", "Aktive Fähigkeit" },
      { "UI/ControlGuide/AutoFiring", "Automatisches Feuern" },
      { "UI/ControlGuide/Movement", "Bewegung" },
      { "UI/ControlGuide/Fire!", "Feuer!" },
      { "UI/ControlGuide/AutoAiming", "Automatische Zielauswahl" },
      { "UI/ControlGuide/Aim", "Zielen" },
      {
        "UI/ControlGuide/MoveToUseController",
        "Bewege den Joystick, um mit dem Controller zu zielen (nicht im automatischen Zielmodus)."
      },
      {
        "UI/ControlGuide/MoveToUseMouse",
        "Bewege die Maus, um mit der Maus zu zielen (nicht im automatischen Zielmodus)."
      },
      { "UI/Control/AutoAiming", "Automatisches Zielen" },
      { "UI/Control/MouseAiming", "Mauszielen" },
      { "UI/Control/ControllerAiming", "Controller-Zielen" },
      { "UI/Control/AutoFiring", "Automatisches Feuern" },
      { "UI/Control/ManualFiring", "Manuelles Feuern" },
      { "UI/Control/UseMouseSelectHero", "Verwenden Sie die Maus, um einen Helden auszuwählen" },
      { "UI/Text/LevelUp!", "Level aufgestiegen!" },
      { "UI/Button/Choose", "Auswählen" },
      { "UI/Start", "Start" },
      { "UI/Languages", "Sprachen" },
      { "UI/Options", "Optionen" },
      { "UI/Exit", "Beenden" },
      { "UI/Name", "Name" },
      { "UI/Description", "Beschreibung" },
      { "UI/Properties", "Eigenschaften" },
      { "UI/Properties/AttacksPerRound", "Angriffe pro Runde" },
      { "UI/Properties/RateOfFire", "Feuerrate" },
      { "UI/Properties/ReloadTime", "Nachladezeit" },
      { "UI/Properties/Projectiles", "Projektile" },
      { "UI/Properties/FreezeDamagePerSecond", "Schaden pro Sekunde" },
      { "UI/Properties/MovingSpeed", "Bewegungsgeschwindigkeit" },
      { "UI/Properties/Physical", "Physisch" },
      { "UI/Properties/Ice", "Eis" },
      { "UI/Properties/Fire", "Feuer" },
      { "UI/Properties/Thunder", "Donner" },
      { "UI/Properties/Poisoning", "Vergiftung" },
      { "UI/GameOver", "Spiel vorbei" },
      { "UI/FinalScore", "Endstand" },
      { "UI/HonorGained", "Ehre erhalten" },
      { "UI/PlayerHonor", "Spielerehre" },
      { "UI/PlayerScore", "Spielergebnis" },
      { "UI/EnemyKilled", "Gegner getötet" },
      { "UI/ExperienceGained", "Erfahrung gesammelt" },
      { "UI/FinalLevel", "Endlevel" },
      { "UI/SurvivalTime", "Überlebenszeit" },
      { "UI/Upgrades", "Upgrades" },
      { "UI/GameOver/Failed", "NIEDERLAGE!" },
      { "UI/GameOver/Success", "LEVEL GESCHAFFT!" },
      { "UI/GameOver/Aborted", "MISSION ABGEBROCHEN" },
      { "UI/Achievements", "Erfolge" },
      { "UI/Furnace", "Schmiede" },
      { "UI/Gems", "Edelsteine" },
      { "UI/Runes", "Runen" },
      { "UI/Difficulty", "Schwierigkeitsgrad" },
      { "UI/Mode", "Spielmodus" },
      { "UI/CasualMode", "Casual-Modus" },
      { "UI/ChallengeMode", "Herausforderungsmodus" },
      { "UI/InfiniteMode", "Unendlichkeitsmodus" },
      { "UI/Play", "Spielen" },
      { "UI/Confirm", "Bestätigen" },
      { "UI/Cancel", "Abbrechen" },
      { "UI/GemList", "Edelsteinliste" },
      { "UI/HeroDescription/MaxHp", "Max. HP" },
      { "UI/HeroDescription/HpRecover", "HP-Erholung" },
      { "UI/HeroDescription/Resurrection", "Auferstehung" },
      { "UI/HeroDescription/BasicSkillHurt", "Grundfertigkeitsschaden" },
      { "UI/HeroDescription/BasicSkillProperty", "Grundfertigkeit Attribut" },
      { "UI/HeroDescription/Possibility", "AE-Möglichkeit" },
      { "UI/HeroDescription/AttackRange", "Angriffsreichweite" },
      { "UI/HeroDescription/AttackPerRound", "Angriffe pro Runde" },
      { "UI/HeroDescription/AttackSpeed", "Angriffsgeschwindigkeit" },
      { "UI/HeroDescription/PickUpRange", "Aufnahmereichweite" },
      { "UI/HeroDescription/MovingSpeed", "Bewegungsgeschwindigkeit" },
      { "UI/HeroDescription/AttackMovingSpeed", "Angriffsbewegung S." },
      { "UI/HeroDescription/BaseSkill", "Basisfähigkeit" },
      { "UI/HeroDescription/AdditionalEffect", "Zusätzlicher Effekt" },
      { "UI/HeroDescription/ProjectileCount", "Projektilanzahl" },
      { "UI/HeroDescription/AEHurt", "AE-Schaden" },
      { "UI/FightPreparation/TwoPlayersUniqueHero", "Zwei Spieler können nicht denselben Helden wählen" },
      { "UI/FightPreparation/BrowserHeroes", "Browser-Helden" },
      { "UI/FightPreparation/Back", "Zurück" },
      { "UI/FightPreparation/ApplyGems", "Edelsteine tragen" },
      { "UI/FightPreparation/ApplyRunes", "Runen aktivieren" },
      { "UI/FightPreparation/SelectHero", "Held auswählen" },
      { "UI/FightPreparation/Unlock", "Freischalten" },
      {
        "UI/FightPreparation/AddPlayer",
        "Drücke <color=red>A</color> auf dem Gamepad oder Strg rechts auf der Tastatur, um den 2. Spieler hinzuzufügen"
      },
      { "UI/FightPreparation/RemovePlayer", "Entfernen" },
      { "UI/PickGemUI/PickGemTitle", "Edelsteine tragen" },
      { "UI/PickGemUI/OneGemOnePlayer", "Ein Edelstein kann nur von einem Spieler verwendet werden" },
      { "UI/PickRuneUI/PickRuneTitle", "Runen aktivieren" },
      { "UI/PickRuneUI/Class", "Level" },
      { "UI/PickRuneUI/InspirationRune", "Inspirationsrune" },
      { "UI/PickRuneUI/DominationRune", "Dominanzrune" },
      { "UI/PickRuneUI/ImmortalRune", "Unsterblichkeitsrune" },
      { "UI/PickRuneUI/Upgrade", "Upgrade" },
      { "UI/PickRuneUI/Price", "Preis" },
      { "UI/PickRuneUI/OneRunePerClass", "Nur eine Rune pro Level wählbar" },
      { "UI/Pause/PhysicalAttack", "Physischer Angriff" },
      { "UI/Pause/IceAttack", "Eisangriff" },
      { "UI/Pause/FireAttack", "Feuerangriff" },
      { "UI/Pause/ThunderAttack", "Donnerangriff" },
      { "UI/Pause/Poisoning", "Vergiftung" },
      { "UI/Pause/MovingSpeed", "Bewegungsgeschwindigkeit" },
      { "UI/Pause/AttackingSpeed", "Geschwindigkeit beim Angreifen" },
      { "UI/Pause/AttackSpeed", "Angriffsgeschwindigkeit" },
      { "UI/Pause/RecoverSpeed", "Erholungsgeschwindigkeit" },
      { "UI/Pause/ExpBonus", "Erfahrungsbonus" },
      { "UI/Pause/RoundsPerSecond", " Runden/s" },
      { "UI/Pause/Resume", "Fortsetzen" },
      { "UI/Pause/ControlGuide", "Steuerungsanleitung" },
      { "UI/Pause/GiveUp", "Aufgeben" },
      {
        "UI/Upgrade/DemoUpgradeUnlocked",
        "Bereits freigeschaltete Upgrades in der Demoversion: 96/188In der offiziellen Version kann es Anpassungen am Upgrade geben"
      },
      { "UI/GameOver/Quit", "Beenden" },
      { "UI/FightReady/Difficulty", "Schwierigkeitsgrad" },
      { "UI/FightReady/DifficultyNumber", "Level.{0}" },
      { "UI/FightReady/UnlockedDifficulty", "Freigeschaltet {0}/{1}" },
      { "UI/FightReady/Mode", "Modus" },
      { "UI/FightReady/Map", "Karte" },
      { "UI/FightReady/Description", "Beschreibung" },
      { "UI/FightReady/Casual", "Casual" },
      { "UI/FightReady/Standard", "Standard" },
      { "UI/FightReady/Infinite", "Unendlich" },
      { "UI/FightReady/Forest", "Nebelwald" },
      { "UI/FightReady/Desert", "Versengende Wüste" },
      { "UI/FightReady/Dungeon", "Dunkler Dungeon" },
      { "UI/FightReady/Graveyard", "Todesfriedhof" },
      { "UI/FightReady/Hell", "Ultimative Hölle" },
      { "UI/FightReady/ModeDescription/CasualMode", "Die Schwierigkeit der Monster wird im Casual-Modus reduziert." },
      { "UI/FightReady/ModeDescription/BossAppearTime", "Der Boss erscheint nach {0} Minuten" },
      {
        "UI/FightReady/AttributeDescription",
        "Gegnerattribut-Verhältnis: Physisch - {0}%, Eis - {1}%, Feuer - {2}%, Donner - {3}%, Gift - {4}%"
      },
      { "UI/Prompt/UnlockRune", "Rune <b>{0}</b> für {1} freischalten, fortsetzen?" },
      { "UI/Prompt/UnlockHero", "Held <b>{0}</b> für {1} freischalten, fortsetzen?" },
      {
        "UI/Info/Unlock/InsufficientBalance",
        "Entschuldigung, unzureichendes Guthaben zum Freischalten. (Erforderlich: {0})"
      },
      { "UI/Info/Upgrade/InsufficientBalance", "Entschuldigung, Guthaben zum Upgraden unzureichend." },
      { "UI/Info/RunePrerequisitesNotSatisfied", "Bitte schalte zuerst die Rune des vorherigen Levels frei." },
      { "UI/Audio/Overall", "Gesamt" },
      { "UI/Audio/Bgm", "Hintergrundmusik" },
      { "UI/Audio/Sfx", "SFX" },
      { "Physical", "Physisch" },
      { "Ice", "Eis" },
      { "Thunder", "Donner" },
      { "Fire", "Feuer" },
      { "Poison", "Gift" },
      { "Hero/Archangel", "Seraphiel" },
      { "Hero/CaptainG", "Geschosshagel" },
      { "Hero/Cutie", "Faye Funke" },
      { "Hero/Gumdam", "Kometen-Angreifer" },
      { "Hero/JeanneDArc", "Heilige Jeanne" },
      { "Hero/MountainKing", "Donnerfürst" },
      { "Hero/Paladin", "Eiserner Templer" },
      { "Hero/Ranger", "Giftstachel" },
      { "Hero/Witch", "Arkane Meisterin" },
      { "Hero/WuKong", "Sun Wukong" },
      { "Hero/ZhaoYun", "Zhao Yun" },
      {
        "UI/Hero/Description/Archangel",
        "Seraphiel schießt Eiszapfen mit der Möglichkeit, Erfrieren zu verursachen. Sie kann jede Sekunde etwas Gesundheit wiederherstellen und ist immun gegen Höllenschaden."
      },
      {
        "UI/Hero/Description/CaptainG",
        "Geschosshagel schießt 3 Feuergeschosse mit der Möglichkeit, Verbrennung zu verursachen. Außerdem ist er immun gegen Verbrennung."
      },
      { "UI/Hero/Description/Cutie", "Faye Funke feuert Geschosse mit unendlicher Durchschlagskraft ab." },
      {
        "UI/Hero/Description/Gumdam",
        "Kometen-Angreifer feuert 2 Raketen von einer Bazooka ab, die explodieren und alle nahen Gegner schädigen. Raketen können jedoch eine tote Zone haben."
      },
      {
        "UI/Hero/Description/JeanneDArc",
        "Heilige Jeanne greift mit ihrem Schwert an (Nahkampfangriff), das Gegner durchdringen kann. Außerdem benötigt sie keine Nachladezeit."
      },
      {
        "UI/Hero/Description/MountainKing",
        "Donnerfürst entfesselt Blitze, die Gegner betäuben können, und ist auch immun gegen Betäubung."
      },
      {
        "UI/Hero/Description/Paladin",
        "Der Eiserne Templer kann jede Sekunde etwas Gesundheit wiederherstellen und ist immun gegen den Höllenschaden."
      },
      {
        "UI/Hero/Description/Ranger",
        "Giftstachel feuert 2 giftige Pfeile ab, die das Ziel automatisch umkreisen. Pfeile können jedoch eine tote Zone haben."
      },
      {
        "UI/Hero/Description/Witch",
        "Die Arkanistin entfesselt 2 Blitzkugeln, um den Feind anzugreifen. Sie hat eine zusätzliche Upgrade-Option beim Aufsteigen im Level."
      },
      {
        "UI/Hero/Description/WuKong",
        "Sun Wukong kann sich an andere Orte teleportieren (drücke die Taste dreimal schnell, um dies auszulösen)."
      },
      {
        "UI/Hero/Description/ZhaoYun",
        "Zhao Yun schießt 3 eisige Schwertenergien in alle Richtungen, um den Gegner anzugreifen. Außerdem ist er immun gegen Erfrieren."
      },
      { "Level/Pause", "Pausiert" },
      { "Level/PressEscToResume", "Drücke Esc, um fortzufahren" },
      { "Upgrade/RecoverHp/Name", "Erholung Lv{0}" },
      { "Upgrade/RecoverHp/Description", "Stellt {0:F2}% der HP wieder her" },
      { "Upgrade/IncreaseMaxHp/Name", "MaxHP Lv{0}" },
      { "Upgrade/IncreaseMaxHp/Description", "Erhöht die maximale HP um {0:F2}%" },
      { "Upgrade/IncreaseMaxHpOnHurt/Name", "Furcht Lv{0}" },
      {
        "Upgrade/IncreaseMaxHpOnHurt/Description",
        "Erhöht die maximale HP um {0:F2}%, wenn der Spieler verletzt wird. Die maximale Erhöhung beträgt {1:F2}%."
      },
      { "Upgrade/IncreaseDefense/Name", "Schild Lv{0}" },
      { "Upgrade/IncreaseDefense/Description", "Erhöht die Verteidigung um {0:F2}%" },
      { "Upgrade/IncreaseAllSpeed/Name", "Schnell! Lv{0}" },
      {
        "Upgrade/IncreaseAllSpeed/Description",
        "Erhöht die Geschwindigkeit um {0:F2}%, einschließlich der normalen Geschwindigkeit und der Angriffsgeschwindigkeit."
      },
      { "Upgrade/IncreaseMovingSpeed/Name", "Laufschuh Lv{0}" },
      { "Upgrade/IncreaseMovingSpeed/Description", "Erhöht die normale Bewegungsgeschwindigkeit um {0:F2}%." },
      { "Upgrade/IncreaseAttackingMovingSpeed/Name", "Treffer & Lauf Lv{0}" },
      {
        "Upgrade/IncreaseAttackingMovingSpeed/Description",
        "Erhöhen Sie die Bewegungsgeschwindigkeit während des Angriffs um {0:F2}%."
      },
      { "Upgrade/KillToIncreaseSpeed/Name", "Söldner Lv{0}" },
      {
        "Upgrade/KillToIncreaseSpeed/Description",
        "Töte {0} Gegner, um die Geschwindigkeit um {1:F2}% zu erhöhen, und die maximale Erhöhung beträgt {2:F2}%. Die Geschwindigkeitserhöhung wird nach einer Verletzung zurückgesetzt."
      },
      { "Upgrade/ExpBonus/Name", "Nerd Lv{0}" },
      { "Upgrade/ExpBonus/Description", "Erhöht die Erfahrung, die jedes Mal gewonnen wird, um {0:F2}%." },
      { "Upgrade/IncreasePickUp/Name", "Indiana Jones Lv{0}" },
      { "Upgrade/IncreasePickUp/Description", "Erhöht den Aufnahmeradius um {0:F2}%." },
      { "Upgrade/IncreaseShootingRange/Name", "Barrett Lv{0}" },
      { "Upgrade/IncreaseShootingRange/Description", "Erhöht die Schussreichweite um {0:F2}%." },
      { "Upgrade/AddProjectile/Name", "Schrotflinte Lv{0}" },
      {
        "Upgrade/AddProjectile/Description",
        "Fügt {0} zusätzliche Projektile hinzu. Die Streuung wird jedoch leicht zunehmen."
      },
      { "Upgrade/IncreaseDispersion/Name", "Streuung Lv{0}" },
      { "Upgrade/IncreaseDispersion/Description", "Erhöht die Streuung um {0:F2}%." },
      { "Upgrade/ReduceDispersion/Name", "Scharfschützengewehr Lv{0}" },
      { "Upgrade/ReduceDispersion/Description", "Reduziert die Streuung um {0:F2}%." },
      { "Upgrade/IncreaseFiringRate/Name", "Schnellfeuer Lv{0}" },
      { "Upgrade/IncreaseFiringRate/Description", "Erhöht die Feuerrate um {0:F2}%." },
      { "Upgrade/BurstFire/Name", "Feuerstoß Lv{0}" },
      {
        "Upgrade/BurstFire/Description", "Erhöht die Feuerrate um {0:F2}% für {1:F2} Sekunden nach erlittenem Schaden."
      },
      { "Upgrade/IncreaseMagazineSize/Name", "Trommelmagazin Lv{0}" },
      { "Upgrade/IncreaseMagazineSize/Description", "Erhöht die Magazingröße um {0}." },
      { "Upgrade/ReduceReloadingTime/Name", "Schnelllader Lv{0}" },
      { "Upgrade/ReduceReloadingTime/Description", "Reduziert die Nachladezeit um {0:F2}%." },
      { "Upgrade/BurstReload/Name", "Angstlader Lv{0}" },
      {
        "Upgrade/BurstReload/Description",
        "Reduziert die Nachladezeit um {0:F2}% für {1:F2} Sekunden nach erlittenem Schaden."
      },
      { "Upgrade/IncreaseHurtPercentage/Name", "Schärfen Lv{0}" },
      { "Upgrade/IncreaseHurtPercentage/Description", "Erhöht den Grundfertigkeitsschaden um {0:F2}%." },
      { "Upgrade/IncreaseIceAeDuration/Name", "Eisig Lv{0}" },
      { "Upgrade/IncreaseIceAeDuration/Description", "Erhöht die Dauer des Gefriereffekts um {0:F2}%." },
      { "Upgrade/IncreaseFireAeDuration/Name", "Niederbrennen Lv{0}" },
      { "Upgrade/IncreaseFireAeDuration/Description", "Erhöht die Dauer des Verbrennungseffekts um {0:F2}%." },
      { "Upgrade/IncreaseThunderAeDuration/Name", "Gewitter Lv{0}" },
      { "Upgrade/IncreaseThunderAeDuration/Description", "Erhöht die Dauer des Betäubungseffekts um {0:F2}%." },
      { "Upgrade/ActivateDart/Name", "Ninja" },
      { "Upgrade/ActivateDart/Description", "Wirft einen Pfeil, der dich umkreist." },
      { "Upgrade/AddDart/Name", "Sensei Lv{0}" },
      { "Upgrade/AddDart/Description", "Wirft {0} weitere Pfeile um dich herum." },
      { "Upgrade/PoisonDart/Name", "Giftpfeil Lv{0}" },
      {
        "Upgrade/PoisonDart/Description",
        "Tränkt alle Pfeile in Gift und gibt ihnen eine Chance von {0:F2}%, den Gegner zu vergiften und {1:F2}% Schaden pro Sekunde zu verursachen (wirkt nicht gegen Bosse)."
      },
      { "Upgrade/ImproveDartHurt/Name", "Geschärft Lv{0}" },
      {
        "Upgrade/ImproveDartHurt/Description",
        "Schärft alle Pfeile um dich herum und verbessert den Schaden um {0:F2}%."
      },
      { "Upgrade/ActivateBoomerang/Name", "Aborigines" },
      { "Upgrade/ActivateBoomerang/Description", "Wirft einen Bumerang, der dich umkreist." },
      { "Upgrade/AddBoomerang/Name", "Manuel Schütz Lv{0}" },
      { "Upgrade/AddBoomerang/Description", "Wirft {0} weitere Bumerangs um dich herum." },
      { "Upgrade/BurnBoomerang/Name", "Feuerbumerang Lv{0}" },
      {
        "Upgrade/BurnBoomerang/Description",
        "Gibt dem Bumerang eine Chance von {0:F2}%, den Gegner für {1:F2} Sekunden in Brand zu setzen und {2:F2}% Schaden pro Sekunde zu verursachen."
      },
      { "Upgrade/IncreaseBoomerangSpeed/Name", "Aerodynamik Lv{0}" },
      {
        "Upgrade/IncreaseBoomerangSpeed/Description",
        "Optimiert die Aerodynamik, um den Bumerang um {0:F2}% zu beschleunigen."
      },
      { "Upgrade/ImproveBoomerangHurt/Name", "Bleifüllung Lv{0}" },
      {
        "Upgrade/ImproveBoomerangHurt/Description",
        "Lädt den Bumerang mit Blei auf und erhöht seine Angriffskraft um {0:F2}%."
      },
      { "Upgrade/ActivateIceTower/Name", "Eisturm" },
      {
        "Upgrade/ActivateIceTower/Description",
        "Lässt einen automatischen Eisturm um dich herum fliegen, der automatisch Hagel feuert."
      },
      { "Upgrade/AddIceTower/Name", "Winter Lv{0}" },
      { "Upgrade/AddIceTower/Description", "Fügt {0} weitere Eistürme hinzu." },
      { "Upgrade/IncreaseIceTowerFiringRate/Name", "Skadi Lv{0}" },
      {
        "Upgrade/IncreaseIceTowerFiringRate/Description", "Erhöht die Angriffsgeschwindigkeit des Eisturms um {0:F2}%."
      },
      { "Upgrade/ImproveIceTowerHurt/Name", "Eiswürfel Lv{0}" },
      { "Upgrade/ImproveIceTowerHurt/Description", "Verbessert den Schaden des Eisturms um {0:F2}%." },
      { "Upgrade/ActivateFireTower/Name", "Feuerturm" },
      {
        "Upgrade/ActivateFireTower/Description",
        "Lässt einen automatischen Feuerturm um dich herum fliegen, der automatisch Feuerbälle feuert."
      },
      { "Upgrade/AddFireTower/Name", "Heiß Lv{0}" },
      { "Upgrade/AddFireTower/Description", "Fügt {0} weitere Feuertürme hinzu." },
      { "Upgrade/IncreaseFireTowerFiringRate/Name", "Apollo Lv{0}" },
      {
        "Upgrade/IncreaseFireTowerFiringRate/Description",
        "Erhöht die Angriffsgeschwindigkeit des Feuerturms um {0:F2}%."
      },
      { "Upgrade/ImproveFireTowerHurt/Name", "Flammenwerfer Lv{0}" },
      { "Upgrade/ImproveFireTowerHurt/Description", "Verbessert den Schaden des Feuerturms um {0:F2}%." },
      { "Upgrade/ActivateThunderTower/Name", "Donnerturm" },
      {
        "Upgrade/ActivateThunderTower/Description",
        "Lässt einen automatischen Donnerturm um dich herum fliegen, der automatisch Donner feuert."
      },
      { "Upgrade/AddThunderTower/Name", "Donnerblitz Lv{0}" },
      { "Upgrade/AddThunderTower/Description", "Fügt {0} weitere Donnertürme hinzu." },
      { "Upgrade/IncreaseThunderTowerFiringRate/Name", "Thor Lv{0}" },
      {
        "Upgrade/IncreaseThunderTowerFiringRate/Description",
        "Erhöht die Angriffsgeschwindigkeit des Donnerturms um {0:F2}%."
      },
      { "Upgrade/ImproveThunderTowerHurt/Name", "Teslaspule" },
      { "Upgrade/ImproveThunderTowerHurt/Description", "Verbessert den Schaden des Donnerturms um {0:F2}%." },
      { "Upgrade/ActivateSpiral/Name", "Spirale" },
      {
        "Upgrade/ActivateSpiral/Description",
        "Von Zeit zu Zeit setzt der Held eine Spirale mit zufälligen Attributen frei, um den Gegner anzugreifen."
      },
      { "Upgrade/AddSpiral/Name", "Nautilidae Lv{0}" },
      { "Upgrade/AddSpiral/Description", "Fügt {0} weitere Spiralen um dich herum hinzu." },
      { "Upgrade/ReduceSpiralInterval/Name", "Regenfall Lv{0}" },
      { "Upgrade/ReduceSpiralInterval/Description", "Reduziert das Spiralintervall um {0:F2}%." },
      { "Upgrade/IncreaseSpiralHurt/Name", "Gewitter Lv{0}" },
      { "Upgrade/IncreaseSpiralHurt/Description", "Erhöht den Schaden der Spirale um {0:F2}%." },
      { "Upgrade/ActivatePuppet/Name", "Puppenkämpfer" },
      { "Upgrade/ActivatePuppet/Description", "Beschwört alle {1:F2} Sekunden {0} Puppen." },
      { "Upgrade/LevelUpPuppetToLv1/Name", "Bewaffnete Puppe" },
      {
        "Upgrade/LevelUpPuppetToLv1/Description",
        "Beschwört alle {1:F2} Sekunden {0} Level-Up-Puppen. Die Upgrades der aktuellen Puppen werden jedoch zurückgesetzt."
      },
      { "Upgrade/LevelUpPuppetToLv2/Name", "Saiya-Puppe" },
      {
        "Upgrade/LevelUpPuppetToLv2/Description",
        "Beschwört alle {1:F2} Sekunden {0} ultimative Puppen. Die Upgrades der aktuellen Puppen werden jedoch zurückgesetzt."
      },
      { "Upgrade/ReducePuppetIntervalLv0/Name", "Team Lv{0}" },
      { "Upgrade/ReducePuppetIntervalLv0/Description", "Reduziert das Puppenbeschwörungsintervall um {0:F2}%." },
      { "Upgrade/AddPuppetLv0/Name", "Schrei Lv{0}" },
      { "Upgrade/AddPuppetLv0/Description", "Beschwöre {0} weitere Puppen für Gruppenkämpfe." },
      { "Upgrade/IncreasePuppetHurtLv0/Name", "Messer Lv{0}" },
      { "Upgrade/IncreasePuppetHurtLv0/Description", "Erhöht den Schaden aller Puppen um {0:F2}%." },
      { "Upgrade/ReducePuppetIntervalLv1/Name", "Pager Lv{0}" },
      { "Upgrade/ReducePuppetIntervalLv1/Description", "Reduziert das Puppenbeschwörungsintervall um {0:F2}%." },
      { "Upgrade/AddPuppetLv1/Name", "Mafia Lv{0}" },
      {
        "Upgrade/AddPuppetLv1/Description", "Beschwöre {0} weitere Puppen für Gruppenkämpfe."
      },
      { "Upgrade/IncreasePuppetHurtLv1/Name", "Pistole Lv{0}" },
      { "Upgrade/IncreasePuppetHurtLv1/Description", "Erhöht den Schaden aller Puppen um {0:F2}%." },
      { "Upgrade/ReducePuppetIntervalLv2/Name", "Smartphone Lv{0}" },
      { "Upgrade/ReducePuppetIntervalLv2/Description", "Reduziert das Puppenbeschwörungsintervall um {0:F2}%." },
      { "Upgrade/AddPuppetLv2/Name", "Armee Lv{0}" },
      {
        "Upgrade/AddPuppetLv2/Description", "Beschwöre {0} weitere Puppen für Gruppenkämpfe."
      },
      { "Upgrade/IncreasePuppetHurtLv2/Name", "Gewehr Lv{0}" },
      { "Upgrade/IncreasePuppetHurtLv2/Description", "Erhöht den Schaden aller Puppen um {0:F2}%." },
      { "Upgrade/ActivateFlyingSword/Name", "Fliegendes Schwert" },
      {
        "Upgrade/ActivateFlyingSword/Description",
        "Verwendet Qi, um Schwerter zu kontrollieren und alle {1:F2} Sekunden {0} Schwerter freizusetzen."
      },
      { "Upgrade/LevelUpFlyingSwordToLv1/Name", "Schweres Schwert" },
      {
        "Upgrade/LevelUpFlyingSwordToLv1/Description",
        "Verwendet schwerere Schwerter und setzt alle {1:F2} Sekunden {0} Schwerter frei. Die Upgrades der Schwerter werden jedoch zurückgesetzt."
      },
      { "Upgrade/LevelUpFlyingSwordToLv2/Name", "Geschärftes Schwert" },
      {
        "Upgrade/LevelUpFlyingSwordToLv2/Description",
        "Verwendet geschärfte Schwerter und setzt alle {1:F2} Sekunden {0} Schwerter frei. Die Upgrades der Schwerter werden jedoch zurückgesetzt."
      },
      { "Upgrade/AddFlyingSwordLv0/Name", "Junior Lv{0}" },
      { "Upgrade/AddFlyingSwordLv0/Description", "Fügt {0} weitere fliegende Schwerter hinzu." },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Name", "Lehrling Lv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv0/Description", "Reduziert das Intervall der Schwertfreisetzung um {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Name", "Einführung des Schwertkämpfers Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Description", "Erhöht den Schaden der Schwerter um {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv1/Name", "Senior Lv{0}" },
      { "Upgrade/AddFlyingSwordLv1/Description", "Fügt {0} weitere fliegende Schwerter hinzu." },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Name", "Handwerker Lv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv1/Description", "Reduziert das Intervall der Schwertfreisetzung um {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Name", "Senior des Schwertkämpfers Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Description", "Erhöht den Schaden der Schwerter um {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv2/Name", "Meister Lv{0}" },
      { "Upgrade/AddFlyingSwordLv2/Description", "Fügt {0} weitere fliegende Schwerter hinzu." },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Name", "Sonnenjäger Lv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv2/Description", "Reduziert das Intervall der Schwertfreisetzung um {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Name", "Zongshi Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Description", "Erhöht den Schaden der Schwerter um {0:F2}%." },
      { "Upgrade/ActivateMine/Name", "Landmine" },
      {
        "Upgrade/ActivateMine/Description",
        "Pflanzt alle {3:F2} Sekunden {0} Minen, die explodieren und innerhalb eines Radius von {2:F2} Metern {1:F2} Schaden verursachen. Zusätzlich lösen Minen die Explosion von Minen in der Nähe aus."
      },
      { "Upgrade/LevelUpMineToLv1/Name", "Donnermine" },
      {
        "Upgrade/LevelUpMineToLv1/Description",
        "Pflanzt alle {3:F2} Sekunden {0} verbesserte Minen, die explodieren und innerhalb eines Radius von {2:F2} Metern {1:F2} Schaden verursachen."
      },
      { "Upgrade/LevelUpMineToLv2/Name", "Claymore-Mine" },
      {
        "Upgrade/LevelUpMineToLv2/Description",
        "Pflanzt alle {3:F2} Sekunden {0} extrem tödliche Minen, die explodieren und innerhalb eines Radius von {2:F2} Metern {1:F2} Schaden verursachen."
      },
      { "Upgrade/AddMineLv0/Name", "Pfadfinder Lv{0}" },
      { "Upgrade/AddMineLv0/Description", "Pflanzt jedes Mal {0} weitere Landminen." },
      { "Upgrade/ReduceMineIntervalLv0/Name", "Zange Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv0/Description", "Reduziert das Intervall zwischen den Minen um {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv0/Name", "Kratzer Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv0/Description", "Erhöht den Schaden der Landminen um {0:F2}%." },
      { "Upgrade/AddMineLv1/Name", "Minutenmänner Lv{0}" },
      { "Upgrade/AddMineLv1/Description", "Pflanzt jedes Mal {0} weitere Donnerminen." },
      { "Upgrade/ReduceMineIntervalLv1/Name", "Werkzeugkasten Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv1/Description", "Reduziert das Intervall zwischen den Minen um {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv1/Name", "Stahlkugel Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv1/Description", "Erhöht den Schaden der Donnerminen um {0:F2}%." },
      { "Upgrade/AddMineLv2/Name", "Spezialeinheiten Lv{0}" },
      { "Upgrade/AddMineLv2/Description", "Pflanzt jedes Mal {0} weitere Donnerminen." },
      { "Upgrade/ReduceMineIntervalLv2/Name", "Minensystem Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv2/Description", "Reduziert das Intervall zwischen den Minen um {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv2/Name", "Splitterwirkung Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv2/Description", "Erhöht den Schaden der Claymore-Minen um {0:F2}%." },
      { "MapIndicator/Forest", "Nebelwald" },
      { "MapIndicator/Desert", "Versengende Wüste" },
      { "MapIndicator/Dungeon", "Dunkler Dungeon" },
      { "MapIndicator/Graveyard", "Todesfriedhof" },
      { "MapIndicator/Hell", "Ultimative Hölle" },
      {
        "MapDescription/Forest",
        "Ein nebeliger Wald, in dem Kugeln oft aus dem Nichts in den scheinbar friedlichen Bäumen geschossen kommen."
      },
      {
        "MapDescription/Desert",
        "Es scheint, dass viele Monster, die in der rauen Wüste überleben können, immun gegen Attribute sind."
      },
      {
        "MapDescription/Dungeon",
        "Im Dungeon hört man oft das Heulen von Wölfen und das Geräusch von schwingenden Hämmern."
      },
      {
        "MapDescription/Graveyard",
        "Achte auf die schwer fassbaren Schädel auf dem Friedhof! Du könntest verletzt werden, wenn du sie berührst!"
      },
      {
        "MapDescription/Hell",
        "Die Hölle ist vom Teufel verzaubert und verursacht alle 10 Sekunden mehr oder weniger Schaden an Helden! Aber es scheint, dass einige Helden das überhaupt nicht interessiert."
      },
      { "AdditionalEffect/Type/Freeze", "Erfrieren" },
      { "AdditionalEffect/Type/Stun", "Betäubung" },
      { "AdditionalEffect/Type/Burn", "Verbrennung" },
      { "AdditionalEffect/Type/Poison", "Vergiftung" },
      { "Gem/GemSynthesis", "Edelsteinsynthese" },
      { "Gem/Rarity/R", "Selten" },
      { "Gem/Rarity/SR", "Super selten" },
      { "Gem/Rarity/SSR", "Super super selten" },
      { "Gem/Rarity/XR", "Extrem selten" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByPercentage", "Angriffsbewegungsgeschwindigkeit {0}{1}%" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByValue", "Angriffsbewegungsgeschwindigkeit {0}{1}" },
      { "Gem/Upgrade/AugmentMovingSpeedByPercentage", "Bewegungsgeschwindigkeit {0}{1}%" },
      { "Gem/Upgrade/AugmentMovingSpeedByValue", "Bewegungsgeschwindigkeit {0}{1}" },
      { "Gem/Upgrade/CoolingCountdownDecrementByPercentage", "Grundfertigkeit Abklingzeit {0}{1}%" },
      { "Gem/Upgrade/ReloadCountdownDecrementByPercentage", "Grundfertigkeit Nachladezeit {0}{1}%" },
      { "Gem/Upgrade/DispersionDecrementByPercentage", "Grundfertigkeit Streuung {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByPercentage", "Grundfertigkeit Durchdringung {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByValue", "Grundfertigkeit Durchdringung {0}{1}" },
      { "Gem/Upgrade/RangeIncrementByPercentage", "Grundfertigkeit Reichweite {0}{1}%" },
      { "Gem/Upgrade/RangeIncrementByValue", "Grundfertigkeit Reichweite {0}{1}" },
      { "Gem/Upgrade/RepelForceIncrementByPercentage", "Grundfertigkeit Rückstoßkraft {0}{1}%" },
      { "Gem/Upgrade/RepelForceIncrementByValue", "Grundfertigkeit Rückstoßkraft {0}{1}" },
      { "Gem/Upgrade/SkillCountIncrementByValue", "Grundfertigkeit Projektil {0}{1}" },
      { "Gem/Upgrade/SpeedIncrementByPercentage", "Grundfertigkeit Geschwindigkeit {0}{1}%" },
      { "Gem/Upgrade/SpeedIncrementByValue", "Grundfertigkeit Geschwindigkeit {0}{1}" },
      { "Gem/Upgrade/EnableRangeAttack", "Aktiviere den Fernkampfangriff für die Grundfertigkeit" },
      { "Gem/Upgrade/AugmentDamageRangeByValue", "Grundfertigkeit Schadensreichweite {0}{1}" },
      { "Gem/Upgrade/AugmentDamageRangeByPercentage", "Grundfertigkeit Schadensreichweite {0}{1}%" },
      { "Gem/Upgrade/AddPoisoningAdditionalEffect", "Füge Vergiftung als zusätzlichen Effekt hinzu" },
      { "Gem/Upgrade/SetPoisoningPossibility", "Setze die Vergiftungsmöglichkeit auf {0}%" },
      { "Gem/Upgrade/SetPoisoningHurtPercentage", "Setze den Vergiftungsschadensprozentsatz auf {0}%" },
      { "Gem/Upgrade/AugmentPoisoningPossibility", "Fertigkeit Vergiftungsmöglichkeit {0}{1}%" },
      { "Gem/Upgrade/AugmentPoisoningHurtPercentage", "Fertigkeit Vergiftungsschadensprozentsatz {0}{1}%" },
      {
        "Gem/Upgrade/AddOrConvertBasicAdditionalEffect",
        "Füge den zusätzlichen Effekt hinzu oder wandle ihn in das Attribut <b>{0}</b> um"
      },
      {
        "Gem/Upgrade/SetBasicAdditionalEffectPossibility",
        "Setze die Fertigkeit Zusatzliche Effekt Möglichkeit auf {0}%"
      },
      { "Gem/Upgrade/AugmentBasicAdditionalEffectPossibility", "Fertigkeit Zusätzliche Effekt Möglichkeit {0}{1}%" },
      { "Gem/Upgrade/AugmentPhysicalHurtByValue", "Physischer Schaden {0}{1}" },
      { "Gem/Upgrade/AugmentPhysicalHurtByPercentage", "Physischer Schaden {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicIceHurtByValue", "Eismagieschaden {0}{1}" },
      { "Gem/Upgrade/AugmentMagicIceHurtByPercentage", "Eismagieschaden {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicFireHurtByValue", "Feuermagieschaden {0}{1}" },
      { "Gem/Upgrade/AugmentMagicFireHurtByPercentage", "Feuermagieschaden {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicThunderByValue", "Donnermagieschaden {0}{1}" },
      { "Gem/Upgrade/Augment/MagicThunderByPercentage", "Donnermagieschaden {0}{1}%" },
      { "Gem/Upgrade/Augment/Resurrection", "Anzahl der Wiederbelebungen {0}{1}" },
      { "Gem/Upgrade/Augment/IncreaseMaxHp", "Max. HP {0}{1}" },
      {
        "Rune/Description/ClonedProjectile",
        "Erhöht die Anzahl der Grundfertigkeiten. Jedes Upgrade fügt ein Projektil hinzu."
      },
      {
        "Rune/Description/ExpBonus",
        "Erhöht die Erfahrung, die Helden erhalten, um 10%. Erfahrung, die mit jedem Levelaufstieg des Helden um 7% gewonnen wird."
      },
      {
        "Rune/Description/Fission",
        "Nachdem die Grundfertigkeiten des Helden den Gegner getroffen haben, besteht eine gewisse Chance, sich zu teilen. Die anfängliche Teilungswahrscheinlichkeit beträgt 10% und erhöht sich mit jedem Upgrade um 2%."
      },
      {
        "Rune/Description/HailStrike",
        "aktive Fähigkeiten. Beim Auslösen werden Hagelkörner beschworen, um Gegner in Sichtweite anzugreifen. Die Fähigkeit hält 7 Sekunden an und verlängert sich mit jedem Upgrade um 2 Sekunden."
      },
      {
        "Rune/Description/HolyShield",
        "aktive Fähigkeiten. Nach dem Auslösen erhält der Held 10 Sekunden Unverwundbarkeit, und jedes Upgrade verlängert diese um 2 Sekunden."
      },
      {
        "Rune/Description/IncreaseHonor",
        "Am Ende des Spiels erhöht sich der vom Spieler verdiente Ehrenwert. Die anfängliche Erhöhung beträgt 10 %, und jedes Upgrade erhöht sie um 10 %."
      },
      {
        "Rune/Description/InstantKill",
        "Wenn die Grundfertigkeiten des Helden den Gegner treffen, besteht eine gewisse Wahrscheinlichkeit, dass der Gegner sofort stirbt. Die anfängliche Wahrscheinlichkeit beträgt 1 %, und jedes Upgrade erhöht sie um 0,5 %."
      },
      {
        "Rune/Description/InstantReload",
        "Es besteht eine Chance von 10 %, sofort nachzuladen, wenn die Munition zur Neige geht. Jedes Upgrade erhöht die Chance um 5 %."
      },
      {
        "Rune/Description/KillAndRecover",
        "aktive Fähigkeiten. Jedes Mal, wenn der Held innerhalb von 10 Sekunden einen Gegner tötet, kann er 1 % seiner Gesundheit wiederherstellen. Die Dauer jedes Upgrades wird um 2 Sekunden verlängert."
      },
      {
        "Rune/Description/IncreaseImmortalTime",
        "Erhöht die Unverwundbarkeitszeit nach erlittenem Schaden. Der Anfangszustand erhöht sich um 0,25 Sekunden, und jedes Upgrade erhöht ihn um 0,15 Sekunden."
      },
      {
        "Rune/Description/IncreaseMaxHp",
        "Erhöht die maximale Gesundheit des Helden um 10 % und um 5 % für jedes Upgrade."
      },
      {
        "Rune/Description/MeteoriteStrike",
        "aktive Fähigkeiten. Beschwört Meteoriten, um Gegner in Sichtweite für 10 Sekunden anzugreifen. Anfangs fallen 10 Meteoriten pro Sekunde, und jedes Upgrade erhöht diese Anzahl um 5."
      },
      {
        "Rune/Description/PickUpDistance",
        "Die Aufnahmedistanz des Helden erhöht sich um 10 %, und jedes Upgrade erhöht sie um 10 %."
      },
      {
        "Rune/Description/Poisonous",
        "aktive Fähigkeiten. Verursacht 10 Sekunden lang jede Sekunde Giftschaden an Gegnern in Sichtweite. Jedes Level erhöht die Dauer um 2 Sekunden."
      },
      {
        "Rune/Description/PushAway",
        "Stößt nahe Gegner weg, wenn dem Helden die Munition ausgeht (10 Sekunden Abklingzeit). Jedes Upgrade erhöht den Stoß um 20 %."
      },
      {
        "Rune/Description/HpRecovery",
        "Stellt 0,2 % der Helden-HP pro Sekunde wieder her, und erhöht die Wiederherstellungsmenge um 0,2 % pro Level."
      },
      {
        "Rune/Description/ReducedInjuery",
        "Der Schaden, den der Held erleidet, wird um 5% reduziert, und um weitere 5% pro Level."
      },
      {
        "Rune/Description/Resurrection",
        "Wenn ein Held stirbt, wird er sofort mit 25 % seiner Gesundheit wiederbelebt. Nach der Wiederbelebung steigt die HP mit jedem Upgrade um 15 %."
      },
      {
        "Rune/Description/ThunderStrike",
        "aktive Fähigkeiten. Beschwört Blitze, um Gegner in Sichtweite präzise anzugreifen. Die Fähigkeit hält 10 Sekunden an. Anfangs werden 10 % der Gegner pro Sekunde angegriffen, jedes Upgrade greift zusätzlich 3 % der Gegner an."
      },
      {
        "Rune/Description/TimeStop",
        "Pausiert alle Aktionen des Gegners für 5 Sekunden. Jedes Upgrade verlängert die Dauer der Zeitpause um 2 Sekunden."
      },
      { "Rune/Title/ClonedProjectile", "Geklontes Projektil" },
      { "Rune/Title/ExpBonus", "Buch der Erfahrung" },
      { "Rune/Title/Fission", "Spaltung" },
      { "Rune/Title/HailStrike", "Hagel" },
      { "Rune/Title/HolyShield", "Heiliger Schild" },
      { "Rune/Title/IncreaseHonor", "Champion" },
      { "Rune/Title/InstantKill", "Ein Schuss, ein Kill" },
      { "Rune/Title/InstantReload", "Mehrfachmagazin" },
      { "Rune/Title/KillAndRecover", "Blutdurstig" },
      { "Rune/Title/IncreaseImmortalTime", "Ritterhelm" },
      { "Rune/Title/IncreaseMaxHp", "Starker Arm" },
      { "Rune/Title/MeteoriteStrike", "Meteorit" },
      { "Rune/Title/PickUpDistance", "Kopfgeldjäger" },
      { "Rune/Title/Poisonous", "Gaszone" },
      { "Rune/Title/PushAway", "In Ruhe lassen" },
      { "Rune/Title/HpRecovery", "Rotes Kreuz" },
      { "Rune/Title/ReducedInjuery", "Gepanzerter Ritter" },
      { "Rune/Title/Resurrection", "Wunder" },
      { "Rune/Title/ThunderStrike", "Blitzschlag" },
      { "Rune/Title/TimeStop", "Zeitmaschine" },
      { "Exception/CorruptedSaveFile", "Die Speicherdatei ist beschädigt und wurde in {0} gesichert." },
    };
  }
}