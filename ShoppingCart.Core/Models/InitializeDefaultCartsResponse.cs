using ShoppingCart.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Models
{
    public class InitializeDefaultCartsResponse
    {
        public List<Cart> Carts { get; set; }

        public List<Item> Items { get; set; }

        public List<StockInfo> Stock { get; set; }
    }
}
