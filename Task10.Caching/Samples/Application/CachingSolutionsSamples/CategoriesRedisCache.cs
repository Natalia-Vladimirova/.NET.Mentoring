using System.Collections.Generic;
using NorthwindLibrary;
using StackExchange.Redis;
using System.IO;
using System.Runtime.Serialization;

namespace CachingSolutionsSamples
{
    public class CategoriesRedisCache : ICategoriesCache
	{
        private readonly string _prefix = "Cache_Categories";
		private readonly ConnectionMultiplexer _redisConnection;
        private readonly DataContractSerializer _serializer;

		public CategoriesRedisCache(string hostName)
		{
			_redisConnection = ConnectionMultiplexer.Connect(hostName);
            _serializer = new DataContractSerializer(typeof(IEnumerable<Category>));
        }

		public IEnumerable<Category> Get(string forUser)
		{
			var db = _redisConnection.GetDatabase();
			byte[] value = db.StringGet(_prefix + forUser);

			return value == null
                ? null
                : (IEnumerable<Category>)_serializer.ReadObject(new MemoryStream(value));
		}

		public void Set(string forUser, IEnumerable<Category> categories)
		{
			var db = _redisConnection.GetDatabase();
			var key = _prefix + forUser;

			if (categories == null)
			{
				db.StringSet(key, RedisValue.Null);
			}
			else
			{
				var stream = new MemoryStream();
				_serializer.WriteObject(stream, categories);
				db.StringSet(key, stream.ToArray());
			}
		}
	}
}
