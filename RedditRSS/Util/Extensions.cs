using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RedditRSS.Util
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
                action(item);
        }
        public static void ForEach<T>(this IQueryable<T> queryable, Action<T> action)
        {
            foreach (T item in queryable)
                action(item);
        }
    }
}
