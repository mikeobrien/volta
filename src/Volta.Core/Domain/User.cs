namespace Volta.Core.Domain
{
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public string ApiKey { get; set; }
    }
}