using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Urls;
using Volta.Core.Infrastructure.Framework.Web.FubuMvc;
using Volta.Core.Infrastructure.Framework.Web.Navigation;

namespace Volta.Web.Handlers.Widgets
{
    public class TabsOutputModel
    {
        public IEnumerable<Tab> Tabs { get; set; }

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
        private readonly IUrlRegistry _urlRegistry;
        private readonly TabCollection _tabCollection;
        private readonly CurrentRequest _request;

        public TabsHandler(IUrlRegistry urlRegistry, TabCollection tabCollection, CurrentRequest request)
        {
            _urlRegistry = urlRegistry;
            _tabCollection = tabCollection;
            _request = request;
        }

        [FubuPartial]
        public TabsOutputModel Query(TabsOutputModel output)
        {
            // TODO: Add come caching of the tabs
            Func<MethodInfo, string> getUrl = m => _urlRegistry.UrlFor(m.DeclaringType, m);
            Func<Tab, bool> isSelected = link => link.HasAction && getUrl(link.Action).MatchesUrl(_request.Path);
            output.Tabs = _tabCollection.
                            OrderBy(x => x.Order).
                            Select(x => new TabsOutputModel.Tab
                                            {
                                                Name = x.Name,
                                                Selected = isSelected(x) || (x.HasChildren && x.Any(isSelected)),
                                                Disabled = false,
                                                Url = getUrl(x.HasAction ? x.Action : x.First().Action)
                                            });
            return output;
        }
    }
}