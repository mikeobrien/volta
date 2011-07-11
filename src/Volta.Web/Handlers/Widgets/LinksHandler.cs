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
    public class LinksOutputModel
    {
        public IEnumerable<Link> Links { get; set; }

        public class Link
        {
            public string Name { get; set; }
            public int Index { get; set; }
            public bool Selected { get; set; }
            public bool Disabled { get; set; }
            public string Url { get; set; }
        }
    }

    public class LinksHandler
    {
        private readonly IUrlRegistry _urlRegistry;
        private readonly TabCollection _tabCollection;
        private readonly CurrentRequest _request;

        public LinksHandler(IUrlRegistry urlRegistry, TabCollection tabCollection, CurrentRequest request)
        {
            _urlRegistry = urlRegistry;
            _tabCollection = tabCollection;
            _request = request;
        }

        [FubuPartial]
        public LinksOutputModel Query(LinksOutputModel output)
        {
            // TODO: Add come caching of the links
            Func<MethodInfo, string> getUrl = m => _urlRegistry.UrlFor(m.DeclaringType, m);
            Func<Tab, bool> isSelected = link => link.HasAction && getUrl(link.Action).MatchesUrl(_request.Path);
            var links = _tabCollection.Where(x => x.HasChildren && x.Any(isSelected)).
                                       SelectMany(x => x).
                                       Where(x => x.HasName).
                                       OrderBy(x => x.Order).
                                       Select((x, i) => new LinksOutputModel.Link
                                                {
                                                    Name = x.Name,
                                                    Index = i,
                                                    Selected = isSelected(x),
                                                    Disabled = false,
                                                    Url = getUrl(x.Action)
                                                });
            output.Links = links.Count() > 1 ? links : Enumerable.Empty<LinksOutputModel.Link>();
            return output;
        }
    }
}