using System;

namespace DataModel
{
    public interface IObservable
    {
        event Action StateChanged;
        void AddObserver(IObserver obj);
        void RemoveObserver(IObserver obj);
    }
}