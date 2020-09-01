using ShoppingCart.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Repositories
{
    /// <summary>
    /// Interface for Cart Repository
    /// </summary>
    public interface ICartRepository : ICrudRepository<Cart>
    {
    }
}
