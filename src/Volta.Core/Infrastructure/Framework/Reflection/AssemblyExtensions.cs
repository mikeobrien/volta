using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Volta.Core.Infrastructure.Framework.IO;

namespace Volta.Core.Infrastructure.Framework.Reflection
{
    public static class AssemblyExtensions
    {
        public static string GetResourceString(this Assembly assembly, string resourceName)
        {
            return GetResourceReader(assembly, resourceName).ReadToEnd();
        }

        public static void SaveResource(this Assembly assembly, string resourceName, string path)
        {
            assembly.GetManifestResourceStream(
                    ResolveResourceNamespace(assembly, resourceName)).Save(path);
        }

        public static string SaveResourceAsTempFile(this Assembly assembly, string resourceName)
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            assembly.SaveResource(resourceName, path);
            return path;
        }

        public static StreamReader GetResourceReader(this Assembly assembly, string resourceName)
        {
            return new StreamReader(
                assembly.GetManifestResourceStream(
                    ResolveResourceNamespace(assembly, resourceName)));
        }

        private static string ResolveResourceNamespace(this Assembly assembly, string resourceName)
        {
            string[] resourceNames =
                assembly.GetManifestResourceNames();
            string currentResourceName = string.Empty;

            foreach (string resource in resourceNames)
            {
                if (resource.ToLower().EndsWith(resourceName.ToLower()) &&
                    ((currentResourceName == string.Empty) |
                        (resource.Length < currentResourceName.Length)))
                    currentResourceName = resource;
            }

            return currentResourceName;
        }

        public static bool IsInDebugMode(this Assembly assembly)
        {
            return assembly.GetCustomAttributes(typeof (DebuggableAttribute), false)
                .Cast<DebuggableAttribute>().Any(x => x.IsJITTrackingEnabled);
        }
    }
}