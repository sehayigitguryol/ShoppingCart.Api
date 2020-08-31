using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using ShoppingCart.Infrastructure.Configurations;
using ShoppingCart.Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ShoppingCart.Api.Tests
{
    public class MongoDbContextTests
    {
        private IShoppingCartContext _context;
        private void SetupEnvironment()
        {
            var configs = new MongoDbConfigurations()
            {
                Host = "test",
                Port = 27017,
                Database = "TestDB"
            };
            _context = new ShoppingCartContext(configs);
        }

        [Fact]
        public void SSS()
        {
            // Arrange

            var configs = new MongoDbConfigurations()
            {
                Host = "test",
                Port = 27017,
                Database = "TestDB"
            };
            // Act

            var context = new ShoppingCartContext(configs);

            Assert.NotNull(context);
        }

    }

}
