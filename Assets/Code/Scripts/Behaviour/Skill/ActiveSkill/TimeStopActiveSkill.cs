using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill.ActiveSkill
{
  public class TimeStopActiveSkill : PlayerActiveSkill
  {
    private readonly ObjectPool objectPool = ObjectPool.Instance;
    private readonly AudioClip timeStopBegin = Resources.Load<AudioClip>("Audio/Sound/Event/timestop-begin");
    private readonly AudioClip timeStopEnd = Resources.Load<AudioClip>("Audio/Sound/Event/timestop-end");

    public override void Launch(int playerNumber)
    {
      storeManager.Commit(StoreNames.GameStateStore, StoreActions.GameStateStore_SET_GAME_STATE, new GameStateData()
      {
        GameState = GameStates.ONLY_PLAYER_CAN_MOVE,
      });

      var playerPositionState = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore);
      var centralPlayerPosition = playerPositionState.CenterPosition;
      AudioWrapper.PlayClip(timeStopBegin, centralPlayerPosition);

      var enemies = storeManager.GetState<LevelState>(StoreNames.LevelStore).Enemies;
      foreach (var enemy in enemies)
      {
        var timeStopAnimation = objectPool.GetObject("Status/TimeStop");
        timeStopAnimation.transform.SetParent(enemy.transform, false);
        timeStopAnimation.transform.localPosition = Vector2.zero;
      }

      var duration = ((PersistActiveSkillConfigurations)ActiveSkillConfigurations).Duration;

      scheduling.SetTimeout(() =>
      {
        var playerPositionState = storeManager.GetState<PlayerPositionState>(StoreNames.PlayerPositionStore);
        var centralPlayerPosition = playerPositionState.CenterPosition;
        AudioWrapper.PlayClip(timeStopEnd, centralPlayerPosition);
      }, duration - 2);

      scheduling.SetTimeout(() =>
      {
        storeManager.Commit(StoreNames.GameStateStore, StoreActions.GameStateStore_SET_GAME_STATE, new GameStateData()
        {
          GameState = GameStates.NORMAL,
        });
      }, duration);
    }
  }
}