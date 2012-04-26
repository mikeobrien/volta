using System;
using System.IO;

namespace Volta.Core.Infrastructure.Framework.IO
{
    public class TempFile : IDisposable
    {
        private readonly Lazy<string> _path;

        public TempFile(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            _path = new Lazy<string>(() => {
                var path = System.IO.Path.GetTempFileName();
                using (var file = File.OpenWrite(path)) stream.CopyTo(file);
                return path;
            });
        }

        public string Path { get { return _path.Value; } }

        public void Dispose()
        {
            if (_path.IsValueCreated && File.Exists(_path.Value)) File.Delete(_path.Value);
        }
    }
}