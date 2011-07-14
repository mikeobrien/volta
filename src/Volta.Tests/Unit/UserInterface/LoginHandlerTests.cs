using Should;
using Volta.Core.Application.Security;
using Volta.Core.Domain;
using NSubstitute;
using NUnit.Framework;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Security;
using Volta.Web.Handlers;

namespace Volta.Tests.Unit.UserInterface
{
    [TestFixture]
    public class LoginHandlerTests
    {
        private const string RedirectUrl = "/myaccount";
        private const string Username = "username";

        [Test]
        public void Should_Return_Authorization_Error_If_Has_Redirect_Url_And_No_Message()
        {
            var loginController = new LoginHandler(null);
            var result = loginController.Query(new LoginOutputModel { RedirectUrl = RedirectUrl });
            result.RedirectUrl.ShouldEqual(RedirectUrl);
            result.Message.MessageText.ShouldEqual(LoginHandler.NotAuthenticatedMessage);
        }

        [Test]
        public void Should_Return_Message_If_Has_Redirect_Url_And_Message()
        {
            const string message = "Oh hai";
            var loginController = new LoginHandler(null);
            var result = loginController.Query(new LoginOutputModel { Message = MessageModel.Information(message), RedirectUrl = RedirectUrl });
            result.RedirectUrl.ShouldEqual(RedirectUrl);
            result.Message.MessageText.ShouldEqual(message);
        }

        [Test]
        public void Should_Login_Valid_User_And_Redirect_To_The_Dashboard()
        {
            var loginController = new LoginHandler(Substitute.For<ISecureSession<Token>>());
            var result = loginController.Command(new LoginInputModel());
            result.AssertWasRedirectedTo<DashboardHandler>(x => x.Query());
        }

        [Test]
        public void Should_Login_Valid_User_And_Redirect_To_The_Original_Url()
        {
            var loginController = new LoginHandler(Substitute.For<ISecureSession<Token>>());
            var result = loginController.Command(new LoginInputModel { RedirectUrl = RedirectUrl });
            result.AssertWasRedirectedTo(RedirectUrl);
        }

        [Test]
        public void Should_Redirect_Invalid_User_Back_To_the_Login_Page_On_Access_Denied_Exception()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.WhenForAnyArgs(x => x.Login(Arg.Any<string>(), Arg.Any<string>())).Do(x => { throw new AuthenticationService.AccessDeniedException(); });
            var loginController = new LoginHandler(secureSession);
            var result = loginController.Command(new LoginInputModel { Username = Username });
            result.AssertWasTransferedTo(new LoginOutputModel { Username = Username, Message = MessageModel.Information(LoginHandler.InvalidUsernameOrPasswordMessage) });
        }

        [Test]
        public void Should_Redirect_Invalid_User_Back_To_the_Login_Page_On_Empty_Username_Exception()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.WhenForAnyArgs(x => x.Login(Arg.Any<string>(), Arg.Any<string>())).Do(x => { throw new EmptyUsernameException(); });
            var loginController = new LoginHandler(secureSession);
            var result = loginController.Command(new LoginInputModel { Username = Username });
            result.AssertWasTransferedTo(new LoginOutputModel { Username = Username, Message = MessageModel.Information(LoginHandler.InvalidUsernameOrPasswordMessage) });
        }

        [Test]
        public void Should_Redirect_Invalid_User_Back_To_the_Login_Page_On_Empty_Password_Exception()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.WhenForAnyArgs(x => x.Login(Arg.Any<string>(), Arg.Any<string>())).Do(x => { throw new EmptyPasswordException(); });
            var loginController = new LoginHandler(secureSession);
            var result = loginController.Command(new LoginInputModel { Username = Username });
            result.AssertWasTransferedTo(new LoginOutputModel { Username = Username, Message = MessageModel.Information(LoginHandler.InvalidUsernameOrPasswordMessage) });
        }

        [Test]
        public void Should_Redirect_Invalid_User_Back_To_the_Login_Page_On_Access_Denied_Exception_With_A_Redirection()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.WhenForAnyArgs(x => x.Login(Arg.Any<string>(), Arg.Any<string>())).Do(x => { throw new AuthenticationService.AccessDeniedException(); });
            var loginController = new LoginHandler(secureSession);
            var result = loginController.Command(new LoginInputModel { Username = Username, RedirectUrl = RedirectUrl });
            result.AssertWasTransferedTo(new LoginOutputModel { Username = Username, Message = MessageModel.Information(LoginHandler.InvalidUsernameOrPasswordMessage), RedirectUrl = RedirectUrl });
        }
    }
}