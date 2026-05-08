using Code.Scripts.Src;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src;
using Code.Scripts.Src.I18n;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI
{
    public class LanguageButton : MonoBehaviour
    {
        public Languages Language;

        private StoreManager storeManager;

        private Text text;

        private void Awake()
        {
            text = transform.Find("Text").GetComponent<Text>();
        }

        private void Start()
        {
            storeManager = StoreManager.Instance;

            text.text = I18nUtils.GetText(Language, "Language");
        }

        public void SwitchToCurrentLanguage()
        {
            storeManager.Commit(StoreNames.GameSettingStore, StoreActions.GameSettingStore_SET_LOCALE, new GameSettingData()
            {
                Language = Language,
            });
            SaveSystem.SaveGame();
        }
    }
}