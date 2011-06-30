using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Volta.Core.Infrastructure.Framework.Reflection
{
    public static class AssemblyExtensions
    {
         public static bool IsInDebugMode(this Assembly assembly)
         {
             return assembly.GetCustomAttributes(typeof (DebuggableAttribute), false)
                            .Cast<DebuggableAttribute>()
                            .Where(x => x.IsJITTrackingEnabled).Any();
         }
    }
}