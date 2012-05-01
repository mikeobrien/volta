using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Latex;

namespace Volta.Tests.Integration.Infrastructure.Framework.Latex
{
    [TestFixture]
    public class LatexEngineTests
    {
        private readonly string _path = Path.GetFullPath("Integration/Infrastructure/Framework/Latex/template.razor");
        
        [Test]
        public void should_create_pdf()
        {
            var path = new LatexEngine().GeneratePdf(_path, new LatexOptions { OutputDirectory = Path.GetDirectoryName(_path)});
            Debug.Print(path);
            File.Exists(_path).ShouldBeTrue();
            File.Exists(path).ShouldBeTrue();
            Directory.GetFiles(Path.GetDirectoryName(_path), Path.GetFileNameWithoutExtension(_path) + ".*")
                .Where(x => x != _path && x != path).Any().ShouldBeFalse();
            path.ShouldEqual(Path.Combine(Path.GetDirectoryName(_path), Path.GetFileNameWithoutExtension(_path) + ".pdf"));
        }
    }
}