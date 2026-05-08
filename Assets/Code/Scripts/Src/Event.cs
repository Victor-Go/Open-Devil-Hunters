using System;
using System.Collections.Generic;
using Code.Scripts.Behaviour.Level;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Utils;

namespace Code.Scripts.Src
{
  #region EventData

  #endregion

  public enum Events
  {
    PLAYER_SELECT_HERO, // For FightPreparation.
    PLAYER_HOVER_HERO, // For FightPreparation.

    UPDATE_PLAYER_DIRECTION,

    LEVEL_INITIALIZED,

    LOCALE_CHANGED,

    QTE_PRECISE_RESOLVED,

    PLAYER_UPGRADE_FINISHED,

    AIMING_CHANGED, // Aiming

    PLAYER_HURT, // Shake Camara

    PLAYER_DIED,

    BOSS_APPEAR, // For little boss to build wall.
    BOSS_DIED,
    GAME_OVER,
    SAVE_CORRUPTED,
  }

  public interface IEventData
  {
  }

  public class PlayerSelectOrHoverHeroEventData : IEventData
  {
    public int PlayerNumber { get; set; }
    public PlayerNames PlayerName { get; set; }
  }

  public class UpdatePlayerDirectionEventData : IEventData
  {
    public int PlayerNumber { get; set; }
    public PlayerDirection PlayerDirection { get; set; }
  }

  public class PlayerDiedEventData : IEventData
  {
    public int PlayerNumber { get; set; }
  }

  public class BossAppearEventData : IEventData
  {
    public Types.Enemy Enemy { get; set; }
  }

  public class SaveCorruptedEventData : IEventData
  {
    public string BackUpPath { get; set; }
  }

  public interface IEventHandler
  {
    public void OnEvent(Events @event, IEventData data);
  }

  [Serializable]
  public class InvalidEventHandlingException : Exception
  {
    public InvalidEventHandlingException() : base()
    {
    }

    public InvalidEventHandlingException(Events @event)
      : base($"Invalid Event {@event} is handled.")
    {
      Console.WriteLine("Invalid Event {0} is handled.", @event);
    }
  }

  [Serializable]
  public class InvalidEventNameException : Exception
  {
    public InvalidEventNameException() : base()
    {
    }

    public InvalidEventNameException(Events @event)
      : base($"Invalid Event Name: {@event}, check if the event is listened.")
    {
      Console.WriteLine("Invalid Event Name: {0}, check if the event is listened.", @event);
    }
  }

  public class EventManager : Singleton<EventManager>
  {
    private readonly Dictionary<Events, List<IEventHandler>> handlers = new();

    public EventManager AddEventHandler(Events @event, IEventHandler handler)
    {
      if (!handlers.ContainsKey(@event))
      {
        handlers.Add(@event, new List<IEventHandler>());
      }

      handlers[@event].Add(handler);
      return this;
    }

    public EventManager PublishEvent(Events @event, IEventData data)
    {
      if (!handlers.ContainsKey(@event))
      {
        UnityEngine.Debug.LogWarningFormat("Published a new event {0}, but nobody is listening to this event.", @event);
      }
      else
      {
        handlers[@event].RemoveAll(item => item == null);
        foreach (var handler in handlers[@event])
        {
          handler.OnEvent(@event, data);
        }
      }

      return this;
    }

    public EventManager RemoveEventHandler(IEventHandler handler)
    {
      foreach (var eventHandler in handlers)
      {
        eventHandler.Value.Remove(handler);
      }

      return this;
    }

    public void Destroy()
    {
      handlers.Clear();
    }
  }
}