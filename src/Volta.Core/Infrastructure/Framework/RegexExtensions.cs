using System.Linq;
using System.Text.RegularExpressions;

namespace Volta.Core.Infrastructure.Framework
{
    public static class RegexExtensions
    {
        public static string Match(this string value, string regex)
        {
            return Regex.Match(value, regex).Groups.Cast<Group>().Last().Value;
        }

        public static bool IsMatch(this string value, string regex)
        {
            return new Regex(regex).IsMatch(value);
        }
    }
}