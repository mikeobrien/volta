using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Volta.Core.Infrastructure.Framework.Web
{
    public class Session : ISession
    {
        private readonly Lazy<HttpSessionState> _session = new Lazy<HttpSessionState>(
            () => HttpContext.Current != null ? HttpContext.Current.Session : null);

        public bool Exists { get { return _session.Value != null; }}
        public string Id { get { return _session.Value.SessionID; } }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return KeyAndValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _session.Value.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            Array.Copy(KeyAndValues.ToArray(), array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return Remove(item.Key);
        }

        public int Count
        {
            get { return _session.Value.Count; }
        }

        public bool IsReadOnly
        {
            get { return _session.Value.IsReadOnly; }
        }

        public bool ContainsKey(string key)
        {
            return Keys.Contains(key);
        }

        public void Add(string key, object value)
        {
            _session.Value.Add(key, value);
        }

        public bool Remove(string key)
        {
            var exists = ContainsKey(key);
            if (exists) _session.Value.Remove(key);
            return exists;
        }

        public bool TryGetValue(string key, out object value)
        {
            var exists = ContainsKey(key);
            value = exists ? this[key] : null;
            return exists;
        }

        public object this[string key]
        {
            get { return _session.Value[key]; }
            set { _session.Value[key] = value; }
        }

        public ICollection<string> Keys
        {
            get { return _session.Value.Keys.Cast<string>().ToList(); }
        }

        public ICollection<object> Values
        {
            get { return Keys.Select(x => this[x]).ToList(); }
        }

        public IEnumerable<KeyValuePair<string, object>> KeyAndValues
        {
            get { return Keys.ToDictionary(x => x, x => this[x]); }
        } 
    }
}