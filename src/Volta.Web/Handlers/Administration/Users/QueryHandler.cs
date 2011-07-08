using Volta.Core.UserInterface.Navigation;

namespace Volta.Web.Handlers.Administration.Users
{
    public class QueryOutputModel
    {
    }

    public class QueryHandler
    {
        [Navigation(Module.Administration, "Users", 0)]
        public QueryOutputModel Query()
        {
            return new QueryOutputModel();
        }
    }
}