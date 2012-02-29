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
            var sessionState = Substitute.For<ISession>();
            var tokenStore = new TokenStore<Token>(sessionState);
            var token = new Token();
            tokenStore.Set(token);
            sessionState.Received()[TokenStore<Token>.SessionKey] = token;
        }

        [Test]
        public void Should_Get_Existing_Token()
        {
            var sessionState = Substitute.For<ISession>();
            var tokenStore = new TokenStore<Token>(sessionState);
            var token = new Token();
            sessionState[TokenStore<Token>.SessionKey].Returns(token);
            var result = tokenStore.Get();
            token.ShouldEqual(result);
        }

        [Test]
        public void Should_Indicate_Token_Exists()
        {
            var sessionState = Substitute.For<ISession>();
            var tokenStore = new TokenStore<Token>(sessionState);
            sessionState[TokenStore<Token>.SessionKey].Returns(new Token());
            tokenStore.Exists().ShouldBeTrue();
        }

        [Test]
        public void Should_Indicate_Token_Doesent_Exist()
        {
            var sessionState = Substitute.For<ISession>();
            var tokenStore = new TokenStore<Token>(sessionState);
            sessionState[TokenStore<Token>.SessionKey].Returns((Token)null);
            tokenStore.Exists().ShouldBeFalse();
        }

        [Test]
        public void Should_Clear_Token()
        {
            var sessionState = Substitute.For<ISession>();
            var tokenStore = new TokenStore<Token>(sessionState);
            tokenStore.Clear();
            sessionState.Received()[TokenStore<Token>.SessionKey] = (Token)null;
        }
    }
}