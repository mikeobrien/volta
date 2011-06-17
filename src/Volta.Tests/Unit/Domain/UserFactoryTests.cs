using Volta.Core.Domain;
using NUnit.Framework;
using Should;

namespace Volta.Tests.Unit.Domain
{
    [TestFixture]
    public class UserFactoryTests
    {
        private const string Username = "Username";
        private const string Password = "P@$$word";

        [Test]
        public void Should_Create_Non_Admin_User()
        {
            var userFactory = new UserFactory();
            var user = userFactory.Create(Username, Password, false);
            user.IsAdmin.ShouldBeFalse();
        }

        [Test]
        public void Should_Create_User_With_Hashed_Password()
        {
            var userFactory = new UserFactory();
            var user = userFactory.Create(Username, Password, false);
            user.PasswordHash.ShouldNotBeNull();
            user.PasswordHash.ShouldNotBeEmpty();
            user.PasswordHash.ShouldNotEqual(Password);
        }

        [Test]
        public void Should_Create_User_With_Api_Key()
        {
            var userFactory = new UserFactory();
            var user = userFactory.Create(Username, Password, false);
            user.ApiKey.ShouldNotBeNull();
            user.ApiKey.ShouldNotBeEmpty();
        }

        [Test]
        public void Should_Create_User_With_Lower_Case_Username()
        {
            var userFactory = new UserFactory();
            var user = userFactory.Create(Username.ToUpper(), Password, false);
            user.Username.ShouldEqual(Username.ToLower());
        }

        [Test]
        public void Should_Create_Admin_User()
        {
            var userFactory = new UserFactory();
            var user = userFactory.Create(Username, Password, true);
            user.IsAdmin.ShouldBeTrue();
        }

        [Test]
        public void Should_Throw_Exception_When_Username_Is_Empty()
        {
            var userFactory = new UserFactory();
            Assert.Throws<EmptyUsernameOrPasswordException>(() => userFactory.Create(string.Empty, Password, false));
        }

        [Test]
        public void Should_Throw_Exception_When_Password_Is_Empty()
        {
            var userFactory = new UserFactory();
            Assert.Throws<EmptyUsernameOrPasswordException>(() => userFactory.Create(Username, string.Empty, false));
        }
    }
}