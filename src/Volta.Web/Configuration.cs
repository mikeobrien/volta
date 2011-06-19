using Volta.Web.Controllers.Dashboard;
using FubuMVC.Core;
using FubuMVC.Spark;

namespace Volta.Web
{
    public class Configuration : FubuRegistry
    {
        public Configuration()
        {
            #if DEBUG
                IncludeDiagnostics(true);
            #endif

            Actions.IncludeClassesSuffixedWithController();

            Routes
                .HomeIs<DashboardController>(x => x.Index())
                .IgnoreControllerNamesEntirely()
                .IgnoreMethodSuffix("Html")
                .IgnoreNamespaceText("Volta.Web.Controllers");

            this.UseSpark();

            HtmlConvention<HtmlConventions>();

            Views.TryToAttachWithDefaultConventions();
        }
    }
}