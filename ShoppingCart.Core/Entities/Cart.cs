using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Entities
{
    public class Cart : BaseEntity
    {
        public List<Item> Items { get; set; }
    }
}
