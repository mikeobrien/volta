namespace Volta.Core.Infrastructure.Framework.Web.Fubu
{
    public class DownloadDataModel
    {
        public enum DownloadType { File, Data }

        public DownloadDataModel(string path, string filename, string mimeType)
        {
            Type = DownloadType.File;
            Path = path;
            Filename = filename;
            MimeType = mimeType;
        }

        public DownloadDataModel(byte[] data, string filename, string mimeType)
        {
            Type = DownloadType.Data;
            Data = data;
            Filename = filename;
            MimeType = mimeType;
        }

        public DownloadType Type { get; private set; }
        public string Path { get; private set; }
        public byte[] Data { get; private set; }
        public string Filename { get; private set; }
        public string MimeType { get; private set; }
    }
}