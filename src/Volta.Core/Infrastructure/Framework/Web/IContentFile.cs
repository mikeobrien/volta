using System;

namespace Volta.Core.Infrastructure.Framework.Web
{
    public interface IContentFile
    {
        string ReadAllText(string relativePath);
    }
}