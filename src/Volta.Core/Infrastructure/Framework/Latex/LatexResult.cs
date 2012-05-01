namespace Volta.Core.Infrastructure.Framework.Latex
{
    public class LatexResult
    {
        public LatexResult(bool error, string output, string path)
        {
            Error = error;
            Output = output;
            Path = path;
        }
        public bool Error { get; private set; }
        public string Output { get; private set; }
        public string Path { get; private set; }
    }
}