using System.IO;
using FubuMVC.Core;

namespace Volta.Core.Infrastructure.Framework.Web
{
    public class ContentFile : IContentFile
    {
        private readonly CurrentRequest _request;
        public const string ContentFolder = "content";

        public ContentFile(CurrentRequest request)
        {
            _request = request;
        }

        public string ReadAllText(string relativePath)
        {
            return File.ReadAllText(Path.Combine(_request.PhysicalApplicationPath, ContentFolder, relativePath));
        }
    }
}