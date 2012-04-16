using System;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Application.Security
{
    public class Token
    {
        public Token(Guid userId, Username username, bool administrator)
        {
            UserId = userId;
            Username = username;
            IsAdministrator = administrator;
        }

        public Username Username { get; private set; }
        public Guid UserId { get; set; }
        public bool IsAdministrator { get; private set; }
    }
}