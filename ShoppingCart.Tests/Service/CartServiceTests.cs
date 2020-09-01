using ShoppingCart.Core.Entities;
using ShoppingCart.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.Tests.Service
{
    public class CartServiceTests
    {
        [Fact]
        public async Task CreateCart_WithoutItem_Success()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var cart = new Cart()
                {
                    Id = "d3ab2dfa878227b15f1a0575",
                    Items = new List<Item>()
                };

                // Act
                await tester.CartRepository.Add(cart);

                // Assert
                var resultItems = tester.CartRepository.GetAll().Result.ToList();

                Assert.Equal(1, resultItems.Count);
                Assert.Equal(cart.Id, resultItems.First().Id);
            }
        }

        [Fact]
        public async Task AddItemToCart_NotExistingCart_Failure()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var cartId = "16a70cb62d6a2ff9bfffe180";

                var item1 = new Item()
                {
                    Name = "Rose",
                    Description = "Red",
                    Quantity = 10,
                    Price = 5
                };

                await tester.ItemRepository.Add(item1);

                tester.StockCache.SetStock(item1.Id, item1.Quantity);

                var addModel = new AddItemToCartModel()
                {
                    CartId = cartId,
                    ItemId = item1.Id,
                    Amount = 3
                };

                // Act
                var (result, message) = await tester.CartService.AddItemToCart(addModel);

                // Assert
                var cartFromDb = tester.CartRepository.Find(cartId).Result;

                Assert.Null(result);
                Assert.NotNull(message);
                Assert.Null(cartFromDb);
            }
        }

        [Fact]
        public async Task AddItemToCart_EmptyCart_ExistingItem_SufficientQuantity_Success()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var cartId = "16a70cb62d6a2ff9bfffe180";

                var cart = new Cart()
                {
                    Id = cartId,
                    Items = new List<Item>()
                };

                var item1 = new Item()
                {
                    Name = "Rose",
                    Description = "Red",
                    Quantity = 10,
                    Price = 5
                };

                var item2 = new Item()
                {
                    Name = "Daisy",
                    Description = "White",
                    Quantity = 3,
                    Price = 2
                };

                await tester.CartRepository.Add(cart);
                await tester.ItemRepository.Add(item1);
                await tester.ItemRepository.Add(item2);

                tester.StockCache.SetStock(item1.Id, item1.Quantity);
                tester.StockCache.SetStock(item2.Id, item2.Quantity);

                var addModel = new AddItemToCartModel()
                {
                    CartId = cart.Id,
                    ItemId = item1.Id,
                    Amount = 3
                };

                // Act
                var (result, message) = await tester.CartService.AddItemToCart(addModel);

                // Assert
                var cartFromDb = tester.CartRepository.Find(cartId).Result;
                var remainingStock = tester.StockCache.GetStock(item1.Id);

                Assert.NotNull(result);
                Assert.Null(message);
                Assert.NotNull(cartFromDb);
                Assert.Equal(cart.Id, cartFromDb.Id);
                Assert.Single(cartFromDb.Items);
                Assert.Equal(item1.Id, cartFromDb.Items.First().Id);
                Assert.Equal(3, cartFromDb.Items.First().Quantity);
                Assert.Equal(7, remainingStock);
            }
        }

        [Fact]
        public async Task AddItemToCart_EmptyCart_ExistingItem_InsufficientQuantity_Success()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var cartId = "16a70cb62d6a2ff9bfffe180";

                var cart = new Cart()
                {
                    Id = cartId,
                    Items = new List<Item>()
                };

                var item1 = new Item()
                {
                    Name = "Rose",
                    Description = "Red",
                    Quantity = 10,
                    Price = 5
                };


                var item2 = new Item()
                {
                    Name = "Daisy",
                    Description = "White",
                    Quantity = 3,
                    Price = 2
                };

                await tester.CartRepository.Add(cart);
                await tester.ItemRepository.Add(item1);
                await tester.ItemRepository.Add(item2);

                tester.StockCache.SetStock(item1.Id, item1.Quantity);
                tester.StockCache.SetStock(item2.Id, item2.Quantity);

                var addModel = new AddItemToCartModel()
                {
                    CartId = cart.Id,
                    ItemId = item1.Id,
                    Amount = 100
                };

                // Act
                var (result, message) = await tester.CartService.AddItemToCart(addModel);

                // Assert
                var cartFromDb = tester.CartRepository.Find(cartId).Result;
                var remainingStock = tester.StockCache.GetStock(item1.Id);

                Assert.Null(result);
                Assert.NotNull(message);
                Assert.NotNull(cartFromDb);
                Assert.Equal(cart.Id, cartFromDb.Id);
                Assert.Empty(cartFromDb.Items);
                Assert.Equal(10, remainingStock);
            }
        }

        [Fact]
        public async Task AddItemToCart_EmptyCart_ExistingItem_ZeroStock_Failure()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var cartId = "16a70cb62d6a2ff9bfffe180";

                var cart = new Cart()
                {
                    Id = cartId,
                    Items = new List<Item>()
                };

                var item1 = new Item()
                {
                    Name = "Rose",
                    Description = "Red",
                    Quantity = 0,
                    Price = 5
                };


                var item2 = new Item()
                {
                    Name = "Daisy",
                    Description = "White",
                    Quantity = 3,
                    Price = 2
                };

                await tester.CartRepository.Add(cart);
                await tester.ItemRepository.Add(item1);
                await tester.ItemRepository.Add(item2);

                tester.StockCache.SetStock(item1.Id, item1.Quantity);
                tester.StockCache.SetStock(item2.Id, item2.Quantity);

                var addModel = new AddItemToCartModel()
                {
                    CartId = cart.Id,
                    ItemId = item1.Id,
                    Amount = 3
                };

                // Act
                var (result, message) = await tester.CartService.AddItemToCart(addModel);

                // Assert
                var cartFromDb = tester.CartRepository.Find(cartId).Result;
                var remainingStock = tester.StockCache.GetStock(item1.Id);

                Assert.Null(result);
                Assert.NotNull(message);
                Assert.NotNull(cartFromDb);
                Assert.Equal(cart.Id, cartFromDb.Id);
                Assert.Empty(cartFromDb.Items);
                Assert.Equal(0, remainingStock);
            }
        }

        [Fact]
        public async Task AddItemToCart_NotEmptyCart_ExistingItem_SufficientQuantity_Success()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var cartId = "16a70cb62d6a2ff9bfffe180";

                var item1 = new Item()
                {
                    Id = "f8b23481c857dc3ca51be522",
                    Name = "Rose",
                    Description = "Red",
                    Quantity = 10,
                    Price = 5
                };

                var item2 = new Item()
                {
                    Id = "dc6ae8884a76e5f948f97d9f",
                    Name = "Daisy",
                    Description = "White",
                    Quantity = 3,
                    Price = 2
                };

                var cart = new Cart()
                {
                    Id = cartId,
                    Items = new List<Item>() { item1, item2 }
                };

                await tester.ItemRepository.Add(item1);
                await tester.ItemRepository.Add(item2);
                await tester.CartRepository.Add(cart);

                tester.StockCache.SetStock(item1.Id, 20);
                tester.StockCache.SetStock(item2.Id, 10);

                var addModel = new AddItemToCartModel()
                {
                    CartId = cart.Id,
                    ItemId = item1.Id,
                    Amount = 3
                };

                // Act
                var (result, message) = await tester.CartService.AddItemToCart(addModel);

                // Assert
                var cartFromDb = tester.CartRepository.Find(cartId).Result;
                var remainingStock = tester.StockCache.GetStock(item1.Id);

                Assert.NotNull(result);
                Assert.Null(message);
                Assert.NotNull(cartFromDb);
                Assert.Equal(cart.Id, cartFromDb.Id);

                Assert.Equal(item1.Id, cartFromDb.Items.First().Id);
                Assert.Equal(13, cartFromDb.Items.First().Quantity);
                Assert.Equal(17, remainingStock);
            }
        }

        [Fact]
        public async Task AddItemToCart_NotExistingItem_Failure()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var cart = new Cart()
                {
                    Id = "d3ab2dfa878227b15f1a0575",
                    Items = new List<Item>()
                };

                var notExistingItemId = "00002dfa878227b15f1a0000";

                await tester.CartRepository.Add(cart);


                var addModel = new AddItemToCartModel()
                {
                    CartId = cart.Id,
                    ItemId = notExistingItemId,
                    Amount = 3
                };

                // Act
                var (result, message) = await tester.CartService.AddItemToCart(addModel);

                // Assert
                var resultItems = tester.CartRepository.GetAll().Result.ToList();

                Assert.Empty(resultItems.First().Items);
                Assert.Null(result);
                Assert.NotNull(message);
            }
        }

    }
}
