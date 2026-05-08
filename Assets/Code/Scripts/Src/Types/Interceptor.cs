using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Src.Types
{
  public struct ObtainExperienceContext
  {
    public float Experience { get; set; }
  }

  public interface IObtainExperienceInterceptor
  {
    public ObtainExperienceContext OnObtainExperience(ObtainExperienceContext obtainExperienceContext);
  }

  public struct PlayerReloadContext
  {
    public float CoolingTime { get; set; }
    public float ReloadTime { get; set; }
    public int AttackPerRound { get; set; }
    public Vector2 PlayerPosition { get; set; }
  }

  public interface IPlayerReloadInterceptor
  {
    public PlayerReloadContext OnPlayerReload(PlayerReloadContext playerReloadContext);
  }

  public struct PlayerHurtContext
  {
    public int PlayerNumber { get; set; }
    public float HitPoint { get; set; }
  }

  public interface IPlayerHurtInterceptor
  {
    public PlayerHurtContext OnPlayerHurt(PlayerHurtContext playerHurtContext);
  }

  public struct PlayerDieContext
  {
    public Vector2 PlayerPosition { get; set; }
    public bool AllowPlayerDie { get; set; }
  }

  public interface IPlayerDieInterceptor
  {
    public PlayerDieContext OnPlayerDie(PlayerDieContext playerDieContext);
  }

  public struct EnemyHurtContext
  {
    public int PlayerNumber { get; set; }
    public float HitPoint { get; set; }
    public Vector2 CurrentPosition { get; set; }
    public bool IsBoss { get; set; }
    public BaseSkillConfigurations SkillConfigurations { get; set; }
  }

  public interface IEnemyHurtInterceptor
  {
    public EnemyHurtContext OnEnemyHurt(EnemyHurtContext enemyHurtContext);
  }

  public struct EnemyDieContext
  {
    public int PlayerNumber { get; set; }
    public Vector2 EnemyPosition { get; set; }
  }

  public interface IEnemyDieInterceptor
  {
    public EnemyDieContext OnEnemyDie(EnemyDieContext enemyDieContext);
  }
}