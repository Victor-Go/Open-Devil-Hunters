using System;
using System.Linq;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI.Main
{
  [Obsolete("This class is obsolete. See FightPreparationUI2, HeroSelection")]
  public class HeroCardContainer : MonoBehaviour
  {
    public GameObject HeroContainer;
    public ResourceManager resourceManager;

    private void Awake()
    {
      resourceManager = ResourceManager.Instance;
    }

    private void Start()
    {
      GameObject heroCardPrefab = resourceManager.GetResource("UI/Main/HeroCard");
      float cardHeight = heroCardPrefab.GetComponent<RectTransform>().rect.height,
        cardWidth = heroCardPrefab.GetComponent<RectTransform>().rect.width;

      foreach (var (item, index) in PlayerConfigurations.Players.Select((value, index) => (value, index)))
      {
        var heroCard = Instantiate(heroCardPrefab);
        heroCard.transform.SetParent(HeroContainer.transform, false);
        heroCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -index * cardHeight);

        var heroConfig = item.Value;
        var heroCardController = heroCard.GetComponent<HeroCard>();
        heroCardController.SetHeroConfiguration(heroConfig);

        if (index == 0)
        {
          heroCardController.InteractWithCurrentHero();
        }
      }
    }
  }
}