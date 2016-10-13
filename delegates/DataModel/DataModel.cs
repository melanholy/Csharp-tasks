using System;
using System.Collections.Generic;

namespace DataModel
{
    public class DataModel : IObservable
    {
        public event Action StateChanged;
        private readonly List<IObserver> Observers;

        public DataModel()
        {
            Observers = new List<IObserver>();
        }

        public void AddObserver(IObserver observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            Observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in Observers)
                observer.Update();
        }

        public void Put(int row, int column, int value)
        {

            NotifyObservers();
            //StateChanged?.Invoke();
        }

        public void InsertRow(int rowIndex)
        {

            NotifyObservers();
            //StateChanged?.Invoke();
        }

        public void InsertColumn(int columnIndex)
        {

            NotifyObservers();
            //StateChanged?.Invoke();
        }

        public void Get(int row, int column)
        {
            
        }
    }
}