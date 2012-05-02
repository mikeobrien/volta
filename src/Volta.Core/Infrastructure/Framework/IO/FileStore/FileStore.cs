using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Volta.Core.Infrastructure.Framework.IO.FileStore
{
    public class FileStore : IFileStore
    {
        private static readonly string DefaultPath = 
            Path.Combine(Path.GetTempPath(), string.Format("{0}-{1}-2C2CC19A-EAE6-476E-B823-02583825C28A", 
                typeof(FileStore).Name, Assembly.GetExecutingAssembly().GetName().Name));
        private readonly string _rootPath;
        public const string TransientPath = "transient";

        public FileStore() : this(null) {}

        public FileStore(string rootPath)
        {
            _rootPath = !string.IsNullOrEmpty(rootPath) ? rootPath : DefaultPath;
        }

        public Stream Open(Guid id)
        {
            try
            {
                return StoreFile.FromGuid(id).OpenRead(_rootPath);
            }
            catch (Exception e)
            {
                throw new StoreFileAccessException(id, e);
            }
        }

        public Guid Save(string data, Lifespan lifespan)
        {
            return Save(new MemoryStream(Encoding.ASCII.GetBytes(data)), lifespan);
        }

        public Guid Save(Stream data, Lifespan lifespan)
        {
            var file = StoreFile.Create();
            try
            {
                StoreFile.FromGuid(file.Id).Overwrite(_rootPath, data, lifespan);
                return file.Id;
            }
            catch (Exception e)
            {
                throw new StoreFileSaveException(file.Id, e);
            }
        }

        public Guid Import(string path, Lifespan lifespan)
        {
            try
            {
                var file = StoreFile.Create();
                file.ReplaceWith(_rootPath, path, lifespan);
                return file.Id;
            }
            catch (Exception e)
            {
                throw new StoreFileImportException(path, e);
            }
        }

        public StoreFileInfo Create(Lifespan lifespan)
        {
            var file = StoreFile.Create();
            file.EnsureDirectoryExists(_rootPath, lifespan);
            return new StoreFileInfo(file.Id, file.GetFullPath(_rootPath, lifespan), 0, DateTime.MinValue, lifespan);
        }

        public string GetPath(Guid id)
        {
            try
            {
                bool exists;
                var path = StoreFile.FromGuid(id).TryGetExistingPath(_rootPath, out exists);
                if (!exists) throw new StoreFileAccessException(id, new FileNotFoundException("Could not find file.", path));
                return path;
            }
            catch (Exception e)
            {
                throw new StoreFileAccessException(id, e);
            }
        }

        public void SetLifespan(Guid id, Lifespan lifespan)
        {
            try
            {
                StoreFile.FromGuid(id).SetLifespan(_rootPath, lifespan);
            }
            catch (Exception e)
            {
                throw new StoreFileLifespanException(id, e);
            }
        }

        public bool Exists(Guid id)
        {
            try
            {
                return StoreFile.FromGuid(id).Exists(_rootPath);
            }
            catch (Exception e)
            {
                throw new StoreFileAccessException(id, e);
            }
        }

        public StoreFileInfo GetInfo(Guid id)
        {
            try
            {
                return StoreFile.FromGuid(id).GetInfo(_rootPath);
            }
            catch (Exception e)
            {
                throw new StoreFileAccessException(id, e);
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                StoreFile.FromGuid(id).Delete(_rootPath);
            }
            catch (Exception e)
            {
                throw new StoreFileDeletionException(id ,e);
            }
        }

        public void PurgeTransientFiles(TimeSpan maxAge)
        {
            var files = Directory.EnumerateFiles(Path.Combine(_rootPath, TransientPath));
            foreach (var file in files)
            {
                try
                {
                    var created = File.GetCreationTime(file);
                    if ((DateTime.Now - created) >= maxAge) File.Delete(file);
                } catch {}
            }
        }
    }
}


