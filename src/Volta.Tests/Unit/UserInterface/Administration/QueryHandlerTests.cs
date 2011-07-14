using System.Linq;
using NUnit.Framework;
using Should;
using Volta.Core.Domain.Administration;
using Volta.Core.Infrastructure.Framework.Data;
using Volta.Web.Handlers.Administration.Users;

namespace Volta.Tests.Unit.UserInterface.Administration
{
    [TestFixture]
    public class QueryHandlerTests
    {
        private const int TotalRecords = 53;
        private const int PageSize = 20;
        private IRepository<User> _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new MemoryRepository<User>(Enumerable.Range(1, TotalRecords).Select(x => new User { Username = x.ToString().PadLeft(2, '0'), Administrator = (x % 2 == 0) }).ToArray());
        }

        [Test]
        public void Should_Return_First_Page_Of_Results()
        {
            var handler = new QueryHandler(_userRepository);
            var output = handler.Query();
            output.Paging.InTheMiddle.ShouldBeFalse();
            output.Paging.NextPage.ShouldEqual(2);
            output.Paging.NotTheFirstPage.ShouldBeFalse();
            output.Paging.NotTheLastPage.ShouldBeTrue();
            output.Paging.PreviousPage.ShouldEqual(0);
            output.Paging.RecordStart.ShouldEqual(1);
            output.Paging.RecordEnd.ShouldEqual(20);
            output.Paging.SelectedRecords.ShouldEqual(PageSize);
            output.Paging.TotalRecords.ShouldEqual(TotalRecords);
            output.Users.Count().ShouldEqual(20);
            output.Users.Min(x => int.Parse(x.Username)).ShouldEqual(1);
            output.Users.Max(x => int.Parse(x.Username)).ShouldEqual(20);
        }

        [Test]
        public void Should_Return_Page_Of_Results()
        {
            var handler = new QueryHandler(_userRepository);
            var output = handler.Query_Page(new QueryPagedModel {Page = 2});
            output.Paging.InTheMiddle.ShouldBeTrue();
            output.Paging.NextPage.ShouldEqual(3);
            output.Paging.NotTheFirstPage.ShouldBeTrue();
            output.Paging.NotTheLastPage.ShouldBeTrue();
            output.Paging.PreviousPage.ShouldEqual(1);
            output.Paging.RecordStart.ShouldEqual(21);
            output.Paging.RecordEnd.ShouldEqual(40);
            output.Paging.SelectedRecords.ShouldEqual(PageSize);
            output.Paging.TotalRecords.ShouldEqual(TotalRecords);
            output.Users.Count().ShouldEqual(20);
            output.Users.Min(x => int.Parse(x.Username)).ShouldEqual(21);
            output.Users.Max(x => int.Parse(x.Username)).ShouldEqual(40);
        }
    }
}