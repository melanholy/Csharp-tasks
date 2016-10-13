using System;
using System.Collections.Generic;
using System.Linq;

namespace IDStore
{
    internal class Storage
    {
        private readonly Dictionary<Type, ISource<object>> Sources;
        private readonly Dictionary<Type, ITuple<object>[]> Receivers;
        private int Guid;
        private int Capacity;

        public Storage()
        {
            Guid = 0;
            Capacity = 128;
            Sources = new Dictionary<Type, ISource<object>>();
            Receivers = new Dictionary<Type, ITuple<object>[]>();
        }

        private void Enlarge()
        {
            var keys = Receivers.Keys;
            for (var i = 0; i < keys.Count; i++)
            {
                var arr = Receivers[keys.ElementAt(i)];
                if (arr.Length <= Capacity)
                    Array.Resize(ref arr, Capacity * 2);
                Receivers[keys.ElementAt(i)] = arr;
            }
            Capacity *= 2;
        }

        public T AddObject<T>()
            where T : class, new()
        {
            var obj = new T();
            if (!Sources.ContainsKey(obj.GetType()))
            {
                var storage = new ITuple<T>[Capacity];
                Sources.Add(obj.GetType(), new Source<T>(storage));
                Receivers.Add(obj.GetType(), storage);
            }
            if (Guid == Capacity)
                Enlarge();
            Receivers[obj.GetType()][Guid] = new MyTuple<T>(Guid, obj);
            Guid++;
            return obj;
        }

        public ITuple<T>[] GetObjectsByClass<T>()
        {
            return Sources.ContainsKey(typeof(T)) ? ((ISource<T>)Sources[typeof(T)]).GetAll() : new ITuple<T>[0];
        }

        public T GetObjectById<T>(int guid)
        {
            return (T)Sources[typeof(T)].GetById(guid);
        }
    }
}
