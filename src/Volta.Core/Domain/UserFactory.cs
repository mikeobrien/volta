using System;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain
{
    public class EmptyUsernameOrPasswordException : Exception { }

    public class UserFactory : IUserFactory
    {
        public User Create(string username, string password, bool isAdmin)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new EmptyUsernameOrPasswordException();
            return new User
            {
                Username = username.ToLower(),
                PasswordHash = HashedPassword.Create(password).ToString(),
                IsAdmin = isAdmin,
                ApiKey = Guid.NewGuid().ToString("N").ToLower()
            };
        }
    }
}