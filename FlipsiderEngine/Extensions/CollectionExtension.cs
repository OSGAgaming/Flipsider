using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flipsider.Extensions
{
    public static class CollectionExtension
    {
        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dict, Predicate<KeyValuePair<TKey, TValue>> match) where TKey : notnull
        {
            var toRemove = from d in dict
                           where match(d)
                           select d.Key;
            foreach (var item in toRemove)
            {
                dict.Remove(item);
            }
        }
    }
}
