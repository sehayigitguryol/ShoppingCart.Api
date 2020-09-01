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
        Task<(Cart, string)> AddItemToCart(AddItemToCartModel model);
    }

    public class CartService : ICartService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICartRepository _cartRepository;

        public CartService(IItemRepository itemRepository, ICartRepository cartRepository)
        {
            _itemRepository = itemRepository;
            _cartRepository = cartRepository;
        }

        public async Task<(Cart, string)> AddItemToCart(AddItemToCartModel model)
        {
            var cart = await _cartRepository.Find(model.CartId);

            if (cart == null)
            {
                return (null, "Cart is not found");
            }

            var itemInCart = cart.Items.Where(i => i.Id.Equals(model.ItemId)).FirstOrDefault();
            var itemInDb = await _itemRepository.Find(model.ItemId);

            if (itemInDb == null)
            {
                return (null, "Item is not found");
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

            itemInDb.Quantity -= model.Amount;
            var itemSaveResult = await _itemRepository.Update(itemInDb);

            if (!itemSaveResult)
            {
                return (null, "Item save is failed");
            }

            var cartSaveResult = await _cartRepository.Update(cart);

            if (cartSaveResult)
            {
                var updatedCart = await _cartRepository.Find(model.CartId);

                return (updatedCart, null);
            }

            return (null, "Cart save is failed");
        }
    }
}
