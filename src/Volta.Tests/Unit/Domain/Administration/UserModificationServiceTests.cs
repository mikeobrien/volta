using System;
using NSubstitute;
using NUnit.Framework;
using Should;
using System.Linq;
using Volta.Core.Application.Security;
using Volta.Core.Domain;
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
        private static readonly Guid UserId = Guid.NewGuid();
            
        [SetUp]
        public void Setup()
        {
            _userRepository = new MemoryRepository<User>(new User { Id = UserId, Username = Username1, Administrator = true, PasswordHash = PasswordHash });
        }

        [Test]
        public void should_update_existing_user_with_no_change_to_the_username()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.GetCurrentToken().Returns(new Token(Guid.NewGuid(), Username3, true));
            secureSession.IsLoggedIn().Returns(true);
            var service = new UserModificationService(_userRepository, secureSession);
            service.Modify(UserId, Username1, null, false, Password);
            var modifiedUser = _userRepository.First();
            modifiedUser.Username.ShouldEqual(Username1);
            HashedPassword.FromHash(modifiedUser.PasswordHash).MatchesPassword(Password).ShouldBeTrue();
            modifiedUser.Administrator.ShouldBeFalse();
            secureSession.DidNotReceiveWithAnyArgs().Login(Arg.Any<Token>());
        }

        [Test]
        public void should_update_existing_user_with_modified_username()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.GetCurrentToken().Returns(new Token(Guid.NewGuid(), Username3, true));
            secureSession.IsLoggedIn().Returns(true);
            var service = new UserModificationService(_userRepository, secureSession);
            service.Modify(UserId, Username2, null, false, Password);
            var modifiedUser = _userRepository.First();
            modifiedUser.Username.ShouldEqual(Username2);
            HashedPassword.FromHash(modifiedUser.PasswordHash).MatchesPassword(Password).ShouldBeTrue();
            modifiedUser.Administrator.ShouldBeFalse();
            secureSession.DidNotReceiveWithAnyArgs().Login(Arg.Any<Token>());
        }

        [Test]
        public void should_update_logged_in_user()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.GetCurrentToken().Returns(new Token(UserId, Username1, true));
            secureSession.IsLoggedIn().Returns(true);
            var service = new UserModificationService(_userRepository, secureSession);
            service.Modify(UserId, Username1, null, false, Password);
            secureSession.Received().Login(Arg.Is<Token>(x => x.Username == Username1 && !x.IsAdministrator));
        }

        [Test]
        public void should_update_existing_user_and_ignore_empty_password()
        {
            var secureSession = Substitute.For<ISecureSession<Token>>();
            secureSession.GetCurrentToken().Returns(new Token(UserId, Username1, true));
            secureSession.IsLoggedIn().Returns(true);
            var service = new UserModificationService(_userRepository, secureSession);
            service.Modify(UserId, Username2, null, false);
            var modifiedUser = _userRepository.First();
            modifiedUser.Username.ShouldEqual(Username2);
            modifiedUser.PasswordHash.ShouldEqual(PasswordHash);
            modifiedUser.Administrator.ShouldBeFalse();
            service.Modify(UserId, Username2, null, false, "");
            modifiedUser = _userRepository.First();
            modifiedUser.Username.ShouldEqual(Username2);
            modifiedUser.PasswordHash.ShouldEqual(PasswordHash);
            modifiedUser.Administrator.ShouldBeFalse();
        }

        [Test]
        public void should_return_an_error_if_trying_to_update_a_user_that_doesent_exist()
        {
            var service = new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>());
            Assert.Throws<NotFoundException>(() => service.Modify(Guid.NewGuid(), Username2, null, false));
        }

        [Test]
        public void should_return_a_message_if_the_new_username_already_exists()
        {
            _userRepository.Add(new User { Username = Username3 });
            var service = new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>());
            Assert.Throws<DuplicateUsernameException>(() => service.Modify(UserId, Username3, null, true));
        }

        [Test]
        public void should_return_a_message_if_the_new_username_is_empty()
        {
            _userRepository.Add(new User { Username = Username3 });
            var service = new UserModificationService(_userRepository, Substitute.For<ISecureSession<Token>>());
            Assert.Throws<EmptyUsernameException>(() => service.Modify(UserId, string.Empty, null, true));
        }
    }
}