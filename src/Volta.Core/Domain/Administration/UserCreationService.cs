using System;
using System.Linq;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain.Administration
{
    public class DuplicateUsernameException : Exception { }
    public class EmptyUsernameException : Exception { }
    public class EmptyPasswordException : Exception { }

    public class UserCreationService : IUserCreationService
    {
        private readonly IRepository<User> _userRepository;

        public UserCreationService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public User Create(Username username, string password, string email, bool isAdministrator)
        {
            if (_userRepository.Any(x => x.Username == (string)username))
                throw new DuplicateUsernameException();
            if (username.IsEmpty) throw new EmptyUsernameException();
            if (string.IsNullOrEmpty(password)) throw new EmptyPasswordException();
                
            var user = new User
                {
                    Username = username,
                    Password = HashedPassword.Create(password).ToString(),
                    Email = email,
                    Administrator = isAdministrator
                };
            _userRepository.Add(user);
            return user;
        }
    }
}