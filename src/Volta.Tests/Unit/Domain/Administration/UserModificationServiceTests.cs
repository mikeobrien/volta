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
        public void should_update_existing_user_with_no_change_to_the_username()
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
        public void should_update_existing_user_with_modified_username()
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
        public void should_update_logged_in_user()
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
        public void should_update_existing_user_and_ignore_empty_password()
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
        public void should_return_an_error_if_trying_to_update_a_user_that_doesent_exist()
        {
            var service = new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>());
            var user = new User { Username = Username2, Administrator = false };
            Assert.Throws<UserNotFoundException>(() => service.Modify(Username2, user));
        }

        [Test]
        public void should_return_a_message_if_the_new_username_already_exists()
        {
            _userRepository.Add(new User { Username = Username3 });
            var service = new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>());
            var user = new User { Username = Username3, Administrator = true };
            Assert.Throws<DuplicateUsernameException>(() => service.Modify(Username1, user));
        }

        [Test]
        public void should_return_a_message_if_the_new_username_is_empty()
        {
            _userRepository.Add(new User { Username = Username3 });
            var service = new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>());
            var user = new User { Username = string.Empty, Administrator = true };
            Assert.Throws<EmptyUsernameException>(() => service.Modify(Username1, user));
        }
    }
}