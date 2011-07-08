using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Volta.Core.Infrastructure.Framework.Web.Navigation
{
    public class Tab : IEnumerable<Tab>
    {
        private readonly IList<Tab> _tabs = new List<Tab>();

        public string Name { get; set; }
        public bool HasName { get { return !string.IsNullOrEmpty(Name); } }
        public int Order { get; set; }
        public MethodInfo Action { get; set; }
        public bool HasAction { get { return Action != null; } }
        public bool HasChildren { get { return this.Any(); } }

        public Tab Add(Action<Tab> config)
        {
            var tab = new Tab();
            tab.Order = NextIndex();
            config(tab);
            _tabs.Add(tab);
            return tab;
        }

        public Tab Add<THandler>(Expression<Action<THandler>> action)
        {
            return Add(null, action);
        }

        public Tab Add<THandler>(string name, Expression<Action<THandler>> action)
        {
            var tab = new Tab();
            tab.Name = name;
            tab.Order = NextIndex();
            tab.Action = ((MethodCallExpression)action.Body).Method;
            _tabs.Add(tab);
            return tab;
        }

        private int NextIndex()
        {
            return _tabs.Any() ? _tabs.Max(x => x.Order) + 1 : 0;
        }

        public IEnumerator<Tab> GetEnumerator()
        {
            return _tabs.OrderBy(x => x.Order).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}