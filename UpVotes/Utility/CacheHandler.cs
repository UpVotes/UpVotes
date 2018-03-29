using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UpVotes.Utility
{    
    public static class CacheHandler
    {
        public static void Add<T>(T objInfo, string key)
        {
            HttpContext.Current.Cache.Insert(key, objInfo, null, DateTime.Now.AddMinutes(1000), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        public static void Clear(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }
        public static bool Exists(string key)
        {
            return HttpContext.Current.Cache[key] != null;
        }
        public static bool Get<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value =
                        default(T);
                    return false;
                }
                value = (T)HttpContext.Current.Cache[key];
            }
            catch
            {
                value =
                    default(T);
                return false;
            }
            return true;
        }
    }
}