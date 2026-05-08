using Code.Scripts.Src.Store.Game;
using System.Collections.Generic;
using System.Linq;

namespace Code.Scripts.Src
{
  public enum NotificationCatagories
  {
    FRONT_PAGE,
    BATTLEFIELD_BEGIN,
  }

  public enum NotificationType
  {
    NORMAL,
    TEXT,
  }

  public class Notification
  {
    public string Id { get; set; }
    public NotificationType NotificationType { get; set; }
    public int Priority { get; set; }
  }

  public class TextNotification : Notification
  {
    public string TextName { get; set; }
  }

  public static class NotificationUtils
  {
    private static readonly Dictionary<NotificationCatagories, List<Notification>> notifications = new()
    {
      {
        NotificationCatagories.FRONT_PAGE,
        new()
        {
          new TextNotification()
          {
            Id = "WelcomeNotification",
            NotificationType = NotificationType.TEXT,
            TextName = "Notification/WelcomeNotification",
          },
        }
      },
      {
        NotificationCatagories.BATTLEFIELD_BEGIN,
        new List<Notification>
        {
          new()
          {
            Id = "ControlGuide",
            NotificationType = NotificationType.NORMAL,
          }
        }
      }
    };

    public static Notification GetNotReadNotification(NotificationCatagories category)
    {
      var gameSetting = StoreManager.Instance.GetState<GameSettingState>(StoreNames.GameSettingStore);

      var got = notifications.TryGetValue(category, out var selectedCategory);
      if (!got || !selectedCategory.Any()) return null;


      var matchedNotifs = selectedCategory
        .Where(n => !gameSetting.ShownNotificationIds.Contains(n.Id))
        .OrderByDescending(n => n.Priority)
        .ToList();


      return matchedNotifs.Any() ? matchedNotifs.First() : null;
    }

    public static Notification GetNotReadNotification(NotificationCatagories category, NotificationType type)
    {
      var gameSetting = StoreManager.Instance.GetState<GameSettingState>(StoreNames.GameSettingStore);

      var got = notifications.TryGetValue(category, out var selectedCategory);
      if (!got || !selectedCategory.Any()) return null;

      var matchedNotifs = selectedCategory
        .Where(n => !gameSetting.ShownNotificationIds.Contains(n.Id) && n.NotificationType == type)
        .OrderByDescending(n => n.Priority)
        .ToList();

      return matchedNotifs.Any() ? matchedNotifs.First() : null;
    }
  }
}