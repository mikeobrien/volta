using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Tests.Integration.Infrastructure.Framework.Data
{
    [TestFixture]
    public class MongoRepositoryTests
    {
        private MongoConnection _mongo;
        private IRepository<_Person> _repository; 

        private static readonly _Person Person1 = new _Person { Age = 23, Id = Guid.NewGuid(), Name = "Niels" };
        private static readonly _Person Person2 = new _Person { Age = 45, Id = Guid.NewGuid(), Name = "Werner" };
        private static readonly _Person Person3 = new _Person { Age = 65, Id = Guid.NewGuid(), Name = "Wolfgang" };

        public class _Person
        {
            public int Age { get; set; }
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class PersonName
        {
            public string Name { get; set; }
        }

        [SetUp]
        public void Setup()
        {
            _mongo = new MongoConnection(Constants.VoltaIntegrationConnectionString);
            DropCollection<_Person>();
            var collection = GetCollection<_Person>();
            collection.Insert(Person1);
            collection.Insert(Person2);
            collection.Insert(Person3);
            _repository = new MongoRepository<_Person>(_mongo);
        }

        [TearDown]
        public void TearDown()
        {
            DropCollection<_Person>();
        }

        private MongoCollection<T> GetCollection<T>()
        {
            return _mongo.CreateConnection().GetDatabase(_mongo.DefaultDatabase).GetCollection<T>(typeof(T).Name);
        }

        private void DropCollection<T>()
        {
            if (_mongo.CreateConnection().GetDatabase(_mongo.DefaultDatabase).CollectionExists(typeof(T).Name))
                _mongo.CreateConnection().GetDatabase(_mongo.DefaultDatabase).DropCollection(typeof(T).Name);
        }

        [Test]
        public void should_query()
        {
            var results = _repository.Where(x => x.Name.StartsWith("W")).ToList();
            results.Count.ShouldEqual(2);
            results.Exists(x => x.Id == Person2.Id && x.Name == Person2.Name && x.Age == Person2.Age).ShouldBeTrue();
            results.Exists(x => x.Id == Person3.Id && x.Name == Person3.Name && x.Age == Person3.Age).ShouldBeTrue();
        }

        [Test]
        public void should_get_entity()
        {
            var result = _repository.Get(Person1.Id);
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(Person1.Id);
            result.Name.ShouldEqual(Person1.Name);
        }
         
        [Test]
        public void should_add_entity()
        {
            var person = new _Person { Name = "yada", Age = 22 };
            _repository.Add(person);
            var result = GetCollection<_Person>().AsQueryable<_Person>().FirstOrDefault(x => x.Name == "yada" && x.Age == 22);
            result.ShouldNotBeNull();
            result.Id.ShouldNotEqual(Guid.Empty);
            result.Name.ShouldEqual("yada");
            result.Age.ShouldEqual(22);
        }

        [Test]
        public void should_update_entity()
        {
            var person = new _Person { Id = Person2.Id, Name = "yada" };
            _repository.Replace(person);
            var result = GetCollection<_Person>().AsQueryable<_Person>().FirstOrDefault(x => x.Id == Person2.Id);
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(Person2.Id);
            result.Name.ShouldEqual("yada");
            result.Age.ShouldEqual(0);
        }

        [Test]
        public void should_update_object_entity()
        {
            _repository.Modify(Person2.Id, new PersonName { Name = "yada" });
            var result = GetCollection<_Person>().AsQueryable<_Person>().FirstOrDefault(x => x.Id == Person2.Id);
            result.ShouldNotBeNull();
            result.Name.ShouldEqual("yada");
            result.Id.ShouldEqual(Person2.Id);
            result.Age.ShouldEqual(45);
        }

        [Test]
        public void should_update_dynamic_entity()
        {
            _repository.Modify(Person2.Id, new { Name = "yada" });
            var result = GetCollection<_Person>().AsQueryable<_Person>().FirstOrDefault(x => x.Id == Person2.Id);
            result.ShouldNotBeNull();
            result.Name.ShouldEqual("yada");
            result.Id.ShouldEqual(Person2.Id);
            result.Age.ShouldEqual(45);
        }

        [Test]
        public void should_delete_entity_by_id()
        {
            _repository.Delete(Person2.Id);
            var collection = GetCollection<_Person>().AsQueryable<_Person>();
            collection.Count().ShouldEqual(2);
            collection.Any(x => x.Id == Person1.Id).ShouldBeTrue();
            collection.Any(x => x.Id == Person2.Id).ShouldBeFalse();
            collection.Any(x => x.Id == Person3.Id).ShouldBeTrue();
        }

        [Test]
        public void should_delete_entity()
        {
            _repository.Delete(Person2);
            var collection = GetCollection<_Person>().AsQueryable<_Person>();
            collection.Count().ShouldEqual(2);
            collection.Any(x => x.Id == Person1.Id).ShouldBeTrue();
            collection.Any(x => x.Id == Person2.Id).ShouldBeFalse();
            collection.Any(x => x.Id == Person3.Id).ShouldBeTrue();
        }
    }
}