using ShoppingCart.Core.Entities;
using ShoppingCart.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Core.Services
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();

        Task<Item> GetItemByIdAsync(string id);

        Task<Item> CreateItemAsync(Item item);

        Task<bool> UpdateItemAsync(Item item);

        Task<bool> DeleteItemAsync(string id);
    }

    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<Item> CreateItemAsync(Item item)
        {
            await _itemRepository.Add(item);
            return item;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            return await _itemRepository.Delete(id);
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _itemRepository.GetAll();
        }

        public async Task<Item> GetItemByIdAsync(string id)
        {
            return await _itemRepository.Find(id);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            return await _itemRepository.Update(item);
        }
    }
}
