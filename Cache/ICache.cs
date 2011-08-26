using System;
using System.Web.Caching;

namespace GameTools
{
    interface ICache
    {
        void Insert(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback callback);
        void Remove(string key);
    }
}
