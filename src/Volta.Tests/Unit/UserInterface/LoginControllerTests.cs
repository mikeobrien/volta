using System;
using Volta.Core.Application.Security;
using Volta.Core.Domain;
using NSubstitute;
using NUnit.Framework;
using Volta.Web.Handlers;

namespace Volta.Tests.Unit.UserInterface
{
    [TestFixture]
    public class LoginControllerTests
    {
        [Test]
        public void Should_Login_Valid_User_And_Redirect_To_The_Dashboard()
        {
            var loginController = new LoginHandler(Substitute.For<ISecureSession>());
            var result = loginController.Command(new LoginInputModel());
            result.AssertWasRedirectedTo<DashboardHandler>(x => x.Query());
        }

        [Test]
        public void Should_Redirect_Invalid_User_Back_To_the_Login_Page_On_Access_Denied_Exception()
        {
            var secureSession = Substitute.For<ISecureSession>();
            secureSession.When(x => x.Login(Arg.Any<string>(), Arg.Any<string>())).Do(x => {throw new AccessDeniedException();});
            var loginController = new LoginHandler(secureSession);
            var result = loginController.Command(new LoginInputModel {Username = "username"});
            result.AssertWasTransferedTo(new LoginOutputModel {Username = "username", Message = "Invalid username or password." } );
        }

        [Test]
        public void Should_Redirect_Invalid_User_Back_To_the_Login_Page_On_Empty_Username_Or_Password_Exception()
        {
            var secureSession = Substitute.For<ISecureSession>();
            secureSession.When(x => x.Login(Arg.Any<string>(), Arg.Any<string>())).Do(x => { throw new EmptyUsernameOrPasswordException(); });
            var loginController = new LoginHandler(secureSession);
            var result = loginController.Command(new LoginInputModel { Username = "username" });
            result.AssertWasTransferedTo(new LoginOutputModel { Username = "username", Message = "Invalid username or password." });
        }
    }
}