using System.Collections.Generic;

namespace Code.Scripts.Src.I18n
{
  public struct pt
  {
    public static readonly Dictionary<string, string> locales = new()
    {
      { "HelloWorld", "Olá, Mundo!" },
      { "Language", "Português" },
      { "NotAvailableInDemo", "Em Desenvolvimento" },
      { "Player1", "Jogador 1" },
      { "Player2", "Jogador 2" },
      {
        "Notification/WelcomeNotification",
        "Obrigado por adquirir nosso jogo!Sabemos que nosso jogo não é perfeito, por isso precisamos do seu feedback!Fique ligado para atualizações contínuas."
      },
      { "Measure/PerSecond", "/seg" },
      {
        "UI/FightPreparationGuide/Page0/0",
        "Existem 5 atributos para os ataques de heróis e inimigos, nomeadamente físico, gelo, fogo, trovão e veneno."
      },
      {
        "UI/FightPreparationGuide/Page0/1",
        "Entre eles, o atributo gelo restringe o atributo fogo, o atributo fogo restringe o atributo trovão e o atributo trovão restringe o atributo gelo."
      },
      {
        "UI/FightPreparationGuide/Page1/0",
        "O atributo gelo pode desencadear o efeito de congelamento e causar dano numérico;\nO atributo fogo pode desencadear o efeito de ignição e causar dano proporcional;\nO atributo trovão pode desencadear o efeito de atordoamento, mas não causará dano adicional.\nTodos os atributos acima serão eliminados após um período de tempo.\nO atributo veneno pode desencadear envenenamento e continuar a causar dano proporcional. Mas os chefes podem frequentemente aliviar ou mesmo remover o envenenamento."
      },
      {
        "UI/FightPreparationGuide/Page1/1",
        "Alguns inimigos têm defesa de atributo, imunidade a efeitos de atributo e quebra de defesa de atributo. Fazer bom uso dos atributos que restringem o inimigo pode facilitar a derrota do oponente!"
      },
      {
        "UI/FightReadyGuide/Page0/0",
        "Aqui serão mostrados os atributos dos monstros no mapa atual. Lembre-se desses atributos e combine sua própria estratégia para derrotar os monstros!"
      },
      {
        "UI/FightReadyGuide/Page1/0",
        "Essas duas posições mostrarão os chefes sentados no mapa. Cada chefe tem diferentes habilidades e estilos de combate. Tente usar diferentes técnicas para derrotar diferentes chefes!"
      },
      {
        "UI/FightReadyGuide/Page1/1",
        "Não se esqueça de explorar o mapa com mais frequência, onde você pode coletar diferentes gemas para fortalecer seus heróis.Você também pode usar as honras que ganha nas batalhas para desbloquear novos heróis e runas para uma melhor exploração em diferentes mapas!"
      },
      { "UI/ControlGuide/ActiveSkill", "Habilidade Ativa" },
      { "UI/ControlGuide/AutoFiring", "Disparo Automático" },
      { "UI/ControlGuide/Movement", "Movimento" },
      { "UI/ControlGuide/Fire!", "Fogo!" },
      { "UI/ControlGuide/AutoAiming", "Mira Automática" },
      { "UI/ControlGuide/Aim", "Mirando" },
      {
        "UI/ControlGuide/MoveToUseController",
        "Mova o joystick para usar a mira do controle (fora do modo de mira automática)."
      },
      {
        "UI/ControlGuide/MoveToUseMouse", "Mova o mouse para usar a mira do mouse (fora do modo de mira automática)."
      },
      { "UI/Control/AutoAiming", "Mira automática" },
      { "UI/Control/MouseAiming", "Mira do mouse" },
      { "UI/Control/ControllerAiming", "Mira do controle" },
      { "UI/Control/AutoFiring", "Tiro automático" },
      { "UI/Control/ManualFiring", "Tiro manual" },
      { "UI/Control/UseMouseSelectHero", "Use o mouse para selecionar um herói" },
      { "UI/Text/LevelUp!", "Subir de Nível!" },
      { "UI/Button/Choose", "Escolher" },
      { "UI/Start", "Começar" },
      { "UI/Languages", "Idiomas" },
      { "UI/Options", "Opções" },
      { "UI/Exit", "Sair" },
      { "UI/Name", "Nome" },
      { "UI/Description", "Descrição" },
      { "UI/Properties", "Propriedades" },
      { "UI/Properties/AttacksPerRound", "Ataques por Rodada" },
      { "UI/Properties/RateOfFire", "Taxa de Disparo" },
      { "UI/Properties/ReloadTime", "Tempo de Recarga" },
      { "UI/Properties/Projectiles", "Projéteis" },
      { "UI/Properties/FreezeDamagePerSecond", "Dano por Segundo" },
      { "UI/Properties/MovingSpeed", "Velocidade de Movimento" },
      { "UI/Properties/Physical", "Físico" },
      { "UI/Properties/Ice", "Gelo" },
      { "UI/Properties/Fire", "Fogo" },
      { "UI/Properties/Thunder", "Trovão" },
      { "UI/Properties/Poisoning", "Envenenamento" },
      { "UI/GameOver", "Fim de Jogo" },
      { "UI/FinalScore", "Pontuação Final" },
      { "UI/HonorGained", "Honra Ganha" },
      { "UI/PlayerHonor", "Honra do Jogador" },
      { "UI/PlayerScore", "Pontuação do Jogador" },
      { "UI/EnemyKilled", "Inimigo Morto" },
      { "UI/ExperienceGained", "Experiência Ganha" },
      { "UI/FinalLevel", "Nível Final" },
      { "UI/SurvivalTime", "Tempo de Sobrevivência" },
      { "UI/Upgrades", "Melhorias" },
      { "UI/GameOver/Failed", "DERROTA!" },
      { "UI/GameOver/Success", "NÍVEL CONCLUÍDO!" },
      { "UI/GameOver/Aborted", "MISSÃO ABORTADA" },
      { "UI/Achievements", "Conquistas" },
      { "UI/Furnace", "Fornalha" },
      { "UI/Gems", "Gemas" },
      { "UI/Runes", "Runas" },
      { "UI/Difficulty", "Dificuldade" },
      { "UI/Mode", "Modo de Jogo" },
      { "UI/CasualMode", "Modo Casual" },
      { "UI/ChallengeMode", "Modo Desafio" },
      { "UI/InfiniteMode", "Modo Infinito" },
      { "UI/Play", "Jogar" },
      { "UI/Confirm", "Confirmar" },
      { "UI/Cancel", "Cancelar" },
      { "UI/GemList", "Lista de Gemas" },
      { "UI/HeroDescription/MaxHp", "HP Máximo" },
      { "UI/HeroDescription/HpRecover", "Recuperação de HP" },
      { "UI/HeroDescription/Resurrection", "Ressurreição" },
      { "UI/HeroDescription/BasicSkillHurt", "Dano da Habilidade Básica" },
      { "UI/HeroDescription/BasicSkillProperty", "Atributo da Habilidade Básica" },
      { "UI/HeroDescription/Possibility", "Possibilidade de EA" },
      { "UI/HeroDescription/AttackRange", "Alcance de Ataque" },
      { "UI/HeroDescription/AttackPerRound", "Ataques por Rodada" },
      { "UI/HeroDescription/AttackSpeed", "Velocidade de Ataque" },
      { "UI/HeroDescription/PickUpRange", "Alcance de Coleta" },
      { "UI/HeroDescription/MovingSpeed", "Velocidade de Movimento" },
      { "UI/HeroDescription/AttackMovingSpeed", "Velocidade de Movimento Atacando" },
      { "UI/HeroDescription/BaseSkill", "Habilidade Base" },
      { "UI/HeroDescription/AdditionalEffect", "Efeito Adicional" },
      { "UI/HeroDescription/ProjectileCount", "Contagem de Projéteis" },
      { "UI/HeroDescription/AEHurt", "Dano EA" },
      { "UI/FightPreparation/TwoPlayersUniqueHero", "Dois jogadores não podem escolher o mesmo herói" },
      { "UI/FightPreparation/BrowserHeroes", "Heróis do Navegador" },
      { "UI/FightPreparation/Back", "Voltar" },
      { "UI/FightPreparation/ApplyGems", "Usar Gemas" },
      { "UI/FightPreparation/ApplyRunes", "Ativar Runas" },
      { "UI/FightPreparation/SelectHero", "Selecionar Herói" },
      { "UI/FightPreparation/Unlock", "Desbloquear" },
      {
        "UI/FightPreparation/AddPlayer",
        "Pressione <color=red>A</color> no gamepad ou Ctrl Direito no teclado para adicionar o 2º jogador"
      },
      { "UI/FightPreparation/RemovePlayer", "Remover" },
      { "UI/PickGemUI/PickGemTitle", "Usar Gemas" },
      { "UI/PickGemUI/OneGemOnePlayer", "Uma gema só pode ser usada por um único jogador" },
      { "UI/PickRuneUI/PickRuneTitle", "Ativar Runas" },
      { "UI/PickRuneUI/Class", "Nível" },
      { "UI/PickRuneUI/InspirationRune", "Runa de Inspiração" },
      { "UI/PickRuneUI/DominationRune", "Runa de Dominação" },
      { "UI/PickRuneUI/ImmortalRune", "Runa Imortal" },
      { "UI/PickRuneUI/Upgrade", "Melhorar" },
      { "UI/PickRuneUI/Price", "Preço" },
      { "UI/PickRuneUI/OneRunePerClass", "Só pode ser escolhida uma runa por nível" },
      { "UI/Pause/PhysicalAttack", "Ataque Físico" },
      { "UI/Pause/IceAttack", "Ataque de Gelo" },
      { "UI/Pause/FireAttack", "Ataque de Fogo" },
      { "UI/Pause/ThunderAttack", "Ataque de Relâmpago" },
      { "UI/Pause/Poisoning", "Envenenamento" },
      { "UI/Pause/MovingSpeed", "Velocidade de Movimento" },
      { "UI/Pause/AttackingSpeed", "Velocidade ao Atacar" },
      { "UI/Pause/AttackSpeed", "Velocidade de Ataque" },
      { "UI/Pause/RecoverSpeed", "Velocidade de Recuperação" },
      { "UI/Pause/ExpBonus", "Bônus de Experiência" },
      { "UI/Pause/RoundsPerSecond", " rodadas/s" },
      { "UI/Pause/Resume", "Retomar" },
      { "UI/Pause/ControlGuide", "Guia de Controles" },
      { "UI/Pause/GiveUp", "Desistir" },
      {
        "UI/Upgrade/DemoUpgradeUnlocked",
        "Melhorias já desbloqueadas na versão demo: 96/188Pode haver ajustes na melhoria na versão oficial"
      },
      { "UI/GameOver/Quit", "Sair" },
      { "UI/FightReady/Difficulty", "Dificuldade" },
      { "UI/FightReady/DifficultyNumber", "Nível.{0}" },
      { "UI/FightReady/UnlockedDifficulty", "Desbloqueado {0}/{1}" },
      { "UI/FightReady/Mode", "Modo" },
      { "UI/FightReady/Map", "Mapa" },
      { "UI/FightReady/Description", "Descrição" },
      { "UI/FightReady/Casual", "Casual" },
      { "UI/FightReady/Standard", "Padrão" },
      { "UI/FightReady/Infinite", "Infinito" },
      { "UI/FightReady/Forest", "Floresta Nebulosa" },
      { "UI/FightReady/Desert", "Deserto Abrasador" },
      { "UI/FightReady/Dungeon", "Masmorra Escura" },
      { "UI/FightReady/Graveyard", "Cemitério da Morte" },
      { "UI/FightReady/Hell", "Inferno Supremo" },
      { "UI/FightReady/ModeDescription/CasualMode", "A dificuldade dos monstros será reduzida no Modo Casual." },
      { "UI/FightReady/ModeDescription/BossAppearTime", "O chefe aparecerá após {0} minutos" },
      {
        "UI/FightReady/AttributeDescription",
        "Taxa de Atributos do Inimigo: Físico - {0}%, Gelo - {1}%, Fogo - {2}%, Relâmpago - {3}%, Veneno - {4}%"
      },
      { "UI/Prompt/UnlockRune", "Desbloquear a runa <b>{0}</b> por {1}, continuar?" },
      { "UI/Prompt/UnlockHero", "Desbloquear o herói <b>{0}</b> por {1}, continuar?" },
      { "UI/Info/Unlock/InsufficientBalance", "Desculpe, saldo insuficiente para desbloquear. (Requisito: {0})" },
      { "UI/Info/Upgrade/InsufficientBalance", "Desculpe, saldo insuficiente para melhorar." },
      { "UI/Info/RunePrerequisitesNotSatisfied", "Por favor, desbloqueie a runa do nível anterior primeiro." },
      { "UI/Audio/Overall", "Geral" },
      { "UI/Audio/Bgm", "BGM" },
      { "UI/Audio/Sfx", "SFX" },
      { "Physical", "Físico" },
      { "Ice", "Gelo" },
      { "Thunder", "Relâmpago" },
      { "Fire", "Fogo" },
      { "Poison", "Veneno" },
      { "Hero/Archangel", "Seraphiel" },
      { "Hero/CaptainG", "Tempestade de Balas" },
      { "Hero/Cutie", "Faye Spark" },
      { "Hero/Gumdam", "Atacante de Cometas" },
      { "Hero/JeanneDArc", "Santa Joana" },
      { "Hero/MountainKing", "Senhor do Trovão" },
      { "Hero/Paladin", "Templário de Ferro" },
      { "Hero/Ranger", "Atacante Venenoso" },
      { "Hero/Witch", "Arcanista" },
      { "Hero/WuKong", "Sun Wukong" },
      { "Hero/ZhaoYun", "Zhao Yun" },
      {
        "UI/Hero/Description/Archangel",
        "Seraphiel atira estalactites com a possibilidade de causar Congelamento. Ela pode recuperar uma certa quantidade de saúde por segundo e é imune a dano do Inferno."
      },
      {
        "UI/Hero/Description/CaptainG",
        "Tempestade de Balas atira 3 balas de fogo com a possibilidade de causar Queimadura. Além disso, ele é imune a Queimadura."
      },
      { "UI/Hero/Description/Cutie", "Faye Spark dispara balas com penetração infinita." },
      {
        "UI/Hero/Description/Gumdam",
        "Atacante de Cometas dispara 2 mísseis de uma bazuca que explode, danificando todos os inimigos próximos. Mas os mísseis podem ter uma zona morta."
      },
      {
        "UI/Hero/Description/JeanneDArc",
        "Santa Joana ataca com sua espada (Ataque Corpo a Corpo), que pode perfurar os inimigos. Além disso, ela não precisa de tempo de recarga."
      },
      {
        "UI/Hero/Description/MountainKing",
        "Senhor do Trovão libera raios que podem causar Atordoamento inimigo, e também é imune a Atordoamento."
      },
      {
        "UI/Hero/Description/Paladin",
        "O Templário de Ferro pode restaurar uma pequena quantidade de saúde por segundo e é imune ao dano do Inferno."
      },
      {
        "UI/Hero/Description/Ranger",
        "Atacante Venenoso dispara 2 flechas venenosas que automaticamente circulam o alvo. Mas as flechas podem ter uma zona morta."
      },
      {
        "UI/Hero/Description/Witch",
        "A Arcanista libera 2 bolas de relâmpago para atacar o inimigo. Ela tem uma opção de melhoria adicional ao subir de nível."
      },
      {
        "UI/Hero/Description/WuKong",
        "Sun Wukong pode se teletransportar para outros lugares (pressione o botão três vezes rapidamente para ativar)."
      },
      {
        "UI/Hero/Description/ZhaoYun",
        "Zhao Yun atira 3 energias de espada gelada em todas as direções para atacar o inimigo. Além disso, ele é imune a Congelamento."
      },
      { "Level/Pause", "Pausado" },
      { "Level/PressEscToResume", "Pressione Esc para continuar" },
      { "Upgrade/RecoverHp/Name", "Recuperar Lv{0}" },
      { "Upgrade/RecoverHp/Description", "Recuperar {0:F2}% de HP" },
      { "Upgrade/IncreaseMaxHp/Name", "HP Máximo Lv{0}" },
      { "Upgrade/IncreaseMaxHp/Description", "Aumentar HP Máximo em {0:F2}%" },
      { "Upgrade/IncreaseMaxHpOnHurt/Name", "Pavor Lv{0}" },
      {
        "Upgrade/IncreaseMaxHpOnHurt/Description",
        "Aumentar HP Máximo em {0:F2}% quando o jogador é ferido. O incremento máximo será de {1:F2}%."
      },
      { "Upgrade/IncreaseDefense/Name", "Escudo Lv{0}" },
      { "Upgrade/IncreaseDefense/Description", "Aumentar a defesa em {0:F2}%" },
      { "Upgrade/IncreaseAllSpeed/Name", "Vá Rápido! Lv{0}" },
      {
        "Upgrade/IncreaseAllSpeed/Description",
        "Aumentar a velocidade em {0:F2}% incluindo velocidade normal e velocidade de ataque."
      },
      { "Upgrade/IncreaseMovingSpeed/Name", "Tênis de Corrida Lv{0}" },
      { "Upgrade/IncreaseMovingSpeed/Description", "Aumentar a velocidade de movimento normal em {0:F2}%." },
      { "Upgrade/IncreaseAttackingMovingSpeed/Name", "Ataque e Fuja Lv{0}" },
      {
        "Upgrade/IncreaseAttackingMovingSpeed/Description",
        "Aumentar a velocidade de movimento durante o ataque em {0:F2}%."
      },
      { "Upgrade/KillToIncreaseSpeed/Name", "Mercenário Lv{0}" },
      {
        "Upgrade/KillToIncreaseSpeed/Description",
        "Mate {0} inimigos para aumentar a velocidade em {1:F2}% e o incremento máximo será de {2:F2}%. O incremento de velocidade será resetado após ser ferido."
      },
      { "Upgrade/ExpBonus/Name", "Nerd Lv{0}" },
      { "Upgrade/ExpBonus/Description", "Aumenta a experiência ganha a cada vez em {0:F2}%." },
      { "Upgrade/IncreasePickUp/Name", "Indiana Jones Lv{0}" },
      { "Upgrade/IncreasePickUp/Description", "Aumentar o raio de coleta em {0:F2}%." },
      { "Upgrade/IncreaseShootingRange/Name", "Barrett Lv{0}" },
      { "Upgrade/IncreaseShootingRange/Description", "Aumentar o alcance de tiro em {0:F2}%." },
      { "Upgrade/AddProjectile/Name", "Escopeta Lv{0}" },
      {
        "Upgrade/AddProjectile/Description",
        "Adicionar {0} projéteis adicionais. No entanto, a dispersão aumentará ligeiramente."
      },
      { "Upgrade/IncreaseDispersion/Name", "Dispersão Lv{0}" },
      { "Upgrade/IncreaseDispersion/Description", "Aumentar a dispersão em {0:F2}%." },
      { "Upgrade/ReduceDispersion/Name", "Rifle de Precisão Lv{0}" },
      { "Upgrade/ReduceDispersion/Description", "Reduzir a dispersão em {0:F2}%." },
      { "Upgrade/IncreaseFiringRate/Name", "Fogo Rápido Lv{0}" },
      { "Upgrade/IncreaseFiringRate/Description", "Aumenta a taxa de disparo em {0:F2}%" },
      { "Upgrade/BurstFire/Name", "Rajada de Fogo Nível {0}" },
      { "Upgrade/BurstFire/Description", "Aumenta a taxa de disparo em {0:F2}% por {1:F2} segundos após sofrer dano." },
      { "Upgrade/IncreaseMagazineSize/Name", "Carregador de Tambor Nível {0}" },
      { "Upgrade/IncreaseMagazineSize/Description", "Aumenta o tamanho do carregador em {0}." },
      { "Upgrade/ReduceReloadingTime/Name", "Carregador Rápido Nível {0}" },
      { "Upgrade/ReduceReloadingTime/Description", "Reduz o tempo de recarga em {0:F2}%" },
      { "Upgrade/BurstReload/Name", "Carregador do Medo Nível {0}" },
      {
        "Upgrade/BurstReload/Description", "Reduz o tempo de recarga em {0:F2}% por {1:F2} segundos após sofrer dano."
      },
      { "Upgrade/IncreaseHurtPercentage/Name", "Afiação Nível {0}" },
      { "Upgrade/IncreaseHurtPercentage/Description", "Aumenta o dano da habilidade básica em {0:F2}%" },
      { "Upgrade/IncreaseIceAeDuration/Name", "Gelado Nível {0}" },
      { "Upgrade/IncreaseIceAeDuration/Description", "Aumenta a duração do efeito Congelado em {0:F2}%" },
      { "Upgrade/IncreaseFireAeDuration/Name", "Queimar Nível {0}" },
      { "Upgrade/IncreaseFireAeDuration/Description", "Aumenta a duração do efeito Queimadura em {0:F2}%" },
      { "Upgrade/IncreaseThunderAeDuration/Name", "Tempestade Trovejante Nível {0}" },
      { "Upgrade/IncreaseThunderAeDuration/Description", "Aumenta a duração do efeito Atordoamento em {0:F2}%" },
      { "Upgrade/ActivateDart/Name", "Ninja" },
      { "Upgrade/ActivateDart/Description", "Lança um dardo que circula ao seu redor." },
      { "Upgrade/AddDart/Name", "Sensei Nível {0}" },
      { "Upgrade/AddDart/Description", "Lança {0} dardos adicionais ao seu redor." },
      { "Upgrade/PoisonDart/Name", "Dardo Envenenado Nível {0}" },
      {
        "Upgrade/PoisonDart/Description",
        "Mergulha todos os dardos em veneno, dando a eles {0:F2}% de chance de envenenar o inimigo e causar {1:F2}% de dano por segundo (ineficaz contra chefes)."
      },
      { "Upgrade/ImproveDartHurt/Name", "Afiado Nível {0}" },
      { "Upgrade/ImproveDartHurt/Description", "Afia todos os dardos ao seu redor, melhorando {0:F2}% dos danos." },
      { "Upgrade/ActivateBoomerang/Name", "Aborígenes" },
      { "Upgrade/ActivateBoomerang/Description", "Lança um bumerangue que circula ao seu redor." },
      { "Upgrade/AddBoomerang/Name", "Manuel Schütz Nível {0}" },
      { "Upgrade/AddBoomerang/Description", "Lança {0} bumerangues adicionais ao seu redor." },
      { "Upgrade/BurnBoomerang/Name", "Bumerangue de Fogo Nível {0}" },
      {
        "Upgrade/BurnBoomerang/Description",
        "Dá ao bumerangue {0:F2}% de chance de incendiar o inimigo por {1:F2} segundos, causando {2:F2}% de dano por segundo."
      },
      { "Upgrade/IncreaseBoomerangSpeed/Name", "Aerodinâmica Nível {0}" },
      { "Upgrade/IncreaseBoomerangSpeed/Description", "Otimiza a aerodinâmica para acelerar o bumerangue em {0:F2}%" },
      { "Upgrade/ImproveBoomerangHurt/Name", "Recheio de Chumbo Nível {0}" },
      {
        "Upgrade/ImproveBoomerangHurt/Description",
        "Carrega o bumerangue com chumbo, aumentando seu poder de ataque em {0:F2}%"
      },
      { "Upgrade/ActivateIceTower/Name", "Torre Congelada" },
      {
        "Upgrade/ActivateIceTower/Description",
        "Envia uma Torre de Gelo automática ao seu redor que dispara granizo automaticamente."
      },
      { "Upgrade/AddIceTower/Name", "Inverno Nível {0}" },
      { "Upgrade/AddIceTower/Description", "Adiciona {0} Torres de Gelo adicionais." },
      { "Upgrade/IncreaseIceTowerFiringRate/Name", "Skadi Nível {0}" },
      {
        "Upgrade/IncreaseIceTowerFiringRate/Description", "Aumenta a velocidade de ataque da Torre de Gelo em {0:F2}%"
      },
      { "Upgrade/ImproveIceTowerHurt/Name", "Cubo de Gelo Nível {0}" },
      { "Upgrade/ImproveIceTowerHurt/Description", "Melhora o dano da Torre de Gelo em {0:F2}%" },
      { "Upgrade/ActivateFireTower/Name", "Torre de Fogo" },
      {
        "Upgrade/ActivateFireTower/Description",
        "Envia uma Torre de Fogo automática ao seu redor que dispara bolas de fogo automaticamente."
      },
      { "Upgrade/AddFireTower/Name", "Acalorado Nível {0}" },
      { "Upgrade/AddFireTower/Description", "Adiciona {0} Torres de Fogo adicionais." },
      { "Upgrade/IncreaseFireTowerFiringRate/Name", "Apolo Nível {0}" },
      {
        "Upgrade/IncreaseFireTowerFiringRate/Description", "Aumenta a velocidade de ataque da Torre de Fogo em {0:F2}%"
      },
      { "Upgrade/ImproveFireTowerHurt/Name", "Lança-Chamas Nível {0}" },
      { "Upgrade/ImproveFireTowerHurt/Description", "Melhora o dano da Torre de Fogo em {0:F2}%" },
      { "Upgrade/ActivateThunderTower/Name", "Torre de Relâmpagos" },
      {
        "Upgrade/ActivateThunderTower/Description",
        "Envia uma Torre de Relâmpagos automática ao seu redor que dispara raios automaticamente."
      },
      { "Upgrade/AddThunderTower/Name", "Raio Nível {0}" },
      { "Upgrade/AddThunderTower/Description", "Adiciona {0} Torres de Relâmpagos adicionais." },
      { "Upgrade/IncreaseThunderTowerFiringRate/Name", "Thor Nível {0}" },
      {
        "Upgrade/IncreaseThunderTowerFiringRate/Description",
        "Aumenta a velocidade de ataque da Torre de Relâmpagos em {0:F2}%"
      },
      { "Upgrade/ImproveThunderTowerHurt/Name", "Bobina de Tesla" },
      { "Upgrade/ImproveThunderTowerHurt/Description", "Melhora o dano da Torre de Relâmpagos em {0:F2}%" },
      { "Upgrade/ActivateSpiral/Name", "Espiral" },
      {
        "Upgrade/ActivateSpiral/Description",
        "De vez em quando, o herói libera uma espiral com atributos aleatórios para atacar o inimigo."
      },
      { "Upgrade/AddSpiral/Name", "Nautilidae Nível {0}" },
      { "Upgrade/AddSpiral/Description", "Adiciona {0} Espirais adicionais ao seu redor." },
      { "Upgrade/ReduceSpiralInterval/Name", "Tempestade Nível {0}" },
      { "Upgrade/ReduceSpiralInterval/Description", "Reduz o intervalo da Espiral em {0:F2}%" },
      { "Upgrade/IncreaseSpiralHurt/Name", "Tempestade Trovejante Nível {0}" },
      { "Upgrade/IncreaseSpiralHurt/Description", "Aumenta o dano da Espiral em {0:F2}%" },
      { "Upgrade/ActivatePuppet/Name", "Lutador de Marionetes" },
      { "Upgrade/ActivatePuppet/Description", "Invoca {0} marionetes a cada {1:F2} segundos." },
      { "Upgrade/LevelUpPuppetToLv1/Name", "Marionete Armada" },
      {
        "Upgrade/LevelUpPuppetToLv1/Description",
        "Invoca {0} marionetes de nível superior a cada {1:F2} segundos. No entanto, as melhorias das marionetes atuais são redefinidas."
      },
      { "Upgrade/LevelUpPuppetToLv2/Name", "Marionete Saiya" },
      {
        "Upgrade/LevelUpPuppetToLv2/Description",
        "Invoca {0} marionetes supremas a cada {1:F2} segundos. No entanto, as melhorias das marionetes atuais são redefinidas."
      },
      { "Upgrade/ReducePuppetIntervalLv0/Name", "Equipe Nível {0}" },
      { "Upgrade/ReducePuppetIntervalLv0/Description", "Reduz o intervalo de invocação de marionetes em {0:F2}%" },
      { "Upgrade/AddPuppetLv0/Name", "Grito Nível {0}" },
      { "Upgrade/AddPuppetLv0/Description", "Invoca {0} marionetes adicionais para combates em grupo." },
      { "Upgrade/IncreasePuppetHurtLv0/Name", "Faca Nível {0}" },
      { "Upgrade/IncreasePuppetHurtLv0/Description", "Aumenta o dano de todas as marionetes em {0:F2}%" },
      { "Upgrade/ReducePuppetIntervalLv1/Name", "Pager Nível {0}" },
      { "Upgrade/ReducePuppetIntervalLv1/Description", "Reduz o intervalo de invocação de marionetes em {0:F2}%" },
      { "Upgrade/AddPuppetLv1/Name", "Máfia Nível {0}" },
      {
        "Upgrade/AddPuppetLv1/Description",
        "Invoca {0} marionetes adicionais para combates em grupo."
      },
      { "Upgrade/IncreasePuppetHurtLv1/Name", "Pistola Nível {0}" },
      { "Upgrade/IncreasePuppetHurtLv1/Description", "Aumenta o dano de todas as marionetes em {0:F2}%" },
      { "Upgrade/ReducePuppetIntervalLv2/Name", "Smartphone Nível {0}" },
      { "Upgrade/ReducePuppetIntervalLv2/Description", "Reduz o intervalo de invocação de marionetes em {0:F2}%" },
      { "Upgrade/AddPuppetLv2/Name", "Exército Nível {0}" },
      {
        "Upgrade/AddPuppetLv2/Description",
        "Invoca {0} marionetes adicionais para combates em grupo."
      },
      { "Upgrade/IncreasePuppetHurtLv2/Name", "Rifle Nível {0}" },
      { "Upgrade/IncreasePuppetHurtLv2/Description", "Aumenta o dano de todas as marionetes em {0:F2}%" },
      { "Upgrade/ActivateFlyingSword/Name", "Espada Voadora" },
      {
        "Upgrade/ActivateFlyingSword/Description",
        "Usa Qi para controlar espadas, liberando {0} espadas a cada {1:F2} segundos."
      },
      { "Upgrade/LevelUpFlyingSwordToLv1/Name", "Espada Pesada" },
      {
        "Upgrade/LevelUpFlyingSwordToLv1/Description",
        "Usa espadas mais pesadas, liberando {0} espadas a cada {1:F2} segundos. No entanto, as melhorias das espadas serão redefinidas."
      },
      { "Upgrade/LevelUpFlyingSwordToLv2/Name", "Espada Afiada" },
      {
        "Upgrade/LevelUpFlyingSwordToLv2/Description",
        "Usa espadas afiadas, liberando {0} espadas a cada {1:F2} segundos. No entanto, as melhorias das espadas serão redefinidas."
      },
      { "Upgrade/AddFlyingSwordLv0/Name", "Júnior Nível {0}" },
      { "Upgrade/AddFlyingSwordLv0/Description", "Adiciona {0} espadas voadoras adicionais." },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Name", "Aprendiz Nível {0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv0/Description", "Reduzir o intervalo de lançamento das espadas em {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Name", "Entrada do Espadachim Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Description", "Aumentar o dano das espadas em {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv1/Name", "Sênior Lv{0}" },
      { "Upgrade/AddFlyingSwordLv1/Description", "Adicionar {0} espadas voadoras extras." },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Name", "Artesão Lv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv1/Description", "Reduzir o intervalo de lançamento das espadas em {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Name", "Sênior do Espadachim Lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Description", "Aumentar o dano das espadas em {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv2/Name", "Shifu Lv{0}" },
      { "Upgrade/AddFlyingSwordLv2/Description", "Adicionar {0} espadas voadoras extras." },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Name", "Caçador de Sóis Lv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv2/Description", "Reduzir o intervalo de lançamento das espadas em {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Name", "Zongshi lv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Description", "Aumentar o dano das espadas em {0:F2}%." },
      { "Upgrade/ActivateMine/Name", "Mina Terrestre" },
      {
        "Upgrade/ActivateMine/Description",
        "Plantar {0} minas que explodem e causam {1:F2} de dano dentro de um raio de {2:F2} metros a cada {3:F2} segundos. Além disso, as minas acionam a explosão das minas próximas."
      },
      { "Upgrade/LevelUpMineToLv1/Name", "Mina de Trovoada" },
      {
        "Upgrade/LevelUpMineToLv1/Description",
        "Plantar {0} minas aprimoradas que explodem e causam {1:F2} de dano dentro de um raio de {2:F2} metros a cada {3:F2} segundos."
      },
      { "Upgrade/LevelUpMineToLv2/Name", "Mina Claymore" },
      {
        "Upgrade/LevelUpMineToLv2/Description",
        "Plantar {0} minas extremamente letais que explodem e causam {1:F2} de dano dentro de um raio de {2:F2} metros a cada {3:F2} segundos."
      },
      { "Upgrade/AddMineLv0/Name", "Escoteiro Lv{0}" },
      { "Upgrade/AddMineLv0/Description", "Plantar {0} minas terrestres extras a cada vez." },
      { "Upgrade/ReduceMineIntervalLv0/Name", "Alicate Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv0/Description", "Reduz o intervalo entre as minas em {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv0/Name", "Arranhão Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv0/Description", "Aumentar o dano das minas terrestres em {0:F2}%." },
      { "Upgrade/AddMineLv1/Name", "Minuteman Lv{0}" },
      { "Upgrade/AddMineLv1/Description", "Plantar {0} minas de trovoada extras a cada vez." },
      { "Upgrade/ReduceMineIntervalLv1/Name", "Caixa de Ferramentas Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv1/Description", "Reduzir o intervalo entre as minas em {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv1/Name", "Bola de Aço Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv1/Description", "Aumentar o dano das minas de trovoada em {0:F2}%." },
      { "Upgrade/AddMineLv2/Name", "Forças Especiais Lv{0}" },
      { "Upgrade/AddMineLv2/Description", "Plantar {0} minas de trovoada extras a cada vez." },
      { "Upgrade/ReduceMineIntervalLv2/Name", "Sistema de Minas Lv{0}" },
      { "Upgrade/ReduceMineIntervalLv2/Description", "Reduzir o intervalo entre as minas em {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv2/Name", "Fragmentação Lv{0}" },
      { "Upgrade/IncreaseMineHurtLv2/Description", "Aumentar o dano das minas Claymore em {0:F2}%." },
      { "MapIndicator/Forest", "Floresta Nebulosa" },
      { "MapIndicator/Desert", "Deserto Abrasador" },
      { "MapIndicator/Dungeon", "Masmorra Escura" },
      { "MapIndicator/Graveyard", "Cemitério da Morte" },
      { "MapIndicator/Hell", "Inferno Supremo" },
      {
        "MapDescription/Forest",
        "Uma floresta nebulosa, onde balas muitas vezes disparam do nada nas árvores aparentemente pacíficas."
      },
      {
        "MapDescription/Desert",
        "Parece que muitos monstros que podem sobreviver no deserto hostil são imunes a atributos."
      },
      { "MapDescription/Dungeon", "Na masmorra, muitas vezes há o uivo de lobos e o som de martelos balançando." },
      {
        "MapDescription/Graveyard",
        "Cuidado com os crânios elusivos no cemitério! Você pode se machucar se esbarrar neles!"
      },
      {
        "MapDescription/Hell",
        "O inferno é encantado pelo diabo, causando mais ou menos dano aos heróis a cada 10 segundos! Mas parece que alguns heróis não se importam com isso."
      },
      { "AdditionalEffect/Type/Freeze", "Congelar" },
      { "AdditionalEffect/Type/Stun", "Atordoar" },
      { "AdditionalEffect/Type/Burn", "Queimar" },
      { "AdditionalEffect/Type/Poison", "Envenenar" },
      { "Gem/GemSynthesis", "Síntese de Gemas" },
      { "Gem/Rarity/R", "Raro" },
      { "Gem/Rarity/SR", "Super Raro" },
      { "Gem/Rarity/SSR", "Super Super Raro" },
      { "Gem/Rarity/XR", "Extremamente Raro" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByPercentage", "Velocidade de Movimento Atacando {0}{1}%" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByValue", "Velocidade de Movimento Atacando {0}{1}" },
      { "Gem/Upgrade/AugmentMovingSpeedByPercentage", "Velocidade de Movimento {0}{1}%" },
      { "Gem/Upgrade/AugmentMovingSpeedByValue", "Velocidade de Movimento {0}{1}" },
      { "Gem/Upgrade/CoolingCountdownDecrementByPercentage", "Tempo de Recarga da Habilidade Básica {0}{1}%" },
      { "Gem/Upgrade/ReloadCountdownDecrementByPercentage", "Tempo de Recarga da Habilidade Básica {0}{1}%" },
      { "Gem/Upgrade/DispersionDecrementByPercentage", "Dispersão da Habilidade Básica {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByPercentage", "Penetração da Habilidade Básica {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByValue", "Penetração da Habilidade Básica {0}{1}" },
      { "Gem/Upgrade/RangeIncrementByPercentage", "Alcance da Habilidade Básica {0}{1}%" },
      { "Gem/Upgrade/RangeIncrementByValue", "Alcance da Habilidade Básica {0}{1}" },
      { "Gem/Upgrade/RepelForceIncrementByPercentage", "Força de Repulsão da Habilidade Básica {0}{1}%" },
      { "Gem/Upgrade/RepelForceIncrementByValue", "Força de Repulsão da Habilidade Básica {0}{1}" },
      { "Gem/Upgrade/SkillCountIncrementByValue", "Projétil da Habilidade Básica {0}{1}" },
      { "Gem/Upgrade/SpeedIncrementByPercentage", "Velocidade da Habilidade Básica {0}{1}%" },
      { "Gem/Upgrade/SpeedIncrementByValue", "Velocidade da Habilidade Básica {0}{1}" },
      { "Gem/Upgrade/EnableRangeAttack", "Habilitar ataque de alcance para habilidade básica" },
      { "Gem/Upgrade/AugmentDamageRangeByValue", "Alcance de Dano da Habilidade Básica {0}{1}" },
      { "Gem/Upgrade/AugmentDamageRangeByPercentage", "Alcance de Dano da Habilidade Básica {0}{1}%" },
      { "Gem/Upgrade/AddPoisoningAdditionalEffect", "Adicionar Efeito Adicional de Envenenamento" },
      { "Gem/Upgrade/SetPoisoningPossibility", "Definir a Possibilidade de Envenenamento para {0}%" },
      { "Gem/Upgrade/SetPoisoningHurtPercentage", "Definir a Porcentagem de Dano de Envenenamento para {0}%" },
      { "Gem/Upgrade/AugmentPoisoningPossibility", "Possibilidade de Envenenamento da Habilidade {0}{1}%" },
      { "Gem/Upgrade/AugmentPoisoningHurtPercentage", "Porcentagem de Dano de Envenenamento da Habilidade {0}{1}%" },
      {
        "Gem/Upgrade/AddOrConvertBasicAdditionalEffect",
        "Adicionar ou converter Atributo de Efeito Adicional para <b>{0}</b>"
      },
      {
        "Gem/Upgrade/SetBasicAdditionalEffectPossibility",
        "Definir a Possibilidade de Efeito Adicional da Habilidade para {0}%"
      },
      {
        "Gem/Upgrade/AugmentBasicAdditionalEffectPossibility", "Possibilidade de Efeito Adicional da Habilidade {0}{1}%"
      },
      { "Gem/Upgrade/AugmentPhysicalHurtByValue", "Dano Físico {0}{1}" },
      { "Gem/Upgrade/AugmentPhysicalHurtByPercentage", "Dano Físico {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicIceHurtByValue", "Dano Mágico de Gelo {0}{1}" },
      { "Gem/Upgrade/AugmentMagicIceHurtByPercentage", "Dano Mágico de Gelo {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicFireHurtByValue", "Dano Mágico de Fogo {0}{1}" },
      { "Gem/Upgrade/AugmentMagicFireHurtByPercentage", "Dano Mágico de Fogo {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicThunderByValue", "Dano Mágico de Raio {0}{1}" },
      { "Gem/Upgrade/Augment/MagicThunderByPercentage", "Dano Mágico de Raio {0}{1}%" },
      { "Gem/Upgrade/Augment/Resurrection", "Contagem de Ressurreição {0}{1}" },
      { "Gem/Upgrade/Augment/IncreaseMaxHp", "HP Máximo {0}{1}" },
      {
        "Rune/Description/ClonedProjectile",
        "Aumenta o número de habilidades básicas. Cada melhoria adiciona um projétil."
      },
      {
        "Rune/Description/ExpBonus",
        "Aumenta a experiência ganha pelos heróis em 10%. A experiência ganha aumenta em 7% cada vez que o herói sobe de nível."
      },
      {
        "Rune/Description/Fission",
        "Depois que as habilidades básicas do herói atingem o inimigo, há uma certa chance de se dividir. A probabilidade de divisão inicial é de 10% e aumenta em 2% a cada vez que é aprimorada."
      },
      {
        "Rune/Description/HailStrike",
        "habilidades ativas. Quando acionada, invoca granizo para atacar inimigos dentro do alcance visível. A habilidade dura 7 segundos e aumenta em 2 segundos a cada melhoria."
      },
      {
        "Rune/Description/HolyShield",
        "habilidades ativas. Após o acionamento, o herói ganha 10 segundos de tempo de invencibilidade, e cada melhoria aumenta em 2 segundos."
      },
      {
        "Rune/Description/IncreaseHonor",
        "No final do jogo, o valor de honra ganho pelo jogador aumenta. O aumento inicial é de 10%, e cada melhoria aumenta em 10%."
      },
      {
        "Rune/Description/InstantKill",
        "Quando as habilidades básicas do herói atingem o inimigo, há uma certa probabilidade de que o inimigo morra instantaneamente. A probabilidade inicial é de 1%, e cada melhoria aumenta em 0,5%."
      },
      {
        "Rune/Description/InstantReload",
        "Há uma chance de 10% de recarregar instantaneamente quando a munição acaba. Cada melhoria aumenta a chance em 5%."
      },
      {
        "Rune/Description/KillAndRecover",
        "Habilidades ativas. Toda vez que o herói mata um inimigo dentro de 10 segundos, o herói pode recuperar 1% de sua saúde. A duração de cada melhoria é aumentada em 2 segundos."
      },
      {
        "Rune/Description/IncreaseImmortalTime",
        "Aumenta o tempo de invulnerabilidade após sofrer dano. O estado inicial aumenta em 0,25 segundos, e cada melhoria aumenta em 0,15 segundos."
      },
      {
        "Rune/Description/IncreaseMaxHp", "Aumenta a saúde máxima do herói em 10%, e aumenta em 5% para cada melhoria."
      },
      {
        "Rune/Description/MeteoriteStrike",
        "Habilidades ativas. Invoca meteoritos para atacar inimigos dentro do alcance visível por 10 segundos. Inicialmente, 10 meteoritos caem por segundo, e cada melhoria aumenta em 5."
      },
      {
        "Rune/Description/PickUpDistance",
        "A distância de coleta do herói aumenta em 10%, e cada melhoria aumenta em 10%."
      },
      {
        "Rune/Description/Poisonous",
        "Habilidades ativas. Causa dano de veneno a inimigos dentro do alcance visível a cada segundo por 10 segundos. Cada nível aumenta a duração em 2 segundos."
      },
      {
        "Rune/Description/PushAway",
        "Empurra inimigos próximos sempre que o herói fica sem munição (10 segundos de tempo de recarga). Cada melhoria aumenta o impulso em 20%."
      },
      {
        "Rune/Description/HpRecovery",
        "Recupera 0,2% do HP do herói por segundo, e aumenta a quantidade de recuperação em 0,2% por nível."
      },
      {
        "Rune/Description/ReducedInjuery",
        "O dano recebido pelo herói é reduzido em 5%, e um adicional de 5% é reduzido para cada nível."
      },
      {
        "Rune/Description/Resurrection",
        "Quando um herói morre, ele é instantaneamente revivido com 25% de sua saúde restaurada. Após a ressurreição, o HP aumenta em 15% cada vez que é aprimorado."
      },
      {
        "Rune/Description/ThunderStrike",
        "Habilidades ativas. Invoca raios para atacar inimigos dentro do alcance visível com precisão. A habilidade dura 10 segundos. Inicialmente, ataca 10% dos inimigos por segundo, cada melhoria atacará um adicional de 3% de inimigos."
      },
      {
        "Rune/Description/TimeStop",
        "Pausa todas as ações inimigas por 5 segundos. Cada melhoria aumenta a duração da pausa em 2 segundos."
      },
      { "Rune/Title/ClonedProjectile", "Projétil Clonado" },
      { "Rune/Title/ExpBonus", "Livro de Experiência" },
      { "Rune/Title/Fission", "Fissão" },
      { "Rune/Title/HailStrike", "Granizo" },
      { "Rune/Title/HolyShield", "Escudo Sagrado" },
      { "Rune/Title/IncreaseHonor", "Campeão" },
      { "Rune/Title/InstantKill", "Um Tiro, Uma Morte" },
      { "Rune/Title/InstantReload", "Multi Carregador" },
      { "Rune/Title/KillAndRecover", "Sedento de Sangue" },
      { "Rune/Title/IncreaseImmortalTime", "Elmo de Cavaleiro" },
      { "Rune/Title/IncreaseMaxHp", "Braço Forte" },
      { "Rune/Title/MeteoriteStrike", "Meteorito" },
      { "Rune/Title/PickUpDistance", "Caçador de Recompensas" },
      { "Rune/Title/Poisonous", "Zona de Gás" },
      { "Rune/Title/PushAway", "Deixe em Paz" },
      { "Rune/Title/HpRecovery", "Cruz Vermelha" },
      { "Rune/Title/ReducedInjuery", "Cavaleiro Blindado" },
      { "Rune/Title/Resurrection", "Milagre" },
      { "Rune/Title/ThunderStrike", "Ataque de Trovão" },
      { "Rune/Title/TimeStop", "Máquina do Tempo" },
      { "Exception/CorruptedSaveFile", "O arquivo salvo está corrompido e foi feito backup em {0}." },
    };
  }
}