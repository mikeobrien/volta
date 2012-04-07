using System;
using System.Collections.Generic;
using System.Linq;

namespace Volta.Core.Infrastructure.Framework
{
    public static class EnumerableExtensions
    {
        private static readonly Func<int, int, int> PagesToSkip = (index, size) => (Math.Max(index, 1) - 1) * size; 

        public static IQueryable<T> Page<T>(this IQueryable<T> items, int pageIndex, int pageSize)
        {
            return items.Skip(PagesToSkip(pageIndex, pageSize)).Take(pageSize);
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> items, int pageIndex, int pageSize)
        {
            return items.Skip(PagesToSkip(pageIndex, pageSize)).Take(pageSize);
        }
    }
}