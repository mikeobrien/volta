using System.Collections.Generic;
using System.Linq;
using FubuCore.Reflection;
using FubuMVC.Core.Diagnostics;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;

namespace Volta.Core.Infrastructure.Framework.Web
{
    public class FullNameUrlPolicy : IUrlPolicy
    {
        private readonly List<string> _ignoreNamespace = new List<string>();
        private readonly List<string> _ignoreClass = new List<string>();
        private readonly List<string> _ignoreMethod = new List<string>();

        public static FullNameUrlPolicy Create()
        {
            return new FullNameUrlPolicy();
        }

        public bool Matches(ActionCall call, IConfigurationObserver log)
        {
            return true;
        }

        public FullNameUrlPolicy IgnoreNamespace<T>()
        {
            _ignoreNamespace.Add(typeof(T).Namespace);
            return this;
        }

        public FullNameUrlPolicy IgnoreClassName(params string[] names)
        {
            _ignoreClass.AddRange(names);
            return this;
        }

        public FullNameUrlPolicy IgnoreMethodName(params string[] names)
        {
            _ignoreMethod.AddRange(names);
            return this;
        }

        public IRouteDefinition Build(ActionCall call)
        {
            var route = call.ToRouteDefinition();
            AppendNamespace(route, call, _ignoreNamespace);
            AppendClass(route, call, _ignoreClass);
            AppendMethod(route, call, _ignoreMethod);
            return route;
        }

        private static void AppendNamespace(IRouteDefinition route, ActionCall call, IEnumerable<string> ignore)
        {
            route.Append(RemovePattern(call.HandlerType.Namespace, ignore).Replace('.', '/').ToLower());
        }

        private static void AppendClass(IRouteDefinition route, ActionCall call, IEnumerable<string> ignore)
        {
            route.Append(RemovePattern(call.HandlerType.Name, ignore).ToLower());
        }

        private static void AppendMethod(IRouteDefinition route, ActionCall call, IEnumerable<string> ignore)
        {
            var methodName = RemovePattern(call.Method.Name, ignore);
            if (MethodToUrlBuilder.Matches(call.Method.Name) && call.HasInput)
            {
                MethodToUrlBuilder.Alter(route, methodName, new TypeDescriptorCache().GetPropertiesFor(call.InputType()).Keys, x => { });
                route.ApplyInputType(call.InputType());
            }
            else if (!string.IsNullOrEmpty(methodName)) route.Append(methodName);
        }

        private static string RemovePattern(string source, IEnumerable<string> pattern)
        {
            return pattern.Aggregate(source, (a, i) => a.Replace(i, string.Empty));
        }
    }
}