using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using NSubstitute;
using NUnit.Framework;
using Should;
using Volta.Core.UserInterface.Navigation;
using Volta.Web.Handlers.Widgets;

namespace Volta.Tests.Unit.UserInterface.Widgets
{
    [TestFixture]
    public class LinksHandlerTests
    {
        private readonly IEnumerable<Link> _links = 
            new List<Link> { new Link { Name = "Test Batches", Order = 0, Visible = true, Url = "/testbatches" },
                             new Link { Name = "Administration", Order = 1, Visible = true, Children =
                                    {
                                        new Link { Name = "Users", Order = 0, Visible = true, Url = "/administration/users/query" },
                                        new Link { Name = "Add User", Order = 1, Visible = true, Url = "/administration/users/add" },
                                        new Link { Name = "Delete User", Visible = false, Url = "/administration/users/delete" },
                                        new Link { Visible = false, Url = "/administration/users/edit" }
                                    }},
                             new Link { Name = "Logging", Order = 2, Visible = true, Children =
                                    {
                                        new Link { Name = "Files", Order = 0, Visible = true, Url = "/logging/files" },
                                        new Link { Name = "Configuration", Order = 1, Visible = true, Url = "/logging/configuration" }
                                    }}};

        [Test]
        public void Should_Set_Link_Names_In_Proper_Order()
        {
            var linkFactory = Substitute.For<ILinkFactory>();
            linkFactory.Build().Returns(_links);
            var handler = new LinksHandler(linkFactory, new CurrentRequest { Path = "/administration/users/add" });
            var output = handler.Query(null);

            output.Links.ElementAt(0).Name.ShouldEqual(_links.ElementAt(1).Children.First(x => x.Order == 0).Name);
            output.Links.ElementAt(1).Name.ShouldEqual(_links.ElementAt(1).Children.First(x => x.Order == 1).Name);
        }

        [Test]
        public void Should_Set_Link_Urls_In_Proper_Order()
        {
            var linkFactory = Substitute.For<ILinkFactory>();
            linkFactory.Build().Returns(_links);
            var handler = new LinksHandler(linkFactory, new CurrentRequest { Path = "/administration/users/add" });
            var output = handler.Query(null);

            output.Links.ElementAt(0).Url.ShouldEqual(_links.ElementAt(1).Children.First(x => x.Order == 0).Url);
            output.Links.ElementAt(1).Url.ShouldEqual(_links.ElementAt(1).Children.First(x => x.Order == 1).Url);
        }

        [Test]
        public void Should_Not_Display_Invisible_Links()
        {
            var linkFactory = Substitute.For<ILinkFactory>();
            linkFactory.Build().Returns(_links);
            var handler = new LinksHandler(linkFactory, new CurrentRequest { Path = "/administration/users/add" });
            var output = handler.Query(null);

            output.Links.Any(x => x.Name == "Delete User").ShouldBeFalse();
        }

        [Test]
        public void Should_Only_Set_Selected_Link_As_Selected()
        {
            var linkFactory = Substitute.For<ILinkFactory>();
            linkFactory.Build().Returns(_links);
            var handler = new LinksHandler(linkFactory, new CurrentRequest { Path = "/administration/users/add" });
            var output = handler.Query(null);

            output.Links.ElementAt(0).Selected.ShouldBeFalse();
            output.Links.ElementAt(1).Selected.ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Set_Any_Links_As_Selected_When_On_Invisible_Url()
        {
            var linkFactory = Substitute.For<ILinkFactory>();
            linkFactory.Build().Returns(_links);
            var handler = new LinksHandler(linkFactory, new CurrentRequest { Path = "/administration/users/edit/someuser" });
            var output = handler.Query(null);

            output.Links.ElementAt(0).Selected.ShouldBeFalse();
            output.Links.ElementAt(1).Selected.ShouldBeFalse();
        }
    }
}