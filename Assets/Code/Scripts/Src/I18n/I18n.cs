using System;
using System.Collections.Generic;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Game;

namespace Code.Scripts.Src.I18n
{
  public enum Languages
  {
    auto,
    de,
    en,
    es,
    fr,
    it,
    ja,
    ko,
    pl,
    pt,
    ru,
    uk,
    zh_CHS,
    zh_CHT,
  }

  public static class I18nUtils
  {
    private static readonly Dictionary<Languages, string> localePairs = new()
    {
      { Languages.auto, "en" },
      { Languages.de, "de" },
      { Languages.en, "en" },
      { Languages.es, "es" },
      { Languages.fr, "fr" },
      { Languages.it, "it" },
      { Languages.ja, "ja" },
      { Languages.ko, "ko" },
      { Languages.pl, "pl" },
      { Languages.pt, "pt" },
      { Languages.ru, "ru" },
      { Languages.uk, "uk" },
      { Languages.zh_CHS, "zh-CHS" },
      { Languages.zh_CHT, "zh-CHT" },
    };

    public static string GetText(string indicator)
    {
      var language = StoreManager.Instance.GetState<GameSettingState>(StoreNames.GameSettingStore).Language;
      return GetText(language, indicator);
    }

    public static string GetI18nLocale()
    {
      var language = StoreManager.Instance.GetState<GameSettingState>(StoreNames.GameSettingStore).Language;
      return localePairs[language];
    }

    public static string GetText(Languages language, string indicator)
    {
      var text = "";
      switch (language)
      {
        case Languages.de:
          if (de.locales.ContainsKey(indicator))
            text = de.locales[indicator];
          break;
        case Languages.en:
          if (en.locales.ContainsKey(indicator))
            text = en.locales[indicator];
          break;
        case Languages.es:
          if (es.locales.ContainsKey(indicator))
            text = es.locales[indicator];
          break;
        case Languages.fr:
          if (fr.locales.ContainsKey(indicator))
            text = fr.locales[indicator];
          break;
        case Languages.it:
          if (it.locales.ContainsKey(indicator))
            text = it.locales[indicator];
          break;
        case Languages.ja:
          if (ja.locales.ContainsKey(indicator))
            text = ja.locales[indicator];
          break;
        case Languages.ko:
          if (ko.locales.ContainsKey(indicator))
            text = ko.locales[indicator];
          break;
        case Languages.pl:
          if (pl.locales.ContainsKey(indicator))
            text = pl.locales[indicator];
          break;
        case Languages.pt:
          if (pt.locales.ContainsKey(indicator))
            text = pt.locales[indicator];
          break;
        case Languages.ru:
          if (ru.locales.ContainsKey(indicator))
            text = ru.locales[indicator];
          break;
        case Languages.uk:
          if (uk.locales.ContainsKey(indicator))
            text = uk.locales[indicator];
          break;
        case Languages.zh_CHS:
          if (zh_CHS.locales.ContainsKey(indicator))
            text = zh_CHS.locales[indicator];
          break;
        case Languages.zh_CHT:
          if (zh_CHT.locales.ContainsKey(indicator))
            text = zh_CHT.locales[indicator];
          break;
        case Languages.auto:
        default:
          text = en.locales[indicator];
          break;
      }

      return !text.Equals("") ? text : en.locales.ContainsKey(indicator) ? en.locales[indicator] : indicator;
    }
  }

  public interface ILanguage
  {
    public static Dictionary<string, string> locales;
  }
}