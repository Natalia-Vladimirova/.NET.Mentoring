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
            builder.RegisterType<MemoryCache<Category>>()
                .Named<ICache<Category>>("CategoryMemoryCache");

            builder.RegisterType<RedisCache<Category>>()
                .WithParameter("hostName", "localhost")
                .Named<ICache<Category>>("CategoryRedisCache");

            builder.RegisterType<CategoriesManager>()
                .WithParameter(new ResolvedParameter(
                            (info, context) => info.Name == "cache",
                            (info, context) => context.ResolveNamed<ICache<Category>>("CategoryMemoryCache")))
                .Named<IRepository<Category>>("MemoryCategoriesManager");

            builder.RegisterType<CategoriesManager>()
                .WithParameter(new ResolvedParameter(
                            (info, context) => info.Name == "cache",
                            (info, context) => context.ResolveNamed<ICache<Category>>("CategoryRedisCache")))
                .Named<IRepository<Category>>("RedisCategoriesManager");
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
            builder.RegisterType<MemoryCache<Supplier>>()
                .Named<ICache<Supplier>>("SupplierMemoryCache");

            builder.RegisterType<RedisCache<Supplier>>()
                .WithParameter("hostName", "localhost")
                .Named<ICache<Supplier>>("SupplierRedisCache");

            builder.RegisterType<SupplierManager>()
                .WithParameter(new ResolvedParameter(
                            (info, context) => info.Name == "cache",
                            (info, context) => context.ResolveNamed<ICache<Supplier>>("SupplierMemoryCache")))
                .Named<IRepository<Supplier>>("MemorySupplierManager");

            builder.RegisterType<SupplierManager>()
                .WithParameter(new ResolvedParameter(
                            (info, context) => info.Name == "cache",
                            (info, context) => context.ResolveNamed<ICache<Supplier>>("SupplierRedisCache")))
                .Named<IRepository<Supplier>>("RedisSupplierManager");
        }
    }
}
