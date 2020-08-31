using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Entities
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
    }
}
