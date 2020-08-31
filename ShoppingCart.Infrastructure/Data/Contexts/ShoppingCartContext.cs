using MongoDB.Driver;
using ShoppingCart.Core.Entities;
using ShoppingCart.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Infrastructure.Data.Contexts
{
    public interface IShoppingCartContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
        IMongoCollection<Item> Items { get; }
    }

    public class ShoppingCartContext : IShoppingCartContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoClient _mongoClient;

        public ShoppingCartContext(MongoDbConfigurations config)
        {
            MongoCredential credential = MongoCredential.CreateCredential(config.MasterDatabaseName, config.User, config.Password);

            var settings = new MongoClientSettings
            {
                Credential = credential,
                Server = new MongoServerAddress(config.Host, config.Port)
            };

            _mongoClient = new MongoClient(settings);

            _mongoDatabase = _mongoClient.GetDatabase(config.Database);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _mongoDatabase.GetCollection<T>(name);
        }

        private string GetConnectionString(MongoDbConfigurations config)
        {
            string connectionString;
            if (string.IsNullOrEmpty(config.User) || string.IsNullOrEmpty(config.Password))
            {
                connectionString = $@"mongodb://{config.Host}:{config.Port}";
            }
            else
            {
                connectionString = $@"mongodb://{config.User}:{config.Password}@{config.Host}:{config.Port}/admin";
            }

            return connectionString;
        }

        public IMongoCollection<Item> Items => _mongoDatabase.GetCollection<Item>("Items");

    }
}
