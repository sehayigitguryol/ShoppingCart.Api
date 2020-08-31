using MongoDB.Bson;
using ShoppingCart.Core.Entities;
using ShoppingCart.Core.Repositories;
using ShoppingCart.Infrastructure.Configurations;
using ShoppingCart.Infrastructure.Data.Contexts;
using ShoppingCart.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.Tests
{
    public class CrudRepositoryTests
    {
        private ShoppingCartContext _context;

        private IItemRepository _itemRepository;


        private void SetupEnvironment()
        {
            var dbGuid = Guid.NewGuid();

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
        }

        [Fact]
        public async Task ItemRepository_GetItems_Success()
        {
            // Arrange
            SetupEnvironment();

            var item = new Item()
            {
                Id = "d3ab2dfa878227b15f1a0575",
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
            Assert.Single(result.ToList());
            Assert.Equal(item.Id, result.First().Id);
            Assert.Equal(item.Name, result.First().Name);
            Assert.Equal(item.Id, result.First().Id);

            _context.DropDatabase();
        }

        [Fact]
        public async Task ItemRepository_GetItemById_ExistingId_Success()
        {
            // Arrange
            SetupEnvironment();

            var id = "d3ab2dfa878227b15f1a0575";

            var item = new Item()
            {
                Id = id,
                Name = "Rose",
                Description = "Thorns",
                Price = 12,
                Quantity = 5
            };

            await _itemRepository.Add(item);

            // Act
            var result = await _itemRepository.Find(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(item.Id, result.Id);
            Assert.Equal(item.Name, result.Name);
            Assert.Equal(item.Id, result.Id);

            _context.DropDatabase();
        }

        [Fact]
        public async Task ItemRepository_GetItemById_NotExistingId_Failure()
        {
            // Arrange
            SetupEnvironment();

            var id = "d3ab2dfa878227b15f1a0575";

            var item = new Item()
            {
                Id = id,
                Name = "Rose",
                Description = "Thorns",
                Price = 12,
                Quantity = 5
            };

            var notExistingId = "111111fa878227b15f1a0575";

            await _itemRepository.Add(item);

            // Act
            var result = await _itemRepository.Find(notExistingId);

            // Assert
            Assert.Null(result);
            _context.DropDatabase();
        }

        [Fact]
        public async Task ItemRepository_UpdateItem_ExistingItem_Success()
        {
            // Arrange
            SetupEnvironment();

            var id = "d3ab2dfa878227b15f1a0575";

            var item = new Item()
            {
                Id = id,
                Name = "Rose",
                Description = "Thorns",
                Price = 12,
                Quantity = 5
            };

            await _itemRepository.Add(item);

            var modifiedItem = new Item()
            {
                Id = id,
                Name = "Petunia",
                Description = "smells good",
                Price = 33,
                Quantity = 10
            };

            // Act
            var updateResult = _itemRepository.Update(modifiedItem).Result;

            // Assert
            var result = await _itemRepository.Find(id);

            Assert.True(updateResult);
            Assert.Equal(id, result.Id);
            Assert.Equal(modifiedItem.Name, result.Name);
            _context.DropDatabase();
        }

        [Fact]
        public async Task ItemRepository_UpdateItem_NotExistingItem_Failure()
        {
            // Arrange
            SetupEnvironment();

            var id = "d3ab2dfa878227b15f1a0575";

            var modifiedItem = new Item()
            {
                Id = id,
                Name = "Petunia",
                Description = "smells good",
                Price = 33,
                Quantity = 10
            };

            // Act
            var updateResult = _itemRepository.Update(modifiedItem).Result;

            // Assert
            var result = await _itemRepository.Find(id);

            Assert.False(updateResult);
            Assert.Null(result);
            _context.DropDatabase();
        }

    }
}
