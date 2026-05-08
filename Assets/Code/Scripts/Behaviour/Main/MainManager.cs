using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Behaviour.UI.Main;
using UnityEngine;

namespace Code.Scripts.Behaviour.Main
{
  public class MainManager : MonoBehaviour, IEventHandler
  {
    private StoreManager storeManager;

    private void Awake()
    {
      if (!DebugConfigurations.DebugEnabled)
      {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
      }

      EventManager.Instance.AddEventHandler(Events.SAVE_CORRUPTED, this);

      storeManager = StoreManager.Instance;
      storeManager
        .SetStore(StoreNames.GameDataStore, new GameDataStore())
        .SetStore(StoreNames.GameSettingStore, new GameSettingStore())
        .SetStore(StoreNames.LevelConfigurationStore, new LevelConfigurationStore())
        .SetStore(StoreNames.PlayerStore, new PlayerStore());

      SetDefaultData();

      SaveSystem.LoadGame();

      SetDefaultLanguage();
    }

    private void SetDefaultData()
    {
      storeManager.Commit(StoreNames.LevelConfigurationStore,
        StoreActions.LevelConfigurationStore_SET_LEVEL_INIT_NUMBER_OF_PLAYERS, new LevelConfigurationData()
        {
          InitialNumberOfPlayers = 1,
        });

      storeManager.Commit(StoreNames.LevelConfigurationStore, StoreActions.LevelConfigurationStore_SET_MAP,
        new LevelConfigurationData()
        {
          MapName = MapNames.FOREST
        });
    }

    private void SetDefaultLanguage()
    {
      var gameSetting = storeManager.GetState<GameSettingState>(StoreNames.GameSettingStore);
      if (gameSetting.Language == Languages.auto)
      {
        Languages language;
        switch (Application.systemLanguage)
        {
          case SystemLanguage.German: language = Languages.de; break;
          case SystemLanguage.English: language = Languages.en; break;
          case SystemLanguage.Spanish: language = Languages.es; break;
          case SystemLanguage.French: language = Languages.fr; break;
          case SystemLanguage.Italian: language = Languages.it; break;
          case SystemLanguage.Japanese: language = Languages.ja; break;
          case SystemLanguage.Polish: language = Languages.pl; break;
          case SystemLanguage.Portuguese: language = Languages.pt; break;
          case SystemLanguage.Russian: language = Languages.ru; break;
          case SystemLanguage.ChineseSimplified: language = Languages.zh_CHS; break;
          case SystemLanguage.ChineseTraditional: language = Languages.zh_CHT; break;
          default: language = Languages.en; break;
        }

        storeManager.Commit(StoreNames.GameSettingStore, StoreActions.GameSettingStore_SET_LOCALE, new GameSettingData()
        {
          Language = language
        });
        SaveSystem.SaveGame();
      }
    }

    public void OnEvent(Events @event, IEventData data)
    {
      if (@event == Events.SAVE_CORRUPTED)
      {
        var eventData = (SaveCorruptedEventData)data;
        var infoBox = WindowManager.Instance.OpenWindow(WindowNames.InfoBoxUI, OpenWindowActions.WaitForOthers);
        infoBox.GetComponent<InfoBoxUI>()
          .SetTitle(string.Format(I18nUtils.GetText("Exception/CorruptedSaveFile"), eventData.BackUpPath));
      }
    }

    private void OnDestroy()
    {
      if (EventManager.Instance != null)
      {
        EventManager.Instance.RemoveEventHandler(this);
      }
    }
  }
}