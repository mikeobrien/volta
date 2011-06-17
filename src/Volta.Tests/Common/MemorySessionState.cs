using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Runtime;

namespace Volta.Tests.Common
{
    public class MemorySessionState : ISessionState
    {
        private readonly IList<object> _session = new List<object>();
        public T Get<T>() where T : class
        {
            return _session.OfType<T>().FirstOrDefault();
        }

        public void Set<T>(T value)
        {
            if (_session.OfType<T>().Any()) _session.Remove(_session.OfType<T>().First());
            _session.Add(value);
        }
    }
}