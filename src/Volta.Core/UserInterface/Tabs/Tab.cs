using System.Collections.Generic;
using System.Linq;

namespace Volta.Core.UserInterface.Tabs
{
    public class Tab
    {
        public Tab()
        {
            Tabs = new List<Tab>();
        }

        public string Name { get; set; }
        public int Order { get; set; }
        public string Url { get; set; }
        public bool HasUrl { get { return !string.IsNullOrEmpty(Url); } }
        public IList<Tab> Tabs { get; private set; }
        public bool HasTabs { get { return Tabs.Any(); } }
    }
}