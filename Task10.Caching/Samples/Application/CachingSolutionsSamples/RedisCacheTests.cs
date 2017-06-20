using System;
using System.Linq;
using System.Threading;
using Autofac;
using CachingSolutionsSamples.DI;
using CachingSolutionsSamples.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;

namespace CachingSolutionsSamples
{
	[TestClass]
    public class RedisCacheTests
    {
        private readonly IContainer _container;

        public RedisCacheTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AutofacModule>();
            _container = builder.Build();
        }
        
        [TestMethod]
        public void RedisRegionCache()
        {
            var manager = _container.ResolveNamed<IRepository<Region>>("RedisRegionManager");

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(manager.GetAll().Count());
                Thread.Sleep(100);
            }
        }
    }
}
