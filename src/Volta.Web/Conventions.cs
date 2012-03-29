using System.Linq;
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

            Actions
                .IncludeTypeNamesSuffixed("Handler")
                .IncludeMethodsPrefixed("Execute");

            var publicHandlers = new[] { "PublicGetHandler", "PublicPostHandler", "PublicPutHandler", "PublicDeleteHandler" };
            var privateHandlers = new[] { "GetHandler", "PostHandler", "PutHandler", "DeleteHandler" };

            Routes
                .HomeIs<IndexGetHandler>(x => x.Execute())
                .OverrideFolders()
                .UrlPolicy(RegexUrlPolicy.Create()
                    .IgnoreAssemblyNamespace()
                    .IgnoreClassNames(publicHandlers)
                    .IgnoreClassNames(privateHandlers)
                    .IgnoreMethodNames("Execute")
                    .ConstrainClassToHttpGetEndingWith(publicHandlers[0], privateHandlers[0])
                    .ConstrainClassToHttpPostEndingWith(publicHandlers[1], privateHandlers[1])
                    .ConstrainClassToHttpPutEndingWith(publicHandlers[2], privateHandlers[2])
                    .ConstrainClassToHttpDeleteEndingWith(publicHandlers[3], privateHandlers[3]));

            Policies
                .ConditionallyWrapBehaviorChainsWith<AuthorizationBehavior>(x =>
                    x.HandlerType.Assembly == GetType().Assembly && x.HasAnyOutputBehavior() &&
                    !publicHandlers.Any(suffix => x.HandlerType.Name.EndsWith(suffix)))
                .ConditionallyWrapBehaviorChainsWith<AjaxAuthorizationBehavior>(x => 
                    x.HandlerType.Assembly == GetType().Assembly && !x.HasAnyOutputBehavior() &&
                    !publicHandlers.Any(suffix => x.HandlerType.Name.EndsWith(suffix)))
                .WrapBehaviorChainsWith<ExceptionHandlerBehavior>();

            Media.ApplyContentNegotiationToActions(x => x.HandlerType.Assembly == GetType().Assembly && !x.HasAnyOutputBehavior());

            this.UseSpark();

            Views.TryToAttachWithDefaultConventions();
        }
    }
}