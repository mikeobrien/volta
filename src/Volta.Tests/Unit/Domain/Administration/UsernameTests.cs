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
        public void Should_Equal_Equivalent_Mixed_Case_String()
        {
            var stringUsename = "SomeUser";
            var objectUsername = new Username("someuser");
            (stringUsename == objectUsername).ShouldBeTrue();
            (objectUsername == stringUsename).ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Equal_Different_String()
        {
            var stringUsename = "SomeUser2";
            var objectUsername = new Username("someuser");
            (stringUsename != objectUsername).ShouldBeTrue();
            (objectUsername != stringUsename).ShouldBeTrue();
        }
        [Test]
        public void Should_Equal_Equivalent_Username()
        {
            (new Username("someuser") == new Username("someuser")).ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Equal_Different_Username()
        {
            (new Username("someuser") != new Username("someuser2")).ShouldBeTrue();
        }

        [Test]
        public void Should_Implicitely_Cast_Username_To_String()
        {
            string stringUsename = new Username("someuser");
            stringUsename.ShouldEqual("someuser");
        }

        [Test]
        public void Should_Implicitely_Cast_String_To_Username()
        {
            Username objectUsername = "someuser";
            objectUsername.ToString().ShouldEqual("someuser");
        }
    }
}