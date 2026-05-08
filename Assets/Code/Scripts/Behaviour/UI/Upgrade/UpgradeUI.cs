using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Upgrade;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Upgrade
{
  public class UpgradeUI : UIAnimation
  {
    private int _playerNumber { get; set; }

    private StoreManager storeManager;
    private readonly List<GameObject> upgradeCards = new();
    private Transform upgradeCardContainer;
    private Image currentPlayer;
    private int optionCount = 4;
    private const float margin = 10;
    private GameStates previousGameState;

    private void Awake()
    {
      storeManager = StoreManager.Instance;
      upgradeCardContainer = transform.Find("UpgradeCardContainer");
      currentPlayer = transform.Find("CurrentPlayer").GetComponent<Image>();
    }


    public override void OnWindowOpen()
    {
      base.OnWindowOpen();

      storeManager.Commit(StoreNames.BackgroundCanvas,
        StoreActions.BackgroundCanvas_SET_ACTIVE,
        new BackgroundCanvasActionData()
        {
          Active = true
        }
      );

      previousGameState = storeManager.GetState<GameState>(StoreNames.GameStateStore).CurrentGameState;

      storeManager.Commit(StoreNames.GameStateStore,
        StoreActions.GameStateStore_SET_GAME_STATE,
        new GameStateData { GameState = GameStates.UPGRADING }
      );
      
      SetFocus();
    }

    private void SetFocus()
    {
      if (upgradeCards.Any() && isActiveAndEnabled)
      {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelected(upgradeCards[0]));
      }
    }

    public void SetPlayerNumber(int playerNumber)
    {
      _playerNumber = playerNumber;
      var levelConfigs = storeManager.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);
      if (levelConfigs.PlayerConfigurations[playerNumber].PlayerName == PlayerNames.WITCH)
      {
        optionCount = 5;
      }

      SetPlayerImage();
    }

    protected override void OnWindowOpened()
    {
      base.OnWindowOpened();
      
      ShuffleUpgradeSkills();
    }

    private IEnumerator SetSelected(GameObject button)
    {
      yield return new WaitForEndOfFrame();
      EventSystem.current.SetSelectedGameObject(button.transform.Find("Choose").gameObject);
    }

    private void SetPlayerImage()
    {
      var levelConfigs = StoreManager.Instance.GetState<LevelConfigurationState>(StoreNames.LevelConfigurationStore);

      Texture2D tex =
        ResourceManager.Instance.GetResource(levelConfigs.PlayerConfigurations[_playerNumber].AvatarImageIndicator);
      currentPlayer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2);
    }

    private void ShuffleUpgradeSkills()
    {
      upgradeCards.ForEach(Destroy);
      upgradeCards.Clear();

      var shuffledUpgradeSkills = UpgradeReducers.GetCandidateUpgrades(_playerNumber, optionCount);

      for (var i = 0; i < optionCount && i < shuffledUpgradeSkills.Count; i++)
      {
        var _upgradeCard = Instantiate(ResourceManager.Instance.GetResource("UI/Upgrade/UpgradeCard"),
          upgradeCardContainer, false);
        var _upgradeCardScript = _upgradeCard.GetComponent<UpgradeCard>();
        _upgradeCardScript.SetUpgrade(shuffledUpgradeSkills[i]).PlayerNumber = _playerNumber;
        _upgradeCardScript.SetUpgradeUI(this);

        _upgradeCard.transform.SetParent(upgradeCardContainer);

        upgradeCards.Add(_upgradeCard);
      }

      var arrangeCount = shuffledUpgradeSkills.Count;
      var upgradeCard = upgradeCards[0].GetComponent<RectTransform>();
      var width = upgradeCard.rect.width;
      var scale = upgradeCard.localScale.x;
      var initialPosition = -(arrangeCount * width * scale + (arrangeCount - 1) * margin * scale) / 2;
      for (var i = 0; i < upgradeCards.Count; i++)
      {
        upgradeCards[i].GetComponent<RectTransform>().anchoredPosition =
          new Vector2(initialPosition + i * width * scale + i * margin * scale, 0);
      }

      SetFocus();
    }

    protected override void OnCurrentWindowClosed()
    {
      // State change should be put advanced since OnCurrentWindowClosed will show potential next UpgradeUI that commit game state change
      storeManager.Commit(StoreNames.GameStateStore,
        StoreActions.GameStateStore_SET_GAME_STATE,
        new GameStateData { GameState = previousGameState }
      );
      storeManager.Commit(StoreNames.BackgroundCanvas, StoreActions.BackgroundCanvas_SET_ACTIVE,
        new BackgroundCanvasActionData()
        {
          Active = false
        });

      base.OnCurrentWindowClosed();
    }
  }
}