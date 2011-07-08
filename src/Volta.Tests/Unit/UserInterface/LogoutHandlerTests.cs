using Volta.Core.Application.Security;
using NSubstitute;
using NUnit.Framework;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Web.Handlers;

namespace Volta.Tests.Unit.UserInterface
{
    [TestFixture]
    public class LogoutHandlerTests
    {
        [Test]
        public void Should_Login_Valid_User_And_Redirect_To_The_Dashboard()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            var controller = new LogoutHandler(secureSession);
            var result = controller.Query();
            result.AssertWasRedirectedTo<LoginHandler>(x => x.Query(null));
            secureSession.Received().Logout();
        }
    }
}