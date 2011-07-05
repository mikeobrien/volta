using System;
using System.IO;
using System.Reflection;
using Volta.Core.Infrastructure.Framework.Reflection;

namespace Volta.Core.Application
{
    public class Application : IApplication
    {
        public Application()
        {
            IsInDebugMode = Assembly.GetExecutingAssembly().IsInDebugMode();
            var assembly = Assembly.GetExecutingAssembly();
            Version = assembly.GetName().Version.ToString();
            ReleaseDate = File.GetCreationTime(assembly.Location);
            CopyrightYear = DateTime.Now.Year.ToString();
        }

        public bool IsInDebugMode { get; private set; }
        public string Version { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public string CopyrightYear { get; private set; }
    }
}
