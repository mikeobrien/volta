using System.Linq;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Tests.Unit.Infrastructure.Framework.Data
{
    [TestFixture]
    public class QueryableExtensionsTests
    {
        public class Entity { public int Id { get; set; } }        
        
        private readonly IRepository<Entity> _userRepository  = 
            new MemoryRepository<Entity>(Enumerable.Range(1, 53).Select(x => new Entity { Id = x }).ToArray());

        [Test]
        public void Should_Return_Paged_Result_With_Correct_Total()
        {
            var result = _userRepository.GetPage(10, 2);
            result.TotalRecords.ShouldEqual(53);
            result.Results.Count().ShouldEqual(10);
            result.Results.Min(x => x.Id).ShouldEqual(11);
            result.Results.Max(x => x.Id).ShouldEqual(20);
        }
    }
}