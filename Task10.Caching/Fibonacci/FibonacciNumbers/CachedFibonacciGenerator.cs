using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FibonacciNumbers.Cache;

namespace FibonacciNumbers
{
    public class CachedFibonacciGenerator : IFibonacciGenerator
    {
        private readonly IFibonacciGenerator _generator;
        private readonly ICache _cache;

        public CachedFibonacciGenerator(IFibonacciGenerator generator, ICache cache)
        {
            _generator = generator;
            _cache = cache;
        }

        public IEnumerable<int> Get(int count)
        {
            Console.WriteLine("Get Fibonacci Numbers");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var numbers = _cache.Get(user);

            if (numbers == null)
            {
                Console.WriteLine("Calculating Fibonacci Numbers");

                numbers = _generator.Get(count);
                _cache.Set(user, numbers.ToList());
            }

            return numbers;
        }
    }
}
