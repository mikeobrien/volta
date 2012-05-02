using System;

namespace Volta.Core.Infrastructure.Framework.IO.FileStore
{
    public enum Lifespan
    {
        Permanent,
        Transient
    }

    public class StoreFileInfo
    {
        public StoreFileInfo(Guid id, string path, long size, DateTime created, Lifespan? lifespan)
        {
            Id = id;
            Path = path;
            Size = size;
            Created = created;
            Lifespan = lifespan;
        }

        public Guid Id { get; private set; }
        public string Path { get; private set; }
        public long Size { get; private set; }
        public DateTime Created { get; private set; }
        public Lifespan? Lifespan { get; private set; }
    }
}


