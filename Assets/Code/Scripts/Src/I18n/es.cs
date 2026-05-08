using System.Collections.Generic;

namespace Code.Scripts.Src.I18n
{
  public struct es : ILanguage
  {
    public static readonly Dictionary<string, string> locales = new()
    {
      { "HelloWorld", "¡Hola, Mundo!" },
      { "Language", "Español" },
      { "NotAvailableInDemo", "En desarrollo" },
      { "Player1", "Jugador 1" },
      { "Player2", "Jugador 2" },
      {
        "Notification/WelcomeNotification",
        "¡Gracias por comprar nuestro juego!Sabemos que nuestro juego no es perfecto, ¡por eso necesitamos tus comentarios!Mantente atento a las actualizaciones continuas."
      },
      { "Measure/PerSecond", "/seg" },
      {
        "UI/FightPreparationGuide/Page0/0",
        "Hay 5 atributos para los ataques de héroes y enemigos, a saber, físico, hielo, fuego, trueno y veneno."
      },
      {
        "UI/FightPreparationGuide/Page0/1",
        "Entre ellos, el atributo de hielo restringe el atributo de fuego, el atributo de fuego restringe el atributo de trueno y el atributo de trueno restringe el atributo de hielo."
      },
      {
        "UI/FightPreparationGuide/Page1/0",
        "El atributo de hielo puede desencadenar el efecto de congelación y causar daño numérico;\nEl atributo de fuego puede desencadenar el efecto de ignición y causar daño proporcional;\nEl atributo de trueno puede desencadenar el efecto de aturdimiento, pero no causará daño adicional.\nTodos los atributos anteriores se eliminarán después de un período de tiempo.\nEl atributo de veneno puede desencadenar envenenamiento y continuar causando daño proporcional. Pero los jefes a menudo pueden aliviar o incluso eliminar el envenenamiento."
      },
      {
        "UI/FightPreparationGuide/Page1/1",
        "Algunos enemigos tienen defensa de atributo, inmunidad al efecto de atributo y ruptura de la defensa de atributo. ¡Hacer un buen uso de los atributos que restringen al enemigo puede facilitar la derrota del oponente!"
      },
      {
        "UI/FightReadyGuide/Page0/0",
        "Aquí se mostrarán los atributos de los monstruos en el mapa actual. ¡Recuerda estos atributos y combina tu propia estrategia para derrotar a los monstruos!"
      },
      {
        "UI/FightReadyGuide/Page1/0",
        "Estas dos posiciones mostrarán a los jefes sentados en el mapa. Cada jefe tiene diferentes habilidades y estilos de combate. ¡Intenta usar diferentes técnicas para derrotar a diferentes jefes!"
      },
      {
        "UI/FightReadyGuide/Page1/1",
        "No olvides explorar el mapa más a menudo, donde puedes recoger diferentes gemas para fortalecer a tus héroes.¡También puedes usar los honores que obtienes en las batallas para desbloquear nuevos héroes y runas para una mejor exploración en diferentes mapas!"
      },
      { "UI/ControlGuide/ActiveSkill", "Habilidad activa" },
      { "UI/ControlGuide/AutoFiring", "Disparo automático" },
      { "UI/ControlGuide/Movement", "Movimiento" },
      { "UI/ControlGuide/Fire!", "¡Fuego!" },
      { "UI/ControlGuide/AutoAiming", "Apuntado automático" },
      { "UI/ControlGuide/Aim", "Apuntando" },
      {
        "UI/ControlGuide/MoveToUseController",
        "Mueve el joystick para usar la puntería con el mando (fuera del modo de puntería automática)."
      },
      {
        "UI/ControlGuide/MoveToUseMouse",
        "Mueve el ratón para usar la puntería con el ratón (fuera del modo de puntería automática)."
      },
      { "UI/Control/AutoAiming", "Puntería automática" },
      { "UI/Control/MouseAiming", "Puntería con el ratón" },
      { "UI/Control/ControllerAiming", "Puntería con el mando" },
      { "UI/Control/AutoFiring", "Disparo automático" },
      { "UI/Control/ManualFiring", "Disparo manual" },
      { "UI/Control/UseMouseSelectHero", "Usa el ratón para seleccionar un héroe" },
      { "UI/Text/LevelUp!", "¡Subir de nivel!" },
      { "UI/Button/Choose", "Elegir" },
      { "UI/Start", "Comenzar" },
      { "UI/Languages", "Idiomas" },
      { "UI/Options", "Opciones" },
      { "UI/Exit", "Salir" },
      { "UI/Name", "Nombre" },
      { "UI/Description", "Descripción" },
      { "UI/Properties", "Propiedades" },
      { "UI/Properties/AttacksPerRound", "Ataques por ronda" },
      { "UI/Properties/RateOfFire", "Tasa de fuego" },
      { "UI/Properties/ReloadTime", "Tiempo de recarga" },
      { "UI/Properties/Projectiles", "Proyectiles" },
      { "UI/Properties/FreezeDamagePerSecond", "Daño por segundo" },
      { "UI/Properties/MovingSpeed", "Velocidad de movimiento" },
      { "UI/Properties/Physical", "Físico" },
      { "UI/Properties/Ice", "Hielo" },
      { "UI/Properties/Fire", "Fuego" },
      { "UI/Properties/Thunder", "Trueno" },
      { "UI/Properties/Poisoning", "Envenenamiento" },
      { "UI/GameOver", "Fin del juego" },
      { "UI/FinalScore", "Puntuación final" },
      { "UI/HonorGained", "Honor ganado" },
      { "UI/PlayerHonor", "Honor del jugador" },
      { "UI/PlayerScore", "Puntuación del jugador" },
      { "UI/EnemyKilled", "Enemigo asesinado" },
      { "UI/ExperienceGained", "Experiencia ganada" },
      { "UI/FinalLevel", "Nivel final" },
      { "UI/SurvivalTime", "Tiempo de supervivencia" },
      { "UI/Upgrades", "Mejoras" },
      { "UI/GameOver/Failed", "¡DERROTA!" },
      { "UI/GameOver/Success", "¡NIVEL SUPERADO!" },
      { "UI/GameOver/Aborted", "MISIÓN ABORTADA" },
      { "UI/Achievements", "Logros" },
      { "UI/Furnace", "Horno" },
      { "UI/Gems", "Gemas" },
      { "UI/Runes", "Runas" },
      { "UI/Difficulty", "Dificultad" },
      { "UI/Mode", "Modo de juego" },
      { "UI/CasualMode", "Modo casual" },
      { "UI/ChallengeMode", "Modo desafío" },
      { "UI/InfiniteMode", "Modo infinito" },
      { "UI/Play", "Jugar" },
      { "UI/Confirm", "Confirmar" },
      { "UI/Cancel", "Cancelar" },
      { "UI/GemList", "Lista de gemas" },
      { "UI/HeroDescription/MaxHp", "HP máxima" },
      { "UI/HeroDescription/HpRecover", "Recuperación de HP" },
      { "UI/HeroDescription/Resurrection", "Resurrección" },
      { "UI/HeroDescription/BasicSkillHurt", "Daño de habilidad básica" },
      { "UI/HeroDescription/BasicSkillProperty", "Atributo de habilidad básica" },
      { "UI/HeroDescription/Possibility", "Posibilidad de AE" },
      { "UI/HeroDescription/AttackRange", "Rango de ataque" },
      { "UI/HeroDescription/AttackPerRound", "Ataques por ronda" },
      { "UI/HeroDescription/AttackSpeed", "Velocidad de ataque" },
      { "UI/HeroDescription/PickUpRange", "Rango de recogida" },
      { "UI/HeroDescription/MovingSpeed", "Velocidad de movimiento" },
      { "UI/HeroDescription/AttackMovingSpeed", "Movimiento de ataque S." },
      { "UI/HeroDescription/BaseSkill", "Habilidad base" },
      { "UI/HeroDescription/AdditionalEffect", "Efecto adicional" },
      { "UI/HeroDescription/ProjectileCount", "Recuento de proyectiles" },
      { "UI/HeroDescription/AEHurt", "Daño de AE" },
      { "UI/FightPreparation/TwoPlayersUniqueHero", "Dos jugadores no pueden elegir el mismo héroe" },
      { "UI/FightPreparation/BrowserHeroes", "Héroes del navegador" },
      { "UI/FightPreparation/Back", "Atrás" },
      { "UI/FightPreparation/ApplyGems", "Usar gemas" },
      { "UI/FightPreparation/ApplyRunes", "Activar runas" },
      { "UI/FightPreparation/SelectHero", "Seleccionar héroe" },
      { "UI/FightPreparation/Unlock", "Desbloquear" },
      {
        "UI/FightPreparation/AddPlayer",
        "Presiona <color=red>A</color> en el gamepad o Ctrl derecho en el teclado para agregar al segundo jugador"
      },
      { "UI/FightPreparation/RemovePlayer", "Eliminar" },
      { "UI/PickGemUI/PickGemTitle", "Usar gemas" },
      { "UI/PickGemUI/OneGemOnePlayer", "Una gema solo puede ser utilizada por un solo jugador" },
      { "UI/PickRuneUI/PickRuneTitle", "Activar runas" },
      { "UI/PickRuneUI/Class", "Nivel" },
      { "UI/PickRuneUI/InspirationRune", "Runa de inspiración" },
      { "UI/PickRuneUI/DominationRune", "Runa de dominación" },
      { "UI/PickRuneUI/ImmortalRune", "Runa inmortal" },
      { "UI/PickRuneUI/Upgrade", "Mejorar" },
      { "UI/PickRuneUI/Price", "Precio" },
      { "UI/PickRuneUI/OneRunePerClass", "Solo se puede elegir una runa por nivel" },
      { "UI/Pause/PhysicalAttack", "Ataque físico" },
      { "UI/Pause/IceAttack", "Ataque de hielo" },
      { "UI/Pause/FireAttack", "Ataque de fuego" },
      { "UI/Pause/ThunderAttack", "Ataque de trueno" },
      { "UI/Pause/Poisoning", "Envenenamiento" },
      { "UI/Pause/MovingSpeed", "Velocidad de movimiento" },
      { "UI/Pause/AttackingSpeed", "Velocidad al atacar" },
      { "UI/Pause/AttackSpeed", "Velocidad de ataque" },
      { "UI/Pause/RecoverSpeed", "Velocidad de recuperación" },
      { "UI/Pause/ExpBonus", "Bonificación de experiencia" },
      { "UI/Pause/RoundsPerSecond", " rondas/s" },
      { "UI/Pause/Resume", "Reanudar" },
      { "UI/Pause/ControlGuide", "Guía de control" },
      { "UI/Pause/GiveUp", "Rendirse" },
      {
        "UI/Upgrade/DemoUpgradeUnlocked",
        "Mejoras ya desbloqueadas en la versión demo: 96/188Puede haber ajustes en la mejora en la versión oficial"
      },
      { "UI/GameOver/Quit", "Salir" },
      { "UI/FightReady/Difficulty", "Dificultad" },
      { "UI/FightReady/DifficultyNumber", "Nivel.{0}" },
      { "UI/FightReady/UnlockedDifficulty", "Desbloqueado {0}/{1}" },
      { "UI/FightReady/Mode", "Modo" },
      { "UI/FightReady/Map", "Mapa" },
      { "UI/FightReady/Description", "Descripción" },
      { "UI/FightReady/Casual", "Casual" },
      { "UI/FightReady/Standard", "Estándar" },
      { "UI/FightReady/Infinite", "Infinito" },
      { "UI/FightReady/Forest", "Bosque Misty" },
      { "UI/FightReady/Desert", "Desierto abrasador" },
      { "UI/FightReady/Dungeon", "Mazmorra oscura" },
      { "UI/FightReady/Graveyard", "Cementerio de la muerte" },
      { "UI/FightReady/Hell", "Infierno supremo" },
      { "UI/FightReady/ModeDescription/CasualMode", "La dificultad de los monstruos se reducirá en el Modo Casual." },
      { "UI/FightReady/ModeDescription/BossAppearTime", "El jefe aparecerá después de {0} minutos" },
      {
        "UI/FightReady/AttributeDescription",
        "Relación de atributos del enemigo: Físico - {0}%, Hielo - {1}%, Fuego - {2}%, Trueno - {3}%, Veneno - {4}%"
      },
      { "UI/Prompt/UnlockRune", "¿Desbloquear la runa <b>{0}</b> por {1}, continuar?" },
      { "UI/Prompt/UnlockHero", "¿Desbloquear al héroe <b>{0}</b> por {1}, continuar?" },
      { "UI/Info/Unlock/InsufficientBalance", "Lo sentimos, saldo insuficiente para desbloquear. (Requisito: {0})" },
      { "UI/Info/Upgrade/InsufficientBalance", "Lo siento, saldo insuficiente para mejorar." },
      { "UI/Info/RunePrerequisitesNotSatisfied", "Por favor, desbloquea primero la runa del nivel anterior." },
      { "UI/Audio/Overall", "General" },
      { "UI/Audio/Bgm", "Música de fondo" },
      { "UI/Audio/Sfx", "SFX" },
      { "Physical", "Físico" },
      { "Ice", "Hielo" },
      { "Thunder", "Trueno" },
      { "Fire", "Fuego" },
      { "Poison", "Veneno" },
      { "Hero/Archangel", "Seraphiel" },
      { "Hero/CaptainG", "Tormenta de balas" },
      { "Hero/Cutie", "Faye Spark" },
      { "Hero/Gumdam", "Atacante de cometas" },
      { "Hero/JeanneDArc", "Santa Juana" },
      { "Hero/MountainKing", "Señor del trueno" },
      { "Hero/Paladin", "Templario de hierro" },
      { "Hero/Ranger", "Atacante venenoso" },
      { "Hero/Witch", "Arcanista" },
      { "Hero/WuKong", "Sun Wukong" },
      { "Hero/ZhaoYun", "Zhao Yun" },
      {
        "UI/Hero/Description/Archangel",
        "Seraphiel dispara carámbanos con la posibilidad de causar congelación. Puede recuperar una cierta cantidad de salud por segundo y es inmune al daño del Infierno."
      },
      {
        "UI/Hero/Description/CaptainG",
        "Tormenta de balas dispara 3 balas de fuego con la posibilidad de causar quemaduras. Además, es inmune a las quemaduras."
      },
      { "UI/Hero/Description/Cutie", "Faye Spark dispara balas con penetración infinita." },
      {
        "UI/Hero/Description/Gumdam",
        "Atacante de cometas dispara 2 misiles desde una bazuca que explota, dañando a todos los enemigos cercanos. Pero los misiles pueden tener una zona muerta."
      },
      {
        "UI/Hero/Description/JeanneDArc",
        "Santa Juana ataca con su espada (Ataque cuerpo a cuerpo), que puede atravesar a los enemigos. Además, no necesita tiempo de recarga."
      },
      {
        "UI/Hero/Description/MountainKing",
        "Señor del trueno libera rayos que pueden aturdir al enemigo, y también es inmune al aturdimiento."
      },
      {
        "UI/Hero/Description/Paladin",
        "El Templario de hierro puede restaurar una pequeña cantidad de salud por segundo y es inmune al daño del Infierno."
      },
      {
        "UI/Hero/Description/Ranger",
        "Atacante venenoso dispara 2 flechas venenosas que circulan automáticamente alrededor del objetivo. Pero las flechas pueden tener una zona muerta."
      },
      {
        "UI/Hero/Description/Witch",
        "La Arcanista desata 2 bolas de relámpago para atacar al enemigo. Ella tiene una opción de mejora adicional al subir de nivel."
      },
      {
        "UI/Hero/Description/WuKong",
        "Sun Wukong puede teletransportarse a otros lugares (presiona el botón tres veces rápidamente para activar)."
      },
      {
        "UI/Hero/Description/ZhaoYun",
        "Zhao Yun dispara 3 energías de espada heladas en todas direcciones para atacar al enemigo. Además, es inmune a la congelación."
      },
      { "Level/Pause", "Pausado" },
      { "Level/PressEscToResume", "Presiona Esc para reanudar" },
      { "Upgrade/RecoverHp/Name", "Recuperar Lv{0}" },
      { "Upgrade/RecoverHp/Description", "Recuperar {0:F2}% de HP" },
      { "Upgrade/IncreaseMaxHp/Name", "MaxHP Lv{0}" },
      { "Upgrade/IncreaseMaxHp/Description", "Aumentar HP máxima en {0:F2}%" },
      { "Upgrade/IncreaseMaxHpOnHurt/Name", "Pavor Lv{0}" },
      {
        "Upgrade/IncreaseMaxHpOnHurt/Description",
        "Aumentar HP máxima en {0:F2}% cuando el jugador es herido. El incremento máximo será de {1:F2}%."
      },
      { "Upgrade/IncreaseDefense/Name", "Escudo Lv{0}" },
      { "Upgrade/IncreaseDefense/Description", "Aumentar la defensa en {0:F2}%" },
      { "Upgrade/IncreaseAllSpeed/Name", "¡Ve rápido! Lv{0}" },
      {
        "Upgrade/IncreaseAllSpeed/Description",
        "Aumentar la velocidad en {0:F2}% incluyendo la velocidad normal y la velocidad de ataque."
      },
      { "Upgrade/IncreaseMovingSpeed/Name", "Zapato para correr Lv{0}" },
      { "Upgrade/IncreaseMovingSpeed/Description", "Aumentar la velocidad de movimiento normal en {0:F2}%." },
      { "Upgrade/IncreaseAttackingMovingSpeed/Name", "Golpear y correr Lv{0}" },
      {
        "Upgrade/IncreaseAttackingMovingSpeed/Description",
        "Aumentar la velocidad de movimiento mientras se ataca en {0:F2}%."
      },
      { "Upgrade/KillToIncreaseSpeed/Name", "Mercenario Lv{0}" },
      {
        "Upgrade/KillToIncreaseSpeed/Description",
        "Mata {0} enemigos para aumentar {1:F2}% de velocidad y el incremento máximo será de {2:F2}%. El incremento de velocidad se restablecerá después de ser herido."
      },
      { "Upgrade/ExpBonus/Name", "Nerd Lv{0}" },
      { "Upgrade/ExpBonus/Description", "Aumenta la experiencia ganada cada vez en {0:F2}%." },
      { "Upgrade/IncreasePickUp/Name", "Indiana Jones Lv{0}" },
      { "Upgrade/IncreasePickUp/Description", "Aumentar el radio de recogida en {0:F2}%." },
      { "Upgrade/IncreaseShootingRange/Name", "Barrett Lv{0}" },
      { "Upgrade/IncreaseShootingRange/Description", "Aumentar el alcance de disparo en {0:F2}%." },
      { "Upgrade/AddProjectile/Name", "Escopeta Lv{0}" },
      {
        "Upgrade/AddProjectile/Description",
        "Añadir {0} proyectiles adicionales. Sin embargo, la dispersión aumentará ligeramente."
      },
      { "Upgrade/IncreaseDispersion/Name", "Dispersión Lv{0}" },
      { "Upgrade/IncreaseDispersion/Description", "Aumentar la dispersión en {0:F2}%." },
      { "Upgrade/ReduceDispersion/Name", "Rifle de francotirador Lv{0}" },
      { "Upgrade/ReduceDispersion/Description", "Reducir la dispersión en {0:F2}%." },
      { "Upgrade/IncreaseFiringRate/Name", "Fuego rápido Lv{0}" },
      { "Upgrade/IncreaseFiringRate/Description", "Aumentar la tasa de disparo en {0:F2}%." },
      { "Upgrade/BurstFire/Name", "Fuego Ráfaga Lv{0}" },
      {
        "Upgrade/BurstFire/Description",
        "Aumenta la tasa de disparo en {0:F2}% durante {1:F2} segundos después de recibir daño."
      },
      { "Upgrade/IncreaseMagazineSize/Name", "Cargador de Tambor Lv{0}" },
      { "Upgrade/IncreaseMagazineSize/Description", "Aumentar el tamaño del cargador en {0}." },
      { "Upgrade/ReduceReloadingTime/Name", "Cargador Rápido Lv{0}" },
      { "Upgrade/ReduceReloadingTime/Description", "Reducir el tiempo de recarga en {0:F2}%." },
      { "Upgrade/BurstReload/Name", "Cargador del Miedo Lv{0}" },
      {
        "Upgrade/BurstReload/Description",
        "Reducir el tiempo de recarga en {0:F2}% durante {1:F2} segundos después de recibir daño."
      },
      { "Upgrade/IncreaseHurtPercentage/Name", "Afilado Lv{0}" },
      { "Upgrade/IncreaseHurtPercentage/Description", "Aumentar el daño de la habilidad base en {0:F2}%." },
      { "Upgrade/IncreaseIceAeDuration/Name", "Helado Lv{0}" },
      { "Upgrade/IncreaseIceAeDuration/Description", "Aumentar la duración del efecto Congelado en {0:F2}%." },
      { "Upgrade/IncreaseFireAeDuration/Name", "Quemar Lv{0}" },
      { "Upgrade/IncreaseFireAeDuration/Description", "Aumentar la duración del efecto Quemadura en {0:F2}%." },
      { "Upgrade/IncreaseThunderAeDuration/Name", "Tormenta Eléctrica Lv{0}" },
      { "Upgrade/IncreaseThunderAeDuration/Description", "Aumentar la duración del efecto Aturdimiento en {0:F2}%." },
      { "Upgrade/ActivateDart/Name", "Ninja" },
      { "Upgrade/ActivateDart/Description", "Lanza un dardo que gira a tu alrededor." },
      { "Upgrade/AddDart/Name", "Sensei Lv{0}" },
      { "Upgrade/AddDart/Description", "Lanza {0} dardos más a tu alrededor." },
      { "Upgrade/PoisonDart/Name", "Dardo Venenoso Lv{0}" },
      {
        "Upgrade/PoisonDart/Description",
        "Empapa todos los dardos en veneno, dándoles una probabilidad de {0:F2}% de envenenar al enemigo y causar {1:F2}% de daño por segundo (ineficaz contra jefes)."
      },
      { "Upgrade/ImproveDartHurt/Name", "Afilado Lv{0}" },
      {
        "Upgrade/ImproveDartHurt/Description", "Afila todos los dardos a tu alrededor, mejorando {0:F2}% de los daños."
      },
      { "Upgrade/ActivateBoomerang/Name", "Aborígenes" },
      { "Upgrade/ActivateBoomerang/Description", "Lanza un bumerang que gira a tu alrededor." },
      { "Upgrade/AddBoomerang/Name", "Manuel Schütz Lv{0}" },
      { "Upgrade/AddBoomerang/Description", "Lanza {0} bumerangs más a tu alrededor." },
      { "Upgrade/BurnBoomerang/Name", "Bumerang de Fuego Lv{0}" },
      {
        "Upgrade/BurnBoomerang/Description",
        "Da al bumerang una probabilidad de {0:F2}% de prender fuego al enemigo durante {1:F2} segundos, infligiendo {2:F2}% de daño por segundo."
      },
      { "Upgrade/IncreaseBoomerangSpeed/Name", "Aerodinámica Lv{0}" },
      {
        "Upgrade/IncreaseBoomerangSpeed/Description", "Optimiza la aerodinámica para acelerar el bumerang en {0:F2}%."
      },
      { "Upgrade/ImproveBoomerangHurt/Name", "Relleno de Plomo Lv{0}" },
      {
        "Upgrade/ImproveBoomerangHurt/Description",
        "Carga el bumerang con plomo, aumentando su poder de ataque en {0:F2}%"
      },
      { "Upgrade/ActivateIceTower/Name", "Torre Congelada" },
      {
        "Upgrade/ActivateIceTower/Description",
        "Vuela una Torre de Hielo automática a tu alrededor que dispara granizo automáticamente."
      },
      { "Upgrade/AddIceTower/Name", "Invierno Lv{0}" },
      { "Upgrade/AddIceTower/Description", "Añade {0} Torres de Hielo más." },
      { "Upgrade/IncreaseIceTowerFiringRate/Name", "Skadi Lv{0}" },
      {
        "Upgrade/IncreaseIceTowerFiringRate/Description",
        "Aumenta la velocidad de ataque de la Torre de Hielo en {0:F2}%."
      },
      { "Upgrade/ImproveIceTowerHurt/Name", "Cubo de Hielo Lv{0}" },
      { "Upgrade/ImproveIceTowerHurt/Description", "Mejora el daño de la Torre de Hielo en {0:F2}%." },
      { "Upgrade/ActivateFireTower/Name", "Torre de Fuego" },
      {
        "Upgrade/ActivateFireTower/Description",
        "Vuela una Torre de Fuego automática a tu alrededor que dispara bolas de fuego automáticamente."
      },
      { "Upgrade/AddFireTower/Name", "Acalorado Lv{0}" },
      { "Upgrade/AddFireTower/Description", "Añade {0} Torres de Fuego más." },
      { "Upgrade/IncreaseFireTowerFiringRate/Name", "Apolo Lv{0}" },
      {
        "Upgrade/IncreaseFireTowerFiringRate/Description",
        "Aumenta la velocidad de ataque de la Torre de Fuego en {0:F2}%."
      },
      { "Upgrade/ImproveFireTowerHurt/Name", "Lanzallamas Lv{0}" },
      { "Upgrade/ImproveFireTowerHurt/Description", "Mejora el daño de la Torre de Fuego en {0:F2}%." },
      { "Upgrade/ActivateThunderTower/Name", "Torre de Trueno" },
      {
        "Upgrade/ActivateThunderTower/Description",
        "Vuela una Torre de Trueno automática a tu alrededor que dispara truenos automáticamente."
      },
      { "Upgrade/AddThunderTower/Name", "Rayo Lv{0}" },
      { "Upgrade/AddThunderTower/Description", "Añade {0} Torres de Trueno más." },
      { "Upgrade/IncreaseThunderTowerFiringRate/Name", "Thor Lv{0}" },
      {
        "Upgrade/IncreaseThunderTowerFiringRate/Description",
        "Aumenta la velocidad de ataque de la Torre de Trueno en {0:F2}%."
      },
      { "Upgrade/ImproveThunderTowerHurt/Name", "Bobina Tesla" },
      { "Upgrade/ImproveThunderTowerHurt/Description", "Mejora el daño de la Torre de Trueno en {0:F2}%." },
      { "Upgrade/ActivateSpiral/Name", "Espiral" },
      {
        "Upgrade/ActivateSpiral/Description",
        "De vez en cuando, el héroe libera una espiral con atributos aleatorios para atacar al enemigo."
      },
      { "Upgrade/AddSpiral/Name", "Nautilidae Lv{0}" },
      { "Upgrade/AddSpiral/Description", "Añade {0} espirales más a tu alrededor." },
      { "Upgrade/ReduceSpiralInterval/Name", "Lluvia Torrencial Lv{0}" },
      { "Upgrade/ReduceSpiralInterval/Description", "Reduce el intervalo de la Espiral en {0:F2}%." },
      { "Upgrade/IncreaseSpiralHurt/Name", "Tormenta Eléctrica Lv{0}" },
      { "Upgrade/IncreaseSpiralHurt/Description", "Aumenta el daño de la Espiral en {0:F2}%." },
      { "Upgrade/ActivatePuppet/Name", "Luchador de Marionetas" },
      { "Upgrade/ActivatePuppet/Description", "Invoca {0} marionetas cada {1:F2} segundos." },
      { "Upgrade/LevelUpPuppetToLv1/Name", "Marioneta Armada" },
      {
        "Upgrade/LevelUpPuppetToLv1/Description",
        "Invoca {0} marionetas mejoradas cada {1:F2} segundos. Sin embargo, las mejoras de las marionetas actuales se restablecen."
      },
      { "Upgrade/LevelUpPuppetToLv2/Name", "Marioneta Saiya" },
      {
        "Upgrade/LevelUpPuppetToLv2/Description",
        "Invoca {0} marionetas definitivas cada {1:F2} segundos. Sin embargo, las mejoras de las marionetas actuales se restablecen."
      },
      { "Upgrade/ReducePuppetIntervalLv0/Name", "Equipo Lv{0}" },
      { "Upgrade/ReducePuppetIntervalLv0/Description", "Reduce el intervalo de invocación de marionetas en {0:F2}%." },
      { "Upgrade/AddPuppetLv0/Name", "Grito Lv{0}" },
      { "Upgrade/AddPuppetLv0/Description", "Invoca {0} marionetas más para peleas en grupo." },
      { "Upgrade/IncreasePuppetHurtLv0/Name", "Cuchillo Lv{0}" },
      { "Upgrade/IncreasePuppetHurtLv0/Description", "Aumenta el daño de todas las marionetas en {0:F2}%." },
      { "Upgrade/ReducePuppetIntervalLv1/Name", "Buscapersonas Lv{0}" },
      { "Upgrade/ReducePuppetIntervalLv1/Description", "Reduce el intervalo de invocación de marionetas en {0:F2}%." },
      { "Upgrade/AddPuppetLv1/Name", "Mafia Lv{0}" },
      {
        "Upgrade/AddPuppetLv1/Description",
        "Invocar {0} marionetas más para peleas en grupo."
      },
      { "Upgrade/IncreasePuppetHurtLv1/Name", "Pistola Lv{0}" },
      { "Upgrade/IncreasePuppetHurtLv1/Description", "Aumenta el daño de todas las marionetas en {0:F2}%." },
      { "Upgrade/ReducePuppetIntervalLv2/Name", "Smartphone Lv{0}" },
      { "Upgrade/ReducePuppetIntervalLv2/Description", "Reduce el intervalo de invocación de marionetas en {0:F2}%." },
      { "Upgrade/AddPuppetLv2/Name", "Ejército Lv{0}" },
      {
        "Upgrade/AddPuppetLv2/Description",
        "Invocar {0} marionetas más para peleas en grupo."
      },
      { "Upgrade/IncreasePuppetHurtLv2/Name", "Rifle Lv{0}" },
      { "Upgrade/IncreasePuppetHurtLv2/Description", "Aumenta el daño de todas las marionetas en {0:F2}%." },
      { "Upgrade/ActivateFlyingSword/Name", "Espada Voladora" },
      {
        "Upgrade/ActivateFlyingSword/Description",
        "Usa Qi para controlar espadas, liberando {0} espadas cada {1:F2} segundos."
      },
      { "Upgrade/LevelUpFlyingSwordToLv1/Name", "Espada Pesada" },
      {
        "Upgrade/LevelUpFlyingSwordToLv1/Description",
        "Usa espadas más pesadas, liberando {0} espadas cada {1:F2} segundos. Sin embargo, las mejoras de las espadas se restablecerán."
      },
      { "Upgrade/LevelUpFlyingSwordToLv2/Name", "Espada Afilada" },
      {
        "Upgrade/LevelUpFlyingSwordToLv2/Description",
        "Usa espadas afiladas, liberando {0} espadas cada {1:F2} segundos. Sin embargo, las mejoras de las espadas se restablecerán."
      },
      { "Upgrade/AddFlyingSwordLv0/Name", "Junior Lv{0}" },
      { "Upgrade/AddFlyingSwordLv0/Description", "Añade {0} espadas voladoras más." },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Name", "Aprendiz Lv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv0/Description", "Reducir el intervalo de liberación de espadas en {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Name", "Entrada del espadachín Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Description", "Aumentar el daño de las espadas en {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv1/Name", "Senior Lv{0}" },
      { "Upgrade/AddFlyingSwordLv1/Description", "Añadir {0} espadas voladoras más." },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Name", "Artesano Lv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv1/Description", "Reducir el intervalo de liberación de espadas en {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Name", "Senior del espadachín Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Description", "Aumentar el daño de las espadas en {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv2/Name", "Shifu Lv{0}" },
      { "Upgrade/AddFlyingSwordLv2/Description", "Añadir {0} espadas voladoras más." },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Name", "Cazador de Sol Lv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv2/Description", "Reducir el intervalo de liberación de espadas en {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Name", "Zongshi lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Description", "Aumentar el daño de las espadas en {0:F2}%." },
      { "Upgrade/ActivateMine/Name", "Mina Terrestre" },
      {
        "Upgrade/ActivateMine/Description",
        "Planta {0} minas que explotan y causan {1:F2} de daño dentro de un radio de {2:F2} metros cada {3:F2} segundos. Además, las minas activan la explosión de las minas cercanas."
      },
      { "Upgrade/LevelUpMineToLv1/Name", "Mina de Trueno" },
      {
        "Upgrade/LevelUpMineToLv1/Description",
        "Planta {0} minas mejoradas que explotan y causan {1:F2} de daño dentro de un radio de {2:F2} metros cada {3:F2} segundos."
      },
      { "Upgrade/LevelUpMineToLv2/Name", "Mina Claymore" },
      {
        "Upgrade/LevelUpMineToLv2/Description",
        "Planta {0} minas extremadamente letales que explotan y causan {1:F2} de daño dentro de un radio de {2:F2} metros cada {3:F2} segundos."
      },
      { "Upgrade/AddMineLv0/Name", "Boy Scout Lv{0}" },
      { "Upgrade/AddMineLv0/Description", "Planta {0} minas terrestres más cada vez." },
      { "Upgrade/ReduceMineIntervalLv0/Name", "Alicates Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv0/Description", "Reduce el intervalo entre minas en {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv0/Name", "Arañazo Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv0/Description", "Aumenta el daño de las minas terrestres en {0:F2}%." },
      { "Upgrade/AddMineLv1/Name", "Minutemen Lv{0}" },
      { "Upgrade/AddMineLv1/Description", "Planta {0} minas de trueno más cada vez." },
      { "Upgrade/ReduceMineIntervalLv1/Name", "Caja de herramientas Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv1/Description", "Reduce el intervalo entre minas en {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv1/Name", "Bola de acero Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv1/Description", "Aumenta el daño de las minas de trueno en {0:F2}%." },
      { "Upgrade/AddMineLv2/Name", "Fuerzas especiales Lv{0}" },
      { "Upgrade/AddMineLv2/Description", "Planta {0} minas de trueno más cada vez." },
      { "Upgrade/ReduceMineIntervalLv2/Name", "Sistema de minas Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv2/Description", "Reduce el intervalo entre minas en {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv2/Name", "Fragmentación Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv2/Description", "Aumenta el daño de las minas Claymore en {0:F2}%." },
      { "MapIndicator/Forest", "Bosque Misty" },
      { "MapIndicator/Desert", "Desierto abrasador" },
      { "MapIndicator/Dungeon", "Mazmorra oscura" },
      { "MapIndicator/Graveyard", "Cementerio de la muerte" },
      { "MapIndicator/Hell", "Infierno supremo" },
      {
        "MapDescription/Forest",
        "Un bosque brumoso, donde las balas a menudo salen de la nada en los árboles aparentemente pacíficos."
      },
      {
        "MapDescription/Desert",
        "Parece que muchos monstruos que pueden sobrevivir en el duro desierto son inmunes a los atributos."
      },
      {
        "MapDescription/Dungeon",
        "En la mazmorra, a menudo se escuchan los aullidos de los lobos y el sonido de los martillos balanceándose."
      },
      {
        "MapDescription/Graveyard",
        "¡Cuidado con los cráneos esquivos en el cementerio! ¡Puedes salir herido si te topas con ellos!"
      },
      {
        "MapDescription/Hell",
        "El infierno está encantado por el diablo, causando más o menos daño a los héroes cada 10 segundos. ¡Pero parece que a algunos héroes no les importa esto en absoluto."
      },
      { "AdditionalEffect/Type/Freeze", "Congelar" },
      { "AdditionalEffect/Type/Stun", "Aturdir" },
      { "AdditionalEffect/Type/Burn", "Quemar" },
      { "AdditionalEffect/Type/Poison", "Envenenar" },
      { "Gem/GemSynthesis", "Síntesis de gemas" },
      { "Gem/Rarity/R", "Raro" },
      { "Gem/Rarity/SR", "Súper raro" },
      { "Gem/Rarity/SSR", "Súper súper raro" },
      { "Gem/Rarity/XR", "Extremadamente raro" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByPercentage", "Velocidad de movimiento al atacar {0}{1}%" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByValue", "Velocidad de movimiento al atacar {0}{1}" },
      { "Gem/Upgrade/AugmentMovingSpeedByPercentage", "Velocidad de movimiento {0}{1}%" },
      { "Gem/Upgrade/AugmentMovingSpeedByValue", "Velocidad de movimiento {0}{1}" },
      { "Gem/Upgrade/CoolingCountdownDecrementByPercentage", "Tiempo de enfriamiento de habilidad básica {0}{1}%" },
      { "Gem/Upgrade/ReloadCountdownDecrementByPercentage", "Tiempo de recarga de habilidad básica {0}{1}%" },
      { "Gem/Upgrade/DispersionDecrementByPercentage", "Dispersión de habilidad básica {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByPercentage", "Penetración de habilidad básica {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByValue", "Penetración de habilidad básica {0}{1}" },
      { "Gem/Upgrade/RangeIncrementByPercentage", "Alcance de habilidad básica {0}{1}%" },
      { "Gem/Upgrade/RangeIncrementByValue", "Alcance de habilidad básica {0}{1}" },
      { "Gem/Upgrade/RepelForceIncrementByPercentage", "Fuerza de repulsión de habilidad básica {0}{1}%" },
      { "Gem/Upgrade/RepelForceIncrementByValue", "Fuerza de repulsión de habilidad básica {0}{1}" },
      { "Gem/Upgrade/SkillCountIncrementByValue", "Proyectil de habilidad básica {0}{1}" },
      { "Gem/Upgrade/SpeedIncrementByPercentage", "Velocidad de habilidad básica {0}{1}%" },
      { "Gem/Upgrade/SpeedIncrementByValue", "Velocidad de habilidad básica {0}{1}" },
      { "Gem/Upgrade/EnableRangeAttack", "Habilitar ataque de rango para la habilidad básica" },
      { "Gem/Upgrade/AugmentDamageRangeByValue", "Rango de daño de habilidad básica {0}{1}" },
      { "Gem/Upgrade/AugmentDamageRangeByPercentage", "Rango de daño de habilidad básica {0}{1}%" },
      { "Gem/Upgrade/AddPoisoningAdditionalEffect", "Añadir efecto adicional de envenenamiento" },
      { "Gem/Upgrade/SetPoisoningPossibility", "Establecer la posibilidad de envenenamiento en {0}%" },
      { "Gem/Upgrade/SetPoisoningHurtPercentage", "Establecer el porcentaje de daño por envenenamiento en {0}%" },
      { "Gem/Upgrade/AugmentPoisoningPossibility", "Posibilidad de envenenamiento de habilidad {0}{1}%" },
      { "Gem/Upgrade/AugmentPoisoningHurtPercentage", "Porcentaje de daño por envenenamiento de habilidad {0}{1}%" },
      {
        "Gem/Upgrade/AddOrConvertBasicAdditionalEffect",
        "Añadir o convertir el atributo de efecto adicional a <b>{0}</b>"
      },
      {
        "Gem/Upgrade/SetBasicAdditionalEffectPossibility",
        "Establecer la posibilidad de efecto adicional de habilidad en {0}%"
      },
      { "Gem/Upgrade/AugmentBasicAdditionalEffectPossibility", "Posibilidad de efecto adicional de habilidad {0}{1}%" },
      { "Gem/Upgrade/AugmentPhysicalHurtByValue", "Daño físico {0}{1}" },
      { "Gem/Upgrade/AugmentPhysicalHurtByPercentage", "Daño físico {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicIceHurtByValue", "Daño de magia de hielo {0}{1}" },
      { "Gem/Upgrade/AugmentMagicIceHurtByPercentage", "Daño de magia de hielo {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicFireHurtByValue", "Daño de magia de fuego {0}{1}" },
      { "Gem/Upgrade/AugmentMagicFireHurtByPercentage", "Daño de magia de fuego {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicThunderByValue", "Daño de magia de trueno {0}{1}" },
      { "Gem/Upgrade/Augment/MagicThunderByPercentage", "Daño de magia de trueno {0}{1}%" },
      { "Gem/Upgrade/Augment/Resurrection", "Recuento de resurrecciones {0}{1}" },
      { "Gem/Upgrade/Augment/IncreaseMaxHp", "HP máxima {0}{1}" },
      {
        "Rune/Description/ClonedProjectile", "Aumenta el número de habilidades básicas. Cada mejora añade un proyectil."
      },
      {
        "Rune/Description/ExpBonus",
        "Aumenta la experiencia ganada por los héroes en un 10%. La experiencia ganada aumenta en un 7% cada vez que el héroe sube de nivel."
      },
      {
        "Rune/Description/Fission",
        "Después de que las habilidades básicas del héroe golpean al enemigo, existe una cierta posibilidad de dividirse. La probabilidad de división inicial es del 10%, y aumenta en un 2% cada vez que se mejora."
      },
      {
        "Rune/Description/HailStrike",
        "habilidades activas. Cuando se activa, invoca granizo para atacar a los enemigos dentro del alcance visible. La habilidad dura 7 segundos y aumenta en 2 segundos con cada mejora."
      },
      {
        "Rune/Description/HolyShield",
        "habilidades activas. Después de activar, el héroe gana 10 segundos de invencibilidad, y cada mejora aumenta en 2 segundos."
      },
      {
        "Rune/Description/IncreaseHonor",
        "Al final del juego, el valor de honor ganado por el jugador aumenta. El aumento inicial es del 10%, y cada mejora lo aumenta en un 10%."
      },
      {
        "Rune/Description/InstantKill",
        "Cuando las habilidades básicas del héroe golpean al enemigo, existe una cierta probabilidad de que el enemigo muera instantáneamente. La probabilidad inicial es del 1%, y cada mejora la aumenta en un 0,5%."
      },
      {
        "Rune/Description/InstantReload",
        "Hay un 10% de probabilidad de recargar instantáneamente cuando se agota la munición. Cada mejora aumenta la probabilidad en un 5%."
      },
      {
        "Rune/Description/KillAndRecover",
        "habilidades activas. Cada vez que el héroe mata a un enemigo en 10 segundos, el héroe puede recuperar el 1% de su salud. La duración de cada mejora se incrementa en 2 segundos."
      },
      {
        "Rune/Description/IncreaseImmortalTime",
        "Aumenta el tiempo de invulnerabilidad después de recibir daño. El estado inicial aumenta en 0,25 segundos, y cada mejora lo aumenta en 0,15 segundos."
      },
      {
        "Rune/Description/IncreaseMaxHp",
        "Aumenta la salud máxima del héroe en un 10%, y aumenta en un 5% por cada mejora."
      },
      {
        "Rune/Description/MeteoriteStrike",
        "habilidades activas. Invoca meteoritos para atacar a los enemigos dentro del alcance visible durante 10 segundos. Inicialmente, caen 10 meteoritos por segundo, y cada mejora aumenta en 5."
      },
      {
        "Rune/Description/PickUpDistance",
        "La distancia de recogida del héroe aumenta en un 10%, y cada mejora lo aumenta en un 10%."
      },
      {
        "Rune/Description/Poisonous",
        "habilidades activas. Inflige daño de veneno a los enemigos dentro del alcance visible cada segundo durante 10 segundos. Cada nivel aumenta la duración en 2 segundos."
      },
      {
        "Rune/Description/PushAway",
        "Empuja a los enemigos cercanos cada vez que el héroe se queda sin munición (10 segundos de enfriamiento). Cada mejora aumenta el empuje en un 20%."
      },
      {
        "Rune/Description/HpRecovery",
        "Recupera el 0,2% de la HP del héroe por segundo, y aumenta la cantidad de recuperación en un 0,2% por nivel."
      },
      {
        "Rune/Description/ReducedInjuery",
        "El daño recibido por el héroe se reduce en un 5%, y se reduce un 5% adicional por cada nivel."
      },
      {
        "Rune/Description/Resurrection",
        "Cuando un héroe muere, revive instantáneamente con el 25% de su salud restaurada. Después de la resurrección, la HP aumenta en un 15% cada vez que se mejora."
      },
      {
        "Rune/Description/ThunderStrike",
        "habilidades activas. Invoca relámpagos para atacar a los enemigos dentro del alcance visible con precisión. La habilidad dura 10 segundos. Inicialmente, ataca al 10% de los enemigos por segundo, cada mejora atacará a un 3% adicional de enemigos."
      },
      {
        "Rune/Description/TimeStop",
        "Pausa todas las acciones enemigas durante 5 segundos. Cada mejora aumenta la duración de la pausa de tiempo en 2 segundos."
      },
      { "Rune/Title/ClonedProjectile", "Proyectil clonado" },
      { "Rune/Title/ExpBonus", "Libro de experiencia" },
      { "Rune/Title/Fission", "Fisión" },
      { "Rune/Title/HailStrike", "Granizo" },
      { "Rune/Title/HolyShield", "Escudo sagrado" },
      { "Rune/Title/IncreaseHonor", "Campeón" },
      { "Rune/Title/InstantKill", "Un disparo, una muerte" },
      { "Rune/Title/InstantReload", "Multicargador" },
      { "Rune/Title/KillAndRecover", "Sed de sangre" },
      { "Rune/Title/IncreaseImmortalTime", "Casco de caballero" },
      { "Rune/Title/IncreaseMaxHp", "Brazo fuerte" },
      { "Rune/Title/MeteoriteStrike", "Meteorito" },
      { "Rune/Title/PickUpDistance", "Cazador de recompensas" },
      { "Rune/Title/Poisonous", "Zona de gas" },
      { "Rune/Title/PushAway", "Dejar en paz" },
      { "Rune/Title/HpRecovery", "Cruz roja" },
      { "Rune/Title/ReducedInjuery", "Caballero blindado" },
      { "Rune/Title/Resurrection", "Milagro" },
      { "Rune/Title/ThunderStrike", "Golpe de trueno" },
      { "Rune/Title/TimeStop", "Máquina del tiempo" },
      { "Exception/CorruptedSaveFile", "El archivo guardado está dañado y se ha copiado a {0}." },
    };
  }
}