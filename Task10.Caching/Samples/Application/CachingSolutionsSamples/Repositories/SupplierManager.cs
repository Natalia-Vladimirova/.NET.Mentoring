using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using CachingSolutionsSamples.Cache;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Repositories
{
    public class SupplierManager : IRepository<Supplier>
    {
        private readonly ICacheWithPolicy<Supplier> _cache;
        private readonly int _expirationInSeconds;

        public SupplierManager(ICacheWithPolicy<Supplier> cache, int expirationInSeconds)
        {
            _cache = cache;
            _expirationInSeconds = expirationInSeconds;
        }

        public IEnumerable<Supplier> GetAll()
        {
            Console.WriteLine("Get Suppliers");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var suppliers = _cache.Get(user);

            if (suppliers == null)
            {
                Console.WriteLine("Load suppliers from DB");

                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    suppliers = dbContext.Suppliers.ToList();

                    var policy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddSeconds(_expirationInSeconds))
                    };

                    _cache.Set(user, suppliers, policy);
                }
            }

            return suppliers;
        }
    }
}
