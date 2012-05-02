using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Volta.Core.Infrastructure.Framework.Reflection;

namespace Volta.Core.Infrastructure.Framework.Web
{
    public static class MimeType
    {
        private static readonly Lazy<Dictionary<string, string>> MimeTypes = new Lazy<Dictionary<string, string>>(LoadMimeTypeMapping);

        public static string ResolveMimeTypeFromFilename(string filename)
        {
            var extension = Path.GetExtension(filename).Replace(".", "").ToLower().Trim();
            return MimeTypes.Value.ContainsKey(extension) ? MimeTypes.Value[extension] : "";
        }

        private static Dictionary<string, string> LoadMimeTypeMapping()
        {
            var mapping = new Dictionary<string, string>();
            // http://svn.apache.org/viewvc/httpd/httpd/trunk/docs/conf/mime.types?view=co
            foreach (var map in Assembly.GetExecutingAssembly().GetResourceString("mime.txt")
                .Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.Trim().Length > 0 && !x.Trim().StartsWith("#"))
                .Select(x => x.Split(new[] {'\t', ' '}, StringSplitOptions.RemoveEmptyEntries))
                .Select(x => new { MimeType = x.First(), Extensions = x.Skip(1).ToList()}))
            {
                map.Extensions.Where(x => !mapping.ContainsKey(x)).ToList().ForEach(x => mapping.Add(x, map.MimeType));
            }
            return mapping;
        }
    }
}


