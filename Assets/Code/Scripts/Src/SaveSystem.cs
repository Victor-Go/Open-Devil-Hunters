using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.I18n;
using Code.Scripts.Src.Store.Game;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Src
{
  [Serializable]
  public class GameSave
  {
    public int Version { get; set; }
    public GameDataState GameData { get; set; }
    public GameSettingState GameSetting { get; set; }
  }

  public static class SaveSystem
  {
    private static readonly int CurrentSaveVersion = GeneralConfigurations.CurrentSaveVersion;
    private static readonly string savePath = $"{Application.persistentDataPath}/save0.dhsav";

    private static readonly byte[] Key = Encoding.UTF8.GetBytes("DevilHuntersAES12345678901234567"); // 32 bytes
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("DevilHuntersIV12"); // 16 bytes

    private static readonly Func<string> getBackUpPath = () =>
      $"{Application.persistentDataPath}/corrupted-{DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff", CultureInfo.InvariantCulture)}.dhsav";

    private static string Encrypt(string plainText)
    {
      using var aes = Aes.Create();
      aes.Key = Key;
      aes.IV = IV;
      using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
      using var ms = new MemoryStream();
      using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
      using (var sw = new StreamWriter(cs))
      {
        sw.Write(plainText);
      }
      return Convert.ToBase64String(ms.ToArray());
    }

    private static string Decrypt(string cipherText)
    {
      using var aes = Aes.Create();
      aes.Key = Key;
      aes.IV = IV;
      using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
      using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
      using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
      using var sr = new StreamReader(cs);
      return sr.ReadToEnd();
    }

    public static void SaveGame()
    {
      StoreManager storeManager = StoreManager.Instance;

      FileStream stream = null;
      try
      {
        var gameData = storeManager.GetState<GameDataState>(StoreNames.GameDataStore);
        var gameSetting = storeManager.GetState<GameSettingState>(StoreNames.GameSettingStore);

        var gameSave = new GameSave()
        {
          Version = CurrentSaveVersion,
          GameData = gameData,
          GameSetting = ObjectCopier.Clone(gameSetting),
        };

        var json = JsonConvert.SerializeObject(gameSave, Formatting.None);
        var encrypted = Encrypt(json);
        File.WriteAllText(savePath, encrypted);

        Debug.LogFormat("Saved game to <{0}>.", savePath);
      }
      catch (Exception ex)
      {
        Debug.LogErrorFormat("Unable to save game to <{0}>!", savePath);
        Debug.LogException(ex);

      }
    }

    public static void LoadGame()
    {
      StoreManager storeManager = StoreManager.Instance;

      if (File.Exists(savePath))
      {
        FileStream stream = null;
        try
        {
          var encrypted = File.ReadAllText(savePath);
          var json = Decrypt(encrypted);
          var gameSave = JsonConvert.DeserializeObject<GameSave>(json);

          if (gameSave != null)
          {
            var gameData = gameSave.GameData;
            var gameSetting = gameSave.GameSetting;

            storeManager.Commit(StoreNames.GameDataStore, StoreActions.GameDataStore_LOAD_FROM_SAVE,
              new GameDataActionData
              {
                SavedState = gameData,
              });

            storeManager.Commit(StoreNames.GameSettingStore, StoreActions.GameSettingStore_LOAD_FROM_SAVE,
              new GameSettingData
              {
                SavedState = gameSetting,
              });
            
            Debug.LogFormat("Loaded game from <{0}>.", savePath);
          }
        }
        catch (Exception ex)
        {
          var backUpPath = getBackUpPath();
          Debug.LogErrorFormat("Saved file <{0}> corrupted! Already backed up to <{1}>.", savePath, backUpPath);
          Debug.LogException(ex);

          EventManager.Instance.PublishEvent(Events.SAVE_CORRUPTED, new SaveCorruptedEventData { BackUpPath = backUpPath });

          File.Copy(savePath, backUpPath);
          File.Delete(savePath);
        }
      }
      else
      {
        Debug.LogFormat("There's no saved file in <{0}>. Game not loaded.", savePath);
      }
    }
  }
}