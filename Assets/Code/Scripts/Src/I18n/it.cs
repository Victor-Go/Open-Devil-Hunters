using System.Collections.Generic;

namespace Code.Scripts.Src.I18n
{
  public struct it : ILanguage
  {
    public static readonly Dictionary<string, string> locales = new()
    {
      { "HelloWorld", "Ciao, Mondo!" },
      { "Language", "Italiano" },
      { "NotAvailableInDemo", "In sviluppo" },
      { "Player1", "Giocatore 1" },
      { "Player2", "Giocatore 2" },
      {
        "Notification/WelcomeNotification",
        "Grazie per aver acquistato il nostro gioco!Sappiamo che il nostro gioco non è perfetto, ecco perché abbiamo bisogno del tuo feedback!Rimani sintonizzato per continui aggiornamenti."
      },
      { "Measure/PerSecond", "/sec" },
      {
        "UI/FightPreparationGuide/Page0/0",
        "Ci sono 5 attributi per gli attacchi di eroi e nemici, ovvero fisico, ghiaccio, fuoco, tuono e veleno."
      },
      {
        "UI/FightPreparationGuide/Page0/1",
        "Tra questi, l'attributo ghiaccio limita l'attributo fuoco, l'attributo fuoco limita l'attributo tuono e l'attributo tuono limita l'attributo ghiaccio."
      },
      {
        "UI/FightPreparationGuide/Page1/0",
        "L'attributo ghiaccio può innescare l'effetto congelamento e causare danni numerici;\nL'attributo fuoco può innescare l'effetto infiammabile e causare danni proporzionali;\nL'attributo tuono può innescare l'effetto stordimento, ma non causerà danni aggiuntivi.\nTutti gli attributi sopra indicati verranno eliminati dopo un certo periodo di tempo.\nL'attributo veleno può innescare l'avvelenamento e continuare a causare danni proporzionali. Ma i boss possono spesso alleviare o addirittura rimuovere l'avvelenamento."
      },
      {
        "UI/FightPreparationGuide/Page1/1",
        "Alcuni nemici hanno difesa dagli attributi, immunità agli effetti degli attributi e rottura della difesa dagli attributi. Fare buon uso degli attributi che limitano il nemico può rendere più facile sconfiggere l'avversario!"
      },
      {
        "UI/FightReadyGuide/Page0/0",
        "Qui verranno mostrati gli attributi dei mostri nella mappa corrente. Ricorda questi attributi e abbina la tua strategia per sconfiggere i mostri!"
      },
      {
        "UI/FightReadyGuide/Page1/0",
        "Queste due posizioni mostreranno i boss seduti sulla mappa. Ogni boss ha diverse abilità e stili di combattimento. Cerca di usare diverse tecniche per sconfiggere boss diversi!"
      },
      {
        "UI/FightReadyGuide/Page1/1",
        "Non dimenticare di esplorare più spesso la mappa, dove puoi raccogliere gemme diverse per rafforzare i tuoi eroi.Puoi anche usare gli onori che ottieni dalle battaglie per sbloccare nuovi eroi e rune per una migliore esplorazione in mappe diverse!"
      },
      { "UI/ControlGuide/ActiveSkill", "Abilità attiva" },
      { "UI/ControlGuide/AutoFiring", "Fuoco automatico" },
      { "UI/ControlGuide/Movement", "Movimento" },
      { "UI/ControlGuide/Fire!", "Fuoco!" },
      { "UI/ControlGuide/AutoAiming", "Mira automatica" },
      { "UI/ControlGuide/Aim", "Mirare" },
      {
        "UI/ControlGuide/MoveToUseController",
        "Muovi il joystick per utilizzare la mira del controller (non in modalità di mira automatica)."
      },
      {
        "UI/ControlGuide/MoveToUseMouse",
        "Muovi il mouse per utilizzare la mira del mouse (non in modalità di mira automatica)."
      },
      { "UI/Control/AutoAiming", "Mira automatica" },
      { "UI/Control/MouseAiming", "Mira con il mouse" },
      { "UI/Control/ControllerAiming", "Mira con il controller" },
      { "UI/Control/AutoFiring", "Fuoco automatico" },
      { "UI/Control/ManualFiring", "Fuoco manuale" },
      { "UI/Control/UseMouseSelectHero", "Usa il mouse per selezionare un eroe" },
      { "UI/Text/LevelUp!", "Salire di livello!" },
      { "UI/Button/Choose", "Scegliere" },
      { "UI/Start", "Inizia" },
      { "UI/Languages", "Lingue" },
      { "UI/Options", "Opzioni" },
      { "UI/Exit", "Esci" },
      { "UI/Name", "Nome" },
      { "UI/Description", "Descrizione" },
      { "UI/Properties", "Proprietà" },
      { "UI/Properties/AttacksPerRound", "Attacchi per round" },
      { "UI/Properties/RateOfFire", "Frequenza di fuoco" },
      { "UI/Properties/ReloadTime", "Tempo di ricarica" },
      { "UI/Properties/Projectiles", "Proiettili" },
      { "UI/Properties/FreezeDamagePerSecond", "Danni al secondo" },
      { "UI/Properties/MovingSpeed", "Velocità di movimento" },
      { "UI/Properties/Physical", "Fisico" },
      { "UI/Properties/Ice", "Ghiaccio" },
      { "UI/Properties/Fire", "Fuoco" },
      { "UI/Properties/Thunder", "Tuono" },
      { "UI/Properties/Poisoning", "Avvelenamento" },
      { "UI/GameOver", "Game Over" },
      { "UI/FinalScore", "Punteggio finale" },
      { "UI/HonorGained", "Onore guadagnato" },
      { "UI/PlayerHonor", "Onore giocatore" },
      { "UI/PlayerScore", "Punteggio giocatore" },
      { "UI/EnemyKilled", "Nemici uccisi" },
      { "UI/ExperienceGained", "Esperienza guadagnata" },
      { "UI/FinalLevel", "Livello finale" },
      { "UI/SurvivalTime", "Tempo di sopravvivenza" },
      { "UI/Upgrades", "Aggiornamenti" },
      { "UI/GameOver/Failed", "SCONFITTA!" },
      { "UI/GameOver/Success", "LIVELLO COMPLETATO!" },
      { "UI/GameOver/Aborted", "MISSIONE ANNULLATA" },
      { "UI/Achievements", "Obiettivi" },
      { "UI/Furnace", "Fucina" },
      { "UI/Gems", "Gemme" },
      { "UI/Runes", "Rune" },
      { "UI/Difficulty", "Difficoltà" },
      { "UI/Mode", "Modalità di gioco" },
      { "UI/CasualMode", "Modalità casuale" },
      { "UI/ChallengeMode", "Modalità sfida" },
      { "UI/InfiniteMode", "Modalità infinita" },
      { "UI/Play", "Gioca" },
      { "UI/Confirm", "Conferma" },
      { "UI/Cancel", "Annulla" },
      { "UI/GemList", "Elenco gemme" },
      { "UI/HeroDescription/MaxHp", "HP massimi" },
      { "UI/HeroDescription/HpRecover", "Recupero HP" },
      { "UI/HeroDescription/Resurrection", "Resurrezione" },
      { "UI/HeroDescription/BasicSkillHurt", "Danni abilità base" },
      { "UI/HeroDescription/BasicSkillProperty", "Attributo abilità base" },
      { "UI/HeroDescription/Possibility", "Possibilità EA" },
      { "UI/HeroDescription/AttackRange", "Raggio d'attacco" },
      { "UI/HeroDescription/AttackPerRound", "Attacchi per round" },
      { "UI/HeroDescription/AttackSpeed", "Velocità d'attacco" },
      { "UI/HeroDescription/PickUpRange", "Raggio di raccolta" },
      { "UI/HeroDescription/MovingSpeed", "Velocità di movimento" },
      { "UI/HeroDescription/AttackMovingSpeed", "Velocità movimento attacco" },
      { "UI/HeroDescription/BaseSkill", "Abilità base" },
      { "UI/HeroDescription/AdditionalEffect", "Effetto aggiuntivo" },
      { "UI/HeroDescription/ProjectileCount", "Conteggio proiettili" },
      { "UI/HeroDescription/AEHurt", "Danni EA" },
      { "UI/FightPreparation/TwoPlayersUniqueHero", "Due giocatori non possono scegliere lo stesso eroe" },
      { "UI/FightPreparation/BrowserHeroes", "Eroi del browser" },
      { "UI/FightPreparation/Back", "Indietro" },
      { "UI/FightPreparation/ApplyGems", "Indossare gemme" },
      { "UI/FightPreparation/ApplyRunes", "Attivare rune" },
      { "UI/FightPreparation/SelectHero", "Seleziona eroe" },
      { "UI/FightPreparation/Unlock", "Sblocca" },
      {
        "UI/FightPreparation/AddPlayer",
        "Premi <color=red>A</color> sul gamepad o Ctrl destro sulla tastiera per aggiungere il 2° giocatore"
      },
      { "UI/FightPreparation/RemovePlayer", "Rimuovi" },
      { "UI/PickGemUI/PickGemTitle", "Indossare gemme" },
      { "UI/PickGemUI/OneGemOnePlayer", "Una gemma può essere utilizzata solo da un singolo giocatore" },
      { "UI/PickRuneUI/PickRuneTitle", "Attivare rune" },
      { "UI/PickRuneUI/Class", "Livello" },
      { "UI/PickRuneUI/InspirationRune", "Runa ispirazione" },
      { "UI/PickRuneUI/DominationRune", "Runa dominazione" },
      { "UI/PickRuneUI/ImmortalRune", "Runa immortale" },
      { "UI/PickRuneUI/Upgrade", "Aggiornamento" },
      { "UI/PickRuneUI/Price", "Prezzo" },
      { "UI/PickRuneUI/OneRunePerClass", "Si può scegliere una sola runa per livello" },
      { "UI/Pause/PhysicalAttack", "Attacco fisico" },
      { "UI/Pause/IceAttack", "Attacco di ghiaccio" },
      { "UI/Pause/FireAttack", "Attacco di fuoco" },
      { "UI/Pause/ThunderAttack", "Attacco di fulmine" },
      { "UI/Pause/Poisoning", "Avvelenamento" },
      { "UI/Pause/MovingSpeed", "Velocità di movimento" },
      { "UI/Pause/AttackingSpeed", "Velocità durante l'attacco" },
      { "UI/Pause/AttackSpeed", "Velocità di attacco" },
      { "UI/Pause/RecoverSpeed", "Velocità di recupero" },
      { "UI/Pause/ExpBonus", "Bonus esperienza" },
      { "UI/Pause/RoundsPerSecond", " round/s" },
      { "UI/Pause/Resume", "Riprendi" },
      { "UI/Pause/ControlGuide", "Guida ai comandi" },
      { "UI/Pause/GiveUp", "Arrenditi" },
      {
        "UI/Upgrade/DemoUpgradeUnlocked",
        "Aggiornamenti già sbloccati nella versione demo: 96/188Potrebbero esserci modifiche all'aggiornamento nella versione ufficiale"
      },
      { "UI/GameOver/Quit", "Esci" },
      { "UI/FightReady/Difficulty", "Difficoltà" },
      { "UI/FightReady/DifficultyNumber", "Livello.{0}" },
      { "UI/FightReady/UnlockedDifficulty", "Sbloccato {0}/{1}" },
      { "UI/FightReady/Mode", "Modalità" },
      { "UI/FightReady/Map", "Mappa" },
      { "UI/FightReady/Description", "Descrizione" },
      { "UI/FightReady/Casual", "Casuale" },
      { "UI/FightReady/Standard", "Standard" },
      { "UI/FightReady/Infinite", "Infinito" },
      { "UI/FightReady/Forest", "Foresta Nebbiosa" },
      { "UI/FightReady/Desert", "Deserto Infuocato" },
      { "UI/FightReady/Dungeon", "Segrete Oscure" },
      { "UI/FightReady/Graveyard", "Cimitero della Morte" },
      { "UI/FightReady/Hell", "Inferno Supremo" },
      { "UI/FightReady/ModeDescription/CasualMode", "La difficoltà dei mostri sarà ridotta nella Modalità Casuale." },
      { "UI/FightReady/ModeDescription/BossAppearTime", "Il boss apparirà dopo {0} minuti" },
      {
        "UI/FightReady/AttributeDescription",
        "Rapporto attributi nemico: Fisico - {0}%, Ghiaccio - {1}%, Fuoco - {2}%, Fulmine - {3}%, Veleno - {4}%"
      },
      { "UI/Prompt/UnlockRune", "Sbloccare la runa <b>{0}</b> per {1}, continuare?" },
      { "UI/Prompt/UnlockHero", "Sbloccare l'eroe <b>{0}</b> per {1}, continuare?" },
      { "UI/Info/Unlock/InsufficientBalance", "Spiacenti, saldo insufficiente per sbloccare. (Requisito: {0})" },
      { "UI/Info/Upgrade/InsufficientBalance", "Spiacenti, saldo insufficiente per aggiornare." },
      { "UI/Info/RunePrerequisitesNotSatisfied", "Si prega di sbloccare prima la runa del livello precedente." },
      { "UI/Audio/Overall", "Generale" },
      { "UI/Audio/Bgm", "BGM" },
      { "UI/Audio/Sfx", "SFX" },
      { "Physical", "Fisico" },
      { "Ice", "Ghiaccio" },
      { "Thunder", "Fulmine" },
      { "Fire", "Fuoco" },
      { "Poison", "Veleno" },
      { "Hero/Archangel", "Seraphiel" },
      { "Hero/CaptainG", "Tempesta di proiettili" },
      { "Hero/Cutie", "Faye Spark" },
      { "Hero/Gumdam", "Attaccante di comete" },
      { "Hero/JeanneDArc", "Santa Giovanna" },
      { "Hero/MountainKing", "Signore del tuono" },
      { "Hero/Paladin", "Templare di ferro" },
      { "Hero/Ranger", "Velenospina" },
      { "Hero/Witch", "Arcanista" },
      { "Hero/WuKong", "Sun Wukong" },
      { "Hero/ZhaoYun", "Zhao Yun" },
      {
        "UI/Hero/Description/Archangel",
        "Seraphiel spara stalattiti con una possibilità di causare Congelamento. Può recuperare una certa quantità di salute al secondo ed è immune ai danni dell'Inferno."
      },
      {
        "UI/Hero/Description/CaptainG",
        "Tempesta di proiettili spara 3 proiettili di fuoco con una possibilità di causare Bruciatura. Inoltre, è immune a Bruciatura."
      },
      { "UI/Hero/Description/Cutie", "Faye Spark spara proiettili con penetrazione infinita." },
      {
        "UI/Hero/Description/Gumdam",
        "Attaccante di comete spara 2 missili da un bazooka che esplode, danneggiando tutti i nemici vicini. Ma i missili possono avere una zona morta."
      },
      {
        "UI/Hero/Description/JeanneDArc",
        "Santa Giovanna attacca con la sua spada (Attacco in mischia), che può trapassare i nemici. Inoltre, non ha bisogno di tempo di ricarica."
      },
      {
        "UI/Hero/Description/MountainKing",
        "Signore del tuono rilascia fulmini che possono causare Stordimento nemico, ed è anche immune a Stordimento."
      },
      {
        "UI/Hero/Description/Paladin",
        "Il Templare di ferro può ripristinare una piccola quantità di salute al secondo ed è immune ai danni dell'Inferno."
      },
      {
        "UI/Hero/Description/Ranger",
        "Velenospina spara 2 frecce velenose che automaticamente circondano il bersaglio. Ma le frecce possono avere una zona morta."
      },
      {
        "UI/Hero/Description/Witch",
        "L'Arcanista scatena 2 sfere di fulmine per attaccare il nemico. Ha un'opzione di potenziamento aggiuntiva salendo di livello."
      },
      {
        "UI/Hero/Description/WuKong",
        "Sun Wukong può teletrasportarsi in altri luoghi (premere rapidamente il pulsante tre volte per attivare)."
      },
      {
        "UI/Hero/Description/ZhaoYun",
        "Zhao Yun spara 3 energie di spada ghiacciata in tutte le direzioni per attaccare il nemico. Inoltre, è immune a Congelamento."
      },
      { "Level/Pause", "Pausa" },
      { "Level/PressEscToResume", "Premere Esc per riprendere" },
      { "Upgrade/RecoverHp/Name", "Recupera Liv{0}" },
      { "Upgrade/RecoverHp/Description", "Recupera {0:F2}% di HP" },
      { "Upgrade/IncreaseMaxHp/Name", "MaxHP Liv{0}" },
      { "Upgrade/IncreaseMaxHp/Description", "Aumenta HP massima di {0:F2}%" },
      { "Upgrade/IncreaseMaxHpOnHurt/Name", "Timore Liv{0}" },
      {
        "Upgrade/IncreaseMaxHpOnHurt/Description",
        "Aumenta HP massima di {0:F2}% quando il giocatore è ferito. L'incremento massimo sarà di {1:F2}%."
      },
      { "Upgrade/IncreaseDefense/Name", "Scudo Liv{0}" },
      { "Upgrade/IncreaseDefense/Description", "Aumenta la difesa di {0:F2}%" },
      { "Upgrade/IncreaseAllSpeed/Name", "Vai veloce! Liv{0}" },
      {
        "Upgrade/IncreaseAllSpeed/Description",
        "Aumenta la velocità di {0:F2}% inclusa la velocità normale e la velocità di attacco."
      },
      { "Upgrade/IncreaseMovingSpeed/Name", "Scarpa da corsa Liv{0}" },
      { "Upgrade/IncreaseMovingSpeed/Description", "Aumenta la velocità di movimento normale di {0:F2}%." },
      { "Upgrade/IncreaseAttackingMovingSpeed/Name", "Colpisci e corri Liv{0}" },
      {
        "Upgrade/IncreaseAttackingMovingSpeed/Description",
        "Aumentare la velocità di movimento durante l'attacco di {0:F2}%."
      },
      { "Upgrade/KillToIncreaseSpeed/Name", "Mercenario Liv{0}" },
      {
        "Upgrade/KillToIncreaseSpeed/Description",
        "Uccidi {0} nemici per aumentare la velocità di {1:F2}% e l'incremento massimo sarà di {2:F2}%. L'incremento di velocità verrà resettato dopo essere stato ferito."
      },
      { "Upgrade/ExpBonus/Name", "Nerd Liv{0}" },
      { "Upgrade/ExpBonus/Description", "Aumenta l'esperienza guadagnata ogni volta di {0:F2}%." },
      { "Upgrade/IncreasePickUp/Name", "Indiana Jones Liv{0}" },
      { "Upgrade/IncreasePickUp/Description", "Aumenta il raggio di raccolta di {0:F2}%." },
      { "Upgrade/IncreaseShootingRange/Name", "Barrett Liv{0}" },
      { "Upgrade/IncreaseShootingRange/Description", "Aumenta la gittata di tiro di {0:F2}%." },
      { "Upgrade/AddProjectile/Name", "Fucile a canne mozze Liv{0}" },
      {
        "Upgrade/AddProjectile/Description",
        "Aggiungi {0} proiettili aggiuntivi. Tuttavia, la dispersione aumenterà leggermente."
      },
      { "Upgrade/IncreaseDispersion/Name", "Dispersione Liv{0}" },
      { "Upgrade/IncreaseDispersion/Description", "Aumenta la dispersione di {0:F2}%." },
      { "Upgrade/ReduceDispersion/Name", "Fucile di precisione Liv{0}" },
      { "Upgrade/ReduceDispersion/Description", "Riduci la dispersione di {0:F2}%." },
      { "Upgrade/IncreaseFiringRate/Name", "Fuoco rapido Liv{0}" },
      { "Upgrade/IncreaseFiringRate/Description", "Aumenta la frequenza di fuoco di {0:F2}%." },
      { "Upgrade/BurstFire/Name", "Raffica di fuoco Liv{0}" },
      {
        "Upgrade/BurstFire/Description",
        "Aumenta la frequenza di fuoco di {0:F2}% per {1:F2} secondi dopo aver subito danni."
      },
      { "Upgrade/IncreaseMagazineSize/Name", "Caricatore a tamburo Liv{0}" },
      { "Upgrade/IncreaseMagazineSize/Description", "Aumenta la dimensione del caricatore di {0}." },
      { "Upgrade/ReduceReloadingTime/Name", "Caricatore rapido Liv{0}" },
      { "Upgrade/ReduceReloadingTime/Description", "Riduci il tempo di ricarica di {0:F2}%." },
      { "Upgrade/BurstReload/Name", "Caricatore della paura Liv{0}" },
      {
        "Upgrade/BurstReload/Description",
        "Riduci il tempo di ricarica di {0:F2}% per {1:F2} secondi dopo aver subito danni."
      },
      { "Upgrade/IncreaseHurtPercentage/Name", "Affilatura Liv{0}" },
      { "Upgrade/IncreaseHurtPercentage/Description", "Aumenta il danno dell'abilità base di {0:F2}%." },
      { "Upgrade/IncreaseIceAeDuration/Name", "Ghiacciato Liv{0}" },
      { "Upgrade/IncreaseIceAeDuration/Description", "Aumenta la durata dell'effetto Congelato di {0:F2}%." },
      { "Upgrade/IncreaseFireAeDuration/Name", "Brucia Liv{0}" },
      { "Upgrade/IncreaseFireAeDuration/Description", "Aumenta la durata dell'effetto Bruciatura di {0:F2}%." },
      { "Upgrade/IncreaseThunderAeDuration/Name", "Tempesta di fulmini Liv{0}" },
      { "Upgrade/IncreaseThunderAeDuration/Description", "Aumenta la durata dell'effetto Stordimento di {0:F2}%." },
      { "Upgrade/ActivateDart/Name", "Ninja" },
      { "Upgrade/ActivateDart/Description", "Lancia un dardo che ti circonda." },
      { "Upgrade/AddDart/Name", "Sensei Liv{0}" },
      { "Upgrade/AddDart/Description", "Lancia {0} dardi in più intorno a te." },
      { "Upgrade/PoisonDart/Name", "Dardo velenoso Liv{0}" },
      {
        "Upgrade/PoisonDart/Description",
        "Imbevi tutti i dardi nel veleno, dando loro una probabilità di {0:F2}% di avvelenare il nemico e infliggere {1:F2}% di danno al secondo (inefficace contro i boss)."
      },
      { "Upgrade/ImproveDartHurt/Name", "Affilato Liv{0}" },
      { "Upgrade/ImproveDartHurt/Description", "Affila tutti i dardi intorno a te, migliorando {0:F2}% dei danni." },
      { "Upgrade/ActivateBoomerang/Name", "Aborigeni" },
      { "Upgrade/ActivateBoomerang/Description", "Lancia un boomerang che ti circonda." },
      { "Upgrade/AddBoomerang/Name", "Manuel Schütz Liv{0}" },
      { "Upgrade/AddBoomerang/Description", "Lancia {0} boomerang in più intorno a te." },
      { "Upgrade/BurnBoomerang/Name", "Boomerang di fuoco Liv{0}" },
      {
        "Upgrade/BurnBoomerang/Description",
        "Dà al boomerang una probabilità di {0:F2}% di incendiare il nemico per {1:F2} secondi, infliggendo {2:F2}% di danno al secondo."
      },
      { "Upgrade/IncreaseBoomerangSpeed/Name", "Aerodinamica Liv{0}" },
      {
        "Upgrade/IncreaseBoomerangSpeed/Description", "Ottimizza l'aerodinamica per accelerare il boomerang di {0:F2}%."
      },
      { "Upgrade/ImproveBoomerangHurt/Name", "Riempimento di piombo Liv{0}" },
      {
        "Upgrade/ImproveBoomerangHurt/Description",
        "Carica il boomerang con piombo, aumentando il suo potere di attacco di {0:F2}%"
      },
      { "Upgrade/ActivateIceTower/Name", "Torre di ghiaccio" },
      {
        "Upgrade/ActivateIceTower/Description",
        "Fai volare una Torre di Ghiaccio automatica intorno a te che spara automaticamente grandine."
      },
      { "Upgrade/AddIceTower/Name", "Inverno Liv{0}" },
      { "Upgrade/AddIceTower/Description", "Aggiungi {0} Torri di Ghiaccio in più." },
      { "Upgrade/IncreaseIceTowerFiringRate/Name", "Skadi Liv{0}" },
      {
        "Upgrade/IncreaseIceTowerFiringRate/Description",
        "Aumenta la velocità di attacco della Torre di Ghiaccio di {0:F2}%."
      },
      { "Upgrade/ImproveIceTowerHurt/Name", "Cubo di ghiaccio Liv{0}" },
      { "Upgrade/ImproveIceTowerHurt/Description", "Migliora il danno della Torre di Ghiaccio di {0:F2}%." },
      { "Upgrade/ActivateFireTower/Name", "Torre di fuoco" },
      {
        "Upgrade/ActivateFireTower/Description",
        "Fai volare una Torre di Fuoco automatica intorno a te che spara automaticamente palle di fuoco."
      },
      { "Upgrade/AddFireTower/Name", "Afoso Liv{0}" },
      { "Upgrade/AddFireTower/Description", "Aggiungi {0} Torri di Fuoco in più." },
      { "Upgrade/IncreaseFireTowerFiringRate/Name", "Apollo Liv{0}" },
      {
        "Upgrade/IncreaseFireTowerFiringRate/Description",
        "Aumenta la velocità di attacco della Torre di Fuoco di {0:F2}%."
      },
      { "Upgrade/ImproveFireTowerHurt/Name", "Lanciafiamme Liv{0}" },
      { "Upgrade/ImproveFireTowerHurt/Description", "Migliora il danno della Torre di Fuoco di {0:F2}%." },
      { "Upgrade/ActivateThunderTower/Name", "Torre di fulmini" },
      {
        "Upgrade/ActivateThunderTower/Description",
        "Fai volare una Torre di Fulmini automatica intorno a te che spara automaticamente fulmini."
      },
      { "Upgrade/AddThunderTower/Name", "Fulmine Liv{0}" },
      { "Upgrade/AddThunderTower/Description", "Aggiungi {0} Torri di Fulmini in più." },
      { "Upgrade/IncreaseThunderTowerFiringRate/Name", "Thor Liv{0}" },
      {
        "Upgrade/IncreaseThunderTowerFiringRate/Description",
        "Aumenta la velocità di attacco della Torre di Fulmini di {0:F2}%."
      },
      { "Upgrade/ImproveThunderTowerHurt/Name", "Bobina di Tesla" },
      { "Upgrade/ImproveThunderTowerHurt/Description", "Migliora il danno della Torre di Fulmini di {0:F2}%." },
      { "Upgrade/ActivateSpiral/Name", "Spirale" },
      {
        "Upgrade/ActivateSpiral/Description",
        "Di tanto in tanto, l'eroe rilascia una spirale con attributi casuali per attaccare il nemico."
      },
      { "Upgrade/AddSpiral/Name", "Nautilidae Liv{0}" },
      { "Upgrade/AddSpiral/Description", "Aggiungi {0} spirali in più intorno a te." },
      { "Upgrade/ReduceSpiralInterval/Name", "Pioggia battente Liv{0}" },
      { "Upgrade/ReduceSpiralInterval/Description", "Riduci l'intervallo della Spirale di {0:F2}%." },
      { "Upgrade/IncreaseSpiralHurt/Name", "Tempesta di fulmini Liv{0}" },
      { "Upgrade/IncreaseSpiralHurt/Description", "Aumenta il danno della Spirale di {0:F2}%." },
      { "Upgrade/ActivatePuppet/Name", "Combattente di marionette" },
      { "Upgrade/ActivatePuppet/Description", "Evoca {0} marionette ogni {1:F2} secondi." },
      { "Upgrade/LevelUpPuppetToLv1/Name", "Marionetta armata" },
      {
        "Upgrade/LevelUpPuppetToLv1/Description",
        "Evoca {0} marionette di livello superiore ogni {1:F2} secondi. Tuttavia, i potenziamenti delle marionette attuali vengono resettati."
      },
      { "Upgrade/LevelUpPuppetToLv2/Name", "Marionetta Saiya" },
      {
        "Upgrade/LevelUpPuppetToLv2/Description",
        "Evoca {0} marionette definitive ogni {1:F2} secondi. Tuttavia, i potenziamenti delle marionette attuali vengono resettati."
      },
      { "Upgrade/ReducePuppetIntervalLv0/Name", "Squadra Liv{0}" },
      {
        "Upgrade/ReducePuppetIntervalLv0/Description", "Riduci l'intervallo di evocazione delle marionette di {0:F2}%."
      },
      { "Upgrade/AddPuppetLv0/Name", "Grido Liv{0}" },
      { "Upgrade/AddPuppetLv0/Description", "Evoca {0} marionette in più per combattimenti di gruppo." },
      { "Upgrade/IncreasePuppetHurtLv0/Name", "Coltello Liv{0}" },
      { "Upgrade/IncreasePuppetHurtLv0/Description", "Aumenta il danno di tutte le marionette di {0:F2}%." },
      { "Upgrade/ReducePuppetIntervalLv1/Name", "Cercapersone Liv{0}" },
      {
        "Upgrade/ReducePuppetIntervalLv1/Description", "Riduci l'intervallo di evocazione delle marionette di {0:F2}%."
      },
      { "Upgrade/AddPuppetLv1/Name", "Mafia Liv{0}" },
      {
        "Upgrade/AddPuppetLv1/Description",
        "Evoca {0} burattini aggiuntivi per i combattimenti di gruppo."
      },
      { "Upgrade/IncreasePuppetHurtLv1/Name", "Pistola Liv{0}" },
      { "Upgrade/IncreasePuppetHurtLv1/Description", "Aumenta il danno di tutte le marionette di {0:F2}%." },
      { "Upgrade/ReducePuppetIntervalLv2/Name", "Smartphone Liv{0}" },
      {
        "Upgrade/ReducePuppetIntervalLv2/Description", "Riduci l'intervallo di evocazione delle marionette di {0:F2}%."
      },
      { "Upgrade/AddPuppetLv2/Name", "Esercito Liv{0}" },
      {
        "Upgrade/AddPuppetLv2/Description",
        "Evoca {0} burattini aggiuntivi per i combattimenti di gruppo."
      },
      { "Upgrade/IncreasePuppetHurtLv2/Name", "Fucile Liv{0}" },
      { "Upgrade/IncreasePuppetHurtLv2/Description", "Aumenta il danno di tutte le marionette di {0:F2}%." },
      { "Upgrade/ActivateFlyingSword/Name", "Spada volante" },
      {
        "Upgrade/ActivateFlyingSword/Description",
        "Usa il Qi per controllare le spade, rilasciando {0} spade ogni {1:F2} secondi."
      },
      { "Upgrade/LevelUpFlyingSwordToLv1/Name", "Spada pesante" },
      {
        "Upgrade/LevelUpFlyingSwordToLv1/Description",
        "Usa spade più pesanti, rilasciando {0} spade ogni {1:F2} secondi. Tuttavia, i potenziamenti delle spade verranno resettati."
      },
      { "Upgrade/LevelUpFlyingSwordToLv2/Name", "Spada affilata" },
      {
        "Upgrade/LevelUpFlyingSwordToLv2/Description",
        "Usa spade affilate, rilasciando {0} spade ogni {1:F2} secondi. Tuttavia, i potenziamenti delle spade verranno resettati."
      },
      { "Upgrade/AddFlyingSwordLv0/Name", "Junior Liv{0}" },
      { "Upgrade/AddFlyingSwordLv0/Description", "Aggiungi {0} spade volanti in più." },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Name", "Apprendista Liv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv0/Description", "Riduci l'intervallo di rilascio delle spade del {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Name", "Ingresso dello spadaccino Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Description", "Aumenta il danno delle spade del {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv1/Name", "Senior Lv{0}" },
      { "Upgrade/AddFlyingSwordLv1/Description", "Aggiungi {0} spade volanti in più." },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Name", "Artigiano Lv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv1/Description", "Riduci l'intervallo di rilascio delle spade del {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Name", "Senior dello spadaccino Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Description", "Aumenta il danno delle spade del {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv2/Name", "Shifu Lv{0}" },
      { "Upgrade/AddFlyingSwordLv2/Description", "Aggiungi {0} spade volanti in più." },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Name", "Cacciatore di Sole Lv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv2/Description", "Riduci l'intervallo di rilascio delle spade del {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Name", "Zongshi lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Description", "Aumenta il danno delle spade del {0:F2}%." },
      { "Upgrade/ActivateMine/Name", "Mina terrestre" },
      {
        "Upgrade/ActivateMine/Description",
        "Piazza {0} mine che esplodono e causano {1:F2} danni entro un raggio di {2:F2} metri ogni {3:F2} secondi. Inoltre, le mine innescano l'esplosione delle mine vicine.\n"
      },
      { "Upgrade/LevelUpMineToLv1/Name", "Mina di fulmine" },
      {
        "Upgrade/LevelUpMineToLv1/Description",
        "Piazza {0} mine potenziate che esplodono e causano {1:F2} danni entro un raggio di {2:F2} metri ogni {3:F2} secondi."
      },
      { "Upgrade/LevelUpMineToLv2/Name", "Mina Claymore" },
      {
        "Upgrade/LevelUpMineToLv2/Description",
        "Piazza {0} mine estremamente letali che esplodono e causano {1:F2} danni entro un raggio di {2:F2} metri ogni {3:F2} secondi."
      },
      { "Upgrade/AddMineLv0/Name", "Boy Scout Lv{0}" },
      { "Upgrade/AddMineLv0/Description", "Piazza {0} mine terrestri in più ogni volta." },
      { "Upgrade/ReduceMineIntervalLv0/Name", "Pinze Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv0/Description", "Riduce l'intervallo tra le mine del {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv0/Name", "Graffio Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv0/Description", "Aumenta il danno delle mine terrestri del {0:F2}%." },
      { "Upgrade/AddMineLv1/Name", "Minutemen Lv{0}" },
      { "Upgrade/AddMineLv1/Description", "Piazza {0} mine di fulmine in più ogni volta." },
      { "Upgrade/ReduceMineIntervalLv1/Name", "Cassetta degli attrezzi Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv1/Description", "Riduci l'intervallo tra le mine del {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv1/Name", "Sfera d'acciaio Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv1/Description", "Aumenta il danno delle mine di fulmine del {0:F2}%." },
      { "Upgrade/AddMineLv2/Name", "Forze Speciali Lv{0}" },
      { "Upgrade/AddMineLv2/Description", "Piazza {0} mine di fulmine in più ogni volta." },
      { "Upgrade/ReduceMineIntervalLv2/Name", "Sistema di Mine Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv2/Description", "Riduci l'intervallo tra le mine del {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv2/Name", "Frammentazione Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv2/Description", "Aumenta il danno delle mine Claymore del {0:F2}%." },
      { "MapIndicator/Forest", "Foresta Nebbiosa" },
      { "MapIndicator/Desert", "Deserto Ardente" },
      { "MapIndicator/Dungeon", "Dungeon Oscuro" },
      { "MapIndicator/Graveyard", "Cimitero della Morte" },
      { "MapIndicator/Hell", "Inferno Ultimo" },
      {
        "MapDescription/Forest",
        "Una foresta nebbiosa, dove i proiettili spesso sparano fuori dal nulla tra gli alberi apparentemente tranquilli."
      },
      {
        "MapDescription/Desert",
        "Sembra che molti mostri che possono sopravvivere nel duro deserto siano immuni agli attributi."
      },
      {
        "MapDescription/Dungeon",
        "Nel dungeon, ci sono spesso l'ululato dei lupi e il suono dei martelli che oscillano."
      },
      {
        "MapDescription/Graveyard", "Fai attenzione ai teschi elusivi nel cimitero! Potresti farti male se li incontri!"
      },
      {
        "MapDescription/Hell",
        "L'inferno è incantato dal diavolo, causando più o meno danni agli eroi ogni 10 secondi! Ma sembra che alcuni eroi non se ne preoccupino affatto."
      },
      { "AdditionalEffect/Type/Freeze", "Congelamento" },
      { "AdditionalEffect/Type/Stun", "Stordimento" },
      { "AdditionalEffect/Type/Burn", "Ustione" },
      { "AdditionalEffect/Type/Poison", "Veleno" },
      { "Gem/GemSynthesis", "Sintesi di gemme" },
      { "Gem/Rarity/R", "Raro" },
      { "Gem/Rarity/SR", "Super Raro" },
      { "Gem/Rarity/SSR", "Super Super Raro" },
      { "Gem/Rarity/XR", "Estremamente Raro" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByPercentage", "Velocità di movimento in attacco {0}{1}%" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByValue", "Velocità di movimento in attacco {0}{1}" },
      { "Gem/Upgrade/AugmentMovingSpeedByPercentage", "Velocità di movimento {0}{1}%" },
      { "Gem/Upgrade/AugmentMovingSpeedByValue", "Velocità di movimento {0}{1}" },
      { "Gem/Upgrade/CoolingCountdownDecrementByPercentage", "Tempo di ricarica abilità base {0}{1}%" },
      { "Gem/Upgrade/ReloadCountdownDecrementByPercentage", "Tempo di ricarica abilità base {0}{1}%" },
      { "Gem/Upgrade/DispersionDecrementByPercentage", "Dispersione abilità base {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByPercentage", "Penetrazione abilità base {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByValue", "Penetrazione abilità base {0}{1}" },
      { "Gem/Upgrade/RangeIncrementByPercentage", "Gittata abilità base {0}{1}%" },
      { "Gem/Upgrade/RangeIncrementByValue", "Gittata abilità base {0}{1}" },
      { "Gem/Upgrade/RepelForceIncrementByPercentage", "Forza di respinta abilità base {0}{1}%" },
      { "Gem/Upgrade/RepelForceIncrementByValue", "Forza di respinta abilità base {0}{1}" },
      { "Gem/Upgrade/SkillCountIncrementByValue", "Proiettile abilità base {0}{1}" },
      { "Gem/Upgrade/SpeedIncrementByPercentage", "Velocità abilità base {0}{1}%" },
      { "Gem/Upgrade/SpeedIncrementByValue", "Velocità abilità base {0}{1}" },
      { "Gem/Upgrade/EnableRangeAttack", "Abilita l'attacco a distanza per l'abilità base" },
      { "Gem/Upgrade/AugmentDamageRangeByValue", "Gittata danno abilità base {0}{1}" },
      { "Gem/Upgrade/AugmentDamageRangeByPercentage", "Gittata danno abilità base {0}{1}%" },
      { "Gem/Upgrade/AddPoisoningAdditionalEffect", "Aggiungi effetto aggiuntivo Avvelenamento" },
      { "Gem/Upgrade/SetPoisoningPossibility", "Imposta la probabilità di avvelenamento al {0}%" },
      { "Gem/Upgrade/SetPoisoningHurtPercentage", "Imposta la percentuale di danno da avvelenamento al {0}%" },
      { "Gem/Upgrade/AugmentPoisoningPossibility", "Probabilità di avvelenamento abilità {0}{1}%" },
      { "Gem/Upgrade/AugmentPoisoningHurtPercentage", "Percentuale di danno da avvelenamento abilità {0}{1}%" },
      {
        "Gem/Upgrade/AddOrConvertBasicAdditionalEffect",
        "Aggiungi o converti l'attributo dell'effetto aggiuntivo in <b>{0}</b>"
      },
      {
        "Gem/Upgrade/SetBasicAdditionalEffectPossibility",
        "Imposta la probabilità dell'effetto aggiuntivo dell'abilità al {0}%"
      },
      { "Gem/Upgrade/AugmentBasicAdditionalEffectPossibility", "Probabilità effetto aggiuntivo abilità {0}{1}%" },
      { "Gem/Upgrade/AugmentPhysicalHurtByValue", "Danno fisico {0}{1}" },
      { "Gem/Upgrade/AugmentPhysicalHurtByPercentage", "Danno fisico {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicIceHurtByValue", "Danno magico di ghiaccio {0}{1}" },
      { "Gem/Upgrade/AugmentMagicIceHurtByPercentage", "Danno magico di ghiaccio {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicFireHurtByValue", "Danno magico di fuoco {0}{1}" },
      { "Gem/Upgrade/AugmentMagicFireHurtByPercentage", "Danno magico di fuoco {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicThunderByValue", "Danno magico di fulmine {0}{1}" },
      { "Gem/Upgrade/Augment/MagicThunderByPercentage", "Danno magico di fulmine {0}{1}%" },
      { "Gem/Upgrade/Augment/Resurrection", "Conteggio resurrezione {0}{1}" },
      { "Gem/Upgrade/Augment/IncreaseMaxHp", "Hp massimo {0}{1}" },
      {
        "Rune/Description/ClonedProjectile",
        "Aumenta il numero di abilità di base. Ogni potenziamento aggiunge un proiettile."
      },
      {
        "Rune/Description/ExpBonus",
        "Aumenta l'esperienza guadagnata dagli eroi del 10%. L'esperienza guadagnata aumenta del 7% ogni volta che l'eroe sale di livello."
      },
      {
        "Rune/Description/Fission",
        "Dopo che le abilità di base dell'eroe colpiscono il nemico, c'è una certa possibilità di dividersi. La probabilità di divisione iniziale è del 10% e aumenta del 2% ogni volta che viene potenziata."
      },
      {
        "Rune/Description/HailStrike",
        "abilità attive. Quando attivata, evoca chicchi di grandine per attaccare i nemici nel raggio visibile. L'abilità dura 7 secondi e aumenta di 2 secondi con ogni potenziamento."
      },
      {
        "Rune/Description/HolyShield",
        "abilità attive. Dopo l'attivazione, l'eroe ottiene 10 secondi di invincibilità e ogni potenziamento aumenta di 2 secondi."
      },
      {
        "Rune/Description/IncreaseHonor",
        "Alla fine del gioco, il valore onore guadagnato dal giocatore aumenta. L'aumento iniziale è del 10% e ogni potenziamento aumenta del 10%."
      },
      {
        "Rune/Description/InstantKill",
        "Quando le abilità base dell'eroe colpiscono il nemico, c'è una certa probabilità che il nemico muoia all'istante. La probabilità iniziale è dell'1% e ogni potenziamento aumenta dello 0,5%."
      },
      {
        "Rune/Description/InstantReload",
        "C'è una probabilità del 10% di ricaricare istantaneamente quando le munizioni si esauriscono. Ogni potenziamento aumenta la probabilità del 5%."
      },
      {
        "Rune/Description/KillAndRecover",
        "abilità attive. Ogni volta che l'eroe uccide un nemico entro 10 secondi, l'eroe può recuperare l'1% della sua salute. La durata di ogni potenziamento aumenta di 2 secondi."
      },
      {
        "Rune/Description/IncreaseImmortalTime",
        "Aumenta il tempo di invulnerabilità dopo aver subito danni. Lo stato iniziale aumenta di 0,25 secondi e ogni potenziamento aumenta di 0,15 secondi."
      },
      {
        "Rune/Description/IncreaseMaxHp",
        "Aumenta la salute massima dell'eroe del 10% e aumenta del 5% per ogni potenziamento."
      },
      {
        "Rune/Description/MeteoriteStrike",
        "abilità attive. Evoca meteoriti per attaccare i nemici nel raggio visibile per 10 secondi. Inizialmente, cadono 10 meteoriti al secondo e ogni potenziamento aumenta di 5."
      },
      {
        "Rune/Description/PickUpDistance",
        "La distanza di raccolta dell'eroe aumenta del 10% e ogni potenziamento aumenta del 10%."
      },
      {
        "Rune/Description/Poisonous",
        "abilità attive. Infligge danni da veleno ai nemici nel raggio visibile ogni secondo per 10 secondi. Ogni livello aumenta la durata di 2 secondi."
      },
      {
        "Rune/Description/PushAway",
        "Respinge i nemici vicini ogni volta che l'eroe esaurisce le munizioni (tempo di ricarica di 10 secondi). Ogni potenziamento aumenta la spinta del 20%."
      },
      {
        "Rune/Description/HpRecovery",
        "Recupera lo 0,2% della salute dell'eroe al secondo e aumenta la quantità di recupero dello 0,2% per livello."
      },
      {
        "Rune/Description/ReducedInjuery",
        "Il danno subito dall'eroe è ridotto del 5%, e un ulteriore 5% è ridotto per ogni livello."
      },
      {
        "Rune/Description/Resurrection",
        "Quando un eroe muore, viene immediatamente rianimato con il 25% della sua salute ripristinata. Dopo la resurrezione, la salute aumenta del 15% ogni volta che viene potenziata."
      },
      {
        "Rune/Description/ThunderStrike",
        "abilità attive. Evoca fulmini per attaccare i nemici nel raggio visibile con precisione. L'abilità dura 10 secondi. Inizialmente attacca il 10% dei nemici al secondo, ogni potenziamento attaccherà un ulteriore 3% di nemici."
      },
      {
        "Rune/Description/TimeStop",
        "Mette in pausa tutte le azioni nemiche per 5 secondi. Ogni potenziamento aumenta la durata della pausa temporale di 2 secondi."
      },
      { "Rune/Title/ClonedProjectile", "Proiettile Clonato" },
      { "Rune/Title/ExpBonus", "Libro dell'Esperienza" },
      { "Rune/Title/Fission", "Scissione" },
      { "Rune/Title/HailStrike", "Grandine" },
      { "Rune/Title/HolyShield", "Scudo Sacro" },
      { "Rune/Title/IncreaseHonor", "Campione" },
      { "Rune/Title/InstantKill", "Un Colpo Un'Uccisione" },
      { "Rune/Title/InstantReload", "Multi Caricatore" },
      { "Rune/Title/KillAndRecover", "Assetato di Sangue" },
      { "Rune/Title/IncreaseImmortalTime", "Elmo da Cavaliere" },
      { "Rune/Title/IncreaseMaxHp", "Braccio Forte" },
      { "Rune/Title/MeteoriteStrike", "Meteorite" },
      { "Rune/Title/PickUpDistance", "Cacciatore di Taglie" },
      { "Rune/Title/Poisonous", "Zona Gas" },
      { "Rune/Title/PushAway", "Lascia Stare" },
      { "Rune/Title/HpRecovery", "Croce Rossa" },
      { "Rune/Title/ReducedInjuery", "Cavaliere Corazzato" },
      { "Rune/Title/Resurrection", "Miracolo" },
      { "Rune/Title/ThunderStrike", "Fulmine" },
      { "Rune/Title/TimeStop", "Macchina del Tempo" },
      { "Exception/CorruptedSaveFile", "Il file di salvataggio è danneggiato ed è stato eseguito il backup in {0}." },
    };
  }
}