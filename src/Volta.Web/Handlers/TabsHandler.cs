using System.Collections.Generic;
using FubuMVC.Core;
using Volta.Core.UserInterface.Tabs;

namespace Volta.Web.Handlers
{
    public class TabsInputModel { }

    public class TabsOutputModel
    {
        public IEnumerable<Tab> Tabs { get; set; }
        public IEnumerable<Tab> SubTabs { get; set; }

        public class Tab
        {
            public string Name { get; set; }
            public bool Selected { get; set; }
            public bool Disabled { get; set; }
            public string Url { get; set; }
        }
    }

    public class TabsHandler
    {
        private readonly ITabFactory _tabFactory;
        private readonly CurrentRequest _request;

        public TabsHandler(ITabFactory tabFactory, CurrentRequest request)
        {
            _tabFactory = tabFactory;
            _request = request;
        }

        [FubuPartial]
        public TabsOutputModel Query(TabsInputModel input)
        {
            return new TabsOutputModel
                       {
                           Tabs = new List<TabsOutputModel.Tab>
                                      {
                                          
                                      }
                       };
        }
    }
}