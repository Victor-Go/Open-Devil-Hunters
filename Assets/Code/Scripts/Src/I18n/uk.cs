using System.Collections.Generic;

namespace Code.Scripts.Src.I18n
{
  public struct uk : ILanguage
  {
    public static readonly Dictionary<string, string> locales = new()
    {
      { "HelloWorld", "Привіт, Світ!" },
      { "Language", "українська мова" },
      { "NotAvailableInDemo", "В розробці" },
      { "Player1", "Гравець 1" },
      { "Player2", "Гравець 2" },
      {
        "Notification/WelcomeNotification",
        "Дякуємо за придбання нашої гри!Ми знаємо, що наша гра не ідеальна, тому нам потрібен ваш відгук!Слідкуйте за постійними оновленнями."
      },
      { "Measure/PerSecond", "/сек" },
      {
        "UI/FightPreparationGuide/Page0/0",
        "Існує 5 атрибутів для атак героїв та ворогів, а саме фізичний, крижаний, вогняний, громовий та отруйний."
      },
      {
        "UI/FightPreparationGuide/Page0/1",
        "Серед них крижаний атрибут стримує вогняний атрибут, вогняний атрибут стримує громовий атрибут, а громовий атрибут стримує крижаний атрибут."
      },
      {
        "UI/FightPreparationGuide/Page1/0",
        "Крижаний атрибут може викликати ефект замороження та завдати числової шкоди;\nВогняний атрибут може викликати ефект займання та завдати пропорційної шкоди;\nГромовий атрибут може викликати ефект оглушення, але не завдасть додаткової шкоди.\nУсі вищезазначені атрибути зникають через деякий час.\nОтруйний атрибут може викликати отруєння та продовжувати завдавати пропорційної шкоди. Але боси часто можуть полегшити або навіть зняти отруєння."
      },
      {
        "UI/FightPreparationGuide/Page1/1",
        "Деякі вороги мають захист від атрибутів, імунітет до ефектів атрибутів та пробиття захисту від атрибутів. Правильне використання атрибутів, які стримують ворога, може полегшити перемогу над супротивником!"
      },
      {
        "UI/FightReadyGuide/Page0/0",
        "Тут буде показано атрибути монстрів на поточній карті. Запам'ятайте ці атрибути та підберіть власну стратегію для перемоги над монстрами!"
      },
      {
        "UI/FightReadyGuide/Page1/0",
        "Ці дві позиції показуватимуть босів, що сидять на карті. Кожен бос має різні бойові навички та стилі. Спробуйте використовувати різні техніки, щоб перемогти різних босів!"
      },
      {
        "UI/FightReadyGuide/Page1/1",
        "Не забувайте досліджувати карту частіше, де ви можете збирати різні дорогоцінні камені для посилення своїх героїв.Ви також можете використовувати честь, яку ви отримуєте в боях, щоб розблокувати нових героїв та руни для кращого дослідження різних карт!"
      },
      { "UI/ControlGuide/ActiveSkill", "Активна навичка" },
      { "UI/ControlGuide/AutoFiring", "Автоматична стрільба" },
      { "UI/ControlGuide/Movement", "Рух" },
      { "UI/ControlGuide/Fire!", "Вогонь!" },
      { "UI/ControlGuide/AutoAiming", "Автоприцілювання" },
      { "UI/ControlGuide/Aim", "Прицілювання" },
      {
        "UI/ControlGuide/MoveToUseController",
        "Перемістіть джойстик, щоб використовувати прицілювання контролером (поза режимом автоматичного прицілювання)."
      },
      {
        "UI/ControlGuide/MoveToUseMouse",
        "Перемістіть мишу, щоб використовувати прицілювання мишею (поза режимом автоматичного прицілювання)."
      },
      { "UI/Control/AutoAiming", "Автоматичне прицілювання" },
      { "UI/Control/MouseAiming", "Прицілювання мишею" },
      { "UI/Control/ControllerAiming", "Прицілювання контролером" },
      { "UI/Control/AutoFiring", "Автоматична стрільба" },
      { "UI/Control/ManualFiring", "Ручна стрільба" },
      { "UI/Control/UseMouseSelectHero", "Використовуйте мишу, щоб вибрати героя" },
      { "UI/Text/LevelUp!", "Підвищення рівня!" },
      { "UI/Button/Choose", "Вибрати" },
      { "UI/Start", "Почати" },
      { "UI/Languages", "Мови" },
      { "UI/Options", "Налаштування" },
      { "UI/Exit", "Вихід" },
      { "UI/Name", "Ім'я" },
      { "UI/Description", "Опис" },
      { "UI/Properties", "Властивості" },
      { "UI/Properties/AttacksPerRound", "Атак за раунд" },
      { "UI/Properties/RateOfFire", "Швидкість стрільби" },
      { "UI/Properties/ReloadTime", "Час перезарядки" },
      { "UI/Properties/Projectiles", "Снаряди" },
      { "UI/Properties/FreezeDamagePerSecond", "Шкода за секунду" },
      { "UI/Properties/MovingSpeed", "Швидкість пересування" },
      { "UI/Properties/Physical", "Фізичний" },
      { "UI/Properties/Ice", "Крижаний" },
      { "UI/Properties/Fire", "Вогняний" },
      { "UI/Properties/Thunder", "Громовий" },
      { "UI/Properties/Poisoning", "Отруєння" },
      { "UI/GameOver", "Гра закінчена" },
      { "UI/FinalScore", "Остаточний рахунок" },
      { "UI/HonorGained", "Отримана честь" },
      { "UI/PlayerHonor", "Честь гравця" },
      { "UI/PlayerScore", "Рахунок гравця" },
      { "UI/EnemyKilled", "Вбито ворогів" },
      { "UI/ExperienceGained", "Отримано досвіду" },
      { "UI/FinalLevel", "Останній рівень" },
      { "UI/SurvivalTime", "Час виживання" },
      { "UI/Upgrades", "Покращення" },
      { "UI/GameOver/Failed", "ПОРАЗКА!" },
      { "UI/GameOver/Success", "РІВЕНЬ ПРОЙДЕНО!" },
      { "UI/GameOver/Aborted", "МІСІЮ ПЕРЕРВАНО" },
      { "UI/Achievements", "Досягнення" },
      { "UI/Furnace", "Піч" },
      { "UI/Gems", "Коштовності" },
      { "UI/Runes", "Руни" },
      { "UI/Difficulty", "Складність" },
      { "UI/Mode", "Режим гри" },
      { "UI/CasualMode", "Звичайний режим" },
      { "UI/ChallengeMode", "Режим випробувань" },
      { "UI/InfiniteMode", "Безкінечний режим" },
      { "UI/Play", "Грати" },
      { "UI/Confirm", "Підтвердити" },
      { "UI/Cancel", "Скасувати" },
      { "UI/GemList", "Список дорогоцінних каменів" },
      { "UI/HeroDescription/MaxHp", "Макс. HP" },
      { "UI/HeroDescription/HpRecover", "Відновлення HP" },
      { "UI/HeroDescription/Resurrection", "Воскресіння" },
      { "UI/HeroDescription/BasicSkillHurt", "Шкода від базової навички" },
      { "UI/HeroDescription/BasicSkillProperty", "Атрибут базової навички" },
      { "UI/HeroDescription/Possibility", "Можливість додаткового ефекту" },
      { "UI/HeroDescription/AttackRange", "Радіус атаки" },
      { "UI/HeroDescription/AttackPerRound", "Атак за раунд" },
      { "UI/HeroDescription/AttackSpeed", "Швидкість атаки" },
      { "UI/HeroDescription/PickUpRange", "Радіус підбору" },
      { "UI/HeroDescription/MovingSpeed", "Швидкість пересування" },
      { "UI/HeroDescription/AttackMovingSpeed", "Швидкість пересування під час атаки" },
      { "UI/HeroDescription/BaseSkill", "Базова навичка" },
      { "UI/HeroDescription/AdditionalEffect", "Додатковий ефект" },
      { "UI/HeroDescription/ProjectileCount", "Кількість снарядів" },
      { "UI/HeroDescription/AEHurt", "Шкода від додаткового ефекту" },
      { "UI/FightPreparation/TwoPlayersUniqueHero", "Два гравці не можуть вибрати одного й того ж героя" },
      { "UI/FightPreparation/BrowserHeroes", "Герої браузера" },
      { "UI/FightPreparation/Back", "Назад" },
      { "UI/FightPreparation/ApplyGems", "Одягнути коштовності" },
      { "UI/FightPreparation/ApplyRunes", "Активувати руни" },
      { "UI/FightPreparation/SelectHero", "Вибрати героя" },
      { "UI/FightPreparation/Unlock", "Розблокувати" },
      {
        "UI/FightPreparation/AddPlayer",
        "Натисніть <color=red>A</color> на геймпаді або праву клавішу Ctrl на клавіату, щоб додати 2-го гравця"
      },
      { "UI/FightPreparation/RemovePlayer", "Видалити" },
      { "UI/PickGemUI/PickGemTitle", "Одягнути коштовності" },
      { "UI/PickGemUI/OneGemOnePlayer", "Коштовність може використовуватися лише одним гравцем" },
      { "UI/PickRuneUI/PickRuneTitle", "Активувати руни" },
      { "UI/PickRuneUI/Class", "Рівень" },
      { "UI/PickRuneUI/InspirationRune", "Руна натхнення" },
      { "UI/PickRuneUI/DominationRune", "Руна домінування" },
      { "UI/PickRuneUI/ImmortalRune", "Руна безсмертя" },
      { "UI/PickRuneUI/Upgrade", "Покращити" },
      { "UI/PickRuneUI/Price", "Ціна" },
      { "UI/PickRuneUI/OneRunePerClass", "На рівень можна вибрати лише одну руну" },
      { "UI/Pause/PhysicalAttack", "Фізична атака" },
      { "UI/Pause/IceAttack", "Крижана атака" },
      { "UI/Pause/FireAttack", "Вогняна атака" },
      { "UI/Pause/ThunderAttack", "Громова атака" },
      { "UI/Pause/Poisoning", "Отруєння" },
      { "UI/Pause/MovingSpeed", "Швидкість пересування" },
      { "UI/Pause/AttackingSpeed", "Швидкість під час атаки" },
      { "UI/Pause/AttackSpeed", "Швидкість атаки" },
      { "UI/Pause/RecoverSpeed", "Швидкість відновлення" },
      { "UI/Pause/ExpBonus", "Бонус досвіду" },
      { "UI/Pause/RoundsPerSecond", " раундів/с" },
      { "UI/Pause/Resume", "Відновити" },
      { "UI/Pause/ControlGuide", "Посібник з керування" },
      { "UI/Pause/GiveUp", "Здатися" },
      {
        "UI/Upgrade/DemoUpgradeUnlocked",
        "У демо-версії вже розблоковано покращень: 96/188У офіційній версії можуть бути зміни в покращеннях"
      },
      { "UI/GameOver/Quit", "Вийти" },
      { "UI/FightReady/Difficulty", "Складність" },
      { "UI/FightReady/DifficultyNumber", "Рівень.{0}" },
      { "UI/FightReady/UnlockedDifficulty", "Розблоковано {0}/{1}" },
      { "UI/FightReady/Mode", "Режим" },
      { "UI/FightReady/Map", "Карта" },
      { "UI/FightReady/Description", "Опис" },
      { "UI/FightReady/Casual", "Звичайний" },
      { "UI/FightReady/Standard", "Стандартний" },
      { "UI/FightReady/Infinite", "Безкінечний" },
      { "UI/FightReady/Forest", "Туманний ліс" },
      { "UI/FightReady/Desert", "Палюча пустеля" },
      { "UI/FightReady/Dungeon", "Темне підземелля" },
      { "UI/FightReady/Graveyard", "Кладовище смерті" },
      { "UI/FightReady/Hell", "Найкраще пекло" },
      { "UI/FightReady/ModeDescription/CasualMode", "У звичайному режимі складність монстрів буде зменшена." },
      { "UI/FightReady/ModeDescription/BossAppearTime", "Бос з'явиться через {0} хвилин" },
      {
        "UI/FightReady/AttributeDescription",
        "Співвідношення атрибутів ворога: Фізичний - {0}%, Крижаний - {1}%, Вогняний - {2}%, Громовий - {3}%, Отруйний - {4}%"
      },
      { "UI/Prompt/UnlockRune", "Розблокувати руну <b>{0}</b> за {1}, продовжити?" },
      { "UI/Prompt/UnlockHero", "Розблокувати героя <b>{0}</b> за {1}, продовжити?" },
      { "UI/Info/Unlock/InsufficientBalance", "Вибачте, недостатньо коштів для розблокування. (Потрібно: {0})" },
      { "UI/Info/Upgrade/InsufficientBalance", "Вибачте, недостатньо балансу для покращення." },
      { "UI/Info/RunePrerequisitesNotSatisfied", "Будь ласка, спочатку розблокуйте руну попереднього рівня." },
      { "UI/Audio/Overall", "Загалом" },
      { "UI/Audio/Bgm", "Фонова музика" },
      { "UI/Audio/Sfx", "Звукові ефекти" },
      { "Physical", "Фізичний" },
      { "Ice", "Крижаний" },
      { "Thunder", "Громовий" },
      { "Fire", "Вогняний" },
      { "Poison", "Отруйний" },
      { "Hero/Archangel", "Серафіель" },
      { "Hero/CaptainG", "Шторм куль" },
      { "Hero/Cutie", "Фей Спарк" },
      { "Hero/Gumdam", "Удар комети" },
      { "Hero/JeanneDArc", "Свята Жанна" },
      { "Hero/MountainKing", "Володар грому" },
      { "Hero/Paladin", "Залізний тамплієр" },
      { "Hero/Ranger", "Отруювач" },
      { "Hero/Witch", "Арканіст" },
      { "Hero/WuKong", "Сунь Укун" },
      { "Hero/ZhaoYun", "Чжао Юнь" },
      {
        "UI/Hero/Description/Archangel",
        "Серафіель стріляє крижинками з можливістю викликати замороження. Вона може відновлювати деяку кількість здоров'я за секунду і має імунітет до пошкоджень від Пекла."
      },
      {
        "UI/Hero/Description/CaptainG",
        "Шторм куль вистрілює 3 вогняними кулями з можливістю викликати горіння. Також він має імунітет до опіків."
      },
      { "UI/Hero/Description/Cutie", "Фей Спарк стріляє кулями з нескінченним проникненням." },
      {
        "UI/Hero/Description/Gumdam",
        "Удар комети вистрілює 2 ракетами з базуки, які вибухають, завдаючи шкоди всім довколишнім ворогам. Але ракети можуть мати мертву зону."
      },
      {
        "UI/Hero/Description/JeanneDArc",
        "Свята Жанна атакує своїм мечем (атака в ближньому бою), який може пробивати ворогів. Крім того, їй не потрібен час перезарядки."
      },
      {
        "UI/Hero/Description/MountainKing",
        "Володар грому випускає блискавки, які можуть викликати оглушення ворога, і також має імунітет до оглушення."
      },
      {
        "UI/Hero/Description/Paladin",
        "Залізний тамплієр може відновлювати невелику кількість здоров'я за секунду і має імунітет до пошкоджень від Пекла."
      },
      {
        "UI/Hero/Description/Ranger",
        "Отруювач вистрілює 2 отруйними стрілами, які автоматично кружляють навколо цілі. Але стріли можуть мати мертву зону."
      },
      {
        "UI/Hero/Description/Witch",
        "Чарівниця випускає 2 кулі блискавки для атаки ворога. Вона має одну додаткову опцію покращення під час підвищення рівня."
      },
      {
        "UI/Hero/Description/WuKong",
        "Сунь Укун може телепортуватися в інші місця (натисніть кнопку тричі швидко, щоб активувати)."
      },
      {
        "UI/Hero/Description/ZhaoYun",
        "Чжао Юнь стріляє 3 крижаними енергіями меча в усіх напрямках, щоб атакувати ворога. Також він має імунітет до замороження."
      },
      { "Level/Pause", "Пауза" },
      { "Level/PressEscToResume", "Натисніть Esc, щоб відновити" },
      { "Upgrade/RecoverHp/Name", "Відновлення Lv{0}" },
      { "Upgrade/RecoverHp/Description", "Відновлення {0:F2}% HP" },
      { "Upgrade/IncreaseMaxHp/Name", "Макс. HP Lv{0}" },
      { "Upgrade/IncreaseMaxHp/Description", "Збільшення Макс. HP на {0:F2}%" },
      { "Upgrade/IncreaseMaxHpOnHurt/Name", "Страх Lv{0}" },
      {
        "Upgrade/IncreaseMaxHpOnHurt/Description",
        "Збільшення Макс. HP на {0:F2}% при отриманні гравцем поранення. Максимальне збільшення становитиме {1:F2}%."
      },
      { "Upgrade/IncreaseDefense/Name", "Щит Lv{0}" },
      { "Upgrade/IncreaseDefense/Description", "Збільшення захисту на {0:F2}%" },
      { "Upgrade/IncreaseAllSpeed/Name", "Вперед! Lv{0}" },
      {
        "Upgrade/IncreaseAllSpeed/Description",
        "Збільшення швидкості на {0:F2}% включно зі звичайною швидкістю та швидкістю атаки."
      },
      { "Upgrade/IncreaseMovingSpeed/Name", "Біговий кросівок Lv{0}" },
      { "Upgrade/IncreaseMovingSpeed/Description", "Збільшення звичайної швидкості пересування на {0:F2}%." },
      { "Upgrade/IncreaseAttackingMovingSpeed/Name", "Удар і втеча Lv{0}" },
      {
        "Upgrade/IncreaseAttackingMovingSpeed/Description", "Збільшити швидкість пересування під час атаки на {0:F2}%."
      },
      { "Upgrade/KillToIncreaseSpeed/Name", "Найманець Lv{0}" },
      {
        "Upgrade/KillToIncreaseSpeed/Description",
        "Вбийте {0} ворогів, щоб збільшити швидкість на {1:F2}%, а максимальне збільшення становитиме {2:F2}%. Збільшення швидкості буде скинуто після поранення."
      },
      { "Upgrade/ExpBonus/Name", "Ботан Lv{0}" },
      { "Upgrade/ExpBonus/Description", "Збільшує досвід, отриманий кожного разу, на {0:F2}%." },
      { "Upgrade/IncreasePickUp/Name", "Індіана Джонс Lv{0}" },
      { "Upgrade/IncreasePickUp/Description", "Збільшення радіусу підбору на {0:F2}%." },
      { "Upgrade/IncreaseShootingRange/Name", "Барретт Lv{0}" },
      { "Upgrade/IncreaseShootingRange/Description", "Збільшення дальності стрільби на {0:F2}%." },
      { "Upgrade/AddProjectile/Name", "Дробовик Lv{0}" },
      { "Upgrade/AddProjectile/Description", "Додає {0} додаткових снарядів. Однак розсіювання трохи збільшиться." },
      { "Upgrade/IncreaseDispersion/Name", "Розсіювання Lv{0}" },
      { "Upgrade/IncreaseDispersion/Description", "Збільшення розсіювання на {0:F2}%." },
      { "Upgrade/ReduceDispersion/Name", "Снайперська гвинтівка Lv{0}" },
      { "Upgrade/ReduceDispersion/Description", "Зменшення розсіювання на {0:F2}%." },
      { "Upgrade/IncreaseFiringRate/Name", "Швидкий вогонь Lv{0}" },
      { "Upgrade/IncreaseFiringRate/Description", "Збільшення швидкості стрільби на {0:F2}%." },
      { "Upgrade/BurstFire/Name", "Серія пострілів Lv{0}" },
      {
        "Upgrade/BurstFire/Description",
        "Збільшує швидкість стрільби на {0:F2}% протягом {1:F2} секунд після отримання пошкодження."
      },
      { "Upgrade/IncreaseMagazineSize/Name", "Барабанний магазин Lv{0}" },
      { "Upgrade/IncreaseMagazineSize/Description", "Збільшення розміру магазину на {0}." },
      { "Upgrade/ReduceReloadingTime/Name", "Швидке заряджання Lv{0}" },
      { "Upgrade/ReduceReloadingTime/Description", "Скорочення часу перезаряджання на {0:F2}%." },
      { "Upgrade/BurstReload/Name", "Заряджання під страхом Lv{0}" },
      {
        "Upgrade/BurstReload/Description",
        "Скорочення часу перезаряджання на {0:F2}% протягом {1:F2} секунд після отримання пошкодження."
      },
      { "Upgrade/IncreaseHurtPercentage/Name", "Загострення Lv{0}" },
      { "Upgrade/IncreaseHurtPercentage/Description", "Збільшення шкоди від базової навички на {0:F2}%." },
      { "Upgrade/IncreaseIceAeDuration/Name", "Крижаний Lv{0}" },
      { "Upgrade/IncreaseIceAeDuration/Description", "Збільшення тривалості ефекту замороження на {0:F2}%." },
      { "Upgrade/IncreaseFireAeDuration/Name", "Спалити Lv{0}" },
      { "Upgrade/IncreaseFireAeDuration/Description", "Збільшення тривалості ефекту горіння на {0:F2}%." },
      { "Upgrade/IncreaseThunderAeDuration/Name", "Гроза Lv{0}" },
      { "Upgrade/IncreaseThunderAeDuration/Description", "Збільшення тривалості ефекту оглушення на {0:F2}%." },
      { "Upgrade/ActivateDart/Name", "Ніндзя" },
      { "Upgrade/ActivateDart/Description", "Кидає дротик, який кружляє навколо вас." },
      { "Upgrade/AddDart/Name", "Сенсей Lv{0}" },
      { "Upgrade/AddDart/Description", "Кидає на {0} дротиків більше навколо вас." },
      { "Upgrade/PoisonDart/Name", "Отруйний дротик Lv{0}" },
      {
        "Upgrade/PoisonDart/Description",
        "Замочує всі дротики в отруті, даючи їм {0:F2}% шанс отруїти ворога та завдати {1:F2}% шкоди за секунду (неефективно проти босів)."
      },
      { "Upgrade/ImproveDartHurt/Name", "Загострений Lv{0}" },
      { "Upgrade/ImproveDartHurt/Description", "Загострює всі дротики навколо вас, покращуючи {0:F2}% шкоди." },
      { "Upgrade/ActivateBoomerang/Name", "Абориген" },
      { "Upgrade/ActivateBoomerang/Description", "Кидає бумеранг, який кружляє навколо вас." },
      { "Upgrade/AddBoomerang/Name", "Мануель Шютц Lv{0}" },
      { "Upgrade/AddBoomerang/Description", "Кидає на {0} бумерангів більше навколо вас." },
      { "Upgrade/BurnBoomerang/Name", "Вогняний бумеранг Lv{0}" },
      {
        "Upgrade/BurnBoomerang/Description",
        "Дає бумерангу {0:F2}% шанс підпалити ворога на {1:F2} секунд, завдаючи {2:F2}% шкоди за секунду."
      },
      { "Upgrade/IncreaseBoomerangSpeed/Name", "Аеродинаміка Lv{0}" },
      {
        "Upgrade/IncreaseBoomerangSpeed/Description",
        "Оптимізує аеродинаміку для збільшення швидкості бумеранга на {0:F2}%."
      },
      { "Upgrade/ImproveBoomerangHurt/Name", "Свинцеве наповнення Lv{0}" },
      {
        "Upgrade/ImproveBoomerangHurt/Description", "Заряджає бумеранг свинцем, збільшуючи його силу атаки на {0:F2}%."
      },
      { "Upgrade/ActivateIceTower/Name", "Крижана вежа" },
      {
        "Upgrade/ActivateIceTower/Description",
        "Запускає автоматичну крижану вежу навколо вас, яка автоматично стріляє градом."
      },
      { "Upgrade/AddIceTower/Name", "Зима Lv{0}" },
      { "Upgrade/AddIceTower/Description", "Додає {0} крижаних веж." },
      { "Upgrade/IncreaseIceTowerFiringRate/Name", "Скаді Lv{0}" },
      { "Upgrade/IncreaseIceTowerFiringRate/Description", "Збільшує швидкість атаки крижаної вежі на {0:F2}%." },
      { "Upgrade/ImproveIceTowerHurt/Name", "Крижаний куб Lv{0}" },
      { "Upgrade/ImproveIceTowerHurt/Description", "Покращує шкоду від крижаної вежі на {0:F2}%." },
      { "Upgrade/ActivateFireTower/Name", "Вогняна вежа" },
      {
        "Upgrade/ActivateFireTower/Description",
        "Запускає автоматичну вогняну вежу навколо вас, яка автоматично стріляє вогняною кулею."
      },
      { "Upgrade/AddFireTower/Name", "Спекотний Lv{0}" },
      { "Upgrade/AddFireTower/Description", "Додає {0} вогняних веж." },
      { "Upgrade/IncreaseFireTowerFiringRate/Name", "Аполлон Lv{0}" },
      { "Upgrade/IncreaseFireTowerFiringRate/Description", "Збільшує швидкість атаки вогняної вежі на {0:F2}%." },
      { "Upgrade/ImproveFireTowerHurt/Name", "Вогнемет Lv{0}" },
      { "Upgrade/ImproveFireTowerHurt/Description", "Покращує шкоду від вогняної вежі на {0:F2}%." },
      { "Upgrade/ActivateThunderTower/Name", "Громова вежа" },
      {
        "Upgrade/ActivateThunderTower/Description",
        "Запускає автоматичну громову вежу навколо вас, яка автоматично стріляє громом."
      },
      { "Upgrade/AddThunderTower/Name", "Удар блискавки Lv{0}" },
      { "Upgrade/AddThunderTower/Description", "Додає {0} громових веж." },
      { "Upgrade/IncreaseThunderTowerFiringRate/Name", "Тор Lv{0}" },
      { "Upgrade/IncreaseThunderTowerFiringRate/Description", "Збільшує швидкість атаки громової вежі на {0:F2}%." },
      { "Upgrade/ImproveThunderTowerHurt/Name", "Котушка Тесла" },
      { "Upgrade/ImproveThunderTowerHurt/Description", "Покращує шкоду від громової вежі на {0:F2}%." },
      { "Upgrade/ActivateSpiral/Name", "Спіраль" },
      {
        "Upgrade/ActivateSpiral/Description",
        "Час від часу герой випускає спіраль із випадковими атрибутами для атаки ворога."
      },
      { "Upgrade/AddSpiral/Name", "Наутиліда Lv{0}" },
      { "Upgrade/AddSpiral/Description", "Додає {0} спіралей навколо вас." },
      { "Upgrade/ReduceSpiralInterval/Name", "Злива Lv{0}" },
      { "Upgrade/ReduceSpiralInterval/Description", "Зменшує інтервал спіралі на {0:F2}%." },
      { "Upgrade/IncreaseSpiralHurt/Name", "Гроза Lv{0}" },
      { "Upgrade/IncreaseSpiralHurt/Description", "Збільшує шкоду від спіралі на {0:F2}%." },
      { "Upgrade/ActivatePuppet/Name", "Боєць-маріонетка" },
      { "Upgrade/ActivatePuppet/Description", "Закликає {0} маріонеток кожні {1:F2} секунди." },
      { "Upgrade/LevelUpPuppetToLv1/Name", "Озброєна маріонетка" },
      {
        "Upgrade/LevelUpPuppetToLv1/Description",
        "Закликає {0} маріонеток підвищеного рівня кожні {1:F2} секунди. Проте, покращення поточних маріонеток скидаються."
      },
      { "Upgrade/LevelUpPuppetToLv2/Name", "Маріонетка Сайя" },
      {
        "Upgrade/LevelUpPuppetToLv2/Description",
        "Закликає {0} найсильніших маріонеток кожні {1:F2} секунди. Проте, покращення поточних маріонеток скидаються."
      },
      { "Upgrade/ReducePuppetIntervalLv0/Name", "Команда Lv{0}" },
      { "Upgrade/ReducePuppetIntervalLv0/Description", "Зменшує інтервал виклику маріонеток на {0:F2}%." },
      { "Upgrade/AddPuppetLv0/Name", "Крик Lv{0}" },
      { "Upgrade/AddPuppetLv0/Description", "Закликає на {0} маріонеток більше для групових боїв." },
      { "Upgrade/IncreasePuppetHurtLv0/Name", "Ніж Lv{0}" },
      { "Upgrade/IncreasePuppetHurtLv0/Description", "Збільшує шкоду всіх маріонеток на {0:F2}%." },
      { "Upgrade/ReducePuppetIntervalLv1/Name", "Пейджер Lv{0}" },
      { "Upgrade/ReducePuppetIntervalLv1/Description", "Зменшує інтервал виклику маріонеток на {0:F2}%." },
      { "Upgrade/AddPuppetLv1/Name", "Мафія Lv{0}" },
      {
        "Upgrade/AddPuppetLv1/Description",
        "Закликає на {0} маріонеток більше для групових боїв."
      },
      { "Upgrade/IncreasePuppetHurtLv1/Name", "Пістолет Lv{0}" },
      { "Upgrade/IncreasePuppetHurtLv1/Description", "Збільшує шкоду всіх маріонеток на {0:F2}%." },
      { "Upgrade/ReducePuppetIntervalLv2/Name", "Смартфон Lv{0}" },
      { "Upgrade/ReducePuppetIntervalLv2/Description", "Зменшує інтервал виклику маріонеток на {0:F2}%." },
      { "Upgrade/AddPuppetLv2/Name", "Армія Lv{0}" },
      {
        "Upgrade/AddPuppetLv2/Description",
        "Закликає на {0} маріонеток більше для групових боїв."
      },
      { "Upgrade/IncreasePuppetHurtLv2/Name", "Гвинтівка Lv{0}" },
      { "Upgrade/IncreasePuppetHurtLv2/Description", "Збільшує шкоду всіх маріонеток на {0:F2}%." },
      { "Upgrade/ActivateFlyingSword/Name", "Літаючі мечі" },
      {
        "Upgrade/ActivateFlyingSword/Description",
        "Використовуйте Ци для керування мечами, випускаючи {0} мечів кожні {1:F2} секунди."
      },
      { "Upgrade/LevelUpFlyingSwordToLv1/Name", "Важкий меч" },
      {
        "Upgrade/LevelUpFlyingSwordToLv1/Description",
        "Використовуйте важчі мечі, випускаючи {0} мечів кожні {1:F2} секунди. Проте, покращення мечів буде скинуто."
      },
      { "Upgrade/LevelUpFlyingSwordToLv2/Name", "Загострений меч" },
      {
        "Upgrade/LevelUpFlyingSwordToLv2/Description",
        "Використовуйте загострені мечі, випускаючи {0} мечів кожні {1:F2} секунди. Проте, покращення мечів буде скинуто."
      },
      { "Upgrade/AddFlyingSwordLv0/Name", "Юніор Lv{0}" },
      { "Upgrade/AddFlyingSwordLv0/Description", "Додає {0} літаючих мечів." },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Name", "Учень Lv{0}" },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Description", "Зменшити інтервал випускання мечів на {0:F2}%." },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Name", "Поява мечника Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Description", "Збільшити урон мечів на {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv1/Name", "Старший Lv{0}" },
      { "Upgrade/AddFlyingSwordLv1/Description", "Додати {0} літаючих мечів." },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Name", "Майстер Lv{0}" },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Description", "Зменшити інтервал випускання мечів на {0:F2}%." },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Name", "Старший мечник Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Description", "Збільшити урон мечів на {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv2/Name", "Шифу Lv{0}" },
      { "Upgrade/AddFlyingSwordLv2/Description", "Додати {0} літаючих мечів." },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Name", "Мисливець за сонцем Lv{0}" },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Description", "Зменшити інтервал випускання мечів на {0:F2}%." },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Name", "Зонгші lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Description", "Збільшити урон мечів на {0:F2}%." },
      { "Upgrade/ActivateMine/Name", "Наземна міна" },
      {
        "Upgrade/ActivateMine/Description",
        "Встановити {0} мін, які вибухають і завдають {1:F2} шкоди в радіусі {2:F2} метрів кожні {3:F2} секунди."
      },
      { "Upgrade/LevelUpMineToLv1/Name", "Громова міна" },
      {
        "Upgrade/LevelUpMineToLv1/Description",
        "Встановити {0} посилених мін, які вибухають і завдають {1:F2} шкоди в радіусі {2:F2} метрів кожні {3:F2} секунди."
      },
      { "Upgrade/LevelUpMineToLv2/Name", "Міна Клеймор" },
      {
        "Upgrade/LevelUpMineToLv2/Description",
        "Встановити {0} надзвичайно смертоносних мін, які вибухають і завдають {1:F2} шкоди в радіусі {2:F2} метрів кожні {3:F2} секунди."
      },
      { "Upgrade/AddMineLv0/Name", "Скаут Lv{0}" },
      { "Upgrade/AddMineLv0/Description", "Щоразу встановлювати на {0} більше наземних мін." },
      { "Upgrade/ReduceMineIntervalLv0/Name", "Плоскогубці Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv0/Description", "Зменшує інтервал між мінами на {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv0/Name", "Подряпина Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv0/Description", "Збільшити шкоду від наземних мін на {0:F2}%." },
      { "Upgrade/AddMineLv1/Name", "Міліціонер Lv{0}" },
      { "Upgrade/AddMineLv1/Description", "Щоразу встановлювати на {0} більше громових мін." },
      { "Upgrade/ReduceMineIntervalLv1/Name", "Ящик з інструментами Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv1/Description", "Зменшити інтервал між мінами на {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv1/Name", "Сталева куля Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv1/Description", "Збільшити шкоду від громових мін на {0:F2}%." },
      { "Upgrade/AddMineLv2/Name", "Спецназ Lv{0}" },
      { "Upgrade/AddMineLv2/Description", "Щоразу встановлювати на {0} більше громових мін." },
      { "Upgrade/ReduceMineIntervalLv2/Name", "Система мін Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv2/Description", "Зменшити інтервал між мінами на {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv2/Name", "Фрагментація Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv2/Description", "Збільшити шкоду від мін Клеймор на {0:F2}%." },
      { "MapIndicator/Forest", "Туманний ліс" },
      { "MapIndicator/Desert", "Палюча пустеля" },
      { "MapIndicator/Dungeon", "Темне підземелля" },
      { "MapIndicator/Graveyard", "Кладовище смерті" },
      { "MapIndicator/Hell", "Найкраще пекло" },
      { "MapDescription/Forest", "Туманний ліс, де кулі часто вилітають нізвідки серед, здавалося б, мирних дерев." },
      {
        "MapDescription/Desert",
        "Здається, багато монстрів, які можуть вижити в суворій пустелі, мають імунітет до атрибутів."
      },
      { "MapDescription/Dungeon", "У підземеллі часто чути виття вовків і звук ударів молотів." },
      {
        "MapDescription/Graveyard",
        "Стережіться невловимих черепів на кладовищі! Ви можете отримати поранення, якщо зіткнетеся з ними!"
      },
      {
        "MapDescription/Hell",
        "Пекло зачаровано дияволом, завдаючи більш-менш шкоди героям кожні 10 секунд! Але, здається, деякі герої зовсім не звертають на це уваги."
      },
      { "AdditionalEffect/Type/Freeze", "Замороження" },
      { "AdditionalEffect/Type/Stun", "Оглушення" },
      { "AdditionalEffect/Type/Burn", "Опік" },
      { "AdditionalEffect/Type/Poison", "Отруєння" },
      { "Gem/GemSynthesis", "Синтез каменів" },
      { "Gem/Rarity/R", "Рідкісний" },
      { "Gem/Rarity/SR", "Дуже рідкісний" },
      { "Gem/Rarity/SSR", "Надзвичайно рідкісний" },
      { "Gem/Rarity/XR", "Винятково рідкісний" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByPercentage", "Швидкість пересування під час атаки {0}{1}%" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByValue", "Швидкість пересування під час атаки {0}{1}" },
      { "Gem/Upgrade/AugmentMovingSpeedByPercentage", "Швидкість пересування {0}{1}%" },
      { "Gem/Upgrade/AugmentMovingSpeedByValue", "Швидкість пересування {0}{1}" },
      { "Gem/Upgrade/CoolingCountdownDecrementByPercentage", "Час відновлення базової навички {0}{1}%" },
      { "Gem/Upgrade/ReloadCountdownDecrementByPercentage", "Час перезарядки базової навички {0}{1}%" },
      { "Gem/Upgrade/DispersionDecrementByPercentage", "Розсіювання базової навички {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByPercentage", "Проникнення базової навички {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByValue", "Проникнення базової навички {0}{1}" },
      { "Gem/Upgrade/RangeIncrementByPercentage", "Радіус дії базової навички {0}{1}%" },
      { "Gem/Upgrade/RangeIncrementByValue", "Радіус дії базової навички {0}{1}" },
      { "Gem/Upgrade/RepelForceIncrementByPercentage", "Сила відштовхування базової навички {0}{1}%" },
      { "Gem/Upgrade/RepelForceIncrementByValue", "Сила відштовхування базової навички {0}{1}" },
      { "Gem/Upgrade/SkillCountIncrementByValue", "Снаряд базової навички {0}{1}" },
      { "Gem/Upgrade/SpeedIncrementByPercentage", "Швидкість базової навички {0}{1}%" },
      { "Gem/Upgrade/SpeedIncrementByValue", "Швидкість базової навички {0}{1}" },
      { "Gem/Upgrade/EnableRangeAttack", "Увімкнути атаку по дальності для базової навички" },
      { "Gem/Upgrade/AugmentDamageRangeByValue", "Діапазон шкоди базової навички {0}{1}" },
      { "Gem/Upgrade/AugmentDamageRangeByPercentage", "Діапазон шкоди базової навички {0}{1}%" },
      { "Gem/Upgrade/AddPoisoningAdditionalEffect", "Додати додатковий ефект отруєння" },
      { "Gem/Upgrade/SetPoisoningPossibility", "Встановити ймовірність отруєння на {0}%" },
      { "Gem/Upgrade/SetPoisoningHurtPercentage", "Встановити відсоток шкоди від отруєння на {0}%" },
      { "Gem/Upgrade/AugmentPoisoningPossibility", "Ймовірність отруєння навички {0}{1}%" },
      { "Gem/Upgrade/AugmentPoisoningHurtPercentage", "Відсоток шкоди від отруєння навички {0}{1}%" },
      {
        "Gem/Upgrade/AddOrConvertBasicAdditionalEffect",
        "Додати або перетворити атрибут додаткового ефекту на <b>{0}</b>"
      },
      {
        "Gem/Upgrade/SetBasicAdditionalEffectPossibility", "Встановити ймовірність додаткового ефекту навички на {0}%"
      },
      { "Gem/Upgrade/AugmentBasicAdditionalEffectPossibility", "Ймовірність додаткового ефекту навички {0}{1}%" },
      { "Gem/Upgrade/AugmentPhysicalHurtByValue", "Фізична шкода {0}{1}" },
      { "Gem/Upgrade/AugmentPhysicalHurtByPercentage", "Фізична шкода {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicIceHurtByValue", "Шкода магією льоду {0}{1}" },
      { "Gem/Upgrade/AugmentMagicIceHurtByPercentage", "Шкода магією льоду {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicFireHurtByValue", "Шкода магією вогню {0}{1}" },
      { "Gem/Upgrade/AugmentMagicFireHurtByPercentage", "Шкода магією вогню {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicThunderByValue", "Шкода магією блискавки {0}{1}" },
      { "Gem/Upgrade/Augment/MagicThunderByPercentage", "Шкода магією блискавки {0}{1}%" },
      { "Gem/Upgrade/Augment/Resurrection", "Кількість воскресінь {0}{1}" },
      { "Gem/Upgrade/Augment/IncreaseMaxHp", "Максимальне HP {0}{1}" },
      {
        "Rune/Description/ClonedProjectile", "Збільшує кількість базових навичок. Кожне покращення додає один снаряд."
      },
      {
        "Rune/Description/ExpBonus",
        "Збільшує досвід, отриманий героями, на 10%. Досвід, отриманий при підвищенні рівня героя, збільшується на 7% щоразу."
      },
      {
        "Rune/Description/Fission",
        "Після того, як базові навички героя потрапляють у ворога, є певний шанс розділення. Початкова ймовірність розділення становить 10% і збільшується на 2% з кожним покращенням."
      },
      {
        "Rune/Description/HailStrike",
        "активні навички. При активації викликає град для атаки ворогів в межах видимості. Навичка триває 7 секунд і збільшується на 2 секунди з кожним покращенням."
      },
      {
        "Rune/Description/HolyShield",
        "активні навички. Після активації герой отримує 10 секунд непереможності, і кожне покращення збільшує цей час на 2 секунди."
      },
      {
        "Rune/Description/IncreaseHonor",
        "Наприкінці гри значення честі, отримане гравцем, збільшується. Початкове збільшення становить 10%, і кожне оновлення збільшує його на 10%."
      },
      {
        "Rune/Description/InstantKill",
        "Коли базові навички героя вражають ворога, є певна ймовірність того, що ворог миттєво помре. Початкова ймовірність становить 1%, і кожне оновлення збільшує її на 0,5%."
      },
      {
        "Rune/Description/InstantReload",
        "Є 10% шанс миттєво перезарядити зброю, коли закінчуються патрони. Кожне оновлення збільшує цей шанс на 5%."
      },
      {
        "Rune/Description/KillAndRecover",
        "активні навички. Щоразу, коли герой вбиває ворога протягом 10 секунд, він може відновити 1% свого здоров'я. Тривалість кожного оновлення збільшується на 2 секунди."
      },
      {
        "Rune/Description/IncreaseImmortalTime",
        "Збільшує час невразливості після отримання пошкодження. Початковий стан збільшується на 0,25 секунди, і кожне оновлення збільшує його на 0,15 секунди."
      },
      {
        "Rune/Description/IncreaseMaxHp",
        "Збільшує максимальне здоров'я героя на 10%, і кожне оновлення збільшує його на 5%."
      },
      {
        "Rune/Description/MeteoriteStrike",
        "активні навички. Викликає метеорити для атаки ворогів в межах видимості протягом 10 секунд. Спочатку падає 10 метеоритів за секунду, і кожне оновлення збільшує їх кількість на 5."
      },
      {
        "Rune/Description/PickUpDistance",
        "Відстань підбирання предметів героєм збільшується на 10%, і кожне оновлення збільшує її на 10%."
      },
      {
        "Rune/Description/Poisonous",
        "активні навички. Завдає шкоди отрутою ворогам в межах видимості щосекунди протягом 10 секунд. Кожен рівень збільшує тривалість на 2 секунди."
      },
      {
        "Rune/Description/PushAway",
        "Відштовхує найближчих ворогів щоразу, коли у героя закінчуються патрони (10 секунд перезарядки). Кожне оновлення збільшує силу поштовху на 20%."
      },
      {
        "Rune/Description/HpRecovery",
        "Відновлює 0,2% HP героя за секунду, і кожне оновлення збільшує кількість відновлення на 0,2%."
      },
      { "Rune/Description/ReducedInjuery", "Урон, що отримує герой, зменшується на 5%, і додатково на 5% за кожен рівень." },
      {
        "Rune/Description/Resurrection",
        "Коли герой помирає, він миттєво воскресає з відновленими 25% здоров'я. Після воскресіння HP збільшується на 15% з кожним оновленням."
      },
      {
        "Rune/Description/ThunderStrike",
        "активні навички. Викликає блискавки для точної атаки ворогів в межах видимості. Навичка триває 10 секунд. Спочатку атакує 10% ворогів за секунду, кожне оновлення атакуватиме додаткові 3% ворогів."
      },
      {
        "Rune/Description/TimeStop",
        "Призупиняє всі дії ворогів на 5 секунд. Кожне оновлення збільшує тривалість паузи на 2 секунди."
      },
      { "Rune/Title/ClonedProjectile", "Клонований снаряд" },
      { "Rune/Title/ExpBonus", "Книга досвіду" },
      { "Rune/Title/Fission", "Поділ" },
      { "Rune/Title/HailStrike", "Град" },
      { "Rune/Title/HolyShield", "Святий щит" },
      { "Rune/Title/IncreaseHonor", "Чемпіон" },
      { "Rune/Title/InstantKill", "Один постріл - одне вбивство" },
      { "Rune/Title/InstantReload", "Мульти-магазин" },
      { "Rune/Title/KillAndRecover", "Кровожерливий" },
      { "Rune/Title/IncreaseImmortalTime", "Лицарський шолом" },
      { "Rune/Title/IncreaseMaxHp", "Сильна рука" },
      { "Rune/Title/MeteoriteStrike", "Метеорит" },
      { "Rune/Title/PickUpDistance", "Мисливець за головами" },
      { "Rune/Title/Poisonous", "Газова зона" },
      { "Rune/Title/PushAway", "Залишити в спокої" },
      { "Rune/Title/HpRecovery", "Червоний хрест" },
      { "Rune/Title/ReducedInjuery", "Броньований лицар" },
      { "Rune/Title/Resurrection", "Диво" },
      { "Rune/Title/ThunderStrike", "Удар блискавки" },
      { "Rune/Title/TimeStop", "Машина часу" },
      { "Exception/CorruptedSaveFile", "Файл збереження пошкоджено та створено резервну копію в {0}." },
    };
  }
}