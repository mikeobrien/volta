using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Volta.Core.Infrastructure.Application
{
    // This is here because the Fubu transfer/redirect assertion
    // compares the model with one you pass in. 
    // TODO: Add an overload to the fubu assertion to take a Func<TModel, bool>
    //       which will make this go away.
    public abstract class ComparableModelBase
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = 
            new ConcurrentDictionary<Type, PropertyInfo[]>();

        public override bool Equals(object obj) 
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            PropertyInfo[] properties;
            if (!PropertyCache.TryGetValue(GetType(), out properties))
            {
                properties = GetType().GetProperties(
                    BindingFlags.Public | 
                    BindingFlags.Instance | 
                    BindingFlags.GetProperty);
                if (!properties.Any()) throw new Exception(
                    string.Format("Unable to create hash code for type '{0}'. " + "" +
                                  "Type does not have any public instance property getters.", GetType().Name));
                PropertyCache.TryAdd(GetType(), properties);
            }
            unchecked
            {
                return properties.Select(x => x.GetValue(this, null)).Select(x => x != null ? x.GetHashCode() : 0).
                    Aggregate(0, (a, i) => a == 0 ? i : (a * 397) ^ i);
            }
        }
    }
}