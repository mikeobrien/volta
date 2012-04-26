using System.Linq;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Tests.Integration.Infrastructure.Framework.Data
{
    [TestFixture]
    public class IniFileTests
    {
        public string Data = @"[section1]
setting1=foo
setting2=bar
[section2]
setting=yada";
         
        [Test]
        public void should_read_ini_file()
        {
            var ini = new IniFile(Data);

            ini.Count().ShouldEqual(2);
            var section = ini.ElementAt(0);
            section.Name.ShouldEqual("section1");
            section.Values.Count.ShouldEqual(2);
            section.Values["setting1"].ShouldEqual("foo");
            section.Values["setting2"].ShouldEqual("bar");

            section = ini.ElementAt(1);
            section.Name.ShouldEqual("section2");
            section.Values.Count.ShouldEqual(1);
            section.Values["setting"].ShouldEqual("yada");
        }
    }
}