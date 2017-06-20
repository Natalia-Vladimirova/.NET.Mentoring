using System.Configuration;
using Autofac;
using Autofac.Core;
using CachingSolutionsSamples.Cache;
using CachingSolutionsSamples.Repositories;
using NorthwindLibrary;

namespace CachingSolutionsSamples.DI
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            LoadCategoryInstances(builder);
            LoadRegionInstances(builder);
            LoadSupplierInstances(builder);
        }

        private void LoadCategoryInstances(ContainerBuilder builder)
        {
            builder.RegisterType<MemoryCacheWithPolicy<Category>>()
                .Named<ICacheWithPolicy<Category>>("CategoryMemoryCache");
            
            builder.RegisterType<CategoriesManager>()
                .WithParameter(new ResolvedParameter(
                            (info, context) => info.Name == "cache",
                            (info, context) => context.ResolveNamed<ICacheWithPolicy<Category>>("CategoryMemoryCache")))
                .WithParameter("connectionString", ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString)
                .Named<IRepository<Category>>("MemoryCategoriesManager");
        }

        private void LoadRegionInstances(ContainerBuilder builder)
        {
            builder.RegisterType<MemoryCache<Region>>()
                .Named<ICache<Region>>("RegionMemoryCache");

            builder.RegisterType<RedisCache<Region>>()
                .WithParameter("hostName", "localhost")
                .Named<ICache<Region>>("RegionRedisCache");

            builder.RegisterType<RegionManager>()
                .WithParameter(new ResolvedParameter(
                            (info, context) => info.Name == "cache",
                            (info, context) => context.ResolveNamed<ICache<Region>>("RegionMemoryCache")))
                .Named<IRepository<Region>>("MemoryRegionManager");

            builder.RegisterType<RegionManager>()
                .WithParameter(new ResolvedParameter(
                            (info, context) => info.Name == "cache",
                            (info, context) => context.ResolveNamed<ICache<Region>>("RegionRedisCache")))
                .Named<IRepository<Region>>("RedisRegionManager");
        }

        private void LoadSupplierInstances(ContainerBuilder builder)
        {
            builder.RegisterType<MemoryCacheWithPolicy<Supplier>>()
                .Named<ICacheWithPolicy<Supplier>>("SupplierMemoryCache");
            
            builder.RegisterType<SupplierManager>()
                .WithParameter(new ResolvedParameter(
                            (info, context) => info.Name == "cache",
                            (info, context) => context.ResolveNamed<ICacheWithPolicy<Supplier>>("SupplierMemoryCache")))
                .WithParameter("expirationInSeconds", 2)
                .Named<IRepository<Supplier>>("MemorySupplierManager");
        }
    }
}
