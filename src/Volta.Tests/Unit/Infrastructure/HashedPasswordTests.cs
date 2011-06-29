using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Tests.Unit.Infrastructure
{
    [TestFixture]
    public class HashedPasswordTests
    {
        private const string Password = "P@$$word";
        private const string PasswordHash = "7cf40b7962f449e093fe3a40f97bf70b4e0bfb02a6787c6912fa74990a4d2b07ffd24984f7612c62ae8810e49aa358db";

        [Test]
        public void Should_Hash_Plain_Text_Password()
        {
            HashedPassword.Create(Password).ToString().ShouldNotEqual(Password);
        }

        [Test]
        public void Should_Generate_A_Different_Hash_For_The_Same_Password()
        {
            HashedPassword.Create(Password).ToString().ShouldNotEqual(HashedPassword.Create(Password).ToString());
        }

        [Test]
        public void Should_Open_Hashed_Password()
        {
            HashedPassword.FromHash(PasswordHash).ToString().ShouldEqual(PasswordHash);
        }

        [Test]
        public void Same_Password_String_Should_Equal_Hashed_Password()
        {
            HashedPassword.FromHash(PasswordHash).MatchesPassword(Password).ShouldBeTrue();
        }

        [Test]
        public void Different_Password_String_Should_Not_Equal_Hashed_Password()
        {
            HashedPassword.FromHash(PasswordHash).MatchesPassword("yada").ShouldBeFalse();
        }
    }
}