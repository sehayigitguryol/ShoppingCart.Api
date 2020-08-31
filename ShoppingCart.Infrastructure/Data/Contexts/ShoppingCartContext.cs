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
        IMongoCollection<Item> Items { get; }
    }

    public class ShoppingCartContext : IShoppingCartContext
    {
        private readonly IMongoDatabase _mongoDatabase;

        public ShoppingCartContext(MongoDbConfigurations config)
        {
            var connectionString = GetConnectionString(config);
            var client = new MongoClient(connectionString);

            _mongoDatabase = client.GetDatabase(config.Database);
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
                connectionString = $@"mongodb://{config.User}:{config.Password}@{config.Host}:{config.Port}";
            }

            return connectionString;
        }

        public IMongoCollection<Item> Items => _mongoDatabase.GetCollection<Item>("Items");

    }
}
