using Code.Scripts.Src;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Store.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Src
{
  public struct Scheduler
  {
    public long UniqueId { get; set; }
    public Action Action { get; set; }
    public Action<object> ActionWithState { get; set; }
    public object State { get; set; }
    public float ScheduledTime { get; set; }
    public float Timeout { get; set; }
    public bool Iterate { get; set; }
    public string Trace { get; set; }
  }

  /**
   * Scheduling only works for battlefield
   */
  public class Scheduling : Singleton<Scheduling>, IStoreChangedHandler
  {
    private StoreManager storeManager;
    private float gameTime;
    private readonly List<Scheduler> schedulers = new();
    private readonly List<long> schedulersRemovedInCurrentTick = new();
    private readonly List<Scheduler> timeUpSchedulers = new();
    private static long nextScheduleId = 0;

    protected Scheduling()
    {
      Init();
    }

    public void Init()
    {
      storeManager = StoreManager.Instance;
      storeManager.Subscribe(StoreNames.HighPrecisionLevelTimeStore, this);
    }

    public long SetTimeout(Action action, float timeoutSeconds)
    {
      long uniqueId = ++nextScheduleId;

      schedulers.Add(new()
      {
        UniqueId = uniqueId,
        Action = action,
        ScheduledTime = timeoutSeconds + gameTime,
        Timeout = timeoutSeconds,
        Trace = Environment.StackTrace
      });

      return uniqueId;
    }

    public long SetTimeout(Action<object> action, object state, float timeoutSeconds)
    {
      long uniqueId = ++nextScheduleId;

      schedulers.Add(new()
      {
        UniqueId = uniqueId,
        ActionWithState = action,
        State = state,
        ScheduledTime = timeoutSeconds + gameTime,
        Timeout = timeoutSeconds,
        Trace = Environment.StackTrace
      });

      return uniqueId;
    }

    // Important: Use SetInterval(() => action(), 1) to call action. Otherwise, it will lost context. 
    public long SetInterval(Action action, float timeoutSeconds)
    {
      long uniqueId = ++nextScheduleId;

      schedulers.Add(new()
      {
        UniqueId = uniqueId,
        Action = action,
        ScheduledTime = timeoutSeconds + gameTime,
        Timeout = timeoutSeconds,
        Iterate = true,
        Trace = Environment.StackTrace.ToString()
      });

      return uniqueId;
    }

    public long ResetInterval(Action action, Action<object> actionWithState, object state, float timeoutSeconds, string trace, long uniqueId)
    {
      schedulers.Add(new()
      {
        UniqueId = uniqueId,
        Action = action,
        ActionWithState = actionWithState,
        State = state,
        ScheduledTime = timeoutSeconds + gameTime,
        Timeout = timeoutSeconds,
        Iterate = true,
        Trace = trace
      });

      return uniqueId;
    }

    public long SetInterval(Action<object> action, object state, float timeoutSeconds)
    {
      long uniqueId = ++nextScheduleId;

      schedulers.Add(new()
      {
        UniqueId = uniqueId,
        ActionWithState = action,
        State = state,
        ScheduledTime = timeoutSeconds + gameTime,
        Timeout = timeoutSeconds,
        Iterate = true,
        Trace = Environment.StackTrace.ToString()
      });

      return uniqueId;
    }

    public void ClearSchedule(long uniqueId)
    {
      for (int i = schedulers.Count - 1; i >= 0; i--)
      {
        if (schedulers[i].UniqueId == uniqueId)
        {
          schedulers.RemoveAt(i);
        }
      }
      schedulersRemovedInCurrentTick.Add(uniqueId);
    }

    public void Reset()
    {
      schedulers.Clear();
      storeManager.Unsubscribe(this);
    }

    private void PopulateTimeUpSchedulers()
    {
      timeUpSchedulers.Clear();
      for (int i = schedulers.Count - 1; i >= 0; i--)
      {
        var s = schedulers[i];
        if (s.ScheduledTime <= gameTime)
        {
          timeUpSchedulers.Add(s);
          schedulers.RemoveAt(i);
        }
      }
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.HighPrecisionLevelTimeStore:
          var highPrecisionClock = (HighPrecisionLevelTimeState)state;
          OnUpdate(highPrecisionClock.LevelTimePassed);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void OnUpdate(float gameTime)
    {
      this.gameTime = gameTime;

      if (schedulers.Count == 0)
      {
        return;
      }

      PopulateTimeUpSchedulers();
      foreach (var scheduler in timeUpSchedulers)
      {
        try
        {
          if (scheduler.Action != null)
          {
            scheduler.Action();
          }
          else if (scheduler.ActionWithState != null)
          {
            scheduler.ActionWithState(scheduler.State);
          }
          
          if (scheduler.Iterate && !schedulersRemovedInCurrentTick.Contains(scheduler.UniqueId))
          {
            ResetInterval(scheduler.Action, scheduler.ActionWithState, scheduler.State, scheduler.Timeout, scheduler.Trace, scheduler.UniqueId);
          }
        }
        catch (Exception e)
        {
          Debug.LogErrorFormat("Action call in scheduler throws error: {0}\nStack Trace: \n{1}\nSchedule Trace:\n{2}",
            e.Message, e.StackTrace, scheduler.Trace);
          if (DebugConfigurations.DebugEnabled)
          {
            throw e;
          }
        }
      }

      schedulersRemovedInCurrentTick.Clear();
    }

    ~Scheduling()
    {
      Reset();
    }
  }
}