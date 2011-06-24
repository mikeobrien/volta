using FubuMVC.Core;
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

            Policies.WrapBehaviorChainsWith<AuthenticationBehavior>();

            this.UseSpark();

            HtmlConvention(x => { x.Editors.Always.Modify((r, t) => t.Id(r.ElementId));
                                  x.Labels.Always.Modify((r, t) => t.Id(r.ElementId));
                                  x.Displays.Always.Modify((r, t) => t.Id(r.ElementId)); });

            Views.TryToAttach(x => x.by_ViewModel_and_Namespace());
        }
    }
}