using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Tests.Unit.Infrastructure.Framework
{
    [TestFixture]
    public class HashedPasswordTests
    {
        private const string Password = "P@$$word";
        private const string PasswordHash = "7cf40b7962f449e093fe3a40f97bf70b4e0bfb02a6787c6912fa74990a4d2b07ffd24984f7612c62ae8810e49aa358db";

        [Test]
        public void should_hash_plain_text_password()
        {
            HashedPassword.Create(Password).ToString().ShouldNotEqual(Password);
        }

        [Test]
        public void should_generate_a_different_hash_for_the_same_password()
        {
            HashedPassword.Create(Password).ToString().ShouldNotEqual(HashedPassword.Create(Password).ToString());
        }

        [Test]
        public void should_open_hashed_password()
        {
            HashedPassword.FromHash(PasswordHash).ToString().ShouldEqual(PasswordHash);
        }

        [Test]
        public void same_password_string_should_equal_hashed_password()
        {
            HashedPassword.FromHash(PasswordHash).MatchesPassword(Password).ShouldBeTrue();
        }

        [Test]
        public void different_password_string_should_not_equal_hashed_password()
        {
            HashedPassword.FromHash(PasswordHash).MatchesPassword("yada").ShouldBeFalse();
        }
    }
}