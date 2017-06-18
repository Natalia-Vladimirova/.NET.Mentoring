using System;
using System.Linq;
using FibonacciNumbers;
using FibonacciNumbers.Cache;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1 - In-process cache; 2 - Out-of-process cache; other - exit");
                Console.Write("Enter a key: ");
                var key = Console.ReadLine();
                Console.WriteLine();

                switch (key)
                {
                    case "1":
                        MemoryCache();
                        break;
                    case "2":
                        RedisCache();
                        break;
                    default:
                        return;
                }

                Console.WriteLine();
            }
        }

        private static void MemoryCache()
        {
            var generator = new CachedFibonacciGenerator(new FibonacciGenerator(), new FibonacciMemoryCache());
            generator.Get(10).ToList().ForEach(Console.WriteLine);
        }

        private static void RedisCache()
        {
            var generator = new CachedFibonacciGenerator(new FibonacciGenerator(), new FibonacciRedisCache("localhost"));
            generator.Get(10).ToList().ForEach(Console.WriteLine);
        }
    }
}
