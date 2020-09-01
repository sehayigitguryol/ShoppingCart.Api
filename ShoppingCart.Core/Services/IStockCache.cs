using ShoppingCart.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Services
{
    /// <summary>
    /// General key value cache interface
    /// </summary>
    public interface IStockCache
    {
        int GetStock(string key);

        void SetStock(string key, int value);
    }
}
