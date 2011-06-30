using System;
using System.IO;
using FubuMVC.Core;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Web;

namespace Volta.Tests.Integration.Infrastructure.Web
{
    [TestFixture]
    public class ContentFileTests
    {
        private const string ContentFileData = "<html></html>";
        private string _webRoot;
        private string _contentFilename;

        [SetUp]
        public void Setup()
        {
            _webRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var contentFolder = Path.Combine(_webRoot, ContentFile.ContentFolder);
            Directory.CreateDirectory(contentFolder);
            _contentFilename = Guid.NewGuid().ToString();
            File.WriteAllText(Path.Combine(contentFolder, _contentFilename), ContentFileData);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_webRoot, true);
        }

        [Test]
        public void Should_Return_Contents_Of_Content_File()
        {
            var contentFile = new ContentFile(new CurrentRequest { PhysicalApplicationPath = _webRoot});
            contentFile.ReadAllText(_contentFilename).ShouldEqual(ContentFileData);
        }
    }
}