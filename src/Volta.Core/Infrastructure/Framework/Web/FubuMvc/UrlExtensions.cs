using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using FubuCore;
using FubuCore.Reflection;

namespace Volta.Core.Infrastructure.Framework.Web.FubuMvc
{
    public static class UrlExtensions
    {
        public static string AppendQueryStringValueFor<T>(this string url, Expression<Func<T, object>> property, string value)
        {
            url = url.Trim();
            var seperator = !url.Contains("?") ? "?" : (url.EndsWith("?") ? string.Empty : (url.EndsWith("&") ? string.Empty : "&"));
            return string.Format("{0}{1}{2}={3}", url, seperator, property.GetName(), value.UrlEncode());
        }

        public static bool MatchesUrl(this string url, string compareUrl)
        {
            return Regex.IsMatch(compareUrl, Regex.Replace(url, "{\\w*}", "\\w*"), RegexOptions.IgnoreCase);
        }
    }
}