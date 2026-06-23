using CatalogService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Persistence
{
    public class CatalogDbContext
    {
        private readonly IMongoDatabase _database;

        static CatalogDbContext()
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        }

        public CatalogDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoConnection");
            var databaseName = configuration["MongoDB:DatabaseName"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Category> Categories =>
            _database.GetCollection<Category>("Categories");

        public IMongoCollection<Product> Products =>
            _database.GetCollection<Product>("Products");
    }
}
