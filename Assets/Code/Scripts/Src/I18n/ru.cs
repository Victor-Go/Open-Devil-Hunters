using System.Collections.Generic;

namespace Code.Scripts.Src.I18n
{
  public struct ru : ILanguage
  {
    public static readonly Dictionary<string, string> locales = new()
    {
      { "HelloWorld", "Привет, Мир!" },
      { "Language", "Русский" },
      { "NotAvailableInDemo", "В разработке" },
      { "Player1", "Игрок 1" },
      { "Player2", "Игрок 2" },
      {
        "Notification/WelcomeNotification",
        "Спасибо за покупку нашей игры!Мы знаем, что наша игра не идеальна, поэтому нам нужны ваши отзывы!Следите за постоянными обновлениями."
      },
      { "Measure/PerSecond", "/сек" },
      {
        "UI/FightPreparationGuide/Page0/0",
        "Атаки героев и врагов имеют 5 атрибутов: физический, лед, огонь, гром и яд."
      },
      {
        "UI/FightPreparationGuide/Page0/1",
        "Среди них атрибут льда сдерживает атрибут огня, атрибут огня сдерживает атрибут грома, а атрибут грома сдерживает атрибут льда."
      },
      {
        "UI/FightPreparationGuide/Page1/0",
        "Атрибут льда может вызвать эффект замораживания и нанести численный урон;\nАтрибут огня может вызвать эффект возгорания и нанести пропорциональный урон;\nАтрибут грома может вызвать эффект оглушения, но не нанесет дополнительного урона.\nВсе вышеуказанные атрибуты будут устранены через некоторое время.\nАтрибут яда может вызвать отравление и продолжать наносить пропорциональный урон. Но боссы часто могут облегчить или даже устранить отравление."
      },
      {
        "UI/FightPreparationGuide/Page1/1",
        "Некоторые враги имеют защиту от атрибутов, иммунитет к эффектам атрибутов и пробитие защиты атрибутов. Правильное использование атрибутов, которые сдерживают врага, может облегчить победу над противником!"
      },
      {
        "UI/FightReadyGuide/Page0/0",
        "Здесь будут показаны атрибуты монстров на текущей карте. Запомните эти атрибуты и подберите свою стратегию, чтобы победить монстров!"
      },
      {
        "UI/FightReadyGuide/Page1/0",
        "Эти две позиции покажут боссов, сидящих на карте. Каждый босс имеет разные боевые навыки и стили. Попробуйте использовать разные техники, чтобы победить разных боссов!"
      },
      {
        "UI/FightReadyGuide/Page1/1",
        "Не забудьте исследовать карту чаще, где вы можете собирать разные драгоценные камни, чтобы усилить своих героев.Вы также можете использовать почести, которые вы получаете в битвах, чтобы разблокировать новых героев и руны для лучшего исследования на разных картах!"
      },
      { "UI/ControlGuide/ActiveSkill", "Активное умение" },
      { "UI/ControlGuide/AutoFiring", "Автоматическая стрельба" },
      { "UI/ControlGuide/Movement", "Движение" },
      { "UI/ControlGuide/Fire!", "Огонь!" },
      { "UI/ControlGuide/AutoAiming", "Автоматическое прицеливание" },
      { "UI/ControlGuide/Aim", "Прицеливание" },
      {
        "UI/ControlGuide/MoveToUseController",
        "Переместите джойстик, чтобы использовать прицеливание контроллером (не в режиме автоприцеливания)."
      },
      {
        "UI/ControlGuide/MoveToUseMouse",
        "Переместите мышь, чтобы использовать прицеливание мышью (не в режиме автоприцеливания)."
      },
      { "UI/Control/AutoAiming", "Автоприцеливание" },
      { "UI/Control/MouseAiming", "Прицеливание мышью" },
      { "UI/Control/ControllerAiming", "Прицеливание контроллером" },
      { "UI/Control/AutoFiring", "Автоматическая стрельба" },
      { "UI/Control/ManualFiring", "Ручная стрельба" },
      { "UI/Control/UseMouseSelectHero", "Используйте мышь, чтобы выбрать героя" },
      { "UI/Text/LevelUp!", "Повышение уровня!" },
      { "UI/Button/Choose", "Выбрать" },
      { "UI/Start", "Старт" },
      { "UI/Languages", "Языки" },
      { "UI/Options", "Настройки" },
      { "UI/Exit", "Выход" },
      { "UI/Name", "Имя" },
      { "UI/Description", "Описание" },
      { "UI/Properties", "Свойства" },
      { "UI/Properties/AttacksPerRound", "Атак за раунд" },
      { "UI/Properties/RateOfFire", "Скорострельность" },
      { "UI/Properties/ReloadTime", "Время перезарядки" },
      { "UI/Properties/Projectiles", "Снаряды" },
      { "UI/Properties/FreezeDamagePerSecond", "Урон в секунду" },
      { "UI/Properties/MovingSpeed", "Скорость передвижения" },
      { "UI/Properties/Physical", "Физический" },
      { "UI/Properties/Ice", "Лед" },
      { "UI/Properties/Fire", "Огонь" },
      { "UI/Properties/Thunder", "Гром" },
      { "UI/Properties/Poisoning", "Отравление" },
      { "UI/GameOver", "Конец игры" },
      { "UI/FinalScore", "Финальный счет" },
      { "UI/HonorGained", "Полученный почет" },
      { "UI/PlayerHonor", "Почет игрока" },
      { "UI/PlayerScore", "Счет игрока" },
      { "UI/EnemyKilled", "Убитый враг" },
      { "UI/ExperienceGained", "Полученный опыт" },
      { "UI/FinalLevel", "Финальный уровень" },
      { "UI/SurvivalTime", "Время выживания" },
      { "UI/Upgrades", "Улучшения" },
      { "UI/GameOver/Failed", "ПОРАЖЕНИЕ!" },
      { "UI/GameOver/Success", "УРОВЕНЬ ПРОЙДЕН!" },
      { "UI/GameOver/Aborted", "МИССИЯ ПРЕРВАНА" },
      { "UI/Achievements", "Достижения" },
      { "UI/Furnace", "Печь" },
      { "UI/Gems", "Камни" },
      { "UI/Runes", "Руны" },
      { "UI/Difficulty", "Сложность" },
      { "UI/Mode", "Режим игры" },
      { "UI/CasualMode", "Обычный режим" },
      { "UI/ChallengeMode", "Режим испытаний" },
      { "UI/InfiniteMode", "Бесконечный режим" },
      { "UI/Play", "Играть" },
      { "UI/Confirm", "Подтвердить" },
      { "UI/Cancel", "Отменить" },
      { "UI/GemList", "Список камней" },
      { "UI/HeroDescription/MaxHp", "Макс. HP" },
      { "UI/HeroDescription/HpRecover", "Восстановление HP" },
      { "UI/HeroDescription/Resurrection", "Воскрешение" },
      { "UI/HeroDescription/BasicSkillHurt", "Урон от базового навыка" },
      { "UI/HeroDescription/BasicSkillProperty", "Атрибут базового навыка" },
      { "UI/HeroDescription/Possibility", "Возможность AE" },
      { "UI/HeroDescription/AttackRange", "Дальность атаки" },
      { "UI/HeroDescription/AttackPerRound", "Атак за раунд" },
      { "UI/HeroDescription/AttackSpeed", "Скорость атаки" },
      { "UI/HeroDescription/PickUpRange", "Дальность подбора" },
      { "UI/HeroDescription/MovingSpeed", "Скорость передвижения" },
      { "UI/HeroDescription/AttackMovingSpeed", "Скорость передвижения в атаке" },
      { "UI/HeroDescription/BaseSkill", "Базовый навык" },
      { "UI/HeroDescription/AdditionalEffect", "Дополнительный эффект" },
      { "UI/HeroDescription/ProjectileCount", "Количество снарядов" },
      { "UI/HeroDescription/AEHurt", "Урон AE" },
      { "UI/FightPreparation/TwoPlayersUniqueHero", "Два игрока не могут выбрать одного и того же героя" },
      { "UI/FightPreparation/BrowserHeroes", "Герои браузера" },
      { "UI/FightPreparation/Back", "Назад" },
      { "UI/FightPreparation/ApplyGems", "Надеть камни" },
      { "UI/FightPreparation/ApplyRunes", "Активировать руны" },
      { "UI/FightPreparation/SelectHero", "Выбрать героя" },
      { "UI/FightPreparation/Unlock", "Разблокировать" },
      {
        "UI/FightPreparation/AddPlayer",
        "Нажмите <color=red>A</color> на геймпаде или правую Ctrl на клавиатуре, чтобы добавить 2-го игрока"
      },
      { "UI/FightPreparation/RemovePlayer", "Удалить" },
      { "UI/PickGemUI/PickGemTitle", "Надеть камни" },
      { "UI/PickGemUI/OneGemOnePlayer", "Один камень может быть использован только одним игроком" },
      { "UI/PickRuneUI/PickRuneTitle", "Активировать руны" },
      { "UI/PickRuneUI/Class", "Уровень" },
      { "UI/PickRuneUI/InspirationRune", "Руна вдохновения" },
      { "UI/PickRuneUI/DominationRune", "Руна господства" },
      { "UI/PickRuneUI/ImmortalRune", "Руна бессмертия" },
      { "UI/PickRuneUI/Upgrade", "Улучшить" },
      { "UI/PickRuneUI/Price", "Цена" },
      { "UI/PickRuneUI/OneRunePerClass", "Можно выбрать только одну руну на уровень" },
      { "UI/Pause/PhysicalAttack", "Физическая атака" },
      { "UI/Pause/IceAttack", "Ледяная атака" },
      { "UI/Pause/FireAttack", "Огненная атака" },
      { "UI/Pause/ThunderAttack", "Атака молнией" },
      { "UI/Pause/Poisoning", "Отравление" },
      { "UI/Pause/MovingSpeed", "Скорость передвижения" },
      { "UI/Pause/AttackingSpeed", "Скорость при атаке" },
      { "UI/Pause/AttackSpeed", "Скорость атаки" },
      { "UI/Pause/RecoverSpeed", "Скорость восстановления" },
      { "UI/Pause/ExpBonus", "Бонус опыта" },
      { "UI/Pause/RoundsPerSecond", " раундов/с" },
      { "UI/Pause/Resume", "Продолжить" },
      { "UI/Pause/ControlGuide", "Руководство по управлению" },
      { "UI/Pause/GiveUp", "Сдаться" },
      {
        "UI/Upgrade/DemoUpgradeUnlocked",
        "Улучшения, уже разблокированные в демо-версии: 96/188В официальной версии могут быть изменения в улучшениях"
      },
      { "UI/GameOver/Quit", "Выйти" },
      { "UI/FightReady/Difficulty", "Сложность" },
      { "UI/FightReady/DifficultyNumber", "Уровень.{0}" },
      { "UI/FightReady/UnlockedDifficulty", "Разблокировано {0}/{1}" },
      { "UI/FightReady/Mode", "Режим" },
      { "UI/FightReady/Map", "Карта" },
      { "UI/FightReady/Description", "Описание" },
      { "UI/FightReady/Casual", "Обычный" },
      { "UI/FightReady/Standard", "Стандартный" },
      { "UI/FightReady/Infinite", "Бесконечный" },
      { "UI/FightReady/Forest", "Туманный лес" },
      { "UI/FightReady/Desert", "Пылающая пустыня" },
      { "UI/FightReady/Dungeon", "Темное подземелье" },
      { "UI/FightReady/Graveyard", "Кладбище смерти" },
      { "UI/FightReady/Hell", "Преисподняя" },
      { "UI/FightReady/ModeDescription/CasualMode", "Сложность монстров будет снижена в обычном режиме." },
      { "UI/FightReady/ModeDescription/BossAppearTime", "Босс появится через {0} минут" },
      {
        "UI/FightReady/AttributeDescription",
        "Соотношение атрибутов врага: Физический - {0}%, Лед - {1}%, Огонь - {2}%, Молния - {3}%, Яд - {4}%"
      },
      { "UI/Prompt/UnlockRune", "Разблокировать руну <b>{0}</b> за {1}, продолжить?" },
      { "UI/Prompt/UnlockHero", "Разблокировать героя <b>{0}</b> за {1}, продолжить?" },
      { "UI/Info/Unlock/InsufficientBalance", "Извините, недостаточно средств для разблокировки. (Требование: {0})" },
      { "UI/Info/Upgrade/InsufficientBalance", "Извините, недостаточно средств для улучшения." },
      { "UI/Info/RunePrerequisitesNotSatisfied", "Пожалуйста, сначала разблокируйте руну предыдущего уровня." },
      { "UI/Audio/Overall", "Общее" },
      { "UI/Audio/Bgm", "BGM" },
      { "UI/Audio/Sfx", "SFX" },
      { "Physical", "Физический" },
      { "Ice", "Лед" },
      { "Thunder", "Молния" },
      { "Fire", "Огонь" },
      { "Poison", "Яд" },
      { "Hero/Archangel", "Серафиэль" },
      { "Hero/CaptainG", "Шквал пуль" },
      { "Hero/Cutie", "Фэй Спарк" },
      { "Hero/Gumdam", "Удар кометы" },
      { "Hero/JeanneDArc", "Святая Жанна" },
      { "Hero/MountainKing", "Повелитель грома" },
      { "Hero/Paladin", "Железный тамплиер" },
      { "Hero/Ranger", "Веномстрайкер" },
      { "Hero/Witch", "Арканист" },
      { "Hero/WuKong", "Сунь Укун" },
      { "Hero/ZhaoYun", "Чжао Юнь" },
      {
        "UI/Hero/Description/Archangel",
        "Серафиэль стреляет сосульками с возможностью вызвать Замерзание. Она может восстанавливать некоторое количество здоровья в секунду и невосприимчива к урону от Ада."
      },
      {
        "UI/Hero/Description/CaptainG",
        "Шквал пуль стреляет 3 огненными пулями с возможностью вызвать Горение. Также он невосприимчив к Ожогу."
      },
      { "UI/Hero/Description/Cutie", "Фэй Спарк стреляет пулями с бесконечным проникновением." },
      {
        "UI/Hero/Description/Gumdam",
        "Удар кометы стреляет 2 ракетами из базуки, которая взрывается, нанося урон всем ближайшим врагам. Но у ракет могут быть мертвые зоны."
      },
      {
        "UI/Hero/Description/JeanneDArc",
        "Святая Жанна атакует своим мечом (Атака в ближнем бою), который может пронзать врагов. Кроме того, ей не нужно время перезарядки."
      },
      {
        "UI/Hero/Description/MountainKing",
        "Повелитель грома выпускает молнии, которые могут оглушить врага, и также невосприимчив к Оглушению."
      },
      {
        "UI/Hero/Description/Paladin",
        "Железный тамплиер может восстанавливать небольшое количество здоровья в секунду и невосприимчив к урону от Ада."
      },
      {
        "UI/Hero/Description/Ranger",
        "Веномстрайкер стреляет 2 ядовитыми стрелами, которые автоматически кружат вокруг цели. Но у стрел могут быть мертвые зоны."
      },
      {
        "UI/Hero/Description/Witch",
        "Чародейка выпускает 2 шара молнии для атаки врага. У неё есть одна дополнительная опция улучшения при повышении уровня."
      },
      {
        "UI/Hero/Description/WuKong",
        "Сунь Укун может телепортироваться в другие места (нажмите кнопку три раза быстро, чтобы активировать)."
      },
      {
        "UI/Hero/Description/ZhaoYun",
        "Чжао Юнь стреляет 3 энергиями ледяного меча во всех направлениях, чтобы атаковать врага. Также он невосприимчив к Замерзанию."
      },
      { "Level/Pause", "Пауза" },
      { "Level/PressEscToResume", "Нажмите Esc для возобновления" },
      { "Upgrade/RecoverHp/Name", "Восстановить Ур{0}" },
      { "Upgrade/RecoverHp/Description", "Восстановить {0:F2}% HP" },
      { "Upgrade/IncreaseMaxHp/Name", "Макс. HP Ур{0}" },
      { "Upgrade/IncreaseMaxHp/Description", "Увеличить макс. HP на {0:F2}%" },
      { "Upgrade/IncreaseMaxHpOnHurt/Name", "Страх Ур{0}" },
      {
        "Upgrade/IncreaseMaxHpOnHurt/Description",
        "Увеличить макс. HP на {0:F2}% , когда игрок ранен. Максимальное увеличение составит {1:F2}%."
      },
      { "Upgrade/IncreaseDefense/Name", "Щит Ур{0}" },
      { "Upgrade/IncreaseDefense/Description", "Увеличить защиту на {0:F2}%" },
      { "Upgrade/IncreaseAllSpeed/Name", "Давай быстрее! Ур{0}" },
      {
        "Upgrade/IncreaseAllSpeed/Description",
        "Увеличить скорость на {0:F2}%, включая обычную скорость и скорость атаки."
      },
      { "Upgrade/IncreaseMovingSpeed/Name", "Бег трусцой Ур{0}" },
      { "Upgrade/IncreaseMovingSpeed/Description", "Увеличить обычную скорость передвижения на {0:F2}%." },
      { "Upgrade/IncreaseAttackingMovingSpeed/Name", "Ударь и беги Ур{0}" },
      {
        "Upgrade/IncreaseAttackingMovingSpeed/Description", "Увеличить скорость передвижения во время атаки на {0:F2}%."
      },
      { "Upgrade/KillToIncreaseSpeed/Name", "Наемник Ур{0}" },
      {
        "Upgrade/KillToIncreaseSpeed/Description",
        "Убейте {0} врагов, чтобы увеличить скорость на {1:F2}%, и максимальное увеличение составит {2:F2}%. Увеличение скорости будет сброшено после получения урона."
      },
      { "Upgrade/ExpBonus/Name", "Нерд Ур{0}" },
      { "Upgrade/ExpBonus/Description", "Увеличивает получаемый опыт каждый раз на {0:F2}%." },
      { "Upgrade/IncreasePickUp/Name", "Индиана Джонс Ур{0}" },
      { "Upgrade/IncreasePickUp/Description", "Увеличить радиус подбора на {0:F2}%." },
      { "Upgrade/IncreaseShootingRange/Name", "Барретт Ур{0}" },
      { "Upgrade/IncreaseShootingRange/Description", "Увеличить дальность стрельбы на {0:F2}%." },
      { "Upgrade/AddProjectile/Name", "Дробовик Ур{0}" },
      {
        "Upgrade/AddProjectile/Description",
        "Добавить {0} дополнительных снарядов. Однако рассеивание немного увеличится."
      },
      { "Upgrade/IncreaseDispersion/Name", "Рассеивание Ур{0}" },
      { "Upgrade/IncreaseDispersion/Description", "Увеличить рассеивание на {0:F2}%." },
      { "Upgrade/ReduceDispersion/Name", "Снайперская винтовка Ур{0}" },
      { "Upgrade/ReduceDispersion/Description", "Уменьшить рассеивание на {0:F2}%." },
      { "Upgrade/IncreaseFiringRate/Name", "Быстрая стрельба Ур{0}" },
      { "Upgrade/IncreaseFiringRate/Description", "Увеличивает скорость стрельбы на {0:F2}%" },
      { "Upgrade/BurstFire/Name", "Серийный огонь Уровень {0}" },
      {
        "Upgrade/BurstFire/Description",
        "Увеличивает скорость стрельбы на {0:F2}% в течение {1:F2} секунд после получения урона."
      },
      { "Upgrade/IncreaseMagazineSize/Name", "Барабанный магазин Уровень {0}" },
      { "Upgrade/IncreaseMagazineSize/Description", "Увеличивает размер магазина на {0}." },
      { "Upgrade/ReduceReloadingTime/Name", "Быстрая зарядка Уровень {0}" },
      { "Upgrade/ReduceReloadingTime/Description", "Сокращает время перезарядки на {0:F2}%" },
      { "Upgrade/BurstReload/Name", "Зарядка Страхом Уровень {0}" },
      {
        "Upgrade/BurstReload/Description",
        "Сокращает время перезарядки на {0:F2}% в течение {1:F2} секунд после получения урона."
      },
      { "Upgrade/IncreaseHurtPercentage/Name", "Заточка Уровень {0}" },
      { "Upgrade/IncreaseHurtPercentage/Description", "Увеличивает урон базового навыка на {0:F2}%" },
      { "Upgrade/IncreaseIceAeDuration/Name", "Ледяной Уровень {0}" },
      { "Upgrade/IncreaseIceAeDuration/Description", "Увеличивает длительность эффекта Заморозки на {0:F2}%" },
      { "Upgrade/IncreaseFireAeDuration/Name", "Горение Уровень {0}" },
      { "Upgrade/IncreaseFireAeDuration/Description", "Увеличивает длительность эффекта Горения на {0:F2}%" },
      { "Upgrade/IncreaseThunderAeDuration/Name", "Гроза Уровень {0}" },
      { "Upgrade/IncreaseThunderAeDuration/Description", "Увеличивает длительность эффекта Оглушения на {0:F2}%" },
      { "Upgrade/ActivateDart/Name", "Ниндзя" },
      { "Upgrade/ActivateDart/Description", "Бросает дротик, который вращается вокруг вас." },
      { "Upgrade/AddDart/Name", "Сэнсей Уровень {0}" },
      { "Upgrade/AddDart/Description", "Бросает на {0} дротиков больше вокруг вас." },
      { "Upgrade/PoisonDart/Name", "Отравленный дротик Уровень {0}" },
      {
        "Upgrade/PoisonDart/Description",
        "Смачивает все дротики ядом, давая им {0:F2}% шанс отравить врага и нанести {1:F2}% урона в секунду (неэффективно против боссов)."
      },
      { "Upgrade/ImproveDartHurt/Name", "Заточенный Уровень {0}" },
      { "Upgrade/ImproveDartHurt/Description", "Затачивает все дротики вокруг вас, улучшая урон на {0:F2}%" },
      { "Upgrade/ActivateBoomerang/Name", "Абориген" },
      { "Upgrade/ActivateBoomerang/Description", "Бросает бумеранг, который вращается вокруг вас." },
      { "Upgrade/AddBoomerang/Name", "Мануэль Шютц Уровень {0}" },
      { "Upgrade/AddBoomerang/Description", "Бросает на {0} бумерангов больше вокруг вас." },
      { "Upgrade/BurnBoomerang/Name", "Огненный бумеранг Уровень {0}" },
      {
        "Upgrade/BurnBoomerang/Description",
        "Дает бумерангу {0:F2}% шанс поджечь врага на {1:F2} секунд, нанося {2:F2}% урона в секунду."
      },
      { "Upgrade/IncreaseBoomerangSpeed/Name", "Аэродинамика Уровень {0}" },
      { "Upgrade/IncreaseBoomerangSpeed/Description", "Оптимизирует аэродинамику для ускорения бумеранга на {0:F2}%" },
      { "Upgrade/ImproveBoomerangHurt/Name", "Свинцовая начинка Уровень {0}" },
      { "Upgrade/ImproveBoomerangHurt/Description", "Заряжает бумеранг свинцом, увеличивая его силу атаки на {0:F2}%" },
      { "Upgrade/ActivateIceTower/Name", "Ледяная башня" },
      {
        "Upgrade/ActivateIceTower/Description",
        "Запускает автоматическую Ледяную башню вокруг вас, которая автоматически стреляет градом."
      },
      { "Upgrade/AddIceTower/Name", "Зима Уровень {0}" },
      { "Upgrade/AddIceTower/Description", "Добавляет {0} Ледяных башен." },
      { "Upgrade/IncreaseIceTowerFiringRate/Name", "Скади Уровень {0}" },
      { "Upgrade/IncreaseIceTowerFiringRate/Description", "Увеличивает скорость атаки Ледяной башни на {0:F2}%" },
      { "Upgrade/ImproveIceTowerHurt/Name", "Кубик льда Уровень {0}" },
      { "Upgrade/ImproveIceTowerHurt/Description", "Улучшает урон Ледяной башни на {0:F2}%" },
      { "Upgrade/ActivateFireTower/Name", "Огненная башня" },
      {
        "Upgrade/ActivateFireTower/Description",
        "Запускает автоматическую Огненную башню вокруг вас, которая автоматически стреляет огненными шарами."
      },
      { "Upgrade/AddFireTower/Name", "Знойный Уровень {0}" },
      { "Upgrade/AddFireTower/Description", "Добавляет {0} Огненных башен." },
      { "Upgrade/IncreaseFireTowerFiringRate/Name", "Аполлон Уровень {0}" },
      { "Upgrade/IncreaseFireTowerFiringRate/Description", "Увеличивает скорость атаки Огненной башни на {0:F2}%" },
      { "Upgrade/ImproveFireTowerHurt/Name", "Огнемет Уровень {0}" },
      { "Upgrade/ImproveFireTowerHurt/Description", "Улучшает урон Огненной башни на {0:F2}%" },
      { "Upgrade/ActivateThunderTower/Name", "Башня молний" },
      {
        "Upgrade/ActivateThunderTower/Description",
        "Запускает автоматическую Башню молний вокруг вас, которая автоматически стреляет молниями."
      },
      { "Upgrade/AddThunderTower/Name", "Молния Уровень {0}" },
      { "Upgrade/AddThunderTower/Description", "Добавляет {0} Башен молний." },
      { "Upgrade/IncreaseThunderTowerFiringRate/Name", "Тор Уровень {0}" },
      { "Upgrade/IncreaseThunderTowerFiringRate/Description", "Увеличивает скорость атаки Башни молний на {0:F2}%" },
      { "Upgrade/ImproveThunderTowerHurt/Name", "Катушка Тесла" },
      { "Upgrade/ImproveThunderTowerHurt/Description", "Улучшает урон Башни молний на {0:F2}%" },
      { "Upgrade/ActivateSpiral/Name", "Спираль" },
      {
        "Upgrade/ActivateSpiral/Description",
        "Время от времени герой выпускает спираль со случайными атрибутами, чтобы атаковать врага."
      },
      { "Upgrade/AddSpiral/Name", "Наутилус Уровень {0}" },
      { "Upgrade/AddSpiral/Description", "Добавляет {0} спиралей вокруг вас." },
      { "Upgrade/ReduceSpiralInterval/Name", "Ливень Уровень {0}" },
      { "Upgrade/ReduceSpiralInterval/Description", "Сокращает интервал Спирали на {0:F2}%" },
      { "Upgrade/IncreaseSpiralHurt/Name", "Гроза Уровень {0}" },
      { "Upgrade/IncreaseSpiralHurt/Description", "Увеличивает урон Спирали на {0:F2}%" },
      { "Upgrade/ActivatePuppet/Name", "Кукловод" },
      { "Upgrade/ActivatePuppet/Description", "Призывает {0} кукол каждые {1:F2} секунд." },
      { "Upgrade/LevelUpPuppetToLv1/Name", "Вооруженная кукла" },
      {
        "Upgrade/LevelUpPuppetToLv1/Description",
        "Призывает {0} улучшенных кукол каждые {1:F2} секунд. Однако улучшения текущих кукол сбрасываются."
      },
      { "Upgrade/LevelUpPuppetToLv2/Name", "Кукла Сайя" },
      {
        "Upgrade/LevelUpPuppetToLv2/Description",
        "Призывает {0} совершенных кукол каждые {1:F2} секунд. Однако улучшения текущих кукол сбрасываются."
      },
      { "Upgrade/ReducePuppetIntervalLv0/Name", "Команда Уровень {0}" },
      { "Upgrade/ReducePuppetIntervalLv0/Description", "Сокращает интервал призывания кукол на {0:F2}%" },
      { "Upgrade/AddPuppetLv0/Name", "Крик Уровень {0}" },
      { "Upgrade/AddPuppetLv0/Description", "Призывает на {0} кукол больше для групповых боев." },
      { "Upgrade/IncreasePuppetHurtLv0/Name", "Нож Уровень {0}" },
      { "Upgrade/IncreasePuppetHurtLv0/Description", "Увеличивает урон всех кукол на {0:F2}%" },
      { "Upgrade/ReducePuppetIntervalLv1/Name", "Пейджер Уровень {0}" },
      { "Upgrade/ReducePuppetIntervalLv1/Description", "Сокращает интервал призывания кукол на {0:F2}%" },
      { "Upgrade/AddPuppetLv1/Name", "Мафия Уровень {0}" },
      {
        "Upgrade/AddPuppetLv1/Description", "Призывает на {0} кукол больше для групповых боев."
      },
      { "Upgrade/IncreasePuppetHurtLv1/Name", "Пистолет Уровень {0}" },
      { "Upgrade/IncreasePuppetHurtLv1/Description", "Увеличивает урон всех кукол на {0:F2}%" },
      { "Upgrade/ReducePuppetIntervalLv2/Name", "Смартфон Уровень {0}" },
      { "Upgrade/ReducePuppetIntervalLv2/Description", "Сокращает интервал призывания кукол на {0:F2}%" },
      { "Upgrade/AddPuppetLv2/Name", "Армия Уровень {0}" },
      {
        "Upgrade/AddPuppetLv2/Description", "Призывает на {0} кукол больше для групповых боев."
      },
      { "Upgrade/IncreasePuppetHurtLv2/Name", "Винтовка Уровень {0}" },
      { "Upgrade/IncreasePuppetHurtLv2/Description", "Увеличивает урон всех кукол на {0:F2}%" },
      { "Upgrade/ActivateFlyingSword/Name", "Летающий меч" },
      {
        "Upgrade/ActivateFlyingSword/Description",
        "Использует Ци для управления мечами, выпуская {0} мечей каждые {1:F2} секунд."
      },
      { "Upgrade/LevelUpFlyingSwordToLv1/Name", "Тяжелый меч" },
      {
        "Upgrade/LevelUpFlyingSwordToLv1/Description",
        "Использует более тяжелые мечи, выпуская {0} мечей каждые {1:F2} секунд. Однако улучшения мечей будут сброшены."
      },
      { "Upgrade/LevelUpFlyingSwordToLv2/Name", "Заточенный меч" },
      {
        "Upgrade/LevelUpFlyingSwordToLv2/Description",
        "Использует заточенные мечи, выпуская {0} мечей каждые {1:F2} секунд. Однако улучшения мечей будут сброшены."
      },
      { "Upgrade/AddFlyingSwordLv0/Name", "Юниор Уровень {0}" },
      { "Upgrade/AddFlyingSwordLv0/Description", "Добавляет {0} летающих мечей." },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Name", "Подмастерье Уровень {0}" },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Description", "Уменьшить интервал выпуска мечей на {0:F2}%." },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Name", "Появление Мечника Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Description", "Увеличить урон мечей на {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv1/Name", "Старший Lv{0}" },
      { "Upgrade/AddFlyingSwordLv1/Description", "Добавить {0} летающих мечей." },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Name", "Ремесленник Lv{0}" },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Description", "Уменьшить интервал выпуска мечей на {0:F2}%." },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Name", "Старший Мечник Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Description", "Увеличить урон мечей на {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv2/Name", "Шифу Lv{0}" },
      { "Upgrade/AddFlyingSwordLv2/Description", "Добавить {0} летающих мечей." },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Name", "Охотник за Солнцем Lv{0}" },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Description", "Уменьшить интервал выпуска мечей на {0:F2}%." },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Name", "Зонгши lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Description", "Увеличить урон мечей на {0:F2}%." },
      { "Upgrade/ActivateMine/Name", "Наземная мина" },
      {
        "Upgrade/ActivateMine/Description",
        "Установить {0} мин, которые взрываются и наносят {1:F2} урона в радиусе {2:F2} метров каждые {3:F2} секунд."
      },
      { "Upgrade/LevelUpMineToLv1/Name", "Громовая мина" },
      {
        "Upgrade/LevelUpMineToLv1/Description",
        "Установить {0} улучшенных мин, которые взрываются и наносят {1:F2} урона в радиусе {2:F2} метров каждые {3:F2} секунд."
      },
      { "Upgrade/LevelUpMineToLv2/Name", "Мина Клеймор" },
      {
        "Upgrade/LevelUpMineToLv2/Description",
        "Установить {0} чрезвычайно смертоносных мин, которые взрываются и наносят {1:F2} урона в радиусе {2:F2} метров каждые {3:F2} секунд."
      },
      { "Upgrade/AddMineLv0/Name", "Бойскаут Lv{0}" },
      { "Upgrade/AddMineLv0/Description", "Каждый раз устанавливать на {0} больше наземных мин." },
      { "Upgrade/ReduceMineIntervalLv0/Name", "Плоскогубцы Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv0/Description", "Уменьшает интервал между минами на {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv0/Name", "Царапина Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv0/Description", "Увеличивает урон наземных мин на {0:F2}%." },
      { "Upgrade/AddMineLv1/Name", "Минутмен Lv{0}" },
      { "Upgrade/AddMineLv1/Description", "Каждый раз устанавливать на {0} больше громовых мин." },
      { "Upgrade/ReduceMineIntervalLv1/Name", "Ящик с инструментами Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv1/Description", "Уменьшает интервал между минами на {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv1/Name", "Стальной шар Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv1/Description", "Увеличивает урон громовых мин на {0:F2}%." },
      { "Upgrade/AddMineLv2/Name", "Спецназ Lv{0}" },
      { "Upgrade/AddMineLv2/Description", "Каждый раз устанавливать на {0} больше громовых мин." },
      { "Upgrade/ReduceMineIntervalLv2/Name", "Система мин Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv2/Description", "Уменьшает интервал между минами на {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv2/Name", "Фрагментация Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv2/Description", "Увеличивает урон мин Клеймор на {0:F2}%." },
      { "MapIndicator/Forest", "Туманный лес" },
      { "MapIndicator/Desert", "Пылающая пустыня" },
      { "MapIndicator/Dungeon", "Темное подземелье" },
      { "MapIndicator/Graveyard", "Кладбище смерти" },
      { "MapIndicator/Hell", "Преисподняя" },
      {
        "MapDescription/Forest",
        "Туманный лес, где пули часто вылетают из ниоткуда среди, казалось бы, мирных деревьев."
      },
      {
        "MapDescription/Desert",
        "Похоже, многие монстры, способные выжить в суровой пустыне, невосприимчивы к атрибутам."
      },
      { "MapDescription/Dungeon", "В подземелье часто слышны вой волков и звук ударов молотов." },
      {
        "MapDescription/Graveyard",
        "Остерегайтесь неуловимых черепов на кладбище! Вы можете пораниться, если столкнетесь с ними!"
      },
      {
        "MapDescription/Hell",
        "Ад заколдован дьяволом, нанося более или менее урона героям каждые 10 секунд! Но, похоже, некоторых героев это совсем не волнует."
      },
      { "AdditionalEffect/Type/Freeze", "Заморозка" },
      { "AdditionalEffect/Type/Stun", "Оглушение" },
      { "AdditionalEffect/Type/Burn", "Ожог" },
      { "AdditionalEffect/Type/Poison", "Отравление" },
      { "Gem/GemSynthesis", "Синтез камней" },
      { "Gem/Rarity/R", "Редкий" },
      { "Gem/Rarity/SR", "Супер редкий" },
      { "Gem/Rarity/SSR", "Супер-супер редкий" },
      { "Gem/Rarity/XR", "Крайне редкий" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByPercentage", "Скорость передвижения при атаке {0}{1}%" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByValue", "Скорость передвижения при атаке {0}{1}" },
      { "Gem/Upgrade/AugmentMovingSpeedByPercentage", "Скорость передвижения {0}{1}%" },
      { "Gem/Upgrade/AugmentMovingSpeedByValue", "Скорость передвижения {0}{1}" },
      { "Gem/Upgrade/CoolingCountdownDecrementByPercentage", "Время восстановления базового навыка {0}{1}%" },
      { "Gem/Upgrade/ReloadCountdownDecrementByPercentage", "Время перезарядки базового навыка {0}{1}%" },
      { "Gem/Upgrade/DispersionDecrementByPercentage", "Рассеивание базового навыка {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByPercentage", "Пробитие базового навыка {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByValue", "Пробитие базового навыка {0}{1}" },
      { "Gem/Upgrade/RangeIncrementByPercentage", "Дальность базового навыка {0}{1}%" },
      { "Gem/Upgrade/RangeIncrementByValue", "Дальность базового навыка {0}{1}" },
      { "Gem/Upgrade/RepelForceIncrementByPercentage", "Сила отталкивания базового навыка {0}{1}%" },
      { "Gem/Upgrade/RepelForceIncrementByValue", "Сила отталкивания базового навыка {0}{1}" },
      { "Gem/Upgrade/SkillCountIncrementByValue", "Снаряд базового навыка {0}{1}" },
      { "Gem/Upgrade/SpeedIncrementByPercentage", "Скорость базового навыка {0}{1}%" },
      { "Gem/Upgrade/SpeedIncrementByValue", "Скорость базового навыка {0}{1}" },
      { "Gem/Upgrade/EnableRangeAttack", "Включить атаку по дальности для базового навыка" },
      { "Gem/Upgrade/AugmentDamageRangeByValue", "Диапазон урона базового навыка {0}{1}" },
      { "Gem/Upgrade/AugmentDamageRangeByPercentage", "Диапазон урона базового навыка {0}{1}%" },
      { "Gem/Upgrade/AddPoisoningAdditionalEffect", "Добавить дополнительный эффект отравления" },
      { "Gem/Upgrade/SetPoisoningPossibility", "Установить вероятность отравления на {0}%" },
      { "Gem/Upgrade/SetPoisoningHurtPercentage", "Установить процент урона от отравления на {0}%" },
      { "Gem/Upgrade/AugmentPoisoningPossibility", "Вероятность отравления навыка {0}{1}%" },
      { "Gem/Upgrade/AugmentPoisoningHurtPercentage", "Процент урона от отравления навыка {0}{1}%" },
      {
        "Gem/Upgrade/AddOrConvertBasicAdditionalEffect",
        "Добавить или преобразовать атрибут дополнительного эффекта в <b>{0}</b>"
      },
      {
        "Gem/Upgrade/SetBasicAdditionalEffectPossibility",
        "Установить вероятность дополнительного эффекта навыка на {0}%"
      },
      { "Gem/Upgrade/AugmentBasicAdditionalEffectPossibility", "Вероятность дополнительного эффекта навыка {0}{1}%" },
      { "Gem/Upgrade/AugmentPhysicalHurtByValue", "Физический урон {0}{1}" },
      { "Gem/Upgrade/AugmentPhysicalHurtByPercentage", "Физический урон {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicIceHurtByValue", "Урон магией льда {0}{1}" },
      { "Gem/Upgrade/AugmentMagicIceHurtByPercentage", "Урон магией льда {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicFireHurtByValue", "Урон магией огня {0}{1}" },
      { "Gem/Upgrade/AugmentMagicFireHurtByPercentage", "Урон магией огня {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicThunderByValue", "Урон магией молнии {0}{1}" },
      { "Gem/Upgrade/Augment/MagicThunderByPercentage", "Урон магией молнии {0}{1}%" },
      { "Gem/Upgrade/Augment/Resurrection", "Количество воскрешений {0}{1}" },
      { "Gem/Upgrade/Augment/IncreaseMaxHp", "Максимальное HP {0}{1}" },
      {
        "Rune/Description/ClonedProjectile",
        "Увеличивает количество базовых навыков. Каждое улучшение добавляет один снаряд."
      },
      {
        "Rune/Description/ExpBonus",
        "Увеличивает опыт, получаемый героями, на 10%. Опыт, получаемый при повышении уровня героя, увеличивается на 7% каждый раз."
      },
      {
        "Rune/Description/Fission",
        "После того, как базовые навыки героя попадают в врага, есть шанс разделения. Начальная вероятность разделения составляет 10% и увеличивается на 2% с каждым улучшением."
      },
      {
        "Rune/Description/HailStrike",
        "активные навыки. При активации призывает град для атаки врагов в пределах видимости. Навык длится 7 секунд и увеличивается на 2 секунды с каждым улучшением."
      },
      {
        "Rune/Description/HolyShield",
        "активные навыки. После активации герой получает 10 секунд неуязвимости, и каждое улучшение увеличивает это время на 2 секунды."
      },
      {
        "Rune/Description/IncreaseHonor",
        "В конце игры количество очков чести, полученных игроком, увеличивается. Начальное увеличение составляет 10%, и каждое улучшение увеличивает его на 10%."
      },
      {
        "Rune/Description/InstantKill",
        "Когда базовые навыки героя попадают во врага, есть определенная вероятность того, что враг мгновенно умрет. Начальная вероятность составляет 1%, и каждое улучшение увеличивает ее на 0,5%."
      },
      {
        "Rune/Description/InstantReload",
        "Есть 10% шанс мгновенно перезарядить оружие, когда заканчиваются патроны. Каждое улучшение увеличивает этот шанс на 5%."
      },
      {
        "Rune/Description/KillAndRecover",
        "Активные навыки. Каждый раз, когда герой убивает врага в течение 10 секунд, он может восстановить 1% своего здоровья. Продолжительность каждого улучшения увеличивается на 2 секунды."
      },
      {
        "Rune/Description/IncreaseImmortalTime",
        "Увеличивает время неуязвимости после получения урона. Начальное состояние увеличивается на 0,25 секунды, и каждое улучшение увеличивает его на 0,15 секунды."
      },
      {
        "Rune/Description/IncreaseMaxHp",
        "Увеличивает максимальное здоровье героя на 10%, и каждое улучшение увеличивает его на 5%."
      },
      {
        "Rune/Description/MeteoriteStrike",
        "Активные навыки. Призывает метеориты для атаки врагов в пределах видимости в течение 10 секунд. Изначально падает 10 метеоритов в секунду, и каждое улучшение увеличивает это количество на 5."
      },
      {
        "Rune/Description/PickUpDistance",
        "Дальность подбора предметов героем увеличивается на 10%, и каждое улучшение увеличивает ее на 10%."
      },
      {
        "Rune/Description/Poisonous",
        "Активные навыки. Наносит урон ядом врагам в пределах видимости каждую секунду в течение 10 секунд. Каждый уровень увеличивает продолжительность на 2 секунды."
      },
      {
        "Rune/Description/PushAway",
        "Отталкивает ближайших врагов всякий раз, когда у героя заканчиваются патроны (10 секунд перезарядки). Каждое улучшение увеличивает силу толчка на 20%."
      },
      {
        "Rune/Description/HpRecovery",
        "Восстанавливает 0,2% HP героя в секунду, и каждое улучшение увеличивает количество восстановления на 0,2%."
      },
      {
        "Rune/Description/ReducedInjuery", "Урон, получаемый героем, уменьшается на 5%, и дополнительно на 5% за каждый уровень. Кроме того, мины вызывают взрыв близлежащих мин."
      },
      {
        "Rune/Description/Resurrection",
        "Когда герой умирает, он мгновенно воскрешается с восстановленными 25% здоровья. После воскрешения HP увеличивается на 15% с каждым улучшением."
      },
      {
        "Rune/Description/ThunderStrike",
        "Активные навыки. Призывает молнии для точной атаки врагов в пределах видимости. Навык длится 10 секунд. Изначально атакует 10% врагов в секунду, каждое улучшение будет атаковать дополнительных 3% врагов."
      },
      {
        "Rune/Description/TimeStop",
        "Приостанавливает все действия врагов на 5 секунд. Каждое улучшение увеличивает продолжительность паузы на 2 секунды."
      },
      { "Rune/Title/ClonedProjectile", "Клонированный снаряд" },
      { "Rune/Title/ExpBonus", "Книга опыта" },
      { "Rune/Title/Fission", "Деление" },
      { "Rune/Title/HailStrike", "Град" },
      { "Rune/Title/HolyShield", "Святой щит" },
      { "Rune/Title/IncreaseHonor", "Чемпион" },
      { "Rune/Title/InstantKill", "Один выстрел - одно убийство" },
      { "Rune/Title/InstantReload", "Мульти-магазин" },
      { "Rune/Title/KillAndRecover", "Кровожадный" },
      { "Rune/Title/IncreaseImmortalTime", "Рыцарский шлем" },
      { "Rune/Title/IncreaseMaxHp", "Сильная рука" },
      { "Rune/Title/MeteoriteStrike", "Метеорит" },
      { "Rune/Title/PickUpDistance", "Охотник за головами" },
      { "Rune/Title/Poisonous", "Газовая зона" },
      { "Rune/Title/PushAway", "Оставить в покое" },
      { "Rune/Title/HpRecovery", "Красный крест" },
      { "Rune/Title/ReducedInjuery", "Бронированный рыцарь" },
      { "Rune/Title/Resurrection", "Чудо" },
      { "Rune/Title/ThunderStrike", "Удар молнии" },
      { "Rune/Title/TimeStop", "Машина времени" },
      { "Exception/CorruptedSaveFile", "Файл сохранения поврежден и был создан резерв в {0}." },
    };
  }
}