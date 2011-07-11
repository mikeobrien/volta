using System;
using System.Linq;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain.Administration
{
    public class UserNotFoundException : Exception { }

    public class UserModificationService : IUserModificationService
    {
        private readonly IRepository<User> _userRepository;
        private readonly ISecureSession<Token> _secureSession;

        public UserModificationService(IRepository<User> userRepository, ISecureSession<Token> secureSession)
        {
            _userRepository = userRepository;
            _secureSession = secureSession;
        }

        public User Modify(string username, User modifedUser)
        {
            var user = _userRepository.FirstOrDefault(x => x.Username == username);

            if (user == null) throw new UserNotFoundException();

            if (_userRepository.Any(x => x.Username == modifedUser.Username))
                throw new DuplicateUsernameException();

            user.Username = modifedUser.Username;
            user.Password = !string.IsNullOrEmpty(modifedUser.Password) ?
                HashedPassword.Create(modifedUser.Password).ToString() : user.Password;
            user.Administrator = modifedUser.Administrator;
            _userRepository.Update(x => x.Id, user);
            return user;
        }
    }
}