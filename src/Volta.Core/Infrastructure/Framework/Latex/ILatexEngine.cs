namespace Volta.Core.Infrastructure.Framework.Latex
{
    public interface ILatexEngine
    {
        string GeneratePdf(string path, LatexOptions options = null);
    }
}