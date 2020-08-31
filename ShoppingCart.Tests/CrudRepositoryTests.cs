using MongoDB.Bson;
using ShoppingCart.Core.Entities;
using ShoppingCart.Core.Repositories;
using ShoppingCart.Infrastructure.Configurations;
using ShoppingCart.Infrastructure.Data.Contexts;
using ShoppingCart.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.Tests
{
    public class CrudRepositoryTests
    {
        private IShoppingCartContext _context;

        private IItemRepository _itemRepository;


        private void SetupEnvironment()
        {
            var dbGuid = new Guid();

            var configs = new MongoDbConfigurations()
            {
                Database = $"test_dv_{dbGuid.ToString()}",
                Host = "localhost",
                Port = 27017,
                User = "root",
                Password = "password",
                MasterDatabaseName = "admin"
            };

            _context = new ShoppingCartContext(configs);

            _itemRepository = new ItemRepository(_context);
            var item = new Item()
            {
                Id = "e6e6e6e611",
                Name = "Rose",
                Description = "Thorns",
                Price = 12,
                Quantity = 5
            };

            _context.Items.InsertOne(item);
        }

        [Fact]
        public async Task ItemRepository_GetItems_Success()
        {
            // Arrange

            SetupEnvironment();

            var item = new Item()
            {
                Id = "e6e6e6e611",
                Name = "Rose",
                Description = "Thorns",
                Price = 12,
                Quantity = 5
            };

            await _itemRepository.Add(item);

            // Act
            var result = await _itemRepository.GetAll();

            // Assert
            Assert.NotEmpty(result);

        }
    }
}
