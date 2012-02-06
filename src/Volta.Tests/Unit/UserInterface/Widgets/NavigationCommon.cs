using System;
using System.Reflection;
using FubuMVC.Core.Urls;
using Volta.Core.Infrastructure.Framework.Web.FubuMvc;
using Volta.Core.Infrastructure.Framework.Web.Navigation;

namespace Volta.Tests.Unit.UserInterface.Widgets
{
    public class NavigationCommon
    {
        public class TestBatchHandler { public void Query() { } }
        public class UsersHandler { public void Query() { } }
        public class AddUserHandler { public void Query() { } }
        public class EditUserHandler { public void Query() { } }
        public class DeleteUserHandler { public void Query() { } }

        public class Navigation : TabCollection
        {
            public Navigation()
            {
                Add(x =>
                        {
                            x.Name = TestBatchesName;
                            x.Add<TestBatchHandler>(BatchesName, y => y.Query());
                        });

                Add(x =>
                        {
                            x.Name = AdministrationName;
                            x.Add<UsersHandler>(UsersName, y => y.Query());
                            x.Add<AddUserHandler>(AddUserName, y => y.Query());
                            x.Add<EditUserHandler>(y => y.Query());
                            x.Add<DeleteUserHandler>(y => y.Query());
                        });
            }
        }

        public class UrlRegistry : UrlRegistryBase
        {
            public override string UrlFor(Type handlerType, MethodInfo method, string categoryOrHttpMethodOrHttpMethod = null)
            {
                Func<Type, bool> isMatch = t => (handlerType == t && method == t.GetMethod("Query"));
                string url = null;
                if (isMatch(typeof(TestBatchHandler))) url = TestBatchesUrl;
                if (isMatch(typeof(UsersHandler))) url = UsersUrl;
                if (isMatch(typeof(AddUserHandler))) url = AddUserUrl;
                if (isMatch(typeof(EditUserHandler))) url = EditUserUrl;
                if (isMatch(typeof(DeleteUserHandler))) url = DeleteUserUrl;
                return url;
            }
        }

        public static readonly IUrlRegistry TestUrlRegistry = new UrlRegistry();
        public static readonly TabCollection TestNavigation = new Navigation();

        public const string TestBatchesName = "Test Batches";
        public const string BatchesName = "Batches";
        public const string AdministrationName = "Administration";
        public const string UsersName = "Users";
        public const string AddUserName = "Add User";

        public const string TestBatchesUrl = "/testbatches";
        public const string UsersUrl = "/administration/users/query";
        public const string AddUserUrl = "/administration/users/add";
        public const string EditUserUrl = "/administration/users/edit/{Username}";
        public const string EditSomeUserUrl = "/administration/users/edit/someuser";
        public const string DeleteUserUrl = "/administration/users/delete";
    }
}