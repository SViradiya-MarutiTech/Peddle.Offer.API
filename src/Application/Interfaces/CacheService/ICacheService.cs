using System.Collections.Generic;
using Peddle.Foundation.CacheManager.Core;

namespace Application.Interfaces.CacheService
{
    public interface ICacheService<T>
    {
        List<T> GetItems(List<string> keys);
        T GetItem(string key);
        void UpsertItems(List<CacheItem<T>> cacheItems);
        void UpsertItem(string key, T value, int? expireInMinute = null);
        void RemoveItem(string key);
    }
}