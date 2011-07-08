using FubuMVC.Core.Continuations;

namespace Volta.Web.Handlers.Administration.Users
{
    public class AddInputModel
    {
        public string Username { get; set; }
    }

    public class AddOutputModel { }

    public class AddHandler
    {
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