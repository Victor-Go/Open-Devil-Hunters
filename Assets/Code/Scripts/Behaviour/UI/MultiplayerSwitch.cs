using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts.Behaviour.UI
{
  public class MultiplayerSwitch : MonoBehaviour, IStoreChangedHandler
  {
    public GameObject[] Player0Layers;
    public GameObject[] Player1Layers;
    public GameObject[] MultiplayerLayers;

    private StoreManager storeManager;

    private int numberOfPlayers;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
    }

    private void Start()
    {
      storeManager.Subscribe(StoreNames.LevelConfigurationStore, this);
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);

      numberOfPlayers = levelConfigs.NumberOfPlayers;

      switch (levelConfigs.NumberOfPlayers)
      {
        case 1:
          var playerAlive = levelConfigs.PlayerAlive;
          if (playerAlive[0])
          {
            MakeAppear(Player0Layers);
          }
          else if (playerAlive[1])
          {
            MakeAppear(Player1Layers);
          }

          break;
        case 2:
          MakeAppear(MultiplayerLayers);
          break;
        default:
          throw new System.Exception($"Invalid number of players: {levelConfigs.NumberOfPlayers}");
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:

          var levelConfigs = (LevelConfigurationState)state;
          var newNumberOfPlayers = levelConfigs.NumberOfPlayers;
          if (newNumberOfPlayers < numberOfPlayers)
          {
            numberOfPlayers = newNumberOfPlayers;

            MakeDisappear(MultiplayerLayers);
            var playerAlive = levelConfigs.PlayerAlive;
            if (playerAlive[0])
            {
              MakeAppear(Player0Layers);
            }
            else if (playerAlive[1])
            {
              MakeAppear(Player1Layers);
            }
          }


          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void MakeAppear(GameObject[] gameObjects)
    {
      foreach (var gameObject in gameObjects)
      {
        gameObject.GetComponent<MultiplayerUI>().Appear();
      }
    }

    private void MakeDisappear(GameObject[] gameObjects)
    {
      foreach (var gameObject in gameObjects)
      {
        gameObject.GetComponent<MultiplayerUI>().Disappear();
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}