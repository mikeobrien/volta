using System;
using System.Linq.Expressions;
using FubuCore;
using FubuCore.Reflection;

namespace Volta.Core.Infrastructure.Framework.Web
{
    public static class FubuUrlExtensions
    {
        public static string AppendQueryStringValueFor<T>(this string url, Expression<Func<T, object>> property, string value)
        {
            url = url.Trim();
            var seperator = !url.Contains("?") ? "?" : (url.EndsWith("?") ? string.Empty : (url.EndsWith("&") ? string.Empty : "&"));
            return string.Format("{0}{1}{2}={3}", url, seperator, property.GetName(), value.UrlEncode());
        }
    }
}