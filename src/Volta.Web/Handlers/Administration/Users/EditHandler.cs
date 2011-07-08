using Volta.Core.UserInterface.Navigation;

namespace Volta.Web.Handlers.Administration.Users
{
    public class EditInputModel
    {
        public string Username { get; set; }
    }

    public class EditOutputModel
    {
        public string Username { get; set; }
    }

    public class EditHandler
    {
        [Navigation(Module.Administration)]
        public EditOutputModel Query_Username(EditInputModel input)
        {
            return new EditOutputModel { Username = input.Username };
        }
    }
}