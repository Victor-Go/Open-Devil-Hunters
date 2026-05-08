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
  public class DebugAddGem : MonoBehaviour, IPointerClickHandler
  {
    private int clickedTimes;

    public void OnPointerClick(PointerEventData eventData)
    {
      if (!DebugConfigurations.DebugEnabled) return;

      clickedTimes++;
      if (clickedTimes <= 5) return;

      clickedTimes = 0;

      var gemProperties = GemUtils.GenerateGemProperties();
      StoreManager.Instance.Commit(StoreNames.GameDataStore, StoreActions.GameDataStore_ADD_GEM,
        new GameDataActionData()
        {
          Gem = gemProperties
        });

      SaveSystem.SaveGame();
      
      AudioWrapper.PlayClip(ResourceManager.Instance.GetResource("Audio/Sound/UI/buy"), Vector2.zero);
    }
  }
}