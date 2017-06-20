using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using CachingSolutionsSamples.Cache;
using Dapper;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Repositories
{
	public class CategoriesManager : IRepository<Category>
	{
		private readonly ICacheWithPolicy<Category> _cache;
	    private readonly string _cacheKey;
        private CacheItemPolicy _policy;

	    private readonly string _commandText = "SELECT [CategoryID], [CategoryName] FROM [dbo].[Categories];";
	    private readonly SqlConnection _сonnection;

	    private IEnumerable<Category> _categories;

        public CategoriesManager(ICacheWithPolicy<Category> cache, string connectionString)
		{
			_cache = cache;
            _cacheKey = Thread.CurrentPrincipal.Identity.Name;
            
            SqlDependency.Start(connectionString);

            _сonnection = new SqlConnection(connectionString);
            _сonnection.Open();

            Setup();
		}

        public IEnumerable<Category> GetAll()
		{
			Console.WriteLine("Get Categories");
			_categories = _cache.Get(_cacheKey) ?? GetData();

            return _categories;
		}

	    private void OnChange(object sender, SqlNotificationEventArgs e)
	    {
            SqlDependency oldDependency = (SqlDependency)sender;
            oldDependency.OnChange -= OnChange;

            Setup();

	        _categories = GetData();
	    }

	    private void Setup()
	    {
            var command = new SqlCommand(_commandText, _сonnection);
            command.CommandType = CommandType.Text;
            command.Notification = null;

            var dependency = new SqlDependency(command);
            dependency.OnChange += OnChange;

            _policy = new CacheItemPolicy();
            _policy.ChangeMonitors.Add(new SqlChangeMonitor(dependency));
            
            command.ExecuteReader(CommandBehavior.CloseConnection);
	    }

	    private IEnumerable<Category> GetData()
	    {
            Console.WriteLine("Load categories from DB");
            var categories = _сonnection.Query<Category>(_commandText);
            _cache.Set(_cacheKey, categories.ToList(), _policy);

	        return categories;
	    }
    }
}
