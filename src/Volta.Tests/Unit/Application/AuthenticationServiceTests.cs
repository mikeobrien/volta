using System.Linq;
using Volta.Core.Application.Security;
using NUnit.Framework;
using Should;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Tests.Unit.Application
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private const string Username = "username";
        private const string Password = "P@$$word";
        private const string PasswordHash = "7cf40b7962f449e093fe3a40f97bf70b4e0bfb02a6787c6912fa74990a4d2b07ffd24984f7612c62ae8810e49aa358db";

        private readonly IRepository<User> _userRepository = new MemoryRepository<User>(new User { Username = Username, Password = PasswordHash, Administrator = true});

        [Test]
        public void Should_Successfully_Authenticate_With_Correct_Credentials()
        {
            var authenticationService = new AuthenticationService(_userRepository, new UserCreationService(_userRepository));
            var result = authenticationService.Authenticate(Username, Password);
            result.ShouldNotBeNull();
            result.Username.ShouldEqual(Username);
            result.IsAdministrator.ShouldBeTrue();
        }

        [Test]
        public void Should_Fail_To_Authenticate_With_Incorrect_Credentials()
        {
            var authenticationService = new AuthenticationService(_userRepository, new UserCreationService(_userRepository));
            Assert.Throws<AuthenticationService.AccessDeniedException>(() => authenticationService.Authenticate(Username, "yada"));
        }

        [Test]
        public void Should_Add_And_Successfully_Authenticate_The_First_User()
        {
            var userRepository = new MemoryRepository<User>();
            var authenticationService = new AuthenticationService(userRepository, new UserCreationService(userRepository));
            authenticationService.Authenticate(Username, Password);
            userRepository.Count().ShouldEqual(1);
        }
    }
}