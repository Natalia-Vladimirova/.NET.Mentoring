using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using StackExchange.Redis;

namespace CachingSolutionsSamples.Cache
{
    public class RedisCache<T> : ICache<T>
    {
        private readonly string _prefix = $"Cache_{typeof(T)}";
		private readonly ConnectionMultiplexer _redisConnection;
        private readonly DataContractSerializer _serializer;

		public RedisCache(string hostName)
		{
			_redisConnection = ConnectionMultiplexer.Connect(hostName);
            _serializer = new DataContractSerializer(typeof(IEnumerable<T>));
        }

		public IEnumerable<T> Get(string forUser)
		{
			var db = _redisConnection.GetDatabase();
			byte[] value = db.StringGet(_prefix + forUser);

			return value == null
                ? null
                : (IEnumerable<T>)_serializer.ReadObject(new MemoryStream(value));
		}

		public void Set(string forUser, IEnumerable<T> entities)
		{
			var db = _redisConnection.GetDatabase();
			var key = _prefix + forUser;

			if (entities == null)
			{
				db.StringSet(key, RedisValue.Null);
			}
			else
			{
				var stream = new MemoryStream();
				_serializer.WriteObject(stream, entities);
				db.StringSet(key, stream.ToArray());
			}
		}
	}
}
