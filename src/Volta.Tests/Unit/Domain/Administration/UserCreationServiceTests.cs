using System.Linq;
using NUnit.Framework;
using Should;
using Volta.Core.Domain.Administration;

namespace Volta.Tests.Unit.Domain.Administration
{
    [TestFixture]
    public class UserCreationServiceTests
    {
        private const string Username = "Username";
        private const string Password = "P@$$word";
        private const string Email = "test@test.com";

        [Test]
        public void should_create_unique_non_admin_user()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserCreationService(userRepository);
            var user = userCreationService.Create(Username, Password, Email, false);
            userRepository.Count().ShouldEqual(1);
            userRepository.First().ShouldEqual(user);
            user.Username.ShouldEqual(Username.ToLower());
            user.Administrator.ShouldBeFalse();
            user.Email.ShouldEqual(Email);
        }

        [Test]
        public void should_create_user_password_hash()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserCreationService(userRepository);
            var user = userCreationService.Create(Username, Password, Email, false);
            user.Password.ShouldNotBeNull();
            user.Password.ShouldNotBeEmpty();
            user.Password.ShouldNotEqual(Password);
        }

        [Test]
        public void should_create_unique_admin_user()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserCreationService(userRepository);
            var user = userCreationService.Create(Username, Password, Email, true);
            user.Administrator.ShouldBeTrue();
        }

        [Test]
        public void should_not_create_user_with_duplicate_differently_cased_usernames()
        {
            var userRepository = new MemoryRepository<User>(new User { Username = Username.ToUpper() });
            var userCreationService = new UserCreationService(userRepository);
            Assert.Throws<DuplicateUsernameException>(() => userCreationService.Create(Username.ToLower(), Password, Email, false));
        }

        [Test]
        public void should_not_create_user_with_empty_username()
        {
            var userCreationService = new UserCreationService(new MemoryRepository<User>());
            Assert.Throws<EmptyUsernameException>(() => userCreationService.Create(string.Empty, Password, Email, false));
        }

        [Test]
        public void should_not_create_user_with_empty_password()
        {
            var userCreationService = new UserCreationService(new MemoryRepository<User>());
            Assert.Throws<EmptyPasswordException>(() => userCreationService.Create(Username, string.Empty, Email, false));
        }
    }
}