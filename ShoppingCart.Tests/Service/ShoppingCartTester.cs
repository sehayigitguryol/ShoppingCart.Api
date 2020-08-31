using ShoppingCart.Core.Repositories;
using ShoppingCart.Core.Services;
using ShoppingCart.Infrastructure.Configurations;
using ShoppingCart.Infrastructure.Data.Contexts;
using ShoppingCart.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Tests.Service
{
    public class ShoppingCartTester : IDisposable
    {
        public IShoppingCartContext Context { get; }

        public IItemRepository ItemRepository { get; }

        public IItemService ItemService { get; }

        public ShoppingCartTester()
        {
            var dbGuid = new Guid();

            var configs = new MongoDbConfigurations()
            {
                Database = dbGuid.ToString(),
                Host = "localhost",
                Port = 27017,
                User = "root",
                Password = "password"
            };

            Context = new ShoppingCartContext(configs);

            ItemRepository = new ItemRepository(Context);

            ItemService = new ItemService(ItemRepository);
        }

        public void Dispose()
        {
            Context.DropDatabase();
        }
    }
}
