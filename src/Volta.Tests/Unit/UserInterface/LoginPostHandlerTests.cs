using NSubstitute;
using NUnit.Framework;
using Should;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Web;

namespace Volta.Tests.Unit.UserInterface.Login
{
    [TestFixture]
    public class LoginPostHandlerTests
    {
        [Test]
        public void should_return_successful_authentication_result()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.Login(null, null).ReturnsForAnyArgs(true);
            var handler = new LoginPublicPostHandler(secureSession);
            var model = handler.Execute(new LoginRequest());
            model.success.ShouldBeTrue();
        }

        [Test]
        public void should_return_unsuccessful_authentication_result()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.Login(null, null).ReturnsForAnyArgs(false);
            var handler = new LoginPublicPostHandler(secureSession);
            var model = handler.Execute(new LoginRequest());
            model.success.ShouldBeFalse();
        }
    }
}