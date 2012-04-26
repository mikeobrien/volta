using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Tests.Integration.Infrastructure.Framework.Data
{
    [TestFixture]
    public class JetQueryTests
    {
        public string Path = @"Integration\Infrastructure\Framework\Data\test.mdb";

        public class WidgetData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Length { get; set; }
            public DateTime CreatedOn { get; set; }
            public int[] Value { get; set; }
        }

        [Test]
        public void should_query_access_database()
        {
            using (var query = new JetQuery(Path))
            {
                var widgets = query.Table<WidgetData>().ToList();

                widgets[0].Id.ShouldEqual(1);
                widgets[0].Name.ShouldEqual("foo");
                widgets[0].Length.ShouldEqual(3.14159);
                widgets[0].CreatedOn.ShouldEqual(new DateTime(1985, 10, 5));
                widgets[0].Value[0].ShouldEqual(4);
                widgets[0].Value[1].ShouldEqual(2);
                widgets[0].Value[2].ShouldEqual(9);

                widgets[1].Id.ShouldEqual(2);
                widgets[1].Name.ShouldEqual("bar");
                widgets[1].Length.ShouldEqual(6.22);
                widgets[1].CreatedOn.ShouldEqual(new DateTime(2000, 1, 1));
                widgets[1].Value[0].ShouldEqual(3);
                widgets[1].Value[1].ShouldEqual(7);
                widgets[1].Value[2].ShouldEqual(6);
            }
        }
    }
}