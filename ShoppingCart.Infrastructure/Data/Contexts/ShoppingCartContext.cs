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
        /// <summary>
        /// Gets specified mongo collection for given name
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="name">name of collection</param>
        /// <returns>Instance of mongo collection by name</returns>
        IMongoCollection<T> GetCollection<T>(string name);

        /// <summary>
        /// Cleans DB after test
        /// </summary>
        void DropDatabase();
    }

    public class ShoppingCartContext : IShoppingCartContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoClient _mongoClient;
        private readonly MongoDbConfigurations _mongoDbConfigurations;

        public ShoppingCartContext(MongoDbConfigurations config)
        {
            _mongoDbConfigurations = config;

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

        public void DropDatabase()
        {
            _mongoClient.DropDatabase(_mongoDbConfigurations.Database);
        }

    }
}
