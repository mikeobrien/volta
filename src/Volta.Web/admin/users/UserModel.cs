namespace Volta.Web.Admin.Users
{
    public class UserModel
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool admin { get; set; }
    }
}