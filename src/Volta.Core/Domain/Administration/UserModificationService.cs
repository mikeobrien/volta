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

        public User Modify(Username username, User modifiedUser)
        {
            if (string.IsNullOrEmpty(modifiedUser.Username))
                throw new EmptyUsernameException();

            var user = _userRepository.FirstOrDefault(x => x.Username == (string)username);

            if (user == null) throw new UserNotFoundException();

            if (username != modifiedUser.Username && _userRepository.Any(x => x.Username == modifiedUser.Username))
                throw new DuplicateUsernameException();

            user.Username = modifiedUser.Username;
            user.Password = !string.IsNullOrEmpty(modifiedUser.Password) ?
                HashedPassword.Create(modifiedUser.Password).ToString() : user.Password;
            user.Administrator = modifiedUser.Administrator;
            _userRepository.Update(x => x.Id, user);

            if (_secureSession.IsLoggedIn() && _secureSession.GetCurrentToken().Username == username) 
                _secureSession.Login(new Token(user.Username, user.Administrator));

            return user;
        }
    }
}