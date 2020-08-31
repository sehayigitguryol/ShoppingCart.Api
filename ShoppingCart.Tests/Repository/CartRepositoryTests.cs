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

namespace ShoppingCart.Tests.Repository
{
    public class CartRepositoryTests
    {
        private ShoppingCartContext _context;

        private IItemRepository _itemRepository;

        private ICartRepository _cartRepository;

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
            _cartRepository = new CartRepository(_context);
        }

        [Fact]
        public async Task CartRepository_GetItems_Success()
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

            var cart = new Cart()
            {
                Id = "c7f01298c560c211ebe9a73e",
                Items = new List<Item>() { item },
            };

            await _cartRepository.Add(cart);

            // Act
            var result = await _cartRepository.GetAll();

            // Assert
            Assert.NotEmpty(result);
            Assert.Single(result.ToList());
            Assert.Equal(cart.Id, result.First().Id);
            Assert.Equal(cart.Items.First().Id, result.First().Items.First().Id);

            _context.DropDatabase();
        }

        [Fact]
        public async Task CartRepository_GetItemById_ExistingId_Success()
        {
            // Arrange
            SetupEnvironment();
            var cartId = "c7f01298c560c211ebe9a73e";

            var item = new Item()
            {
                Id = "d3ab2dfa878227b15f1a0575",
                Name = "Rose",
                Description = "Thorns",
                Price = 12,
                Quantity = 5
            };

            await _itemRepository.Add(item);

            var cart = new Cart()
            {
                Id = cartId,
                Items = new List<Item>() { item },
            };

            await _cartRepository.Add(cart);

            // Act
            var result = await _cartRepository.Find(cartId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cart.Id, result.Id);
            Assert.Equal(cart.Items.Count, result.Items.Count);
            Assert.Equal(cart.Items.First().Id, result.Items.First().Id);

            _context.DropDatabase();
        }

        [Fact]
        public async Task CartRepository_GetItemById_NotExistingId_Failure()
        {
            // Arrange
            SetupEnvironment();

            var id = "d3ab2dfa878227b15f1a0575";

            // Act
            var result = await _cartRepository.Find(id);

            // Assert
            Assert.Null(result);

            _context.DropDatabase();
        }

        [Fact]
        public async Task CartRepository_UpdateCart_AddNewItemExistingCart_Success()
        {
            // Arrange
            SetupEnvironment();
            var cartId = "c7f01298c560c211ebe9a73e";

            var item = new Item()
            {
                Id = "d3ab2dfa878227b15f1a0575",
                Name = "Rose",
                Description = "Thorns",
                Price = 12,
                Quantity = 5
            };

            await _itemRepository.Add(item);

            var cart = new Cart()
            {
                Id = cartId,
                Items = new List<Item>() { item },
            };

            await _cartRepository.Add(cart);


            var modifiedCart = new Cart()
            {
                Id = cartId,
                Items = new List<Item>() {
                    item,
                    new Item()
                    {
                        Id = "745a1ff5056c2673f21bfe50",
                        Name = "Gardenia",
                        Description = "Purple",
                        Price = 5,
                        Quantity = 3
                    }
                }
            };

            // Act
            var result = await _cartRepository.Update(modifiedCart);

            // Assert
            var retrievedCart = await _cartRepository.Find(cartId);

            Assert.True(result);

            Assert.Equal(cart.Id, retrievedCart.Id);
            Assert.Equal(2, retrievedCart.Items.Count);
            Assert.Equal("d3ab2dfa878227b15f1a0575", retrievedCart.Items[0].Id);
            Assert.Equal("Rose", retrievedCart.Items[0].Name);
            Assert.Equal("745a1ff5056c2673f21bfe50", retrievedCart.Items[1].Id);
            Assert.Equal("Gardenia", retrievedCart.Items[1].Name);

            _context.DropDatabase();
        }

        [Fact]
        public async Task CartRepository_UpdateCart_DeleteItemExistingCart_Success()
        {
            // Arrange
            SetupEnvironment();
            var cartId = "c7f01298c560c211ebe9a73e";

            var item = new Item()
            {
                Id = "d3ab2dfa878227b15f1a0575",
                Name = "Rose",
                Description = "Thorns",
                Price = 12,
                Quantity = 5
            };

            await _itemRepository.Add(item);

            var cart = new Cart()
            {
                Id = cartId,
                Items = new List<Item>() { item },
            };

            await _cartRepository.Add(cart);


            var modifiedCart = new Cart()
            {
                Id = cartId,
                Items = new List<Item>() { }
            };

            // Act
            var result = await _cartRepository.Update(modifiedCart);

            // Assert
            var retrievedCart = await _cartRepository.Find(cartId);

            Assert.True(result);
            Assert.Equal(cart.Id, retrievedCart.Id);
            Assert.Empty(retrievedCart.Items);

            _context.DropDatabase();
        }

        [Fact]
        public async Task CartRepository_UpdateCart_NotExistingCart_Success()
        {
            // Arrange
            SetupEnvironment();
            var cartId = "c7f01298c560c211ebe9a73e";

            var item = new Item()
            {
                Id = "d3ab2dfa878227b15f1a0575",
                Name = "Rose",
                Description = "Thorns",
                Price = 12,
                Quantity = 5
            };

            await _itemRepository.Add(item);

            var cart = new Cart()
            {
                Id = cartId,
                Items = new List<Item>() { item },
            };

            // Act
            var result = await _cartRepository.Update(cart);

            // Assert
            var retrievedCart = await _cartRepository.Find(cartId);

            Assert.False(result);
            Assert.Null(retrievedCart);

            _context.DropDatabase();
        }

        [Fact]
        public async Task CartRepository_DeleteItemById_ExistingId_Success()
        {
            // Arrange
            SetupEnvironment();
            var cartId = "c7f01298c560c211ebe9a73e";

            var item = new Item()
            {
                Id = "d3ab2dfa878227b15f1a0575",
                Name = "Rose",
                Description = "Thorns",
                Price = 12,
                Quantity = 5
            };

            await _itemRepository.Add(item);

            var cart = new Cart()
            {
                Id = cartId,
                Items = new List<Item>() { item },
            };

            await _cartRepository.Add(cart);

            // Act
            var result = await _cartRepository.Delete(cartId);

            // Assert
            var retrievedCart = await _cartRepository.Find(cartId);

            Assert.True(result);
            Assert.Null(retrievedCart);

            _context.DropDatabase();
        }

        [Fact]
        public async Task CartRepository_DeleteItemById_NotExistingId_Success()
        {
            // Arrange
            SetupEnvironment();
            var cartId = "c7f01298c560c211ebe9a73e";

            // Act
            var result = await _cartRepository.Delete(cartId);

            // Assert
            var retrievedCart = await _cartRepository.Find(cartId);

            Assert.False(result);
            Assert.Null(retrievedCart);

            _context.DropDatabase();
        }
    }
}
