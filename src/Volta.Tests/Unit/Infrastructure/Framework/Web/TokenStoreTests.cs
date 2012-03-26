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
        public void should_set_token()
        {
            var sessionState = Substitute.For<ISession>();
            var tokenStore = new TokenStore<Token>(sessionState);
            var token = new Token();
            tokenStore.Set(token);
            sessionState.Received()[TokenStore<Token>.SessionKey] = token;
        }

        [Test]
        public void should_get_existing_token()
        {
            var sessionState = Substitute.For<ISession>();
            var tokenStore = new TokenStore<Token>(sessionState);
            var token = new Token();
            sessionState[TokenStore<Token>.SessionKey].Returns(token);
            var result = tokenStore.Get();
            token.ShouldEqual(result);
        }

        [Test]
        public void should_indicate_token_exists()
        {
            var sessionState = Substitute.For<ISession>();
            var tokenStore = new TokenStore<Token>(sessionState);
            sessionState[TokenStore<Token>.SessionKey].Returns(new Token());
            tokenStore.Exists().ShouldBeTrue();
        }

        [Test]
        public void should_indicate_token_doesent_exist()
        {
            var sessionState = Substitute.For<ISession>();
            var tokenStore = new TokenStore<Token>(sessionState);
            sessionState[TokenStore<Token>.SessionKey].Returns((Token)null);
            tokenStore.Exists().ShouldBeFalse();
        }

        [Test]
        public void should_clear_token()
        {
            var sessionState = Substitute.For<ISession>();
            var tokenStore = new TokenStore<Token>(sessionState);
            tokenStore.Clear();
            sessionState.Received()[TokenStore<Token>.SessionKey] = (Token)null;
        }
    }
}