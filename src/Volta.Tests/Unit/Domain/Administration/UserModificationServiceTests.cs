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
        public void Should_Update_Existing_User_With_No_Change_To_The_Username()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.GetCurrentToken().Returns(new Token(Username3, true));
            secureSession.IsLoggedIn().Returns(true);
            var service = new UserModificationService(_userRepository, secureSession);
            var user = new User { Username = Username1, Administrator = false, Password = Password };
            service.Modify(Username1, user);
            var modifiedUser = _userRepository.First();
            modifiedUser.Username.ShouldEqual(Username1);
            HashedPassword.FromHash(modifiedUser.Password).MatchesPassword(Password).ShouldBeTrue();
            modifiedUser.Administrator.ShouldBeFalse();
            secureSession.DidNotReceiveWithAnyArgs().Login(Arg.Any<Token>());
        }

        [Test]
        public void Should_Update_Existing_User_With_Modified_Username()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.GetCurrentToken().Returns(new Token(Username3, true));
            secureSession.IsLoggedIn().Returns(true);
            var service = new UserModificationService(_userRepository, secureSession);
            var user = new User { Username = Username2, Administrator = false, Password = Password };
            service.Modify(Username1, user);
            var modifiedUser = _userRepository.First();
            modifiedUser.Username.ShouldEqual(Username2);
            HashedPassword.FromHash(modifiedUser.Password).MatchesPassword(Password).ShouldBeTrue();
            modifiedUser.Administrator.ShouldBeFalse();
            secureSession.DidNotReceiveWithAnyArgs().Login(Arg.Any<Token>());
        }

        [Test]
        public void Should_Update_Logged_In_User()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.GetCurrentToken().Returns(new Token(Username1, true));
            secureSession.IsLoggedIn().Returns(true);
            var service = new UserModificationService(_userRepository, secureSession);
            var user = new User { Username = Username1, Administrator = false, Password = Password };
            service.Modify(Username1, user);
            secureSession.Received().Login(Arg.Is<Token>(x => x.Username == Username1 && !x.IsAdministrator));
        }

        [Test]
        public void Should_Update_Existing_User_And_Ignore_Empty_Password()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.GetCurrentToken().Returns(new Token(Username1, true));
            secureSession.IsLoggedIn().Returns(true);
            var service = new UserModificationService(_userRepository, secureSession);
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

        [Test]
        public void Should_Return_A_Message_If_The_New_Username_Is_Empty()
        {
            _userRepository.Add(new User { Username = Username3 });
            var service = new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>());
            var user = new User { Username = string.Empty, Administrator = true };
            Assert.Throws<EmptyUsernameException>(() => service.Modify(Username1, user));
        }
    }
}