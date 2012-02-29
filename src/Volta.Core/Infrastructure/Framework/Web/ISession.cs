using System.Collections.Generic;

namespace Volta.Core.Infrastructure.Framework.Web
{
    public interface ISession : IDictionary<string, object>
    {
        string Id { get; }
        bool Exists { get; }
    }
}