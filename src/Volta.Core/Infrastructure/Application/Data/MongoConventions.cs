using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Volta.Core.Infrastructure.Application.Data
{
    public class MongoConventions
    {
        public static void Register()
        {
            BsonSerializer.RegisterIdGenerator(typeof (Guid), CombGuidGenerator.Instance);
        } 
    }
}