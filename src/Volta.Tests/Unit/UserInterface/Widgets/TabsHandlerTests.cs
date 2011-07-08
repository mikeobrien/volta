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
    public class TabsHandlerTests
    {
        private readonly IEnumerable<Link> _links =
            new List<Link> { new Link { Name = "Test Batches", Order = 0, Visible = true, Url = "/testbatches" },
                             new Link { Name = "Test Groups", Order = 0, Visible = false, Url = "/testgroups" },
                             new Link { Name = "Administration", Order = 1, Visible = true, Children =
                                    {
                                        new Link { Name = "Users", Order = 0, Visible = true, Url = "/administration/users/query" },
                                        new Link { Name = "Add User", Order = 1, Visible = true, Url = "/administration/users/add" },
                                        new Link { Name = "Edit User", Order = 2, Visible = true, Url = "/administration/users/edit" }
                                    }},
                             new Link { Name = "Logging", Order = 2, Visible = true, Children =
                                    {
                                        new Link { Name = "Files", Order = 0, Visible = false, Url = "/logging/files" },
                                        new Link { Name = "Configuration", Order = 1, Visible = true, Url = "/logging/configuration" }
                                    }}};

        [Test]
        public void Should_Set_Tab_Names_In_Proper_Order()
        {
            var linkFactory = Substitute.For<ILinkFactory>();
            linkFactory.Build().Returns(_links);
            var handler = new TabsHandler(linkFactory, new CurrentRequest { Path = "/testbatches" });
            var output = handler.Query(null);

            output.Tabs.ElementAt(0).Name.ShouldEqual(_links.First(x => x.Order == 0).Name);
            output.Tabs.ElementAt(1).Name.ShouldEqual(_links.First(x => x.Order == 1).Name);
            output.Tabs.ElementAt(2).Name.ShouldEqual(_links.First(x => x.Order == 2).Name);
        }

        [Test]
        public void Should_Set_Tab_Urls_From_Tab_Or_First_Child_In_Proper_Order()
        {
            var linkFactory = Substitute.For<ILinkFactory>();
            linkFactory.Build().Returns(_links);
            var handler = new TabsHandler(linkFactory, new CurrentRequest { Path = "/testbatches" });
            var output = handler.Query(null);

            output.Tabs.ElementAt(0).Url.ShouldEqual(_links.First(x => x.Order == 0).Url);
            output.Tabs.ElementAt(1).Url.ShouldEqual(_links.First(x => x.Order == 1).Children.ElementAt(0).Url);
            output.Tabs.ElementAt(2).Url.ShouldEqual(_links.First(x => x.Order == 2).Children.ElementAt(0).Url);
        }

        [Test]
        public void Should_Not_Display_Non_Visible_Tabs()
        {
            var linkFactory = Substitute.For<ILinkFactory>();
            linkFactory.Build().Returns(_links);
            var handler = new TabsHandler(linkFactory, new CurrentRequest { Path = "/testbatches" });
            var output = handler.Query(null);

            output.Tabs.Any(x => x.Name == "Test Groups").ShouldBeFalse();
        }

        [Test]
        public void Should_Only_Set_Tab_As_Selected()
        {
            var linkFactory = Substitute.For<ILinkFactory>();
            linkFactory.Build().Returns(_links);
            var handler = new TabsHandler(linkFactory, new CurrentRequest { Path = "/testbatches" });
            var output = handler.Query(null);

            output.Tabs.ElementAt(0).Selected.ShouldBeTrue();
            output.Tabs.ElementAt(1).Selected.ShouldBeFalse();
            output.Tabs.ElementAt(2).Selected.ShouldBeFalse();
        }

        [Test]
        public void Should_Set_Link_Parent_As_Selected_When_Link_Visible()
        {
            var linkFactory = Substitute.For<ILinkFactory>();
            linkFactory.Build().Returns(_links);
            var handler = new TabsHandler(linkFactory, new CurrentRequest { Path = "/administration/users/add" });
            var output = handler.Query(null);

            output.Tabs.ElementAt(0).Selected.ShouldBeFalse();
            output.Tabs.ElementAt(1).Selected.ShouldBeTrue();
            output.Tabs.ElementAt(2).Selected.ShouldBeFalse();
        }

        [Test]
        public void Should_Set_Link_Parent_As_Selected_When_Link_With_Parameterized_Url_Visible()
        {
            var linkFactory = Substitute.For<ILinkFactory>();
            linkFactory.Build().Returns(_links);
            var handler = new TabsHandler(linkFactory, new CurrentRequest { Path = "/administration/users/edit/someuser" });
            var output = handler.Query(null);

            output.Tabs.ElementAt(0).Selected.ShouldBeFalse();
            output.Tabs.ElementAt(1).Selected.ShouldBeTrue();
            output.Tabs.ElementAt(2).Selected.ShouldBeFalse();
        }

        [Test]
        public void Should_Set_Link_Parent_As_Selected_When_Link_Not_Visible()
        {
            var linkFactory = Substitute.For<ILinkFactory>();
            linkFactory.Build().Returns(_links);
            var handler = new TabsHandler(linkFactory, new CurrentRequest { Path = "/logging/files" });
            var output = handler.Query(null);

            output.Tabs.ElementAt(0).Selected.ShouldBeFalse();
            output.Tabs.ElementAt(1).Selected.ShouldBeFalse();
            output.Tabs.ElementAt(2).Selected.ShouldBeTrue();
        }
    }
}