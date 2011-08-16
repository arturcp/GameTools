using System;
using System.Web;
using System.Web.Caching;

namespace GameMotor
{
    public class AspnetCache: ICache
    {
        #region ICache Members

        public void Insert(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback callback)
        {
            HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, slidingExpiration, priority, callback);
        }

        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);            
        }

        #endregion
    }
}
