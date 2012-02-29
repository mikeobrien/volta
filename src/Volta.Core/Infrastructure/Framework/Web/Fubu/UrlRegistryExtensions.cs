using FubuCore;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.Urls;

namespace Volta.Core.Infrastructure.Framework.Web.Fubu
{
    public static class UrlRegistryExtensions
    {
        public static string QuotedUrlFor<TInputModel>(this IUrlRegistry urls)
        {
            return "'{0}'".ToFormat(urls.UrlFor(typeof(TInputModel), new RouteParameters()));
        }
    }
}