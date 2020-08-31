using ShoppingCart.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Repositories
{
    public interface IItemRepository : ICrudRepository<Item>
    {
    }
}
