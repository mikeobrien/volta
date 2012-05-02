using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.IO.FileStore;

namespace Volta.Tests.Integration.Infrastructure.Framework.IO.FileStore
{
    [TestFixture]
    public class FileSystemFileRepositoryTests
    {
        private static readonly string RepositoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        private readonly Core.Infrastructure.Framework.IO.FileStore.FileStore _repo =
            new Core.Infrastructure.Framework.IO.FileStore.FileStore(RepositoryPath);

        private const string FileContents = "This is a file!";
        private Stream _fileStream;

        [SetUp]
        public void Setup()
        {
            if (Directory.Exists(RepositoryPath)) Directory.Delete(RepositoryPath, true);
            Directory.CreateDirectory(RepositoryPath);
            _fileStream = new MemoryStream(Encoding.ASCII.GetBytes(FileContents));
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(RepositoryPath)) Directory.Delete(RepositoryPath, true);
        }

        [Test]
        public void Should_Save_String_File_And_Open_It()
        {
            var id = _repo.Save(FileContents, Lifespan.Permanent);
            var contents = _repo.Open(id);
            contents.ReadAllText().ShouldEqual(FileContents);
        }

        [Test]
        public void Should_Save_Permanent_File_And_Open_It()
        {
            var id = _repo.Save(_fileStream, Lifespan.Permanent);
            var contents = _repo.Open(id);
            contents.ReadAllText().ShouldEqual(FileContents);
        }

        [Test]
        public void Should_Save_Transient_File_And_Open_It()
        {
            var id = _repo.Save(_fileStream, Lifespan.Transient);
            var contents = _repo.Open(id);
            contents.ReadAllText().ShouldEqual(FileContents);
        }

        [Test]
        public void Should_Generate_Import_Path()
        {
            var fileInfo = _repo.Create(Lifespan.Transient);
            fileInfo.Path.Directory().DirectoryExists().ShouldBeTrue();
            fileInfo.Path.FileExists().ShouldBeFalse();
            fileInfo.Path.Filename().IsNullOrEmpty().ShouldBeFalse();
        }

        [Test]
        public void Should_Import_Transient_File()
        {
            var fileInfo = _repo.Create(Lifespan.Transient);
            File.WriteAllText(fileInfo.Path, FileContents);
            var id = _repo.Import(fileInfo.Path, Lifespan.Transient);
            id.ShouldNotEqual(Guid.Empty);
            fileInfo.Path.FileExists().ShouldBeFalse();
            _repo.Open(id).ReadAllText().ShouldEqual(FileContents);
        }

        [Test]
        public void Should_Import_Permanent_File()
        {
            var fileInfo = _repo.Create(Lifespan.Transient);
            File.WriteAllText(fileInfo.Path, FileContents);
            var id = _repo.Import(fileInfo.Path, Lifespan.Permanent);
            id.ShouldNotEqual(Guid.Empty);
            fileInfo.Path.FileExists().ShouldBeFalse();
            _repo.Open(id).ReadAllText().ShouldEqual(FileContents);
        }

        [Test]
        public void Should_Export_Permanent_File()
        {
            var id = _repo.Save(_fileStream, Lifespan.Permanent);
            var path = _repo.GetPath(id);
            path.FileExists().ShouldBeTrue();
            path.ReadAllText().ShouldEqual(FileContents);
        }

        [Test]
        public void Should_Export_Transient_File()
        {
            var id = _repo.Save(_fileStream, Lifespan.Transient);
            var path = _repo.GetPath(id);
            path.FileExists().ShouldBeTrue();
            path.ReadAllText().ShouldEqual(FileContents);
        }

        [Test]
        public void Should_Make_File_Permanent()
        {
            var id = _repo.Save(_fileStream, Lifespan.Transient);
            var transientPath = _repo.GetPath(id);
            transientPath.FileExists().ShouldBeTrue();
            _repo.SetLifespan(id, Lifespan.Permanent);
            var permanentPath = _repo.GetPath(id);
            transientPath.ShouldNotEqual(permanentPath);
            transientPath.FileExists().ShouldBeFalse();
            permanentPath.FileExists().ShouldBeTrue();
        }

        [Test]
        public void Should_Make_File_Transient()
        {
            var id = _repo.Save(_fileStream, Lifespan.Permanent);
            var permanentPath = _repo.GetPath(id);
            permanentPath.FileExists().ShouldBeTrue();
            _repo.SetLifespan(id, Lifespan.Transient);
            var transientPath = _repo.GetPath(id);
            transientPath.ShouldNotEqual(permanentPath);
            permanentPath.FileExists().ShouldBeFalse();
            transientPath.FileExists().ShouldBeTrue();
        }

        [Test]
        public void Transient_File_Should_Exist()
        {
            var id = _repo.Save(_fileStream, Lifespan.Transient);
            _repo.Exists(id).ShouldBeTrue();
            _repo.GetPath(id).FileExists().ShouldBeTrue();
        }

