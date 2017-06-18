using System.Collections.Generic;
using System.Runtime.Caching;

namespace CachingSolutionsSamples.Cache
{
    public class MemoryCache<T> : ICache<T>
	{
        private readonly string _prefix  = $"Cache_{typeof(T)}";
		private readonly ObjectCache _cache = MemoryCache.Default;

		public IEnumerable<T> Get(string forUser)
		{
		    return (IEnumerable<T>) _cache.Get(_prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<T> entities)
		{
			_cache.Set(_prefix + forUser, entities, ObjectCache.InfiniteAbsoluteExpiration);
		}
	}
}
