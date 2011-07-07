using FubuMVC.Core.Continuations;
using Volta.Core.UserInterface.Tabs;

namespace Volta.Web.Handlers.Administration.Users
{
    public class AddInputModel
    {
        public string Username { get; set; }
    }

    public class AddOutputModel { }

    public class AddHandler
    {
        [Tab(TabName.Administration, "Add User", 1)]
        public AddOutputModel Query()
        {
            return new AddOutputModel();
        }

        public FubuContinuation Command(AddInputModel input)
        {
            return FubuContinuation.RedirectTo<QueryHandler>(x => x.Query());
        }
    }
}