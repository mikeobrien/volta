using System;

namespace Volta.Web.Admin.Users
{
    public class UserModel
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool administrator { get; set; }
    }
}