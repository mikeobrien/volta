using Norm;

namespace Volta.Core.Domain.Administration
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Administrator { get; set; }
    }
}