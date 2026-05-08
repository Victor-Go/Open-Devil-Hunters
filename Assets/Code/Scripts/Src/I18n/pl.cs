using System.Collections.Generic;

namespace Code.Scripts.Src.I18n
{
  public struct pl : ILanguage
  {
    public static readonly Dictionary<string, string> locales = new()
    {
      { "HelloWorld", "Witaj Świecie!" },
      { "Language", "Polski" },
      { "NotAvailableInDemo", "W trakcie tworzenia" },
      { "Player1", "Gracz 1" },
      { "Player2", "Gracz 2" },
      {
        "Notification/WelcomeNotification",
        "Dziękujemy za zakup naszej gry!Wiemy, że nasza gra nie jest idealna, dlatego potrzebujemy Twojej opinii!Bądź na bieżąco z ciągłymi aktualizacjami."
      },
      { "Measure/PerSecond", "/sek" },
      {
        "UI/FightPreparationGuide/Page0/0",
        "Ataki bohaterów i wrogów mają 5 atrybutów: fizyczny, lód, ogień, błyskawica i trucizna."
      },
      {
        "UI/FightPreparationGuide/Page0/1",
        "Wśród nich atrybut lodu osłabia atrybut ognia, atrybut ognia osłabia atrybut błyskawicy, a atrybut błyskawicy osłabia atrybut lodu."
      },
      {
        "UI/FightPreparationGuide/Page1/0",
        "Atrybut lodu może wywołać efekt zamrożenia i spowodować obrażenia numeryczne;\nAtrybut ognia może wywołać efekt podpalenia i spowodować obrażenia proporcjonalne;\nAtrybut błyskawicy może wywołać efekt ogłuszenia, ale nie spowoduje dodatkowych obrażeń.\nWszystkie powyższe atrybuty zostaną wyeliminowane po pewnym czasie.\nAtrybut trucizny może wywołać zatrucie i nadal powodować obrażenia proporcjonalne. Jednak bossowie często mogą złagodzić, a nawet usunąć zatrucie."
      },
      {
        "UI/FightPreparationGuide/Page1/1",
        "Niektórzy wrogowie mają obronę atrybutów, odporność na efekty atrybutów i przełamanie obrony atrybutów. Dobre wykorzystanie atrybutów, które ograniczają wroga, może ułatwić pokonanie przeciwnika!"
      },
      {
        "UI/FightReadyGuide/Page0/0",
        "Tutaj zostaną wyświetlone atrybuty potworów na bieżącej mapie. Zapamiętaj te atrybuty i dopasuj własną strategię, aby pokonać potwory!"
      },
      {
        "UI/FightReadyGuide/Page1/0",
        "Te dwie pozycje pokażą bossów siedzących na mapie. Każdy boss ma inne umiejętności i style walki. Spróbuj użyć różnych technik, aby pokonać różnych bossów!"
      },
      {
        "UI/FightReadyGuide/Page1/1",
        "Nie zapomnij częściej eksplorować mapę, gdzie możesz zbierać różne klejnoty, aby wzmocnić swoich bohaterów.Możesz także użyć honorów, które zdobędziesz w bitwach, aby odblokować nowych bohaterów i runy, aby lepiej eksplorować różne mapy!"
      },
      { "UI/ControlGuide/ActiveSkill", "Umiejętność aktywna" },
      { "UI/ControlGuide/AutoFiring", "Automatyczny ostrzał" },
      { "UI/ControlGuide/Movement", "Ruch" },
      { "UI/ControlGuide/Fire!", "Ogień!" },
      { "UI/ControlGuide/AutoAiming", "Automatyczne celowanie" },
      { "UI/ControlGuide/Aim", "Celowanie" },
      {
        "UI/ControlGuide/MoveToUseController",
        "Poruszaj joystickiem, aby użyć celowania kontrolerem (poza trybem automatycznego celowania)."
      },
      {
        "UI/ControlGuide/MoveToUseMouse",
        "Poruszaj myszą, aby użyć celowania myszą (poza trybem automatycznego celowania)."
      },
      { "UI/Control/AutoAiming", "Automatyczne celowanie" },
      { "UI/Control/MouseAiming", "Celowanie myszą" },
      { "UI/Control/ControllerAiming", "Celowanie kontrolerem" },
      { "UI/Control/AutoFiring", "Automatyczny ogień" },
      { "UI/Control/ManualFiring", "Manualny ogień" },
      { "UI/Control/UseMouseSelectHero", "Użyj myszy, aby wybrać bohatera" },
      { "UI/Text/LevelUp!", "Poziom w górę!" },
      { "UI/Button/Choose", "Wybierz" },
      { "UI/Start", "Start" },
      { "UI/Languages", "Języki" },
      { "UI/Options", "Opcje" },
      { "UI/Exit", "Wyjście" },
      { "UI/Name", "Nazwa" },
      { "UI/Description", "Opis" },
      { "UI/Properties", "Właściwości" },
      { "UI/Properties/AttacksPerRound", "Ataki na rundę" },
      { "UI/Properties/RateOfFire", "Szybkość ognia" },
      { "UI/Properties/ReloadTime", "Czas przeładowania" },
      { "UI/Properties/Projectiles", "Pociski" },
      { "UI/Properties/FreezeDamagePerSecond", "Obrażenia na sekundę" },
      { "UI/Properties/MovingSpeed", "Prędkość poruszania się" },
      { "UI/Properties/Physical", "Fizyczny" },
      { "UI/Properties/Ice", "Lód" },
      { "UI/Properties/Fire", "Ogień" },
      { "UI/Properties/Thunder", "Błyskawica" },
      { "UI/Properties/Poisoning", "Zatrucie" },
      { "UI/GameOver", "Koniec gry" },
      { "UI/FinalScore", "Wynik końcowy" },
      { "UI/HonorGained", "Zdobyty honor" },
      { "UI/PlayerHonor", "Honor gracza" },
      { "UI/PlayerScore", "Wynik gracza" },
      { "UI/EnemyKilled", "Zabity wróg" },
      { "UI/ExperienceGained", "Zdobyte doświadczenie" },
      { "UI/FinalLevel", "Poziom końcowy" },
      { "UI/SurvivalTime", "Czas przetrwania" },
      { "UI/Upgrades", "Ulepszenia" },
      { "UI/GameOver/Failed", "PORAŻKA!" },
      { "UI/GameOver/Success", "POZIOM UKOŃCZONY!" },
      { "UI/GameOver/Aborted", "MISJA PRZERWANA" },
      { "UI/Achievements", "Osiągnięcia" },
      { "UI/Furnace", "Piec" },
      { "UI/Gems", "Klejnoty" },
      { "UI/Runes", "Runy" },
      { "UI/Difficulty", "Trudność" },
      { "UI/Mode", "Tryb gry" },
      { "UI/CasualMode", "Tryb casualowy" },
      { "UI/ChallengeMode", "Tryb wyzwania" },
      { "UI/InfiniteMode", "Tryb nieskończony" },
      { "UI/Play", "Graj" },
      { "UI/Confirm", "Potwierdź" },
      { "UI/Cancel", "Anuluj" },
      { "UI/GemList", "Lista klejnotów" },
      { "UI/HeroDescription/MaxHp", "Maks. HP" },
      { "UI/HeroDescription/HpRecover", "Odzyskiwanie HP" },
      { "UI/HeroDescription/Resurrection", "Wskrzeszenie" },
      { "UI/HeroDescription/BasicSkillHurt", "Obrażenia umiejętności podstawowej" },
      { "UI/HeroDescription/BasicSkillProperty", "Atrybut umiejętności podstawowej" },
      { "UI/HeroDescription/Possibility", "Możliwość AE" },
      { "UI/HeroDescription/AttackRange", "Zasięg ataku" },
      { "UI/HeroDescription/AttackPerRound", "Ataki na rundę" },
      { "UI/HeroDescription/AttackSpeed", "Szybkość ataku" },
      { "UI/HeroDescription/PickUpRange", "Zasięg podnoszenia" },
      { "UI/HeroDescription/MovingSpeed", "Prędkość poruszania się" },
      { "UI/HeroDescription/AttackMovingSpeed", "Prędkość poruszania się podczas ataku" },
      { "UI/HeroDescription/BaseSkill", "Umiejętność bazowa" },
      { "UI/HeroDescription/AdditionalEffect", "Efekt dodatkowy" },
      { "UI/HeroDescription/ProjectileCount", "Liczba pocisków" },
      { "UI/HeroDescription/AEHurt", "Obrażenia AE" },
      { "UI/FightPreparation/TwoPlayersUniqueHero", "Dwóch graczy nie może wybrać tego samego bohatera" },
      { "UI/FightPreparation/BrowserHeroes", "Bohaterowie przeglądarki" },
      { "UI/FightPreparation/Back", "Powrót" },
      { "UI/FightPreparation/ApplyGems", "Noś klejnoty" },
      { "UI/FightPreparation/ApplyRunes", "Aktywuj runy" },
      { "UI/FightPreparation/SelectHero", "Wybierz bohatera" },
      { "UI/FightPreparation/Unlock", "Odblokuj" },
      {
        "UI/FightPreparation/AddPlayer",
        "Naciśnij <color=red>A</color> na gamepadzie lub Prawy Ctrl na klawiaturze, aby dodać 2 gracza"
      },
      { "UI/FightPreparation/RemovePlayer", "Usuń" },
      { "UI/PickGemUI/PickGemTitle", "Noś klejnoty" },
      { "UI/PickGemUI/OneGemOnePlayer", "Jeden klejnot może być używany tylko przez jednego gracza" },
      { "UI/PickRuneUI/PickRuneTitle", "Aktywuj runy" },
      { "UI/PickRuneUI/Class", "Poziom" },
      { "UI/PickRuneUI/InspirationRune", "Runa inspiracji" },
      { "UI/PickRuneUI/DominationRune", "Runa dominacji" },
      { "UI/PickRuneUI/ImmortalRune", "Runa nieśmiertelności" },
      { "UI/PickRuneUI/Upgrade", "Ulepsz" },
      { "UI/PickRuneUI/Price", "Cena" },
      { "UI/PickRuneUI/OneRunePerClass", "Tylko jedna runa może być wybrana na poziom" },
      { "UI/Pause/PhysicalAttack", "Atak fizyczny" },
      { "UI/Pause/IceAttack", "Atak lodu" },
      { "UI/Pause/FireAttack", "Atak ognia" },
      { "UI/Pause/ThunderAttack", "Atak błyskawic" },
      { "UI/Pause/Poisoning", "Zatrucie" },
      { "UI/Pause/MovingSpeed", "Prędkość poruszania się" },
      { "UI/Pause/AttackingSpeed", "Prędkość podczas ataku" },
      { "UI/Pause/AttackSpeed", "Szybkość ataku" },
      { "UI/Pause/RecoverSpeed", "Szybkość odzyskiwania" },
      { "UI/Pause/ExpBonus", "Bonus doświadczenia" },
      { "UI/Pause/RoundsPerSecond", " rund/s" },
      { "UI/Pause/Resume", "Wznów" },
      { "UI/Pause/ControlGuide", "Przewodnik sterowania" },
      { "UI/Pause/GiveUp", "Poddaj się" },
      {
        "UI/Upgrade/DemoUpgradeUnlocked",
        "Ulepszenia odblokowane w wersji demo: 96/188W oficjalnej wersji mogą wystąpić zmiany w ulepszeniach"
      },
      { "UI/GameOver/Quit", "Wyjdź" },
      { "UI/FightReady/Difficulty", "Trudność" },
      { "UI/FightReady/DifficultyNumber", "Poziom.{0}" },
      { "UI/FightReady/UnlockedDifficulty", "Odblokowano {0}/{1}" },
      { "UI/FightReady/Mode", "Tryb" },
      { "UI/FightReady/Map", "Mapa" },
      { "UI/FightReady/Description", "Opis" },
      { "UI/FightReady/Casual", "Casual" },
      { "UI/FightReady/Standard", "Standardowy" },
      { "UI/FightReady/Infinite", "Nieskończony" },
      { "UI/FightReady/Forest", "Mglisty Las" },
      { "UI/FightReady/Desert", "Spalona Pustynia" },
      { "UI/FightReady/Dungeon", "Mroczny Loch" },
      { "UI/FightReady/Graveyard", "Cmentarz Śmierci" },
      { "UI/FightReady/Hell", "Piekło Ostateczne" },
      { "UI/FightReady/ModeDescription/CasualMode", "Trudność potworów zostanie zmniejszona w Trybie Casual." },
      { "UI/FightReady/ModeDescription/BossAppearTime", "Boss pojawi się po {0} minutach" },
      {
        "UI/FightReady/AttributeDescription",
        "Współczynnik atrybutów wroga: Fizyczny - {0}%, Lód - {1}%, Ogień - {2}%, Błyskawica - {3}%, Trucizna - {4}%"
      },
      { "UI/Prompt/UnlockRune", "Odblokować runę <b>{0}</b> za {1}, kontynuować?" },
      { "UI/Prompt/UnlockHero", "Odblokować bohatera <b>{0}</b> za {1}, kontynuować?" },
      {
        "UI/Info/Unlock/InsufficientBalance", "Przepraszamy, niewystarczające środki, aby odblokować. (Wymagane: {0})"
      },
      { "UI/Info/Upgrade/InsufficientBalance", "Przepraszam, niewystarczające środki, aby ulepszyć." },
      { "UI/Info/RunePrerequisitesNotSatisfied", "Proszę najpierw odblokować runę z poprzedniego poziomu." },
      { "UI/Audio/Overall", "Ogólne" },
      { "UI/Audio/Bgm", "BGM" },
      { "UI/Audio/Sfx", "SFX" },
      { "Physical", "Fizyczny" },
      { "Ice", "Lód" },
      { "Thunder", "Błyskawica" },
      { "Fire", "Ogień" },
      { "Poison", "Trucizna" },
      { "Hero/Archangel", "Seraphiel" },
      { "Hero/CaptainG", "Burza pocisków" },
      { "Hero/Cutie", "Faye Spark" },
      { "Hero/Gumdam", "Uderzający kometą" },
      { "Hero/JeanneDArc", "Święta Joanna" },
      { "Hero/MountainKing", "Władca piorunów" },
      { "Hero/Paladin", "Żelazny Templariusz" },
      { "Hero/Ranger", "Jadowity Uderzacz" },
      { "Hero/Witch", "Arcanista" },
      { "Hero/WuKong", "Sun Wukong" },
      { "Hero/ZhaoYun", "Zhao Yun" },
      {
        "UI/Hero/Description/Archangel",
        "Seraphiel strzela sopelami lodu z możliwością wywołania Zamrożenia. Może odzyskać pewną ilość zdrowia na sekundę i jest odporna na obrażenia Piekła."
      },
      {
        "UI/Hero/Description/CaptainG",
        "Burza pocisków strzela 3 kulami ognia z możliwością wywołania Płonięcia. Ponadto jest odporny na Oparzenia."
      },
      { "UI/Hero/Description/Cutie", "Faye Spark strzela pociskami z nieskończoną penetracją." },
      {
        "UI/Hero/Description/Gumdam",
        "Uderzający kometą wystrzeliwuje 2 pociski z bazooki, które eksplodują, raniąc wszystkich pobliskich wrogów. Ale pociski mogą mieć martwą strefę."
      },
      {
        "UI/Hero/Description/JeanneDArc",
        "Święta Joanna atakuje swoim mieczem (Atak w zwarciu), który może przenikać przez wrogów. Dodatkowo nie potrzebuje czasu przeładowania."
      },
      {
        "UI/Hero/Description/MountainKing",
        "Władca piorunów uwalnia błyskawice, które mogą ogłuszyć wroga, a także jest odporny na Ogłuszenie."
      },
      {
        "UI/Hero/Description/Paladin",
        "Żelazny Templariusz może odzyskać niewielką ilość zdrowia na sekundę i jest odporny na obrażenia Piekła."
      },
      {
        "UI/Hero/Description/Ranger",
        "Jadowity Uderzacz strzela 2 trującymi strzałami, które automatycznie krążą wokół celu. Ale strzały mogą mieć martwą strefę."
      },
      {
        "UI/Hero/Description/Witch",
        "Arkana rzuca 2 kulami błyskawic, aby zaatakować wroga. Ma jedną dodatkową opcję ulepszenia podczas awansu na wyższy poziom."
      },
      {
        "UI/Hero/Description/WuKong",
        "Sun Wukong może teleportować się w inne miejsca (naciśnij przycisk trzy razy szybko, aby aktywować)."
      },
      {
        "UI/Hero/Description/ZhaoYun",
        "Zhao Yun strzela 3 energiami lodowego miecza we wszystkich kierunkach, aby zaatakować wroga. Ponadto jest odporny na Zamrożenie."
      },
      { "Level/Pause", "Wstrzymano" },
      { "Level/PressEscToResume", "Naciśnij Esc, aby wznowić" },
      { "Upgrade/RecoverHp/Name", "Odzyskaj Poziom{0}" },
      { "Upgrade/RecoverHp/Description", "Odzyskaj {0:F2}% HP" },
      { "Upgrade/IncreaseMaxHp/Name", "MaxHP Poziom{0}" },
      { "Upgrade/IncreaseMaxHp/Description", "Zwiększ Max HP o {0:F2}%" },
      { "Upgrade/IncreaseMaxHpOnHurt/Name", "Przerażenie Poziom{0}" },
      {
        "Upgrade/IncreaseMaxHpOnHurt/Description",
        "Zwiększ Max HP o {0:F2}% ,gdy gracz jest ranny. Maksymalny przyrost wyniesie {1:F2}%."
      },
      { "Upgrade/IncreaseDefense/Name", "Tarcza Poziom{0}" },
      { "Upgrade/IncreaseDefense/Description", "Zwiększ obronę o {0:F2}%" },
      { "Upgrade/IncreaseAllSpeed/Name", "Idź Szybko! Poziom{0}" },
      {
        "Upgrade/IncreaseAllSpeed/Description", "Zwiększ prędkość o {0:F2}%, w tym normalną prędkość i prędkość ataku."
      },
      { "Upgrade/IncreaseMovingSpeed/Name", "But do biegania Poziom{0}" },
      { "Upgrade/IncreaseMovingSpeed/Description", "Zwiększ normalną prędkość poruszania się o {0:F2}%." },
      { "Upgrade/IncreaseAttackingMovingSpeed/Name", "Uderz i Uciekaj Poziom{0}" },
      {
        "Upgrade/IncreaseAttackingMovingSpeed/Description", "Zwiększ prędkość poruszania się podczas ataku o {0:F2}%."
      },
      { "Upgrade/KillToIncreaseSpeed/Name", "Najemnik Poziom{0}" },
      {
        "Upgrade/KillToIncreaseSpeed/Description",
        "Zabij {0} wrogów, aby zwiększyć prędkość o {1:F2}%, a maksymalny przyrost wyniesie {2:F2}%. Przyrost prędkości zostanie zresetowany po zranieniu."
      },
      { "Upgrade/ExpBonus/Name", "Nerd Poziom{0}" },
      { "Upgrade/ExpBonus/Description", "Zwiększa doświadczenie zdobywane za każdym razem o {0:F2}%." },
      { "Upgrade/IncreasePickUp/Name", "Indiana Jones Poziom{0}" },
      { "Upgrade/IncreasePickUp/Description", "Zwiększ promień podnoszenia o {0:F2}%." },
      { "Upgrade/IncreaseShootingRange/Name", "Barrett Poziom{0}" },
      { "Upgrade/IncreaseShootingRange/Description", "Zwiększ zasięg strzału o {0:F2}%." },
      { "Upgrade/AddProjectile/Name", "Strzelba Poziom{0}" },
      {
        "Upgrade/AddProjectile/Description", "Dodaj {0} dodatkowych pocisków. Jednak rozproszenie nieznacznie wzrośnie."
      },
      { "Upgrade/IncreaseDispersion/Name", "Rozproszenie Poziom{0}" },
      { "Upgrade/IncreaseDispersion/Description", "Zwiększ rozproszenie o {0:F2}%." },
      { "Upgrade/ReduceDispersion/Name", "Karabin snajperski Poziom{0}" },
      { "Upgrade/ReduceDispersion/Description", "Zmniejsz rozproszenie o {0:F2}%." },
      { "Upgrade/IncreaseFiringRate/Name", "Szybki Ogień Poziom{0}" },
      { "Upgrade/IncreaseFiringRate/Description", "Zwiększa szybkostrzelność o {0:F2}%" },
      { "Upgrade/BurstFire/Name", "Ogień seriami Poziom {0}" },
      {
        "Upgrade/BurstFire/Description", "Zwiększa szybkostrzelność o {0:F2}% na {1:F2} sekund po otrzymaniu obrażeń."
      },
      { "Upgrade/IncreaseMagazineSize/Name", "Pojemny magazynek Poziom {0}" },
      { "Upgrade/IncreaseMagazineSize/Description", "Zwiększa pojemność magazynka o {0}." },
      { "Upgrade/ReduceReloadingTime/Name", "Szybki ładownik Poziom {0}" },
      { "Upgrade/ReduceReloadingTime/Description", "Skraca czas przeładowania o {0:F2}%" },
      { "Upgrade/BurstReload/Name", "Ładowanie ze strachem Poziom {0}" },
      {
        "Upgrade/BurstReload/Description", "Skraca czas przeładowania o {0:F2}% na {1:F2} sekund po otrzymaniu obrażeń."
      },
      { "Upgrade/IncreaseHurtPercentage/Name", "Ostrzenie Poziom {0}" },
      { "Upgrade/IncreaseHurtPercentage/Description", "Zwiększa obrażenia umiejętności bazowej o {0:F2}%" },
      { "Upgrade/IncreaseIceAeDuration/Name", "Lodowy Poziom {0}" },
      { "Upgrade/IncreaseIceAeDuration/Description", "Zwiększa czas trwania efektu Zamrożenia o {0:F2}%" },
      { "Upgrade/IncreaseFireAeDuration/Name", "Spalenie Poziom {0}" },
      { "Upgrade/IncreaseFireAeDuration/Description", "Zwiększa czas trwania efektu Podpalenia o {0:F2}%" },
      { "Upgrade/IncreaseThunderAeDuration/Name", "Burza Poziom {0}" },
      { "Upgrade/IncreaseThunderAeDuration/Description", "Zwiększa czas trwania efektu Ogłuszenia o {0:F2}%" },
      { "Upgrade/ActivateDart/Name", "Ninja" },
      { "Upgrade/ActivateDart/Description", "Rzuca rzutką, która krąży wokół ciebie." },
      { "Upgrade/AddDart/Name", "Sensei Poziom {0}" },
      { "Upgrade/AddDart/Description", "Rzuca {0} dodatkowymi rzutkami wokół ciebie." },
      { "Upgrade/PoisonDart/Name", "Zatruta rzutka Poziom {0}" },
      {
        "Upgrade/PoisonDart/Description",
        "Nasącza wszystkie rzutki trucizną, dając im {0:F2}% szansy na zatrucie wroga i zadanie {1:F2}% obrażeń na sekundę (nieskuteczne przeciwko bossom)."
      },
      { "Upgrade/ImproveDartHurt/Name", "Wyostrzone Poziom {0}" },
      { "Upgrade/ImproveDartHurt/Description", "Ostrzy wszystkie rzutki wokół ciebie, poprawiając {0:F2}% obrażeń." },
      { "Upgrade/ActivateBoomerang/Name", "Aborigini" },
      { "Upgrade/ActivateBoomerang/Description", "Rzuca bumerangiem, który krąży wokół ciebie." },
      { "Upgrade/AddBoomerang/Name", "Manuel Schütz Poziom {0}" },
      { "Upgrade/AddBoomerang/Description", "Rzuca {0} dodatkowymi bumerangami wokół ciebie." },
      { "Upgrade/BurnBoomerang/Name", "Ognisty Bumerang Poziom {0}" },
      {
        "Upgrade/BurnBoomerang/Description",
        "Daje bumerangowi {0:F2}% szansy na podpalenie wroga na {1:F2} sekund, zadając {2:F2}% obrażeń na sekundę."
      },
      { "Upgrade/IncreaseBoomerangSpeed/Name", "Aerodynamika Poziom {0}" },
      {
        "Upgrade/IncreaseBoomerangSpeed/Description", "Optymalizuje aerodynamikę, aby przyspieszyć bumerang o {0:F2}%"
      },
      { "Upgrade/ImproveBoomerangHurt/Name", "Ołowiane wypełnienie Poziom {0}" },
      { "Upgrade/ImproveBoomerangHurt/Description", "Ładuje bumerang ołowiem, zwiększając jego siłę ataku o {0:F2}%" },
      { "Upgrade/ActivateIceTower/Name", "Lodowa Wieża" },
      {
        "Upgrade/ActivateIceTower/Description",
        "Wysyła automatyczną Wieżę Lodową wokół ciebie, która automatycznie strzela gradem."
      },
      { "Upgrade/AddIceTower/Name", "Zima Poziom {0}" },
      { "Upgrade/AddIceTower/Description", "Dodaje {0} dodatkowych Wież Lodowych." },
      { "Upgrade/IncreaseIceTowerFiringRate/Name", "Skadi Poziom {0}" },
      { "Upgrade/IncreaseIceTowerFiringRate/Description", "Zwiększa prędkość ataku Wieży Lodowej o {0:F2}%" },
      { "Upgrade/ImproveIceTowerHurt/Name", "Kostka Lodu Poziom {0}" },
      { "Upgrade/ImproveIceTowerHurt/Description", "Poprawia obrażenia Wieży Lodowej o {0:F2}%" },
      { "Upgrade/ActivateFireTower/Name", "Ognista Wieża" },
      {
        "Upgrade/ActivateFireTower/Description",
        "Wysyła automatyczną Wieżę Ognia wokół ciebie, która automatycznie strzela kulami ognia."
      },
      { "Upgrade/AddFireTower/Name", "Upał Poziom {0}" },
      { "Upgrade/AddFireTower/Description", "Dodaje {0} dodatkowych Wież Ognia." },
      { "Upgrade/IncreaseFireTowerFiringRate/Name", "Apollo Poziom {0}" },
      { "Upgrade/IncreaseFireTowerFiringRate/Description", "Zwiększa prędkość ataku Wieży Ognia o {0:F2}%" },
      { "Upgrade/ImproveFireTowerHurt/Name", "Miotacz ognia Poziom {0}" },
      { "Upgrade/ImproveFireTowerHurt/Description", "Poprawia obrażenia Wieży Ognia o {0:F2}%" },
      { "Upgrade/ActivateThunderTower/Name", "Wieża Piorunów" },
      {
        "Upgrade/ActivateThunderTower/Description",
        "Wysyła automatyczną Wieżę Piorunów wokół ciebie, która automatycznie strzela piorunami."
      },
      { "Upgrade/AddThunderTower/Name", "Błyskawica Poziom {0}" },
      { "Upgrade/AddThunderTower/Description", "Dodaje {0} dodatkowych Wież Piorunów." },
      { "Upgrade/IncreaseThunderTowerFiringRate/Name", "Thor Poziom {0}" },
      { "Upgrade/IncreaseThunderTowerFiringRate/Description", "Zwiększa prędkość ataku Wieży Piorunów o {0:F2}%" },
      { "Upgrade/ImproveThunderTowerHurt/Name", "Cewka Tesli" },
      { "Upgrade/ImproveThunderTowerHurt/Description", "Poprawia obrażenia Wieży Piorunów o {0:F2}%" },
      { "Upgrade/ActivateSpiral/Name", "Spirala" },
      {
        "Upgrade/ActivateSpiral/Description",
        "Co jakiś czas bohater uwalnia spiralę z losowymi atrybutami, aby zaatakować wroga."
      },
      { "Upgrade/AddSpiral/Name", "Nautilidae Poziom {0}" },
      { "Upgrade/AddSpiral/Description", "Dodaje {0} dodatkowych Spirali wokół ciebie." },
      { "Upgrade/ReduceSpiralInterval/Name", "Ulewa Poziom {0}" },
      { "Upgrade/ReduceSpiralInterval/Description", "Skraca interwał Spirali o {0:F2}%" },
      { "Upgrade/IncreaseSpiralHurt/Name", "Burza Poziom {0}" },
      { "Upgrade/IncreaseSpiralHurt/Description", "Zwiększa obrażenia Spirali o {0:F2}%" },
      { "Upgrade/ActivatePuppet/Name", "Lalkarz" },
      { "Upgrade/ActivatePuppet/Description", "Przywołuje {0} lalek co {1:F2} sekund." },
      { "Upgrade/LevelUpPuppetToLv1/Name", "Uzbrojona Lalka" },
      {
        "Upgrade/LevelUpPuppetToLv1/Description",
        "Przywołuje {0} lalek wyższego poziomu co {1:F2} sekund. Jednak ulepszenia obecnych lalek są resetowane."
      },
      { "Upgrade/LevelUpPuppetToLv2/Name", "Lalka Saiya" },
      {
        "Upgrade/LevelUpPuppetToLv2/Description",
        "Przywołuje {0} lalek ostatecznych co {1:F2} sekund. Jednak ulepszenia obecnych lalek są resetowane."
      },
      { "Upgrade/ReducePuppetIntervalLv0/Name", "Drużyna Poziom {0}" },
      { "Upgrade/ReducePuppetIntervalLv0/Description", "Skraca interwał przywoływania lalek o {0:F2}%" },
      { "Upgrade/AddPuppetLv0/Name", "Krzyk Poziom {0}" },
      { "Upgrade/AddPuppetLv0/Description", "Przywołuje {0} dodatkowych lalek do walk grupowych." },
      { "Upgrade/IncreasePuppetHurtLv0/Name", "Nóż Poziom {0}" },
      { "Upgrade/IncreasePuppetHurtLv0/Description", "Zwiększa obrażenia wszystkich lalek o {0:F2}%" },
      { "Upgrade/ReducePuppetIntervalLv1/Name", "Pager Poziom {0}" },
      { "Upgrade/ReducePuppetIntervalLv1/Description", "Skraca interwał przywoływania lalek o {0:F2}%" },
      { "Upgrade/AddPuppetLv1/Name", "Mafia Poziom {0}" },
      { "Upgrade/AddPuppetLv1/Description", "Przywołuje {0} dodatkowych lalek do walk grupowych." },
      { "Upgrade/IncreasePuppetHurtLv1/Name", "Pistolet Poziom {0}" },
      { "Upgrade/IncreasePuppetHurtLv1/Description", "Zwiększa obrażenia wszystkich lalek o {0:F2}%" },
      { "Upgrade/ReducePuppetIntervalLv2/Name", "Smartfon Poziom {0}" },
      { "Upgrade/ReducePuppetIntervalLv2/Description", "Skraca interwał przywoływania lalek o {0:F2}%" },
      { "Upgrade/AddPuppetLv2/Name", "Armia Poziom {0}" },
      { "Upgrade/AddPuppetLv2/Description", "Przywołuje {0} dodatkowych lalek do walk grupowych." },
      { "Upgrade/IncreasePuppetHurtLv2/Name", "Karabin Poziom {0}" },
      { "Upgrade/IncreasePuppetHurtLv2/Description", "Zwiększa obrażenia wszystkich lalek o {0:F2}%" },
      { "Upgrade/ActivateFlyingSword/Name", "Latający Miecz" },
      {
        "Upgrade/ActivateFlyingSword/Description",
        "Używa Qi, aby kontrolować miecze, uwalniając {0} mieczy co {1:F2} sekund."
      },
      { "Upgrade/LevelUpFlyingSwordToLv1/Name", "Ciężki Miecz" },
      {
        "Upgrade/LevelUpFlyingSwordToLv1/Description",
        "Używa cięższych mieczy, uwalniając {0} mieczy co {1:F2} sekund. Jednak ulepszenia mieczy zostaną zresetowane."
      },
      { "Upgrade/LevelUpFlyingSwordToLv2/Name", "Naostrzony Miecz" },
      {
        "Upgrade/LevelUpFlyingSwordToLv2/Description",
        "Używa naostrzonych mieczy, uwalniając {0} mieczy co {1:F2} sekund. Jednak ulepszenia mieczy zostaną zresetowane."
      },
      { "Upgrade/AddFlyingSwordLv0/Name", "Junior Poziom {0}" },
      { "Upgrade/AddFlyingSwordLv0/Description", "Dodaje {0} dodatkowych latających mieczy." },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Name", "Apprentice Poziom {0}" },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Description", "Skróć interwał uwalniania mieczy o {0:F2}%." },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Name", "Wejście Szermierza Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Description", "Zwiększ obrażenia mieczy o {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv1/Name", "Starszy Lv{0}" },
      { "Upgrade/AddFlyingSwordLv1/Description", "Dodaj {0} więcej latających mieczy." },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Name", "Rzemieślnik Lv{0}" },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Description", "Skróć interwał uwalniania mieczy o {0:F2}%." },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Name", "Starszy Szermierza Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Description", "Zwiększ obrażenia mieczy o {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv2/Name", "Shifu Lv{0}" },
      { "Upgrade/AddFlyingSwordLv2/Description", "Dodaj {0} więcej latających mieczy." },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Name", "Łowca Słońca Lv{0}" },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Description", "Skróć interwał uwalniania mieczy o {0:F2}%." },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Name", "Zongshi lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Description", "Zwiększ obrażenia mieczy o {0:F2}%." },
      { "Upgrade/ActivateMine/Name", "Mina lądowa" },
      {
        "Upgrade/ActivateMine/Description",
        "Podłóż {0} min, które eksplodują i zadają {1:F2} obrażeń w promieniu {2:F2} metrów co {3:F2} sekund. Ponadto miny wywołują eksplozję pobliskich min."
      },
      { "Upgrade/LevelUpMineToLv1/Name", "Mina błyskawiczna" },
      {
        "Upgrade/LevelUpMineToLv1/Description",
        "Podłóż {0} ulepszonych min, które eksplodują i zadają {1:F2} obrażeń w promieniu {2:F2} metrów co {3:F2} sekund."
      },
      { "Upgrade/LevelUpMineToLv2/Name", "Mina Claymore" },
      {
        "Upgrade/LevelUpMineToLv2/Description",
        "Podłóż {0} niezwykle śmiercionośnych min, które eksplodują i zadają {1:F2} obrażeń w promieniu {2:F2} metrów co {3:F2} sekund."
      },
      { "Upgrade/AddMineLv0/Name", "Skaut Lv{0}" },
      { "Upgrade/AddMineLv0/Description", "Podłóż {0} więcej min lądowych za każdym razem." },
      { "Upgrade/ReduceMineIntervalLv0/Name", "Szczypce Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv0/Description", "Zmniejsza interwał między minami o {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv0/Name", "Drapnięcie Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv0/Description", "Zwiększ obrażenia min lądowych o {0:F2}%." },
      { "Upgrade/AddMineLv1/Name", "Minuteman Lv{0}" },
      { "Upgrade/AddMineLv1/Description", "Podłóż {0} więcej min błyskawicznych za każdym razem." },
      { "Upgrade/ReduceMineIntervalLv1/Name", "Skrzynka narzędziowa Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv1/Description", "Zmniejsz interwał między minami o {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv1/Name", "Stalowa kula Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv1/Description", "Zwiększ obrażenia min błyskawicznych o {0:F2}%." },
      { "Upgrade/AddMineLv2/Name", "Siły Specjalne Lv{0}" },
      { "Upgrade/AddMineLv2/Description", "Podłóż {0} więcej min błyskawicznych za każdym razem." },
      { "Upgrade/ReduceMineIntervalLv2/Name", "System Min Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv2/Description", "Zmniejsz interwał między minami o {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv2/Name", "Fragmentacja Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv2/Description", "Zwiększ obrażenia min Claymore o {0:F2}%." },
      { "MapIndicator/Forest", "Mglisty Las" },
      { "MapIndicator/Desert", "Spalona Pustynia" },
      { "MapIndicator/Dungeon", "Mroczny Loch" },
      { "MapIndicator/Graveyard", "Cmentarz Śmierci" },
      { "MapIndicator/Hell", "Ostateczne Piekło" },
      {
        "MapDescription/Forest",
        "Mglisty las, gdzie pociski często wystrzeliwują znikąd w pozornie spokojnych drzewach."
      },
      {
        "MapDescription/Desert",
        "Wygląda na to, że wiele potworów, które potrafią przetrwać na surowej pustyni, jest odpornych na atrybuty."
      },
      { "MapDescription/Dungeon", "W lochu często słychać wycie wilków i dźwięk uderzających młotów." },
      {
        "MapDescription/Graveyard",
        "Uważaj na nieuchwytne czaszki na cmentarzu! Możesz się zranić, jeśli na nie wpadniesz!"
      },
      {
        "MapDescription/Hell",
        "Piekło jest zaczarowane przez diabła, powodując mniej więcej obrażenia bohaterom co 10 sekund! Ale wydaje się, że niektórzy bohaterowie wcale się tym nie przejmują."
      },
      { "AdditionalEffect/Type/Freeze", "Zamrożenie" },
      { "AdditionalEffect/Type/Stun", "Ogłuszenie" },
      { "AdditionalEffect/Type/Burn", "Oparzenie" },
      { "AdditionalEffect/Type/Poison", "Trucizna" },
      { "Gem/GemSynthesis", "Synteza klejnotów" },
      { "Gem/Rarity/R", "Rzadki" },
      { "Gem/Rarity/SR", "Super Rzadki" },
      { "Gem/Rarity/SSR", "Super Super Rzadki" },
      { "Gem/Rarity/XR", "Niezwykle Rzadki" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByPercentage", "Prędkość poruszania się podczas ataku {0}{1}%" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByValue", "Prędkość poruszania się podczas ataku {0}{1}" },
      { "Gem/Upgrade/AugmentMovingSpeedByPercentage", "Prędkość poruszania się {0}{1}%" },
      { "Gem/Upgrade/AugmentMovingSpeedByValue", "Prędkość poruszania się {0}{1}" },
      { "Gem/Upgrade/CoolingCountdownDecrementByPercentage", "Czas odnowienia umiejętności podstawowej {0}{1}%" },
      { "Gem/Upgrade/ReloadCountdownDecrementByPercentage", "Czas przeładowania umiejętności podstawowej {0}{1}%" },
      { "Gem/Upgrade/DispersionDecrementByPercentage", "Rozproszenie umiejętności podstawowej {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByPercentage", "Penetracja umiejętności podstawowej {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByValue", "Penetracja umiejętności podstawowej {0}{1}" },
      { "Gem/Upgrade/RangeIncrementByPercentage", "Zasięg umiejętności podstawowej {0}{1}%" },
      { "Gem/Upgrade/RangeIncrementByValue", "Zasięg umiejętności podstawowej {0}{1}" },
      { "Gem/Upgrade/RepelForceIncrementByPercentage", "Siła odrzutu umiejętności podstawowej {0}{1}%" },
      { "Gem/Upgrade/RepelForceIncrementByValue", "Siła odrzutu umiejętności podstawowej {0}{1}" },
      { "Gem/Upgrade/SkillCountIncrementByValue", "Pocisk umiejętności podstawowej {0}{1}" },
      { "Gem/Upgrade/SpeedIncrementByPercentage", "Szybkość umiejętności podstawowej {0}{1}%" },
      { "Gem/Upgrade/SpeedIncrementByValue", "Szybkość umiejętności podstawowej {0}{1}" },
      { "Gem/Upgrade/EnableRangeAttack", "Włącz atak zasięgowy dla umiejętności podstawowej" },
      { "Gem/Upgrade/AugmentDamageRangeByValue", "Zakres obrażeń umiejętności podstawowej {0}{1}" },
      { "Gem/Upgrade/AugmentDamageRangeByPercentage", "Zakres obrażeń umiejętności podstawowej {0}{1}%" },
      { "Gem/Upgrade/AddPoisoningAdditionalEffect", "Dodaj dodatkowy efekt Zatrucia" },
      { "Gem/Upgrade/SetPoisoningPossibility", "Ustaw prawdopodobieństwo zatrucia na {0}%" },
      { "Gem/Upgrade/SetPoisoningHurtPercentage", "Ustaw procent obrażeń od zatrucia na {0}%" },
      { "Gem/Upgrade/AugmentPoisoningPossibility", "Prawdopodobieństwo zatrucia umiejętności {0}{1}%" },
      { "Gem/Upgrade/AugmentPoisoningHurtPercentage", "Procent obrażeń od zatrucia umiejętności {0}{1}%" },
      {
        "Gem/Upgrade/AddOrConvertBasicAdditionalEffect",
        "Dodaj lub przekonwertuj atrybut dodatkowego efektu na <b>{0}</b>"
      },
      {
        "Gem/Upgrade/SetBasicAdditionalEffectPossibility",
        "Ustaw prawdopodobieństwo dodatkowego efektu umiejętności na {0}%"
      },
      {
        "Gem/Upgrade/AugmentBasicAdditionalEffectPossibility",
        "Prawdopodobieństwo dodatkowego efektu umiejętności {0}{1}%"
      },
      { "Gem/Upgrade/AugmentPhysicalHurtByValue", "Obrażenia fizyczne {0}{1}" },
      { "Gem/Upgrade/AugmentPhysicalHurtByPercentage", "Obrażenia fizyczne {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicIceHurtByValue", "Obrażenia magiczne od lodu {0}{1}" },
      { "Gem/Upgrade/AugmentMagicIceHurtByPercentage", "Obrażenia magiczne od lodu {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicFireHurtByValue", "Obrażenia magiczne od ognia {0}{1}" },
      { "Gem/Upgrade/AugmentMagicFireHurtByPercentage", "Obrażenia magiczne od ognia {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicThunderByValue", "Obrażenia magiczne od błyskawic {0}{1}" },
      { "Gem/Upgrade/Augment/MagicThunderByPercentage", "Obrażenia magiczne od błyskawic {0}{1}%" },
      { "Gem/Upgrade/Augment/Resurrection", "Ilość wskrzeszeń {0}{1}" },
      { "Gem/Upgrade/Augment/IncreaseMaxHp", "Maksymalne HP {0}{1}" },
      {
        "Rune/Description/ClonedProjectile",
        "Zwiększa liczbę umiejętności podstawowych. Każde ulepszenie dodaje jeden pocisk."
      },
      {
        "Rune/Description/ExpBonus",
        "Zwiększa doświadczenie zdobywane przez bohaterów o 10%. Doświadczenie zdobywane wzrasta o 7% za każdym razem, gdy bohater awansuje."
      },
      {
        "Rune/Description/Fission",
        "Po tym, jak umiejętności podstawowe bohatera trafią wroga, istnieje pewna szansa na rozdzielenie. Początkowe prawdopodobieństwo rozdzielenia wynosi 10% i wzrasta o 2% z każdym ulepszeniem."
      },
      {
        "Rune/Description/HailStrike",
        "umiejętności aktywne. Po aktywacji wzywa gradobicie, aby atakować wrogów w zasięgu widzenia. Umiejętność trwa 7 sekund i wzrasta o 2 sekundy z każdym ulepszeniem."
      },
      {
        "Rune/Description/HolyShield",
        "umiejętności aktywne. Po aktywacji bohater zyskuje 10 sekund niewrażliwości, a każde ulepszenie wydłuża ten czas o 2 sekundy."
      },
      {
        "Rune/Description/IncreaseHonor",
        "Na koniec gry, wartość honoru zdobyta przez gracza wzrasta. Początkowy wzrost wynosi 10%, a każde ulepszenie zwiększa go o 10%."
      },
      {
        "Rune/Description/InstantKill",
        "Kiedy podstawowe umiejętności bohatera trafiają wroga, istnieje pewne prawdopodobieństwo, że wróg umrze natychmiast. Początkowe prawdopodobieństwo wynosi 1%, a każde ulepszenie zwiększa je o 0,5%."
      },
      {
        "Rune/Description/InstantReload",
        "Istnieje 10% szansy na natychmiastowe przeładowanie, gdy amunicja się wyczerpie. Każde ulepszenie zwiększa tę szansę o 5%."
      },
      {
        "Rune/Description/KillAndRecover",
        "Umiejętności aktywne. Za każdym razem, gdy bohater zabije wroga w ciągu 10 sekund, może odzyskać 1% swojego zdrowia. Czas trwania każdego ulepszenia jest zwiększony o 2 sekundy."
      },
      {
        "Rune/Description/IncreaseImmortalTime",
        "Zwiększa czas niewrażliwości po otrzymaniu obrażeń. Stan początkowy wzrasta o 0,25 sekundy, a każde ulepszenie o 0,15 sekundy."
      },
      { "Rune/Description/IncreaseMaxHp", "Zwiększa maksymalne zdrowie bohatera o 10%, a każde ulepszenie o 5%." },
      {
        "Rune/Description/MeteoriteStrike",
        "Umiejętności aktywne. Przywołuje meteoryty, aby atakowały wrogów w zasięgu widzenia przez 10 sekund. Początkowo spada 10 meteorytów na sekundę, a każde ulepszenie zwiększa tę liczbę o 5."
      },
      {
        "Rune/Description/PickUpDistance",
        "Zasięg podnoszenia przedmiotów przez bohatera wzrasta o 10%, a każde ulepszenie o 10%."
      },
      {
        "Rune/Description/Poisonous",
        "Umiejętności aktywne. Zadaje obrażenia od trucizny wrogom w zasięgu widzenia co sekundę przez 10 sekund. Każdy poziom zwiększa czas trwania o 2 sekundy."
      },
      {
        "Rune/Description/PushAway",
        "Odrzuca pobliskich wrogów za każdym razem, gdy bohaterowi kończy się amunicja (10 sekund czasu odnowienia). Każde ulepszenie zwiększa siłę odrzutu o 20%."
      },
      {
        "Rune/Description/HpRecovery",
        "Przywraca 0,2% HP bohatera na sekundę, a każde ulepszenie zwiększa ilość przywracanego HP o 0,2%."
      },
      {
        "Rune/Description/ReducedInjuery",
        "Obrażenia otrzymywane przez bohatera są zmniejszone o 5%, a dodatkowe 5% jest zmniejszane za każdy poziom."
      },
      {
        "Rune/Description/Resurrection",
        "Kiedy bohater umiera, jest natychmiast wskrzeszany z 25% przywróconego zdrowia. Po wskrzeszeniu, HP wzrasta o 15% z każdym ulepszeniem."
      },
      {
        "Rune/Description/ThunderStrike",
        "Umiejętności aktywne. Przywołuje błyskawice, aby precyzyjnie atakowały wrogów w zasięgu widzenia. Umiejętność trwa 10 sekund. Początkowo atakuje 10% wrogów na sekundę, każde ulepszenie zaatakuje dodatkowe 3% wrogów."
      },
      {
        "Rune/Description/TimeStop",
        "Wstrzymuje wszystkie działania wrogów na 5 sekund. Każde ulepszenie wydłuża czas wstrzymania o 2 sekundy."
      },
      { "Rune/Title/ClonedProjectile", "Sklonowany Pocisk" },
      { "Rune/Title/ExpBonus", "Księga Doświadczenia" },
      { "Rune/Title/Fission", "Rozszczepienie" },
      { "Rune/Title/HailStrike", "Grad" },
      { "Rune/Title/HolyShield", "Święta Tarcza" },
      { "Rune/Title/IncreaseHonor", "Mistrz" },
      { "Rune/Title/InstantKill", "Jeden Strzał, Jeden Zabity" },
      { "Rune/Title/InstantReload", "Wielomagazynek" },
      { "Rune/Title/KillAndRecover", "Krwiożerczy" },
      { "Rune/Title/IncreaseImmortalTime", "Hełm Rycerza" },
      { "Rune/Title/IncreaseMaxHp", "Silne Ramię" },
      { "Rune/Title/MeteoriteStrike", "Meteoryt" },
      { "Rune/Title/PickUpDistance", "Łowca Nagród" },
      { "Rune/Title/Poisonous", "Strefa Gazowa" },
      { "Rune/Title/PushAway", "Zostaw w Spokoju" },
      { "Rune/Title/HpRecovery", "Czerwony Krzyż" },
      { "Rune/Title/ReducedInjuery", "Opancerzony Rycerz" },
      { "Rune/Title/Resurrection", "Cud" },
      { "Rune/Title/ThunderStrike", "Uderzenie Pioruna" },
      { "Rune/Title/TimeStop", "Wehikuł Czasu" },
      { "Exception/CorruptedSaveFile", "Plik zapisu jest uszkodzony i został zarchiwizowany w {0}." },
    };
  }
}