using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Src.Types
{
  public abstract class PauseableGameObject : MonoBehaviour, IStoreChangedHandler
  {
    public bool CanMoveWhenTimeStopped;

    protected StoreManager storeManager;

    // Attention: animator is get everywhere. So, check components that refer to animation to see if it should be movable during TimeStop.
    protected Animator animator;
    protected new ParticleSystem particleSystem;
    protected bool paused;

    public virtual void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.GameStateStore:
          HandleGameStateChanged((GameState)state);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    protected virtual void Awake()
    {
      storeManager = StoreManager.Instance;

      animator = GetComponent<Animator>();
      particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    protected virtual void Start()
    {
      storeManager.Subscribe(StoreNames.GameStateStore, this);

      var gameState = storeManager.GetState<GameState>(StoreNames.GameStateStore);
      HandleGameStateChanged(gameState);
    }

    protected virtual void HandleGameStateChanged(GameState state)
    {
      var gameState = state.CurrentGameState;
      paused = CanMoveWhenTimeStopped
        ? !LevelUtils.PlayerCanMove(gameState)
        : LevelUtils.EverythingCanNotMove(gameState);

      if (animator != null)
      {
        animator.speed = paused ? 0 : 1;
      }

      if (particleSystem != null)
      {
        switch (paused)
        {
          case true: particleSystem.Stop(); break;
          case false: particleSystem.Play(); break;
        }
      }
    }

    protected virtual void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}