using System;
using System.IO;

namespace Volta.Core.Infrastructure.Framework.IO.FileStore
{
    public class StoreFileSaveException : Exception
    { public StoreFileSaveException(Guid id, Exception innerException) : base(string.Format("Unable to save repository file {0}.", id), innerException) { } }

    public class StoreFileImportException : Exception
    { public StoreFileImportException(string path, Exception innerException) : base(string.Format("Unable to import repository file '{0}'.", path), innerException) { } }

    public class StoreFileLifespanException : Exception
    { public StoreFileLifespanException(Guid id, Exception innerException) : base(string.Format("Unable to set lifespan for repository file {0}.", id), innerException) { } }

    public class StoreFileAccessException : Exception
    { public StoreFileAccessException(Guid id, Exception innerException) : base(string.Format("Unable to access repository file {0}.", id), innerException) { } }

    public class StoreFileDeletionException : Exception
    { public StoreFileDeletionException(Guid id, Exception innerException) : base(string.Format("Unable to delete repository file id {0}.", id), innerException) { } }

    public interface IFileStore
    {
        Stream Open(Guid id);
        Guid Save(Stream data, Lifespan lifespan);
        Guid Save(string data, Lifespan lifespan);
        Guid Import(string path, Lifespan lifespan);
        StoreFileInfo Create(Lifespan lifespan);
        string GetPath(Guid id);
        void SetLifespan(Guid id, Lifespan lifespan);
        bool Exists(Guid id);
        StoreFileInfo GetInfo(Guid id);
        void Delete(Guid id);
        void PurgeTransientFiles(TimeSpan maxAge);
    }
}


