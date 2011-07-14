using NUnit.Framework;
using Should;
using Volta.Web.Handlers;

namespace Volta.Tests.Unit.UserInterface
{
    [TestFixture]
    public class PagingModelTests
    {
        [Test]
        public void Should_Be_Valid_With_One_Page()
        {
            var paging = new PagingModel(5, 10, 1);
            paging.NotTheFirstPage.ShouldBeFalse();
            paging.InTheMiddle.ShouldBeFalse();
            paging.NotTheLastPage.ShouldBeFalse();
            paging.NextPage.ShouldEqual(0);
            paging.PreviousPage.ShouldEqual(0);
            paging.RecordStart.ShouldEqual(1);
            paging.RecordEnd.ShouldEqual(5);
            paging.TotalRecords.ShouldEqual(5);
            paging.SelectedRecords.ShouldEqual(5);
        }

        [Test]
        public void Should_Be_Valid_At_The_Begining()
        {
            var paging = new PagingModel(15, 10, 1);
            paging.NotTheFirstPage.ShouldBeFalse();
            paging.InTheMiddle.ShouldBeFalse();
            paging.NotTheLastPage.ShouldBeTrue();
            paging.NextPage.ShouldEqual(2);
            paging.PreviousPage.ShouldEqual(0);
            paging.RecordStart.ShouldEqual(1);
            paging.RecordEnd.ShouldEqual(10);
            paging.TotalRecords.ShouldEqual(15);
            paging.SelectedRecords.ShouldEqual(10);
        }

        [Test]
        public void Should_Be_Valid_In_The_Middle()
        {
            var paging = new PagingModel(25, 10, 2);
            paging.NotTheFirstPage.ShouldBeTrue();
            paging.InTheMiddle.ShouldBeTrue();
            paging.NotTheLastPage.ShouldBeTrue();
            paging.NextPage.ShouldEqual(3);
            paging.PreviousPage.ShouldEqual(1);
            paging.RecordStart.ShouldEqual(11);
            paging.RecordEnd.ShouldEqual(20);
            paging.TotalRecords.ShouldEqual(25);
            paging.SelectedRecords.ShouldEqual(10);
        }

        [Test]
        public void Should_Be_Valid_At_The_End()
        {
            var paging = new PagingModel(15, 10, 2);
            paging.NotTheFirstPage.ShouldBeTrue();
            paging.InTheMiddle.ShouldBeFalse();
            paging.NotTheLastPage.ShouldBeFalse();
            paging.NextPage.ShouldEqual(0);
            paging.PreviousPage.ShouldEqual(1);
            paging.RecordStart.ShouldEqual(11);
            paging.RecordEnd.ShouldEqual(15);
            paging.TotalRecords.ShouldEqual(15);
            paging.SelectedRecords.ShouldEqual(5);
        }
    }
}