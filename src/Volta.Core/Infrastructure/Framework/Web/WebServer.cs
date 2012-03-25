using System.Web;

namespace Volta.Core.Infrastructure.Framework.Web
{
    public class WebServer : IWebServer
    {
        public bool IgnoreErrorStatus
        {
            get { return HttpContext.Current.Response.TrySkipIisCustomErrors; }
            set { HttpContext.Current.Response.TrySkipIisCustomErrors = value; }
        }
    }
}