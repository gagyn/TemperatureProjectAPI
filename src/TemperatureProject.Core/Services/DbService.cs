using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace TemperatureProject.Core.Services
{
    public class DbService : IDbService
    {
        private const string DbName = "temperatureProject";
        private const string CollectionName = "temperatures";

        private readonly MongoClient client;

        private IMongoDatabase Database => client.GetDatabase(DbName);
        private IMongoCollection<TemperatureRecord> TemperaturesCollection
            => Database.GetCollection<TemperatureRecord>(CollectionName);

        static DbService()
        {
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("PascalCase", conventionPack, _ => true);
        }

        public DbService(string connectionString)
        {
            client = new MongoClient(connectionString);
        }

        public void Insert(IEnumerable<TemperatureRecord> records)
        {
            TemperaturesCollection.InsertMany(records);
        }

        public IQueryable<TemperatureRecord> GetAll()
        {
            return TemperaturesCollection.AsQueryable();
        }

        public IQueryable<TemperatureRecord> GetWhere(Expression<Func<TemperatureRecord, bool>> where)
        {
            return TemperaturesCollection.AsQueryable().Where(where);
        }

        public IQueryable<TemperatureRecord> GetBetween(DateTime fromDate, DateTime toDate)
        {
            return TemperaturesCollection.AsQueryable().Where(x => x.CreatedAt >= fromDate && x.CreatedAt <= toDate);
        }

        public long CountToNow(DateTime fromDate)
        {
            return CountBetween(fromDate, DateTime.Now);
        }

        public long CountBetween(DateTime fromDate, DateTime toDate)
        {
            return TemperaturesCollection.CountDocuments(x => x.CreatedAt >= fromDate && x.CreatedAt <= toDate);
        }

        public long CountAll()
        {
            return TemperaturesCollection.CountDocuments(_ => true);
        }

        public TemperatureRecord GetLast()
        {
            return TemperaturesCollection.Find(new BsonDocument()).Sort(new BsonDocument("$natural", -1)).First();
        }
    }
}
