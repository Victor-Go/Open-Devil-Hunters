using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.Behaviour.UI.ControlGuide
{
  public class ControlGuide : UIAnimation
  {
    private StoreManager storeManager;
    private GameStates previousGameState;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
    }

    public override void OnWindowOpen()
    {
      base.OnWindowOpen();
      previousGameState = storeManager.GetState<GameState>(StoreNames.GameStateStore).CurrentGameState;
      storeManager.Commit(StoreNames.GameStateStore, StoreActions.GameStateStore_SET_GAME_STATE, new GameStateData()
      {
        GameState = GameStates.PAUSED_OTHERS,
      });


      EventSystem.current.SetSelectedGameObject(null);
      StartCoroutine(UIUtils.SetSelectedGameObject(transform.Find("Confirm").gameObject));
    }

    private void OnApplicationFocus(bool focus)
    {
      if (focus)
      {
        EventSystem.current.SetSelectedGameObject(transform.Find("Confirm").gameObject);
      }
    }

    protected override void OnCurrentWindowClosed()
    {
      // State change should be put advanced since OnCurrentWindowClosed will show potential next UI that commit game state change
      storeManager.Commit(StoreNames.GameStateStore, StoreActions.GameStateStore_SET_GAME_STATE, new GameStateData()
      {
        GameState = previousGameState,
      });
      
      base.OnCurrentWindowClosed();
    }

    public void Confirm()
    {
      AudioWrapper.PlayClip(ResourceManager.Instance.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      CloseWindow();
    }

    public void OK()
    {
      Confirm();
    }
  }
}