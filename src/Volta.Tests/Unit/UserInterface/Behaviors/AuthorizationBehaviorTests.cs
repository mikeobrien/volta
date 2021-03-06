using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using NSubstitute;
using NUnit.Framework;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Web.Behaviors;

namespace Volta.Tests.Unit.UserInterface.Behaviors
{
    [TestFixture]
    public class AuthorizationBehaviorTests
    {
        [Test]
        public void when_logged_in_should_continue()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.IsLoggedIn().Returns(true);
            var outputWriter = Substitute.For<IOutputWriter>();
            var actionBehavior = Substitute.For<IActionBehavior>();
            var behavior = new AuthorizationBehavior(outputWriter, actionBehavior, secureSession);
            behavior.Invoke();
            outputWriter.DidNotReceiveWithAnyArgs().RedirectToUrl(null);
            actionBehavior.Received().Invoke();
        }

        [Test]
        public void when_not_logged_in_should_redirect_to_login_page()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.IsLoggedIn().Returns(false);
            var outputWriter = Substitute.For<IOutputWriter>();
            var actionBehavior = Substitute.For<IActionBehavior>();
            var behavior = new AuthorizationBehavior(outputWriter, actionBehavior, secureSession);
            behavior.Invoke();
            actionBehavior.DidNotReceive().Invoke();
            outputWriter.Received().RedirectToUrl(AuthorizationBehavior.LoginUrl);
        }
    }
}