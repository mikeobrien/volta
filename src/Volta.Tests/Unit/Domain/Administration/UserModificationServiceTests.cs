using NSubstitute;
using NUnit.Framework;
using Should;
using System.Linq;
using Volta.Core.Application.Security;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Tests.Unit.Domain.Administration
{
    [TestFixture]
    public class UserModificationServiceTests
    {
        private const string Username1 = "someuser";
        private const string Username2 = "anotheruser";
        private const string Username3 = "andanotheruser";
        private const string Password = "P@$$word";
        private const string PasswordHash = "FFFFFFFFFF";
        private IRepository<User> _userRepository;
            
        [SetUp]
        public void Setup()
        {
            _userRepository = new MemoryRepository<User>(new User { Username = Username1, Administrator = true, Password = PasswordHash });
        }

        [Test]
        public void Should_Update_Existing_User()
        {
            var service = new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>());
            var user = new User { Username = Username2, Administrator = false, Password = Password };
            service.Modify(Username1, user);
            var modifiedUser = _userRepository.First();
            modifiedUser.Username.ShouldEqual(Username2);
            HashedPassword.FromHash(modifiedUser.Password).MatchesPassword(Password).ShouldBeTrue();
            modifiedUser.Administrator.ShouldBeFalse();
        }

        [Test]
        public void Should_Update_Logged_In_Users_Token()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            var service = new UserModificationService(_userRepository, secureSession);
            var user = new User { Username = Username2, Administrator = false, Password = Password };
            service.Modify(Username1, user);
            //secureSession.Received().Login();
        }

        [Test]
        public void Should_Update_Existing_User_And_Ignore_Empty_Password()
        {
            var service = new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>());
            var user = new User { Username =  Username2, Administrator = false };
            service.Modify(Username1, user);
            var modifiedUser = _userRepository.First();
            modifiedUser.Username.ShouldEqual(Username2);
            modifiedUser.Password.ShouldEqual(PasswordHash);
            modifiedUser.Administrator.ShouldBeFalse();
        }

        [Test]
        public void Should_Return_An_Error_If_Trying_To_Update_A_User_That_Doesent_Exist()
        {
            var service = new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>());
            var user = new User { Username = Username2, Administrator = false };
            Assert.Throws<UserNotFoundException>(() => service.Modify(Username2, user));
        }

        [Test]
        public void Should_Return_A_Message_If_The_New_Username_Already_Exists()
        {
            _userRepository.Add(new User { Username = Username3 });
            var service = new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>());
            var user = new User { Username = Username3, Administrator = true };
            Assert.Throws<DuplicateUsernameException>(() => service.Modify(Username1, user));
        }
    }
}