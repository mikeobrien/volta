using System.Reflection;
using FubuMVC.Core;
using FubuMVC.Spark;
using Volta.Core.Infrastructure.Framework.Reflection;
using Volta.Core.Infrastructure.Framework.Web;
using Volta.Web.Behaviors;
using Volta.Web.Handlers;

namespace Volta.Web
{
    public class Configuration : FubuRegistry
    {
        public Configuration()
        {
            var inDebugMode = Assembly.GetExecutingAssembly().IsInDebugMode();

            if (inDebugMode) IncludeDiagnostics(true);

            Actions.IncludeTypeNameSuffix("Handler")
                   .IncludeMethodsNamed("Query", "Command");

            Routes.HomeIs<DashboardHandler>(x => x.Query())
                  .ConstrainMethodNameToHttpGet("Query")
                  .ConstrainMethodNameToHttpPost("Command")
                  .IgnoreMethodsNamed("Query", "Command")
                  .IgnoreNamespaceTextOfType<Configuration>("Handlers")
                  .IgnoreClassSuffix("Handler");

            Policies.WrapBehaviorChainsWith<AuthenticationBehavior>()
                    .ConditionallyWrapBehaviorChainsWith<ExceptionHandlerBehavior>(x => !inDebugMode);

            this.UseSpark();

            HtmlConvention(x => { x.Editors.Always.Modify((r, t) => t.Id(r.ElementId));
                                  x.Labels.Always.Modify((r, t) => t.Id(r.ElementId));
                                  x.Displays.Always.Modify((r, t) => t.Id(r.ElementId)); });

            Views.TryToAttach(x => x.by_ViewModel_and_Namespace());
        }
    }
}