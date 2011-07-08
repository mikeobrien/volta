using Volta.Core.Infrastructure.Framework.Web.Navigation;

namespace Volta.Web
{
    public class Navigation : TabCollection
    {
        public Navigation()
        {
            Add(x =>
                {
                    x.Name = "Test Batches";
                    x.Add<Handlers.TestBatches.QueryHandler>("Batches", y => y.Query());
                });

            Add(x =>
                {
                    x.Name = "Administration";
                    x.Add<Handlers.Administration.Users.QueryHandler>("Users", y => y.Query());
                    x.Add<Handlers.Administration.Users.AddHandler>("Add User", y => y.Query());
                    x.Add<Handlers.Administration.Users.EditHandler>(y => y.Query_Username(null));
                });
        }
    }
}