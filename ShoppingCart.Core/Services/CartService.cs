using ShoppingCart.Core.Entities;
using ShoppingCart.Core.Models;
using ShoppingCart.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Core.Services
{
    public interface ICartService
    {
        Task<(Cart, string)> AddItemToCart(AddItemToCartRequest model);

        Task<InitializeDefaultCartsResponse> InitializeCarts();

        Task AddCart();
    }

    public class CartService : ICartService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IStockCache _stockCache;

        public CartService(IItemRepository itemRepository, ICartRepository cartRepository, IStockCache stockCache)
        {
            _itemRepository = itemRepository;
            _cartRepository = cartRepository;
            _stockCache = stockCache;
        }

        public async Task AddCart()
        {
            var cart = new Cart();
            await _cartRepository.Add(cart);
        }

        public async Task<(Cart, string)> AddItemToCart(AddItemToCartRequest model)
        {
            var cart = await _cartRepository.Find(model.CartId);

            if (cart == null)
            {
                return (null, "Cart is not found");
            }

            var itemInDb = await _itemRepository.Find(model.ItemId);

            if (itemInDb == null)
            {
                return (null, "Item is not found");
            }

            var itemInCart = cart.Items.Where(i => i.Id.Equals(model.ItemId)).FirstOrDefault();

            var currentStock = _stockCache.GetStock(model.ItemId);
            if (currentStock == 0 || currentStock < model.Amount)
            {
                return (null, "Insufficent stock");
            }

            if (itemInCart != null)
            {
                itemInCart.Quantity += model.Amount;
            }
            else
            {
                itemInCart = new Item()
                {
                    Id = itemInDb.Id,
                    Name = itemInDb.Name,
                    Description = itemInDb.Description,
                    Price = itemInDb.Price,
                    Quantity = model.Amount
                };

                cart.Items.Add(itemInCart);
            }

            _stockCache.SetStock(model.ItemId, currentStock - model.Amount);

            var cartSaveResult = await _cartRepository.Update(cart);

            if (cartSaveResult)
            {
                var updatedCart = await _cartRepository.Find(model.CartId);

                return (updatedCart, null);
            }

            return (null, "Cart save is failed");
        }

        public async Task<InitializeDefaultCartsResponse> InitializeCarts()
        {
            var itemIds = new List<string>()
            {
                "ded4fe7c45f3184aa4f416cf",
                "499e102b5d40f98e8c960292",
                "c354ff10fca1a56b44668a3f",
                "48e2670c8baa72cb069ba400",
                "c1305b19d4a83ea7e47be035",
                "6009aeb8ab89d2d38680589c"
            };

            var item1 = new Item()
            {
                Id = itemIds[0],
                Name = "Rose",
                Description = "Red",
                Price = 10,
                Quantity = 0
            };

            var item2 = new Item()
            {
                Id = itemIds[1],
                Name = "Lavender",
                Description = "Lavender",
                Price = 4,
                Quantity = 0
            };

            var item3 = new Item()
            {
                Id = itemIds[2],
                Name = "Lily",
                Description = "Pink",
                Price = 15,
                Quantity = 0
            };

            var item4 = new Item()
            {
                Id = itemIds[3],
                Name = "Orchid",
                Description = "White",
                Price = 30,
                Quantity = 0
            };

            var item5 = new Item()
            {
                Id = itemIds[4],
                Name = "Daisy",
                Description = "White",
                Price = 2,
                Quantity = 0
            };

            var item6 = new Item()
            {
                Id = itemIds[5],
                Name = "Poppy",
                Description = "Red",
                Price = 4,
                Quantity = 0
            };

            await _itemRepository.Add(item1);
            await _itemRepository.Add(item2);
            await _itemRepository.Add(item3);
            await _itemRepository.Add(item4);
            await _itemRepository.Add(item5);
            await _itemRepository.Add(item6);

            var cart1 = new Cart()
            {
                Id = "f7b193ab388576a75ece4a6c",
                Items = new List<Item>()
                {
                    item1.GenerateItem(3),
                    item2.GenerateItem(4)
                }
            };

            var cart2 = new Cart()
            {
                Id = "0a53087ba7720a3c57691c26",
                Items = new List<Item>()
                {
                    item3.GenerateItem(1),
                    item4.GenerateItem(2)
                }
            };

            var cart3 = new Cart()
            {
                Id = "56ab4789c32ff7a99a4ed345",
                Items = new List<Item>()
                {
                }
            };

            await _cartRepository.Add(cart1);
            await _cartRepository.Add(cart2);
            await _cartRepository.Add(cart3);

            _stockCache.SetStock(item1.Id, 3);
            _stockCache.SetStock(item2.Id, 9);
            _stockCache.SetStock(item3.Id, 10);
            _stockCache.SetStock(item4.Id, 8);
            _stockCache.SetStock(item5.Id, 12);
            _stockCache.SetStock(item6.Id, 3);

            var carts = await _cartRepository.GetAll();
            var items = await _itemRepository.GetAll();

            var stockStatuses = new List<StockInfo>();

            foreach (var itemId in itemIds)
            {
                stockStatuses.Add(new StockInfo()
                {
                    ItemId = itemId,
                    Quantity = _stockCache.GetStock(itemId)
                });
            }

            var response = new InitializeDefaultCartsResponse()
            {
                Carts = carts.ToList(),
                Items = items.ToList(),
                Stock = stockStatuses
            };

            return response;
        }
    }
}
