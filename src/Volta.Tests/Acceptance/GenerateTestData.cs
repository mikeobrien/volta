using System.Linq;
using NUnit.Framework;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Tests.Integration;

namespace Volta.Tests.Acceptance
{
    [TestFixture(Ignore = true)]
    public class GenerateTestData
    {
        [Test] 
        public void generate_users()
        {
            var users = new UserFactory(new MongoRepository<User>(new MongoConnection(Constants.VoltaAcceptanceConnectionString)));
            Enumerable.Range(1, 50).ToList().ForEach(x => users.Create(string.Format("testuser{0}", x), "t3$t", string.Format("test{0}@test.com", x), x % 5 == 0));
        }
    }
}