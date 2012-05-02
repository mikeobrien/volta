using System;
using Volta.Core.Domain;
using Volta.Core.Infrastructure.Framework.IO.FileStore;
using Volta.Core.Infrastructure.Framework.Web;
using Volta.Core.Infrastructure.Framework.Web.Fubu;

namespace Volta.Web
{
    public class DownloadGetRequest
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
    }

    public class DownloadGetHandler
    {
        private readonly IFileStore _fileStore;

        public DownloadGetHandler(IFileStore fileStore)
        {
            _fileStore = fileStore;
        }

        public DownloadDataModel ExecuteDownload_Id_Filename(DownloadGetRequest request)
        {
            var mimeType = MimeType.ResolveMimeTypeFromFilename(request.Filename);
            if (!_fileStore.Exists(request.Id)) throw new NotFoundException("File");
            return new DownloadDataModel(
                _fileStore.GetPath(request.Id), 
                request.Filename,
                !string.IsNullOrEmpty(mimeType) ? mimeType : "application/octet-stream");
        }
    }
}