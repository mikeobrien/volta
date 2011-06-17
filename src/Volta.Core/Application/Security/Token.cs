namespace Volta.Core.Application.Security
{
    public class Token
    {
        public Token(string username, bool administrator)
        {
            Username = username;
            IsAdministrator = administrator;
        }

        public string Username { get; private set; }
        public bool IsAdministrator { get; private set; }
    }
}