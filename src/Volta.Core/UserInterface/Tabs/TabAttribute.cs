using System;

namespace Volta.Core.UserInterface.Tabs
{
    public enum TabName
    {
        TestBatches,
        Administration
    }

    public class TabAttribute : Attribute
    {
        public TabAttribute(TabName tab)
        {
            Tab = tab;
        }

        public TabAttribute(TabName tab, string subTab, int subTabOrder)
        {
            Tab = tab;
            SubTab = subTab;
            SubTabOrder = subTabOrder;
        }

        public TabName Tab { get; private set; }
        public string SubTab { get; private set; }
        public int SubTabOrder { get; private set; }
        public bool HasSubTab { get { return !string.IsNullOrEmpty(SubTab); } }
    }
}