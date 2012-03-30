using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Tests.Integration.Infrastructure.Framework.Data
{
    [TestFixture]
    public class MongoRepositoryTests
    {
        private MongoConnection _mongo;
        private IRepository<Person> _repository; 

        private static readonly Person Person1 = new Person { Id = ObjectId.GenerateNewId(), Name = "Niels" };
        private static readonly Person Person2 = new Person { Id = ObjectId.GenerateNewId(), Name = "Werner" };
        private static readonly Person Person3 = new Person { Id = ObjectId.GenerateNewId(), Name = "Wolfgang" };

        public class Person
        {
            public ObjectId Id { get; set; }
            public string Name { get; set; }
        }

        [SetUp]
        public void Setup()
        {
            _mongo = new MongoConnection(Constants.VoltaConnectionString);
            DropCollection<Person>();
            var collection = GetCollection<Person>();
            collection.Insert(Person1);
            collection.Insert(Person2);
            collection.Insert(Person3);
            _repository = new MongoRepository<Person>(_mongo);
        }

        [TearDown]
        public void TearDown()
        {
            DropCollection<Person>();
        }

        private MongoCollection<T> GetCollection<T>()
        {
            return _mongo.Connection.GetDatabase(_mongo.DefaultDatabase).GetCollection<T>(typeof(T).Name);
        }

        private void DropCollection<T>()
        {
            if (_mongo.Connection.GetDatabase(_mongo.DefaultDatabase).CollectionExists(typeof(T).Name))
                _mongo.Connection.GetDatabase(_mongo.DefaultDatabase).DropCollection(typeof(T).Name);
        }

        [Test]
        public void should_query()
        {
            var results = _repository.Where(x => x.Name.StartsWith("W")).ToList();
            results.Count.ShouldEqual(2);
            results.Exists(x => x.Id == Person2.Id && x.Name == Person2.Name);
            results.Exists(x => x.Id == Person3.Id && x.Name == Person3.Name);
        }

        [Test]
        public void should_get_object()
        {
            var result = _repository.Get(Person1.Id);
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(Person1.Id);
            result.Name.ShouldEqual(Person1.Name);
        }

        [Test]
        public void should_add_object()
        {
            var person = new Person { Id = Guid.Empty, Name = "yada" };
            _repository.Add(person);
            var result = _mongo.Connection.GetCollection<Person>().AsQueryable().FirstOrDefault(x => x.Id == Guid.Empty);
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(Guid.Empty);
            result.Name.ShouldEqual("yada");
        }

        [Test]
        public void should_update_object()
        {
            var person = new Person { Id = Person2.Id, Name = "yada" };
            _repository.Update(x => x.Id, person);
            var collection = _mongo.Connection.GetCollection<Person>().AsQueryable().ToList();
            collection.Count().ShouldEqual(3);
            collection.Exists(x => x.Id == Person1.Id && x.Name == Person1.Name);
            collection.Exists(x => x.Id == Person2.Id && x.Name == "yada");
            collection.Exists(x => x.Id == Person3.Id && x.Name == Person3.Name);
        }

        [Test]
        public void should_delete_object()
        {
            _repository.Delete(x => x.Id == Person2.Id);
            var collection = _mongo.Connection.GetCollection<Person>().AsQueryable().ToList();
            collection.Count().ShouldEqual(2);
            collection.Exists(x => x.Id == Person1.Id && x.Name == Person1.Name);
            collection.Exists(x => x.Id == Person3.Id && x.Name == Person3.Name);
        }
    }
}