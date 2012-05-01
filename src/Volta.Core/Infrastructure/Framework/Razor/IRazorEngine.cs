namespace Volta.Core.Infrastructure.Framework.Razor
{
    public interface IRazorEngine
    {
        string Transform<T>(string template, T model);
    }
}