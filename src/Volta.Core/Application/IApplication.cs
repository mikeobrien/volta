using System;

namespace Volta.Core.Application
{
    public interface IApplication
    {
        bool IsInDebugMode { get; }
        string Version { get; }
        DateTime ReleaseDate { get; }
        string CopyrightYear { get; }
    }
}