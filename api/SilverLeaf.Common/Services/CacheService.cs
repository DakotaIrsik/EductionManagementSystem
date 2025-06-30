
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SilverLeaf.common.Services;
//using StackExchange.Redis;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SilverLeaf.common.Services
{
    public interface ICacheService
    {
        T Get<T>(string key);
        void Set(string key, object data, TimeSpan? cacheTime = null);
        bool IsSet(string key);
        void Remove(string key);
    }

    //TODO Fix for new net core 3 caching.
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            var obj = Activator.CreateInstance(typeof(T));
            _cache.TryGetValue(key, out obj);
            return (T)obj;
        }

        public bool IsSet(string key)
        {
            var t = new object();
            return _cache.TryGetValue(key, out t);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Set(string key, object data, TimeSpan? cacheTime)
        {
            using (var entry = _cache.CreateEntry(key))
            {
                entry.Value = data;

                _cache.Set(key, data, cacheTime ?? TimeSpan.MinValue);
            }
        }
    }

    public class NoCacheService : ICacheService
    {
        public T Get<T>(string key)
        {
            return default(T);
        }

        public bool IsSet(string key)
        {
            return false;
        }

        public void Remove(string key)
        {
        }

        public void Set(string key, object data, TimeSpan? cacheTime)
        {
        }
    }

    //public class RedisCacheService : ICacheService
    //{
    //    private static ConnectionMultiplexer _redis;
    //    private static IDatabase _cache;

    //    public RedisCacheService(string csvRedisEndpoints)
    //    {
    //        var options = ConfigurationOptions.Parse(csvRedisEndpoints);
    //        var connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
    //        _redis = connection.Value;
    //        _cache = _redis.GetDatabase();
    //    }

    //    public T Get<T>(string key)
    //    {
    //        var data = _cache.StringGet(key);
    //        return Deserialize<T>(data);
    //    }

    //    public void Set(string key, object data, TimeSpan? cacheTime)
    //    {
    //        if (_redis != null)
    //        {
    //            _cache.StringSet(key, Serialize(data), cacheTime);
    //        }
    //    }

    //    public bool IsSet(string key)
    //    {
    //        return _cache.KeyExists(key);
    //    }

    //    public void Remove(string key)
    //    {
    //        if (_redis != null)
    //        {
    //            _cache.KeyDelete(key);
    //        }
    //    }

    //    private static string Serialize(object o)
    //    {
    //        if (o == null)
    //        {
    //            return null;
    //        }

    //        return JsonConvert.SerializeObject(o);
    //    }

    //    private static T Deserialize<T>(string data)
    //    {
    //        if (data == null)
    //        {
    //            return default(T);
    //        }

    //        return JsonConvert.DeserializeObject<T>(data);
    //    }
    //}
}

namespace CacheExtensions
{
    public static class CacheExtensions
    {
        public static T GetOrSet<T>(this ICacheService cacheService, string cacheKeyName, object dataToBeCached, TimeSpan? cacheTime = null)
        {
            if (cacheService.IsSet(cacheKeyName))
            {
                return cacheService.Get<T>(cacheKeyName);
            }

            cacheService.Set(cacheKeyName, dataToBeCached, cacheTime);
            return (T)dataToBeCached;
        }

        public static T GetOrSet<T>(this ICacheService cacheService, string cacheKeyName, Func<T> acquire, TimeSpan? cacheTime = null)
        {
            if (cacheService.IsSet(cacheKeyName))
            {
                return cacheService.Get<T>(cacheKeyName);
            }

            var result = acquire();
            cacheService.Set(cacheKeyName, result, cacheTime);
            return result;
        }

        public static T GetOrSet<T>(this ICacheService cacheService, object keyGenerationObj, Func<T> acquire, TimeSpan? cacheTime = null)
        {
            var key = keyGenerationObj.GenerateKey();
            if (cacheService.IsSet(key))
            {
                return cacheService.Get<T>(key);
            }

            var result = acquire();
            cacheService.Set(key, result, cacheTime);
            return result;
        }

        public static async Task<T> GetOrSetAsync<T>(this ICacheService cacheService, Func<Task<T>> acquire, object keyGenerationObj, TimeSpan? cacheTime = null) where T : class
        {
            var key = keyGenerationObj.GenerateKey();
            if (cacheService.IsSet(key))
            {
                return cacheService.Get<T>(key);
            }

            var result = await acquire().ConfigureAwait(false);
            cacheService.Set(key, result, cacheTime);
            return result;
        }

        public static async Task<T> GetOrSetAsync<T>(this ICacheService cacheService, string cacheKeyName, Func<Task<T>> acquire, TimeSpan? cacheTime = null) where T : class
        {
            if (cacheService.IsSet(cacheKeyName))
            {
                return cacheService.Get<T>(cacheKeyName);
            }

            var result = await acquire().ConfigureAwait(false);
            cacheService.Set(cacheKeyName, result, cacheTime);
            return result;
        }

        public static string GenerateKey(this object keyGenerationObject)
        {
            var key = string.Empty;
            var thisClassProperties = keyGenerationObject.GetType().GetProperties
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var prop in thisClassProperties)
            {
                key += prop.Name + "=" + JsonConvert.SerializeObject(prop.GetValue(keyGenerationObject, null));
            }

            return key;
        }
    }
}

