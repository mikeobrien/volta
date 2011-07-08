using System;
using System.Collections;
using System.Collections.Generic;

namespace Volta.Core.Infrastructure.Framework.Web.Navigation
{
    public class TabCollection : IEnumerable<Tab>
    {
        private readonly Tab _tab = new Tab(); 

        public Tab Add(Action<Tab> config)
        {
            return _tab.Add(config);
        }

        public IEnumerator<Tab> GetEnumerator()
        {
            return _tab.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}