using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using StackExchange.Redis;

namespace FibonacciNumbers.Cache
{
    public class FibonacciRedisCache : ICache
    {
        private readonly string _prefix = "FibonacciRedisCache";
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly DataContractSerializer _serializer;

        public FibonacciRedisCache(string hostName)
        {
            _redisConnection = ConnectionMultiplexer.Connect(hostName);
            _serializer = new DataContractSerializer(typeof(IEnumerable<int>));
        }

        public IEnumerable<int> Get(string forUser)
        {
            var db = _redisConnection.GetDatabase();
            byte[] value = db.StringGet(_prefix + forUser);
            
            return value == null
                ? null
                : (IEnumerable<int>)_serializer.ReadObject(new MemoryStream(value));
        }

        public void Set(string forUser, IEnumerable<int> numbers)
        {
            var db = _redisConnection.GetDatabase();
            var key = _prefix + forUser;

            if (numbers == null)
            {
                db.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                _serializer.WriteObject(stream, numbers);
                db.StringSet(key, stream.ToArray());
            }
        }
    }
}
