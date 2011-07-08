using System.Linq;
using FubuMVC.Core;
using NUnit.Framework;
using Should;
using Volta.Web.Handlers.Widgets;

namespace Volta.Tests.Unit.UserInterface.Widgets
{
    [TestFixture]
    public class LinksHandlerTests
    {
        [Test]
        public void Should_Not_Display_A_Single_Link()
        {
            var handler = new LinksHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.TestBatchesUrl });
            var output = handler.Query(null);

            output.Links.Count().ShouldEqual(0);
        }

        [Test]
        public void Should_Set_Link_Names_In_Proper_Order()
        {
            var handler = new LinksHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.UsersUrl });
            var output = handler.Query(null);

            output.Links.Count().ShouldEqual(2);
            output.Links.ElementAt(0).Name.ShouldEqual(NavigationCommon.UsersName);
            output.Links.ElementAt(1).Name.ShouldEqual(NavigationCommon.AddUserName);
        }

        [Test]
        public void Should_Set_Link_Urls_In_Proper_Order()
        {
            var handler = new LinksHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.UsersUrl });
            var output = handler.Query(null);

            output.Links.Count().ShouldEqual(2);
            output.Links.ElementAt(0).Url.ShouldEqual(NavigationCommon.UsersUrl);
            output.Links.ElementAt(1).Url.ShouldEqual(NavigationCommon.AddUserUrl);
        }

        [Test]
        public void Should_Not_Display_Invisible_Links()
        {
            var handler = new LinksHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.DeleteUserUrl });
            var output = handler.Query(null);

            output.Links.Count().ShouldEqual(2);
            output.Links.Any(x => x.Url == NavigationCommon.DeleteUserUrl).ShouldBeFalse();
        }

        [Test]
        public void Should_Only_Set_Selected_Link_As_Selected()
        {
            var handler = new LinksHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.AddUserUrl });
            var output = handler.Query(null);

            output.Links.Count().ShouldEqual(2);
            output.Links.ElementAt(0).Selected.ShouldBeFalse();
            output.Links.ElementAt(1).Selected.ShouldBeTrue();
        }

        [Test]
        public void Should_Not_Set_Any_Links_As_Selected_When_On_Invisible_Url()
        {
            var handler = new LinksHandler(NavigationCommon.TestUrlRegistry, NavigationCommon.TestNavigation, new CurrentRequest { Path = NavigationCommon.EditSomeUserUrl });
            var output = handler.Query(null);

            output.Links.Count().ShouldEqual(2);
            output.Links.ElementAt(0).Selected.ShouldBeFalse();
            output.Links.ElementAt(1).Selected.ShouldBeFalse();
        }
    }
}