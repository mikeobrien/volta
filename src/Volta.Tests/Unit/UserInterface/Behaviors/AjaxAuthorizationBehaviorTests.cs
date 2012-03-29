using System.Net;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using NSubstitute;
using NUnit.Framework;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Core.Infrastructure.Framework.Web;
using Volta.Web.Behaviors;

namespace Volta.Tests.Unit.UserInterface.Behaviors
{
    [TestFixture]
    public class AjaxAuthorizationBehaviorTests
    {
        [Test]
        public void when_logged_in_should_continue()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.IsLoggedIn().Returns(true);
            var outputWriter = Substitute.For<IOutputWriter>();
            var actionBehavior = Substitute.For<IActionBehavior>();
            var webServer = Substitute.For<IWebServer>();
            var behavior = new AjaxAuthorizationBehavior(outputWriter, actionBehavior, secureSession, webServer);
            behavior.Invoke();
            webServer.DidNotReceiveWithAnyArgs().IgnoreErrorStatus = true;
            actionBehavior.Received().Invoke();
        }

        [Test]
        public void when_not_logged_in_should_return_a_401()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.IsLoggedIn().Returns(false);
            var outputWriter = Substitute.For<IOutputWriter>();
            var actionBehavior = Substitute.For<IActionBehavior>();
            var webServer = Substitute.For<IWebServer>();
            var behavior = new AjaxAuthorizationBehavior(outputWriter, actionBehavior, secureSession, webServer);
            behavior.Invoke();
            webServer.Received().IgnoreErrorStatus = true;
            actionBehavior.DidNotReceive().Invoke();
            outputWriter.Received().WriteResponseCode(HttpStatusCode.Unauthorized, AjaxAuthorizationBehavior.UnauthorizedMessage);
        }
    }
}