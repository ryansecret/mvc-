using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace Hsr.Core.Cache
{
    public class RedisCacheManager:ICacheManager
    {
        private readonly IRedisClient _client;
        public RedisCacheManager()
        {
            PooledRedisClientManager redisClientManager;
            //todo:可以添加readonly hosts
            var redisConfig = ConfigurationManager.AppSettings["redisHosts"];
            if (!string.IsNullOrWhiteSpace(redisConfig))
            {
                redisClientManager = new PooledRedisClientManager(redisConfig.Split(new []{','}));
            }
            else
            {
                redisClientManager = new PooledRedisClientManager();
            }
            _client = redisClientManager.GetClient();
        }
        public T Get<T>(string key)
        {
            return _client.Get<T>(key);
        }

        public void Set(string key, object data, int cacheTime)
        {
            _client.Set(key, data, DateTime.Now.AddMinutes(cacheTime));
        }

        public bool IsSet(string key)
        {
           return  _client.ContainsKey(key);
        }

        public void Remove(string key)
        {
            _client.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            _client.RemoveAll(_client.SearchKeys(pattern));
        }

        public void Clear()
        {
            _client.FlushAll();
        }
    }
}
