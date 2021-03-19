using System.Collections.Generic;
using System.Linq;

namespace RocketAnt.Function
{
    public static class Extensions
    {
        public static IAsyncEnumerable<T> AsAsyncQueryable<T>(this IQueryable<T> queryable)
        {
            return (IAsyncEnumerable<T>)queryable;
        }
    }
}