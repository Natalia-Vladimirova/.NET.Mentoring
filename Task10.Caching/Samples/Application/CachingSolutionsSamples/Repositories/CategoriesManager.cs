using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Cache;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Repositories
{
	public class CategoriesManager : IRepository<Category>
	{
		private readonly ICache<Category> _cache;

		public CategoriesManager(ICache<Category> cache)
		{
			_cache = cache;
		}

		public IEnumerable<Category> GetAll()
		{
			Console.WriteLine("Get Categories");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var categories = _cache.Get(user);

			if (categories == null)
			{
				Console.WriteLine("Load categories from DB");

				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;
					categories = dbContext.Categories.ToList();
					_cache.Set(user, categories);
				}
			}

			return categories;
		}
	}
}
