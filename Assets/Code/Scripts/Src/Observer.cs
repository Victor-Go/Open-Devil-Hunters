using System;
using System.Collections.Generic;

namespace Code.Scripts.Src
{
    public class Unsubscriber<T> : IDisposable
    {
        private readonly List<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }

    public interface IDisposable
    {
        public void Dispose();
    }

    public interface IObservable<ObservedInfo>
    {
        public IDisposable Subscribe(IObserver<ObservedInfo> observer);
    }

    public interface IObserver<ObservedInfo>
    {
        public void OnNext(ObservedInfo info);
        public void OnCompleted() { }
        public void OnError(Exception e) { }
    }
}
