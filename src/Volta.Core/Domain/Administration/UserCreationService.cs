using System;
using System.Linq;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain.Administration
{
    public class DuplicateUsernameException : Exception { }
    public class EmptyUsernameOrPasswordException : Exception { }

    public class UserCreationService : IUserCreationService
    {
        private readonly IRepository<User> _userRepository;

        public UserCreationService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public User Create(string username, string password, bool isAdmin)
        {
            if (_userRepository.Any(x => x.Username.ToLower() == username.ToLower()))
                throw new DuplicateUsernameException();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new EmptyUsernameOrPasswordException();
            var user = new User
                {
                    Username = username.ToLower(),
                    Password = HashedPassword.Create(password).ToString(),
                    Administrator = isAdmin
                };
            _userRepository.Add(user);
            return user;
        }
    }
}