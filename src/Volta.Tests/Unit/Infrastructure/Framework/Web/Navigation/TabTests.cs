using System.Linq;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Web.Navigation;

namespace Volta.Tests.Unit.Infrastructure.Framework.Web.Navigation
{
    [TestFixture]
    public class TabTests
    {
        public class AdministrationHandler { public void Query() { } }
        public class ConfigurationHandler { public void Query() { } }

        private const string AdministrationTab = "Administration";
        private const string ConfigurationTab = "Configuration";
        private const string UsersTab = "Users";

        [Test]
        public void Should_Add_Top_Level_Tabs_With_Correct_Order()
        {
            var tabs = new Tab {x => x.Name = AdministrationTab, 
                                x => x.Name = ConfigurationTab};

            tabs.Count().ShouldEqual(2);

            var tab = tabs.First();
            tab.HasName.ShouldBeTrue();
            tab.Name.ShouldEqual(AdministrationTab);
            tab.Order.ShouldEqual(0);
            tab.HasAction.ShouldBeFalse();
            tab.HasChildren.ShouldBeFalse();
            tab.Count().ShouldEqual(0);

            tab = tabs.Skip(1).First();
            tab.HasName.ShouldBeTrue();
            tab.Name.ShouldEqual(ConfigurationTab);
            tab.Order.ShouldEqual(1);
            tab.HasAction.ShouldBeFalse();
            tab.HasChildren.ShouldBeFalse();
            tab.Count().ShouldEqual(0);
        }

        [Test]
        public void Should_Add_Top_Level_And_Sub_Level_Tabs_With_Correct_Order()
        {
            var tabs = new Tab();
            tabs.Add(x => {
                             x.Name = AdministrationTab;
                             x.Add<AdministrationHandler>(UsersTab, y => y.Query());
                             x.Add<ConfigurationHandler>(y => y.Query());
                          });

            tabs.Count().ShouldEqual(1);
            var parent = tabs.First();
            parent.HasName.ShouldBeTrue();
            parent.Name.ShouldEqual(AdministrationTab);
            parent.Order.ShouldEqual(0);
            parent.HasAction.ShouldBeFalse();
            parent.HasChildren.ShouldBeTrue();
            parent.Count().ShouldEqual(2);

            var child = parent.First();
            child.HasName.ShouldBeTrue();
            child.Name.ShouldEqual(UsersTab);
            child.Order.ShouldEqual(0);
            child.HasChildren.ShouldBeFalse();
            child.Action.ShouldEqual(typeof(AdministrationHandler).GetMethod("Query"));

            child = parent.Skip(1).First();
            child.HasName.ShouldBeFalse();
            child.Name.ShouldBeNull();
            child.Order.ShouldEqual(1);
            child.HasChildren.ShouldBeFalse();
            child.Action.ShouldEqual(typeof(ConfigurationHandler).GetMethod("Query"));
        }
    }
}