using NUnit.Framework;
using Should;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Tests.Unit.Domain.Administration
{
    [TestFixture]
    public class UsernameTests
    {
        [Test]
        public void should_equal_equivalent_mixed_case_string()
        {
            var stringUsename = "SomeUser";
            var objectUsername = new Username("someuser");
            (stringUsename == objectUsername).ShouldBeTrue();
            (objectUsername == stringUsename).ShouldBeTrue();
        }

        [Test]
        public void should_not_equal_different_string()
        {
            var stringUsename = "SomeUser2";
            var objectUsername = new Username("someuser");
            (stringUsename != objectUsername).ShouldBeTrue();
            (objectUsername != stringUsename).ShouldBeTrue();
        }
        [Test]
        public void should_equal_equivalent_username()
        {
            (new Username("someuser") == new Username("someuser")).ShouldBeTrue();
        }

        [Test]
        public void should_not_equal_different_username()
        {
            (new Username("someuser") != new Username("someuser2")).ShouldBeTrue();
        }

        [Test]
        public void should_implicitely_cast_username_to_string()
        {
            string stringUsename = new Username("someuser");
            stringUsename.ShouldEqual("someuser");
        }

        [Test]
        public void should_implicitely_cast_string_to_username()
        {
            Username objectUsername = "someuser";
            objectUsername.ToString().ShouldEqual("someuser");
        }
    }
}