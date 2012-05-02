using System.IO;
using System.Linq;

namespace Volta.Core.Infrastructure.Framework.IO
{
    public static class StringExtensions
    {
         public static string RemoveInvalidFileChars(this string filename)
         {
             return Path.GetInvalidFileNameChars().Aggregate(filename, (f, c) => f.Replace(c.ToString(), ""));
         }
    }
}