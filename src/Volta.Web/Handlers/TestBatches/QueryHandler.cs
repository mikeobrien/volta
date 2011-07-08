using Volta.Core.UserInterface.Navigation;

namespace Volta.Web.Handlers.TestBatches
{
    public class QueryOutputModel
    {
    }

    public class QueryHandler
    {
        [Navigation(Module.TestBatches)]
        public QueryOutputModel Query()
        {
            return new QueryOutputModel();
        }
    }
}