using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace Haxgo.Core.Caching
{
    public class CacheManager
    {
        public static void Insert(string Name, object Obj)
        {
            if ((Obj != null))
            {
                Cache Cache = HttpRuntime.Cache;
                if (Cache[Name] == null)
                {
                    Cache.Insert(Name, Obj);
                }
            }
        }
        public static void Insert(string Name, object Obj, double TimeOut, CacheItemPriority Priority)
        {
            Insert(Name, Obj, null, TimeOut, TimeSpan.Zero, Priority, null);
        }

        public static void Insert(string Key, object Obj, CacheDependency Dependency, double TimeOut, TimeSpan SlidingExpiration, CacheItemPriority Priority, CacheItemRemovedCallback RemovedCallback)
        {
            if ((Obj != null))
            {
                Cache Cache = HttpRuntime.Cache;
                if (Cache[Key] == null)
                {
                    Cache.Insert(Key, RuntimeHelpers.GetObjectValue(Obj), Dependency, DateTime.Now.AddSeconds(TimeOut), SlidingExpiration, Priority, RemovedCallback);
                }
            }
        }

        public static object Get(string Key)
        {
            Cache Cache = HttpRuntime.Cache;
            return Cache[Key];
        }

        public static void Remove(string Key)
        {
            Cache Cache = HttpRuntime.Cache;
            if (Cache[Key] != null)
            {
                Cache.Remove(Key);
            }
        }

        public static void RemoveByPattern(string pattern)
        {
            Cache Cache = HttpRuntime.Cache;
            IDictionaryEnumerator enumerator = Cache.GetEnumerator();
            Regex rgx = new Regex(pattern, (RegexOptions.Singleline | (RegexOptions.Compiled | RegexOptions.IgnoreCase)));
            while (enumerator.MoveNext())
            {
                if (rgx.IsMatch(enumerator.Key.ToString()))
                {
                    Cache.Remove(enumerator.Key.ToString());
                }
            }
        }

        public static void Clear()
        {
            Cache Cache = HttpRuntime.Cache;
            IDictionaryEnumerator enumerator = Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Cache.Remove(enumerator.Key.ToString());
            }
        }
    }
}
