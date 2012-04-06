using System;

namespace Volta.Core.Infrastructure.Framework.Web
{
    public class JavascriptError : Exception
    {
        public JavascriptError(string source, string line, string message) : 
            base(string.Format("Error at {0}:{1}: {2}", source, line, message))
        {
            Source = source;
            Line = line;
        }

        public string Line { get; private set; }
    }
}
