using System;
using System.Web;
using FubuMVC.Core.Behaviors;

namespace Volta.Core.Infrastructure.Framework.Web.Fubu
{
    public class CacheBusterBehavior : IActionBehavior
    {
        private readonly IActionBehavior _innerBehavior;

        public CacheBusterBehavior(IActionBehavior innerBehavior)
        {
            _innerBehavior = innerBehavior;
        }

        public void Invoke()
        {
            _innerBehavior.Invoke();
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
        }

        public void InvokePartial()
        {
            _innerBehavior.InvokePartial();
        }
    }
}