using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
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
                  .ConstrainMethodNameToHttpGetMethod("Query")
                  .ConstrainMethodNameToHttpPostMethod("Command")
                  .IgnoreMethodsNamed("Query", "Command")
                  .IgnoreExecutingAssemblyNamespaceText("Handlers")
                  .IgnoreClassSuffix("Handler");

            Policies.WrapBehaviorChainsWith<OhBehavior>()
                    .WrapBehaviorChainsWith<HaiBehavior>()
                    .WrapBehaviorChainsWith<ThereBehavior>();

            this.UseSpark();

            HtmlConvention<HtmlConventions>();

            Views.TryToAttach(x => x.by_ViewModel_and_Namespace());
        }
    }

    public class OhBehavior : BasicBehavior
    {
        public OhBehavior() : base(PartialBehavior.Ignored)
        {
        }
    }

    public class HaiBehavior : BasicBehavior
    {
        public HaiBehavior() : base(PartialBehavior.Ignored)
        {
        }
    }

    public class ThereBehavior : BasicBehavior
    {
        public ThereBehavior() : base(PartialBehavior.Ignored)
        {
        }
    }
}