        [Test]
        public void Permanent_File_Should_Exist()
        {
            var id = _repo.Save(_fileStream, Lifespan.Permanent);
            _repo.Exists(id).ShouldBeTrue();
            _repo.GetPath(id).FileExists().ShouldBeTrue();
        }

        [Test]
        public void File_Should_Not_Exist()
        {
            _repo.Exists(Guid.NewGuid()).ShouldBeFalse();
        }

        [Test]
        public void Get_Permanent_File_Info()
        {
            var id = _repo.Save(_fileStream, Lifespan.Permanent);
            var info = _repo.GetInfo(id);
            info.Size.ShouldEqual(FileContents.Length);
            info.Lifespan.ShouldEqual(Lifespan.Permanent);
            (info.Created > DateTime.Now.AddSeconds(-5)).ShouldBeTrue();
            (info.Created < DateTime.Now.AddSeconds(5)).ShouldBeTrue();
        }

        [Test]
        public void Get_Transient_File_Info()
        {
            var id = _repo.Save(_fileStream, Lifespan.Transient);
            var info = _repo.GetInfo(id);
            info.Size.ShouldEqual(FileContents.Length);
            info.Lifespan.ShouldEqual(Lifespan.Transient);
            (info.Created > DateTime.Now.AddSeconds(-5)).ShouldBeTrue();
            (info.Created < DateTime.Now.AddSeconds(5)).ShouldBeTrue();
        }

        [Test]
        public void Should_Delete_Permanent_File()
        {
            var id = _repo.Save(_fileStream, Lifespan.Permanent);
            _repo.Exists(id).ShouldBeTrue();
            _repo.Delete(id);
            _repo.Exists(id).ShouldBeFalse();
        }

        [Test]
        public void Should_Delete_Transient_File()
        {
            var id = _repo.Save(_fileStream, Lifespan.Transient);
            _repo.Exists(id).ShouldBeTrue();
            _repo.Delete(id);
            _repo.Exists(id).ShouldBeFalse();
        }

        [Test]
        public void Should_Purge_All_Transient_Files()
        {
            var permanentId1 = _repo.Save(FileContents.ToStream(), Lifespan.Permanent);
            var transientId1 = _repo.Save(FileContents.ToStream(), Lifespan.Transient);
            var permanentId2 = _repo.Save(FileContents.ToStream(), Lifespan.Permanent);
            var transientId3 = _repo.Save(FileContents.ToStream(), Lifespan.Transient);

            _repo.Exists(permanentId1).ShouldBeTrue();
            _repo.Exists(transientId1).ShouldBeTrue();
            _repo.Exists(permanentId2).ShouldBeTrue();
            _repo.Exists(transientId3).ShouldBeTrue();

            _repo.PurgeTransientFiles(TimeSpan.MinValue);

            _repo.Exists(permanentId1).ShouldBeTrue();
            _repo.Exists(transientId1).ShouldBeFalse();
            _repo.Exists(permanentId2).ShouldBeTrue();
            _repo.Exists(transientId3).ShouldBeFalse();
        }

        [Test]
        public void Should_Purge_Older_Transient_Files()
        {
            var permanentId1 = _repo.Save(FileContents.ToStream(), Lifespan.Permanent);
            var transientId1 = _repo.Save(FileContents.ToStream(), Lifespan.Transient);
            var permanentId2 = _repo.Save(FileContents.ToStream(), Lifespan.Permanent);
            var transientId3 = _repo.Save(FileContents.ToStream(), Lifespan.Transient);

            _repo.Exists(permanentId1).ShouldBeTrue();
            _repo.Exists(transientId1).ShouldBeTrue();
            _repo.Exists(permanentId2).ShouldBeTrue();
            _repo.Exists(transientId3).ShouldBeTrue();

            _repo.PurgeTransientFiles(TimeSpan.FromSeconds(30));

            _repo.Exists(permanentId1).ShouldBeTrue();
            _repo.Exists(transientId1).ShouldBeTrue();
            _repo.Exists(permanentId2).ShouldBeTrue();
            _repo.Exists(transientId3).ShouldBeTrue();
        }
    }

    static class FluentExtensions
    {
        public static string ReadAllText(this Stream stream)
        {
            using (var reader = new StreamReader(stream)) { return reader.ReadToEnd(); }
        }

        public static bool FileExists(this string path)
        {
            return File.Exists(path);
        }

        public static bool FileIsEmpty(this string path)
        {
            return new System.IO.FileInfo(path).Length == 0;
        }

        public static string ReadAllText(this string path)
        {
            return File.ReadAllText(path);
        }

        public static string Filename(this string path)
        {
            return Path.GetFileName(path);
        }

        public static string Directory(this string path)
        {
            return Path.GetDirectoryName(path);
        }

        public static bool DirectoryExists(this string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static Stream ToStream(this string value)
        {
            return new MemoryStream(Encoding.ASCII.GetBytes(value));
        }
    }
}


