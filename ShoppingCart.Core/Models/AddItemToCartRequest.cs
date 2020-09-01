using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Core.Models
{
    public class AddItemToCartRequest
    {
        public string CartId { get; set; }

        public string ItemId { get; set; }
                
        public int Amount { get; set; }
    }
}
