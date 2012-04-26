using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public class IniFile : IEnumerable<IniFile.Section>
    {
        private readonly IEnumerable<Section> _sections;
 
        public IniFile(string data)
        {
            _sections = Parse(data);
        }

        public bool HasSection(string name) { return _sections.Any(x => x.Name == name); }
        public Section this[string name] { get { return _sections.FirstOrDefault(x => x.Name == name); } }

        private static IEnumerable<Section> Parse(string data)
        {
            var sections = new List<Section>();
            var lines = data.Split(new [] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
            Section section = null;
            foreach (var line in lines)
            {
                var row = line.Trim();
                if (row.StartsWith("[") && row.EndsWith("]"))
                    sections.Add(section = new Section { Name = row.Substring(1, row.Length - 2) });
                else if (row.Contains("="))
                {
                    var keyValue = row.Split(new [] {'='}, 2);
                    section.Values.Add(keyValue[0], keyValue[1]);
                }
            }
            return sections;
        }

        public IEnumerator<Section> GetEnumerator()
        {
            return _sections.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Section
        {
            public Section()
            {
                Values = new Dictionary<string, string>();
            }

            public string Name { get; set; }
            public Dictionary<string, string> Values { get; private set; }
        }
    }
}