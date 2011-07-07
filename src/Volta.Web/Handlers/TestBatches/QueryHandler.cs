using Volta.Core.UserInterface.Tabs;

namespace Volta.Web.Handlers.TestBatches
{
    public class QueryOutputModel
    {
    }

    public class QueryHandler
    {
        [Tab(TabName.TestBatches)]
        public QueryOutputModel Query()
        {
            return new QueryOutputModel();
        }
    }
}