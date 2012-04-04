using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Volta.Core.Infrastructure.Framework
{
    public class Func
    {
        public static Func<TArgument, TResult> Memoize<TArgument, TResult>(Func<TArgument, TResult> function)
        {
            var results = new ConcurrentDictionary<TArgument, TResult>();
            return arg =>
            {
                TResult result;
                if (results.TryGetValue(arg, out result)) return result;
                result = function(arg);
                results.TryAdd(arg, result);
                return result;
            };
        }

        public static Action Lazy(bool executeOnlyOnce, Action command)
        {
            var runFlag = 0;
            return () => { if (!executeOnlyOnce || Interlocked.CompareExchange(ref runFlag, 1, 0) == 0) command(); };
        }
    }
}