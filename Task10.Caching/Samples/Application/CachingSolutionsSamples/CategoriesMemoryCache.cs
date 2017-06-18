using System.Collections.Generic;
using NorthwindLibrary;
using System.Runtime.Caching;

namespace CachingSolutionsSamples
{
    public class CategoriesMemoryCache : ICategoriesCache
	{
        private readonly string _prefix  = "Cache_Categories";
		private readonly ObjectCache _cache = MemoryCache.Default;

		public IEnumerable<Category> Get(string forUser)
		{
			return (IEnumerable<Category>) _cache.Get(_prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<Category> categories)
		{
			_cache.Set(_prefix + forUser, categories, ObjectCache.InfiniteAbsoluteExpiration);
		}
	}
}
