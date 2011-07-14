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
        private readonly IUserCreationService _userCreationService;

        public AuthenticationService(IRepository<User> userRepository, IUserCreationService userCreationService)
        {
            _userRepository = userRepository;
            _userCreationService = userCreationService;
        }

        public Token Authenticate(Username username, string password)
        {
            var user = _userRepository.FirstOrDefault(x => x.Username == (string)username);
            if (user != null && HashedPassword.FromHash(user.Password).MatchesPassword(password)) return CreateToken(user);
            if (!_userRepository.Any()) return CreateToken(_userCreationService.Create(username, password, true));
            throw new AccessDeniedException();
        }

        private static Token CreateToken(User user)
        {
            return new Token(user.Username, user.Administrator);
        }
    }
}