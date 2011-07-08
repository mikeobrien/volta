using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using Volta.Core.UserInterface.Navigation;

namespace Volta.Web.Handlers.Widgets
{
    public class LinksInputModel { }

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
            public bool HasUrl { get; set; }
        }
    }

    public class LinksHandler
    {
        private readonly ILinkFactory _linkFactory;
        private readonly CurrentRequest _request;

        public LinksHandler(ILinkFactory linkFactory, CurrentRequest request)
        {
            _linkFactory = linkFactory;
            _request = request;
        }

        [FubuPartial]
        public LinksOutputModel Query(LinksInputModel input)
        {
            var links = _linkFactory.Build();
            Func<Link, bool> isSelected = link => link.HasUrl && (_request.Path.StartsWith(link.Url));
            return new LinksOutputModel
                       {
                           Links = links.Where(x => x.HasChildren && x.Children.Any(isSelected)).
                                         SelectMany(x => x.Children).
                                         Where(x => x.Visible).
                                         OrderBy(x => x.Order).
                                         Select((x, i) => new LinksOutputModel.Link
                                                  {
                                                      Name = x.Name,
                                                      Index = i,
                                                      Selected = isSelected(x),
                                                      Disabled = false,
                                                      Url = x.Url,
                                                      HasUrl = x.HasUrl
                                                  })
                       };
        }
    }
}