using System.Linq;

namespace Volta.Core.Infrastructure
{
    public static class ObjectExtensions
    {
        public static dynamic ToDynamic(this object instance)
        {
            return instance;
        }

        public static bool ObjectEquals(this object left, object right)
        {
            if (ReferenceEquals(null, right)) return false;
            if (ReferenceEquals(left, right)) return true;
            return left.GetHashCode() == right.GetHashCode();
        }

        public static int ObjectHashCode(this object instance, params object[] values)
        {
            unchecked { return values.Select(x => x != null ? x.GetHashCode() : 0).
                                      Aggregate(0, (a, i) => a == 0 ? i : (a * 397) ^ i); }
        }
    }
}