namespace Volta.Core.Infrastructure.Framework.Razor
{
    public class RazorEngine : IRazorEngine
    {
        public string Transform<T>(string template, T model)
        {
            return global::RazorEngine.Razor.Parse(template, model);
        }
    }
}