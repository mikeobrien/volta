using Volta.Core.UserInterface.Tabs;

namespace Volta.Web.Handlers.Administration.Users
{
    public class QueryOutputModel
    {
    }

    public class QueryHandler
    {
        [Tab(TabName.Administration, "Users", 0)]
        public QueryOutputModel Query()
        {
            return new QueryOutputModel();
        }
    }
}