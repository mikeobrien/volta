using FubuCore;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Web;

namespace Volta.Tests.Unit.Infrastructure.Web
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
    }
}