using ShoppingCart.Core.Entities;
using ShoppingCart.Core.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Infrastructure.Cache
{
    public class StockCacheInMemory : IStockCache
    {
        private readonly ConcurrentDictionary<string, int> cache = new ConcurrentDictionary<string, int>();

        public int GetStock(string key)
        {
            return cache[key];
        }

        public void SetStock(string key, int value)
        {
            cache.AddOrUpdate(key, value, (key, oldValue) => value);
        }
    }
}
