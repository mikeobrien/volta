using System.Reflection;
using FubuMVC.Core;
using FubuMVC.Spark;
using Volta.Core.Infrastructure.Framework.Reflection;
using Volta.Core.Infrastructure.Framework.Web.Fubu;
using Volta.Web.Behaviors;

namespace Volta.Web
{
    public class Conventions : FubuRegistry
    {
        public Conventions()
        {
            var debug = Assembly.GetExecutingAssembly().IsInDebugMode();

            IncludeDiagnostics(debug);

            Applies.ToAssemblyContainingType(GetType());

            Actions
                .IncludeTypeNamesSuffixed("Handler")
                .IncludeMethodsPrefixed("Execute");

            Routes
                .OverrideFolders()
                .UrlPolicy(RegexUrlPolicy.Create()
                    .IgnoreAssemblyNamespace()
                    .IgnoreNamespaces("Handlers")
                    .IgnoreClassName()
                    .IgnoreMethodNames("Execute")
                    .ConstrainClassToHttpGetStartingWith("Get")
                    .ConstrainClassToHttpGetStartingWith("PublicGet")
                    .ConstrainClassToHttpPostStartingWith("Post")
                    .ConstrainClassToHttpPostStartingWith("PublicPost")
                    .ConstrainClassToHttpPutStartingWith("Put")
                    .ConstrainClassToHttpPutStartingWith("PublicPut")
                    .ConstrainClassToHttpDeleteStartingWith("Delete")
                    .ConstrainClassToHttpDeleteStartingWith("PublicDelete"));

            Policies.WrapBehaviorChainsWith<AuthorizationBehavior>()
                    .ConditionallyWrapBehaviorChainsWith<ExceptionHandlerBehavior>(x => !debug);

            Media.ApplyContentNegotiationToActions(x => x.HandlerType.Assembly == GetType().Assembly);

            this.UseSpark();

            Views.TryToAttachWithDefaultConventions();
        }
    }
}