using System.Linq;
using Volta.Core.Domain;
using NUnit.Framework;
using Should;

namespace Volta.Tests.Unit.Domain
{
    [TestFixture]
    public class UserCreationServiceTests
    {
        private const string Username = "Username";
        private const string Password = "P@$$word";

        [Test]
        public void Should_Create_Unique_Non_Admin_User()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserCreationService(userRepository, new UserFactory());
            var user = userCreationService.CreateUser(Username, Password, false);
            userRepository.Count().ShouldEqual(1);
            userRepository.First().ShouldEqual(user);
            user.Username.ShouldEqual(Username.ToLower());
            user.IsAdmin.ShouldBeFalse();
        }

        [Test]
        public void Should_Create_User_Password_Hash()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserCreationService(userRepository, new UserFactory());
            var user = userCreationService.CreateUser(Username, Password, false);
            user.PasswordHash.ShouldNotBeNull();
            user.PasswordHash.ShouldNotBeEmpty();
            user.PasswordHash.ShouldNotEqual(Password);
        }

        [Test]
        public void Should_Create_User_Api_Key()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserCreationService(userRepository, new UserFactory());
            var user = userCreationService.CreateUser(Username, Password, false);
            user.ApiKey.ShouldNotBeNull();
            user.ApiKey.ShouldNotBeEmpty();
        }

        [Test]
        public void Should_Create_Unique_Admin_User()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserCreationService(userRepository, new UserFactory());
            var user = userCreationService.CreateUser(Username, Password, true);
            user.IsAdmin.ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Create_User_With_Duplicate_Differently_Cased_Usernames()
        {
            var userRepository = new MemoryRepository<User>(new User {Username = Username.ToUpper()});
            var userCreationService = new UserCreationService(userRepository, new UserFactory());
            Assert.Throws<DuplicateUsernameException>(() => userCreationService.CreateUser(Username.ToLower(), Password, false));
        }

        [Test]
        public void Should_Not_Create_User_With_Empty_Username()
        {
            var userCreationService = new UserCreationService(new MemoryRepository<User>(), new UserFactory());
            Assert.Throws<EmptyUsernameOrPasswordException>(() => userCreationService.CreateUser(Username, string.Empty, false));
        }

        [Test]
        public void Should_Not_Create_User_With_Empty_Password()
        {
            var userCreationService = new UserCreationService(new MemoryRepository<User>(), new UserFactory());
            Assert.Throws<EmptyUsernameOrPasswordException>(() => userCreationService.CreateUser(string.Empty, Password, false));
        }
    }
}