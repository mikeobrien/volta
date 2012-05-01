using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Latex;

namespace Volta.Tests.Unit.Infrastructure.Framework.Latex
{
    [TestFixture]
    public class LatexArgumentFactoryTest
    {
        [Test]
        public void should_create_command_line_arguments()
        {
            new LatexArgumentFactory().Create(new LatexOptions
            {
                ParamSize = 5,
                Recorder = true,
                Trace = "info",
                Version = false
            }, "d:/yada/yada.tex").ShouldEqual("-param-size=5 -recorder -trace=info d:/yada/yada.tex");
        } 
    }
}