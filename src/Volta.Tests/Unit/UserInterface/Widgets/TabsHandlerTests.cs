using System.Linq;
using FubuMVC.Core;
using NUnit.Framework;
using Should;
using Volta.Web.Handlers.Widgets;

namespace Volta.Tests.Unit.UserInterface.Widgets
{
    [TestFixture]
    public class TabsHandlerTests
    {
        [Test]
        public void Should_Set_Tab_Names_In_Proper_Order()
        {
            var handler = new TabsHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.TestBatchesUrl });
            var output = handler.Query(new TabsOutputModel());

            output.Tabs.Count().ShouldEqual(2);
            output.Tabs.ElementAt(0).Name.ShouldEqual(NavigationCommon.TestBatchesName);
            output.Tabs.ElementAt(1).Name.ShouldEqual(NavigationCommon.AdministrationName);
        }

        [Test]
        public void Should_Set_Tab_Urls_From_Tab_Or_First_Child_In_Proper_Order()
        {
            var handler = new TabsHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.AdministrationName });
            var output = handler.Query(new TabsOutputModel());

            output.Tabs.Count().ShouldEqual(2);
            output.Tabs.ElementAt(0).Url.ShouldEqual(NavigationCommon.TestBatchesUrl);
            output.Tabs.ElementAt(1).Url.ShouldEqual(NavigationCommon.UsersUrl);
        }

        [Test]
        public void Should_Not_Display_Non_Visible_Tabs()
        {
            var handler = new TabsHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.TestBatchesUrl });
            var output = handler.Query(new TabsOutputModel());

            output.Tabs.Count().ShouldEqual(2);
            output.Tabs.Any(x => x.Name == "Test Groups").ShouldBeFalse();
        }

        [Test]
        public void Should_Only_Set_Tab_As_Selected()
        {
            var handler = new TabsHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.TestBatchesUrl });
            var output = handler.Query(new TabsOutputModel());

            output.Tabs.Count().ShouldEqual(2);
            output.Tabs.ElementAt(0).Selected.ShouldBeTrue();
            output.Tabs.ElementAt(1).Selected.ShouldBeFalse();
        }

        [Test]
        public void Should_Set_Link_Parent_As_Selected_When_Link_Visible()
        {
            var handler = new TabsHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.AddUserUrl });
            var output = handler.Query(new TabsOutputModel());

            output.Tabs.Count().ShouldEqual(2);
            output.Tabs.ElementAt(0).Selected.ShouldBeFalse();
            output.Tabs.ElementAt(1).Selected.ShouldBeTrue();
        }

        [Test]
        public void Should_Set_Link_Parent_As_Selected_When_Link_With_Parameterized_Url_Visible()
        {
            var handler = new TabsHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.EditSomeUserUrl });
            var output = handler.Query(new TabsOutputModel());

            output.Tabs.Count().ShouldEqual(2);
            output.Tabs.ElementAt(0).Selected.ShouldBeFalse();
            output.Tabs.ElementAt(1).Selected.ShouldBeTrue();
        }

        [Test]
        public void Should_Set_Link_Parent_As_Selected_When_Link_Not_Visible()
        {
            var handler = new TabsHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.DeleteUserUrl });
            var output = handler.Query(new TabsOutputModel());

            output.Tabs.Count().ShouldEqual(2);
            output.Tabs.ElementAt(0).Selected.ShouldBeFalse();
            output.Tabs.ElementAt(1).Selected.ShouldBeTrue();
        }
    }
}