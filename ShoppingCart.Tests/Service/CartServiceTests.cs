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
        public async Task AddItemToCart_EmptyCart_ExistingItem_SufficientQuantity_Success()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var cart = new Cart()
                {
                    Id = "d3ab2dfa878227b15f1a0575",
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

                var addModel = new AddItemToCartModel()
                {
                    CartId = cart.Id,
                    ItemId = item1.Id,
                    Amount = 3
                };

                // Act
                var (result, message) = await tester.CartService.AddItemToCart(addModel);

                // Assert
                var resultItems = tester.CartRepository.GetAll().Result.ToList();

                Assert.Equal(1, resultItems.Count);
                Assert.Equal(cart.Id, resultItems.First().Id);
                Assert.Null(message);
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
