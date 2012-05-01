using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Latex;

namespace Volta.Tests.Integration.Infrastructure.Framework.Latex
{
    [TestFixture]
    public class LatexTests
    {
        private readonly string _path = Path.GetFullPath("Integration/Infrastructure/Framework/Latex/template.razor");
        
        [Test]
        public void should_create_pdf()
        {
            var result = new LatexEngine().GeneratePdf(_path, new LatexOptions { OutputDirectory = Path.GetDirectoryName(_path)});
            Debug.Print(result.Path);
            Debug.Print(result.Output);
            File.Exists(_path).ShouldBeTrue();
            File.Exists(result.Path).ShouldBeTrue();
            Directory.GetFiles(Path.GetDirectoryName(_path), Path.GetFileNameWithoutExtension(_path) + ".*")
                .Where(x => x != _path && x != result.Path).Any().ShouldBeFalse();
            result.Path.ShouldEqual(Path.Combine(Path.GetDirectoryName(_path), Path.GetFileNameWithoutExtension(_path) + ".pdf"));
            result.Error.ShouldBeFalse();
        }
    }
}