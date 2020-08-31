using ShoppingCart.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.Tests.Service
{
    public class ItemServiceTests
    {
        [Fact]
        public async Task CreateItem_Success()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var item = new Item()
                {
                    Id = "d3ab2dfa878227b15f1a0575",
                    Name = "Rose",
                    Description = "Thorns",
                    Price = 12,
                    Quantity = 5
                };

                // Act
                var result = await tester.ItemService.CreateItemAsync(item);

                // Assert
                var resultItems = tester.ItemRepository.GetAll().Result.ToList();

                Assert.Equal(1, resultItems.Count);
                Assert.Equal(item.Id, resultItems.First().Id);
                Assert.Equal(item.Name, resultItems.First().Name);
                Assert.Equal(item.Description, resultItems.First().Description);
                Assert.Equal(item.Price, resultItems.First().Price);
                Assert.Equal(item.Quantity, resultItems.First().Quantity);
            }
        }

        [Fact]
        public async Task UpdateItem_ExistingItem_Success()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var id = "d3ab2dfa878227b15f1a0575";
                var item = new Item()
                {
                    Id = id,
                    Name = "Rose",
                    Description = "Thorns",
                    Price = 12,
                    Quantity = 5
                };
                await tester.ItemRepository.Add(item);

                var modifiedItem = new Item()
                {
                    Id = id,
                    Name = "Petunia",
                    Description = "smells good",
                    Price = 33,
                    Quantity = 10
                };

                // Act
                await tester.ItemService.UpdateItemAsync(modifiedItem);

                // Assert
                var resultItems = tester.ItemRepository.GetAll().Result.ToList();

                Assert.Single(resultItems);
                Assert.Equal(modifiedItem.Id, resultItems.First().Id);
                Assert.Equal(modifiedItem.Name, resultItems.First().Name);
                Assert.Equal(modifiedItem.Description, resultItems.First().Description);
                Assert.Equal(modifiedItem.Price, resultItems.First().Price);
                Assert.Equal(modifiedItem.Quantity, resultItems.First().Quantity);
            }
        }

        [Fact]
        public async Task UpdateItem_NotExistingItem_Failure()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
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
                var result = await tester.ItemService.UpdateItemAsync(modifiedItem);

                // Assert

                Assert.False(result);
            }
        }

        [Fact]
        public async Task GetAllItems_MultipleItem_Success()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var item1 = new Item()
                {
                    Id = "d3ab2dfa878227b15f1a0575",
                    Name = "Rose",
                    Description = "Thorns",
                    Price = 12,
                    Quantity = 5
                };

                var item2 = new Item()
                {
                    Id = "3d78df8abb8f04bce61b869b",
                    Name = "Petunia",
                    Description = "Yellow",
                    Price = 12,
                    Quantity = 5
                };

                var item3 = new Item()
                {
                    Id = "fab62b09cd8581c44e87ba6a",
                    Name = "Orchid",
                    Description = "White",
                    Price = 12,
                    Quantity = 5
                };

                await tester.ItemRepository.Add(item1);
                await tester.ItemRepository.Add(item2);
                await tester.ItemRepository.Add(item3);

                // Act
                var result = await tester.ItemService.GetAllItemsAsync();
                var resultItems = result.ToList();

                // Assert
                Assert.Equal(3, resultItems.Count);
                Assert.Equal(item1.Id, resultItems.First().Id);
            }
        }

        [Fact]
        public async Task DeleteItem_ExistingItem_Success()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var id = "d3ab2dfa878227b15f1a0575";
                var item = new Item()
                {
                    Id = id,
                    Name = "Rose",
                    Description = "Thorns",
                    Price = 12,
                    Quantity = 5
                };
                await tester.ItemRepository.Add(item);

                // Act
                var result = await tester.ItemService.DeleteItemAsync(id);

                // Assert
                var resultItems = tester.ItemRepository.GetAll().Result.ToList();

                Assert.Empty(resultItems);
                Assert.True(result);
            }
        }

        [Fact]
        public async Task DeleteItem_NotExistingItem_Success()
        {
            using (var tester = new ShoppingCartTester())
            {
                // Assert
                var id = "d3ab2dfa878227b15f1a0575";

                // Act
                var result = await tester.ItemService.DeleteItemAsync(id);

                // Assert
                var resultItems = tester.ItemRepository.GetAll().Result.ToList();

                Assert.Empty(resultItems);
                Assert.False(result);
            }
        }
    }

}
