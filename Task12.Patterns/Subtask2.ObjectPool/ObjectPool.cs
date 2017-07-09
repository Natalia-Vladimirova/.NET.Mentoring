using System.Collections.Concurrent;
using Subtask2.ObjectPool.Interfaces;

namespace Subtask2.ObjectPool
{
    // Imagine that the problem mentioned in the task is that the creation of an object is a very expensive operation.

    // Object pool is a creational design pattern.
    // It is used in the cases when:
    //    1. the creation and / or destruction of an object is a very costly operation;
    //    2. there is a limited number of objects of the type stored in the Pool in the system;
    //    3. objects are often created and destroyed.

    public class ObjectPool<T> : IObjectPool<T> 
        where T : class, IPoolable
    {
        private readonly ConcurrentBag<T> _container = new ConcurrentBag<T>();
        private readonly IPoolObjectCreator<T> _objectCreator;

        public int Count => _container.Count;

        public ObjectPool(IPoolObjectCreator<T> objectCreator)
        {
            _objectCreator = objectCreator;
        }

        public T GetObject()
        {
            T obj;
            
            return _container.TryTake(out obj) 
                ? obj 
                : _objectCreator.Create();
        }

        public void ReturnObject(ref T obj)
        {
            obj.Reset();
            _container.Add(obj);
            obj = null;
        }
    }
}
