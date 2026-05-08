using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.Level;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class AddOrRemovePlayerButton : MonoBehaviour, IPointerClickHandler
  {
    private readonly StoreManager _storeManager = StoreManager.Instance;

    private void Add2NdPlayer()
    {
      _storeManager.Commit(StoreNames.LevelConfigurationStore,
        StoreActions.LevelConfigurationStore_SET_LEVEL_INIT_NUMBER_OF_PLAYERS, new LevelConfigurationData()
        {
          InitialNumberOfPlayers = 2,
        });
      _storeManager.Commit(StoreNames.LevelConfigurationStore,
        StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS, new LevelConfigurationData()
        {
          InitialNumberOfPlayers = 2,
          NumberOfPlayers = 2,
        });
    }

    private void Remove2NdPlayer()
    {
      _storeManager.Commit(StoreNames.LevelConfigurationStore,
        StoreActions.LevelConfigurationStore_SET_LEVEL_INIT_NUMBER_OF_PLAYERS, new LevelConfigurationData()
        {
          InitialNumberOfPlayers = 1,
        });
      _storeManager.Commit(StoreNames.LevelConfigurationStore,
        StoreActions.NumberOfPlayersRelated_SET_NUMBER_OF_PLAYERS, new LevelConfigurationData()
        {
          InitialNumberOfPlayers = 1,
          NumberOfPlayers = 1,
        });
    }

    private void HandleAction()
    {
      var playerCount = _storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore)
        .InitialNumberOfPlayers;
      if (playerCount == 1)
      {
        Add2NdPlayer();
      }
      else
      {
        Remove2NdPlayer();
      }
    }

    private void Update()
    {
      var select = InputUtils.GetButton(1, InputButtons.CONFIRM);
      if (select)
      {
        
      }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      HandleAction();
    }
  }
}