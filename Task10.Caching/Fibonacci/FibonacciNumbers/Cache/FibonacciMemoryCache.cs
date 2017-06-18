using System.Collections.Generic;
using System.Runtime.Caching;

namespace FibonacciNumbers.Cache
{
    public class FibonacciMemoryCache : ICache
    {
        private readonly string _prefix = "FibonacciMemoryCache";
        private readonly ObjectCache _cache = MemoryCache.Default;

        public IEnumerable<int> Get(string forUser)
        {
            return (IEnumerable<int>)_cache.Get(_prefix + forUser);
        }

        public void Set(string forUser, IEnumerable<int> numbers)
        {
            _cache.Set(_prefix + forUser, numbers, ObjectCache.InfiniteAbsoluteExpiration);
        }
    }
}
