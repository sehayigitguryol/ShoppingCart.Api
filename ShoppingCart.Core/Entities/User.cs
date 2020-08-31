using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public Cart Cart { get; set; }
    }
}
