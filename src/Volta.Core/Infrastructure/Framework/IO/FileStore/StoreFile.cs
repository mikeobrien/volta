using System;
using System.IO;

namespace Volta.Core.Infrastructure.Framework.IO.FileStore
{
    public class StoreFile
    {
        private const string GuidFileformat = "N";
        private const string DatePathFormat = @"yyyy\\MM\\dd";

        private readonly string _relativePermanentPath;
        private readonly string _relativeTransientPath;

        private StoreFile(Guid id)
        {
            Id = id;
            Name = id.ToString(GuidFileformat);
            Timestamp = GetTimestamp(id);
            _relativePermanentPath = Path.Combine(Timestamp.ToString(DatePathFormat), Name);
            _relativeTransientPath = Path.Combine(FileStore.TransientPath, Name);
        }

        public static StoreFile FromGuid(Guid id)
        {
            return new StoreFile(id);
        }

        public static StoreFile Create()
        {
            return new StoreFile(GenerateId());
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public DateTime Timestamp { get; private set; }

        public string TryGetExistingPath(string rootPath, out bool exists)
        {
            var fullPath = GetFullPath(rootPath, Lifespan.Permanent);
            if (File.Exists(fullPath))
            {
                exists = true;
                return fullPath;
            }
            fullPath = GetFullPath(rootPath, Lifespan.Transient);
            if (File.Exists(fullPath))
            {
                exists = true;
                return fullPath;
            }
            exists = false;
            return fullPath;
        }

        public void ReplaceWith(string rootPath, string path, Lifespan lifespan)
        {
            Delete(rootPath);
            EnsureDirectoryExists(rootPath, lifespan);
            File.Move(path, GetFullPath(rootPath, lifespan));
        }

        public bool Exists(string rootPath)
        {
            return Exists(rootPath, Lifespan.Permanent) || Exists(rootPath, Lifespan.Transient);
        }

        public bool Exists(string rootPath, Lifespan lifespan)
        {
            return File.Exists(GetFullPath(rootPath, lifespan));
        }

        public string GetExistingOrDefaultFullPath(string rootPath)
        {
            var fullPath = GetFullPath(rootPath, Lifespan.Permanent);
            return File.Exists(fullPath) ? fullPath : GetFullPath(rootPath, Lifespan.Transient);
        }

        public string GetFullPath(string rootPath, Lifespan lifespan)
        {
            return Path.Combine(rootPath, lifespan == Lifespan.Permanent ? 
                                                        _relativePermanentPath :
                                                        _relativeTransientPath);
        }

        public void EnsureDirectoryExists(string rootPath, Lifespan lifespan)
        {
            var path = Path.GetDirectoryName(GetFullPath(rootPath, lifespan));
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public Stream OpenRead(string rootPath)
        {
            bool exists;
            var path = TryGetExistingPath(rootPath, out exists);
            if (!exists) throw new FileNotFoundException(path);
            return File.OpenRead(path);
        }

        public void Overwrite(string rootPath, Stream data, Lifespan lifespan)
        {
            var path = GetFullPath(rootPath, lifespan);
            if (File.Exists(path)) File.Delete(path);
            else EnsureDirectoryExists(rootPath, lifespan);
            data.Save(path);
        }

        public void SetLifespan(string rootPath, Lifespan lifespan)
        {
            EnsureDirectoryExists(rootPath, lifespan);
            if (lifespan == Lifespan.Transient && Exists(rootPath, Lifespan.Permanent) && !Exists(rootPath, Lifespan.Transient))
                File.Move(GetFullPath(rootPath, Lifespan.Permanent), GetFullPath(rootPath, Lifespan.Transient));
            else if (lifespan == Lifespan.Permanent && Exists(rootPath, Lifespan.Transient) && !Exists(rootPath, Lifespan.Permanent))
                File.Move(GetFullPath(rootPath, Lifespan.Transient), GetFullPath(rootPath, Lifespan.Permanent));
        }

        public StoreFileInfo GetInfo(string rootPath)
        {
            bool exists;
            var path = TryGetExistingPath(rootPath, out exists);
            if (!exists) throw new FileNotFoundException(path);
            var info = new FileInfo(path);
            return new StoreFileInfo(Id, path, info.Length, info.CreationTime, GetLifespan(rootPath));
        }

        public void Delete(string rootPath)
        {
            bool exists;
            var path = TryGetExistingPath(rootPath, out exists);
            if (exists) File.Delete(path);
        }

        public Lifespan? GetLifespan(string rootPath)
        {
            var fullPath = GetFullPath(rootPath, Lifespan.Permanent);
            if (File.Exists(fullPath)) return Lifespan.Permanent;
            fullPath = GetFullPath(rootPath, Lifespan.Transient);
            if (File.Exists(fullPath)) return Lifespan.Transient;
            return null;
        }

        private static Guid GenerateId()
        {
            var guidArray = Guid.NewGuid().ToByteArray();

            var now = DateTime.UtcNow;

            var days = new TimeSpan(now.Ticks - new DateTime(1900, 1, 1).Ticks);
            var miliseconds = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));

            var daysArray = BitConverter.GetBytes(days.Days);
            var msecsArray = BitConverter.GetBytes((long)miliseconds.TotalMilliseconds);

            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }

        private static DateTime GetTimestamp(Guid id)
        {
            var daysArray = new byte[4];
            var msecsArray = new byte[4];
            var guidArray = id.ToByteArray();

            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);

            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            var days = BitConverter.ToInt32(daysArray, 0);
            var miliseconds = BitConverter.ToInt32(msecsArray, 0);

            var date = new DateTime(1900, 1, 1).AddDays(days);
            date = date.AddMilliseconds(miliseconds);

            return date;
        }
    }
}
