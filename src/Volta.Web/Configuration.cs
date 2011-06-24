using System;
using System.Linq.Expressions;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using FubuMVC.Spark;
using Volta.Core.Infrastructure.Web;
using Volta.Web.Handlers;

namespace Volta.Web
{
    public class Configuration : FubuRegistry
    {
        public Configuration()
        {
            #if DEBUG
                IncludeDiagnostics(true);
            #endif

            Actions.IncludeTypeNameSuffix("Handler")
                   .IncludeMethodsNamed("Query", "Command");

            Routes.HomeIs<DashboardHandler>(x => x.Query())
                  .ConstrainMethodNameToHttpGet("Query")
                  .ConstrainMethodNameToHttpPost("Command")
                  .IgnoreMethodsNamed("Query", "Command")
                  .IgnoreNamespaceTextOfType<Configuration>("Handlers")
                  .IgnoreClassSuffix("Handler");

            Policies.ConditionallyWrapBehaviorChainsWith<AuthenticationBehavior>(x => x.IsNotInHandlerOfType<LoginHandler>() &&
                                                                                      x.IsInAssemblyContainingType<Configuration>());

            this.UseSpark();

            HtmlConvention(x => { x.Editors.Always.Modify((r, t) => t.Id(r.ElementId));
                                  x.Labels.Always.Modify((r, t) => t.Id(r.ElementId));
                                  x.Displays.Always.Modify((r, t) => t.Id(r.ElementId)); });

            Views.TryToAttach(x => x.by_ViewModel_and_Namespace());
        }
    }

    public class AuthenticationBehavior : IActionBehavior
    {
        private readonly IUrlRegistry _registry;
        private readonly IOutputWriter _writer;
        private readonly IActionBehavior _actionBehavior;

        public AuthenticationBehavior(IUrlRegistry registry, IOutputWriter writer, IActionBehavior actionBehavior)
        {
            _registry = registry;
            _writer = writer;
            _actionBehavior = actionBehavior;
        }

        public void Invoke()
        {
            _writer.RedirectToUrl(_registry.UrlFor<LoginHandler>(x => x.Query(null)));
        }

        public void InvokePartial()
        {
        }
    }
}