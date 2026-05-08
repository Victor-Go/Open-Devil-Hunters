using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.I18n;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.Behaviour.UI.Main
{
  public struct HeroCardNavigation
  {
    public GameObject left { get; set; }
    public GameObject right { get; set; }
    public GameObject up { get; set; }
    public GameObject down { get; set; }
  }

  public class HeroSelection : UIWindow, IStoreChangedHandler
  {
    private int numberOfPlayers;

    private StoreManager storeManager;

    private readonly List<GameObject> heroes = new();

    private void Awake()
    {
      storeManager = StoreManager.Instance;
    }

    private void Start()
    {
      storeManager.Subscribe(StoreNames.LevelConfigurationStore, this);

      numberOfPlayers = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore)
        .NumberOfPlayers;

      var resourceManager = ResourceManager.Instance;
      var playerConfigs = PlayerConfigurations.Players
        .Select(kvp => kvp.Value)
        .ToList()
        .OrderByDescending(p => GeneralConfigurations.DefaultUnlockedPlayers.Contains(p.PlayerName) ? 1 : 0);
      foreach (var playerConfig in playerConfigs)
      {
        GameObject heroCard = Instantiate(resourceManager.GetResource("UI/Main/HeroCard"));
        heroCard.transform.SetParent(transform, false);
        var heroCardScript = heroCard.GetComponent<HeroCard>();
        heroCardScript.SetHeroConfiguration(playerConfig);
        RegisterChildWindow(heroCardScript);
        heroes.Add(heroCard);
      }

      var verticalCount = (int)Mathf.Ceil(heroes.Count / 4f);
      for (var vertical = 0; vertical < verticalCount; vertical++)
      {
        var horizontalCount = Mathf.Min(heroes.Count - vertical * 4, 4);
        for (var horizontal = 0; horizontal < horizontalCount; horizontal++)
        {
          heroes[vertical * 4 + horizontal].GetComponent<RectTransform>().anchoredPosition =
            new Vector2(60 + 110 * horizontal, -85 - vertical * 160);
        }
      }

      for (var vertical = 0; vertical < verticalCount; vertical++)
      {
        var horizontalCount = Mathf.Min(heroes.Count - vertical * 4, 4);
        for (var horizontal = 0; horizontal < horizontalCount; horizontal++)
        {
          var up = (vertical > 0 ? vertical - 1 : verticalCount - 1) * 4 + horizontal;
          if (up > heroes.Count - 1) up = (verticalCount - 2) * 4 + horizontal;

          var down = (vertical < verticalCount - 1 ? vertical + 1 : 0) * 4 + horizontal;
          if (down > heroes.Count - 1) down = horizontal;

          var left = vertical * 4 + (horizontal > 0 ? horizontal - 1 : horizontalCount - 1);

          var right = vertical * 4 + (horizontal < horizontalCount - 1 ? horizontal + 1 : 0);

          var navigation = new HeroCardNavigation()
          {
            up = heroes[up],
            down = heroes[down],
            left = heroes[left],
            right = heroes[right],
          };
          heroes[vertical * 4 + horizontal].GetComponent<HeroCard>().SetNavigation(navigation);
        }
      }

      EventSystem.current.SetSelectedGameObject(heroes[0]);

      for (var i = 0; i < numberOfPlayers; i++)
      {
        SelectFirstAvailableAndNotSelectedHero(i);
      }
    }

    private void SelectFirstAvailableAndNotSelectedHero(int playerNumber)
    {
      foreach (var heroPrefab in heroes)
      {
        var heroCard = heroPrefab.GetComponent<HeroCard>();
        if (heroCard.AvailableInTrailVersion && !(heroCard.Selected && heroCard.SelectedByPlayerNumber != playerNumber))
        {
          heroCard.InteractWithCurrentHero(playerNumber);
          break;
        }
      }
    }

    private void Remove2NdPlayerSelection()
    {
      foreach (var heroPrefab in heroes)
      {
        var heroCard = heroPrefab.GetComponent<HeroCard>();
        if (heroCard.SelectedByPlayerNumber == 1)
        {
          heroCard.UnselectCurrentHero();
        }
      }
    }

    private void HandleHeroSelect(int playerNumber)
    {
      var up = InputUtils.GetButton(playerNumber, InputButtons.UP);
      var down = InputUtils.GetButton(playerNumber, InputButtons.DOWN);
      var left = InputUtils.GetButton(playerNumber, InputButtons.LEFT);
      var right = InputUtils.GetButton(playerNumber, InputButtons.RIGHT);
      var select = InputUtils.GetButton(playerNumber, InputButtons.CONFIRM);

      if (!(up || down || left || right || select)) return;

      var heroCards = heroes.Select(h => h.GetComponent<HeroCard>()).ToList();

      // Find current hovering hero by playerNumber.
      var hoveredHeroCards = heroCards
        .Where(heroCard => heroCard.Hovered && heroCard.HoveredByPlayerNumber == playerNumber)
        .ToList();

      HeroCard currentController = null;
      if (hoveredHeroCards.Any())
      {
        currentController = hoveredHeroCards.First();
      }
      else // If there's not hovering hero, then find current selected hero.
      {
        var selectedHeroCards = heroCards
          .Where(heroCard => heroCard.Selected && heroCard.SelectedByPlayerNumber == playerNumber)
          .ToList();

        if (selectedHeroCards.Any())
        {
          currentController = selectedHeroCards.First();
        }
        else
        {
          currentController = heroCards
            .First(heroCard => heroCard.AvailableInTrailVersion)
            .GetComponent<HeroCard>();
        }
      }

      if (up)
      {
        currentController.UnhoverCurrentHero(playerNumber);
        currentController.Navigation.up.GetComponent<HeroCard>().HandleHover(playerNumber);
      }
      else if (down)
      {
        currentController.UnhoverCurrentHero(playerNumber);
        currentController.Navigation.down.GetComponent<HeroCard>().HandleHover(playerNumber);
      }
      else if (left)
      {
        currentController.UnhoverCurrentHero(playerNumber);
        currentController.Navigation.left.GetComponent<HeroCard>().HandleHover(playerNumber);
      }
      else if (right)
      {
        currentController.UnhoverCurrentHero(playerNumber);
        currentController.Navigation.right.GetComponent<HeroCard>().HandleHover(playerNumber);
      }
      else if (select)
      {
        currentController.InteractWithCurrentHero(playerNumber);
      }
    }

    private void Update()
    {
      HandleHeroSelect(0);
      if (numberOfPlayers > 1)
      {
        HandleHeroSelect(1);
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.LevelConfigurationStore:
          var levelConfigs = (LevelConfigurationState)state;

          if (levelConfigs.NumberOfPlayers > numberOfPlayers)
          {
            SelectFirstAvailableAndNotSelectedHero(1);
          }
          else if (levelConfigs.NumberOfPlayers < numberOfPlayers)
          {
            Remove2NdPlayerSelection();
          }

          numberOfPlayers = levelConfigs.NumberOfPlayers;
          break;
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}