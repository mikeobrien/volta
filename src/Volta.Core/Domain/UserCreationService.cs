using System;
using System.Linq;
using Volta.Core.Infrastructure.Data;

namespace Volta.Core.Domain
{
    public class DuplicateUsernameException : Exception { }

    public class UserCreationService
    {
        private readonly IRepository<User> _userRepository;
        private readonly UserFactory _userFactory;

        public UserCreationService(IRepository<User> userRepository, UserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }

        public User CreateUser(string username, string password, bool isAdmin)
        {
            if (_userRepository.Any(x => x.Username.ToLower() == username.ToLower()))
                throw new DuplicateUsernameException();
            var user = _userFactory.Create(username, password, isAdmin);
            _userRepository.Add(user);
            return user;
        }
    }
}