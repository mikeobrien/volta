using System.IO;
using System.Text;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.IO;

namespace Volta.Tests.Integration.Infrastructure.Framework.IO
{
    [TestFixture]
    public class TempFileTests
    {
        [Test]
        public void should_save_and_delete_file_from_the_temp_folder()
        {
            var data = "yada";
            var dataStream = new MemoryStream(Encoding.ASCII.GetBytes(data));
            string path;
            using (var file = new TempFile(dataStream))
            {
                path = file.Path;
                file.Path.StartsWith(Path.GetTempPath()).ShouldBeTrue();
                File.Exists(file.Path).ShouldBeTrue();
                File.ReadAllText(file.Path).ShouldEqual(data);
            }
            File.Exists(path).ShouldBeFalse();
        }
    }
}