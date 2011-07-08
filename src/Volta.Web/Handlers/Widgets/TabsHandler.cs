using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using Volta.Core.UserInterface.Navigation;

namespace Volta.Web.Handlers.Widgets
{
    public class TabsInputModel { }

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
        private readonly ILinkFactory _linkFactory;
        private readonly CurrentRequest _request;

        public TabsHandler(ILinkFactory linkFactory, CurrentRequest request)
        {
            _linkFactory = linkFactory;
            _request = request;
        }

        [FubuPartial]
        public TabsOutputModel Query(TabsInputModel input)
        {
            var links = _linkFactory.Build();
            Func<Link, bool> isSelected = link => link.HasUrl && (_request.Path.StartsWith(link.Url));
            return new TabsOutputModel
                       {
                           Tabs = links.Where(x => x.Visible).
                                        OrderBy(x => x.Order).
                                        Select(x => new TabsOutputModel.Tab
                                                  {
                                                      Name = x.Name,
                                                      Selected = isSelected(x) || (x.HasChildren && x.Children.Any(isSelected)),
                                                      Disabled = false,
                                                      Url = x.HasUrl ? x.Url : x.Children.First().Url
                                                  })
                       };
        }
    }
}