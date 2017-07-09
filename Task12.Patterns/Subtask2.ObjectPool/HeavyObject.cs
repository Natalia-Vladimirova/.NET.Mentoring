using System;
using Subtask2.ObjectPool.Interfaces;

namespace Subtask2.ObjectPool
{
    public class HeavyObject : IPoolable
    {
        public HeavyObject()
        {
            Console.WriteLine("Initializing heavy object");
        }

        public void Reset()
        {
            Console.WriteLine("Resetting state of object");
        }
    }
}
