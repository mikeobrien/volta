using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Should;

namespace Volta.Tests.Unit.Infrastructure.Framework.IO
{
    [TestFixture]
    public class FileCacheTests
    {
        private const string FileData = "Some file yo.";
        private readonly string _fileCachePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        private static Stream GetFileStream()
        {
            return new MemoryStream(Encoding.ASCII.GetBytes(FileData));
        }

        [SetUp]
        public void Setup()
        {
            Directory.CreateDirectory(_fileCachePath);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_fileCachePath, true);
        }

        [Test]
        public void should_cache_file()
        {
            var cache = new FileCache(_fileCachePath);
            var path = cache.Save(GetFileStream());
            path.ShouldNotBeNull();
            File.Exists(path).ShouldBeTrue();
            var contents = File.ReadAllText(path);
            contents.ShouldEqual(FileData);
        }
    }

    public class FileCache
    {
        private readonly string _path;

        public FileCache(string path)
        {
            _path = path;
        }

        public string Save(Stream stream)
        {
            var path = Path.Combine(_path, Guid.NewGuid().ToString("N"));
            using (var file = File.Create(path)) { stream.CopyTo(file); }
            return path;
        }
    }
}