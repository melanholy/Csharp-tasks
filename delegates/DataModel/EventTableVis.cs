using System;

namespace DataModel
{
    public class EventTableVis : IDisposable
    {
        private readonly DataModel Model;

        public EventTableVis()
        {
            Model = new DataModel();
            Model.StateChanged += Update;
        }

        public void Update()
        {
            
        }

        public void Dispose()
        {
            Model.StateChanged -= Update;
        }
    }
}