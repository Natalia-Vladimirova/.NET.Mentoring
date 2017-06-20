using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using Autofac;
using CachingSolutionsSamples.DI;
using CachingSolutionsSamples.Repositories;
using NorthwindLibrary;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class MemoryCacheTests
	{
	    private readonly IContainer _container;

	    public MemoryCacheTests()
	    {
	        var builder = new ContainerBuilder();
	        builder.RegisterModule<AutofacModule>();
	        _container = builder.Build();
	    }

        [TestMethod]
		public void MemoryCategoriesCache()
		{
			var manager = _container.ResolveNamed<IRepository<Category>>("MemoryCategoriesManager");

            for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(manager.GetAll().Count());
				Thread.Sleep(500);
			}
        }

        [TestMethod]
        public void MemorySupplierCache()
        {
            var manager = _container.ResolveNamed<IRepository<Supplier>>("MemorySupplierManager");

            for (var i = 0; i < 50; i++)
            {
                Console.WriteLine(manager.GetAll().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void MemoryRegionCache()
        {
            var manager = _container.ResolveNamed<IRepository<Region>>("MemoryRegionManager");

            for (var i = 0; i < 50; i++)
            {
                Console.WriteLine(manager.GetAll().Count());
                Thread.Sleep(100);
            }
        }
    }
}
