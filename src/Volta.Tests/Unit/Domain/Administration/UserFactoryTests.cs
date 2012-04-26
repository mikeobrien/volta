using System.Linq;
using NUnit.Framework;
using Should;
using Volta.Core.Domain.Administration;

namespace Volta.Tests.Unit.Domain.Administration
{
    [TestFixture]
    public class UserFactoryTests
    {
        private const string Username = "Username";
        private const string Password = "P@$$word";
        private const string Email = "test@test.com";

        [Test]
        public void should_create_unique_non_admin_user()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserFactory(userRepository);
            var user = userCreationService.Create(Username, Password, Email, false);
            user.Username.ShouldEqual(Username.ToLower());
            user.Administrator.ShouldBeFalse();
            user.Email.ShouldEqual(Email);
        }

        [Test]
        public void should_create_user_password_hash()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserFactory(userRepository);
            var user = userCreationService.Create(Username, Password, Email, false);
            user.PasswordHash.ShouldNotBeNull();
            user.PasswordHash.ShouldNotBeEmpty();
            user.PasswordHash.ShouldNotEqual(Password);
        }

        [Test]
        public void should_create_unique_admin_user()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserFactory(userRepository);
            var user = userCreationService.Create(Username, Password, Email, true);
            user.Administrator.ShouldBeTrue();
        }

        [Test]
        public void should_not_create_user_with_duplicate_differently_cased_usernames()
        {
            var userRepository = new MemoryRepository<User>(new User { Username = Username.ToUpper() });
            var userCreationService = new UserFactory(userRepository);
            Assert.Throws<DuplicateUsernameException>(() => userCreationService.Create(Username.ToLower(), Password, Email, false));
        }

        [Test]
        public void should_not_create_user_with_empty_username()
        {
            var userCreationService = new UserFactory(new MemoryRepository<User>());
            Assert.Throws<EmptyUsernameException>(() => userCreationService.Create(string.Empty, Password, Email, false));
        }

        [Test]
        public void should_not_create_user_with_empty_password()
        {
            var userCreationService = new UserFactory(new MemoryRepository<User>());
            Assert.Throws<EmptyPasswordException>(() => userCreationService.Create(Username, string.Empty, Email, false));
        }
    }
}