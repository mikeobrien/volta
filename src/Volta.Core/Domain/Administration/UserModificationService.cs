using System;
using System.Linq;
using Volta.Core.Application.Security;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Domain.Administration
{
    public class UserNotFoundException : ValidationException
        { public UserNotFoundException() : base("User not found.") {}}

    public class UserModificationService : IUserModificationService
    {
        private readonly IRepository<User> _userRepository;
        private readonly ISecureSession<Token> _secureSession;

        public UserModificationService(IRepository<User> userRepository, ISecureSession<Token> secureSession)
        {
            _userRepository = userRepository;
            _secureSession = secureSession;
        }

        public User Modify(Guid id, Username username, string email, bool administrator, string password = null)
        {
            if (string.IsNullOrEmpty(username))
                throw new EmptyUsernameException();

            var user = _userRepository.Get(id);

            if (user == null) throw new UserNotFoundException();

            if (user.Username != username && _userRepository.Any(x => x.Username == username))
                throw new DuplicateUsernameException();

            user.Username = username;
            user.Email = email;
            if (!string.IsNullOrEmpty(password)) user.SetPassword(password);
            user.Administrator = administrator;
            _userRepository.Replace(user);

            if (_secureSession.IsLoggedIn() && _secureSession.GetCurrentToken().UserId == user.Id) 
                _secureSession.Login(new Token(user.Id, user.Username, user.Administrator));

            return user;
        }
    }
}