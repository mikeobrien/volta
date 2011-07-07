using FubuMVC.Core.Registration.Nodes;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Web;
using Volta.Tests.Unit.Infrastructure.Framework.Web.Handlers;
using Volta.Tests.Unit.Infrastructure.Framework.Web.Handlers.Administration;

namespace Volta.Tests.Unit.Infrastructure.Framework.Web
{
    namespace Handlers
    {
        public class DashboardHandler { }

        namespace Administration
        {
            public class InputModel
            {
                public string Username { get; set; }
                public string Password { get; set; }
            }
            public class OutputModel { public string Username { get; set; } }
            public class UserHandler
            {
                public OutputModel Query() { return null; }
                public OutputModel Query_Username_Password(InputModel input) { return null; }
                public void Command(InputModel input) { }
            }
        }
    }

    [TestFixture]
    public class FullNameUrlPolicyTests
    {
        private static readonly ActionCall UserHandlerQueryActionCall =
            new ActionCall(typeof(UserHandler), typeof(UserHandler).GetMethod("Query"));
        private static readonly ActionCall UserHandlerQueryWithParametersActionCall =
            new ActionCall(typeof(UserHandler), typeof(UserHandler).GetMethod("Query_Username_Password"));
        private static readonly ActionCall UserHandlerCommandActionCall =
            new ActionCall(typeof(UserHandler), typeof(UserHandler).GetMethod("Command"));

        [Test]
        public void Should_Ignore_Namespace()
        {
            var policy = new FullNameUrlPolicy();
            policy.IgnoreNamespace<DashboardHandler>();
            var route = policy.Build(UserHandlerQueryActionCall);
            route.Pattern.ShouldEqual("administration/userhandler/query");
        }

        [Test]
        public void Should_Ignore_Class_Names()
        {
            var policy = new FullNameUrlPolicy();
            policy.IgnoreClassName("Handler");
            var route = policy.Build(UserHandlerQueryActionCall);
            route.Pattern.ShouldEqual("volta/tests/unit/infrastructure/framework/web/handlers/administration/user/query");
        }

        [Test]
        public void Should_Ignore_Method_Names_With_No_Parameters()
        {
            var policy = new FullNameUrlPolicy();
            policy.IgnoreMethodName("Query", "Command");
            var route = policy.Build(UserHandlerQueryActionCall);
            route.Pattern.ShouldEqual("volta/tests/unit/infrastructure/framework/web/handlers/administration/userhandler");
            route.Input.ShouldBeNull();
            route = policy.Build(UserHandlerCommandActionCall);
            route.Pattern.ShouldEqual("volta/tests/unit/infrastructure/framework/web/handlers/administration/userhandler");
            route.Input.ShouldNotBeNull();
        }

        [Test]
        public void Should_Ignore_Method_Names_With_Parameters()
        {
            var policy = new FullNameUrlPolicy();
            policy.IgnoreMethodName("Query", "Command");
            var route = policy.Build(UserHandlerQueryWithParametersActionCall);
            route.Pattern.ShouldEqual("volta/tests/unit/infrastructure/framework/web/handlers/administration/userhandler/{Username}/{Password}");
            route.Input.ShouldNotBeNull();
        }
    }
}