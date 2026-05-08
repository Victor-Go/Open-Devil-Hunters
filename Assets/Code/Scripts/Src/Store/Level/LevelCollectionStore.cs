using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;

namespace Code.Scripts.Src.Store.Level
{
  public struct LevelCollectionState : IState
  {
    public int NumberOfPlayers { get; set; }
    public bool[] PlayerAlive { get; set; }
    public List<GemProperties>[] PlayersCollectedGems { get; set; }
  }

  public struct LevelCollectionData : IActionData
  {
    public int NumberOfPlayers { get; set; }
    public int PlayerNumber { get; set; }
    public bool PlayerAlive { get; set; }
    public GemProperties GemProperties { get; set; }
  }

  public class LevelCollectionStore : IStore
  {
    public IState InitialState => new LevelCollectionState();

    public IState Commit(IState state, StoreActions action, IActionData data)
    {
      var levelCollectionState = (LevelCollectionState)state;
      var levelCollectionData = (LevelCollectionData)data;

      switch (action)
      {
        case StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS:
          int numberOfPlayers = levelCollectionData.NumberOfPlayers;
          levelCollectionState.NumberOfPlayers = numberOfPlayers;
          levelCollectionState.PlayerAlive ??= new int[GeneralConfigurations.MaximumPlayers]
            .Select((_, index) => index < numberOfPlayers).ToArray();

          if (levelCollectionState.PlayersCollectedGems == null)
          {
            var collectedGems = new List<GemProperties>[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
            {
              collectedGems[i] = new();
            }

            levelCollectionState.PlayersCollectedGems = collectedGems;
          }

          return levelCollectionState;
        case StoreActions.NumberOfPlayersRelated_SET_PLAYER_ALIVE:
          levelCollectionState.PlayerAlive[levelCollectionData.PlayerNumber] = levelCollectionData.PlayerAlive;
          levelCollectionState.NumberOfPlayers = levelCollectionState.PlayerAlive.Count(p => p);
          return levelCollectionState;
        case StoreActions.LevelCollectionStore_ADD_GEM:
          levelCollectionState
            .PlayersCollectedGems[levelCollectionData.PlayerNumber]
            .Add(levelCollectionData.GemProperties);
          return levelCollectionState;
        case StoreActions.LevelCollectionStore_CLEAR:
          return new LevelCollectionState();
        default:
          throw new InvalidStoreActionException(action);
      }
    }
  }
}