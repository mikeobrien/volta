using NUnit.Framework;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Tests.Acceptance
{
    [SetUpFixture]
    public class Setup
    {
        [SetUp]
        public void Initialize()
        {
            var users = GetUserRepository();
            users.Delete(x => x.Username == Constants.TestUsername);
            new UserCreationService(users).Create(Constants.TestUsername, Constants.TestPassword, true);
        }

        [TearDown]
        public void Cleanup()
        {
            GetUserRepository().Delete(x => x.Username == Constants.TestUsername);
        }

        private static IRepository<User> GetUserRepository()
        {
            return new MongoRepository<User>(new MongoConnection(Constants.VoltaConnectionString));
        }
    }
}