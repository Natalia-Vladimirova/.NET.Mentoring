using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Cache;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Repositories
{
    public class SupplierManager : IRepository<Supplier>
    {
        private readonly ICache<Supplier> _cache;

        public SupplierManager(ICache<Supplier> cache)
        {
            _cache = cache;
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
                    _cache.Set(user, suppliers);
                }
            }

            return suppliers;
        }
    }
}
