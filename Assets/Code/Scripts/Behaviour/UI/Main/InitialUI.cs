using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Behaviour.UI;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Steam.Achievements;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class InitialUI : AnimatedUI
  {
    public GameObject DefaultButton;
    private ResourceManager resourceManager;
    private AudioMixer audioMixer;

    protected override void Awake()
    {
      base.Awake();
      resourceManager = ResourceManager.Instance;
      audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
    }

    private void Start()
    {
      LoadVolume();

      TryToShowNotification();
      
      var mouse = Mouse.current;
      mouse.WarpCursorPosition(new Vector2(0, 0));
    }

    private void LoadVolume()
    {
      var onChange = new Action<string, float>((key, normalizedVolume) =>
      {
        normalizedVolume = Mathf.Clamp(normalizedVolume, 0.0001f, 1f);
        var volumeInDb = Mathf.Log10(normalizedVolume) * 20f;
        audioMixer.SetFloat(key, volumeInDb);
        PlayerPrefs.SetFloat(key, normalizedVolume);
        PlayerPrefs.Save();
      });

      var doLoadVolume = new Action<string, Action<string, float>, float>((key, onChanged, @default) =>
      {
        if (PlayerPrefs.HasKey(key))
        {
          var savedVolume = PlayerPrefs.GetFloat(key);

          var volumeInDb = Mathf.Log10(savedVolume) * 20f;
          audioMixer.SetFloat(key, volumeInDb);
        }
        else
        {
          onChanged(key, @default);
        }
      });

      doLoadVolume("OverallVolume", onChange, 1f);
      doLoadVolume("BgmVolume", onChange, 0.5f);
      doLoadVolume("SfxVolume", onChange, 1f);
    }

    private void TryToShowNotification()
    {
      var notif = NotificationUtils.GetNotReadNotification(NotificationCatagories.FRONT_PAGE, NotificationType.TEXT);
      if (notif == null) return;

      var _notif = (TextNotification)notif;
      var infoBox = OpenSubWindow(WindowNames.InfoBoxUI);
      infoBox.GetComponent<InfoBoxUI>().SetTitle(I18nUtils.GetText(_notif.TextName));

      StoreManager.Instance.Commit(StoreNames.GameSettingStore, StoreActions.GameSettingStore_ADD_SHOWN_NOTIFICATION,
        new GameSettingData()
        {
          NotificationId = _notif.Id,
        });
      SaveSystem.SaveGame();
    }

    protected override void OnAllUiEnterAnimationFinished()
    {
      base.OnAllUiEnterAnimationFinished();

      if (DefaultButton != null)
      {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelected(DefaultButton));
      }
    }

    private void OnApplicationFocus(bool focus)
    {
      if (focus && DefaultButton != null)
      {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelected(DefaultButton));
      }
    }

    public void OpenFightPreparation()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      _windowManager.OpenWindow(WindowNames.FightPreparation, OpenWindowActions.CloseOthers);
    }

    public void OpenSelectLanguage()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      OpenSubWindow(WindowNames.SelectLanguage);
    }

    public void OpenOptions()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);

      OpenSubWindow(WindowNames.OptionsUI);
    }

    public void ExitApplication()
    {
      AudioWrapper.PlayClip(resourceManager.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      Application.Quit();
    }
  }
}