using ShoppingCart.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Repositories
{
    /// <summary>
    /// Interface for item repository
    /// </summary>
    public interface IItemRepository : ICrudRepository<Item>
    {
    }
}
