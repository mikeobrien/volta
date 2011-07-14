using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Application.Security
{
    public class Token
    {
        public Token(Username username, bool administrator)
        {
            Username = username;
            IsAdministrator = administrator;
        }

        public Username Username { get; private set; }
        public bool IsAdministrator { get; private set; }
    }
}