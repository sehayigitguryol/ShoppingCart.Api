using MongoDB.Bson;
using MongoDB.Driver;
using ShoppingCart.Core.Entities;
using ShoppingCart.Core.Repositories;
using ShoppingCart.Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Infrastructure.Data.Repositories
{
    public class ItemRepository : ICrudRepository<Item, string>, IItemRepository
    {
        private readonly IShoppingCartContext _context;

        public ItemRepository(IShoppingCartContext context)
        {
            _context = context;
        }

        public void Add(Item entity)
        {
            _context.Items.InsertOne(entity);
        }

        public void Delete(string id)
        {
            FilterDefinition<Item> filter = Builders<Item>.Filter.Eq(m => m.Id, id);
            _context.Items.FindOneAndDelete(filter);
        }

        public Item Find(string id)
        {
            FilterDefinition<Item> filter = Builders<Item>.Filter.Eq(m => m.Id, id);

            return _context.Items.Find(filter).FirstOrDefault();
        }

        public IEnumerable<Item> GetAll()
        {
            return _context.Items.Find(_ => true).ToList();
        }

        public void Update(Item entity)
        {
            var filter = Builders<Item>.Filter.Eq(m => m.Id, entity.Id);
            _context.Items.FindOneAndReplace(filter, entity);
        }
    }
}
