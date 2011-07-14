using Volta.Core.Application.Security;
using NSubstitute;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Tests.Unit.Infrastructure.Framework.Security
{
    [TestFixture]
    public class SecureSessionTests
    {
        [Test]
        public void Should_Login_Token()
        {
            var secureSession = new SecureSession<Token>(null, new MemoryTokenStore<Token>());
            var token = new Token(null, false);

            secureSession.Login(token);

            secureSession.IsLoggedIn().ShouldBeTrue();
            secureSession.GetCurrentToken().ShouldEqual(token);
        }

        [Test]
        public void Should_Login_Valid_User()
        {
            var authenticationService = Substitute.For<IAuthenticationService<Token>>();
            var secureSession = new SecureSession<Token>(authenticationService, new MemoryTokenStore<Token>());

            authenticationService.Authenticate(Arg.Any<string>(), Arg.Any<string>()).
                                  ReturnsForAnyArgs(new Token("username", true));

            secureSession.Login("username", "password");

            secureSession.IsLoggedIn().ShouldBeTrue();
            var token = secureSession.GetCurrentToken();
            token.ShouldNotBeNull();
            token.Username.ToString().ShouldEqual("username");
            token.IsAdministrator.ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Log_In_An_Invalid_User()
        {
            var authenticationService = Substitute.For<IAuthenticationService<Token>>();
            var secureSession = new SecureSession<Token>(authenticationService, new MemoryTokenStore<Token>());

            authenticationService.Authenticate(Arg.Any<string>(), Arg.Any<string>()).
                                  ReturnsForAnyArgs(x => { throw new AuthenticationService.AccessDeniedException(); });

            Assert.Throws<AuthenticationService.AccessDeniedException>(() => secureSession.Login("username", "password"));

            secureSession.IsLoggedIn().ShouldBeFalse();
            secureSession.GetCurrentToken().ShouldBeNull();
        }

        [Test]
        public void Should_Logout_User()
        {
            var authenticationService = Substitute.For<IAuthenticationService<Token>>();
            var secureSession = new SecureSession<Token>(authenticationService, new MemoryTokenStore<Token>());

            authenticationService.Authenticate(Arg.Any<string>(), Arg.Any<string>()).
                                  ReturnsForAnyArgs(new Token(null, false));

            secureSession.Login("username", "password");
            secureSession.Logout();

            secureSession.IsLoggedIn().ShouldBeFalse();
            secureSession.GetCurrentToken().ShouldBeNull();
        }
    }
}