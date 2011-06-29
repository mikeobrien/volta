using System.Linq;
using Volta.Core.Domain;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Core.Infrastructure.Framework.Security;

namespace Volta.Core.Application.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUserFactory _userFactory;

        public AuthenticationService(IRepository<User> userRepository, IUserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }

        public User Authenticate(string username, string password)
        {
            var user = _userRepository.FirstOrDefault(x => x.Username == username);
            if (user != null && HashedPassword.FromHash(user.PasswordHash).MatchesPassword(password)) return user;
            if (!_userRepository.Any()) return CreateUser(username, password);
            throw new AccessDeniedException();
        }

        private User CreateUser(string username, string password)
        {
            var user = _userFactory.Create(username, password, true);
            _userRepository.Add(user);
            return user;
        }
    }
}