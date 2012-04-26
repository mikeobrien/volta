using System;
using System.Linq;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Application.Security
{
    public class AuthenticationService : IAuthenticationService<Token>
    {
        public class NotInitializedException : Exception { }
        public class AccessDeniedException : Exception { }

        private readonly IRepository<User> _userRepository;
        private readonly IUserFactory _userFactory;

        public AuthenticationService(IRepository<User> userRepository, IUserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }

        public Token Authenticate(Username username, string password)
        {
            var user = _userRepository.FirstOrDefault(x => x.Username == (string)username);
            if (user != null && HashedPassword.FromHash(user.PasswordHash).MatchesPassword(password)) return CreateToken(user);
            if (!_userRepository.Any()) return CreateToken(_userRepository.Add(_userFactory.Create(username, password, null, true)));
            return null;
        }

        private static Token CreateToken(User user)
        {
            return new Token(user.Id, user.Username, user.Administrator);
        }
    }
}