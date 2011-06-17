using Volta.Core.Application.Security;
using Volta.Core.Domain;
using Volta.Web.Controllers.Dashboard;
using Volta.Web.Controllers.Login;
using NSubstitute;
using NUnit.Framework;

namespace Volta.Tests.Unit.UserInterface
{
    [TestFixture]
    public class LoginControllerTests
    {
        [Test]
        public void Should_Login_Valid_User_And_Redirect_To_The_Dashboard()
        {
            var loginController = new LoginController(Substitute.For<ISecureSession>());
            var result = loginController.post_Login(new LoginInputModel());
            result.AssertWasRedirectedTo<DashboardController>(x => x.Index());
        }

        [Test]
        public void Should_Redirect_Invalid_User_Back_To_the_Login_Page_On_Access_Denied_Exception()
        {
            var secureSession = Substitute.For<ISecureSession>();
            secureSession.When(x => x.Login(Arg.Any<string>(), Arg.Any<string>())).Do(x => {throw new AccessDeniedException();});
            var loginController = new LoginController(secureSession);
            var result = loginController.post_Login(new LoginInputModel {Username = "username"});
            result.AssertWasTransferedTo(new LoginViewModel {Username = "username", AccessDenied = true});
        }

        [Test]
        public void Should_Redirect_Invalid_User_Back_To_the_Login_Page_On_Empty_Username_Or_Password_Exception()
        {
            var secureSession = Substitute.For<ISecureSession>();
            secureSession.When(x => x.Login(Arg.Any<string>(), Arg.Any<string>())).Do(x => { throw new EmptyUsernameOrPasswordException(); });
            var loginController = new LoginController(secureSession);
            var result = loginController.post_Login(new LoginInputModel { Username = "username" });
            result.AssertWasTransferedTo(new LoginViewModel { Username = "username", AccessDenied = true });
        }
    }
}