using System;
using Subtask2.ObjectPool.Interfaces;

namespace Subtask2.ObjectPool.App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IObjectPool<HeavyObject> pool = new ObjectPool<HeavyObject>(new HeavyObjectCreator());

            PrintCount(pool);

            var firstObject = pool.GetObject();
            var secondObject = pool.GetObject();

            PrintCount(pool);

            pool.ReturnObject(ref firstObject);

            PrintCount(pool);

            var thirdObject = pool.GetObject();

            pool.ReturnObject(ref secondObject);
            pool.ReturnObject(ref thirdObject);

            PrintCount(pool);
            
            Console.WriteLine("Press any key to exit ...");
            Console.ReadLine();
        }

        private static void PrintCount(IObjectPool<HeavyObject> pool)
        {
            Console.WriteLine($"Count: {pool.Count}");
        }
    }
}
