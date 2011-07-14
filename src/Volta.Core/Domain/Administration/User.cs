using Norm;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain.Administration
{
    public class User
    {
        private Username _username;

        public ObjectId Id { get; set; }
        public string Username { get { return _username; } set { _username = value; } }
        public string Password { get; set; }
        public bool Administrator { get; set; }
    }
}