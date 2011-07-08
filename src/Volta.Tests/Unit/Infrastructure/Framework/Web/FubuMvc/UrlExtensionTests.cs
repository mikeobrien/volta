using FubuCore;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Web.FubuMvc;

namespace Volta.Tests.Unit.Infrastructure.Framework.Web.FubuMvc
{
    [TestFixture]
    public class FubuUrlExtensionTests
    {
        public class Oh { public string Hai { get; set; } }

        public const string BaseUrl = "http://www.yada.com/somepage";
        public const string Value = "SomeValue";

        [Test]
        public void Should_Add_Querystring_With_Questionmark_Seperator()
        {
            var url = BaseUrl.AppendQueryStringValueFor<Oh>(x => x.Hai, Value);
            url.ShouldEqual("{0}?Hai={1}".ToFormat(BaseUrl, Value));
        }

        [Test]
        public void Should_Add_Querystring_Without_Questionmark_Seperator()
        {
            var url = (BaseUrl + "?").AppendQueryStringValueFor<Oh>(x => x.Hai, Value);
            url.ShouldEqual("{0}?Hai={1}".ToFormat(BaseUrl, Value));
        }

        [Test]
        public void Should_Add_Querystring_With_Ampersand_Seperator()
        {
            var url = (BaseUrl + "?There=Yada").AppendQueryStringValueFor<Oh>(x => x.Hai, Value);
            url.ShouldEqual("{0}?There=Yada&Hai={1}".ToFormat(BaseUrl, Value));
        }

        [Test]
        public void Should_Add_Querystring_Without_Ampersand_Seperator()
        {
            var url = (BaseUrl + "?There=Yada&").AppendQueryStringValueFor<Oh>(x => x.Hai, Value);
            url.ShouldEqual("{0}?There=Yada&Hai={1}".ToFormat(BaseUrl, Value));
        }

        [Test]
        public void Should_Add_Querystring_With_Trailing_Whitespace()
        {
            var url = (BaseUrl + "?There=Yada&   ").AppendQueryStringValueFor<Oh>(x => x.Hai, Value);
            url.ShouldEqual("{0}?There=Yada&Hai={1}".ToFormat(BaseUrl, Value));
        }

        [Test]
        public void Should_Match_Url()
        {
            var url = "/some/url";
            url.MatchesUrl(url).ShouldBeTrue();
        }

        [Test]
        public void Should_Match_Url_With_Different_Case()
        {
            var url = "/some/url";
            url.MatchesUrl(url.ToUpper()).ShouldBeTrue();
        }

        [Test]
        public void Should_Match_Parameterized_Url()
        {
            "/some/url/{With}/{Paramaters}".MatchesUrl("/some/url/with/parameters").ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Match_Different_Urls()
        {
            "/some/other/url/{With}/{Paramaters}".MatchesUrl("/some/url/with/parameters").ShouldBeFalse();
        }
    }
}