using System.Collections.Generic;

namespace Code.Scripts.Src.I18n
{
  public struct fr : ILanguage
  {
    public static readonly Dictionary<string, string> locales = new()
    {
      { "HelloWorld", "Bonjour le monde !" },
      { "Language", "Français" },
      { "NotAvailableInDemo", "En développement" },
      { "Player1", "Joueur 1" },
      { "Player2", "Joueur 2" },
      {
        "Notification/WelcomeNotification",
        "Merci d'avoir acheté notre jeu !Nous savons que notre jeu n'est pas parfait, c'est pourquoi nous avons besoin de vos commentaires !Restez à l'écoute pour des mises à jour continues."
      },
      { "Measure/PerSecond", "/sec" },
      {
        "UI/FightPreparationGuide/Page0/0",
        "Il existe 5 attributs pour les attaques des héros et des ennemis, à savoir physique, glace, feu, tonnerre et poison."
      },
      {
        "UI/FightPreparationGuide/Page0/1",
        "Parmi eux, l'attribut glace restreint l'attribut feu, l'attribut feu restreint l'attribut tonnerre et l'attribut tonnerre restreint l'attribut glace."
      },
      {
        "UI/FightPreparationGuide/Page1/0",
        "L'attribut glace peut déclencher un effet de gel et causer des dégâts numériques;\nL'attribut feu peut déclencher un effet d'embrasement et causer des dégâts proportionnels;\nL'attribut tonnerre peut déclencher un effet d'étourdissement, mais ne causera pas de dégâts supplémentaires.\nTous les attributs ci-dessus seront éliminés après un certain temps.\nL'attribut poison peut déclencher un empoisonnement et continuer à causer des dégâts proportionnels. Mais les boss peuvent souvent atténuer, voire supprimer l'empoisonnement."
      },
      {
        "UI/FightPreparationGuide/Page1/1",
        "Certains ennemis ont une défense d'attribut, une immunité aux effets d'attribut et une rupture de la défense d'attribut. Bien utiliser les attributs qui restreignent l'ennemi peut faciliter la défaite de l'adversaire!"
      },
      {
        "UI/FightReadyGuide/Page0/0",
        "Ici seront affichés les attributs des monstres sur la carte actuelle. Mémorisez ces attributs et adaptez votre propre stratégie pour vaincre les monstres!"
      },
      {
        "UI/FightReadyGuide/Page1/0",
        "Ces deux positions afficheront les boss assis sur la carte. Chaque boss a des compétences et des styles de combat différents. Essayez d'utiliser différentes techniques pour vaincre différents boss!"
      },
      {
        "UI/FightReadyGuide/Page1/1",
        "N'oubliez pas d'explorer plus souvent la carte, où vous pourrez collecter différentes gemmes pour renforcer vos héros.Vous pouvez également utiliser les honneurs que vous obtenez lors des batailles pour débloquer de nouveaux héros et runes pour une meilleure exploration dans différentes cartes!"
      },
      { "UI/ControlGuide/ActiveSkill", "Compétence active" },
      { "UI/ControlGuide/AutoFiring", "Tir automatique" },
      { "UI/ControlGuide/Movement", "Mouvement" },
      { "UI/ControlGuide/Fire!", "Feu !" },
      { "UI/ControlGuide/AutoAiming", "Visée automatique" },
      { "UI/ControlGuide/Aim", "Visée" },
      {
        "UI/ControlGuide/MoveToUseController",
        "Déplacez le joystick pour utiliser la visée à la manette (hors mode de visée automatique)."
      },
      {
        "UI/ControlGuide/MoveToUseMouse",
        "Déplacez la souris pour utiliser la visée à la souris (hors mode de visée automatique)."
      },
      { "UI/Control/AutoAiming", "Visée Automatique" },
      { "UI/Control/MouseAiming", "Visée à la Souris" },
      { "UI/Control/ControllerAiming", "Visée à la Manette" },
      { "UI/Control/AutoFiring", "Tir Automatique" },
      { "UI/Control/ManualFiring", "Tir Manuel" },
      { "UI/Control/UseMouseSelectHero", "Utilisez la souris pour sélectionner un héros" },
      { "UI/Text/LevelUp!", "Niveau supérieur!" },
      { "UI/Button/Choose", "Choisir" },
      { "UI/Start", "Démarrer" },
      { "UI/Languages", "Langues" },
      { "UI/Options", "Options" },
      { "UI/Exit", "Quitter" },
      { "UI/Name", "Nom" },
      { "UI/Description", "Description" },
      { "UI/Properties", "Propriétés" },
      { "UI/Properties/AttacksPerRound", "Attaques par round" },
      { "UI/Properties/RateOfFire", "Cadence de tir" },
      { "UI/Properties/ReloadTime", "Temps de rechargement" },
      { "UI/Properties/Projectiles", "Projectiles" },
      { "UI/Properties/FreezeDamagePerSecond", "Dégâts par seconde" },
      { "UI/Properties/MovingSpeed", "Vitesse de déplacement" },
      { "UI/Properties/Physical", "Physique" },
      { "UI/Properties/Ice", "Glace" },
      { "UI/Properties/Fire", "Feu" },
      { "UI/Properties/Thunder", "Tonnerre" },
      { "UI/Properties/Poisoning", "Empoisonnement" },
      { "UI/GameOver", "Game Over" },
      { "UI/FinalScore", "Score final" },
      { "UI/HonorGained", "Honneur gagné" },
      { "UI/PlayerHonor", "Honneur du joueur" },
      { "UI/PlayerScore", "Score du joueur" },
      { "UI/EnemyKilled", "Ennemi tué" },
      { "UI/ExperienceGained", "Expérience gagnée" },
      { "UI/FinalLevel", "Niveau final" },
      { "UI/SurvivalTime", "Temps de survie" },
      { "UI/Upgrades", "Améliorations" },
      { "UI/GameOver/Failed", "DÉFAITE !" },
      { "UI/GameOver/Success", "NIVEAU TERMINÉ !" },
      { "UI/GameOver/Aborted", "MISSION AVORTÉE" },
      { "UI/Achievements", "Succès" },
      { "UI/Furnace", "Forge" },
      { "UI/Gems", "Gemmes" },
      { "UI/Runes", "Runes" },
      { "UI/Difficulty", "Difficulté" },
      { "UI/Mode", "Mode de jeu" },
      { "UI/CasualMode", "Mode occasionnel" },
      { "UI/ChallengeMode", "Mode défi" },
      { "UI/InfiniteMode", "Mode infini" },
      { "UI/Play", "Jouer" },
      { "UI/Confirm", "Confirmer" },
      { "UI/Cancel", "Annuler" },
      { "UI/GemList", "Liste des gemmes" },
      { "UI/HeroDescription/MaxHp", "HP max." },
      { "UI/HeroDescription/HpRecover", "Récupération de HP" },
      { "UI/HeroDescription/Resurrection", "Résurrection" },
      { "UI/HeroDescription/BasicSkillHurt", "Dégâts de compétence de base" },
      { "UI/HeroDescription/BasicSkillProperty", "Attribut de compétence de base" },
      { "UI/HeroDescription/Possibility", "Possibilité d'EA" },
      { "UI/HeroDescription/AttackRange", "Portée d'attaque" },
      { "UI/HeroDescription/AttackPerRound", "Attaques par round" },
      { "UI/HeroDescription/AttackSpeed", "Vitesse d'attaque" },
      { "UI/HeroDescription/PickUpRange", "Portée de ramassage" },
      { "UI/HeroDescription/MovingSpeed", "Vitesse de déplacement" },
      { "UI/HeroDescription/AttackMovingSpeed", "Déplacement d'attaque S." },
      { "UI/HeroDescription/BaseSkill", "Compétence de base" },
      { "UI/HeroDescription/AdditionalEffect", "Effet supplémentaire" },
      { "UI/HeroDescription/ProjectileCount", "Nombre de projectiles" },
      { "UI/HeroDescription/AEHurt", "Dégâts d'EA" },
      { "UI/FightPreparation/TwoPlayersUniqueHero", "Deux joueurs ne peuvent pas choisir le même héros" },
      { "UI/FightPreparation/BrowserHeroes", "Héros du navigateur" },
      { "UI/FightPreparation/Back", "Retour" },
      { "UI/FightPreparation/ApplyGems", "Porter des gemmes" },
      { "UI/FightPreparation/ApplyRunes", "Activer les runes" },
      { "UI/FightPreparation/SelectHero", "Sélectionner un héros" },
      { "UI/FightPreparation/Unlock", "Débloquer" },
      {
        "UI/FightPreparation/AddPlayer",
        "Appuyez sur <color=red>A</color> sur la manette de jeu ou Ctrl droit sur le clavier pour ajouter le 2ème joueur"
      },
      { "UI/FightPreparation/RemovePlayer", "Supprimer" },
      { "UI/PickGemUI/PickGemTitle", "Porter des gemmes" },
      { "UI/PickGemUI/OneGemOnePlayer", "Une gemme ne peut être utilisée que par un seul joueur" },
      { "UI/PickRuneUI/PickRuneTitle", "Activer les runes" },
      { "UI/PickRuneUI/Class", "Niveau" },
      { "UI/PickRuneUI/InspirationRune", "Rune d'inspiration" },
      { "UI/PickRuneUI/DominationRune", "Rune de domination" },
      { "UI/PickRuneUI/ImmortalRune", "Rune immortelle" },
      { "UI/PickRuneUI/Upgrade", "Améliorer" },
      { "UI/PickRuneUI/Price", "Prix" },
      { "UI/PickRuneUI/OneRunePerClass", "Une seule rune peut être choisie par niveau" },
      { "UI/Pause/PhysicalAttack", "Attaque physique" },
      { "UI/Pause/IceAttack", "Attaque de glace" },
      { "UI/Pause/FireAttack", "Attaque de feu" },
      { "UI/Pause/ThunderAttack", "Attaque de foudre" },
      { "UI/Pause/Poisoning", "Empoisonnement" },
      { "UI/Pause/MovingSpeed", "Vitesse de déplacement" },
      { "UI/Pause/AttackingSpeed", "Vitesse en attaquant" },
      { "UI/Pause/AttackSpeed", "Vitesse d'attaque" },
      { "UI/Pause/RecoverSpeed", "Vitesse de récupération" },
      { "UI/Pause/ExpBonus", "Bonus d'expérience" },
      { "UI/Pause/RoundsPerSecond", " tours/s" },
      { "UI/Pause/Resume", "Reprendre" },
      { "UI/Pause/ControlGuide", "Guide de contrôle" },
      { "UI/Pause/GiveUp", "Abandonner" },
      {
        "UI/Upgrade/DemoUpgradeUnlocked",
        "Améliorations déjà débloquées dans la version démo: 96/188Des ajustements peuvent être apportés à l'amélioration dans la version officielle"
      },
      { "UI/GameOver/Quit", "Quitter" },
      { "UI/FightReady/Difficulty", "Difficulté" },
      { "UI/FightReady/DifficultyNumber", "Niveau.{0}" },
      { "UI/FightReady/UnlockedDifficulty", "Débloqué {0}/{1}" },
      { "UI/FightReady/Mode", "Mode" },
      { "UI/FightReady/Map", "Carte" },
      { "UI/FightReady/Description", "Description" },
      { "UI/FightReady/Casual", "Décontracté" },
      { "UI/FightReady/Standard", "Standard" },
      { "UI/FightReady/Infinite", "Infini" },
      { "UI/FightReady/Forest", "Forêt brumeuse" },
      { "UI/FightReady/Desert", "Désert brûlant" },
      { "UI/FightReady/Dungeon", "Donjon sombre" },
      { "UI/FightReady/Graveyard", "Cimetière de la mort" },
      { "UI/FightReady/Hell", "Enfer ultime" },
      { "UI/FightReady/ModeDescription/CasualMode", "La difficulté des monstres sera réduite en mode décontracté." },
      { "UI/FightReady/ModeDescription/BossAppearTime", "Le boss apparaîtra après {0} minutes" },
      {
        "UI/FightReady/AttributeDescription",
        "Ratio d'attributs de l'ennemi: Physique - {0}%, Glace - {1}%, Feu - {2}%, Foudre - {3}%, Poison - {4}%"
      },
      { "UI/Prompt/UnlockRune", "Débloquer la rune <b>{0}</b> pour {1}, continuer?" },
      { "UI/Prompt/UnlockHero", "Débloquer le héros <b>{0}</b> pour {1}, continuer?" },
      { "UI/Info/Unlock/InsufficientBalance", "Désolé, solde insuffisant pour déverrouiller. (Requis : {0})" },
      { "UI/Info/Upgrade/InsufficientBalance", "Désolé, solde insuffisant pour améliorer." },
      { "UI/Info/RunePrerequisitesNotSatisfied", "Veuillez d'abord débloquer la rune du niveau précédent." },
      { "UI/Audio/Overall", "Global" },
      { "UI/Audio/Bgm", "Musique de fond" },
      { "UI/Audio/Sfx", "SFX" },
      { "Physical", "Physique" },
      { "Ice", "Glace" },
      { "Thunder", "Foudre" },
      { "Fire", "Feu" },
      { "Poison", "Poison" },
      { "Hero/Archangel", "Seraphiel" },
      { "Hero/CaptainG", "Tempête de balles" },
      { "Hero/Cutie", "Faye Spark" },
      { "Hero/Gumdam", "Attaquant de comètes" },
      { "Hero/JeanneDArc", "Sainte Jeanne" },
      { "Hero/MountainKing", "Seigneur du tonnerre" },
      { "Hero/Paladin", "Templier de fer" },
      { "Hero/Ranger", "Vipère" },
      { "Hero/Witch", "Archevêque" },
      { "Hero/WuKong", "Sun Wukong" },
      { "Hero/ZhaoYun", "Zhao Yun" },
      {
        "UI/Hero/Description/Archangel",
        "Seraphiel tire des stalactites avec une possibilité de provoquer un gel. Elle peut récupérer une certaine quantité de santé par seconde et est immunisée contre les dégâts de l'Enfer."
      },
      {
        "UI/Hero/Description/CaptainG",
        "Tempête de balles tire 3 balles de feu avec une possibilité de provoquer une brûlure. De plus, il est immunisé contre les brûlures."
      },
      { "UI/Hero/Description/Cutie", "Faye Spark tire des balles à pénétration infinie." },
      {
        "UI/Hero/Description/Gumdam",
        "L'Attaquant de comètes tire 2 missiles d'un bazooka qui explose, endommageant tous les ennemis proches. Mais les missiles peuvent avoir une zone morte."
      },
      {
        "UI/Hero/Description/JeanneDArc",
        "Sainte Jeanne attaque avec son épée (Attaque de mêlée), qui peut transpercer les ennemis. De plus, elle n'a pas besoin de temps de rechargement."
      },
      {
        "UI/Hero/Description/MountainKing",
        "Le Seigneur du tonnerre libère des éclairs qui peuvent étourdir l'ennemi, et est également immunisé contre l'étourdissement."
      },
      {
        "UI/Hero/Description/Paladin",
        "Le Templier de fer peut restaurer une petite quantité de santé par seconde et est immunisé contre les dégâts de l'Enfer."
      },
      {
        "UI/Hero/Description/Ranger",
        "Vipère tire 2 flèches empoisonnées qui tournent automatiquement autour de la cible. Mais les flèches peuvent avoir une zone morte."
      },
      {
        "UI/Hero/Description/Witch",
        "L'Arcaniste lance 2 boules de foudre pour attaquer l'ennemi. Elle a une option d'amélioration supplémentaire en montant de niveau."
      },
      {
        "UI/Hero/Description/WuKong",
        "Sun Wukong peut se téléporter vers d'autres endroits (appuyez rapidement sur le bouton trois fois pour déclencher)."
      },
      {
        "UI/Hero/Description/ZhaoYun",
        "Zhao Yun tire 3 énergies d'épée glaciale dans toutes les directions pour attaquer l'ennemi. De plus, il est immunisé contre le gel."
      },
      { "Level/Pause", "Pause" },
      { "Level/PressEscToResume", "Appuyez sur Échap pour reprendre" },
      { "Upgrade/RecoverHp/Name", "Récupérer Niv{0}" },
      { "Upgrade/RecoverHp/Description", "Récupérer {0:F2}% des PV" },
      { "Upgrade/IncreaseMaxHp/Name", "PV max Niv{0}" },
      { "Upgrade/IncreaseMaxHp/Description", "Augmenter les PV max de {0:F2}%" },
      { "Upgrade/IncreaseMaxHpOnHurt/Name", "Crainte Niv{0}" },
      {
        "Upgrade/IncreaseMaxHpOnHurt/Description",
        "Augmenter les PV max de {0:F2}% lorsque le joueur est blessé. L'incrément maximal sera de {1:F2}%."
      },
      { "Upgrade/IncreaseDefense/Name", "Bouclier Niv{0}" },
      { "Upgrade/IncreaseDefense/Description", "Augmenter la défense de {0:F2}%" },
      { "Upgrade/IncreaseAllSpeed/Name", "Allez vite! Niv{0}" },
      {
        "Upgrade/IncreaseAllSpeed/Description",
        "Augmenter la vitesse de {0:F2}%, y compris la vitesse normale et la vitesse d'attaque."
      },
      { "Upgrade/IncreaseMovingSpeed/Name", "Chaussure de course Niv{0}" },
      { "Upgrade/IncreaseMovingSpeed/Description", "Augmenter la vitesse de déplacement normale de {0:F2}%." },
      { "Upgrade/IncreaseAttackingMovingSpeed/Name", "Frappe et course Niv{0}" },
      {
        "Upgrade/IncreaseAttackingMovingSpeed/Description",
        "Augmenter la vitesse de déplacement pendant l'attaque de {0:F2}%."
      },
      { "Upgrade/KillToIncreaseSpeed/Name", "Mercenaire Niv{0}" },
      {
        "Upgrade/KillToIncreaseSpeed/Description",
        "Tuer {0} ennemis pour augmenter la vitesse de {1:F2}% et l'incrément maximal sera de {2:F2}%. L'incrément de vitesse sera réinitialisé après avoir été blessé."
      },
      { "Upgrade/ExpBonus/Name", "Intello Niv{0}" },
      { "Upgrade/ExpBonus/Description", "Augmente l'expérience gagnée à chaque fois de {0:F2}%." },
      { "Upgrade/IncreasePickUp/Name", "Indiana Jones Niv{0}" },
      { "Upgrade/IncreasePickUp/Description", "Augmenter le rayon de ramassage de {0:F2}%." },
      { "Upgrade/IncreaseShootingRange/Name", "Barrett Niv{0}" },
      { "Upgrade/IncreaseShootingRange/Description", "Augmenter la portée de tir de {0:F2}%." },
      { "Upgrade/AddProjectile/Name", "Fusil de chasse Niv{0}" },
      {
        "Upgrade/AddProjectile/Description",
        "Ajouter {0} projectiles supplémentaires. Cependant, la dispersion augmentera légèrement."
      },
      { "Upgrade/IncreaseDispersion/Name", "Dispersion Niv{0}" },
      { "Upgrade/IncreaseDispersion/Description", "Augmenter la dispersion de {0:F2}%." },
      { "Upgrade/ReduceDispersion/Name", "Fusil de sniper Niv{0}" },
      { "Upgrade/ReduceDispersion/Description", "Réduire la dispersion de {0:F2}%." },
      { "Upgrade/IncreaseFiringRate/Name", "Tir rapide Niv{0}" },
      { "Upgrade/IncreaseFiringRate/Description", "Augmenter la cadence de tir de {0:F2}%." },
      { "Upgrade/BurstFire/Name", "Rafale de tirs Niv{0}" },
      {
        "Upgrade/BurstFire/Description",
        "Augmente la cadence de tir de {0:F2}% pendant {1:F2} secondes après avoir subi des dégâts."
      },
      { "Upgrade/IncreaseMagazineSize/Name", "Chargeur de tambour Niv{0}" },
      { "Upgrade/IncreaseMagazineSize/Description", "Augmenter la taille du chargeur de {0}." },
      { "Upgrade/ReduceReloadingTime/Name", "Chargeur rapide Niv{0}" },
      { "Upgrade/ReduceReloadingTime/Description", "Réduire le temps de rechargement de {0:F2}%." },
      { "Upgrade/BurstReload/Name", "Chargeur de la peur Niv{0}" },
      {
        "Upgrade/BurstReload/Description",
        "Réduire le temps de rechargement de {0:F2}% pendant {1:F2} secondes après avoir subi des dégâts."
      },
      { "Upgrade/IncreaseHurtPercentage/Name", "Affûtage Niv{0}" },
      { "Upgrade/IncreaseHurtPercentage/Description", "Augmenter les dégâts de la compétence de base de {0:F2}%." },
      { "Upgrade/IncreaseIceAeDuration/Name", "Glacial Niv{0}" },
      { "Upgrade/IncreaseIceAeDuration/Description", "Augmenter la durée de l'effet Gelé de {0:F2}%." },
      { "Upgrade/IncreaseFireAeDuration/Name", "Brûler Niv{0}" },
      { "Upgrade/IncreaseFireAeDuration/Description", "Augmenter la durée de l'effet Brûlure de {0:F2}%." },
      { "Upgrade/IncreaseThunderAeDuration/Name", "Orage Niv{0}" },
      { "Upgrade/IncreaseThunderAeDuration/Description", "Augmenter la durée de l'effet Étourdissement de {0:F2}%." },
      { "Upgrade/ActivateDart/Name", "Ninja" },
      { "Upgrade/ActivateDart/Description", "Lance un dard qui tourne autour de vous." },
      { "Upgrade/AddDart/Name", "Sensei Niv{0}" },
      { "Upgrade/AddDart/Description", "Lance {0} dards de plus autour de vous." },
      { "Upgrade/PoisonDart/Name", "Dard empoisonné Niv{0}" },
      {
        "Upgrade/PoisonDart/Description",
        "Trempez tous les dards dans du poison, leur donnant une chance de {0:F2}% d'empoisonner l'ennemi et d'infliger {1:F2}% de dégâts par seconde (inefficace contre les boss)."
      },
      { "Upgrade/ImproveDartHurt/Name", "Aiguisé Niv{0}" },
      {
        "Upgrade/ImproveDartHurt/Description", "Aiguiser tous les dards autour de vous, améliorant {0:F2}% des dégâts."
      },
      { "Upgrade/ActivateBoomerang/Name", "Aborigènes" },
      { "Upgrade/ActivateBoomerang/Description", "Lance un boomerang qui tourne autour de vous." },
      { "Upgrade/AddBoomerang/Name", "Manuel Schütz Niv{0}" },
      { "Upgrade/AddBoomerang/Description", "Lance {0} boomerangs de plus autour de vous." },
      { "Upgrade/BurnBoomerang/Name", "Boomerang de feu Niv{0}" },
      {
        "Upgrade/BurnBoomerang/Description",
        "Donne au boomerang une chance de {0:F2}% de mettre le feu à l'ennemi pendant {1:F2} secondes, infligeant {2:F2}% de dégâts par seconde."
      },
      { "Upgrade/IncreaseBoomerangSpeed/Name", "Aérodynamisme Niv{0}" },
      {
        "Upgrade/IncreaseBoomerangSpeed/Description",
        "Optimiser l'aérodynamisme pour accélérer le boomerang de {0:F2}%."
      },
      { "Upgrade/ImproveBoomerangHurt/Name", "Remplissage de plomb Niv{0}" },
      {
        "Upgrade/ImproveBoomerangHurt/Description",
        "Charger le boomerang avec du plomb, augmentant sa puissance d'attaque de {0:F2}%"
      },
      { "Upgrade/ActivateIceTower/Name", "Tour de glace" },
      {
        "Upgrade/ActivateIceTower/Description",
        "Faites voler une Tour de glace automatique autour de vous qui tire automatiquement de la grêle."
      },
      { "Upgrade/AddIceTower/Name", "Hiver Niv{0}" },
      { "Upgrade/AddIceTower/Description", "Ajouter {0} Tours de glace supplémentaires." },
      { "Upgrade/IncreaseIceTowerFiringRate/Name", "Skadi Niv{0}" },
      {
        "Upgrade/IncreaseIceTowerFiringRate/Description",
        "Augmenter la vitesse d'attaque de la Tour de glace de {0:F2}%."
      },
      { "Upgrade/ImproveIceTowerHurt/Name", "Cube de glace Niv{0}" },
      { "Upgrade/ImproveIceTowerHurt/Description", "Améliorer les dégâts de la Tour de glace de {0:F2}%." },
      { "Upgrade/ActivateFireTower/Name", "Tour de feu" },
      {
        "Upgrade/ActivateFireTower/Description",
        "Faites voler une Tour de feu automatique autour de vous qui tire automatiquement des boules de feu."
      },
      { "Upgrade/AddFireTower/Name", "Sensuel Niv{0}" },
      { "Upgrade/AddFireTower/Description", "Ajouter {0} Tours de feu supplémentaires." },
      { "Upgrade/IncreaseFireTowerFiringRate/Name", "Apollon Niv{0}" },
      {
        "Upgrade/IncreaseFireTowerFiringRate/Description",
        "Augmenter la vitesse d'attaque de la Tour de feu de {0:F2}%."
      },
      { "Upgrade/ImproveFireTowerHurt/Name", "Lance-flammes Niv{0}" },
      { "Upgrade/ImproveFireTowerHurt/Description", "Améliorer les dégâts de la Tour de feu de {0:F2}%." },
      { "Upgrade/ActivateThunderTower/Name", "Tour de tonnerre" },
      {
        "Upgrade/ActivateThunderTower/Description",
        "Faites voler une Tour de tonnerre automatique autour de vous qui tire automatiquement du tonnerre."
      },
      { "Upgrade/AddThunderTower/Name", "Foudre Niv{0}" },
      { "Upgrade/AddThunderTower/Description", "Ajouter {0} Tours de tonnerre supplémentaires." },
      { "Upgrade/IncreaseThunderTowerFiringRate/Name", "Thor Niv{0}" },
      {
        "Upgrade/IncreaseThunderTowerFiringRate/Description",
        "Augmenter la vitesse d'attaque de la Tour de tonnerre de {0:F2}%."
      },
      { "Upgrade/ImproveThunderTowerHurt/Name", "Bobine Tesla" },
      { "Upgrade/ImproveThunderTowerHurt/Description", "Améliorer les dégâts de la Tour de tonnerre de {0:F2}%." },
      { "Upgrade/ActivateSpiral/Name", "Spirale" },
      {
        "Upgrade/ActivateSpiral/Description",
        "De temps en temps, le héros libère une spirale avec des attributs aléatoires pour attaquer l'ennemi."
      },
      { "Upgrade/AddSpiral/Name", "Nautilidae Niv{0}" },
      { "Upgrade/AddSpiral/Description", "Ajouter {0} spirales de plus autour de vous." },
      { "Upgrade/ReduceSpiralInterval/Name", "Pluie battante Niv{0}" },
      { "Upgrade/ReduceSpiralInterval/Description", "Réduire l'intervalle de la spirale de {0:F2}%." },
      { "Upgrade/IncreaseSpiralHurt/Name", "Orage Niv{0}" },
      { "Upgrade/IncreaseSpiralHurt/Description", "Augmenter les dégâts de la spirale de {0:F2}%." },
      { "Upgrade/ActivatePuppet/Name", "Combattant de marionnettes" },
      { "Upgrade/ActivatePuppet/Description", "Invoquer {0} marionnettes toutes les {1:F2} secondes." },
      { "Upgrade/LevelUpPuppetToLv1/Name", "Marionnette armée" },
      {
        "Upgrade/LevelUpPuppetToLv1/Description",
        "Invoquer {0} marionnettes améliorées toutes les {1:F2} secondes. Cependant, les améliorations des marionnettes actuelles sont réinitialisées."
      },
      { "Upgrade/LevelUpPuppetToLv2/Name", "Marionnette Saiya" },
      {
        "Upgrade/LevelUpPuppetToLv2/Description",
        "Invoquer {0} marionnettes ultimes toutes les {1:F2} secondes. Cependant, les améliorations des marionnettes actuelles sont réinitialisées."
      },
      { "Upgrade/ReducePuppetIntervalLv0/Name", "Équipe Niv{0}" },
      {
        "Upgrade/ReducePuppetIntervalLv0/Description", "Réduire l'intervalle d'invocation de marionnettes de {0:F2}%."
      },
      { "Upgrade/AddPuppetLv0/Name", "Crier Niv{0}" },
      { "Upgrade/AddPuppetLv0/Description", "Invoquer {0} marionnettes supplémentaires pour les combats de groupe." },
      { "Upgrade/IncreasePuppetHurtLv0/Name", "Couteau Niv{0}" },
      { "Upgrade/IncreasePuppetHurtLv0/Description", "Augmenter les dégâts de toutes les marionnettes de {0:F2}%." },
      { "Upgrade/ReducePuppetIntervalLv1/Name", "Bip Niv{0}" },
      {
        "Upgrade/ReducePuppetIntervalLv1/Description", "Réduire l'intervalle d'invocation de marionnettes de {0:F2}%."
      },
      { "Upgrade/AddPuppetLv1/Name", "Mafia Niv{0}" },
      {
        "Upgrade/AddPuppetLv1/Description",
        "Invoquer {0} marionnettes supplémentaires pour les combats de groupe."
      },
      { "Upgrade/IncreasePuppetHurtLv1/Name", "Pistolet Niv{0}" },
      { "Upgrade/IncreasePuppetHurtLv1/Description", "Augmenter les dégâts de toutes les marionnettes de {0:F2}%." },
      { "Upgrade/ReducePuppetIntervalLv2/Name", "Smartphone Niv{0}" },
      {
        "Upgrade/ReducePuppetIntervalLv2/Description", "Réduire l'intervalle d'invocation de marionnettes de {0:F2}%."
      },
      { "Upgrade/AddPuppetLv2/Name", "Armée Niv{0}" },
      {
        "Upgrade/AddPuppetLv2/Description",
        "Invoquer {0} marionnettes supplémentaires pour les combats de groupe."
      },
      { "Upgrade/IncreasePuppetHurtLv2/Name", "Fusil Niv{0}" },
      { "Upgrade/IncreasePuppetHurtLv2/Description", "Augmenter les dégâts de toutes les marionnettes de {0:F2}%." },
      { "Upgrade/ActivateFlyingSword/Name", "Épée volante" },
      {
        "Upgrade/ActivateFlyingSword/Description",
        "Utiliser le Qi pour contrôler les épées, libérant {0} épées toutes les {1:F2} secondes."
      },
      { "Upgrade/LevelUpFlyingSwordToLv1/Name", "Épée lourde" },
      {
        "Upgrade/LevelUpFlyingSwordToLv1/Description",
        "Utiliser des épées plus lourdes, libérant {0} épées toutes les {1:F2} secondes. Cependant, les améliorations des épées seront réinitialisées."
      },
      { "Upgrade/LevelUpFlyingSwordToLv2/Name", "Épée aiguisée" },
      {
        "Upgrade/LevelUpFlyingSwordToLv2/Description",
        "Utiliser des épées aiguisées, libérant {0} épées toutes les {1:F2} secondes. Cependant, les améliorations des épées seront réinitialisées."
      },
      { "Upgrade/AddFlyingSwordLv0/Name", "Junior Niv{0}" },
      { "Upgrade/AddFlyingSwordLv0/Description", "Ajouter {0} épées volantes supplémentaires." },
      { "Upgrade/ReduceFlyingSwordIntervalLv0/Name", "Apprenti Niv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv0/Description", "Réduire l'intervalle de libération des épées de {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Name", "Entrée du bretteur Niv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv0/Description", "Augmenter les dégâts des épées de {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv1/Name", "Senior Niv{0}" },
      { "Upgrade/AddFlyingSwordLv1/Description", "Ajouter {0} épées volantes supplémentaires." },
      { "Upgrade/ReduceFlyingSwordIntervalLv1/Name", "Artisan Niv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv1/Description", "Réduire l'intervalle de libération des épées de {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Name", "Senior du bretteur Niv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv1/Description", "Augmenter les dégâts des épées de {0:F2}%." },
      { "Upgrade/AddFlyingSwordLv2/Name", "Shifu Niv{0}" },
      { "Upgrade/AddFlyingSwordLv2/Description", "Ajouter {0} épées volantes supplémentaires." },
      { "Upgrade/ReduceFlyingSwordIntervalLv2/Name", "Chasseur de soleil Niv{0}" },
      {
        "Upgrade/ReduceFlyingSwordIntervalLv2/Description", "Réduire l'intervalle de libération des épées de {0:F2}%."
      },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Name", "Zongshi Niv{0}" },
      { "Upgrade/IncreaseFlyingSwordHurtLv2/Description", "Augmenter les dégâts des épées de {0:F2}%." },
      { "Upgrade/ActivateMine/Name", "Mine terrestre" },
      {
        "Upgrade/ActivateMine/Description",
        "Plante {0} mines qui explosent et causent {1:F2} dégâts dans un rayon de {2:F2} mètres toutes les {3:F2} secondes. De plus, les mines déclenchent l'explosion des mines à proximité.\n"
      },
      { "Upgrade/LevelUpMineToLv1/Name", "Mine de tonnerre" },
      {
        "Upgrade/LevelUpMineToLv1/Description",
        "Plante {0} mines améliorées qui explosent et causent {1:F2} dégâts dans un rayon de {2:F2} mètres toutes les {3:F2} secondes."
      },
      { "Upgrade/LevelUpMineToLv2/Name", "Mine Claymore" },
      {
        "Upgrade/LevelUpMineToLv2/Description",
        "Plante {0} mines extrêmement mortelles qui explosent et causent {1:F2} dégâts dans un rayon de {2:F2} mètres toutes les {3:F2} secondes."
      },
      { "Upgrade/AddMineLv0/Name", "Boy Scout Niv{0}" },
      { "Upgrade/AddMineLv0/Description", "Plante {0} mines terrestres de plus à chaque fois." },
      { "Upgrade/ReduceMineIntervalLv0/Name", "Pinces Niv{0}" },
      { "Upgrade/ReduceMineIntervalLv0/Description", "Réduit l'intervalle entre les mines de {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv0/Name", "Égratignure Niv{0}" },
      { "Upgrade/IncreaseMineHurtLv0/Description", "Augmenter les dégâts des mines terrestres de {0:F2}%." },
      { "Upgrade/AddMineLv1/Name", "Minutemen Niv{0}" },
      { "Upgrade/AddMineLv1/Description", "Plante {0} mines de tonnerre de plus à chaque fois." },
      { "Upgrade/ReduceMineIntervalLv1/Name", "Boîte à outils Niv{0}" },
      { "Upgrade/ReduceMineIntervalLv1/Description", "Réduire l'intervalle entre les mines de {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv1/Name", "Bille d'acier Niv{0}" },
      { "Upgrade/IncreaseMineHurtLv1/Description", "Augmenter les dégâts des mines de tonnerre de {0:F2}%." },
      { "Upgrade/AddMineLv2/Name", "Forces spéciales Niv{0}" },
      { "Upgrade/AddMineLv2/Description", "Plante {0} mines de tonnerre de plus à chaque fois." },
      { "Upgrade/ReduceMineIntervalLv2/Name", "Système de mines Niv{0}" },
      { "Upgrade/ReduceMineIntervalLv2/Description", "Réduire l'intervalle entre les mines de {0:F2}%." },
      { "Upgrade/IncreaseMineHurtLv2/Name", "Fragmentation Niv{0}" },
      { "Upgrade/IncreaseMineHurtLv2/Description", "Augmenter les dégâts des mines Claymore de {0:F2}%." },
      { "MapIndicator/Forest", "Forêt brumeuse" },
      { "MapIndicator/Desert", "Désert brûlant" },
      { "MapIndicator/Dungeon", "Donjon sombre" },
      { "MapIndicator/Graveyard", "Cimetière de la mort" },
      { "MapIndicator/Hell", "Enfer ultime" },
      {
        "MapDescription/Forest",
        "Une forêt brumeuse, où les balles surgissent souvent de nulle part dans les arbres apparemment paisibles."
      },
      {
        "MapDescription/Desert",
        "Il semble que de nombreux monstres qui peuvent survivre dans le désert aride soient immunisés contre les attributs."
      },
      {
        "MapDescription/Dungeon",
        "Dans le donjon, on entend souvent les hurlements des loups et le bruit des marteaux qui se balancent."
      },
      {
        "MapDescription/Graveyard",
        "Attention aux crânes insaisissables dans le cimetière! Vous pourriez être blessé si vous les rencontrez!"
      },
      {
        "MapDescription/Hell",
        "L'enfer est enchanté par le diable, causant plus ou moins de dégâts aux héros toutes les 10 secondes! Mais il semble que certains héros ne s'en soucient pas du tout."
      },
      { "AdditionalEffect/Type/Freeze", "Geler" },
      { "AdditionalEffect/Type/Stun", "Étourdir" },
      { "AdditionalEffect/Type/Burn", "Brûler" },
      { "AdditionalEffect/Type/Poison", "Poison" },
      { "Gem/GemSynthesis", "Synthèse de gemmes" },
      { "Gem/Rarity/R", "Rare" },
      { "Gem/Rarity/SR", "Super rare" },
      { "Gem/Rarity/SSR", "Super super rare" },
      { "Gem/Rarity/XR", "Extrêmement rare" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByPercentage", "Vitesse de déplacement en attaquant {0}{1}%" },
      { "Gem/Upgrade/AugmentAttackingMovingSpeedByValue", "Vitesse de déplacement en attaquant {0}{1}" },
      { "Gem/Upgrade/AugmentMovingSpeedByPercentage", "Vitesse de déplacement {0}{1}%" },
      { "Gem/Upgrade/AugmentMovingSpeedByValue", "Vitesse de déplacement {0}{1}" },
      { "Gem/Upgrade/CoolingCountdownDecrementByPercentage", "Temps de refroidissement de compétence de base {0}{1}%" },
      { "Gem/Upgrade/ReloadCountdownDecrementByPercentage", "Temps de rechargement de compétence de base {0}{1}%" },
      { "Gem/Upgrade/DispersionDecrementByPercentage", "Dispersion de compétence de base {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByPercentage", "Pénétration de compétence de base {0}{1}%" },
      { "Gem/Upgrade/PenetrationIncrementByValue", "Pénétration de compétence de base {0}{1}" },
      { "Gem/Upgrade/RangeIncrementByPercentage", "Portée de compétence de base {0}{1}%" },
      { "Gem/Upgrade/RangeIncrementByValue", "Portée de compétence de base {0}{1}" },
      { "Gem/Upgrade/RepelForceIncrementByPercentage", "Force de répulsion de compétence de base {0}{1}%" },
      { "Gem/Upgrade/RepelForceIncrementByValue", "Force de répulsion de compétence de base {0}{1}" },
      { "Gem/Upgrade/SkillCountIncrementByValue", "Projectile de compétence de base {0}{1}" },
      { "Gem/Upgrade/SpeedIncrementByPercentage", "Vitesse de compétence de base {0}{1}%" },
      { "Gem/Upgrade/SpeedIncrementByValue", "Vitesse de compétence de base {0}{1}" },
      { "Gem/Upgrade/EnableRangeAttack", "Activer l'attaque à distance pour la compétence de base" },
      { "Gem/Upgrade/AugmentDamageRangeByValue", "Portée des dégâts de compétence de base {0}{1}" },
      { "Gem/Upgrade/AugmentDamageRangeByPercentage", "Portée des dégâts de compétence de base {0}{1}%" },
      { "Gem/Upgrade/AddPoisoningAdditionalEffect", "Ajouter un effet supplémentaire d'empoisonnement" },
      { "Gem/Upgrade/SetPoisoningPossibility", "Définir la possibilité d'empoisonnement à {0}%" },
      { "Gem/Upgrade/SetPoisoningHurtPercentage", "Définir le pourcentage de dégâts d'empoisonnement à {0}%" },
      { "Gem/Upgrade/AugmentPoisoningPossibility", "Possibilité d'empoisonnement de compétence {0}{1}%" },
      { "Gem/Upgrade/AugmentPoisoningHurtPercentage", "Pourcentage de dégâts d'empoisonnement de compétence {0}{1}%" },
      {
        "Gem/Upgrade/AddOrConvertBasicAdditionalEffect",
        "Ajouter ou convertir l'attribut d'effet supplémentaire en <b>{0}</b>"
      },
      {
        "Gem/Upgrade/SetBasicAdditionalEffectPossibility",
        "Définir la possibilité d'effet supplémentaire de compétence à {0}%"
      },
      {
        "Gem/Upgrade/AugmentBasicAdditionalEffectPossibility",
        "Possibilité d'effet supplémentaire de compétence {0}{1}%"
      },
      { "Gem/Upgrade/AugmentPhysicalHurtByValue", "Dégâts physiques {0}{1}" },
      { "Gem/Upgrade/AugmentPhysicalHurtByPercentage", "Dégâts physiques {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicIceHurtByValue", "Dégâts de magie de glace {0}{1}" },
      { "Gem/Upgrade/AugmentMagicIceHurtByPercentage", "Dégâts de magie de glace {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicFireHurtByValue", "Dégâts de magie de feu {0}{1}" },
      { "Gem/Upgrade/AugmentMagicFireHurtByPercentage", "Dégâts de magie de feu {0}{1}%" },
      { "Gem/Upgrade/AugmentMagicThunderByValue", "Dégâts de magie de tonnerre {0}{1}" },
      { "Gem/Upgrade/Augment/MagicThunderByPercentage", "Dégâts de magie de tonnerre {0}{1}%" },
      { "Gem/Upgrade/Augment/Resurrection", "Nombre de résurrections {0}{1}" },
      { "Gem/Upgrade/Augment/IncreaseMaxHp", "PV max {0}{1}" },
      {
        "Rune/Description/ClonedProjectile",
        "Augmenter le nombre de compétences de base. Chaque amélioration ajoute un projectile."
      },
      {
        "Rune/Description/ExpBonus",
        "Augmente l'expérience gagnée par les héros de 10%. L'expérience gagnée augmente de 7% à chaque fois que le héros monte de niveau."
      },
      {
        "Rune/Description/Fission",
        "Après que les compétences de base du héros touchent l'ennemi, il existe une certaine chance de se diviser. La probabilité de division initiale est de 10%, et elle augmente de 2% à chaque fois qu'elle est améliorée."
      },
      {
        "Rune/Description/HailStrike",
        "compétences actives. Lorsqu'elle est déclenchée, invoque des grêlons pour attaquer les ennemis à portée visible. La compétence dure 7 secondes et augmente de 2 secondes à chaque amélioration."
      },
      {
        "Rune/Description/HolyShield",
        "compétences actives. Après le déclenchement, le héros gagne 10 secondes de temps d'invincibilité, et chaque amélioration augmente de 2 secondes."
      },
      {
        "Rune/Description/IncreaseHonor",
        "À la fin de la partie, la valeur d'honneur gagnée par le joueur augmente. L'augmentation initiale est de 10 %, et chaque amélioration l'augmente de 10 %."
      },
      {
        "Rune/Description/InstantKill",
        "Lorsque les compétences de base du héros touchent l'ennemi, il existe une certaine probabilité que l'ennemi meure instantanément. La probabilité initiale est de 1 %, et chaque amélioration l'augmente de 0,5 %."
      },
      {
        "Rune/Description/InstantReload",
        "Il y a 10 % de chances de recharger instantanément lorsque les munitions sont épuisées. Chaque amélioration augmente la chance de 5 %."
      },
      {
        "Rune/Description/KillAndRecover",
        "compétences actives. Chaque fois que le héros tue un ennemi dans les 10 secondes, le héros peut récupérer 1 % de sa santé. La durée de chaque amélioration est augmentée de 2 secondes."
      },
      {
        "Rune/Description/IncreaseImmortalTime",
        "Augmente le temps d'invulnérabilité après avoir subi des dégâts. L'état initial augmente de 0,25 secondes, et chaque amélioration l'augmente de 0,15 secondes."
      },
      {
        "Rune/Description/IncreaseMaxHp",
        "Augmente la santé maximale du héros de 10 %, et augmente de 5 % pour chaque amélioration."
      },
      {
        "Rune/Description/MeteoriteStrike",
        "compétences actives. Invoque des météorites pour attaquer les ennemis à portée visible pendant 10 secondes. Initialement, 10 météorites tombent par seconde, et chaque amélioration augmente de 5."
      },
      {
        "Rune/Description/PickUpDistance",
        "La distance de ramassage du héros augmente de 10 %, et chaque amélioration l'augmente de 10 %."
      },
      {
        "Rune/Description/Poisonous",
        "compétences actives. Inflige des dégâts de poison aux ennemis à portée visible chaque seconde pendant 10 secondes. Chaque niveau augmente la durée de 2 secondes."
      },
      {
        "Rune/Description/PushAway",
        "Repousse les ennemis proches chaque fois que le héros manque de munitions (10 secondes de temps de recharge). Chaque amélioration augmente la poussée de 20 %."
      },
      {
        "Rune/Description/HpRecovery",
        "Récupère 0,2 % des PV du héros par seconde, et augmente la quantité de récupération de 0,2 % par niveau."
      },
      {
        "Rune/Description/ReducedInjuery",
        "Les dégâts subis par le héros sont réduits de 5%, et une réduction supplémentaire de 5% est appliquée pour chaque niveau."
      },
      {
        "Rune/Description/Resurrection",
        "Lorsqu'un héros meurt, il est instantanément ressuscité avec 25 % de sa santé restaurée. Après la résurrection, les PV augmentent de 15 % à chaque amélioration."
      },
      {
        "Rune/Description/ThunderStrike",
        "compétences actives. Invoque la foudre pour attaquer les ennemis à portée visible avec précision. La compétence dure 10 secondes. Initialement, attaque 10 % des ennemis par seconde, chaque amélioration attaquera 3 % d'ennemis supplémentaires."
      },
      {
        "Rune/Description/TimeStop",
        "Met en pause toutes les actions ennemies pendant 5 secondes. Chaque amélioration augmente la durée de la pause de temps de 2 secondes."
      },
      { "Rune/Title/ClonedProjectile", "Projectile cloné" },
      { "Rune/Title/ExpBonus", "Livre d'expérience" },
      { "Rune/Title/Fission", "Fission" },
      { "Rune/Title/HailStrike", "Grêle" },
      { "Rune/Title/HolyShield", "Bouclier sacré" },
      { "Rune/Title/IncreaseHonor", "Champion" },
      { "Rune/Title/InstantKill", "Un coup, un mort" },
      { "Rune/Title/InstantReload", "Multi-chargeur" },
      { "Rune/Title/KillAndRecover", "Soif de sang" },
      { "Rune/Title/IncreaseImmortalTime", "Casque de chevalier" },
      { "Rune/Title/IncreaseMaxHp", "Bras puissant" },
      { "Rune/Title/MeteoriteStrike", "Météorite" },
      { "Rune/Title/PickUpDistance", "Chasseur de primes" },
      { "Rune/Title/Poisonous", "Zone de gaz" },
      { "Rune/Title/PushAway", "Laisser tranquille" },
      { "Rune/Title/HpRecovery", "Croix rouge" },
      { "Rune/Title/ReducedInjuery", "Chevalier blindé" },
      { "Rune/Title/Resurrection", "Miracle" },
      { "Rune/Title/ThunderStrike", "Frappe de tonnerre" },
      { "Rune/Title/TimeStop", "Machine à remonter le temps" },
      { "Exception/CorruptedSaveFile", "Le fichier de sauvegarde est corrompu et a été sauvegardé dans {0}." },
    };
  }
}