using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using TemperatureProject.Core;

namespace TemperatureProject.BackupTool.Services
{
    internal class DbService
    {
        private const string DbName = "temperatureProject";
        private const string CollectionName = "temperatures";
        
        private readonly MongoClient client;

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
            var database = client.GetDatabase(DbName);
            database.GetCollection<TemperatureRecord>(CollectionName).InsertMany(records);
        }

        public IQueryable<TemperatureRecord> GetAll()
        {
            var database = client.GetDatabase(DbName);
            var temperatureRecords = database.GetCollection<TemperatureRecord>(CollectionName);
            return temperatureRecords.AsQueryable();
        }
    }
}
