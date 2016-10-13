using System;

namespace DataModel
{
    public class MethodTableVis : IObserver, IDisposable
    {
        private readonly DataModel Model;

        public MethodTableVis()
        {
            Model = new DataModel();
            Model.AddObserver(this);
        }

        public void Update() { }

        public void Dispose()
        {
            Model.RemoveObserver(this);
        }
    }
}