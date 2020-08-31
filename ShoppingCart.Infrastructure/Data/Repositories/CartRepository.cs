using ShoppingCart.Core.Entities;
using ShoppingCart.Core.Repositories;
using ShoppingCart.Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Infrastructure.Data.Repositories
{
    public class CartRepository : CrudRepository<Cart>, ICartRepository
    {
        public CartRepository(IShoppingCartContext context)
            : base(context)
        {

        }
    }
}
