using System;
using System.Linq;
using Norm;
using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Tests.Integration.Infrastructure
{
    [TestFixture]
    public class MongoRepositoryTests
    {
        private MongoConnection _mongo;
        private IRepository<Person> _repository; 

        private static readonly Person Person1 = new Person { Id = Guid.NewGuid(), Name = "Niels" };
        private static readonly Person Person2 = new Person { Id = Guid.NewGuid(), Name = "Werner" };
        private static readonly Person Person3 = new Person { Id = Guid.NewGuid(), Name = "Wolfgang" };

        public class Person
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        [SetUp]
        public void Setup()
        {
            _mongo = new MongoConnection(Constants.VoltaTestConnectionString);
            DropCollection<Person>();
            var collection = _mongo.Connection.GetCollection<Person>();
            collection.Insert(Person1);
            collection.Insert(Person2);
            collection.Insert(Person3);
            _repository = new MongoRepository<Person>(_mongo, new IdConvention());
        }

        [TearDown]
        public void TearDown()
        {
            DropCollection<Person>();
            _mongo.Connection.Dispose();
        }

        private void DropCollection<T>()
        {
            if (_mongo.Connection.Database.GetAllCollections().Any(x => x.Name.EndsWith(typeof(T).Name)))
                _mongo.Connection.Database.DropCollection(typeof(T).Name);
        }

        [Test]
        public void Should_Query()
        {
            var results = _repository.Where(x => x.Name.StartsWith("W")).ToList();
            results.Count.ShouldEqual(2);
            results.Exists(x => x.Id == Person2.Id && x.Name == Person2.Name);
            results.Exists(x => x.Id == Person3.Id && x.Name == Person3.Name);
        }

        [Test]
        public void Should_Get()
        {
            var result = _repository.Get(Person2.Id);
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(Person2.Id);
            result.Name.ShouldEqual(Person2.Name);
        }

        [Test]
        public void Should_Add()
        {
            var person = new Person {Id = Guid.Empty, Name = "yada"};
            _repository.Add(person);
            var result = _mongo.Connection.GetCollection<Person>().AsQueryable().FirstOrDefault(x => x.Id == Guid.Empty);
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(Guid.Empty);
            result.Name.ShouldEqual("yada");
        }

        [Test]
        public void Should_Update()
        {
            var person = new Person { Id = Person2.Id, Name = "yada" };
            _repository.Update(person);
            var collection = _mongo.Connection.GetCollection<Person>().AsQueryable().ToList();
            collection.Count().ShouldEqual(3);
            collection.Exists(x => x.Id == Person1.Id && x.Name == Person1.Name);
            collection.Exists(x => x.Id == Person2.Id && x.Name == "yada");
            collection.Exists(x => x.Id == Person3.Id && x.Name == Person3.Name);
        }

        [Test]
        public void Should_Delete_Single()
        {
            _repository.Delete(Person2.Id);
            var collection = _mongo.Connection.GetCollection<Person>().AsQueryable().ToList();
            collection.Count().ShouldEqual(2);
            collection.Exists(x => x.Id == Person1.Id && x.Name == Person1.Name);
            collection.Exists(x => x.Id == Person3.Id && x.Name == Person3.Name);
        }

        [Test]
        public void Should_Delete_Single_By_Expression()
        {
            _repository.Delete(Person2.Id);
            var collection = _mongo.Connection.GetCollection<Person>().AsQueryable().ToList();
            collection.Count().ShouldEqual(2);
            collection.Exists(x => x.Id == Person1.Id && x.Name == Person1.Name);
            collection.Exists(x => x.Id == Person3.Id && x.Name == Person3.Name);
        }

        [Test]
        public void Should_Delete_Multiple_By_Expression()
        {
            _repository.DeleteMany(x => x.Name.StartsWith("W"));
            var collection = _mongo.Connection.GetCollection<Person>().AsQueryable().ToList();
            collection.Count().ShouldEqual(1);
            collection.Exists(x => x.Id == Person1.Id && x.Name == Person1.Name);
        }
    }
}