using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Cache;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Repositories
{
    public class RegionManager : IRepository<Region>
    {
        private readonly ICache<Region> _cache;

        public RegionManager(ICache<Region> cache)
        {
            _cache = cache;
        }

        public IEnumerable<Region> GetAll()
        {
            Console.WriteLine("Get Regions");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var regions = _cache.Get(user);

            if (regions == null)
            {
                Console.WriteLine("Load regions from DB");

                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    regions = dbContext.Regions.ToList();
                    _cache.Set(user, regions);
                }
            }

            return regions;
        }
    }
}
