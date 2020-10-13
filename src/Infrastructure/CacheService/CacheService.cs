using System;
using System.Collections.Generic;
using System.Linq;
using Application.Interfaces.CacheService;
using Domain.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Peddle.Foundation.CacheManager.Caching.Memory;
using Peddle.Foundation.CacheManager.Core;
using Peddle.Foundation.CacheManager.Redis;

namespace Infrastructure.ExternalServices
{
    public class CacheService<T> : ICacheService<T> where T : class
    {
        private readonly ILogger<CacheService<T>> _log;
        private readonly string _region = "OfferOperationsService";
        private readonly CacheServiceConfiguration _configurationSettings;

        private ICacheManager<T> _cacheManager;

        public CacheService(IOptions<CacheServiceConfiguration> configuration, ILogger<CacheService<T>> log)
        {
            _configurationSettings = configuration.Value;
            _log = log;
            InitializeCacheManager();
        }
        private void InitializeCacheManager()
        {
            _cacheManager = _cacheManager ?? (_cacheManager = CacheFactory.Build<T>(settings => settings
                .WithMemoryCacheHandle()
                .And
                .WithRedisCacheHandle(_configurationSettings.RedisConnectionString)));
        }
        public List<T> GetItems(List<string> keys)
        {
            IList<CacheItem<T>> cacheItems = new List<CacheItem<T>>();
            try
            {
                cacheItems = _cacheManager.Get(keys, _region);
            }
            catch (Exception exception)
            {
                _log.LogWarning(exception, string.Join(",", keys.ToArray()), "Redis failed to Get the keys");
            }

            return cacheItems.Select(x => x.Value).ToList();
        }


        public T GetItem(string key)
        {
            CacheItem<T> cacheItem = null;
            try
            {
                cacheItem = _cacheManager.Get(key, _region);
            }
            catch (Exception exception)
            {
                _log.LogWarning(exception, null, "Redis failed to Get the key with key name: " + key);
            }

            return cacheItem == null ? default(T) : cacheItem.Value;
        }

        public void UpsertItems(List<CacheItem<T>> cacheItems)
        {
            try
            {
                _cacheManager.Put(cacheItems, _region);
            }
            catch (Exception exception)
            {
                _log.LogWarning(exception, string.Join(",", cacheItems.Select(x => x.Key)),
                    "Redis failed to upsert the keys");
            }
        }

        public void UpsertItem(string key, T value, int? expireInMinute = null)
        {
            try
            {
                _cacheManager.Put(new CacheItem<T>(key, value, _region, expireInMinute));
            }
            catch (Exception exception)
            {
                _log.LogWarning(exception, null, "Redis failed to upsert the key with key name: " + key);
            }
        }

        public void RemoveItem(string key)
        {
            try
            {
                _cacheManager.Remove(key, _region);
            }
            catch (Exception exception)
            {
                _log.LogWarning(exception, null, "Redis failed to remove the key " + key);
            }
        }
    }
}