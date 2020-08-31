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
    public class ItemRepository : CrudRepository<Item>, IItemRepository
    {
        public ItemRepository(IShoppingCartContext context) 
            : base(context)
        {

        }
    }
}
