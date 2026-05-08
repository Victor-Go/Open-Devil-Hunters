using Code.Scripts.Src;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class DebugAddHonor : MonoBehaviour, IPointerClickHandler
  {
    private int clickedTimes;

    public void OnPointerClick(PointerEventData eventData)
    {
      if (!DebugConfigurations.DebugEnabled) return;

      clickedTimes++;
      if (clickedTimes <= 5) return;

      clickedTimes = 0;
      StoreManager.Instance.Commit(StoreNames.GameDataStore, StoreActions.GameDataStore_INCREASE_HONOR,
        new GameDataActionData()
        {
          PlayerHonor = 50000,
        });
      SaveSystem.SaveGame();

      AudioWrapper.PlayClip(ResourceManager.Instance.GetResource("Audio/Sound/UI/buy"), Vector2.zero);
    }
  }
}