using FubuMVC.Core.Runtime;
using NSubstitute;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Web;

namespace Volta.Tests.Unit.Infrastructure.Framework.Web
{
    [TestFixture]
    public class FubuTokenStoreTests
    {
        public class Token {}

        [Test]
        public void Should_Set_Token()
        {
            var sessionState = Substitute.For<ISessionState>();
            var tokenStore = new FubuTokenStore<Token>(sessionState);
            var token = new Token();
            tokenStore.Set(token);
            sessionState.Received().Set(token);
        }

        [Test]
        public void Should_Get_Existing_Token()
        {
            var sessionState = Substitute.For<ISessionState>();
            var tokenStore = new FubuTokenStore<Token>(sessionState);
            var token = new Token();
            sessionState.Get<Token>().Returns(token);
            var result = tokenStore.Get();
            token.ShouldEqual(result);
        }

        [Test]
        public void Should_Indicate_Token_Exists()
        {
            var sessionState = Substitute.For<ISessionState>();
            var tokenStore = new FubuTokenStore<Token>(sessionState);
            sessionState.Get<Token>().Returns(new Token());
            tokenStore.Exists().ShouldBeTrue();
        }

        [Test]
        public void Should_Indicate_Token_Doesent_Exist()
        {
            var sessionState = Substitute.For<ISessionState>();
            var tokenStore = new FubuTokenStore<Token>(sessionState);
            sessionState.Get<Token>().Returns((Token)null);
            tokenStore.Exists().ShouldBeFalse();
        }

        [Test]
        public void Should_Clear_Token()
        {
            var sessionState = Substitute.For<ISessionState>();
            var tokenStore = new FubuTokenStore<Token>(sessionState);
            tokenStore.Clear();
            sessionState.Received().Set((Token)null);
        }
    }
}