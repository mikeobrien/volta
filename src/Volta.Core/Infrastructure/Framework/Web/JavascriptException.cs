using System;

namespace Volta.Core.Infrastructure.Framework.Web
{
    public class JavascriptError : Exception
    {
        public JavascriptError(string userAgent, string source, string line, string message) :
            base(string.Format("Error at {0}:{1}: \r\n\r\n{2}\r\n\r\n{3}", source, line, userAgent, message))
        {
            Source = source;
            Line = line;
            UserAgent = userAgent;
        }

        public string UserAgent { get; set; }
        public string Line { get; private set; }
    }
}
