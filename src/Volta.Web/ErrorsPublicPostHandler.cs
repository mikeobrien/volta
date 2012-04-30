using Volta.Core.Infrastructure.Framework.Logging;
using Volta.Core.Infrastructure.Framework.Web;

namespace Volta.Web
{
    public class ErrorRequest
    {
        public string UserAgent { get; set; }
        public string source { get; set; }
        public string line { get; set; }
        public string message { get; set; }
    }

    public class ErrorsPublicPostHandler
    {
        private readonly ILogger _logger;

        public ErrorsPublicPostHandler(ILogger logger)
        {
            _logger = logger;
        }

        public void ExecuteErrors(ErrorRequest request)
        {
            _logger.Write(new JavascriptError(request.UserAgent, request.source, request.line, request.message));
        }
    }
}