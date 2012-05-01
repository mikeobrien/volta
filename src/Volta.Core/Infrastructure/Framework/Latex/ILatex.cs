namespace Volta.Core.Infrastructure.Framework.Latex
{
    public interface ILatex
    {
        LatexResult GeneratePdf(string path, LatexOptions options = null);
    }
}