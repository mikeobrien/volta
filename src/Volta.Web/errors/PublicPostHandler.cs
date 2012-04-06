using Volta.Core.Infrastructure.Framework.Logging;
using Volta.Core.Infrastructure.Framework.Web;

namespace Volta.Web.Errors
{
    public class ErrorRequest
    {
        public string Source { get; set; }
        public string Line { get; set; }
        public string Message { get; set; }
    }

    public class PublicPostHandler
    {
        private readonly ILogger _logger;

        public PublicPostHandler(ILogger logger)
        {
            _logger = logger;
        }

        public void Execute(ErrorRequest request)
        {
            _logger.Write(new JavascriptError(request.Source, request.Line, request.Message));
        }
    }
}