using System.Reflection;
using FubuMVC.Core;
using FubuMVC.Spark;
using Volta.Core.Infrastructure.Framework.Reflection;
using Volta.Core.Infrastructure.Framework.Web.FubuMvc;
using Volta.Web.Behaviors;
using Volta.Web.Handlers;

namespace Volta.Web
{
    public class ConfigureFubuMVC : FubuRegistry
    {
        public ConfigureFubuMVC()
        {
            var debug = Assembly.GetExecutingAssembly().IsInDebugMode();

            IncludeDiagnostics(debug);

            Actions.IncludeTypeNameSuffix("Handler")
                   .IncludeMethodsPrefixed("Query", "Command");

            Routes.HomeIs<DashboardHandler>(x => x.Query())
                  .ConstrainMethodPrefixToHttpGet("Query")
                  .ConstrainMethodPrefixToHttpPost("Command")
                  .UrlPolicy(FullNameUrlPolicy.Create().
                                IgnoreNamespace<ConfigureFubuMVC>().
                                IgnoreClassName("Handler").
                                IgnoreMethodName("Query", "Command"));

            Policies.WrapBehaviorChainsWith<AuthorizationBehavior>()
                    .ConditionallyWrapBehaviorChainsWith<ExceptionHandlerBehavior>(x => !debug);

            this.UseSpark();

            HtmlConvention<VoltaHtmlConventions>();

            Views.TryToAttach(x => x.by_ViewModel_and_Namespace());
        }
    }
}