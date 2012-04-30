using System;
using System.Linq;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain.Administration
{
    public class DuplicateUsernameException : ValidationException
        { public DuplicateUsernameException() : base("The username already exists.") {}}
    public class EmptyUsernameException : ValidationException
        { public EmptyUsernameException() : base("Username cannot be empty.") {}}
    public class EmptyPasswordException : ValidationException
        { public EmptyPasswordException() : base("Password cannot be empty.") {}}

    public class UserCreateService : IUserFactory
    {
        private readonly IRepository<User> _userRepository;

        public UserCreateService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public User Create(Username username, string password, string email, bool administrator, string createdBy)
        {
            if (_userRepository.Any(x => x.Username == (string)username))
                throw new DuplicateUsernameException();
            if (username.IsEmpty) throw new EmptyUsernameException();
            if (string.IsNullOrEmpty(password)) throw new EmptyPasswordException();
                
            var user = new User
                {
                    Username = username,
                    PasswordHash = HashedPassword.Create(password).ToString(),
                    Email = email,
                    Administrator = administrator,
                    Created = DateTime.Now,
                    CreatedBy = createdBy
                };

            _userRepository.Add(user);
            return user;
        }
    }
}