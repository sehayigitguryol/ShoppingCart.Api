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
    public class MongoDbContextTests
    {
        private IShoppingCartContext _context;

        private IItemRepository _itemRepository;

        private void SetupEnvironment()
        {
            var dbGuid = new Guid();

            var configs = new MongoDbConfigurations()
            {
                Database = dbGuid.ToString(),
                Host = "localhost",
                Port = 27017,
                User = "root",
                Password = "password"
            };
            _context = new ShoppingCartContext(configs);

            _itemRepository = new ItemRepository(_context);
        }

        [Fact]
        public void ShoppingCartContext_Initialize_Success()
        {
            // Arrange
            var configs = new MongoDbConfigurations()
            {
                Host = "test",
                Port = 27017,
                Database = "TestDB"
            };

            // Act
            var context = new ShoppingCartContext(configs);

            // Assert
            Assert.NotNull(context);
        }

        [Fact]
        public void ShoppingCartContext_GetItemCollection_Success()
        {
            // Arrange
            SetupEnvironment();

            // Act
            var itemCollection = _context.GetCollection<Item>("Item");

            // Assert
            Assert.NotNull(itemCollection);
        }

        [Fact]
        public async void ShoppingCartContext_AddItem_Success()
        {
            // Arrange
            SetupEnvironment();

            var item = new Item()
            {
                Id = "123",
                Name = "Rose",
                Description = "Thorns",
                Price = 12,
                Quantity = 5
            };

            // Act
            Task task = _itemRepository.Add(item);

            // Assert
            Assert.True(task.IsCompletedSuccessfully);
        }
    }
}
