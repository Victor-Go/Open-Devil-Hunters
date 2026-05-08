using System;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.Level
{
  public class LevelClock : MonoBehaviour, IStoreChangedHandler
  {
    private StoreManager storeManager;
    private Text text;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      text = GetComponentInChildren<Text>();
    }

    private void Start()
    {
      storeManager.Subscribe(StoreNames.LevelTimePassedStore, this);

      var levelTimePassedState = storeManager.GetState<LevelTimePassedState>(StoreNames.LevelTimePassedStore);
      int timePassed = levelTimePassedState.LevelTimePassed;
      text.text = TimeSpan.FromSeconds(timePassed).ToString(@"mm\:ss");
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelTimePassedStore:
          LevelTimePassedState levelTimePassedState = (LevelTimePassedState)state;
          int timePassed = levelTimePassedState.LevelTimePassed;
          text.text = TimeSpan.FromSeconds(timePassed).ToString(@"mm\:ss");
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}