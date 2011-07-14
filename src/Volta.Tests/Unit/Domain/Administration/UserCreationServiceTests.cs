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

        [Test]
        public void Should_Create_Unique_Non_Admin_User()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserCreationService(userRepository);
            var user = userCreationService.Create(Username, Password, false);
            userRepository.Count().ShouldEqual(1);
            userRepository.First().ShouldEqual(user);
            user.Username.ShouldEqual(Username.ToLower());
            user.Administrator.ShouldBeFalse();
        }

        [Test]
        public void Should_Create_User_Password_Hash()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserCreationService(userRepository);
            var user = userCreationService.Create(Username, Password, false);
            user.Password.ShouldNotBeNull();
            user.Password.ShouldNotBeEmpty();
            user.Password.ShouldNotEqual(Password);
        }

        [Test]
        public void Should_Create_Unique_Admin_User()
        {
            var userRepository = new MemoryRepository<User>();
            var userCreationService = new UserCreationService(userRepository);
            var user = userCreationService.Create(Username, Password, true);
            user.Administrator.ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Create_User_With_Duplicate_Differently_Cased_Usernames()
        {
            var userRepository = new MemoryRepository<User>(new User {Username = Username.ToUpper()});
            var userCreationService = new UserCreationService(userRepository);
            Assert.Throws<DuplicateUsernameException>(() => userCreationService.Create(Username.ToLower(), Password, false));
        }

        [Test]
        public void Should_Not_Create_User_With_Empty_Username()
        {
            var userCreationService = new UserCreationService(new MemoryRepository<User>());
            Assert.Throws<EmptyUsernameException>(() => userCreationService.Create(string.Empty, Password, false));
        }

        [Test]
        public void Should_Not_Create_User_With_Empty_Password()
        {
            var userCreationService = new UserCreationService(new MemoryRepository<User>());
            Assert.Throws<EmptyPasswordException>(() => userCreationService.Create(Username, string.Empty, false));
        }
    }
}