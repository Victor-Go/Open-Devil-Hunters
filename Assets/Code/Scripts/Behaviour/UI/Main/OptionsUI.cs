using System;
using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class OptionsUI : UIAnimation
  {
    private const float defaultBgm = 0.5f;
    private const float defaultSfx = 0.8f;

    private Text overallText;
    private Text bgmText;
    private Text sfxText;
    private Slider overallSlider;
    private Slider bgmSlider;
    private Slider sfxSlider;

    private AudioMixer audioMixer;

    private void Awake()
    {
      targetTransform = transform.Find("Panel").GetComponent<RectTransform>();
      overallText = targetTransform.Find("Overall/Volume").GetComponent<Text>();
      bgmText = targetTransform.Find("Bgm/Volume").GetComponent<Text>();
      sfxText = targetTransform.Find("Sfx/Volume").GetComponent<Text>();
      overallSlider = targetTransform.Find("Overall/Slider").GetComponent<Slider>();
      bgmSlider = targetTransform.Find("Bgm/Slider").GetComponent<Slider>();
      sfxSlider = targetTransform.Find("Sfx/Slider").GetComponent<Slider>();
      audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
    }

    protected override void Start()
    {
      base.Start();

      LoadVolume();
    }

    public void OnOverallChanged(float normalizedVolume)
    {
      if (overallText == null)
        return;
      normalizedVolume = Mathf.Clamp(normalizedVolume, 0.0001f, 1f);
      var volumeInDb = Mathf.Log10(normalizedVolume) * 20f;
      audioMixer.SetFloat(nameof(PlayerPrefVariables.OverallVolume), volumeInDb);
      overallText.text = $"{Mathf.RoundToInt(normalizedVolume * 100)}%";

      PlayerPrefs.SetFloat(nameof(PlayerPrefVariables.OverallVolume), normalizedVolume);
      PlayerPrefs.Save();
    }

    public void OnBgmChanged(float normalizedVolume)
    {
      if (bgmText == null)
        return;
      normalizedVolume = Mathf.Clamp(normalizedVolume, 0.0001f, 1f);
      var volumeInDb = Mathf.Log10(normalizedVolume) * 20f;
      audioMixer.SetFloat(nameof(PlayerPrefVariables.BgmVolume), volumeInDb);
      bgmText.text = $"{Mathf.RoundToInt(normalizedVolume * 100)}%";

      PlayerPrefs.SetFloat(nameof(PlayerPrefVariables.BgmVolume), normalizedVolume);
      PlayerPrefs.Save();
    }

    public void OnSfxChanged(float normalizedVolume)
    {
      if (sfxText == null)
        return;
      normalizedVolume = Mathf.Clamp(normalizedVolume, 0.0001f, 1f);
      var volumeInDb = Mathf.Log10(normalizedVolume) * 20f;
      audioMixer.SetFloat(nameof(PlayerPrefVariables.SfxVolume), volumeInDb);
      sfxText.text = $"{Mathf.RoundToInt(normalizedVolume * 100)}%";

      PlayerPrefs.SetFloat(nameof(PlayerPrefVariables.SfxVolume), normalizedVolume);
      PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
      var doLoadVolume = new Action<string, Slider, Action<float>, float>((key, slider, onChanged, @default) =>
      {
        if (PlayerPrefs.HasKey(key))
        {
          var savedVolume = PlayerPrefs.GetFloat(key);

          var volumeInDb = Mathf.Log10(savedVolume) * 20f;
          audioMixer.SetFloat(key, volumeInDb);

          slider.value = savedVolume;
        }
        else
        {
          onChanged(@default);
        }
      });

      doLoadVolume("OverallVolume", overallSlider, OnOverallChanged, 1f);
      doLoadVolume("BgmVolume", bgmSlider, OnBgmChanged, defaultBgm);
      doLoadVolume("SfxVolume", sfxSlider, OnSfxChanged, defaultSfx);
    }

    public void Confirm()
    {
      AudioWrapper.PlayClip(ResourceManager.Instance.GetResource("AudioClip/ui-button_0"), Vector2.zero);
      CloseWindow();
    }
  }
}