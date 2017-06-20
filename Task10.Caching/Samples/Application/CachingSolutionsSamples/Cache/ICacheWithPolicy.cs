using System.Collections.Generic;
using System.Runtime.Caching;

namespace CachingSolutionsSamples.Cache
{
    public interface ICacheWithPolicy<T>
    {
        IEnumerable<T> Get(string forUser);

        void Set(string forUser, IEnumerable<T> entities, CacheItemPolicy policy = null);
    }
}
