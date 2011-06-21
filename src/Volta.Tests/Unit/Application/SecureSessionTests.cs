using Volta.Core.Application.Security;
using Volta.Core.Domain;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace Volta.Tests.Unit.Application
{
    [TestFixture]
    public class SecureSessionTests
    {
        [Test]
        public void Should_Login_Valid_User()
        {
            var authenticationService = Substitute.For<IAuthenticationService>();
            var secureSession = new SecureSession(authenticationService, new MemorySessionState());

            authenticationService.Authenticate(Arg.Any<string>(), Arg.Any<string>()).
                                  ReturnsForAnyArgs(new User {Username = "username", IsAdmin = true});

            secureSession.Login("username", "password");

            secureSession.IsLoggedIn().ShouldBeTrue();
            var token = secureSession.GetCurrentToken();
            token.ShouldNotBeNull();
            token.Username.ShouldEqual("username");
            token.IsAdministrator.ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Log_In_An_Invalid_User()
        {
            var authenticationService = Substitute.For<IAuthenticationService>();
            var secureSession = new SecureSession(authenticationService, new MemorySessionState());

            authenticationService.Authenticate(Arg.Any<string>(), Arg.Any<string>()).
                                  ReturnsForAnyArgs(x => { throw new AccessDeniedException(); });

            Assert.Throws<AccessDeniedException>(() => secureSession.Login("username", "password"));

            secureSession.IsLoggedIn().ShouldBeFalse();
            secureSession.GetCurrentToken().ShouldBeNull();
        }

        [Test]
        public void Should_Logout_User()
        {
            var authenticationService = Substitute.For<IAuthenticationService>();
            var secureSession = new SecureSession(authenticationService, new MemorySessionState());

            authenticationService.Authenticate(Arg.Any<string>(), Arg.Any<string>()).
                                  ReturnsForAnyArgs(new User());

            secureSession.Login("username", "password");
            secureSession.Logout();

            secureSession.IsLoggedIn().ShouldBeFalse();
            secureSession.GetCurrentToken().ShouldBeNull();
        }
    }
}